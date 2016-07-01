using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.IO;
using WampSharp.Core.Listener;
using WampSharp.Logging;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Contracts;
using WampSharp.V2.Binding.Parsers;
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
        private readonly RecyclableMemoryStreamManager mRecyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        private readonly byte mMaxSize;
        private readonly TimeSpan? mAutoPingInterval;

        /// <summary>
        /// Creates a new instance of <see cref="RawSocketTransport"/>.
        /// </summary>
        /// <param name="listener">The <see cref="TcpListener"/> to use.</param>
        /// <param name="autoPingInterval">The auto pings send interval.</param>
        /// <param name="maxSize">The max message size to receive: </param>
        public RawSocketTransport(TcpListener listener, TimeSpan? autoPingInterval = null, byte maxSize = 15)
        {
            mListener = listener;
            mAutoPingInterval = autoPingInterval;

            if (maxSize >= 16)
            {
                throw new ArgumentException("Expected a number between 0 to 15", "maxSize");
            }

            mMaxSize = maxSize;
        }

        public override void Open()
        {
            mIsStarted = true;

            mListener.Start();

            Task.Run(new Func<Task>(ListenAsync));
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
                    mLogger.Error("An error occured while trying to accept a client", ex);
                }
            }
        }

        private async Task<RawSocketTcpClient> AcceptClient()
        {
            TcpClient tcpClient = null;
            try
            {
                tcpClient = await mListener.AcceptTcpClientAsync().ConfigureAwait(false);

                Handshake handshakeRequest = await mHandshaker.GetHandshakeMessage(tcpClient);

                Handshake handshakeResponse = GetHandshakeResponse(handshakeRequest);

                await mHandshaker.SendHandshake(tcpClient, handshakeResponse)
                                 .ConfigureAwait(false);

                return new RawSocketTcpClient(tcpClient, handshakeRequest, handshakeResponse);
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

        private Handshake GetHandshakeResponse(Handshake handshakeRequest)
        {
            Handshake handshakeResponse;

            if (handshakeRequest.ReservedOctects != 0)
            {
                handshakeResponse = new Handshake(HandshakeErrorCode.UseOfReservedBits);
            }
            else
            {
                SerializerType serializerType = handshakeRequest.SerializerType;

                string requestedSubprotocol = GetSubProtocol(serializerType);

                if (!SubProtocols.Contains(requestedSubprotocol))
                {
                    handshakeResponse = new Handshake(HandshakeErrorCode.SerializerUnsupported);
                }
                else
                {
                    handshakeResponse = new Handshake(MaxSize, serializerType);
                }
            }

            return handshakeResponse;
        }

        public byte MaxSize
        {
            get
            {
                return mMaxSize;
            }
        }

        protected override void OpenConnection<TMessage>(RawSocketTcpClient original, IWampConnection<TMessage> connection)
        {
            TcpClientConnection<TMessage> casted = connection as TcpClientConnection<TMessage>;
            Task.Run(new Func<Task>(casted.HandleTcpClientAsync));
        }

        protected override string GetSubProtocol(RawSocketTcpClient connection)
        {
            SerializerType serializerType = connection.HandshakeRequest.SerializerType;

            return GetSubProtocol(serializerType);
        }

        private static string GetSubProtocol(SerializerType serializerType)
        {
            switch (serializerType)
            {
                case SerializerType.Json:
                    return WampSubProtocols.JsonSubProtocol;
                case SerializerType.MsgPack:
                    return WampSubProtocols.MsgPackSubProtocol;
            }

            return serializerType.ToString();
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

        private TcpClientConnection<TMessage> CreateConnection<TMessage>(RawSocketTcpClient connection, IWampStreamingMessageParser<TMessage> binding)
        {
            return new TcpClientConnection<TMessage>
                (connection.Client,
                 connection.HandshakeResponse.MaxMessageSizeInBytes,
                 connection.HandshakeRequest,
                 binding,
                 mRecyclableMemoryStreamManager,
                 mAutoPingInterval);
        }

        public override void Dispose()
        {
            mIsStarted = false;
            mListener.Stop();
        }
    }
}