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
        private static GoodbyeDetails EmptyGoodbyeDetails = new GoodbyeDetails();
        private static AuthenticateExtraData EmptyAuthenticateDetails = new AuthenticateExtraData();

        private bool mConnectionBrokenRaised = false;
        private readonly IWampRealmProxy mRealm;
        private readonly IWampServerProxy mServerProxy;
        private long mSession;
        private TaskCompletionSource<bool> mOpenTask = new TaskCompletionSource<bool>();
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly object mLock = new object();
        private bool mGoodbyeSent;
		private readonly IWampClientAuthenticator mAuthenticator;
        private HelloDetails mSentDetails;

        private int mIsConnected = 0;

        private static HelloDetails GetDetails()
        {
            return new HelloDetails()
            {
                Roles = new ClientRoles()
                {
                    Caller = new CallerFeatures()
                    {
                        CallerIdentification = true,
                        ProgressiveCallResults = true
                    },
                    Callee = new CalleeFeatures()
                    {
                        ProgressiveCallResults = true
                    },
                    Publisher = new PublisherFeatures()
                    {
                        SubscriberBlackwhiteListing = true,
                        PublisherExclusion = true,
                        PublisherIdentification = true,
                    },
                    Subscriber = new SubscriberFeatures()
                    {
                        PublisherIdentification = true
                    }
                }
            };
        }

        public WampSessionClient(IWampRealmProxy realm, IWampFormatter<TMessage> formatter, IWampClientAuthenticator authenticator)
        {
            mRealm = realm;
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
            mSession = session;

            Interlocked.CompareExchange(ref mIsConnected, 1, 0);

            mOpenTask.TrySetResult(true);

            OnConnectionEstablished(new WampSessionCreatedEventArgs
                (session, mSentDetails, details));
        }

        public void Abort(AbortDetails details, string reason)
        {
            RaiseConnectionBroken(SessionCloseType.Abort, details, reason);
        }

        public void Goodbye(GoodbyeDetails details, string reason)
        {
            if (!mGoodbyeSent)
            {
                mServerProxy.Goodbye(new GoodbyeDetails(), WampErrors.GoodbyeAndOut);
            }

            RaiseConnectionBroken(SessionCloseType.Goodbye, details, reason);
        }

        private void RaiseConnectionBroken(SessionCloseType sessionCloseType, GoodbyeAbortDetails details, string reason)
        {
            bool connectionBrokenRaised = mConnectionBrokenRaised;

            mConnectionBrokenRaised = true;

            WampSessionCloseEventArgs closeEventArgs = new WampSessionCloseEventArgs
                (sessionCloseType, mSession,
                    details,
                    reason);

            SetOpenTaskErrorIfNeeded(new WampConnectionBrokenException(closeEventArgs));

            if (sessionCloseType == SessionCloseType.Disconnection)
            {
                Interlocked.CompareExchange(ref mIsConnected, 0, 1);

                mOpenTask = new TaskCompletionSource<bool>();

                mConnectionBrokenRaised = false;
            }

            if (!connectionBrokenRaised)
            {
                OnConnectionBroken(closeEventArgs);
            }
        }

        public long Session
        {
            get
            {
                return mSession;
            }
        }

        public IWampRealmProxy Realm
        {
            get
            {
                return mRealm;
            }
        }

        public Task OpenTask
        {
            get
            {
                return mOpenTask.Task;
            }
        }

        public void Close(string reason, GoodbyeDetails details)
        {
            reason = reason ?? WampErrors.CloseNormal;
            details = details ?? EmptyGoodbyeDetails;

            mGoodbyeSent = true;
            mServerProxy.Goodbye(details, reason);
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
            SetOpenTaskErrorIfNeeded(new Exception("Connection closed before connection established."));

            RaiseConnectionBroken(SessionCloseType.Disconnection,
                                  null,
                                  null);
        }

        public void OnConnectionError(Exception exception)
        {
            SetOpenTaskErrorIfNeeded(exception);

            OnConnectionError(new WampConnectionErrorEventArgs(exception));

            mConnectionBrokenRaised = false;
        }

        private void SetOpenTaskErrorIfNeeded(Exception exception)
        {
            TaskCompletionSource<bool> openTask = mOpenTask;

            if (openTask != null)
            {
                openTask.TrySetException(exception);
            }
        }

        public event EventHandler<WampSessionCreatedEventArgs> ConnectionEstablished;

        public event EventHandler<WampSessionCloseEventArgs> ConnectionBroken;

        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

        public bool IsConnected
        {
            get
            {
                return mIsConnected == 1;
            }
        }

        protected virtual void OnConnectionEstablished(WampSessionCreatedEventArgs e)
        {
            EventHandler<WampSessionCreatedEventArgs> handler = ConnectionEstablished;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnConnectionBroken(WampSessionCloseEventArgs e)
        {
            EventHandler<WampSessionCloseEventArgs> handler = ConnectionBroken;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnConnectionError(WampConnectionErrorEventArgs e)
        {
            EventHandler<WampConnectionErrorEventArgs> handler = ConnectionError;
            if (handler != null) handler(this, e);
        }
    }
}