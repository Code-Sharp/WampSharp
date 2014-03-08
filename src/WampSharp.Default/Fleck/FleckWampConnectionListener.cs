using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Fleck;
using WampSharp.Binding;
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
        private string mProtocol;

        public FleckWampConnectionListener(string protocol,
                                           string location,
                                           IWampMessageParser<TMessage> parser)
        {
            mLocation = location;
            mParser = parser;
            mProtocol = protocol;
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
            WebSocketServer server = new WebSocketServer(mLocation);
            mServer = server;
            server.SupportedSubProtocols = new[] {mProtocol};

            mServer.Start(connection =>
                              {
                                  FleckWampConnection wampConnection =
                                      new FleckWampConnection(connection,
                                                              mParser);

                                  OnNewConnection(wampConnection);
                              });
        }

        private void OnNewConnection(FleckWampConnection wampConnection)
        {
            mSubject.OnNext(wampConnection);
        }

        private class FleckWampConnection : IWampConnection<TMessage>
        {
            private readonly IWebSocketConnection mWebSocketConnection;
            private readonly IWampMessageParser<TMessage> mMessageParser;
            private readonly object mLock = new object();
            private bool mClosed = false;

            public FleckWampConnection(IWebSocketConnection webSocketConnection,
                                       IWampMessageParser<TMessage> messageParser)
            {
                mWebSocketConnection = webSocketConnection;
                mMessageParser = messageParser;
                mWebSocketConnection.OnOpen = OnConnectionOpen;
                mWebSocketConnection.OnMessage = OnConnectionMessage;
                mWebSocketConnection.OnError = OnConnectionError;
                mWebSocketConnection.OnClose = OnConnectionClose;
            }

            private void OnConnectionOpen()
            {
                RaiseConnectionOpen();
            }

            private void OnConnectionError(Exception exception)
            {
                // throw event :)
            }

            private void OnConnectionClose()
            {
                lock (mLock)
                {
                    if (!mClosed)
                    {
                        mClosed = true;
                    }
                }

                RaiseConnectionClosed();
            }

            private void RaiseConnectionOpen()
            {
                EventHandler connectionOpen = this.ConnectionOpen;

                if (connectionOpen != null)
                {
                    connectionOpen(this, EventArgs.Empty);
                }
            }

            private void RaiseConnectionClosed()
            {
                EventHandler connectionClosed = this.ConnectionClosed;

                if (connectionClosed != null)
                {
                    connectionClosed(this, EventArgs.Empty);
                }
            }

            private void OnConnectionMessage(string message)
            {
                WampMessage<TMessage> parsed =
                    mMessageParser.Parse(message);

                RaiseNewMessageArrived(parsed);
            }

            private void RaiseNewMessageArrived(WampMessage<TMessage> parsed)
            {
                var messageArrived = this.MessageArrived;

                if (messageArrived != null)
                {
                    messageArrived(this, new WampMessageArrivedEventArgs<TMessage>(parsed));
                }
            }

            public void Dispose()
            {
                lock (mLock)
                {
                    if (!mClosed)
                    {
                        mWebSocketConnection.Close();
                    }
                }
            }

            public void Send(WampMessage<TMessage> message)
            {
                lock (mLock)
                {
                    if (!mClosed)
                    {
                        TextMessage<TMessage> casted =
                            message as TextMessage<TMessage>;

                        string raw;

                        if (casted != null)
                        {
                            raw = casted.Text;
                        }
                        else
                        {
                            raw = mMessageParser.Format(message);
                        }

                        mWebSocketConnection.Send(raw);
                    }
                }
            }

            public event EventHandler ConnectionOpen;
            public event EventHandler ConnectionOpening;
            public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
            public event EventHandler ConnectionClosing;
            public event EventHandler ConnectionClosed;
        }
    }
}