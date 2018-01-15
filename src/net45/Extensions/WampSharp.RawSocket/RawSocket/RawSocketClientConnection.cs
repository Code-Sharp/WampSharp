using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SystemEx;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Contracts;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.RawSocket
{
    public class RawSocketClientConnection<TMessage> : IControlledWampConnection<TMessage>
    {
        private readonly Func<TcpClient> mClientBuilder;
        private TcpClientConnection<TMessage> mConnection;
        private readonly Func<TcpClient, Task> mConnector;
        private readonly IWampStreamingMessageParser<TMessage> mParser;
        private readonly Handshaker mHandshaker = new Handshaker();
        private byte mMaxLength = 15;
        private readonly ArrayPool<byte> mByteArrayPool = ArrayPool<byte>.Create();
        private TcpClient mClient;
        private readonly TimeSpan? mAutoPingInterval;

        public RawSocketClientConnection
            (Func<TcpClient> clientBuilder,
             Func<TcpClient, Task> connector,
             IWampStreamingMessageParser<TMessage> parser, 
             TimeSpan? autoPingInterval)
        {
            mClientBuilder = clientBuilder;
            mConnector = connector;

            if (!(parser is JsonBinding<TMessage> || parser is MsgPackBinding<TMessage>))
            {
                throw new ArgumentException("Expected Json or MsgPack binding", "parser");
            }

            mParser = parser;
            mAutoPingInterval = autoPingInterval;
        }

        public void Send(WampMessage<object> message)
        {
            Connection.Send(message);
        }

        public event EventHandler ConnectionOpen;

        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
         
        public event EventHandler ConnectionClosed;

        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

        private void OnConnectionOpen(object sender, EventArgs e)
        {
            RaiseConnectionOpen();
        }

        private void OnMessageArrived(object sender, WampMessageArrivedEventArgs<TMessage> e)
        {
            RaiseMessageArrived(e);
        }

        private void OnConnectionError(object sender, WampConnectionErrorEventArgs wampConnectionErrorEventArgs)
        {
            CleanupConnection();
            OnConnectionError(wampConnectionErrorEventArgs);
        }

        private void OnConnectionClosed(object sender, EventArgs eventArgs)
        {
            CleanupConnection();
            RaiseConnectionClosed();
        }

        public async void Connect()
        {
            if (mConnection == null)
            {
                await InnerConnect().ConfigureAwait(false);
            }
        }

        private async Task InnerConnect()
        {
            try
            {
                mClient = mClientBuilder();

                await mConnector(mClient).ConfigureAwait(false);

                Handshake handshakeRequest = GetHandshakeRequest();

                await mHandshaker.SendHandshake(mClient, handshakeRequest).ConfigureAwait(false);

                Handshake handshakeResponse = await mHandshaker.GetHandshakeMessage(mClient).ConfigureAwait(false);

                if (handshakeResponse.IsError)
                {
                    OnConnectionError(
                        new WampConnectionErrorEventArgs(new RawSocketProtocolException(handshakeResponse.HandshakeError.Value)));
                }
                else
                {
                    Connection =
                        CreateInnerConnection(handshakeRequest, handshakeResponse);

                    Task.Run((Func<Task>) Connection.HandleTcpClientAsync);
                }
            }
            catch (Exception ex)
            {
                OnConnectionError(new WampConnectionErrorEventArgs(ex));
            }
        }

        private Handshake GetHandshakeRequest()
        {
            SerializerType serializerType = GetSerializerType();
            return new Handshake(MaxLength, serializerType);
        }

        /// <summary>
        /// Gets or sets the requested max length of a message to be received.
        /// 0 => 2^9 bytes
        /// 1 => 2^10 bytes
        /// ...
        /// 15 => 2^24 bytes
        /// </summary>
        public byte MaxLength
        {
            get
            {
                return mMaxLength;
            }
            set
            {
                if (value >= 16)
                {
                    throw new ArgumentException("Expected a value between 0 to 15", "value");
                }

                mMaxLength = value;
            }
        }

        private TcpClientConnection<TMessage> Connection
        {
            get
            {
                if (mConnection == null)
                {
                    throw new Exception("Connection wasn't opened yet");
                }

                return mConnection;
            }
            set
            {
                mConnection = value;
            }
        }

        private SerializerType GetSerializerType()
        {
            IWampBinding binding = mParser as IWampBinding;

            switch (binding.Name)
            {
                case WampSubProtocols.JsonSubProtocol:
                    return SerializerType.Json;
                case WampSubProtocols.MsgPackSubProtocol:
                    return SerializerType.MsgPack;
            }

            return SerializerType.Illegal;
        }

        private TcpClientConnection<TMessage> CreateInnerConnection
            (Handshake handshakeRequest,
            Handshake handshakeResponse)
        {
            TcpClientConnection<TMessage> connection =
                new TcpClientConnection<TMessage>
                    (mClient,
                     handshakeRequest.MaxMessageSizeInBytes,
                     handshakeResponse,
                     mParser,
                     mByteArrayPool,
                     mAutoPingInterval);


            connection.ConnectionOpen += OnConnectionOpen;
            connection.MessageArrived += OnMessageArrived;
            connection.ConnectionClosed += OnConnectionClosed;
            connection.ConnectionError += OnConnectionError;

            return connection;
        }

        private void CleanupConnection()
        {
            mConnection.ConnectionOpen -= OnConnectionOpen;
            mConnection.MessageArrived -= OnMessageArrived;
            mConnection.ConnectionClosed -= OnConnectionClosed;
            mConnection.ConnectionError -= OnConnectionError;
            mConnection = null;
        }

        protected void RaiseConnectionOpen()
        {
            EventHandler handler = ConnectionOpen;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected void RaiseMessageArrived(WampMessageArrivedEventArgs<TMessage> e)
        {
            EventHandler<WampMessageArrivedEventArgs<TMessage>> handler = MessageArrived;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void RaiseConnectionClosed()
        {
            EventHandler handler = ConnectionClosed;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected void OnConnectionError(WampConnectionErrorEventArgs e)
        {
            mClient.Close();
            RaiseConnectionErrorEvent(e);
            RaiseConnectionClosed();
        }

        private void RaiseConnectionErrorEvent(WampConnectionErrorEventArgs e)
        {
            EventHandler<WampConnectionErrorEventArgs> handler = ConnectionError;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public Task DisposeAsync()
        {
            IAsyncDisposable asyncDisposable = Connection;
            return asyncDisposable.DisposeAsync();
        }

        public void Dispose()
        {
            IDisposable connection = Connection;
            connection.Dispose();
        }
    }
}