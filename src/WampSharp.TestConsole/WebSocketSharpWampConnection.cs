using System;
using System.IO;
using System.Reactive.Subjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WebSocketSharp;

namespace WampSharp.TestConsole
{
    internal class WebSocketSharpWampConnection : IWampConnection<JToken>
    {
        private readonly Subject<WampMessage<JToken>> mWampMessageSubject =
            new Subject<WampMessage<JToken>>();

        private readonly WebSocket mWebSocketConnection;
        private readonly IWampMessageFormatter<JToken> mMessageFormatter;

        public WebSocketSharpWampConnection(string url,
                                            IWampMessageFormatter<JToken> messageFormatter)
        {
            mWebSocketConnection = new WebSocket(url, (sender, args) => { }, (sender, args) => OnConnectionMessage(args.Data), (sender, args) => OnConnectionError(new Exception(args.Message)), (sender, args) => OnConnectionClose(),
                                                 new[]{"wamp"});
            
            mMessageFormatter = messageFormatter;
        }

        private void OnConnectionError(Exception exception)
        {
            mWampMessageSubject.OnError(exception);
        }

        private void OnConnectionClose()
        {
            mWampMessageSubject.OnCompleted();
        }

        private void OnConnectionMessage(string message)
        {
            JToken raw = JToken.Parse(message);
            WampMessage<JToken> parsed = mMessageFormatter.Parse(raw);
            mWampMessageSubject.OnNext(parsed);
        }

        public void OnNext(WampMessage<JToken> value)
        {
            JToken raw = mMessageFormatter.Format(value);

            StringWriter stringWriter = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(stringWriter)
                                        {
                                            Formatting = Formatting.None
                                        };
            raw.WriteTo(writer);

            mWebSocketConnection.Send(stringWriter.ToString());
        }

        public void OnError(Exception error)
        {
            // Not sure what to do here.
            // Send an error message to the client???
            throw error;
        }

        public void OnCompleted()
        {
            mWebSocketConnection.Close();
        }

        public IDisposable Subscribe(IObserver<WampMessage<JToken>> observer)
        {
            return mWampMessageSubject.Subscribe(observer);
        }
    }
}