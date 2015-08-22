using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class ProcedureRegistration : IWampRpcOperation
    {
        private readonly string mProcedureUri;
        private readonly RegisterOptions mRegisterOptions;

        private IImmutableList<IWampRpcOperation> mOperations =
            ImmutableList.Create<IWampRpcOperation>();

        private readonly IWampRpcOperationSelector mSelector;
        private readonly object mLock = new object();

        public ProcedureRegistration(string procedureUri, RegisterOptions registerOptions)
        {
            mProcedureUri = procedureUri;
            mSelector = GetOperationSelector(registerOptions.Invoke);
            mRegisterOptions = registerOptions;
        }

        private static IWampRpcOperationSelector GetOperationSelector(string invocationPolicy)
        {
            switch (invocationPolicy)
            {
                case WampInvokePolicy.Single:
                case WampInvokePolicy.First:
                    return new FirstOperationSelector();
                case WampInvokePolicy.Last:
                    return new LastOperationSelector();
                case WampInvokePolicy.Random:
                    return new RandomOperationSelector();
                case WampInvokePolicy.Roundrobin:
                    return new RoundrobinOperationSelector();
                default:
                    throw new WampException
                        ("wamp.error.invalid_options",
                         string.Format("invoke = {0} isn't supported", invocationPolicy));
            }
        }

        public long RegistrationId { get; set; }

        public event EventHandler Empty;

        public IWampRegistrationSubscriptionToken Register(IWampRpcOperation operation, RegisterOptions registerOptions)
        {
            VerifyInvokePoliciesAreCompatible(registerOptions);

            lock (mLock)
            {
                if (mRegisterOptions.Invoke != WampInvokePolicy.Single || !mOperations.Any())
                {
                    if (!mOperations.Contains(operation))
                    {
                        mOperations = mOperations.Add(operation);                        
                    }

                    return new WampRegistrationSubscriptionToken(operation, this);
                }
                else
                {
                    string registerError =
                        string.Format("register for already registered procedure '{0}'", operation.Procedure);

                    throw new WampException(WampErrors.ProcedureAlreadyExists,
                                            registerError);
                }
            }
        }

        private void VerifyInvokePoliciesAreCompatible(RegisterOptions registerOptions)
        {
            if (mRegisterOptions.Invoke != registerOptions.Invoke)
            {
                string messageDetails =
                    string.Format(
                        "register for already registered procedure '{0}' with conflicting invocation policy (has {1} and {2} was requested)",
                        this.Procedure, 
                        this.mRegisterOptions.Invoke, 
                        registerOptions.Invoke);

                throw new WampException
                    (WampErrors.ProcedureExistsInvocationPolicyConflict,
                     messageDetails);
            }
        }

        private void RemoveOperation(IWampRpcOperation operation)
        {
            lock (mLock)
            {
                mOperations = mOperations.Remove(operation);

                if (!mOperations.Any())
                {
                    RaiseEmpty();
                }
            }
        }

        protected virtual void RaiseEmpty()
        {
            EventHandler handler = Empty;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public string Procedure
        {
            get
            {
                return mProcedureUri;
            }
        }

        public bool HasOperations
        {
            get
            {
                return mOperations.Any();
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details)
        {
            InvokePattern
                (caller,
                 operation => operation.Invoke(caller, formatter, details));
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     TMessage[] arguments)
        {
            InvokePattern
                (caller,
                 operation => operation.Invoke(caller, formatter, details, arguments));
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            InvokePattern
                (caller,
                 operation => operation.Invoke(caller, formatter, details, arguments, argumentsKeywords));
        }

        private void InvokePattern(IWampRawRpcOperationRouterCallback caller, Action<IWampRpcOperation> invokeAction)
        {
            lock (mLock)
            {
                IWampRpcOperation operation = GetOperation();

                if (operation != null)
                {
                    invokeAction(operation);
                }
            }
        }

        private IWampRpcOperation GetOperation()
        {
            IWampRpcOperation result = mSelector.SelectOperation(mOperations);

            if (result == null)
            {
                WampRpcThrowHelper.NoProcedureRegistered(Procedure);
            }

            return result;
        }

        private class WampRegistrationSubscriptionToken : IWampRegistrationSubscriptionToken
        {
            private readonly IWampRpcOperation mOperation;
            private readonly ProcedureRegistration mRegistration;

            public WampRegistrationSubscriptionToken(IWampRpcOperation operation, ProcedureRegistration registration)
            {
                mOperation = operation;
                mRegistration = registration;
            }

            public void Dispose()
            {
                mRegistration.RemoveOperation(mOperation);
            }

            public long TokenId
            {
                get
                {
                    return mRegistration.RegistrationId;
                }
            }
        }
    }
}