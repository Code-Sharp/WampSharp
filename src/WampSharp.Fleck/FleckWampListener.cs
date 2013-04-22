using System;
using Fleck;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Contracts;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Fleck
{
    public class FleckWampListener
    {
        private readonly IWampMessageFormatter<JToken> mWampMessageFormatter;
        private readonly IWampIncomingMessageHandler<JToken> mHandler;
        private readonly IWampClientContainer<IWebSocketConnection> mClientContainer;
        private readonly IWebSocketServer mServer;

        public FleckWampListener(string location,
                                 IWampMessageFormatter<JToken> wampMessageFormatter,
                                 IWampIncomingMessageHandler<JToken> handler,
                                 IWampClientContainer<IWebSocketConnection> clientContainer)
        {
            mWampMessageFormatter = wampMessageFormatter;
            mHandler = handler;
            mClientContainer = clientContainer;
            mServer = new WebSocketServer(location);
        }

        public void Start()
        {
            mServer.Start(connection =>
                              {
                                  connection.OnOpen = () => OnNewConnection(connection);
                                  connection.OnMessage = message => OnNewMessage(connection, message);
                                  connection.OnClose = () => OnCloseConnection(connection);
                                  connection.OnError = exception => OnConnectionException(connection, exception);
                              });
        }

        private void OnConnectionException(IWebSocketConnection connection, Exception exception)
        {
        }

        private void OnCloseConnection(IWebSocketConnection connection)
        {
            IDisposable client = mClientContainer.GetClient(connection) as IDisposable;

            if (client != null)
            {
                client.Dispose();
            }
        }

        private void OnNewMessage(IWebSocketConnection connection, string message)
        {
            IWampClient client = mClientContainer.GetClient(connection);

            WampMessage<JToken> parsed = mWampMessageFormatter.Parse(JToken.Parse(message));

            mHandler.HandleMessage(client, parsed);
        }

        private void OnNewConnection(IWebSocketConnection connection)
        {
            IWampClient client = mClientContainer.GetClient(connection);

            client.Welcome("NwtXQ8rdfPsy-ewS", 1, "WampSharp");
        }
    }
}
