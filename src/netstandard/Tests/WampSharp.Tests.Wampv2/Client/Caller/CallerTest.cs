using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.Core.Serialization;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Client.Caller
{
    public class CallerTest : CallerTest<JToken>
    {
        public CallerTest() : 
            base(new JTokenJsonBinding(), new JTokenEqualityComparer())
        {
        }
    }

    public class CallerTest<TMessage> : RawTest<TMessage>
    {
        private object[] mExpectedCallParameters;
        private object[] mExpectedResultParameters;
        private object[] mExpectedErrorParameters;
        private Action<IWampRpcOperationCatalogProxy, IWampRawRpcOperationClientCallback> mCallAction;
        private readonly ServerMock mServerMock = new ServerMock();
        private readonly CallbackMock mCallbackMock = new CallbackMock();

        public CallerTest(IWampBinding<TMessage> binding, IEqualityComparer<TMessage> equalityComparer) : base(binding, equalityComparer)
        {
        }

        public void SetupCall(CallOptions options, string procedure)
        {
            mExpectedCallParameters = new object[] {options, procedure};
            mCallAction = (catalog, callback) => catalog.Invoke(callback, options, procedure);
        }

        public void SetupCall(CallOptions options, string procedure, object[] arguments)
        {
            mExpectedCallParameters = new object[] { options, procedure, arguments };
            mCallAction = (catalog, callback) => catalog.Invoke(callback, options, procedure, arguments);
        }

        public void SetupCall(CallOptions options, string procedure, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            mExpectedCallParameters = new object[] {options, procedure, arguments, argumentsKeywords};
            mCallAction = (catalog, callback) => catalog.Invoke(callback, options, procedure, arguments, argumentsKeywords);
        }

        public void SetupResult(ResultDetails details)
        {
            mExpectedResultParameters = new object[] { details };
            mServerMock.SetCallerCallback((caller, requestId) => caller.Result(requestId, details));
        }

        public void SetupResult(ResultDetails details, object[] arguments)
        {
            mExpectedResultParameters = new object[]{details, arguments};
            mServerMock.SetCallerCallback((caller, requestId) => caller.Result(requestId, details, arguments));
        }

        public void SetupResult(ResultDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            mExpectedResultParameters = new object[] { details, arguments, argumentsKeywords };
            mServerMock.SetCallerCallback((caller, requestId) => caller.Result(requestId, details, arguments, argumentsKeywords));
        }

        public void SetupError(object details, string error)
        {
            mExpectedErrorParameters = new object[] { details, error };
            mServerMock.SetCallerCallback((caller, requestId) => caller.CallError(requestId, details, error));
        }

        public void SetupError(object details, string error, object[] arguments)
        {
            mExpectedErrorParameters = new object[] { details, error, arguments };
            mServerMock.SetCallerCallback((caller, requestId) => caller.CallError(requestId, details, error, arguments));
        }

        public void SetupError(object details, string error, object[] arguments, object argumentsKeywords)
        {
            mExpectedErrorParameters = new object[] { details, error, arguments, argumentsKeywords };
            mServerMock.SetCallerCallback((caller, requestId) => caller.CallError(requestId, details, error, arguments, argumentsKeywords));
        }

        public override void Act()
        {
            WampClientPlayground playground =
                new WampClientPlayground();

            IWampChannel channel =
                playground.GetChannel(mServerMock, "realm1", mBinding);

            channel.Open();

            mCallAction(channel.RealmProxy.RpcCatalog, mCallbackMock);
        }

        public override void Assert()
        {
            CompareParameters(mExpectedCallParameters, mServerMock.ActualCallParamters, "call");                

            if (mExpectedErrorParameters != null)
            {
                CompareParameters(mExpectedErrorParameters, mCallbackMock.ActualError, "error");                
            }

            if (mExpectedResultParameters != null)
            {
                CompareParameters(mExpectedResultParameters, mCallbackMock.ActualResult, "result");
            }
        }

        private class ServerMock : IWampServer<TMessage>
        {
            private object[] mActualCallParamters;
            private Action<IWampCaller, long> mCallerCallback;

            public object[] ActualCallParamters
            {
                get => mActualCallParamters;
                set
                {
                    if (mActualCallParamters != null)
                    {
                        throw new Exception("Call parameters already set!");
                    }
                    
                    mActualCallParamters = value;
                }
            }

            public void SetCallerCallback(Action<IWampCaller, long> callback)
            {
                mCallerCallback = callback;
            }

            public void Hello(IWampSessionClient client, string realm, HelloDetails details)
            {
                client.Welcome(83782, new WelcomeDetails());
            }

            public void Abort(IWampSessionClient client, AbortDetails details, string reason)
            {
                throw new System.NotImplementedException();
            }

            public void Authenticate(IWampSessionClient client, string signature, AuthenticateExtraData extra)
            {
                throw new System.NotImplementedException();
            }

            public void Goodbye(IWampSessionClient client, GoodbyeDetails details, string reason)
            {
                throw new System.NotImplementedException();
            }

            public void OnNewClient(IWampClientProxy<TMessage> client)
            {
                throw new System.NotImplementedException();
            }

            public void OnClientDisconnect(IWampClientProxy<TMessage> client)
            {
                throw new NotImplementedException();
            }

            public void Register(IWampCallee callee, long requestId, RegisterOptions options, string procedure)
            {
                throw new System.NotImplementedException();
            }

            public void Unregister(IWampCallee callee, long requestId, long registrationId)
            {
                throw new System.NotImplementedException();
            }

            // TODO: Throw exceptions if 
            public void Call(IWampCaller caller, long requestId, CallOptions options, string procedure)
            {
                ActualCallParamters = new object[] {options, procedure};
                mCallerCallback(caller, requestId);
            }

            public void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments)
            {
                ActualCallParamters = new object[] { options, procedure, arguments };
                mCallerCallback(caller, requestId);
            }

            public void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                ActualCallParamters = new object[] { options, procedure, arguments, argumentsKeywords };
                mCallerCallback(caller, requestId);
            }

            public void Cancel(IWampCaller caller, long requestId, CancelOptions options)
            {
                throw new System.NotImplementedException();
            }

            public void Yield(IWampCallee callee, long requestId, YieldOptions options)
            {
                throw new System.NotImplementedException();
            }

            public void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments)
            {
                throw new System.NotImplementedException();
            }

            public void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                throw new System.NotImplementedException();
            }

            public void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error)
            {
                throw new System.NotImplementedException();
            }

            public void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments)
            {
                throw new System.NotImplementedException();
            }

            public void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments,
                              TMessage argumentsKeywords)
            {
                throw new System.NotImplementedException();
            }

            public void Publish(IWampPublisher publisher, long requestId, PublishOptions options, string topicUri)
            {
                throw new System.NotImplementedException();
            }

            public void Publish(IWampPublisher publisher, long requestId, PublishOptions options, string topicUri, TMessage[] arguments)
            {
                throw new System.NotImplementedException();
            }

            public void Publish(IWampPublisher publisher, long requestId, PublishOptions options, string topicUri, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords)
            {
                throw new System.NotImplementedException();
            }

            public void Subscribe(IWampSubscriber subscriber, long requestId, SubscribeOptions options, string topicUri)
            {
                throw new System.NotImplementedException();
            }

            public void Unsubscribe(IWampSubscriber subscriber, long requestId, long subscriptionId)
            {
                throw new System.NotImplementedException();
            }
        }

        private class CallbackMock : IWampRawRpcOperationClientCallback
        {
            private object[] mActualResult;
            private object[] mActualError;

            public object[] ActualResult
            {
                get => mActualResult;
                set
                {
                    if (ActualResult != null)
                    {
                        throw new Exception("Result already set!");
                    }
                    
                    mActualResult = value;
                }
            }

            public object[] ActualError
            {
                get => mActualError;
                set
                {
                    if (ActualResult != null)
                    {
                        throw new Exception("Error already set!");
                    }
                    
                    mActualError = value;
                }
            }

            public void Result<TMessage1>(IWampFormatter<TMessage1> formatter, ResultDetails details)
            {
                ActualResult = new object[] {details};
            }

            public void Result<TMessage1>(IWampFormatter<TMessage1> formatter, ResultDetails details, TMessage1[] arguments)
            {
                ActualResult = new object[] { details, arguments };
            }

            public void Result<TMessage1>(IWampFormatter<TMessage1> formatter, ResultDetails details, TMessage1[] arguments, IDictionary<string, TMessage1> argumentsKeywords)
            {
                ActualResult = new object[] { details, arguments, argumentsKeywords };
            }

            public void Error<TMessage1>(IWampFormatter<TMessage1> formatter, TMessage1 details, string error)
            {
                ActualError = new object[] { details, error };
            }

            public void Error<TMessage1>(IWampFormatter<TMessage1> formatter, TMessage1 details, string error, TMessage1[] arguments)
            {
                ActualError = new object[] { details, error, arguments };
            }

            public void Error<TMessage1>(IWampFormatter<TMessage1> formatter, TMessage1 details, string error, TMessage1[] arguments,
                                         TMessage1 argumentsKeywords)
            {
                ActualError = new object[] { details, error, arguments, argumentsKeywords };
            }
        }
    }
}