using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm.Binded
{
    internal class WampSessionClientTerminator : IWampSessionClientTerminator
    {
        private readonly IWampSessionClient mSession;

        public WampSessionClientTerminator(IWampSessionClient session)
        {
            mSession = session;
        }

        public void Disconnect(GoodbyeDetails details, string reason)
        {
            mSession.Goodbye(details, reason);
        }
    }
}