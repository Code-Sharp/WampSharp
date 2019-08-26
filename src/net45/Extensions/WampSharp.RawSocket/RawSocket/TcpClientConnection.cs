using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.IO;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding.Parsers;
using WampSharp.V2.Transports;
using static WampSharp.RawSocket.RawSocketFrameHeaderParser;

namespace WampSharp.RawSocket
{
    internal class TcpClientConnection<TMessage> : AsyncWampConnection<TMessage> 
    {
        private const string Tag = "WampSharp.RawSocket";
        private readonly IWampStreamingMessageParser<TMessage> mBinding;
        private readonly RawSocketFrameHeaderParser mFrameHeaderParser = new RawSocketFrameHeaderParser();
        private readonly long mMaxAllowedMessageSize;
        private readonly Handshake mHandshake;
        private readonly RecyclableMemoryStreamManager mByteArrayPool;
        private readonly PingPongHandler mPingPongHandler;
        private readonly Pinger mPinger;

        public TcpClientConnection
        (TcpClient client,
            Stream stream,
            long maxAllowedMessageSize,
            Handshake handshake,
            IWampStreamingMessageParser<TMessage> binding,
            RecyclableMemoryStreamManager byteArrayPool,
            TimeSpan? autoPingInterval)
        {
            TcpClient = client;
            Stream = stream;
            mMaxAllowedMessageSize = maxAllowedMessageSize;
            mHandshake = handshake;
            mBinding = binding;
            mByteArrayPool = byteArrayPool;

            mPinger = new Pinger(this);

            mPingPongHandler =
                new PingPongHandler(mLogger,
                    mPinger,
                    autoPingInterval);
        }

        protected override bool IsConnected => TcpClient.Connected;

        protected override async Task SendAsync(WampMessage<object> message)
        {
            using (MemoryStream memoryStream = mByteArrayPool.GetStream(Tag))
            {
                memoryStream.Position = FrameHeaderSize;

                // Write the message to the memory stream
                mBinding.Format(message, memoryStream);

                int totalMessageLength = (int) memoryStream.Length;

                // Compute the written message length
                int messageLength = totalMessageLength - FrameHeaderSize;

                byte[] buffer;

                buffer = memoryStream.GetBufferWorkaround();

                // Write a message header
                mFrameHeaderParser.WriteHeader(FrameType.WampMessage, messageLength, buffer);

                // Write the whole message to the wire
                await Stream.WriteAsync(buffer, 0, totalMessageLength).ConfigureAwait(false);
            }
        }

        private Stream Stream { get; }

        private TcpClient TcpClient { get; }

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

                    if (mFrameHeaderParser.TryParse(frameHeaderBytes, out FrameType frameType, out int messageLength) &&
                        (messageLength <= mMaxAllowedMessageSize))
                    {
                        await HandleFrame(frameType, messageLength).ConfigureAwait(false);
                    }
                    else
                    {
                        TcpClient.Close();
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
                    await HandleWampMessage(messageLength).ConfigureAwait(false);
                    break;
                case FrameType.Ping:
                    await HandlePing(messageLength).ConfigureAwait(false);
                    break;
                case FrameType.Pong:
                    await HandlePong(messageLength).ConfigureAwait(false);
                    break;
            }
        }

        private async Task HandleWampMessage(int messageLength)
        {
            using (MemoryStream stream = await ReadStream(messageLength).ConfigureAwait(false))
            {
                WampMessage<TMessage> parsed = mBinding.Parse(stream);

                RaiseMessageArrived(parsed);
            }
        }

        private async Task<MemoryStream> ReadStream(int messageLength, int position = 0)
        {
            int length = position + messageLength;

            MemoryStream stream = mByteArrayPool.GetStream(Tag, length, true);

            byte[] buffer = stream.GetBufferWorkaround();

            await Stream
                .ReadExactAsync(buffer, position, messageLength)
                .ConfigureAwait(false);

            stream.SetLength(length);

            stream.Position = 0;

            return stream;
        }

        private async Task HandlePong(int messageLength)
        {
            using (MemoryStream buffer = await ReadStream(messageLength).ConfigureAwait(false))
            {
                ArraySegment<byte> arraySegment =
                    new ArraySegment<byte>(buffer.GetBufferWorkaround(), 0, messageLength);

                mPinger.RaiseOnPong(arraySegment);
            }
        }

        private async Task HandlePing(int messageLength)
        {
            using (MemoryStream memoryStream = await ReadStream(messageLength, FrameHeaderSize).ConfigureAwait(false))
            {
                byte[] buffer = memoryStream.GetBufferWorkaround();

                mFrameHeaderParser.WriteHeader(FrameType.Pong, messageLength, buffer);

                Stream networkStream = Stream;

                int frameSize = messageLength + FrameHeaderSize;

                await networkStream.WriteAsync(buffer, 0, frameSize).ConfigureAwait(false);
            }
        }

        private async Task SendPing(byte[] message)
        {
            int frameSize = message.Length + FrameHeaderSize;

            using (MemoryStream memoryStream = mByteArrayPool.GetStream(Tag, frameSize, true))
            {
                byte[] buffer = memoryStream.GetBufferWorkaround();

                mFrameHeaderParser.WriteHeader(FrameType.Ping, message.Length, buffer);
                memoryStream.SetLength(FrameHeaderSize);
                memoryStream.Position = FrameHeaderSize;

                await memoryStream.WriteAsync(message, 0, message.Length).ConfigureAwait(false);

                await Stream.WriteAsync(buffer, 0, frameSize).ConfigureAwait(false);
            }
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


            public bool IsConnected => mParent.IsConnected;

            public void Disconnect()
            {
                mParent.Dispose();
            }

            public void RaiseOnPong(IList<byte> bytes)
            {
                OnPong?.Invoke(bytes);
            }
        }
    }
}