using System;
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
        private readonly Handshake mHandshake;
        private readonly IWampTransportBinding<TMessage, byte[]> mBinding;
        private readonly RawSocketFrameHeaderParser mFrameHeaderParser = new RawSocketFrameHeaderParser();
        private readonly TcpClient mTcpClient;
        private readonly Func<TcpClient, Task> mTcpClientConnector;

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
            byte[] bytes = mBinding.Format(message);
            await TcpClient.GetStream().WriteAsync(bytes, 0, bytes.Length);
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
            byte[] buffer = await ReadBuffer(messageLength);

            WampMessage<TMessage> parsed = mBinding.Parse(buffer);

            RaiseMessageArrived(parsed);
        }

        private async Task<byte[]> ReadBuffer(int messageLength)
        {
            byte[] buffer = new byte[messageLength];

            await TcpClient.GetStream().ReadExactAsync(buffer);

            return buffer;
        }

        private async Task HandlePong(int messageLength)
        {
        }

        private async Task HandlePing(int messageLength)
        {
            byte[] buffer = await ReadBuffer(messageLength);

            byte[] array = new byte[4];

            mFrameHeaderParser.WriteHeader(FrameType.Pong, messageLength, array);

            await mTcpClient.GetStream().WriteAsync(array, 0, array.Length);
            await mTcpClient.GetStream().WriteAsync(buffer, 0, buffer.Length);
        }
    }
}