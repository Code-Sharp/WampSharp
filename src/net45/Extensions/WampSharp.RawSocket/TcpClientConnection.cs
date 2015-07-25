using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.IO;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.RawSocket
{
    internal class TcpClientConnection<TMessage> : AsyncWampConnection<TMessage> 
    {
        private const string Tag = "WampSharp.RawSocket";
        private const int FrameHeaderSize = 4;
        private readonly IWampStreamingMessageParser<TMessage> mBinding;
        private readonly RawSocketFrameHeaderParser mFrameHeaderParser = new RawSocketFrameHeaderParser();
        private readonly TcpClient mTcpClient;
        private readonly long mMaxAllowedMessageSize;
        private readonly Handshake mHandshake;
        private readonly RecyclableMemoryStreamManager mMemoryStreamManager;

        public TcpClientConnection(TcpClient client,
                                   long maxAllowedMessageSize,
                                   Handshake handshake,
                                   IWampStreamingMessageParser<TMessage> binding,
                                   RecyclableMemoryStreamManager memoryStreamManager)
        {
            mTcpClient = client;
            mMaxAllowedMessageSize = maxAllowedMessageSize;
            mHandshake = handshake;
            mBinding = binding;
            mMemoryStreamManager = memoryStreamManager;
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
            using (MemoryStream memoryStream = mMemoryStreamManager.GetStream())
            {
                memoryStream.Position = FrameHeaderSize;

                // Write the message to the memory stream
                mBinding.Format(message, memoryStream);

                int totalMessageLength = (int) memoryStream.Length;

                // Compute the written message length
                int messageLength = totalMessageLength - FrameHeaderSize;

                byte[] buffer = memoryStream.GetBuffer();

                // Write a message header
                mFrameHeaderParser.WriteHeader(FrameType.WampMessage, messageLength, buffer);

                // Write the whole message to the wire
                await TcpClient.GetStream().WriteAsync(buffer, 0, totalMessageLength);
            }
        }

        private TcpClient TcpClient
        {
            get
            {
                return mTcpClient;
            }
        }

        public override void Dispose()
        {
            TcpClient.Close();
        }

        public async Task HandleTcpClientAsync()
        {
            try
            {
                RaiseConnectionOpen();

                byte[] frameHeaderBytes = new byte[FrameHeaderSize];

                while (IsConnected)
                {
                    await TcpClient.GetStream()
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
            using (MemoryStream memoryStream = await ReadStream(messageLength))
            {
                WampMessage<TMessage> parsed = mBinding.Parse(memoryStream);

                RaiseMessageArrived(parsed);
            }
        }

        private async Task<MemoryStream> ReadStream(int messageLength, int position = 0)
        {
            MemoryStream stream = 
                mMemoryStreamManager.GetStream(Tag, position + messageLength, true);

            await TcpClient.GetStream().ReadExactAsync(stream.GetBuffer(), position, messageLength);

            stream.Position = 0;
            stream.SetLength(position + messageLength);

            return stream;
        }

        private async Task HandlePong(int messageLength)
        {
        }

        private async Task HandlePing(int messageLength)
        {
            using (MemoryStream memoryStream = await ReadStream(messageLength, FrameHeaderSize))
            {
                byte[] buffer = memoryStream.GetBuffer();

                mFrameHeaderParser.WriteHeader(FrameType.Pong, messageLength, buffer);

                NetworkStream networkStream = mTcpClient.GetStream();

                await networkStream.WriteAsync(buffer, 0, (int) memoryStream.Length);
            }
        }
    }
}