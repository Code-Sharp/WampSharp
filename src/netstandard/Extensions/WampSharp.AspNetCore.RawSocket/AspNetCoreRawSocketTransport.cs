using System;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using WampSharp.Core.Listener;
using WampSharp.RawSocket;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;

namespace WampSharp.AspNetCore.RawSocket
{
    public class AspNetCoreRawSocketTransport : TextBinaryTransport<SocketData>
    {
        private readonly TimeSpan? mAutoPingInterval;
        private Func<ConnectionContext, Func<Task>, Task> mHandler;
        private readonly Handshaker mHandshaker = new Handshaker();

        public byte MaxSize { get; }

        public AspNetCoreRawSocketTransport(IConnectionBuilder app,
                                            byte maxSize = 15,
                                            TimeSpan? autoPingInterval = null)
        {
            if (maxSize >= 16)
            {
                throw new ArgumentException("Expected a number between 0 to 15", nameof(maxSize));
            }

            mAutoPingInterval = autoPingInterval;

            MaxSize = maxSize;
            mHandler = this.EmptyHandler;
            app.Use(ConnectionHandler);
        }

        private Task ConnectionHandler(ConnectionContext context, Func<Task> next)
        {
            return mHandler(context, next);
        }

        public override void Dispose()
        {
            mHandler = this.EmptyHandler;
        }

        protected override void OpenConnection<TMessage>(SocketData original, IWampConnection<TMessage> connection)
        {
            RawSocketConnection<TMessage> casted = connection as RawSocketConnection<TMessage>;

            Task task = Task.Run(casted.RunAsync);

            original.ReadTask = task;
        }

        protected override string GetSubProtocol(SocketData connection)
        {
            return connection.HandshakeResponse.SerializerType.GetSubProtocol();
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>(SocketData connection,
                                                                                      IWampBinaryBinding<TMessage>
                                                                                          binding)
        {
            return CreateConnection(connection, binding);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>(SocketData connection,
                                                                                    IWampTextBinding<TMessage> binding)
        {
            return CreateConnection(connection, binding);
        }

        private IWampConnection<TMessage> CreateConnection<TMessage, TRaw>(SocketData connection,
                                                                           IWampTransportBinding<TMessage, TRaw> binding)
        {
            if (binding.ComputeBytes == null)
            {
                binding.ComputeBytes = true;
            }

            return new RawSocketConnection<TMessage>(connection, binding, mAutoPingInterval);
        }

        public override void Open()
        {
            mHandler = this.RawSocketHandler;
        }

        private async Task EmptyHandler(ConnectionContext connectionContext, Func<Task> next)
        {
            await next().ConfigureAwait(false);
        }

        private async Task RawSocketHandler(ConnectionContext connectionContext, Func<Task> next)
        {
            PipeReader input = connectionContext.Transport.Input;

            Handshake handshake = await mHandshaker.GetHandshakeMessage(input)
                                                   .ConfigureAwait(false);

            // If we did not get the magic octet, it is probably an HTTP request.
            if (handshake == null)
            {
                await next().ConfigureAwait(false);
            }
            else
            {
                Handshake response = handshake.GetHandshakeResponse(SubProtocols, MaxSize);

                await mHandshaker.SendHandshake(connectionContext.Transport.Output,
                                                response)
                                 .ConfigureAwait(false);

                SocketData socketData = new SocketData(connectionContext, handshake, response, input);

                OnNewConnection(socketData);

                await socketData.ReadTask.ConfigureAwait(false);
            }
        }
    }
}