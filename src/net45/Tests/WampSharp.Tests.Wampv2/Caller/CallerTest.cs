using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.Core.Serialization;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Caller
{
    public class CallerTest : CallerTest<JToken>
    {
        public CallerTest() : 
            base(new JTokenBinding(), new JTokenEqualityComparer())
        {
        }
    }

    public class CallerTest<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private object[] mExpectedCallParameters;
        private object[] mExpectedResultParameters;
        private object[] mExpectedErrorParameters;
        private Action<IWampRpcOperationCatalogProxy, IWampRawRpcOperationCallback> mCallAction;
        private string mTestName;
        private readonly ServerMock mServerMock = new ServerMock();
        private readonly CallbackMock mCallbackMock = new CallbackMock();
        private readonly IEqualityComparer<TMessage> mEqualityComparer;

        public CallerTest(IWampBinding<TMessage> binding, IEqualityComparer<TMessage> equalityComparer)
        {
            mBinding = binding;
            mEqualityComparer = equalityComparer;
        }

        public string TestName
        {
            get { return mTestName; }
            set { mTestName = value; }
        }

        public void SetupCall(object options, string procedure)
        {
            mExpectedCallParameters = new object[] {options, procedure};
            mCallAction = (catalog, callback) => catalog.Invoke(callback, options, procedure);
        }

        public void SetupCall(object options, string procedure, object[] arguments)
        {
            mExpectedCallParameters = new object[] { options, procedure, arguments };
            mCallAction = (catalog, callback) => catalog.Invoke(callback, options, procedure, arguments);
        }

        public void SetupCall(object options, string procedure, object[] arguments, object argumentsKeywords)
        {
            mExpectedCallParameters = new object[] {options, procedure, arguments, argumentsKeywords};
            mCallAction = (catalog, callback) => catalog.Invoke(callback, options, procedure, arguments, argumentsKeywords);
        }

        public void SetupResult(object details)
        {
            mExpectedResultParameters = new object[] { details };
            mServerMock.SetCallerCallback((caller, requestId) => caller.Result(requestId, details));
        }

        public void SetupResult(object details, object[] arguments)
        {
            mExpectedResultParameters = new object[]{details, arguments};
            mServerMock.SetCallerCallback((caller, requestId) => caller.Result(requestId, details, arguments));
        }

        public void SetupResult(object details, object[] arguments, object argumentsKeywords)
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

        public void Act()
        {
            WampClientPlayground playground =
                new WampClientPlayground();

            IWampChannel channel =
                playground.GetChannel(mServerMock, "realm1", mBinding);

            channel.Open();

            mCallAction(channel.RealmProxy.RpcCatalog, mCallbackMock);
        }

        public void Assert()
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

        private void CompareParameters(object[] expected, object[] actual, string parameterType)
        {
            NUnit.Framework.Assert.That
                (expected.Select(x => SerializeArgument(x)),
                 Is.EquivalentTo(actual)
                   .Using(new ArgumentComparer(mEqualityComparer)),
                 string.Format("Expected {0} parameters were different than actual {0} parameters", parameterType));
        }

        private object SerializeArgument(object x)
        {
            if (x is string || x is long)
            {
                return x;
            }

            IWampFormatter<TMessage> formatter = mBinding.Formatter;
            object[] array = x as object[];
            
            if (array != null)
            {
                TMessage[] arguments = 
                    array.Select(y => formatter.Serialize(y)).ToArray();
                
                return arguments;
            }

            return formatter.Serialize(x);
        }

        private class ServerMock : IWampServer<TMessage>
        {
            private object[] mActualCallParamters;
            private Action<IWampCaller, long> mCallerCallback;

            public object[] ActualCallParamters
            {
                get
                {
                    return mActualCallParamters;
                }
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

            public void Hello(IWampSessionClient client, string realm, TMessage details)
            {
                client.Welcome(83782, new {});
            }

            public void Abort(IWampSessionClient client, TMessage details, string reason)
            {
                throw new System.NotImplementedException();
            }

            public void Authenticate(IWampSessionClient client, string signature, TMessage extra)
            {
                throw new System.NotImplementedException();
            }

            public void Goodbye(IWampSessionClient client, TMessage details, string reason)
            {
                throw new System.NotImplementedException();
            }

            public void Heartbeat(IWampSessionClient client, int incomingSeq, int outgoingSeq)
            {
                throw new System.NotImplementedException();
            }

            public void Heartbeat(IWampSessionClient client, int incomingSeq, int outgoingSeq, string discard)
            {
                throw new System.NotImplementedException();
            }

            public void OnNewClient(IWampClient<TMessage> client)
            {
                throw new System.NotImplementedException();
            }

            public void Register(IWampCallee callee, long requestId, TMessage options, string procedure)
            {
                throw new System.NotImplementedException();
            }

            public void Unregister(IWampCallee callee, long requestId, long registrationId)
            {
                throw new System.NotImplementedException();
            }

            // TODO: Throw exceptions if 
            public void Call(IWampCaller caller, long requestId, TMessage options, string procedure)
            {
                ActualCallParamters = new object[] {options, procedure};
                mCallerCallback(caller, requestId);
            }

            public void Call(IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments)
            {
                ActualCallParamters = new object[] { options, procedure, arguments };
                mCallerCallback(caller, requestId);
            }

            public void Call(IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments,
                             TMessage argumentsKeywords)
            {
                ActualCallParamters = new object[] { options, procedure, arguments, argumentsKeywords };
                mCallerCallback(caller, requestId);
            }

            public void Cancel(IWampCaller caller, long requestId, TMessage options)
            {
                throw new System.NotImplementedException();
            }

            public void Yield(IWampCallee callee, long requestId, TMessage options)
            {
                throw new System.NotImplementedException();
            }

            public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments)
            {
                throw new System.NotImplementedException();
            }

            public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
            {
                throw new System.NotImplementedException();
            }

            public void Error(IWampClient client, int requestType, long requestId, TMessage details, string error)
            {
                throw new System.NotImplementedException();
            }

            public void Error(IWampClient client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments)
            {
                throw new System.NotImplementedException();
            }

            public void Error(IWampClient client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments,
                              TMessage argumentsKeywords)
            {
                throw new System.NotImplementedException();
            }

            public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri)
            {
                throw new System.NotImplementedException();
            }

            public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri, TMessage[] arguments)
            {
                throw new System.NotImplementedException();
            }

            public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri, TMessage[] arguments,
                                TMessage argumentKeywords)
            {
                throw new System.NotImplementedException();
            }

            public void Subscribe(IWampSubscriber subscriber, long requestId, TMessage options, string topicUri)
            {
                throw new System.NotImplementedException();
            }

            public void Unsubscribe(IWampSubscriber subscriber, long requestId, long subscriptionId)
            {
                throw new System.NotImplementedException();
            }
        }

        private class CallbackMock : IWampRawRpcOperationCallback
        {
            private object[] mActualResult;
            private object[] mActualError;

            public object[] ActualResult
            {
                get { return mActualResult; }
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
                get
                {
                    return mActualError;
                }
                set
                {
                    if (ActualResult != null)
                    {
                        throw new Exception("Error already set!");
                    }
                    
                    mActualError = value;
                }
            }

            public void Result<TMessage1>(IWampFormatter<TMessage1> formatter, TMessage1 details)
            {
                ActualResult = new object[] {details};
            }

            public void Result<TMessage1>(IWampFormatter<TMessage1> formatter, TMessage1 details, TMessage1[] arguments)
            {
                ActualResult = new object[] { details, arguments };
            }

            public void Result<TMessage1>(IWampFormatter<TMessage1> formatter, TMessage1 details, TMessage1[] arguments, TMessage1 argumentsKeywords)
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

        private class ArgumentComparer : IEqualityComparer
        {
            private readonly IEqualityComparer<TMessage> mEqualityComparer;

            public ArgumentComparer(IEqualityComparer<TMessage> equalityComparer)
            {
                mEqualityComparer = equalityComparer;
            }

            public bool Equals(object x, object y)
            {
                if (x is TMessage[] && y is TMessage[])
                {
                    IEnumerable<TMessage> first = x as IEnumerable<TMessage>;
                    IEnumerable<TMessage> second = y as IEnumerable<TMessage>;
                    return first.SequenceEqual(second, mEqualityComparer);
                }
                if (x is TMessage && y is TMessage)
                {
                    return mEqualityComparer.Equals((TMessage)x, (TMessage)y);                    
                }

                return false;
            }

            public int GetHashCode(object obj)
            {
                return 0;
            }
        }
    }
}