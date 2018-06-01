using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Client.Callee
{
    public abstract class CalleeTest<TMessage> : RawTest<TMessage>
    {
        protected CalleeTest(IWampBinding<TMessage> binding, IEqualityComparer<TMessage> equalityComparer) : base(binding, equalityComparer)
        {
        }

        protected class DealerMock : IWampServer<TMessage>
        {
            private object[] mActualUnregistration;
            private Action<IWampCallee, long> mRegisterCallback;
            private Action<IWampCallee, long> mUnregisterCallback;
            private object[] mActualError;

            public object[] ActualRegistration { get; private set; }

            public object[] ActualUnregistration
            {
                get => mActualUnregistration;
                private set => mActualUnregistration = value;
            }

            public object[] ActualYield { get; private set; }

            public object[] ActualError
            {
                get => mActualError;
                private set => mActualError = value;
            }

            public void SetRegisterCallback(Action<IWampCallee, long> value)
            {
                mRegisterCallback = value;
            }

            public void SetUnregisterCallback(Action<IWampCallee, long> value)
            {
                mUnregisterCallback = value;
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
                ActualRegistration = new object[] {options, procedure};
                mRegisterCallback(callee, requestId);
            }

            public void Unregister(IWampCallee callee, long requestId, long registrationId)
            {
                ActualUnregistration = new object[] { registrationId };
                mUnregisterCallback(callee, requestId);
            }

            public void Call(IWampCaller caller, long requestId, CallOptions options, string procedure)
            {
                throw new System.NotImplementedException();
            }

            public void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments)
            {
                throw new System.NotImplementedException();
            }

            public void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                throw new System.NotImplementedException();
            }

            public void Cancel(IWampCaller caller, long requestId, CancelOptions options)
            {
                throw new System.NotImplementedException();
            }

            public void Yield(IWampCallee callee, long requestId, YieldOptions options)
            {
                ActualYield = new object[] { requestId, options };
            }

            public void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments)
            {
                ActualYield = new object[] { requestId, options, arguments };
            }

            public void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
            {
                ActualYield = new object[] { requestId, options, arguments, argumentsKeywords };
            }

            public void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error)
            {
                ActualError = new object[] { requestType, requestId, details, error };
            }

            public void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments)
            {
                ActualError = new object[] { requestType, requestId, details, error, arguments };
            }

            public void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments,
                              TMessage argumentsKeywords)
            {
                ActualError = new object[] { requestType, requestId, details, error, arguments, argumentsKeywords };
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

        protected class OperationMock : IWampRpcOperation
        {
            private Action<IWampRawRpcOperationRouterCallback> mInvocationCallback;
            public string Procedure { get; set; }

            public object[] ActualInvoke { get; private set; }

            public void SetInvocationCallback(Action<IWampRawRpcOperationRouterCallback> value)
            {
                mInvocationCallback = value;
            }

            public IWampCancellableInvocation Invoke<TMessage1>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage1> formatter, InvocationDetails details)
            {
                ActualInvoke = new object[] {details};
                mInvocationCallback(caller);
                return null;
            }

            public IWampCancellableInvocation Invoke<TMessage1>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage1> formatter, InvocationDetails details, TMessage1[] arguments)
            {
                ActualInvoke = new object[] { details, arguments };
                mInvocationCallback(caller);
                return null;
            }

            public IWampCancellableInvocation Invoke<TMessage1>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage1> formatter, InvocationDetails details, TMessage1[] arguments, IDictionary<string, TMessage1> argumentsKeywords)
            {
                ActualInvoke = new object[] {details, arguments, argumentsKeywords};
                mInvocationCallback(caller);
                return null;
            }
        }
    }
}