using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Fluent;

namespace WampSharp.Samples
{
    public class SamplesArgumentParser
    {
        public static IWampChannel CreateWampChannel(string[] args)
        {
            (string address, int port, string realm, Transport transport, Serialization serialization) =
                ParseOptions(args);

            WampChannelFactory factory = new WampChannelFactory();

            var realmSyntax =
                factory.ConnectToRealm(realm);

            ChannelFactorySyntax.ITransportSyntax transportSyntax = default;

            if (transport == Transport.WebSocket)
            {
                transportSyntax = realmSyntax.WebSocketTransport(new Uri($"ws://{address}:{port}"));
            }
            else if (transport == Transport.RawSocket)
            {
                transportSyntax = realmSyntax.RawSocketTransport(address, port);
            }

            ChannelFactorySyntax.ISerializationSyntax serializationSyntax;

            if (serialization == Serialization.Json)
            {
                serializationSyntax = transportSyntax.JsonSerialization();
            }
            else if (serialization == Serialization.Msgpack)
            {
                serializationSyntax = transportSyntax.MessagePackSerialization();
            }
            else
            {
                serializationSyntax = transportSyntax.CborSerialization();
            }

            IWampChannel channel = serializationSyntax.Build();
            return channel;
        }

        private static (string address, int port, string realm, Transport transport, Serialization serialization) ParseOptions(string[] args)
        {
            const string defaultAddress = "127.0.0.1";
            Option optionAddress =
                new Option("--address",
                           $"Remote router address. Defaults to {defaultAddress}.")
                {
                    Argument = new Argument<string>()
                };
            optionAddress.AddAlias("-a");
            optionAddress.Argument.SetDefaultValue(defaultAddress);

            const int defaultPort = 8080;
            Option optionPort =
                new Option("--port",
                           $"Remote router port. Defaults to {defaultPort}")
                {
                    Argument = new Argument<int>()
                };
            optionPort.AddAlias("-p");
            optionPort.Argument.SetDefaultValue(defaultPort);

            string defaultRealm = "realm1";
            Option optionRealm =
                new Option("--realm", $"Remote router realm. Defaults to {defaultRealm}")
                {
                    Argument = new Argument<string>()
                };
            optionRealm.AddAlias("-r");
            optionRealm.Argument.SetDefaultValue(defaultRealm);

            const Transport defaultTransport = Transport.WebSocket;
            Option optionTransport = new Option("--transport",
                                          $"Transport to use in order to connect to remote router. Default is {defaultTransport}")
                               {
                                   Argument = new Argument<Transport>()
                               };
            optionTransport.AddAlias("-t");
            optionTransport.Argument.SetDefaultValue(defaultTransport);


            const Serialization defaultSerialization = Serialization.Json;
            Option optionSerialization = new Option("--serialization",
                                          $"Serialization to use in order to connect to remote router. Default is {defaultSerialization}")
                               {
                                   Argument = new Argument<Serialization>()
                               };
            optionSerialization.AddAlias("-s");
            optionSerialization.Argument.SetDefaultValue(defaultSerialization);
            
            string userAddress = default; 
            int userPort = default; 
            string userRealm = default; 
            Transport userTransport = default; 
            Serialization userSerialization = default;

            Parser parser =
                new CommandLineBuilder(new RootCommand
                                       {
                                           Handler = CommandHandler
                                               .Create<string, int, string, Transport, Serialization>
                                                   ((address, port, realm, transport, serialization) =>
                                                        (userAddress, userPort, userRealm, userTransport,
                                                         userSerialization) = 
                                                        (address, port, realm, transport, serialization))
                                       })
                    .AddOption(optionAddress)
                    .AddOption(optionPort)
                    .AddOption(optionRealm)
                    .AddOption(optionTransport)
                    .AddOption(optionSerialization)
                    .Build();

            parser.Invoke(args);

            return (userAddress, userPort, userRealm, userTransport,
                    userSerialization);
        }
    }

    enum Serialization
    {
        Json,
        Msgpack,
        Cbor
    }

    enum Transport
    {
        WebSocket,
        Ws = WebSocket,
        RawSocket
    }
}
