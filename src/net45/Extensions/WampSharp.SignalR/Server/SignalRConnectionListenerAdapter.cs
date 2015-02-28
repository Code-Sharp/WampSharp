using System;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.SignalR
{
    internal class SignalRConnectionListenerAdapter<TMessage> :
        IWampConnectionListener<TMessage>, ISignalRConnectionListenerAdapter
    {
        private readonly IWampTextBinding<TMessage> mBinding;
        private readonly Subject<IWampConnection<TMessage>> mSubject = new Subject<IWampConnection<TMessage>>();

        private readonly ConcurrentDictionary<string, SignalRTextConnection> mConnectionIdToConnection =
            new ConcurrentDictionary<string, SignalRTextConnection>();

        public SignalRConnectionListenerAdapter(IWampTextBinding<TMessage> binding)
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

        private class SignalRTextConnection : AsyncWampConnection<TMessage>
        {
            private readonly IConnection mConnection;
            private readonly string mConnectionId;
            private readonly SignalRConnectionListenerAdapter<TMessage> mParent;
            private readonly IWampTextBinding<TMessage> mBinding;

            public SignalRTextConnection(IConnection connection, string connectionId,
                                         SignalRConnectionListenerAdapter<TMessage> parent)
            {
                mConnection = connection;
                mConnectionId = connectionId;
                mBinding = parent.mBinding;
                mParent = parent;
            }

            public override void Dispose()
            {
                SignalRTextConnection value;
                mParent.mConnectionIdToConnection.TryRemove(this.mConnectionId, out value);
            }

            protected override bool IsConnected
            {
                get { return true; }
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

            protected override Task SendAsync(WampMessage<TMessage> message)
            {
                string text = mBinding.Format(message);
                return mConnection.Send(mConnectionId, text);
            }
        }
    }
}