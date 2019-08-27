using System;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Services;
using WampSharp.V2;

namespace WampSharp.Samples.Caller
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .Build()
                .RunAsync(args);
        }
    }


}