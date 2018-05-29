using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Logging;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.V2.Transports
{
    /// <summary>
    /// Represents a transport that supports text/binary payloads.
    /// </summary>
    public abstract class TextBinaryTransport<TConnection> : IWampTransport
    {
        protected ILog mLogger;

        private readonly IDictionary<string, ConnectionListener> mBindings =
            new Dictionary<string, ConnectionListener>();

        public TextBinaryTransport()
        {
            mLogger = LogProvider.GetLogger(this.GetType());
        }

        /// <summary>
        /// Gets the subprotocols registered within this transport.
        /// </summary>
        protected string[] SubProtocols => mBindings.Keys.ToArray();

        /// <summary>
        /// Call this when a new connection is established.
        /// </summary>
        /// <param name="connection">The new connection.</param>
        protected void OnNewConnection(TConnection connection)
        {
            string protocol = GetSubProtocol(connection);


            if (mBindings.TryGetValue(protocol, out ConnectionListener listener))
            {
                listener.OnNewConnection(connection);
            }
            else
            {
                mLogger.ErrorFormat("No handler registered for protocol '{Protocol}'",
                                    protocol);
            }
        }

        void IDisposable.Dispose()
        {
            foreach (ConnectionListener connectionListener in mBindings.Values)
            {
                connectionListener.Dispose();
            }

            this.Dispose();
        }

        #region Abstract Methods

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Occurs after a WampConnection has been created.
        /// </summary>
        /// <remarks>Override this in order to open your connection.</remarks>
        protected abstract void OpenConnection<TMessage>
            (TConnection original, IWampConnection<TMessage> connection);

        /// <summary>
        /// Gets the sub-protocol associated with the given connection.
        /// </summary>
        /// <param name="connection">The given connection.</param>
        /// <returns>The sub-protocol associated with the given connection</returns>
        protected abstract string GetSubProtocol(TConnection connection);

        /// <summary>
        /// Creates a binary <see cref="IWampConnection{TMessage}"/> using the given 
        /// binding and the given connection.
        /// </summary>
        /// <param name="connection">The given connection.</param>
        /// <param name="binding">The given binding.</param>
        protected abstract IWampConnection<TMessage> CreateBinaryConnection<TMessage>
            (TConnection connection, IWampBinaryBinding<TMessage> binding);

        /// <summary>
        /// Creates a text <see cref="IWampConnection{TMessage}"/> using the given 
        /// binding and the given connection.
        /// </summary>
        /// <param name="connection">The given connection.</param>
        /// <param name="binding">The given binding.</param>
        protected abstract IWampConnection<TMessage> CreateTextConnection<TMessage>
            (TConnection connection, IWampTextBinding<TMessage> binding);

        public abstract void Open();

        #endregion

        #region GetListener Methods

        public IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinding<TMessage> binding)
        {
            switch (binding)
            {
                case IWampTextBinding<TMessage> textBinding:
                    return GetListener(textBinding);
                case IWampBinaryBinding<TMessage> binaryBinding:
                    return GetListener(binaryBinding);
            }

            throw new ArgumentException("WebSockets can only deal with binary/text transports", nameof(binding));
        }

        private IWampConnectionListener<TMessage> GetListener<TMessage>(IWampTextBinding<TMessage> binding)
        {
            TextConnectionListener<TMessage> listener = new TextConnectionListener<TMessage>(binding, this);

            RegisterBinding(binding, listener);

            return listener;
        }

        private IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinaryBinding<TMessage> binding)
        {
            BinaryConnectionListener<TMessage> listener = new BinaryConnectionListener<TMessage>(binding, this);

            RegisterBinding(binding, listener);

            return listener;
        }

        private void RegisterBinding(IWampBinding binding, ConnectionListener listener)
        {
            if (mBindings.ContainsKey(binding.Name))
            {
                throw new ArgumentException("Already registered a binding for protocol: " +
                                            binding.Name,
                                            nameof(binding));
            }

            mBindings.Add(binding.Name, listener);
        }

        #endregion

        #region Nested classes

        protected abstract class ConnectionListener : IDisposable
        {
            public abstract void OnNewConnection(TConnection connection);
            public abstract void Dispose();
        }

        private abstract class ConnectionListener<TMessage> : ConnectionListener,
            IWampConnectionListener<TMessage>
        {
            private readonly Subject<IWampConnection<TMessage>> mSubject =
                new Subject<IWampConnection<TMessage>>();

            protected ConnectionListener(TextBinaryTransport<TConnection> parent)
            {
                Parent = parent;
            }

            public TextBinaryTransport<TConnection> Parent { get; }

            protected void OnNewConnection(IWampConnection<TMessage> connection, TConnection original)
            {
                mSubject.OnNext(connection);
                Parent.OpenConnection(original, connection);
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
            public BinaryConnectionListener
                (IWampBinaryBinding<TMessage> binding,
                 TextBinaryTransport<TConnection> parent)
                : base(parent)
            {
                Binding = binding;
            }

            public IWampBinaryBinding<TMessage> Binding { get; }

            public override void OnNewConnection(TConnection connection)
            {
                OnNewConnection(Parent.CreateBinaryConnection(connection, Binding), connection);
            }
        }

        private class TextConnectionListener<TMessage> : ConnectionListener<TMessage>
        {
            public TextConnectionListener
                (IWampTextBinding<TMessage> binding,
                 TextBinaryTransport<TConnection> parent) :
                     base(parent)
            {
                Binding = binding;
            }

            public IWampTextBinding<TMessage> Binding { get; }

            public override void OnNewConnection(TConnection connection)
            {
                OnNewConnection(Parent.CreateTextConnection(connection, Binding), connection);
            }
        }

        #endregion
    }
}