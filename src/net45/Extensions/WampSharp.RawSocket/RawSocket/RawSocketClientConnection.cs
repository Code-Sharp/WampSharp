using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using SystemEx;
using Microsoft.IO;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Contracts;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.RawSocket
{
    public class RawSocketClientConnection<TMessage> : IControlledWampConnection<TMessage>,
        IAsyncDisposable
    {
        private readonly Func<TcpClient> mClientBuilder;
        private TcpClientConnection<TMessage> mConnection;
        private readonly Func<TcpClient, Task> mConnector;
        private readonly IWampStreamingMessageParser<TMessage> mParser;
        private readonly Handshaker mHandshaker = new Handshaker();
        private byte mMaxLength = 15;
        private readonly RecyclableMemoryStreamManager mByteArrayPool = new RecyclableMemoryStreamManager();
        private TcpClient mClient;
        private readonly TimeSpan? mAutoPingInterval;
        private readonly ClientSslConfiguration mSslConfiguration;

        public RawSocketClientConnection
            (Func<TcpClient> clientBuilder,
             Func<TcpClient, Task> connector,
             IWampStreamingMessageParser<TMessage> parser, 
             TimeSpan? autoPingInterval,
             ClientSslConfiguration sslConfiguration = null)
        {
            mClientBuilder = clientBuilder;
            mConnector = connector;

            if (!(parser is JsonBinding<TMessage> || parser is MsgPackBinding<TMessage>))
            {
                throw new ArgumentException("Expected Json or MsgPack binding", nameof(parser));
            }

            mParser = parser;
            mAutoPingInterval = autoPingInterval;
            mSslConfiguration = sslConfiguration;
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

                Stream stream = mClient.GetStream();

                if (IsSecure)
                {
                    stream = await GetSecureStream(stream)
                        .ConfigureAwait(false);
                }

                Handshake handshakeRequest = GetHandshakeRequest();

                await mHandshaker.SendHandshake(stream, handshakeRequest).ConfigureAwait(false);

                Handshake handshakeResponse = await mHandshaker.GetHandshakeMessage(stream).ConfigureAwait(false);

                if (handshakeResponse.IsError)
                {
                    OnConnectionError(
                        new WampConnectionErrorEventArgs(new RawSocketProtocolException(handshakeResponse.HandshakeError.Value)));
                }
                else
                {
                    Connection =
                        CreateInnerConnection(stream, handshakeRequest, handshakeResponse);

                    Task.Run(Connection.HandleTcpClientAsync);
                }
            }
            catch (Exception ex)
            {
                OnConnectionError(new WampConnectionErrorEventArgs(ex));
            }
        }

        private async Task<Stream> GetSecureStream(Stream stream)
        {
            SslStream sslStream = new SslStream(stream);

            await sslStream.AuthenticateAsClientAsync(mSslConfiguration)
                .ConfigureAwait(false);

            stream = sslStream;
            return stream;
        }

        private bool IsSecure => mSslConfiguration != null;

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
            get => mMaxLength;
            set
            {
                if (value >= 16)
                {
                    throw new ArgumentException("Expected a value between 0 to 15", nameof(value));
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
            set => mConnection = value;
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
            (Stream stream, Handshake handshakeRequest, Handshake handshakeResponse)
        {
            TcpClientConnection<TMessage> connection =
                new TcpClientConnection<TMessage>
                   (mClient,
                    stream,
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
            ConnectionOpen?.Invoke(this, EventArgs.Empty);
        }

        protected void RaiseMessageArrived(WampMessageArrivedEventArgs<TMessage> e)
        {
            MessageArrived?.Invoke(this, e);
        }

        protected void RaiseConnectionClosed()
        {
            ConnectionClosed?.Invoke(this, EventArgs.Empty);
        }

        protected void OnConnectionError(WampConnectionErrorEventArgs e)
        {
            mClient.Close();
            RaiseConnectionErrorEvent(e);
            RaiseConnectionClosed();
        }

        private void RaiseConnectionErrorEvent(WampConnectionErrorEventArgs e)
        {
            ConnectionError?.Invoke(this, e);
        }

        public ValueTask DisposeAsync()
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