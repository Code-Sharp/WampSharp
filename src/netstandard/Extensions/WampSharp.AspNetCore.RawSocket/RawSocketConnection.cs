using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.RawSocket;
using WampSharp.V2.Binding.Parsers;
using WampSharp.V2.Transports;
using static WampSharp.RawSocket.RawSocketFrameHeaderParser;

namespace WampSharp.AspNetCore.RawSocket
{
    public class RawSocketConnection<TMessage> : AsyncWampConnection<TMessage>
    {
        private readonly ConnectionContext mConnection;
        private readonly RawSocketFrameHeaderParser mFrameHeaderParser = new RawSocketFrameHeaderParser();
        private readonly int mMaxAllowedMessageSize;
        private readonly IWampStreamingMessageParser<TMessage> mParser;
        private bool mIsConnected = true;
        private readonly Pinger mPinger;
        private readonly PingPongHandler mPingPongHandler;

        public RawSocketConnection(SocketData connection, 
                                   IWampStreamingMessageParser<TMessage> parser,
                                   TimeSpan? autoPingInterval)
        {
            mParser = parser;
            mConnection = connection.ConnectionContext;
            mMaxAllowedMessageSize = connection.Handshake.MaxMessageSizeInBytes;

            mPinger = new Pinger(this);

            mPingPongHandler =
                new PingPongHandler(mLogger,
                                    mPinger,
                                    autoPingInterval);
        }

        protected override async Task SendAsync(WampMessage<object> message)
        {
            MemoryBufferWriter writer = MemoryBufferWriter.Get();

            WriteBytes(message, writer);

            writer.CopyTo(Writer);

            await Writer.FlushAsync().ConfigureAwait(false);

            MemoryBufferWriter.Return(writer);
        }

        public async Task RunAsync()
        {
            mPingPongHandler.Start();

            try
            {
                while (mIsConnected)
                {
                    ReadResult result = await Reader.ReadAsync()
                                                    .ConfigureAwait(false);

                    ReadOnlySequence<byte> buffer = result.Buffer;

                    try
                    {
                        if (result.IsCanceled || result.IsCompleted)
                        {
                            mIsConnected = false;
                        }

                        if (!buffer.IsEmpty)
                        {
                            ProcessBuffer(buffer);
                        }
                    }
                    finally
                    {
                        Reader.AdvanceTo(buffer.End);
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);
                mIsConnected = false;
            }

            RaiseConnectionClosed();
        }

        private void ProcessBuffer(in ReadOnlySequence<byte> buffer)
        {
            ReadOnlySequence<byte> headerBytes =
                buffer.Slice(0, FrameHeaderSize);

            FrameType frameType;
            int messageLength;

            if (mFrameHeaderParser.TryParse(headerBytes, out frameType, out messageLength) &&
                (messageLength <= mMaxAllowedMessageSize))
            {
                ReadOnlySequence<byte> frameContent =
                    buffer.Slice(FrameHeaderSize, messageLength);

                HandleFrame(frameType, frameContent);
            }
            else
            {
                this.Dispose();
            }
        }

        private void HandleFrame(FrameType frameType, in ReadOnlySequence<byte> message)
        {
            switch (frameType)
            {
                case FrameType.WampMessage:
                    HandleWampFrame(message);
                    break;
                case FrameType.Ping:
                    Task task = HandlePingFrame(message);
                    break;
                case FrameType.Pong:
                    HandlePongFrame(message);
                    break;
            }
        }

        private async Task HandlePingFrame(ReadOnlySequence<byte> message)
        {
            WriteFrameHeader(Writer, FrameType.Pong, (int)message.Length);
            Writer.Write(message.ToArraySegment());
            await Writer.FlushAsync().ConfigureAwait(false);
        }

        private void HandlePongFrame(in ReadOnlySequence<byte> message)
        {
            mPinger.RaiseOnPong(message.ToArraySegment());
        }

        private void HandleWampFrame(in ReadOnlySequence<byte> message)
        {
            WampMessage<TMessage> parsed = ParseMessage(message);
            RaiseMessageArrived(parsed);
        }

        private WampMessage<TMessage> ParseMessage(ReadOnlySequence<byte> messageInBytes)
        {
            ArraySegment<byte> segment = messageInBytes.ToArraySegment();
            MemoryStream memoryStream = new MemoryStream(segment.Array, segment.Offset, segment.Count);
            return mParser.Parse(memoryStream);
        }

        private void WriteBytes(WampMessage<object> message, MemoryBufferWriter writer)
        {
            Span<byte> span = writer.GetSpan(FrameHeaderSize);
            writer.Advance(FrameHeaderSize);
            mParser.Format(message, writer);

            int messageLength = (int) writer.Length - FrameHeaderSize;

            mFrameHeaderParser.WriteHeader(FrameType.WampMessage,
                                           messageLength,
                                           span);
        }

        private async Task SendPing(byte[] message)
        {
            WriteFrameHeader(Writer, FrameType.Ping, message.Length);
            Writer.Write(message);
            await Writer.FlushAsync().ConfigureAwait(false);
        }

        private void WriteFrameHeader(PipeWriter writer, FrameType frameType, int messageLength)
        {
            Span<byte> headerSpan = writer.GetSpan(FrameHeaderSize);
            mFrameHeaderParser.WriteHeader(frameType, messageLength, headerSpan);
            writer.Advance(FrameHeaderSize);
        }

        protected override void Dispose()
        {
            Reader.CancelPendingRead();
            Reader.Complete();
            Writer.Complete();
        }

        protected override bool IsConnected => mIsConnected;

        private PipeReader Reader => mConnection.Transport.Input;

        private PipeWriter Writer => mConnection.Transport.Output;

        internal class Pinger : IPinger
        {
            private readonly RawSocketConnection<TMessage> mParent;

            public Pinger(RawSocketConnection<TMessage> parent)
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