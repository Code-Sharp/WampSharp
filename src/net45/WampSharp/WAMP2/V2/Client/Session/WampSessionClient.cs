using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Client
{
    public class WampSessionClient<TMessage> : IWampSessionClientExtended<TMessage>,
        IWampClientConnectionMonitor
    {
        private static IDictionary<string, object> EmptyDetails = new Dictionary<string, object>();

        private bool mConnectionBrokenRaised = false;
        private readonly IWampRealmProxy mRealm;
        private readonly IWampServerProxy mServerProxy;
        private long mSession;
        private TaskCompletionSource<bool> mOpenTask = new TaskCompletionSource<bool>();
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly object mLock = new object();
        private bool mGoodbyeSent;
        private readonly IDictionary<string, object> mDetails = GetDetails();
		// TODO: Get this in constructor
		private IWampClientAutenticator mAuthenticator;

        private static Dictionary<string, object> GetDetails()
        {
            // TODO: Do it with reflection :)
            return new Dictionary<string, object>
                {
                    {
                        "roles", new Dictionary<string, object>
                            {
                                {
                                    "caller", new Dictionary<string, object>
                                        {
                                            {
                                                "features", new Dictionary<string, object>
                                                    {
                                                        {"caller_identification", true}
                                                    }
                                            }
                                        }
                                },
                                {
                                    "callee", new Dictionary<string, object>
                                        {
                                            {"features", new Dictionary<string, object>()}
                                        }
                                },
                                {
                                    "publisher", new Dictionary<string, object>
                                        {
                                            {
                                                "features", new Dictionary<string, object>
                                                    {
                                                        {"subscriber_blackwhite_listing", true},
                                                        {"publisher_exclusion", true},
                                                        {"publisher_identification", true},
                                                    }
                                            }
                                        }
                                },
                                {
                                    "subscriber", new Dictionary<string, object>
                                        {
                                            {
                                                "features", new Dictionary<string, object>
                                                    {
                                                        {"publisher_identification", true},
                                                    }
                                            }
                                        }
                                }
                            }
                    }
                };
        }

        public WampSessionClient(IWampRealmProxy realm, IWampFormatter<TMessage> formatter)
        {
            mRealm = realm;
            mFormatter = formatter;
            mServerProxy = realm.Proxy;
        }

        public void Challenge(string challenge, ChallengeDetails extra)
		{
			ChallengeResult result = mAuthenticator.Authenticate(challenge, extra);
			
			IDictionary<string, object> authenticationExtraData = result.Extra ?? EmptyDetails;
			
			string authenticationSignature = result.Signature;
			
			mServerProxy.Authenticate(authenticationSignature, authenticationExtraData);
        }

        public void Welcome(long session, TMessage details)
        {
            mSession = session;
            mOpenTask.SetResult(true);
            mOpenTask = null;

            OnConnectionEstablished(new WampSessionEventArgs
                (session, new SerializedValue<TMessage>(mFormatter, details)));
        }

        public void Abort(TMessage details, string reason)
        {
            RaiseConnectionBroken(mFormatter, SessionCloseType.Abort, details, reason);
        }

        public void Goodbye(TMessage details, string reason)
        {
            if (!mGoodbyeSent)
            {
                mServerProxy.Goodbye(new {}, WampErrors.GoodbyeAndOut);
            }

            RaiseConnectionBroken(mFormatter, SessionCloseType.Goodbye, details, reason);
        }

        private void RaiseConnectionBroken<T>(IWampFormatter<T> formatter, SessionCloseType sessionCloseType, T details, string reason)
        {
            mConnectionBrokenRaised = true;
            
            OnConnectionBroken
                (new WampSessionCloseEventArgs
                     (sessionCloseType, mSession,
                      new SerializedValue<T>(formatter, details),
                      reason));
        }

        public void Heartbeat(int incomingSeq, int outgoingSeq)
        {
            throw new System.NotImplementedException();
        }

        public void Heartbeat(int incomingSeq, int outgoingSeq, string discard)
        {
            throw new System.NotImplementedException();
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

        public void Close(string reason, object details)
        {
            reason = reason ?? WampErrors.CloseNormal;
            details = details ?? EmptyDetails;

            mGoodbyeSent = true;
            mServerProxy.Goodbye(details, reason);
        }

        public void OnConnectionOpen()
        {
            mServerProxy.Hello
                (Realm.Name,
                 mDetails);
        }

        public void OnConnectionClosed()
        {
            SetOpenTaskErrorIfNeeded(new Exception("Connection closed before connection established."));

            if (!mConnectionBrokenRaised)
            {
                RaiseConnectionBroken(WampObjectFormatter.Value,
                                      SessionCloseType.Disconnection,
                                      null,
                                      null);                
            }

            mConnectionBrokenRaised = false;
        }

        public void OnConnectionError(Exception exception)
        {
            SetOpenTaskErrorIfNeeded(exception);

            OnConnectionError(new WampConnectionErrorEventArgs(exception));

            mConnectionBrokenRaised = false;
        }

        private void SetOpenTaskErrorIfNeeded(Exception exception)
        {
            TaskCompletionSource<bool> openTask;

            lock (mLock)
            {
                openTask = mOpenTask;
                mOpenTask = new TaskCompletionSource<bool>();
            }

            if (openTask != null)
            {
                openTask.SetException(exception);
            }
        }

        public event EventHandler<WampSessionEventArgs> ConnectionEstablished;

        public event EventHandler<WampSessionCloseEventArgs> ConnectionBroken;

        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

        protected virtual void OnConnectionEstablished(WampSessionEventArgs e)
        {
            EventHandler<WampSessionEventArgs> handler = ConnectionEstablished;
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