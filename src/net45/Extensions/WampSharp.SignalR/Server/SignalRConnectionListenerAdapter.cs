using System;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using Microsoft.AspNet.SignalR;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.SignalR
{
    internal class SignalRConnectionListenerAdapter<TMessage> :
        IWampConnectionListener<TMessage>, ISignalRConnectionListenerAdapter
    {
        private readonly IWampTransportBinding<TMessage, string> mBinding;
        private readonly Subject<IWampConnection<TMessage>> mSubject = new Subject<IWampConnection<TMessage>>();

        private readonly ConcurrentDictionary<string, SignalRTextConnection> mConnectionIdToConnection =
            new ConcurrentDictionary<string, SignalRTextConnection>();

        public SignalRConnectionListenerAdapter(IWampTransportBinding<TMessage, string> binding)
        {
            mBinding = binding;
        }

        public void OnConnected(string connectionId, IConnection connection)
        {
            SignalRTextConnection textConnection = new SignalRTextConnection(connection, connectionId, this);

            mConnectionIdToConnection[connectionId] = textConnection;

            mSubject.OnNext(textConnection);

            textConnection.OnConnected();
        }

        public void OnReceived(string connectionId, string data)
        {
            SignalRTextConnection connection;

            if (mConnectionIdToConnection.TryGetValue(connectionId, out connection))
            {
                connection.OnMessage(data);
            }
        }

        public void OnDisconnected(string connectionId)
        {
            SignalRTextConnection connection;

            if (mConnectionIdToConnection.TryGetValue(connectionId, out connection))
            {
                connection.OnDisconnected();
            }
        }

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
        {
            return mSubject.Subscribe(observer);
        }

        private class SignalRTextConnection : IWampConnection<TMessage>
        {
            private readonly IConnection mConnection;
            private readonly string mConnectionId;
            private readonly SignalRConnectionListenerAdapter<TMessage> mParent;
            private readonly IWampTransportBinding<TMessage, string> mBinding;

            public SignalRTextConnection(IConnection connection, string connectionId,
                                         SignalRConnectionListenerAdapter<TMessage> parent)
            {
                mConnection = connection;
                mConnectionId = connectionId;
                mBinding = parent.mBinding;
                mParent = parent;
            }

            public void Dispose()
            {
                SignalRTextConnection value;
                mParent.mConnectionIdToConnection.TryRemove(this.mConnectionId, out value);
            }

            public void Send(WampMessage<TMessage> message)
            {
                string text = mBinding.Format(message);
                mConnection.Send(mConnectionId, text);
            }

            public void OnConnected()
            {
                RaiseConnectionOpen();
            }

            public void OnMessage(string text)
            {
                WampMessage<TMessage> message = mBinding.Parse(text);

                RaiseMessageArrived(message);
            }

            public void OnDisconnected()
            {
                RaiseConnectionClosed();
            }

            public event EventHandler ConnectionOpen;

            public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;

            public event EventHandler ConnectionClosed;

            public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

            protected virtual void RaiseConnectionOpen()
            {
                EventHandler handler = ConnectionOpen;
                if (handler != null) handler(this, EventArgs.Empty);
            }

            protected virtual void RaiseMessageArrived(WampMessage<TMessage> message)
            {
                EventHandler<WampMessageArrivedEventArgs<TMessage>> handler = MessageArrived;

                if (handler != null)
                {
                    WampMessageArrivedEventArgs<TMessage> e =
                        new WampMessageArrivedEventArgs<TMessage>(message);

                    handler(this, e);
                }
            }

            protected virtual void RaiseConnectionClosed()
            {
                EventHandler handler = ConnectionClosed;
                if (handler != null) handler(this, EventArgs.Empty);
            }

            protected virtual void RaiseConnectionError(WampConnectionErrorEventArgs e)
            {
                EventHandler<WampConnectionErrorEventArgs> handler = ConnectionError;
                if (handler != null) handler(this, e);
            }
        }
    }
}