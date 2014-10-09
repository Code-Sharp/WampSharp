using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using vtortola.WebSockets.Deflate;
using vtortola.WebSockets.Rfc6455;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.Vtortola
{
    /// <summary>
    /// Represents a WebSocket transport implemented with Vtortola.
    /// </summary>
    /// TODO: This was copied and modified from Fleck implementation.
    /// TODO: Refactor these classes in order to avoid code duplication.
    public class VtortolaWebSocketTransport : IWampTransport
    {
        private readonly IPEndPoint mEndpoint;
        private WebSocketListener mListener;

        private readonly IDictionary<string, ConnectionListener> mBindings =
            new Dictionary<string, ConnectionListener>();

        private readonly bool mPerMessageDeflate;

        /// <summary>
        /// Creates a new instance of <see cref="VtortolaWebSocketTransport"/>
        /// given the endpoint to run at.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="perMessageDeflate">A value indicating whether to support permessage-deflate
        /// compression extension or not.</param>
        public VtortolaWebSocketTransport(IPEndPoint endpoint, bool perMessageDeflate)
        {
            mEndpoint = endpoint;
            mPerMessageDeflate = perMessageDeflate;
        }

        public void Dispose()
        {
            foreach (ConnectionListener connectionListener in mBindings.Values)
            {
                connectionListener.Dispose();
            }

            mListener.Stop();
            mListener.Dispose();
        }

        public void Open()
        {
            string[] protocols = mBindings.Keys.ToArray();

            WebSocketListener listener = new WebSocketListener(mEndpoint, new WebSocketListenerOptions()
            {
                SubProtocols = protocols
            });

            WebSocketFactoryRfc6455 factory = new WebSocketFactoryRfc6455(listener);

            if (mPerMessageDeflate)
            {
                factory.MessageExtensions.RegisterExtension(new WebSocketDeflateExtension());                
            }

            listener.Standards.RegisterStandard(factory);

            listener.Start();

            mListener = listener;

            Task.Run((Func<Task>)ListenAsync);            
        }

        private async Task ListenAsync()
        {
            while (mListener.IsStarted)
            {
                WebSocket websocket = await mListener.AcceptWebSocketAsync(CancellationToken.None)
                    .ConfigureAwait(false);

                if (websocket != null)
                {
                    OnNewConnection(websocket);
                }
            }
        }

        private void OnNewConnection(WebSocket connection)
        {
            string protocol = connection.HttpResponse.WebSocketProtocol;

            ConnectionListener listener = mBindings[protocol];

            listener.OnNewConnection(connection);
        }

        public IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinding<TMessage> binding)
        {
            IWampTextBinding<TMessage> textBinding = binding as IWampTextBinding<TMessage>;

            if (textBinding != null)
            {
                return GetListener(textBinding);
            }

            IWampBinaryBinding<TMessage> binaryBinding = binding as IWampBinaryBinding<TMessage>;

            if (binaryBinding != null)
            {
                return GetListener(binaryBinding);
            }

            throw new ArgumentException("WebSockets can only deal with binary/text transports", "binding");
        }

        private IWampConnectionListener<TMessage> GetListener<TMessage>(IWampTextBinding<TMessage> binding)
        {
            TextConnectionListener<TMessage> listener = new TextConnectionListener<TMessage>(binding);

            RegisterBinding(binding, listener);

            return listener;
        }

        private IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinaryBinding<TMessage> binding)
        {
            BinaryConnectionListener<TMessage> listener = new BinaryConnectionListener<TMessage>(binding);

            RegisterBinding(binding, listener);

            return listener;
        }

        private void RegisterBinding(IWampBinding binding, ConnectionListener listener)
        {
            if (mBindings.ContainsKey(binding.Name))
            {
                throw new ArgumentException("Already registered a binding for protocol: " +
                                            binding.Name,
                                            "binding");
            }

            mBindings.Add(binding.Name, listener);
        }

        #region Nested classes

        private abstract class ConnectionListener : IDisposable
        {
            public abstract void OnNewConnection(WebSocket connection);
            public abstract void Dispose();
        }

        private abstract class ConnectionListener<TMessage> : ConnectionListener,
            IWampConnectionListener<TMessage>
        {
            private readonly Subject<IWampConnection<TMessage>> mSubject =
                new Subject<IWampConnection<TMessage>>();

            protected void OnNewConnection(VtortolaWampConnection<TMessage> connection)
            {
                mSubject.OnNext(connection);
                Task task = connection.HandleWebSocketAsync();
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

            public override void OnNewConnection(WebSocket connection)
            {
                OnNewConnection(new VtortolaWampBinaryConnection<TMessage>(connection, Binding));
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

            public override void OnNewConnection(WebSocket connection)
            {
                OnNewConnection(new VtortolaWampTextConnection<TMessage>(connection, Binding));
            }
        }

        #endregion
    }
}