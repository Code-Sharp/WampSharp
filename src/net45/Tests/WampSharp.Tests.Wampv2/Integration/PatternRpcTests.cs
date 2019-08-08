using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Core.Serialization;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    public class PatternRpcTests
    {
        private async Task Register(IWampChannel channel, string procedure, string match, Action<MyOperation, InvocationDetails> action)
        {
            MyOperation operation = new MyOperation(action, procedure);

            Task<IAsyncDisposable> task =
                channel.RealmProxy.RpcCatalog.Register(operation,
                                                   new RegisterOptions()
                                                   {
                                                       Match = match
                                                   });

            await task;
        }

        [TestCase("com.myapp.manage.47837483.create", "com.myapp.manage.47837483.create")]
        [TestCase("com.myapp.hi.hello", "com.myapp")]
        [TestCase("com.myapp.manage.hello.hi", "com.myapp.manage")]
        [TestCase("com.myapp2.manage.hello.hi", "com.myapp")]
        [TestCase("com.2myapp.manage.hello.hi", "com....")]
        [TestCase("net.myapp.manage.hello.create", ".myapp...create")]
        public async Task PatternRpcTest(string procedureToInvoke, string matchedProcedure)
        {
            WampPlayground playground = new WampPlayground();
            playground.Host.Open();
            
            IWampChannel calleeChannel = playground.CreateNewChannel("realm1");
            await calleeChannel.Open();

            Action<MyOperation, InvocationDetails> action =
                (operation, details) =>
                {
                    Assert.That(details.Procedure, Is.EqualTo(procedureToInvoke));
                    Assert.That(operation.Procedure, Is.EqualTo(matchedProcedure));
                };

            await Register(calleeChannel, "com.myapp.manage.47837483.create", "exact", action);
            await Register(calleeChannel, "com.myapp", "prefix", action);
            await Register(calleeChannel, "com.myapp.manage", "prefix", action);
            await Register(calleeChannel, "com....", "wildcard", action);
            await Register(calleeChannel, ".myapp...create", "wildcard", action);

            IWampChannel callerChannel = playground.CreateNewChannel("realm1");
            await callerChannel.Open();

            var callback = new MyCallback();

            callerChannel.RealmProxy.RpcCatalog.Invoke
                (callback, new CallOptions(), procedureToInvoke);

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

        private class MyOperation : LocalRpcOperation
        {
            private readonly Action<MyOperation, InvocationDetails> mAction;

            public MyOperation(Action<MyOperation, InvocationDetails> action, string procedure)
                : base(procedure)
            {
                mAction = action;
            }

            public override RpcParameter[] Parameters => new RpcParameter[0];

            public override bool HasResult => false;

            public override bool SupportsCancellation => false;

            public override CollectionResultTreatment CollectionResultTreatment => CollectionResultTreatment.SingleValue;

            protected override IWampCancellableInvocation InnerInvoke<TMessage>
                (IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                mAction(this, details);
                caller.Result(formatter, new YieldOptions());
                return null;
            }
        }
    }
}