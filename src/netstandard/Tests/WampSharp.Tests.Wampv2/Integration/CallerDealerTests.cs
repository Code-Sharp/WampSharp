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
        public async Task ArgumentsAdd2()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ArgumentsService>(playground);

            IArgumentsService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IArgumentsService>();

            int five = proxy.Add2(2, 3);

            Assert.That(five, Is.EqualTo(5));
        }

        [Test]
        public async Task ArgumentsAdd2Async()
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
        public async Task ArgumentsStarsDefaultValues(string nick, int? stars, string result)
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
                 new CallOptions(), 
                 "com.arguments.stars",
                 new object[0],
                 argumentsKeywords);

            Assert.That(callback.Arguments.Select(x => x.Deserialize<string>()),
                        Is.EquivalentTo(new[] {result}));
        }

        [TestCase(-2, new[] { "The square root of a negative number is non real", "x" }, "wamp.error.runtime_error")]
        [TestCase(0, new[] { "don't ask folly questions;)" }, "wamp.error.runtime_error")]
        [TestCase(2, null, null)]
        public async Task ErrorsService(int number, object[] arguments, string errorUri)
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ErrorsService>(playground);

            IErrorsService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IErrorsService>();

            Task<int> result = proxy.SqrtAsync(number);

            if (arguments == null)
            {
                Assert.That(result.Exception, Is.Null);
                Assert.That(result.IsCompleted, Is.True);
            }
            else
            {
                AggregateException exception = result.Exception;
                Exception actualException = exception.InnerException;
                Assert.That(actualException, Is.TypeOf<WampException>());
                WampException wampException = actualException as WampException;

                Assert.That(wampException.Arguments,
                            Is.EquivalentTo(arguments.Select(x => playground.Binding.Formatter.Serialize(x)))
                              .Using(playground.EqualityComparer));

                Assert.That(wampException.ErrorUri, Is.EqualTo(errorUri));
            }
        }

        [Test]
        public async Task ArgumentsOrders()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ArgumentsService>(playground);

            IArgumentsService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IArgumentsService>();

            string[] orders = 
                proxy.Orders("Book", 3);

            Assert.That(orders, Is.EquivalentTo(new[] {"Product 0", "Product 1", "Product 2"}));
        }

        [Test]
        public async Task ComplexServiceAddComplex()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ComplexResultService>(playground);

            IComplexResultService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IComplexResultService>();
            proxy.AddComplex(2, 3, 4, 5, out int c, out int ci);

            Assert.That(c, Is.EqualTo(6));
            Assert.That(ci, Is.EqualTo(8));
        }


        [Test]
        public async Task ComplexServiceTupleSplitName()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ComplexResultService>(playground);

            IPositionalTupleComplexResultService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IPositionalTupleComplexResultService>();

            var (firstName, surName) = proxy.SplitName("Homer Simpson");

            Assert.That(firstName, Is.EqualTo("Homer"));
            Assert.That(surName, Is.EqualTo("Simpson"));
        }

        [Test]
        public async Task ComplexServiceTupleSplitNameTask()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ComplexResultService>(playground);

            IPositionalTupleComplexResultService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IPositionalTupleComplexResultService>();

            var (firstName, surName) = await proxy.SplitNameAsync("Homer Simpson");

            Assert.That(firstName, Is.EqualTo("Homer"));
            Assert.That(surName, Is.EqualTo("Simpson"));
        }

        [Test]
        public async Task ComplexServiceAddComplex_TupleCalleeProxy()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ComplexResultService>(playground);

            INamedTupleComplexResultService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<INamedTupleComplexResultService>();

            var (c, ci) = proxy.AddComplex(2, 3, 4, 5);

            Assert.That(c, Is.EqualTo(6));
            Assert.That(ci, Is.EqualTo(8));
        }

        [Test]
        public async Task ComplexServiceAddComplex_TupleCalleeProxyTask()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ComplexResultService>(playground);

            INamedTupleComplexResultService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<INamedTupleComplexResultService>();

            var (c, ci) = await proxy.AddComplexAsync(2, 3, 4, 5);

            Assert.That(c, Is.EqualTo(6));
            Assert.That(ci, Is.EqualTo(8));
        }


        [Test]
        public async Task ComplexNamedTupleServiceAddComplex()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<NamedTupleComplexResultService>(playground);

            IComplexResultService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IComplexResultService>();
            proxy.AddComplex(2, 3, 4, 5, out int c, out int ci);

            Assert.That(c, Is.EqualTo(6));
            Assert.That(ci, Is.EqualTo(8));
        }

        [Test]
        public async Task ComplexPositionalTupleServiceAddComplex()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<PositionalTupleComplexResultService>(playground);

            MockRawCallback callback = new MockRawCallback();

            Dictionary<string, object> argumentsKeywords =
                new Dictionary<string, object>()
                {
                    {"a", 2},
                    {"ai", 3},
                    {"b", 4},
                    {"bi", 5}
                };

            channel.RealmProxy.RpcCatalog.Invoke
                (callback,
                 new CallOptions(),
                 "com.myapp.add_complex",
                 new object[0],
                 argumentsKeywords);

            Assert.That(callback.Arguments.Select(x => x.Deserialize<int>()),
                        Is.EquivalentTo(new[] {6, 8}));
        }

        [Test]
        public async Task LongPositionalTupleServiceCalleeProxy()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<LongValueTuplesService>(playground);

            ILongValueTuplesServiceProxy proxy =
                channel.RealmProxy.Services.GetCalleeProxy<ILongValueTuplesServiceProxy>();

            string name = "Homer Simpson";

            (string item1, string item2, string item3, string item4, string item5, string item6, string item7, string item8, string item9, string item10, int length) =
                proxy.GetLongPositionalTuple(name);

            Assert.That(item1, Is.EqualTo(name + " " + 0));
            Assert.That(item10, Is.EqualTo(name + " " + 9));
            Assert.That(length, Is.EqualTo(name.Length));
        }

        [Test]
        public async Task LongKeywordTupleServiceCalleeProxy()
        {
            WampPlayground playground = new WampPlayground();

            CallerCallee dualChannel = await playground.GetCallerCalleeDualChannel();

            IWampChannel calleeChannel = dualChannel.CalleeChannel;

            var registration =
                await calleeChannel.RealmProxy.RpcCatalog.Register(new LongValueTuplesService.KeywordTupleOperation(),
                                                                   new RegisterOptions());

            IWampChannel callerChannel = dualChannel.CallerChannel;

            ILongValueTuplesServiceProxy proxy =
                callerChannel.RealmProxy.Services.GetCalleeProxy<ILongValueTuplesServiceProxy>();

            string name = "Homer Simpson";

            var (item1, item2, item3, item4, item5, item6, item7, item8, length, item9, item10) =
                proxy.GetLongKeywordTuple(name);

            Assert.That(item1, Is.EqualTo(name + " " + 0));
            Assert.That(item10, Is.EqualTo(name + " " + 9));
            Assert.That(length, Is.EqualTo(name.Length));
        }

        [Test]
        public async Task LongPositionalTupleServiceCallee()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<LongValueTuplesCalleeService>(playground);

            MockRawCallback callback = new MockRawCallback();

            string name = "Homer Simpson";

            channel.RealmProxy.RpcCatalog.Invoke
                (callback,
                 new CallOptions(),
                 "com.myapp.get_long_positional_tuple",
                 new object[] { name },
                 new Dictionary<string, object>());

            int count = callback.Arguments.Count() - 1;

            Assert.That(callback.Arguments.Take(count).Select(x => x.Deserialize<string>()),
                        Is.EquivalentTo(Enumerable.Range(1, 10).Select(index => name + " " + index)));

            Assert.That(callback.Arguments.Last().Deserialize<int>(),
                        Is.EqualTo(name.Length));

            Assert.That(callback.ArgumentsKeywords,
                        Is.Null);
        }

        [Test]
        public async Task LongKeywordTupleServiceCallee()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<LongValueTuplesCalleeService>(playground);

            MockRawCallback callback = new MockRawCallback();

            string name = "Homer Simpson";

            channel.RealmProxy.RpcCatalog.Invoke
                (callback,
                 new CallOptions(),
                 "com.myapp.get_long_keyword_tuple",
                 new object[] { name },
                 new Dictionary<string, object>());

            int count = callback.Arguments.Count() - 1;

            Dictionary<string, object> resultDictionary =
                callback.ArgumentsKeywords.Where(x => x.Key.StartsWith("item"))
                        .ToDictionary(x => x.Key, x => (object)x.Value.Deserialize<string>());

            resultDictionary["length"] = callback.ArgumentsKeywords["length"].Deserialize<int>();

            Dictionary<string, object> expectedDicionary =
                Enumerable.Range(1, 10)
                          .ToDictionary(index => "item" + index,
                                        index => (object) (name + " " + index));

            expectedDicionary["length"] = name.Length;

            Assert.That(callback.ArgumentsKeywords.Keys,
                        Is.EquivalentTo(expectedDicionary.Keys));

            Assert.That(resultDictionary,
                        Is.EquivalentTo(expectedDicionary));

            Assert.That(callback.Arguments,
                        Is.Empty);
        }

        [Test]
        public async Task ComplexServiceSplitName()
        {
            WampPlayground playground = new WampPlayground();

            var channel = await SetupService<ComplexResultService>(playground);

            IComplexResultService proxy =
                channel.RealmProxy.Services.GetCalleeProxy<IComplexResultService>();

            string[] splitName = proxy.SplitName("Homer Simpson");

            Assert.That(splitName[0], Is.EqualTo("Homer"));
            Assert.That(splitName[1], Is.EqualTo("Simpson"));
        }

        private static async Task<IWampChannel> SetupService<TService>(WampPlayground playground)
            where TService : new()
        {
            const string realmName = "realm1";

            IWampHostedRealm realm =
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