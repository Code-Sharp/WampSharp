using System;
using System.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeOperationCatalog<TMessage> : IWampCalleeOperationCatalog<TMessage>
    {
        private readonly IWampRpcOperationCatalog mCatalog;
        private readonly IWampCalleeInvocationHandler<TMessage> mInvocationHandler;

        private readonly WampIdMapper<WampCalleeRpcOperation> mRegistrationIdToOperation =
            new WampIdMapper<WampCalleeRpcOperation>();

        public WampCalleeOperationCatalog(IWampRpcOperationCatalog catalog, IWampCalleeInvocationHandler<TMessage> invocationHandler)
        {
            mCatalog = catalog;
            mInvocationHandler = invocationHandler;
        }

        public long Register(IWampCallee callee, TMessage options, string procedure)
        {
            WampCalleeRpcOperation operation =
                new WampCalleeRpcOperation(procedure, callee, options,
                                           mInvocationHandler, this);

            long registrationId = 
                mRegistrationIdToOperation.Add(operation);

            operation.RegistrationId = registrationId; // Hate this setter.

            mCatalog.Register(operation);

            return registrationId;
        }

        public void Unregister(IWampCallee callee, long registrationId)
        {
            WampCalleeRpcOperation operation;
            
            mRegistrationIdToOperation.TryRemove(registrationId, out operation);

            mCatalog.Unregister(operation);
        }

        private class WampCalleeRpcOperation : IWampRpcOperation<TMessage>,
            IWampRpcOperation
        {
            private readonly IWampCallee mCallee;
            private readonly IWampCalleeInvocationHandler<TMessage> mHandler;
            private readonly WampCalleeOperationCatalog<TMessage> mCatalog;
            private readonly string mProcedure;
            private readonly TMessage mOptions;

            public WampCalleeRpcOperation(string procedure, IWampCallee callee, TMessage options, IWampCalleeInvocationHandler<TMessage> handler,
                WampCalleeOperationCatalog<TMessage> catalog)
            {
                mCallee = callee;
                mOptions = options;
                mHandler = handler;
                mCatalog = catalog;
                mProcedure = procedure;

                IWampConnectionMonitor monitor = callee as IWampConnectionMonitor;
                monitor.ConnectionClosed += OnClientDisconnect;
            }

            private void OnClientDisconnect(object sender, System.EventArgs e)
            {
                IWampConnectionMonitor monitor = mCallee as IWampConnectionMonitor;
                monitor.ConnectionClosed -= OnClientDisconnect;

                mCatalog.Unregister(mCallee, RegistrationId);
            }

            public string Procedure
            {
                get
                {
                    return mProcedure;
                }
            }

            public long RegistrationId
            {
                get; 
                set;
            }

            public void Invoke<TOther>(IWampRpcOperationCallback caller,
                                       IWampFormatter<TOther> formatter,
                                       TOther details)
            {
                TMessage castedOptions = formatter.Deserialize<TMessage>(details);

                this.Invoke(caller, castedOptions);
            }

            public void Invoke<TOther>(IWampRpcOperationCallback caller,
                                       IWampFormatter<TOther> formatter,
                                       TOther options,
                                       TOther[] arguments)
            {
                TMessage castedOptions = formatter.Deserialize<TMessage>(options);

                TMessage[] castedArguments =
                    CastArguments(formatter, arguments);

                this.Invoke(caller, castedOptions, castedArguments);
            }

            public void Invoke<TOther>(IWampRpcOperationCallback caller,
                                       IWampFormatter<TOther> formatter,
                                       TOther options,
                                       TOther[] arguments,
                                       TOther argumentsKeywords)
            {
                TMessage castedOptions = formatter.Deserialize<TMessage>(options);

                TMessage[] castedArguments =
                    CastArguments(formatter, arguments);

                TMessage castedArgumentsKeywords =
                    formatter.Deserialize<TMessage>(argumentsKeywords);

                this.Invoke(caller, castedOptions, castedArguments, castedArgumentsKeywords);
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options)
            {
                long requestId = 
                    mHandler.RegisterInvocation(caller, options);

                mCallee.Invocation(requestId, RegistrationId, options);
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments)
            {
                long requestId = 
                    mHandler.RegisterInvocation(caller, options, arguments);

                mCallee.Invocation(requestId, RegistrationId, options, arguments.Cast<object>().ToArray());
            }

            public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
            {
                long requestId = 
                    mHandler.RegisterInvocation(caller, options, arguments, argumentsKeywords);

                mCallee.Invocation(requestId, RegistrationId, options, arguments.Cast<object>().ToArray(), argumentsKeywords);
            }

            private static TMessage[] CastArguments<TOther>(IWampFormatter<TOther> formatter, TOther[] arguments)
            {
                return arguments.Select(x =>
                                        formatter.Deserialize<TMessage>(x))
                                .ToArray();
            }
        }
    }
}