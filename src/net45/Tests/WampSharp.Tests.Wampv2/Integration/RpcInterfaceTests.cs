using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemEx;
using NUnit.Framework;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;
using WampSharp.WAMP2.V2.Rpc.Callee;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class RpcInterfaceTests
    {
        [Test]
        public async Task RpcInterfaceTest()
        {
            WampPlayground playground = new WampPlayground();
            playground.Host.Open();

            IWampChannel calleeChannel = playground.CreateNewChannel("realm1");
            await calleeChannel.Open();

            var instance = new TestCompositeService();

            IWampRealmProxy realm = calleeChannel.RealmProxy;

            //Task<IAsyncDisposable> registrationTask1 = realm.Services.RegisterCallee(instance);

            //await registrationTask1;

            var operation = new LocalRpcInterfaceOperation(new TestCompositeService(), "com.test.instance.124");


            Task<IAsyncDisposable> registrationTask2 =
                calleeChannel.RealmProxy.RpcCatalog.Register(operation,
                                                   new RegisterOptions()
                                                   {
                                                       Match = WampMatchPattern.Prefix
                                                   });

            await registrationTask2;

            IWampChannel callerChannel = playground.CreateNewChannel("realm1");
            await callerChannel.Open();

            var callback = new MyCallback();

            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback, new CallOptions(), "com.test.instance.124.appliance.isTeapot");

            Assert.That(callback.Called, Is.EqualTo(true));
        }

        protected class MyCallback : IWampRawRpcOperationClientCallback
        {
            public bool Called
            {
                get;
                private set;
            }

            public virtual void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
            {
                Called = true;
            }

            public virtual void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
            {
            }

            public virtual void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments,
                                                 IDictionary<string, TMessage> argumentsKeywords)
            {
            }

            public virtual void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
            {
            }

            public virtual void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments)
            {
            }

            public virtual void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments,
                                                TMessage argumentsKeywords)
            {
            }
        }
    }

    public interface IRpcTestSubService
    {
        [WampProcedure("progressiveTest")]
        [WampProgressiveResultProcedure]
        Task<string> progressiveResultsMethod(string param, IProgress<string> progress);

        [WampProcedure("asyncTest")]
        Task<string> asyncMethod(string param);

        [WampProcedure("syncTest")]
        string syncMethod(string param);

    }
    public interface IApplianceSubService
    {
        [WampProcedure("isTeapot")]
        bool isTeapot();

        [WampProcedure("boilWater")]
        string boil(int amount);
    }

    public interface IRpcCompositeService
    {
        [WampProcedure("test")]
        IRpcTestSubService GetTestSubService();

        [WampProcedure("appliance")]
        IApplianceSubService GetApplianceSubService();

        [WampProcedure("version")]
        string version();
    }


    public class TestCompositeService: IRpcCompositeService
    {
        private RpcTestSubService mTestSubService = new RpcTestSubService();
        private ApplianceSubService mAppliance = new ApplianceSubService();

        public IRpcTestSubService GetTestSubService()
        {
            return mTestSubService;
        }

        public IApplianceSubService GetApplianceSubService()
        {
            return mAppliance;
        }

        public string version()
        {
            return "0.1";
        }
    }

    internal class ApplianceSubService : IApplianceSubService
    {
        public bool isTeapot()
        {
            return true;
        }

        public string boil(int amount)
        {
            return "Boiled " + amount;
        }
    }

    internal class RpcTestSubService : IRpcTestSubService
    {
        public async Task<string> progressiveResultsMethod(string param, IProgress<string> progress)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int count = rand.Next(40);
            string value= null;
            for (int i = 0; i < count; i++)
            {
                value = param + rand.Next();
                //Don't return last result as intermediate progress
                if (i < count)
                {
                    progress.Report(value);
                    await Task.Delay(rand.Next(100, 1000));
                }
            }

            return value;
        }

        public async Task<string> asyncMethod(string param)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            await Task.Delay(rand.Next(100, 1000));


            return param + rand.Next();
        }

        public string syncMethod(string param)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return param + rand.Next();
        }
    }



}
