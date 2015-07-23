using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.IO;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.RawSocket
{
    public class TcpClientConnection<TMessage> : AsyncWampConnection<TMessage>, 
        IControlledWampConnection<TMessage>
    {
        private const string Tag = "WampSharp.RawSocket";
        private readonly Handshake mHandshake;
        private readonly IWampTransportBinding<TMessage, byte[]> mBinding;
        private readonly RawSocketFrameHeaderParser mFrameHeaderParser = new RawSocketFrameHeaderParser();
        private readonly TcpClient mTcpClient;
        private readonly Func<TcpClient, Task> mTcpClientConnector;
        private readonly RecyclableMemoryStreamManager mMemoryStreamManager = new RecyclableMemoryStreamManager();

        internal TcpClientConnection(TcpClient client,
                                     Handshake handshake,
                                     IWampTransportBinding<TMessage, byte[]> binding)
        {
            mTcpClient = client;
            mHandshake = handshake;
            mBinding = binding;
        }

        public TcpClientConnection(TcpClient client,
                                   Handshake handshakeRequest,
                                   IWampTextBinding<TMessage> binding)
            : this(client, handshakeRequest, new RawSocketBinding<TMessage>(binding))
        {
        }

        public TcpClientConnection(TcpClient client,
                                   Handshake handshakeRequest,
                                   IWampBinaryBinding<TMessage> binding)
            : this(client, handshakeRequest, new RawSocketBinding<TMessage>(binding))
        {
        }

        protected override bool IsConnected
        {
            get
            {
                return TcpClient.Connected;
            }
        }

        public bool IsClient
        {
            get;
            set;
        }

        protected async override Task SendAsync(WampMessage<object> message)
        {
            using (MemoryStream memoryStream = mMemoryStreamManager.GetStream())
            {
                using (MemoryStream headerStream = mMemoryStreamManager.GetStream(Tag, 4))
                {
                    mBinding.Format(message, memoryStream);
                    mFrameHeaderParser.WriteHeader(FrameType.WampMessage, (int)memoryStream.Length, headerStream.GetBuffer());
                    headerStream.Position = 0;
                    headerStream.SetLength(4);

                    await mTcpClient.GetStream().WriteAsync(headerStream.GetBuffer(), 0, 4);
                    await TcpClient.GetStream().WriteAsync(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }
        }

        private TcpClient TcpClient
        {
            get
            {
                return mTcpClient;
            }
        }

        public async void Connect()
        {
            await mTcpClientConnector(mTcpClient);

            if (IsClient)
            {
                await SendHandshake();

                byte[] buffer = new byte[4];

                await TcpClient.GetStream().ReadExactAsync(buffer);

                Handshake serverResponse = new Handshake(buffer);

                if (serverResponse.IsError)
                {
                    RaiseConnectionClosed();
                }
                else
                {
                    RaiseConnectionOpen();
                    await Task.Run((Func<Task>) HandleTcpClientAsync);
                }
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
                if (!IsClient)
                {
                    await SendHandshake();

                    RaiseConnectionOpen();
                }

                byte[] frameHeaderBytes = new byte[4];

                while (IsConnected)
                {
                    await TcpClient.GetStream()
                                   .ReadExactAsync(frameHeaderBytes)
                                   .ConfigureAwait(false);

                    int messageLength;
                    FrameType frameType;

                    if (mFrameHeaderParser.TryParse(frameHeaderBytes, out frameType, out messageLength))
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

        private async Task SendHandshake()
        {
            byte[] handshakeResponse = mHandshake.ToArray();

            await TcpClient.GetStream()
                           .WriteAsync(handshakeResponse,
                                       0,
                                       handshakeResponse.Length)
                           .ConfigureAwait(false);
        }

        private async Task HandleWampMessage(int messageLength)
        {
            using (MemoryStream buffer = await ReadBuffer(messageLength))
            {
                WampMessage<TMessage> parsed = mBinding.Parse(buffer);

                RaiseMessageArrived(parsed);
            }
        }

        private async Task<MemoryStream> ReadBuffer(int messageLength)
        {
            MemoryStream stream = 
                mMemoryStreamManager.GetStream(Tag, messageLength, true);

            await TcpClient.GetStream().ReadExactAsync(stream.GetBuffer(), messageLength);

            stream.Position = 0;
            stream.SetLength(messageLength);

            return stream;
        }

        private async Task HandlePong(int messageLength)
        {
        }

        private async Task HandlePing(int messageLength)
        {
            using (MemoryStream memoryStream = await ReadBuffer(messageLength))
            {
                using (MemoryStream headerStream = mMemoryStreamManager.GetStream(Tag, 4))
                {
                    mFrameHeaderParser.WriteHeader(FrameType.Pong, messageLength, headerStream.GetBuffer());
                    headerStream.Position = 0;
                    headerStream.SetLength(4);

                    await mTcpClient.GetStream().WriteAsync(headerStream.GetBuffer(), 0, 4);
                    await mTcpClient.GetStream().WriteAsync(memoryStream.GetBuffer(), 0, messageLength);
                }
            }
        }
    }
}