using System;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Authentication;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Client
{
    public class WampSessionClient<TMessage> : IWampSessionClientExtended,
        IWampClientConnectionMonitor
    {
        private static readonly GoodbyeDetails EmptyGoodbyeDetails = new GoodbyeDetails();
        private static readonly AuthenticateExtraData EmptyAuthenticateDetails = new AuthenticateExtraData();
        private readonly IWampServerProxy mServerProxy;
        private TaskCompletionSource<bool> mOpenTask = new TaskCompletionSource<bool>();
        private TaskCompletionSource<GoodbyeMessage> mCloseTask;
        private GoodbyeMessage mGoodbyeMessage;
        private readonly IWampFormatter<TMessage> mFormatter;
        private bool mGoodbyeSent;
		private readonly IWampClientAuthenticator mAuthenticator;
        private HelloDetails mSentDetails;

        private int mIsConnected = 0;
        private WampSessionCloseEventArgs mCloseEventArgs;

        private static HelloDetails GetDetails()
        {
            return new HelloDetails()
            {
                Roles = new ClientRoles()
                {
                    Caller = new CallerFeatures()
                    {
                        CallerIdentification = true,
                        ProgressiveCallResults = true,
                        CallCanceling = true
                    },
                    Callee = new CalleeFeatures()
                    {
                        ProgressiveCallResults = true,
                        CallerIdentification = true,
                        PatternBasedRegistration = true,
                        SharedRegistration = true,
                        CallCanceling = true
                    },
                    Publisher = new PublisherFeatures()
                    {
                        SubscriberBlackwhiteListing = true,
                        PublisherExclusion = true,
                        PublisherIdentification = true
                    },
                    Subscriber = new SubscriberFeatures()
                    {
                        PublisherIdentification = true,
                        PatternBasedSubscription = true
                    }
                }
            };
        }

        public WampSessionClient(IWampRealmProxy realm, IWampFormatter<TMessage> formatter, IWampClientAuthenticator authenticator)
        {
            Realm = realm;
            mFormatter = formatter;
            mServerProxy = realm.Proxy;
            mAuthenticator = authenticator ?? new DefaultWampClientAuthenticator();
        }

        public void Challenge(string authMethod, ChallengeDetails extra)
        {
            try
            {
                AuthenticationResponse response = mAuthenticator.Authenticate(authMethod, extra);

                AuthenticateExtraData authenticationExtraData = response.Extra ?? EmptyAuthenticateDetails;

                string authenticationSignature = response.Signature;

                mServerProxy.Authenticate(authenticationSignature, authenticationExtraData);
            }
            catch (WampAuthenticationException ex)
            {
                mServerProxy.Abort(ex.Details, ex.Reason);
                OnConnectionError(ex);
            }
        }

        public void Welcome(long session, WelcomeDetails details)
        {
            Session = session;

            Interlocked.CompareExchange(ref mIsConnected, 1, 0);

            mOpenTask.TrySetResult(true);

            OnConnectionEstablished(new WampSessionCreatedEventArgs
                (session, mSentDetails, details, new SessionTerminator(this)));
        }

        public void Abort(AbortDetails details, string reason)
        {
            using (IDisposable proxy = mServerProxy as IDisposable)
            {
                TrySetCloseEventArgs(SessionCloseType.Abort, details, reason);
            }
        }

        public void Goodbye(GoodbyeDetails details, string reason)
        {
            using (IDisposable proxy = mServerProxy as IDisposable)
            {
                TrySetCloseEventArgs(SessionCloseType.Goodbye, details, reason);

                if (!mGoodbyeSent)
                {
                    mGoodbyeSent = true;
                    mServerProxy.Goodbye(new GoodbyeDetails(), WampErrors.GoodbyeAndOut);
                }
                else
                {
                    mGoodbyeMessage = new GoodbyeMessage(){Details = details, Reason = reason};
                }
            }
        }

        private void RaiseConnectionBroken()
        {
            TrySetCloseEventArgs(SessionCloseType.Disconnection);

            WampSessionCloseEventArgs closeEventArgs = mCloseEventArgs;

            Interlocked.CompareExchange(ref mIsConnected, 0, 1);

            GoodbyeMessage goodbyeMessage = mGoodbyeMessage;

            if (goodbyeMessage != null)
            {
                mCloseTask?.TrySetResult(goodbyeMessage);
            }

            SetTasksErrorsIfNeeded(new WampConnectionBrokenException(mCloseEventArgs));

            mOpenTask = new TaskCompletionSource<bool>();
            mCloseTask = null;
            mCloseEventArgs = null;
            mGoodbyeMessage = null;

            OnConnectionBroken(closeEventArgs);
        }

        private void TrySetCloseEventArgs(SessionCloseType sessionCloseType,
                                          GoodbyeAbortDetails details = null,
                                          string reason = null)
        {
            if (mCloseEventArgs == null)
            {
                mCloseEventArgs = new WampSessionCloseEventArgs
                (sessionCloseType, Session,
                 details,
                 reason);
            }
        }

        public long Session { get; private set; }

        public IWampRealmProxy Realm { get; }

        public Task OpenTask => mOpenTask.Task;

        public Task<GoodbyeMessage> Close(string reason, GoodbyeDetails details)
        {
            reason = reason ?? WampErrors.CloseNormal;
            details = details ?? EmptyGoodbyeDetails;

            TaskCompletionSource<GoodbyeMessage> closeTask = 
                new TaskCompletionSource<GoodbyeMessage>();

            mCloseTask = closeTask;

            mGoodbyeSent = true;
            mServerProxy.Goodbye(details, reason);

            return closeTask.Task;
        }

        public void OnConnectionOpen()
        {
            HelloDetails helloDetails = GetDetails();

            if (mAuthenticator.AuthenticationId != null)
            {
                helloDetails.AuthenticationId = mAuthenticator.AuthenticationId;
            }

            if (mAuthenticator.AuthenticationMethods != null)
            {
                helloDetails.AuthenticationMethods = mAuthenticator.AuthenticationMethods;
            }

            mServerProxy.Hello
                (Realm.Name,
                 helloDetails);

            mSentDetails = helloDetails;
        }

        public void OnConnectionClosed()
        {
            RaiseConnectionBroken();
        }

        public void OnConnectionError(Exception exception)
        {
            SetTasksErrorsIfNeeded(exception);

            OnConnectionError(new WampConnectionErrorEventArgs(exception));
        }

        private void SetTasksErrorsIfNeeded(Exception exception)
        {
            mOpenTask?.TrySetException(exception);
            mCloseTask?.TrySetException(exception);
        }

        public event EventHandler<WampSessionCreatedEventArgs> ConnectionEstablished;

        public event EventHandler<WampSessionCloseEventArgs> ConnectionBroken;

        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

        public bool IsConnected => mIsConnected == 1;

        protected virtual void OnConnectionEstablished(WampSessionCreatedEventArgs e)
        {
            ConnectionEstablished?.Invoke(this, e);
        }

        protected virtual void OnConnectionBroken(WampSessionCloseEventArgs e)
        {
            ConnectionBroken?.Invoke(this, e);
        }

        protected virtual void OnConnectionError(WampConnectionErrorEventArgs e)
        {
            ConnectionError?.Invoke(this, e);
        }

        private class SessionTerminator : IWampSessionTerminator
        {
            private readonly WampSessionClient<TMessage> mParent;

            public SessionTerminator(WampSessionClient<TMessage> parent)
            {
                mParent = parent;
            }

            public void Disconnect(GoodbyeDetails details, string reason)
            {
                mParent.Close(reason, details);
            }
        }
    }
}
