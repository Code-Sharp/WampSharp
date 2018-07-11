using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm.Binded
{
    internal class WampSessionClientTerminator : IWampSessionTerminator
    {
        private readonly IWampClientProxy mClientProxy;

        public WampSessionClientTerminator(IWampClientProxy clientProxy)
        {
            mClientProxy = clientProxy;
        }

        public void Disconnect(GoodbyeDetails details, string reason)
        {
            using (mClientProxy as IDisposable)
            {
                mClientProxy.Goodbye(details, reason);
                mClientProxy.GoodbyeSent = true;
            }
        }
    }
}