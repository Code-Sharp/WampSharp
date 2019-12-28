using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm.Binded
{
    internal static class WampGoodbyeExtensions
    {
        public static void SendGoodbye<TMessage>(this IWampClientProxy<TMessage> clientProxy, GoodbyeDetails details, string reason)
        {
            using (clientProxy as IDisposable)
            {
                clientProxy.GoodbyeSent = true;
                clientProxy.Goodbye(details, reason);
                clientProxy.Realm.Goodbye(clientProxy.Session, details, reason);
            }
        }
    }
}