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

    public interface IRpcCompositeService
    {
        [WampProcedure("test")]
        IRpcTestSubService GetTestSubService();

        [WampProcedure("switchAppliance")]
        void SwitchAppliance();

        [WampMember]
        IApplianceSubService appliance { get; }

        [WampProcedure("version")]
        string version();
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
        [WampMember]
        int capacity { get; }

        [WampProcedure("brew")]
        Task<string> brew(int amount);
    }


    public class RpcInterfaceTests
    {
        [Test]
        public async Task RpcInterfaceTest()
        {
            WampPlayground playground = new WampPlayground();
            playground.Host.Open();

            IWampChannel calleeChannel = playground.CreateNewChannel("realm1");
            await calleeChannel.Open();

            var callback = new TestCallback();

            var instance = new TestCompositeService(callback);
            IWampRealmProxy realm = calleeChannel.RealmProxy;


            var operation = new LocalRpcInterfaceOperation(instance, "com.test.instance.124");


            Task<IAsyncDisposable> registrationTask2 =
                calleeChannel.RealmProxy.RpcCatalog.Register(operation,
                                                   new RegisterOptions()
                                                   {
                                                       Match = WampMatchPattern.Prefix
                                                   });

            await registrationTask2;

            IWampChannel callerChannel = playground.CreateNewChannel("realm1");
            await callerChannel.Open();


            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback, new CallOptions(), "com.test.instance.124.version");

            Assert.That(callback.WhatWasCalled, Is.EqualTo("version"));

            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback, new CallOptions(), "com.test.instance.124.appliance.brew", new object[]{5});

            Assert.That(callback.WhatWasCalled, Is.EqualTo("CoffeeMaker.brew"));

            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback, new CallOptions(), "com.test.instance.124.switchAppliance");

            Assert.That(callback.WhatWasCalled, Is.EqualTo("switchAppliance"));

            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback, new CallOptions(), "com.test.instance.124.appliance.brew", new object[] { 5 });

            Assert.That(callback.WhatWasCalled, Is.EqualTo("Teapot.brew"));

            //Property test
            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback, new CallOptions(), "com.test.instance.124.appliance.capacity", new object[] { 5 });

            Assert.That(callback.WhatWasCalled, Is.EqualTo("Teapot.capacity"));

        }

        internal class TestCallback : IWampRawRpcOperationClientCallback
        {
            public string WhatWasCalled { get; private set; }
            public string ResultContent { get; private set; }
            public void Called(string what)
            {
                WhatWasCalled = what;
            }

            public virtual void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
            {
                ResultContent = details.ToString();
            }

            public virtual void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
            {
                ResultContent = arguments.ToString();
            }

            public virtual void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments,
                                                 IDictionary<string, TMessage> argumentsKeywords)
            {
                ResultContent = arguments.ToString() + " " + argumentsKeywords.ToString();
            }

            public virtual void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
            {
                ResultContent = "Error: " + details.ToString();
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


    internal class TestCompositeService: IRpcCompositeService
    {
        private readonly RpcTestSubService mTestSubService;
        private IApplianceSubService mAppliance;
        private readonly RpcInterfaceTests.TestCallback mCallback;

        public TestCompositeService(RpcInterfaceTests.TestCallback callback)
        {
            mCallback = callback;
            mAppliance = new CoffeeMaker(callback);
            mTestSubService = new RpcTestSubService(callback);
        }

        public IRpcTestSubService GetTestSubService()
        {
            return mTestSubService;
        }

        public void SwitchAppliance()
        {
            mCallback.Called("switchAppliance");
            mAppliance = new Teapot(mCallback);
        }

        public IApplianceSubService appliance => mAppliance;

        public string version()
        {
            mCallback.Called("version");
            return "0.1";
        }
        
    }

    internal class CoffeeMaker : IApplianceSubService
    {
        private RpcInterfaceTests.TestCallback mCallback;

        public CoffeeMaker(RpcInterfaceTests.TestCallback callback)
        {
            this.mCallback = callback;
        }

        public int capacity
        {
            get
            {
                mCallback.Called("CoffeeMaker.capacity");
                return 3;
            } 
        }

        public async Task<string> brew(int amount)
        {
            mCallback.Called("CoffeeMaker.brew");

            await Task.Delay(1000);
            return "Brewed " + amount;
        }
    }

    internal class Teapot : IApplianceSubService
    {
        private RpcInterfaceTests.TestCallback mCallback;

        public Teapot(RpcInterfaceTests.TestCallback mCallback)
        {
            this.mCallback = mCallback;
        }

        public int capacity
        {
            get
            {
                mCallback.Called("Teapot.capacity");

                return 1;
            } 
        }

        public async Task<string> brew(int amount)
        {
            mCallback.Called("Teapot.brew");
            return "Error 418 I'm a teapot" ;
        }
    }

    internal class RpcTestSubService : IRpcTestSubService
    {
        private RpcInterfaceTests.TestCallback mCallback;

        public RpcTestSubService(RpcInterfaceTests.TestCallback callback)
        {
            this.mCallback = callback;
        }

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
            mCallback.Called("test.progressive");

            return value;
        }

        public async Task<string> asyncMethod(string param)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            await Task.Delay(rand.Next(100, 1000));

            mCallback.Called("test.async");

            return param + rand.Next();
        }

        public string syncMethod(string param)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            mCallback.Called("test.sync");
            return param + rand.Next();
        }
    }



}
