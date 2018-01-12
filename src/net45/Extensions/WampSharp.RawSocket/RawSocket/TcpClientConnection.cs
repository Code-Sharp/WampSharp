using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding.Parsers;
using WampSharp.V2.Transports;

namespace WampSharp.RawSocket
{
    internal class TcpClientConnection<TMessage> : AsyncWampConnection<TMessage> 
    {
        private const string Tag = "WampSharp.RawSocket";
        private const int FrameHeaderSize = 4;
        private readonly IWampStreamingMessageParser<TMessage> mBinding;
        private readonly RawSocketFrameHeaderParser mFrameHeaderParser = new RawSocketFrameHeaderParser();
        private readonly TcpClient mTcpClient;
        private readonly Stream mStream;
        private readonly long mMaxAllowedMessageSize;
        private readonly Handshake mHandshake;
        private readonly ArrayPool<byte> mByteArrayPool;
        private readonly SslConfiguration mSslConfiguration;
        private readonly PingPongHandler mPingPongHandler;
        private readonly Pinger mPinger;

        public TcpClientConnection
        (TcpClient client, Stream stream, long maxAllowedMessageSize, Handshake handshake, IWampStreamingMessageParser<TMessage> binding, ArrayPool<byte> byteArrayPool, TimeSpan? autoPingInterval, SslConfiguration sslConfiguration = null)
        {
            mTcpClient = client;
            mStream = stream;
            mMaxAllowedMessageSize = maxAllowedMessageSize;
            mHandshake = handshake;
            mBinding = binding;
            mByteArrayPool = byteArrayPool;
            mSslConfiguration = sslConfiguration;

            mPinger = new Pinger(this);

            mPingPongHandler =
                new PingPongHandler(mLogger,
                                    mPinger,
                                    autoPingInterval);
        }

        protected override bool IsConnected
        {
            get
            {
                return TcpClient.Connected;
            }
        }

        protected async override Task SendAsync(WampMessage<object> message)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Position = FrameHeaderSize;

                // Write the message to the memory stream
                mBinding.Format(message, memoryStream);

                int totalMessageLength = (int) memoryStream.Length;

                // Compute the written message length
                int messageLength = totalMessageLength - FrameHeaderSize;

                byte[] buffer;

#if NETCORE
                ArraySegment<byte> arraySegment;

                memoryStream.TryGetBuffer(out arraySegment);

                buffer = arraySegment.Array;
#else
                buffer = memoryStream.GetBuffer();
#endif

                // Write a message header
                mFrameHeaderParser.WriteHeader(FrameType.WampMessage, messageLength, buffer);

                // Write the whole message to the wire
                await Stream.WriteAsync(buffer, 0, totalMessageLength);

                mByteArrayPool.Return(buffer);
            }
        }

        private Stream Stream
        {
            get
            {
                return mStream;
            }
        }

        private TcpClient TcpClient
        {
            get
            {
                return mTcpClient;
            }
        }

        protected override void Dispose()
        {
            TcpClient.Close();
        }

        public async Task HandleTcpClientAsync()
        {
            try
            {
                RaiseConnectionOpen();

                mPingPongHandler.Start();

                byte[] frameHeaderBytes = new byte[FrameHeaderSize];

                while (IsConnected)
                {
                    await Stream
                        .ReadExactAsync(frameHeaderBytes)
                        .ConfigureAwait(false);

                    int messageLength;
                    FrameType frameType;

                    if (mFrameHeaderParser.TryParse(frameHeaderBytes, out frameType, out messageLength) &&
                        (messageLength <= mMaxAllowedMessageSize))
                    {
                        await HandleFrame(frameType, messageLength);
                    }
                    else
                    {
                        mTcpClient.Close();
                    }
                }

                RaiseConnectionClosed();
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);
            }
        }

        private async Task HandleFrame(FrameType frameType, int messageLength)
        {
            switch (frameType)
            {
                case FrameType.WampMessage:
                    await HandleWampMessage(messageLength);
                    break;
                case FrameType.Ping:
                    await HandlePing(messageLength);
                    break;
                case FrameType.Pong:
                    await HandlePong(messageLength);
                    break;
            }
        }

        private async Task HandleWampMessage(int messageLength)
        {
            byte[] buffer = await ReadStream(messageLength);

            WampMessage<TMessage> parsed = mBinding.Parse(new MemoryStream(buffer));

            mByteArrayPool.Return(buffer);

            RaiseMessageArrived(parsed);
        }

        private async Task<byte[]> ReadStream(int messageLength, int position = 0)
        {
            int length = position + messageLength;

            byte[] array = mByteArrayPool.Rent(length);

            await Stream.ReadExactAsync(array, position, messageLength);

            return array;
        }

        private async Task HandlePong(int messageLength)
        {
            byte[] buffer = await ReadStream(messageLength);

            ArraySegment<byte> arraySegment =
                new ArraySegment<byte>(buffer, 0, messageLength);

            mPinger.RaiseOnPong(arraySegment);

            mByteArrayPool.Return(buffer);
        }

        private async Task HandlePing(int messageLength)
        {
            byte[] buffer = await ReadStream(messageLength, FrameHeaderSize);

            mFrameHeaderParser.WriteHeader(FrameType.Pong, messageLength, buffer);

            Stream networkStream = Stream;

            int frameSize = messageLength + FrameHeaderSize;

            await networkStream.WriteAsync(buffer, 0, frameSize);

            mByteArrayPool.Return(buffer);
        }

        private async Task SendPing(byte[] message)
        {
            int frameSize = message.Length + FrameHeaderSize;

            byte[] buffer = mByteArrayPool.Rent(frameSize);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                mFrameHeaderParser.WriteHeader(FrameType.Ping, message.Length, buffer);
                memoryStream.SetLength(FrameHeaderSize);
                memoryStream.Position = FrameHeaderSize;

                await memoryStream.WriteAsync(message, 0, message.Length);

                await Stream.WriteAsync(buffer, 0, frameSize);
            }

            mByteArrayPool.Return(buffer);
        }

        internal class Pinger : IPinger
        {
            private readonly TcpClientConnection<TMessage> mParent;

            public Pinger(TcpClientConnection<TMessage> parent)
            {
                mParent = parent;
            }

            public Task SendPing(byte[] message)
            {
                return mParent.SendPing(message);
            }

            public event Action<IList<byte>> OnPong;


            public bool IsConnected
            {
                get
                {
                    return mParent.IsConnected;
                }
            }

            public void Disconnect()
            {
                mParent.Dispose();
            }

            public void RaiseOnPong(IList<byte> bytes)
            {
                Action<IList<byte>> handler = OnPong;

                if (handler != null)
                {
                    handler(bytes);
                }
            }
        }
    }
}