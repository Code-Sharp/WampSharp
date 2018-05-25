using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.RawSocket;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.AspNetCore.RawSocket
{
    public class RawSocketConnection<TMessage> : AsyncWampConnection<TMessage>
    {
        private readonly PipeReader mReader;
        private readonly ConnectionContext mConnection;
        protected const int FrameHeaderSize = 4;
        protected readonly RawSocketFrameHeaderParser mFrameHeaderParser = new RawSocketFrameHeaderParser();
        private readonly int mMaxAllowedMessageSize;
        private IWampStreamingMessageParser<TMessage> mParser;

        public RawSocketConnection(SocketData connection, PipeReader reader, IWampStreamingMessageParser<TMessage> parser)
        {
            mReader = reader;
            mParser = parser;
            mConnection = connection.ConnectionContext;
            mMaxAllowedMessageSize = connection.Handshake.MaxMessageSizeInBytes;
        }

        protected override async Task SendAsync(WampMessage<object> message)
        {
            ReadOnlyMemory<byte> bytes = GetBytes(message);
            
            await mConnection.Transport.Output.WriteAsync(bytes).ConfigureAwait(false);
        }

        public async Task RunAsync()
        {
            while (true)
            {
                ReadResult readResult =
                    await mReader.ReadAsync().ConfigureAwait(false);

                ReadOnlySequence<byte> headerBytes =
                    readResult.Buffer.Slice(0, FrameHeaderSize);

                FrameType frameType;
                int messageLength;

                if (mFrameHeaderParser.TryParse(headerBytes, out frameType, out messageLength) &&
                    (messageLength <= mMaxAllowedMessageSize))
                {
                    ReadOnlySequence<byte> frameContent = 
                        readResult.Buffer.Slice(FrameHeaderSize, messageLength);

                    HandleFrame(frameType, frameContent);
                }
                else
                {

                }
            }
        }

        private void HandleFrame(FrameType frameType, in ReadOnlySequence<byte> message)
        {
            switch (frameType)
            {
                case FrameType.WampMessage:
                    HandleWampFrame(message);
                    break;
            }
        }

        private void HandleWampFrame(in ReadOnlySequence<byte> message)
        {
            WampMessage<TMessage> parsed = ParseMessage(message);
            RaiseMessageArrived(parsed);
        }


        protected WampMessage<TMessage> ParseMessage(ReadOnlySequence<byte> messageInBytes)
        {
            byte[] buffer = messageInBytes.ToArray();
            MemoryStream memoryStream = new MemoryStream(buffer);
            return mParser.Parse(memoryStream);
        }

        protected ReadOnlyMemory<byte> GetBytes(WampMessage<object> message)
        {
            int headerSize = FrameHeaderSize;

            MemoryStream memoryStream = new MemoryStream(headerSize);
            memoryStream.Position = headerSize;
            mParser.Format(message, memoryStream);

            byte[] buffer = memoryStream.GetBuffer();

            mFrameHeaderParser.WriteHeader(FrameType.WampMessage,
                (int)memoryStream.Length - headerSize,
                buffer);

            return new ReadOnlyMemory<byte>(memoryStream.ToArray());
        }

        protected override void Dispose()
        {
            // TODO
        }

        protected override bool IsConnected
        {
            get
            {
                // TODO
                return true;
            }
        }
    }
}