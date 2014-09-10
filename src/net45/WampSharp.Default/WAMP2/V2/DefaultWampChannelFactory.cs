using WampSharp.Binding;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.WebSocket4Net;

namespace WampSharp.V2
{
    public class DefaultWampChannelFactory : WampChannelFactory
    {
        private readonly JTokenMsgpackObjectBinding mMsgpackBinding = new JTokenMsgpackObjectBinding();
        private readonly JTokenBinding mJsonBinding = new JTokenBinding();

        public IWampChannel CreateChannel<TMessage>(string address,
                                                    string realm,
                                                    IWampTextBinding<TMessage> binding)
        {
            var connection = 
                new WebSocket4NetTextConnection<TMessage>(address, binding);
            
            return this.CreateChannel(realm, connection, binding);
        }

        public IWampChannel CreateChannel<TMessage>(string address,
                                                    string realm,
                                                    IWampBinaryBinding<TMessage> binding)
        {
            var connection =
                new WebSocket4NetBinaryConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connection, binding);
        }

        public IWampChannel CreateJsonChannel(string address,
                                              string realm)
        {
            return this.CreateChannel(address, realm, mJsonBinding);
        }

        public IWampChannel CreateMsgpackChannel(string address,
                                                 string realm)
        {
            return this.CreateChannel(address, realm, mMsgpackBinding);
        }
    }
}