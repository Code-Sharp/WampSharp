using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.IO;
using WampSharp.Core.Listener;
using WampSharp.Logging;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Transports;

namespace WampSharp.RawSocket
{
    /// <summary>
    /// A RawSocket <see cref="IWampTransport"/> implementation.
    /// </summary>
    public class RawSocketTransport : TextBinaryTransport<RawSocketTcpClient>
    {
        private readonly Handshaker mHandshaker = new Handshaker();
        private readonly TcpListener mListener;
        private bool mIsStarted = false;
        private readonly RecyclableMemoryStreamManager mByteArrayPool = new RecyclableMemoryStreamManager();
        private readonly TimeSpan? mAutoPingInterval;
        private readonly ServerSslConfiguration mSslConfiguration;

        /// <summary>
        /// Creates a new instance of <see cref="RawSocketTransport"/>.
        /// </summary>
        /// <param name="listener">The <see cref="TcpListener"/> to use.</param>
        /// <param name="sslConfiguration">The <see cref="SslConfiguration"/> to use for SSL security options.</param>
        /// <param name="autoPingInterval">The auto pings send interval.</param>
        /// <param name="maxSize">The max message size to receive.</param>
        public RawSocketTransport(TcpListener listener, ServerSslConfiguration sslConfiguration = null, TimeSpan? autoPingInterval = null, byte maxSize = 15)
        {
            mListener = listener;
            mSslConfiguration = sslConfiguration;
            mAutoPingInterval = autoPingInterval;

            if (maxSize >= 16)
            {
                throw new ArgumentException("Expected a number between 0 to 15", nameof(maxSize));
            }

            MaxSize = maxSize;
        }

        public override void Open()
        {
            mIsStarted = true;

            mListener.Start();

            Task.Run(ListenAsync);
        }

        private async Task ListenAsync()
        {
            while (mIsStarted)
            {
                try
                {
                    RawSocketTcpClient client = 
                        await AcceptClient().ConfigureAwait(false);

                    if (!client.HandshakeResponse.IsError)
                    {
                        OnNewConnection(client);
                    }
                }
                catch (Exception ex)
                {
                    mLogger.Error("An error occurred while trying to accept a client", ex);
                }
            }
        }

        private async Task<RawSocketTcpClient> AcceptClient()
        {
            TcpClient tcpClient = null;

            try
            {
                tcpClient = await mListener.AcceptTcpClientAsync().ConfigureAwait(false);

                Stream stream = tcpClient.GetStream();

                if (mSslConfiguration != null)
                {
                    SslStream sslStream = new SslStream(stream);

                    await sslStream.AuthenticateAsServerAsync(mSslConfiguration)
                        .ConfigureAwait(false);

                    stream = sslStream;
                }

                Handshake handshakeRequest = await mHandshaker.GetHandshakeMessage(stream).ConfigureAwait(false);

                Handshake handshakeResponse = handshakeRequest.GetHandshakeResponse(SubProtocols, MaxSize);

                await mHandshaker.SendHandshake(stream, handshakeResponse)
                                 .ConfigureAwait(false);

                return new RawSocketTcpClient(tcpClient, stream, handshakeRequest, handshakeResponse);
            }
            catch (Exception)
            {
                if ((tcpClient != null) && tcpClient.Connected)
                {
                    tcpClient.Close();
                }

                throw;
            }
        }

        public byte MaxSize { get; }

        protected override void OpenConnection<TMessage>(RawSocketTcpClient original, IWampConnection<TMessage> connection)
        {
            TcpClientConnection<TMessage> casted = connection as TcpClientConnection<TMessage>;
            Task.Run(casted.HandleTcpClientAsync);
        }

        protected override string GetSubProtocol(RawSocketTcpClient connection)
        {
            SerializerType serializerType = connection.HandshakeRequest.SerializerType;

            return serializerType.GetSubProtocol();
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
            (RawSocketTcpClient connection,
             IWampBinaryBinding<TMessage> binding)
        {
            return CreateConnection(connection, binding);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
            (RawSocketTcpClient connection,
             IWampTextBinding<TMessage> binding)
        {
            return CreateConnection(connection, binding);
        }

        private TcpClientConnection<TMessage> CreateConnection<TMessage, TRaw>(RawSocketTcpClient connection, IWampTransportBinding<TMessage, TRaw> binding)
        {
            if (binding.ComputeBytes == null)
            {
                binding.ComputeBytes = true;
            }

            return new TcpClientConnection<TMessage>
            (connection.Client,
                connection.Stream,
                connection.HandshakeResponse.MaxMessageSizeInBytes,
                connection.HandshakeRequest,
                binding,
                mByteArrayPool,
                mAutoPingInterval);
        }

        public override void Dispose()
        {
            mIsStarted = false;
            mListener.Stop();
        }
    }
}