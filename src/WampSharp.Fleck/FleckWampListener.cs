using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Fleck;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.Fleck
{
    public class FleckWampConnectionListener<TMessage> : IWampConnectionListener<TMessage>
    {
        private readonly Subject<IWampConnection<TMessage>> mSubject =
            new Subject<IWampConnection<TMessage>>();

        private IWebSocketServer mServer;
        private readonly string mLocation;
        private readonly IWampMessageParser<TMessage> mParser;
        private readonly object mLock = new object();

        public FleckWampConnectionListener(string location,
                                           IWampMessageParser<TMessage> parser)
        {
            mLocation = location;
            mParser = parser;
        }

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
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

            Action<IWebSocketConnection> defaultInitializer =
                connection =>
                    {
                        FleckWampConnection<TMessage> wampConnection =
                            new FleckWampConnection<TMessage>(connection,
                                                              mParser);

                        connection.OnOpen =
                            () => OnNewConnection(wampConnection);
                    };

            mServer.Start(defaultInitializer,
                          new Dictionary<string, Action<IWebSocketConnection>>()
                              {
                                  {"wamp", defaultInitializer}
                              });
        }


        private void OnNewConnection(FleckWampConnection<TMessage> wampConnection)
        {
            mSubject.OnNext(wampConnection);
        }

        private class FleckWampConnection<TMessage> : IWampConnection<TMessage>
        {
            private readonly Subject<WampMessage<TMessage>> mWampMessageSubject =
                new Subject<WampMessage<TMessage>>();

            private readonly IWebSocketConnection mWebSocketConnection;
            private readonly IWampMessageParser<TMessage> mMessageParser;
            private readonly object mLock = new object();
            private bool mClosed = false;

            public FleckWampConnection(IWebSocketConnection webSocketConnection,
                                       IWampMessageParser<TMessage> messageParser)
            {
                mWebSocketConnection = webSocketConnection;
                mMessageParser = messageParser;
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
                lock (mLock)
                {
                    if (!mClosed)
                    {
                        mWampMessageSubject.OnCompleted();
                        mWampMessageSubject.Dispose();
                        mClosed = true;
                    }
                }
            }

            private void OnConnectionMessage(string message)
            {
                WampMessage<TMessage> parsed =
                    mMessageParser.Parse(message);

                mWampMessageSubject.OnNext(parsed);
            }

            public void OnNext(WampMessage<TMessage> value)
            {
                string raw = mMessageParser.Format(value);

                mWebSocketConnection.Send(raw);
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

            public IDisposable Subscribe(IObserver<WampMessage<TMessage>> observer)
            {
                return mWampMessageSubject.Subscribe(observer);
            }
        }
    }
}