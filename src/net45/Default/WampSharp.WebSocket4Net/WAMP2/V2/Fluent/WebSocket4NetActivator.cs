using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.WebSocket4Net;
using WebSocket4Net;

namespace WampSharp.V2.Fluent
{
    internal class WebSocket4NetActivator : IWampConnectionActivator
    {
        private readonly Func<WebSocket> mWebSocketFactory;

        public WebSocket4NetActivator(Func<WebSocket> webSocketFactory)
        {
            mWebSocketFactory = webSocketFactory;
        }

        public WebSocket4NetActivator(string serverAddress) : 
            this(() => new WebSocket(serverAddress))
        {
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

            IWampBinaryBinding<TMessage> binaryBinding = binding as IWampBinaryBinding<TMessage>;

            if (binaryBinding != null)
            {
                return CreateBinaryConnection(binaryBinding);
            }

            throw new Exception();
        }

        protected IControlledWampConnection<TMessage> CreateBinaryConnection<TMessage>(IWampBinaryBinding<TMessage> binaryBinding)
        {
            return new WebSocket4NetBinaryConnection<TMessage>(mWebSocketFactory(), binaryBinding);
        }

        protected IControlledWampConnection<TMessage> CreateTextConnection<TMessage>(IWampTextBinding<TMessage> textBinding)
        {
            return new WebSocket4NetTextConnection<TMessage>(mWebSocketFactory(), textBinding);
        }
    }
}