using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using SuperSocket.ClientEngine;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WebSocket4Net;

namespace WampSharp.WebSocket4Net
{
    public class WebSocket4NetConnection<TMessage> : IWampConnection<TMessage>
    {
        #region Fields

        private readonly IWampMessageParser<TMessage> mMessageFormatter;
        
        private readonly WebSocket mWebSocket;
        
        private readonly Subject<WampMessage<TMessage>> mSubject = 
            new Subject<WampMessage<TMessage>>();


        private readonly ManualResetEvent mConnected = new ManualResetEvent(false);

        private readonly object mLock = new object();

        #endregion

        public WebSocket4NetConnection(string serverAddress,
                                       IWampMessageParser<TMessage> messageFormatter)
        {
            mMessageFormatter = messageFormatter;
            mWebSocket = new WebSocket(serverAddress, "wamp");
            mWebSocket.Opened += WebSocketOnOpened;
            mWebSocket.Closed += WebSocketOnClosed;
            mWebSocket.MessageReceived += WebSocketOnMessageReceived;
            mWebSocket.Error += WebSocketOnError;
            mWebSocket.Open();
        }

        private void WebSocketOnError(object sender, ErrorEventArgs errorEventArgs)
        {
            mSubject.OnError(errorEventArgs.Exception);
        }

        private void WebSocketOnMessageReceived(object sender, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            WampMessage<TMessage> parsed = 
                mMessageFormatter.Parse(messageReceivedEventArgs.Message);

            mSubject.OnNext(parsed);
        }

        private void WebSocketOnClosed(object sender, EventArgs eventArgs)
        {
            mSubject.OnCompleted();
        }

        private void WebSocketOnOpened(object sender, EventArgs eventArgs)
        {
            mConnected.Set();
        }

        public void OnNext(WampMessage<TMessage> value)
        {
            mConnected.WaitOne();

            string formatted = mMessageFormatter.Format(value);
            mWebSocket.Send(formatted);
        }

        public void OnError(Exception error)
        {
            // No can do.
        }

        public void OnCompleted()
        {
            mWebSocket.Close();
        }

        public IDisposable Subscribe(IObserver<WampMessage<TMessage>> observer)
        {
            lock (mLock)
            {
                bool hasObservers = mSubject.HasObservers;

                IDisposable result = mSubject.Subscribe(observer);

                //if (!hasObservers)
                //{
                //    mWebSocket.Open();
                //}

                return new CompositeDisposable
                    (result,
                     Disposable.Create(OnDisposed));
            }
        }

        private void OnDisposed()
        {
            lock (mLock)
            {
                //if (!mSubject.HasObservers)
                //{
                //    mWebSocket.Close();
                //}
            }
        }
    }
}
