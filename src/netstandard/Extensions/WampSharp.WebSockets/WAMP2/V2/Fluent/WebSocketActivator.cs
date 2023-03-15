using System;
using System.Net.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.WebSockets;

namespace WampSharp.V2.Fluent
{
    internal class WebSocketActivator : IWampConnectionActivator
    {
        private readonly Uri mServerAddress;

        public WebSocketActivator(Uri serverAddress)
        {
            mServerAddress = serverAddress;
            WebSocketFactory = () => new ClientWebSocket();
        }

        public WebSocketFactory WebSocketFactory { get; set; }

        public Action<ClientWebSocketOptions> ConfigureOptions { get; set; }

        public int? MaxFrameSize { get; set; }

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
            switch (binding)
            {
                case IWampTextBinding<TMessage> textBinding:
                    return CreateTextConnection(textBinding, MaxFrameSize);
                case IWampBinaryBinding<TMessage> binaryBinding:
                    return CreateBinaryConnection(binaryBinding, MaxFrameSize);
            }

            throw new Exception();
        }

        protected IControlledWampConnection<TMessage> CreateBinaryConnection<TMessage>(
            IWampBinaryBinding<TMessage> binaryBinding, int? maxFrameSize)
        {
            return new ControlledBinaryWebSocketConnection<TMessage>(ActivateWebSocket(), mServerAddress, binaryBinding, maxFrameSize);
        }

        protected IControlledWampConnection<TMessage> CreateTextConnection<TMessage>(
            IWampTextBinding<TMessage> textBinding, int? maxFrameSize)
        {
            return new ControlledTextWebSocketConnection<TMessage>(ActivateWebSocket(), mServerAddress, textBinding, maxFrameSize);
        }

        private ClientWebSocket ActivateWebSocket()
        {
            ClientWebSocket result = WebSocketFactory();

            ConfigureOptions?.Invoke(result.Options);

            return result;
        }
    }
}
