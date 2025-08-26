using System;
using System.Collections.Generic;
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
        private TaskCompletionSource<bool> mOpenTask = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        private TaskCompletionSource<GoodbyeMessage> mCloseTask = new TaskCompletionSource<GoodbyeMessage>();
        private GoodbyeMessage mGoodbyeMessage;
        private readonly IWampClientAuthenticator mAuthenticator;
        private HelloDetails mSentDetails;

        private SessionState mSessionState = SessionState.Closed;
        private WampSessionCloseEventArgs mCloseEventArgs;
        private readonly object mLock = new object();

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
            mServerProxy = realm.Proxy;
            mAuthenticator = authenticator ?? new DefaultWampClientAuthenticator();
        }

        public long Session { get; private set; }
        public event EventHandler<WampSessionCreatedEventArgs> ConnectionEstablished;
        public event EventHandler<WampSessionCloseEventArgs> ConnectionBroken;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
        public bool IsConnected => mSessionState == SessionState.Established;
        public IWampRealmProxy Realm { get; }
        public Task OpenTask => mOpenTask.Task;

        public void OnConnectionOpen()
        {
            var (_, action) = ChangeState(StateChangeReason.ConnectionOpen);

            if (action == StateChangeAction.SendHello)
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

                mServerProxy.Hello(Realm.Name,
                    helloDetails);

                mSentDetails = helloDetails;
            }
        }

        public void Challenge(string authMethod, ChallengeDetails extra)
        {
            var (_, action) = ChangeState(StateChangeReason.ReceivedChallenge);
            switch (action)
            {
                case StateChangeAction.SendAuthenticate:
                    AuthenticateWithServer(authMethod, extra);
                    break;
                case StateChangeAction.SendProtocolViolation:
                    SendProtocolViolation("The server issued a CHALLENGE from an invalid state");
                    break;
            }
        }

        private void AuthenticateWithServer(string authMethod, ChallengeDetails extra)
        {
            try
            {
                AuthenticationResponse response = mAuthenticator.Authenticate(authMethod, extra);

                AuthenticateExtraData authenticationExtraData = response.Extra ?? EmptyAuthenticateDetails;

                string authenticationSignature = response.Signature;

                ChangeState(StateChangeReason.SentAuthenticate); // put this right before proxy call, as in mServerProxy.Authenticate it can send WELCOME in synchronous manner 
                mServerProxy.Authenticate(authenticationSignature, authenticationExtraData);
            }
            catch (WampAuthenticationException ex)
            {
                var (_, action) = ChangeState(StateChangeReason.AuthenticationFailed);
                if (action == StateChangeAction.SendAbort)
                {
                    mServerProxy.Abort(ex.Details, ex.Reason);
                    OnConnectionError(ex);
                }
            }
        }

        public void Welcome(long session, WelcomeDetails details)
        {
            var (_, action) = ChangeState(StateChangeReason.ReceivedWelcome);
            switch (action)
            {
                case StateChangeAction.RaiseConnectionEstablished:
                    Session = session;

                    OnConnectionEstablished(new WampSessionCreatedEventArgs
                        (session, mSentDetails, details, new SessionTerminator(this)));
                    mOpenTask.TrySetResult(true);
                    break;
                case StateChangeAction.SendProtocolViolation:
                    SendProtocolViolation("Received a WELCOME message from an invalid state");
                    break;
            }
        }

        public void Abort(AbortDetails details, string reason)
        {
            using (IDisposable proxy = mServerProxy as IDisposable)
            {
                var (_, action) = ChangeState(StateChangeReason.ReceivedAbort);
                if (action == StateChangeAction.Close)
                    TrySetCloseEventArgs(SessionCloseType.Abort, details, reason);
            }
        }

        public void Goodbye(GoodbyeDetails details, string reason)
        {
            var (_, action) = ChangeState(StateChangeReason.ReceivedGoodbye);
            using (IDisposable proxy = mServerProxy as IDisposable)
            {
                TrySetCloseEventArgs(SessionCloseType.Goodbye, details, reason);

                switch (action)
                {
                    case StateChangeAction.AcceptGoodbye:
                        Interlocked.CompareExchange(ref mGoodbyeMessage, new GoodbyeMessage { Details = details, Reason = reason }, null);
                        break;
                    case StateChangeAction.SendGoodbye:
                        Interlocked.CompareExchange(ref mGoodbyeMessage, new GoodbyeMessage { Details = details, Reason = reason }, null);
                        mServerProxy.Goodbye(new GoodbyeDetails(), WampErrors.GoodbyeAndOut);
                        break;
                    case StateChangeAction.SendProtocolViolation:
                        SendProtocolViolation("Received GOODBYE message before session was established");
                        break;
                }

            }
        }

        public Task<GoodbyeMessage> Close(string reason, GoodbyeDetails details)
        {
            var (stateData, action) = ChangeState(StateChangeReason.CloseInitiated);
            switch (action)
            {
                case StateChangeAction.SendGoodbye:
                    mServerProxy.Goodbye(details ?? EmptyGoodbyeDetails, reason ?? WampErrors.CloseNormal);
                    break;
                case StateChangeAction.SendAbort:
                    mServerProxy.Abort(new AbortDetails { Message = "The client had to close before a session could be established" }, WampErrors.SystemShutdown);
                    break;
                case StateChangeAction.RaiseConnectionNotEstablished:
                    return Task.FromException<GoodbyeMessage>(new WampSessionNotEstablishedException("Cannot close an unopened session"));
            }

            return stateData.CloseTask.Task;
        }

        public void OnConnectionClosed()
        {
            RaiseConnectionBroken();
        }

        private void RaiseConnectionBroken()
        {
            var (stateData, action) = ChangeState(StateChangeReason.ConnectionBroken);
            if (action == StateChangeAction.RaiseConnectionBroken)
            {
                TrySetCloseEventArgs(SessionCloseType.Disconnection);

                // reset first so that when setting result or exception it won't still hold on old tasks if other thread try to Open immediately after Close task finishes.
                Interlocked.Exchange(ref mSentDetails, null);
                var closeEventArgs = Interlocked.Exchange(ref mCloseEventArgs, null);
                var oldOpenTask = Interlocked.Exchange(ref mOpenTask, new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously));

                if (Interlocked.Exchange(ref mGoodbyeMessage, null) is GoodbyeMessage goodbyeMessage)
                {
                    stateData.CloseTask?.TrySetResult(goodbyeMessage);
                }

                var exception = new WampConnectionBrokenException(closeEventArgs);
                oldOpenTask?.TrySetException(exception);
                stateData.CloseTask?.TrySetException(exception);

                OnConnectionBroken(closeEventArgs);
            }
        }

        private void TrySetCloseEventArgs(SessionCloseType sessionCloseType,
                                          GoodbyeAbortDetails details = null,
                                          string reason = null)
        {
            Interlocked.CompareExchange(
                ref mCloseEventArgs,
                new WampSessionCloseEventArgs(sessionCloseType, Session, details, reason),
                null);
        }

        public void OnConnectionError(Exception exception)
        {
            var (stateData, action) = ChangeState(StateChangeReason.ConnectionError);
            if (action == StateChangeAction.RaiseConnectionError)
            {
                mOpenTask?.TrySetException(exception);
                stateData.CloseTask.TrySetException(exception);
                OnConnectionError(new WampConnectionErrorEventArgs(exception));
            }
        }

        private void SendProtocolViolation(string message)
        {
            using (mServerProxy as IDisposable)
            {
                TrySetCloseEventArgs(SessionCloseType.Abort, new AbortDetails { Message = $"Aborted session with server: '{message}'" }, WampErrors.ProtocolViolation);
                mServerProxy.Abort(new AbortDetails { Message = message }, WampErrors.ProtocolViolation);
            }
        }

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

        private (SessionStateData, StateChangeAction) ChangeState(StateChangeReason reason)
        {
            lock (mLock)
            {
                var (newSessionState, stateChangeAction) = Transition(mSessionState, reason);

                mSessionState = newSessionState;
                
                if (reason == StateChangeReason.ConnectionOpen && stateChangeAction != StateChangeAction.Ignore)
                {
                    mCloseTask = new TaskCompletionSource<GoodbyeMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
                }

                return (new SessionStateData(mSessionState, mCloseTask), stateChangeAction);
            }
        }

        /* https://wamp-proto.org/wamp_latest_ietf.html#section-4-3
                                 +--------------+
        +--------(6)------------->              |
        |                        | CLOSED       <--------------------------+
        | +------(4)------------->              <---+                      |
        | |                      +--------------+   |                      |
        | |                               |         |                      |
        | |                              (1)       (7)                     |
        | |                               |         |                      |
        | |                      +--------v-----+   |                   (11)
        | |                      |              +---+                      |
        | |         +------------+ ESTABLISHING +----------------+         |
        | |         |            |              |                |         |
        | |         |            +--------------+                |         |
        | |         |                     |                     (10)       |
        | |         |                    (9)                     |         |
        | |         |                     |                      |         |
        | |        (2)           +--------v-----+       +--------v-------+ |
        | |         |            |              |       |                | |
        | |         |     +------> FAILED       <--(13)-+ CHALLENGING /  +-+
        | |         |     |      |              |       | AUTHENTICATING |
        | |         |     |      +--------------+       +----------------+
        | |         |    (8)                                     |
        | |         |     |                                      |
        | |         |     |                                      |
        | | +-------v-------+                                    |
        | | |               <-------------------(12)-------------+
        | | | ESTABLISHED   |
        | | |               +--------------+
        | | +---------------+              |
        | |         |                      |
        | |        (3)                    (5)
        | |         |                      |
        | | +-------v-------+     +--------v-----+
        | | |               +--+  |              |
        | +-+ SHUTTING DOWN |  |  | CLOSING      |
        |   |               |(14) |              |
        |   +-------^-------+  |  +--------------+
        |           |----------+           |
        +----------------------------------+
        */
        private static (SessionState, StateChangeAction) Transition(SessionState previousState, StateChangeReason reason)
        {
            return previousState switch
            {
                _ when reason is StateChangeReason.ConnectionBroken => (SessionState.Closed, StateChangeAction.RaiseConnectionBroken),
                _ when reason is StateChangeReason.ConnectionError => (SessionState.Closed, StateChangeAction.RaiseConnectionError),
                SessionState.Closed => reason switch
                {
                    StateChangeReason.ConnectionOpen => (SessionState.Establishing, StateChangeAction.SendHello), // 1
                    StateChangeReason.CloseInitiated => (SessionState.Closed, StateChangeAction.RaiseConnectionNotEstablished),
                    _ => (SessionState.Closed, StateChangeAction.Ignore)
                },
                SessionState.Establishing => reason switch
                {
                    StateChangeReason.CloseInitiated => (SessionState.Closed, StateChangeAction.SendAbort), // 7
                    StateChangeReason.ReceivedWelcome => (SessionState.Established, StateChangeAction.RaiseConnectionEstablished), // 2
                    StateChangeReason.ReceivedChallenge => (SessionState.SendingAuthenticate, StateChangeAction.SendAuthenticate), // 10
                    StateChangeReason.ReceivedAbort => (SessionState.Closed, StateChangeAction.Close), // 7                    
                    _ => (SessionState.Failed, StateChangeAction.SendProtocolViolation) // 9
                },
                SessionState.SendingAuthenticate => reason switch
                {
                    StateChangeReason.CloseInitiated => (SessionState.Closed, StateChangeAction.SendAbort),
                    StateChangeReason.SentAuthenticate => (SessionState.Authenticating, StateChangeAction.Ignore),
                    StateChangeReason.AuthenticationFailed => (SessionState.Closed, StateChangeAction.SendAbort), // 11
                    StateChangeReason.ReceivedAbort => (SessionState.Closed, StateChangeAction.Close),
                    _ => (SessionState.Failed, StateChangeAction.SendProtocolViolation)
                },
                SessionState.Authenticating => reason switch
                {
                    StateChangeReason.CloseInitiated => (SessionState.Closed, StateChangeAction.SendAbort),
                    StateChangeReason.ReceivedWelcome => (SessionState.Established, StateChangeAction.RaiseConnectionEstablished), // 12
                    StateChangeReason.AuthenticationFailed => (SessionState.Closed, StateChangeAction.SendAbort), // 11
                    StateChangeReason.ReceivedAbort => (SessionState.Closed, StateChangeAction.Close), // 11
                    _ => (SessionState.Failed, StateChangeAction.SendProtocolViolation) // 13
                },
                SessionState.Established => reason switch
                {
                    StateChangeReason.CloseInitiated => (SessionState.ShuttingDown, StateChangeAction.SendGoodbye), // 3
                    StateChangeReason.ReceivedGoodbye => (SessionState.Closing, StateChangeAction.SendGoodbye), // 5
                    StateChangeReason.ReceivedAbort => (SessionState.Closed, StateChangeAction.Close),
                    _ => (SessionState.Failed, StateChangeAction.SendProtocolViolation) // 8
                },
                SessionState.ShuttingDown => reason switch
                {
                    StateChangeReason.ReceivedGoodbye => (SessionState.ShuttingDown, StateChangeAction.AcceptGoodbye), // 4
                    _ => (SessionState.ShuttingDown, StateChangeAction.Ignore), // 14
                },
                SessionState.Closing => reason switch
                {
                    StateChangeReason.ReceivedAbort => (SessionState.Closed, StateChangeAction.Close),
                    _ => (SessionState.Closing, StateChangeAction.Ignore)
                },
                SessionState.Failed => (SessionState.Failed, StateChangeAction.Ignore),
                _ => (previousState, StateChangeAction.Ignore)
            };
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

        private class SessionStateData
        {
            public readonly SessionState State;
            public readonly TaskCompletionSource<GoodbyeMessage> CloseTask;
            public SessionStateData(SessionState state, TaskCompletionSource<GoodbyeMessage> closeTask)
            {
                State = state;
                CloseTask = closeTask;
            }
        }

        private enum SessionState
        {
            Closed,
            Establishing,
            SendingAuthenticate,
            Authenticating,
            Established,
            ShuttingDown,
            Closing,
            Failed,
        }

        private enum StateChangeReason
        {
            ConnectionOpen,
            SentAuthenticate,
            ReceivedWelcome,
            CloseInitiated,
            ReceivedGoodbye,
            ConnectionBroken,
            ReceivedChallenge,
            AuthenticationFailed,
            ReceivedAbort,
            ConnectionError
        }

        private enum StateChangeAction
        {
            SendHello,
            RaiseConnectionEstablished,
            SendAuthenticate,
            SendGoodbye,
            AcceptGoodbye,
            RaiseConnectionBroken,
            SendProtocolViolation,
            SendAbort,
            Ignore,
            Close,
            RaiseConnectionNotEstablished,
            RaiseConnectionError
        }
    }
}