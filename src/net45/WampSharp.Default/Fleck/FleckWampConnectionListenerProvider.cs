using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Fleck;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Fleck
{
    public class FleckWampConnectionListenerProvider : IWampConnectionListenerProvider
    {
        private readonly WebSocketServer mServer;

        private readonly IDictionary<string, ConnectionListener> mBindings =
            new Dictionary<string, ConnectionListener>();

        public FleckWampConnectionListenerProvider(string location)
        {
            mServer = new WebSocketServer(location);
        }

        public void Dispose()
        {
            foreach (ConnectionListener connectionListener in mBindings.Values)
            {
                connectionListener.Dispose();
            }

            mServer.Dispose();
        }

        public void Open()
        {
            string[] protocols = mBindings.Keys.ToArray();

            mServer.SupportedSubProtocols = protocols;

            mServer.Start(OnNewConnection);
        }

        private void OnNewConnection(IWebSocketConnection connection)
        {
            string protocol = connection.ConnectionInfo.NegotiatedSubProtocol;

            ConnectionListener listener = mBindings[protocol];

            listener.OnNewConnection(connection);
        }

        public IWampConnectionListener<TMessage> GetTextListener<TMessage>(IWampTextBinding<TMessage> binding)
        {
            TextConnectionListener<TMessage> listener = new TextConnectionListener<TMessage>(binding);

            RegisterBinding(binding, listener);

            return listener;
        }

        public IWampConnectionListener<TMessage> GetBinaryListener<TMessage>(IWampBinaryBinding<TMessage> binding)
        {
            BinaryConnectionListener<TMessage> listener = new BinaryConnectionListener<TMessage>(binding);

            RegisterBinding(binding, listener);

            return listener;
        }

        private void RegisterBinding(IWampBinding binding, ConnectionListener listener)
        {
            mBindings.Add(binding.Name, listener);
        }

        #region Nested classes

        private abstract class ConnectionListener : IDisposable
        {
            public abstract void OnNewConnection(IWebSocketConnection connection);
            public abstract void Dispose();
        }

        private abstract class ConnectionListener<TMessage> : ConnectionListener,
            IWampConnectionListener<TMessage>
        {
            private readonly Subject<IWampConnection<TMessage>> mSubject = 
                new Subject<IWampConnection<TMessage>>(); 

            protected void OnNewConnection(IWampConnection<TMessage> connection)
            {
                mSubject.OnNext(connection);
            }

            public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
            {
                return mSubject.Subscribe(observer);
            }

            public override void Dispose()
            {
                mSubject.OnCompleted();
                mSubject.Dispose();
            }
        }

        private class BinaryConnectionListener<TMessage> : ConnectionListener<TMessage>                                           
        {
            private readonly IWampBinaryBinding<TMessage> mBinding;

            public BinaryConnectionListener(IWampBinaryBinding<TMessage> binding)
            {
                mBinding = binding;
            }

            public IWampBinaryBinding<TMessage> Binding
            {
                get
                {
                    return mBinding;
                }
            }

            public override void OnNewConnection(IWebSocketConnection connection)
            {
                OnNewConnection(new FleckWampBinaryConnection<TMessage>(connection, Binding));
            }
        }

        private class TextConnectionListener<TMessage> : ConnectionListener<TMessage>                                           
        {
            private readonly IWampTextBinding<TMessage> mBinding;

            public TextConnectionListener(IWampTextBinding<TMessage> binding)
            {
                mBinding = binding;
            }

            public IWampTextBinding<TMessage> Binding
            {
                get
                {
                    return mBinding;
                }
            }

            public override void OnNewConnection(IWebSocketConnection connection)
            {
                OnNewConnection(new FleckWampTextConnection<TMessage>(connection, Binding));
            }
        }

        #endregion
    }
}