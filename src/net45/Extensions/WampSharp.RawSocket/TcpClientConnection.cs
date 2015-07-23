using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
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
        private readonly TcpClient mTcpClient;
        private readonly Func<TcpClient, Task> mTcpClientConnector;

        protected TcpClientConnection(TcpClient tcpClient, string hostName, int port)
        {
            mTcpClient = tcpClient;

            mTcpClientConnector = 
                client => client.ConnectAsync(hostName, port);
        }

        protected TcpClientConnection(TcpClient tcpClient, IPAddress address, int port)
        {
            mTcpClient = tcpClient;

            mTcpClientConnector =
                client => client.ConnectAsync(address, port);
        }

        protected TcpClientConnection(TcpClient tcpClient, IPAddress[] addresses, int port)
        {
            mTcpClient = tcpClient;

            mTcpClientConnector =
                client => client.ConnectAsync(addresses, port);
        }

        internal TcpClientConnection(TcpClient client,
                                     Handshake handshake,
                                     IWampTransportBinding<TMessage, byte[]> binding)
        {
            mTcpClient = client;
            mHandshake = handshake;
            mBinding = binding;
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
                    await TcpClient.GetStream().ReadExactAsync(frameHeaderBytes)
                                   .ConfigureAwait(false);

                    WampFrameHeader frameHeader = new WampFrameHeader(frameHeaderBytes);

                    int messageLength = frameHeader.MessageLength;
                    MessageType messageType = frameHeader.MessageType;

                    if (messageType == MessageType.WampMessage)
                    {
                        await HandleWampMessage(messageLength);
                    }
                    else if (messageType == MessageType.Ping)
                    {
                        await HandlePing(messageLength);
                    }
                    else if (messageType == MessageType.Pong)
                    {
                        await HandlePong(messageLength);
                    }
                }

                RaiseConnectionClosed();
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);
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

        private async Task HandlePong(int messageLength)
        {
        }

        private async Task HandlePing(int messageLength)
        {
        }

        private async Task HandleWampMessage(int messageLength)
        {
            byte[] buffer = new byte[messageLength];

            await TcpClient.GetStream().ReadExactAsync(buffer);

            WampMessage<TMessage> parsed = mBinding.Parse(buffer);

            RaiseMessageArrived(parsed);
        }
    }
}