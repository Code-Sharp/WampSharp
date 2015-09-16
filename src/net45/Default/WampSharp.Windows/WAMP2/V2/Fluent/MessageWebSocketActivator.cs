#if PCL
using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.Windows;

namespace WampSharp.V2.Fluent
{
    internal class MessageWebSocketActivator : IWampConnectionActivator
    {
        private readonly string mServerAddress;

        public MessageWebSocketActivator(string serverAddress)
        {
            mServerAddress = serverAddress;
        }

        public IControlledWampConnection<TMessage> Activate<TMessage>(IWampBinding<TMessage> binding)
        {
            Func<IControlledWampConnection<TMessage>> factory = 
                () => GetConnectionFactory(binding);

            ReviveClientConnection<TMessage> result = 
                new ReviveClientConnection<TMessage>(factory);

            return result;
        }

        private IControlledWampConnection<TMessage> GetConnectionFactory<TMessage>(IWampBinding<TMessage> binding)
        {
            IWampTextBinding<TMessage> textBinding = binding as IWampTextBinding<TMessage>;

            if (textBinding != null)
            {
                return CreateTextConnection(textBinding);
            }

            throw new Exception();
        }

        protected IControlledWampConnection<TMessage> CreateTextConnection<TMessage>(IWampTextBinding<TMessage> textBinding)
        {
            return new MessageWebSocketTextConnection<TMessage>(mServerAddress, textBinding);
        }
    }
}

#endif