using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.Binding;
using WampSharp.Core.Contracts;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
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
        private string[] authenticationMethods;
        private IDictionary<string, string> tickets = new Dictionary<string, string>()
        {
            { "peter", "md5f39d45e1da71cf755a7ee5d5840c7b0d" },
            { "joe", "magic_secret_2" }
        };
        private string user = "peter";

        public CustomAuthenticator()
        {
            this.authenticationMethods = new string[] { "ticket" };
        }

        public ChallengeResult Authenticate(string challenge, ChallengeDetails extra)
        {
            var challengeExtra = extra.OriginalValue.Deserialize<IDictionary<string, object>>();

            var method = (string)challengeExtra["authmethod"];
            Console.WriteLine(method);
            foreach (var ce in challengeExtra)
            {
                Console.WriteLine(ce);
            }

            if (method == "ticket")
            {
                Console.WriteLine("authenticating via '" + method + "'");
                var result = new ChallengeResult();
                result.Signature = tickets[user];
                result.Extra = new Dictionary<string, object>() { };
                return result;
            }
            else
            {
                throw new WampAuthenticationException("don't know how to authenticate using '" + method + "'");
            }
        }

        public string[] AuthenticationMethods
        {
            get { return authenticationMethods; }
        }

        public string AuthenticationId
        {
            get { return user; }
        }
    }

    class Program
    {
        private static IWampRealmProxy proxy;

        private static void Test(IWampRealmServiceProvider serviceProvider)
        {
            IArgumentsService proxy = serviceProvider.GetCalleeProxy<IArgumentsService>();
            string now;
            try
            {
                now = proxy.timeservice();
                Console.WriteLine("call result {0}", now);
            }
            catch (Exception e)
            {
                Console.WriteLine("call error {0}", e);
            }
        }

        static void Main(string[] args)
        {
            string url = "ws://127.0.0.1:8080/";
            string realm = "integra-s";

            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

            IWampClientAutenticator authenticator = new CustomAuthenticator();
            IWampChannel channel = channelFactory.CreateJsonChannel(url, realm, authenticator);
            channel.RealmProxy.Monitor.ConnectionEstablished += Monitor_ConnectionEstablished;
            channel.RealmProxy.Monitor.ConnectionBroken += Monitor_ConnectionBroken;
            Program.proxy = channel.RealmProxy;
            channel.Open().Wait();
            Test(channel.RealmProxy.Services);
            Console.ReadLine();
        }

        static void Monitor_ConnectionEstablished(object sender, V2.Realm.WampSessionEventArgs e)
        {
            var details = e.Details.Deserialize<IDictionary<string, object>>();
            
            Console.WriteLine("connected session with ID " + e.SessionId);
            Console.WriteLine("authenticated using method '" + details["authmethod"] + "' and provider '" + details["authprovider"] + "'");
            Console.WriteLine("authenticated with authid '" + details["authid"] + "' and authrole '" + details["authrole"] + "'");

            //Test(Program.proxy.Services);
        }

        static void Monitor_ConnectionBroken(object sender, V2.Realm.WampSessionCloseEventArgs e)
        {
            Console.WriteLine("disconnected {0}", e.Reason);
        }
    }
}