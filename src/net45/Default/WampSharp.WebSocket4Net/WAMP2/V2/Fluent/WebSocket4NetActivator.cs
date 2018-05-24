using System;
using SuperSocket.ClientEngine;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.WebSocket4Net;
using WebSocket4Net;

namespace WampSharp.V2.Fluent
{
    /// <summary>
    /// A delegate that creates a new instance of a <see cref="WebSocket"/>, given the subprotocol to be used.
    /// </summary>
    /// <param name="subprotocolName">The subprotocol to be used.</param>
    public delegate WebSocket WebSocket4NetFactory(string subprotocolName);

    internal class WebSocket4NetActivator : IWampConnectionActivator
    {
        private readonly WebSocket4NetFactory mWebSocketFactory;

        public WebSocket4NetActivator(WebSocket4NetFactory webSocketFactory)
        {
            mWebSocketFactory = webSocketFactory;
        }

        public WebSocket4NetActivator(string serverAddress) : 
            this(subprotocolName => new WebSocket(serverAddress, subprotocolName, WebSocketVersion.None))
        {
        }

        public Action<SecurityOption> SecurityOptionsConfigureAction { get; set; }

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
            return new WebSocket4NetBinaryConnection<TMessage>(ActivateWebSocket(binaryBinding), binaryBinding);
        }

        protected IControlledWampConnection<TMessage> CreateTextConnection<TMessage>(IWampTextBinding<TMessage> textBinding)
        {
            return new WebSocket4NetTextConnection<TMessage>(ActivateWebSocket(textBinding), textBinding);
        }

        private WebSocket ActivateWebSocket(IWampBinding binaryBinding)
        {
            WebSocket webSocket = mWebSocketFactory(binaryBinding.Name);

            if (SecurityOptionsConfigureAction != null)
            {
                SecurityOptionsConfigureAction(webSocket.Security);
            }

            return webSocket;
        }
    }
}