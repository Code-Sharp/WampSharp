using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Tests.Wampv2.Integration.RpcProxies;
using WampSharp.Tests.Wampv2.Integration.RpcServices;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.Tests.Wampv2.Integration
{
    [TestFixture]
    public class CallerDealerTests
    {
        [Test]
        public async void ArgumentsAdd2()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ArgumentsService>(playground);

            IArgumentsService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IArgumentsService>();

            int five = proxy.Add2(2, 3);

            Assert.That(five, Is.EqualTo(5));
        }

        [Test]
        public async void ArgumentsAdd2Async()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ArgumentsService>(playground);

            IArgumentsService proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<IArgumentsService>();

            Task<int> task = proxy.Add2Async(2, 3);
            
            Assert.That(task.Result, Is.EqualTo(5));
        }

        [Test]
        [TestCase(null, null, "somebody starred 0x")]
        [TestCase("Homer", null, "Homer starred 0x")]
        [TestCase(null, 5, "somebody starred 5x")]
        [TestCase("Homer", 5, "Homer starred 5x")]
        public async void ArgumentsStarsDefaultValues(string nick, int? stars, string result)
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ArgumentsService>(playground);

            MockRawCallback callback = new MockRawCallback();

            Dictionary<string, object> argumentsKeywords = 
                new Dictionary<string, object>();

            if (nick != null)
            {
                argumentsKeywords["nick"] = nick;
            }
            if (stars != null)
            {
                argumentsKeywords["stars"] = stars;
            }

            channel.RealmProxy.RpcCatalog.Invoke
                (callback,
                 new Dictionary<string, object>(),
                 "com.arguments.stars",
                 new object[0],
                 argumentsKeywords);

            Assert.That(callback.Arguments.Select(x => x.Deserialize<string>()),
                        Is.EquivalentTo(new[] {result}));
        }

        [TestCase(0, "don't ask folly questions;)", "wamp.error.runtime_error")]
        [TestCase(1, "The square root of a negative number is non real", "wamp.error.runtime_error")]
        [TestCase(2, null)]
        public async void ErrorsService(int number, string exceptionMessage, string errorUri)
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ErrorsService>(playground);

            IErrorsService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IErrorsService>();

            Task<int> result = proxy.SqrtAsync(number);

            if (exceptionMessage != null)
            {
                AggregateException exception = result.Exception;
                Exception actualException = exception.InnerException;
                Assert.That(actualException, Is.TypeOf<WampException>());
                WampException wampException = actualException as WampException;
                
                Assert.That(wampException.Arguments, 
                    Is.EquivalentTo(new string[]{exceptionMessage}));

                Assert.That(wampException.ErrorUri, Is.EqualTo(errorUri));
            }

            Assert.That(result, Is.EqualTo(5));
        }

        private static async Task<IWampChannel> SetupService<TService>(WampPlayground playground)
            where TService : new()
        {
            const string realmName = "realm1";

            IWampRealm realm =
                playground.Host.RealmContainer.GetRealmByName(realmName);

            TService service = new TService();

            await realm.Services.RegisterCallee(service);

            playground.Host.Open();

            IWampChannel channel =
                playground.CreateNewChannel(realmName);

            await channel.Open();

            return channel;
        }
    }
}