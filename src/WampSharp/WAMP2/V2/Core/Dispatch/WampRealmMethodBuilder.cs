using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Core.Dispatch
{
    internal class WampRealmMethodBuilder<TMessage> : WampMethodBuilder<TMessage, IWampClient<TMessage>>
        where TMessage : class 
    {
        public WampRealmMethodBuilder(object instance, IWampFormatter<TMessage> formatter) : 
            base(instance, formatter)
        {
        }

        protected override object GetInstance(IWampClient<TMessage> client, WampSharp.Core.Message.WampMessage<TMessage> message, WampMethodInfo method)
        {
            IWampRealm<TMessage> realm = client.Realm;

            if (realm == null)
            {
                return base.GetInstance(client, message, method);
            }

            return realm.Server;
        }
    }
}