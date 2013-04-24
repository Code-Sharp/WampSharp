using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Fleck
{
    public class FleckWampConnectionListener : IWampConnectionListener<JToken>
    {
        private readonly Subject<IWampConnection<JToken>> mSubject = 
            new Subject<IWampConnection<JToken>>();

        private IWebSocketServer mServer;
        private readonly string mLocation;
        private readonly IWampMessageFormatter<JToken> mMessageFormatter;
        private readonly object mLock = new object();

        public FleckWampConnectionListener(string location, IWampMessageFormatter<JToken> messageFormatter)
        {
            mLocation = location;
            mMessageFormatter = messageFormatter;
        }

        public IDisposable Subscribe(IObserver<IWampConnection<JToken>> observer)
        {
            lock (mLock)
            {
                if (mServer == null)
                {
                    StartServer();
                }

                return new CompositeDisposable(mSubject.Subscribe(observer),
                                               Disposable.Create(StopServerIfNeeded));
            }
        }

        private void StopServerIfNeeded()
        {
            lock (mLock)
            {
                if (!mSubject.HasObservers)
                {
                    mServer.Dispose();
                    mServer = null;
                }                
            }
        }

        private void StartServer()
        {
            mServer = new WebSocketServer(mLocation);

            mServer.Start(connection =>
                              {
                                  FleckWampConnection wampConnection =
                                      new FleckWampConnection(connection,
                                                              mMessageFormatter);

                                  connection.OnOpen = () => OnNewConnection(wampConnection);
                              });
        }


        private void OnNewConnection(FleckWampConnection wampConnection)
        {
            mSubject.OnNext(wampConnection);
        }

        internal class FleckWampConnection : IWampConnection<JToken>
        {
            private readonly Subject<WampMessage<JToken>> mWampMessageSubject =
                new Subject<WampMessage<JToken>>();

            private readonly IWebSocketConnection mWebSocketConnection;
            private readonly IWampMessageFormatter<JToken> mMessageFormatter;

            public FleckWampConnection(IWebSocketConnection webSocketConnection,
                                       IWampMessageFormatter<JToken> messageFormatter)
            {
                mWebSocketConnection = webSocketConnection;
                mMessageFormatter = messageFormatter;
                mWebSocketConnection.OnMessage = OnConnectionMessage;
                mWebSocketConnection.OnError = OnConnectionError;
                mWebSocketConnection.OnClose = OnConnectionClose;
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
}