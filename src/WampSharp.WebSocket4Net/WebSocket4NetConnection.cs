using System;
using System.Reactive.Subjects;
using SuperSocket.ClientEngine;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WebSocket4Net;

namespace WampSharp.WebSocket4Net
{
    public class WebSocket4NetConnection<TMessage> : IControlledWampConnection<TMessage>
    {
        #region Fields

        private readonly IWampMessageParser<TMessage> mMessageFormatter;
        
        private readonly WebSocket mWebSocket;
        
        private readonly Subject<WampMessage<TMessage>> mSubject = 
            new Subject<WampMessage<TMessage>>();

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
        }

        public void OnNext(WampMessage<TMessage> value)
        {
            string formatted = mMessageFormatter.Format(value);
            mWebSocket.Send(formatted);
        }

        public void OnError(Exception error)
        {
            // No can do.
        }

        public void OnCompleted()
        {
            mSubject.OnCompleted();
            mWebSocket.Close();
        }

        public IDisposable Subscribe(IObserver<WampMessage<TMessage>> observer)
        {
            IDisposable result = mSubject.Subscribe(observer);

            return result;
        }

        public void Connect()
        {
            mWebSocket.Open();
        }
    }
}
