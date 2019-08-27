using System;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Caller
{
    public interface IComplexServiceProxy
    {
        [WampProcedure("com.myapp.add_complex")]
        Task<(int c, int ci)> AddComplexAsync(int a, int ai, int b, int bi);

        [WampProcedure("com.myapp.split_name")]
        Task<(string, string)> SplitNameAsync(string fullname);
    }

    [Command("complex")]
    public class ComplexSample : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            IComplexServiceProxy proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<IComplexServiceProxy>();

            (int c, int ci) addResult = await proxy.AddComplexAsync(2, 3, 4, 5)
                                                   .ConfigureAwait(false);

            Console.WriteLine("Result: " + addResult);

            (string, string) splitResult = await proxy.SplitNameAsync("Homer Simpson")
                                                      .ConfigureAwait(false);

            Console.WriteLine($"Forename: {splitResult.Item1}, Surname: {splitResult.Item2}");

            Console.WriteLine("All finished.");

            await channel.Close(WampErrors.CloseNormal, new GoodbyeDetails());

            Console.ReadLine();
        }
    }
}