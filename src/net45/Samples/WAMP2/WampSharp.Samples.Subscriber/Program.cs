﻿using System;
using System.Threading.Tasks;
using WampSharp.V2;

namespace WampSharp.Samples.Subscriber
{
    class Program
    {
        async static Task Main(string[] args)
        {
            IWampChannel channel = SamplesArgumentParser.CreateWampChannel(args);

            await channel.Open();

            ComplexSubscriber service = new ComplexSubscriber();

            await using (IAsyncDisposable disposable =
                await channel.RealmProxy.Services.RegisterSubscriber(service))
            {
                Console.WriteLine($"Subscribered {service.GetType().Name}!");

                await Task.Yield();

                Console.ReadLine();
            }
        }
    }
}