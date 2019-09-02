using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Services;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Fluent;

namespace WampSharp.Samples.Common
{
    public abstract class SampleCommand : ICommand
    {
        private string mAddress;

        [CommandOption("serialization",'s', Description = "Serialization to use in order to connect to remote router. Can be json msgpack or cbor. Default is json.", IsRequired = false)]
        public Serialization Serialization { get; set; } = Serialization.Json;

        [CommandOption("transport",'t', Description = "Transport to use in order to connect to remote router. Can be websocket or rawsocket. Default is websocket.", IsRequired = false)]
        public Transport Transport { get; set; } = Transport.WebSocket;

        [CommandOption("realm", 'r', Description = "Remote router realm. Defaults to realm1.")]
        public string Realm { get; set; } = "realm1";

        [CommandOption("address", 'a', Description =
            "Remote router address. Defaults to ws://127.0.0.1:8080/ws/ for WebSocket transport and to 127.0.0.1:8080 for RawSocket transport.")]
        public string Address
        {
            get
            {
                if (mAddress != null)
                {
                    return mAddress;
                }
                else if (this.Transport == Transport.WebSocket)
                {
                    return "ws://127.0.0.1:8080/ws/";
                }
                else
                {
                    return "127.0.0.1:8080";
                }
            }
            set => mAddress = value;
        }

        public async Task ExecuteAsync(IConsole console)
        {
            IWampChannel channel = CreateChannel();

            await RunAsync(channel).ConfigureAwait(false);
        }

        protected abstract Task RunAsync(IWampChannel channel);

        private IWampChannel CreateChannel()
        {
            WampChannelFactory factory = new WampChannelFactory();

            var realmSyntax = factory.ConnectToRealm(Realm);

            ChannelFactorySyntax.ITransportSyntax transportSyntax;

            switch (Transport)
            {
                case Transport.WebSocket:
                {
                    Uri parsedAddress = ParseWebSocketAddress();
                    transportSyntax = realmSyntax.WebSocketTransport(parsedAddress);

                    break;
                }
                case Transport.RawSocket:
                {
                    IPEndPoint parsedAddress = ParseIPEndpoint(Address);
                    transportSyntax = realmSyntax.RawSocketTransport(parsedAddress.Address, parsedAddress.Port);

                    break;
                }
                default:
                    throw new NotSupposedToReachHereException();
            }

            ChannelFactorySyntax.ISerializationSyntax serializationSyntax;

            switch (Serialization)
            {
                case Serialization.Json:
                    serializationSyntax = transportSyntax.JsonSerialization();
                    break;
                case Serialization.Msgpack:
                    serializationSyntax = transportSyntax.MessagePackSerialization();
                    break;
                case Serialization.Cbor:
                    serializationSyntax = transportSyntax.CborSerialization();
                    break;
                default:
                    throw new NotSupposedToReachHereException();
            }

            IWampChannel channel = serializationSyntax.Build();
            return channel;
        }

        private Uri ParseWebSocketAddress()
        {
            if (Uri.TryCreate(Address, UriKind.Absolute, out var parsed)
                && (parsed.Scheme == "ws" || parsed.Scheme == "wss"))
            {
                return parsed;
            }
            else
            {
                throw new ArgumentException("Invalid uri format. Expected an uri of schema type ws:// or wss://");
            }
        }

        private static IPEndPoint ParseIPEndpoint(string endPoint)
        {
            string[] endpointParts = endPoint.Split(':');

            if (endpointParts.Length < 2)
            {
                throw new FormatException("Invalid endpoint format");
            }

            IPAddress ipAddress;
            
            if (endpointParts.Length > 2)
            {
                if (!IPAddress.TryParse(string.Join(":", endpointParts, 0, endpointParts.Length - 1), out ipAddress))
                {
                    throw new FormatException("Invalid IP Address");
                }
            }
            else
            {
                if (!IPAddress.TryParse(endpointParts[0], out ipAddress))
                {
                    throw new FormatException("Invalid IP Address");
                }
            }

            int port;

            if (!int.TryParse(endpointParts[endpointParts.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }

            return new IPEndPoint(ipAddress, port);
        }
    }
}
