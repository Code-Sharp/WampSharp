using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;

namespace WampSharp.RawSocket
{
    public class TcpListenerRawSocketTransport : WebSocketTransport<RawSocketTcpClient>
    {
        private readonly TcpListener mListener;
        private IDictionary<SerializerType, ConnectionListener> mListeners;
        private bool mIsStarted = false;

        public TcpListenerRawSocketTransport(TcpListener listener)
        {
            mListener = listener;
        }

        public TcpListenerRawSocketTransport(IPAddress localAddress, int port)
        {
            mListener = new TcpListener(localAddress, port);
        }

        public TcpListenerRawSocketTransport(IPEndPoint localEndpoint)
        {
            mListener = new TcpListener(localEndpoint);
        }

        public override void Dispose()
        {
            mIsStarted = false;
            mListener.Stop();
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
                    TcpClient tcpClient = await mListener.AcceptTcpClientAsync()
                                                         .ConfigureAwait(false);

                    RawSocketTcpClient wrapped = new RawSocketTcpClient(tcpClient);

                    await wrapped.GetHandshakeRequest();

                    if (tcpClient != null)
                    {
                        OnNewConnection(wrapped);
                    }
                }
                catch (Exception ex)
                {
                    //mLogger.Error("An error occured while trying to accept a client", ex);
                }
            }
        }

        protected override void OpenConnection<TMessage>(IWampConnection<TMessage> connection)
        {
            TcpClientConnection<TMessage> casted = connection as TcpClientConnection<TMessage>;
            Task.Run(new Func<Task>(casted.HandleTcpClientAsync));
        }

        protected override string GetSubProtocol(RawSocketTcpClient connection)
        {
            SerializerType serializerType = connection.HandshakeRequest.SerializerType;

            if (serializerType == SerializerType.Json)
            {
                return "wamp.2.json";
            }

            if (serializerType == SerializerType.MsgPack)
            {
                return "wamp.2.msgpack";
            }

            return serializerType.ToString();
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
            (RawSocketTcpClient connection,
             IWampBinaryBinding<TMessage> binding)
        {
            return new BinaryTcpClientConnection<TMessage>(connection.Client, connection.HandshakeRequest, binding);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
            (RawSocketTcpClient connection,
             IWampTextBinding<TMessage> binding)
        {
            return new TextTcpClientConnection<TMessage>(connection.Client, connection.HandshakeRequest, binding);
        }
    }
}