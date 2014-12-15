using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.Binding;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Authentication
{
    public interface IArgumentsService
    {
        [WampProcedure("com.timeservice.now")]
        string timeservice();
    }

    public class CustomAuthenticator : IWampClientAutenticator
    {
        public ChallengeResult Authenticate(string challenge, ChallengeDetails extra)
        {
            var result = new ChallengeResult();
            var myData = extra.OriginalValue.Deserialize<IDictionary<string, object>>();
            foreach (var data in myData)
            {
                Console.WriteLine(data);
            }
            result.Signature = "md5f39d45e1da71cf755a7ee5d5840c7b0d";
            result.Extra = new Dictionary<string, object>() { };
            return result;
        }
    }

    class Program
    {
        private static void Test(IWampRealmServiceProvider serviceProvider)
        {
            IArgumentsService proxy = serviceProvider.GetCalleeProxy<IArgumentsService>();

            var res = proxy.timeservice();
            Console.WriteLine(res);
        }

        static void Main(string[] args)
        {
            const string location = "ws://127.0.0.1:8080/";

            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

            var authenticator = new CustomAuthenticator();
            IWampChannel channel = channelFactory.CreateJsonChannel(location, "integra-s", authenticator);
            IWampRealmProxy realmProxy = channel.RealmProxy;

            channel.Open().Wait();

            Test(channel.RealmProxy.Services);

            Console.WriteLine("OK. Press anu key.");
            Console.ReadLine();
        }
    }
}