using Fleck;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;

namespace WampSharp.Fleck
{
    /// <summary>
    /// Represents a WebSocket transport implemented with Fleck.
    /// </summary>
    public class FleckWebSocketTransport : WebSocketTransport<IWebSocketConnection>
    {
        private readonly WebSocketServer mServer;

        /// <summary>
        /// Creates a new instance of <see cref="FleckWebSocketTransport"/>
        /// given the server address to run at.
        /// </summary>
        /// <param name="location">The given server address.</param>
        public FleckWebSocketTransport(string location)
        {
            mServer = new WebSocketServer(location);
        }

        protected override void OpenConnection<TMessage>(IWampConnection<TMessage> connection)
        {
        }

        public override void Dispose()
        {
            mServer.Dispose();
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>(IWebSocketConnection connection, IWampBinaryBinding<TMessage> binding)
        {
            return new FleckWampBinaryConnection<TMessage>(connection, binding);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>(IWebSocketConnection connection, IWampTextBinding<TMessage> binding)
        {
            return new FleckWampTextConnection<TMessage>(connection, binding);
        }

        public override void Open()
        {
            string[] protocols = this.SubProtocols;

            mServer.SupportedSubProtocols = protocols;

            mServer.Start(OnNewConnection);
        }

        protected override string GetSubProtocol(IWebSocketConnection connection)
        {
            string protocol = connection.ConnectionInfo.NegotiatedSubProtocol;
            return protocol;
        }
    }
}