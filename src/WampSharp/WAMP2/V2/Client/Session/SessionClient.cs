using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    public class SessionClient<TMessage> : IWampSessionClientExtended<TMessage>
    {
        private readonly IWampRealmProxy mRealm;
        private IWampServerProxy mServerProxy;
        private long mSession;
        private TaskCompletionSource<bool> mOpenTask = new TaskCompletionSource<bool>();

        public SessionClient(IWampRealmProxy realm)
        {
            mRealm = realm;
            mServerProxy = realm.Proxy;
        }

        public void Challenge(string challenge, TMessage extra)
        {
            throw new System.NotImplementedException();
        }

        public void Welcome(long session, TMessage details)
        {
            mSession = session;
            mOpenTask.SetResult(true);
        }

        public void Abort(TMessage details, string reason)
        {
            throw new System.NotImplementedException();
        }

        public void Goodbye(TMessage details, string reason)
        {
            throw new System.NotImplementedException();
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

        public void OnConnectionOpen()
        {
            mServerProxy.Hello
                (Realm.Name,
                 new Dictionary<string, object>()
                     {
                         {
                             "roles",
                             new Dictionary<string, object>()
                                 {
                                     {"caller", new Dictionary<string, object>()},
                                     {"callee", new Dictionary<string, object>()},
                                     {"publisher", new Dictionary<string, object>()},
                                     {"subscriber", new Dictionary<string, object>()},
                                 }
                         }
                     });
        }

        public void OnConnectionClosed()
        {
            mOpenTask = new TaskCompletionSource<bool>();
        }
    }
}