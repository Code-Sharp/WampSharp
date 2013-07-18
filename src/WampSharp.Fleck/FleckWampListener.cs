using System;
using System.Collections.Generic;
using System.Reactive;
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

        private readonly Subject<Unit> mShutdown = new Subject<Unit>(); 
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
                    mShutdown.OnNext(Unit.Default);
                    mServer = null;
                }
            }
        }

        private void StartServer()
        {
            WebSocketServer server = new WebSocketServer(mLocation);
            mServer = server;
            server.SupportedSubProtocols = new[] {"wamp"};

            mServer.Start(connection =>
                              {
                                  FleckWampConnection<TMessage> wampConnection =
                                      new FleckWampConnection<TMessage>(connection,
                                                                        mParser,
                                                                        mShutdown);

                                  connection.OnOpen =
                                      () => OnNewConnection(wampConnection);
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
            private readonly IDisposable mShutdownSubscrition;

            public FleckWampConnection(IWebSocketConnection webSocketConnection,
                                       IWampMessageParser<TMessage> messageParser, IObservable<Unit> shutdown)
            {
                mWebSocketConnection = webSocketConnection;
                mMessageParser = messageParser;
                mWebSocketConnection.OnMessage = OnConnectionMessage;
                mWebSocketConnection.OnError = OnConnectionError;
                mWebSocketConnection.OnClose = OnConnectionClose;

                mShutdownSubscrition = shutdown.Subscribe
                    (x => OnCompleted());
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
                lock (mLock)
                {
                    if (!mClosed)
                    {
                        WampMessage<TMessage> parsed =
                            mMessageParser.Parse(message);

                        mWampMessageSubject.OnNext(parsed);
                    }
                }
            }

            public void OnNext(WampMessage<TMessage> value)
            {
                lock (mLock)
                {
                    if (!mClosed)
                    {
                        string raw = mMessageParser.Format(value);

                        mWebSocketConnection.Send(raw);
                    }
                }
            }

            public void OnError(Exception error)
            {
                // Not sure what to do here.
                // Send an error message to the client???
                throw error;
            }

            public void OnCompleted()
            {
                lock (mLock)
                {
                    if (!mClosed)
                    {
                        mWebSocketConnection.Close();
                        mShutdownSubscrition.Dispose();
                    }
                }
            }

            public IDisposable Subscribe(IObserver<WampMessage<TMessage>> observer)
            {
                lock (mLock)
                {
                    if (!mClosed)
                    {
                        return mWampMessageSubject.Subscribe(observer);
                    }
                }

                return Disposable.Empty;
            }
        }
    }
}