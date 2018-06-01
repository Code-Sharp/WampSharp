using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.Client.Callee
{
    public class InvocationCalleeeTest : InvocationCalleeeTest<JToken>
    {
        public InvocationCalleeeTest(long registrationId)
            : base(registrationId, new JTokenJsonBinding(), new JTokenEqualityComparer())
        {
        }
    }

    public class InvocationCalleeeTest<TMessage> : CalleeTest<TMessage>
    {
        private readonly DealerMock mDealer = new DealerMock();
        private readonly OperationMock mOperation = new OperationMock();
        private object[] mExpectedInvocation;
        private Action<IWampCallee> mInvocationAction;
        private readonly long mRegistrationId;

        public InvocationCalleeeTest(long registrationId, IWampBinding<TMessage> binding, IEqualityComparer<TMessage> equalityComparer) : base(binding, equalityComparer)
        {
            mRegistrationId = registrationId;
        }

        public object[] ExpectedYield { get; set; }

        public object[] ExpectedError { get; set; }

        public object[] ExpectedInvocation
        {
            get => mExpectedInvocation;
            set => mExpectedInvocation = value;
        }

        public void SetupInvocation(long requestId, long registrationId, InvocationDetails details)
        {
            ExpectedInvocation = new[] {details};
            
            mInvocationAction = 
                callee => callee.Invocation(requestId, registrationId, details);
        }

        public void SetupInvocation(long requestId, long registrationId, InvocationDetails details, object[] arguments)
        {
            ExpectedInvocation = new object[] { details, arguments };

            mInvocationAction =
                callee => callee.Invocation(requestId, registrationId, details, arguments);
        }

        public void SetupInvocation(long requestId, long registrationId, InvocationDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            ExpectedInvocation = new object[] { details, arguments, argumentsKeywords };

            mInvocationAction =
                callee => callee.Invocation(requestId, registrationId, details, arguments, argumentsKeywords);
        }

        public void SetupYield(long requestId, YieldOptions options)
        {
            ExpectedYield = new object[] {requestId, options};

            mOperation.SetInvocationCallback
                (x => x.Result(WampObjectFormatter.Value, options));
        }

        public void SetupYield(long requestId, YieldOptions options, object[] arguments)
        {
            ExpectedYield = new object[] { requestId, options, arguments };

            mOperation.SetInvocationCallback
                (x => x.Result(WampObjectFormatter.Value, options, arguments));
        }

        public void SetupYield(long requestId, YieldOptions options, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            ExpectedYield = new object[] { requestId, options, arguments, argumentsKeywords };

            mOperation.SetInvocationCallback
                (x => x.Result(WampObjectFormatter.Value, options, arguments, argumentsKeywords));
        }

        public void SetupError(int requestType, long requestId, object details, string error)
        {
            ExpectedError = new[] { requestType, requestId, details, error };

            mOperation.SetInvocationCallback
                (x => x.Error(WampObjectFormatter.Value, details, error));
        }

        public void SetupError(int requestType, long requestId, object details, string error, object[] arguments)
        {
            ExpectedError = new[] { requestType, requestId, details, error, arguments };

            mOperation.SetInvocationCallback
                (x => x.Error(WampObjectFormatter.Value, details, error, arguments));
        }

        public void SetupError(int requestType, long requestId, object details, string error, object[] arguments, object argumentsKeywords)
        {
            ExpectedError = new[] { requestType, requestId, details, error, arguments, argumentsKeywords };

            mOperation.SetInvocationCallback
                (x => x.Error(WampObjectFormatter.Value, details, error, arguments, argumentsKeywords));
        }

        public override void Act()
        {
            WampClientPlayground playground = new WampClientPlayground();

            IWampCallee calleeProxy = null;
            
            mDealer.SetRegisterCallback((callee, requestId) =>
                {
                    calleeProxy = callee;
                    callee.Registered(requestId, mRegistrationId);
                });

            IWampChannel channel =
                playground.GetChannel(mDealer, "realm1", mBinding);

            channel.Open();

            Task register = 
                channel.RealmProxy.RpcCatalog.Register(mOperation, new RegisterOptions());

            mInvocationAction(calleeProxy);
        }

        public override void Assert()
        {
            CompareParameters(this.ExpectedInvocation,
                              mOperation.ActualInvoke,
                              "invocation");

            if (this.ExpectedYield != null)
            {
                CompareParameters(this.ExpectedYield,
                                  mDealer.ActualYield,
                                  "yield");                
            }

            if (this.ExpectedError != null)
            {
                CompareParameters(this.ExpectedError,
                                  mDealer.ActualError,
                                  "error");
            }
        }
    }
}