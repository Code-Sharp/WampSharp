using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Core.Dispatch
{
    internal class WampRealmMethodBuilder<TMessage> : WampMethodBuilder<TMessage, IWampClientProxy<TMessage>>
    {
        public WampRealmMethodBuilder(object instance, IWampFormatter<TMessage> formatter) : 
            base(instance, formatter)
        {
        }

        protected override object GetInstance(IWampClientProxy<TMessage> client, WampSharp.Core.Message.WampMessage<TMessage> message, WampMethodInfo method)
        {
            IWampBindedRealm<TMessage> realm = client.Realm;

            if (realm == null)
            {
                return base.GetInstance(client, message, method);
            }

            return realm.Server;
        }
    }
}