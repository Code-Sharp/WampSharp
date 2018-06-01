using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampProcedureRegistration : IWampProcedureRegistration
    {
        private readonly RegisterOptions mRegisterOptions;

        private IImmutableList<IWampRpcOperation> mOperations =
            ImmutableList.Create<IWampRpcOperation>();

        private readonly IWampRpcOperationSelector mSelector;
        private readonly object mLock = new object();

        public WampProcedureRegistration(string procedureUri, RegisterOptions registerOptions)
        {
            Procedure = procedureUri;
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
                        (WampErrors.InvalidOptions,
                         $"invoke = {invocationPolicy} isn't supported");
            }
        }

        public long RegistrationId { get; set; }

        public event EventHandler<WampCalleeAddEventArgs> CalleeRegistering;

        public event EventHandler<WampCalleeAddEventArgs> CalleeRegistered;

        public event EventHandler<WampCalleeRemoveEventArgs> CalleeUnregistering;

        public event EventHandler<WampCalleeRemoveEventArgs> CalleeUnregistered;

        public event EventHandler Empty;

        public IWampRegistrationSubscriptionToken Register(IWampRpcOperation operation, RegisterOptions registerOptions)
        {
            VerifyInvokePoliciesAreCompatible(registerOptions);

            lock (mLock)
            {
                if (RegisterOptions.Invoke != WampInvokePolicy.Single || !mOperations.Any())
                {
                    if (!mOperations.Contains(operation))
                    {
                        RaiseCalleeRegistering(operation);

                        mOperations = mOperations.Add(operation);

                        RaiseCalleeRegistered(operation);
                    }

                    return new WampRegistrationToken(operation, this);
                }
                else
                {
                    string registerError =
                        $"register for already registered procedure '{operation.Procedure}'";

                    throw new WampException(WampErrors.ProcedureAlreadyExists,
                                            registerError);
                }
            }
        }

        private void VerifyInvokePoliciesAreCompatible(RegisterOptions registerOptions)
        {
            if (RegisterOptions.Invoke != registerOptions.Invoke)
            {
                string messageDetails =
                    $"register for already registered procedure '{this.Procedure}' with conflicting invocation policy (has {this.RegisterOptions.Invoke} and {registerOptions.Invoke} was requested)";

                throw new WampException
                    (WampErrors.ProcedureExistsInvocationPolicyConflict,
                     messageDetails);
            }
        }

        private void RemoveOperation(IWampRpcOperation operation)
        {
            lock (mLock)
            {
                RaiseCalleeUnregistering(operation);

                mOperations = mOperations.Remove(operation);

                RaiseCalleeUnregistered(operation);

                if (!mOperations.Any())
                {
                    RaiseEmpty();
                }
            }
        }

        protected virtual void RaiseEmpty()
        {
            Empty?.Invoke(this, EventArgs.Empty);
        }

        public string Procedure { get; }

        public bool HasOperations => mOperations.Any();

        public RegisterOptions RegisterOptions => mRegisterOptions;

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details)
        {
            return InvokePattern
                (operation => operation.Invoke(caller, formatter, details));
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     TMessage[] arguments)
        {
            return InvokePattern
                (operation => operation.Invoke(caller, formatter, details, arguments));
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            return InvokePattern
                (operation => operation.Invoke(caller, formatter, details, arguments, argumentsKeywords));
        }

        private IWampCancellableInvocation InvokePattern(Func<IWampRpcOperation, IWampCancellableInvocation> invokeAction)
        {
            lock (mLock)
            {
                IWampRpcOperation operation = GetOperation();

                if (operation != null)
                {
                    return invokeAction(operation);
                }
            }

            return null;
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

        private void RaiseCalleeRegistering(IWampRpcOperation operation)
        {
            CalleeRegistering?.Invoke(this, new WampCalleeAddEventArgs(operation));
        }

        private void RaiseCalleeRegistered(IWampRpcOperation operation)
        {
            CalleeRegistered?.Invoke(this, new WampCalleeAddEventArgs(operation));
        }

        private void RaiseCalleeUnregistering(IWampRpcOperation operation)
        {
            CalleeUnregistering?.Invoke(this, new WampCalleeRemoveEventArgs(operation));
        }

        private void RaiseCalleeUnregistered(IWampRpcOperation operation)
        {
            CalleeUnregistered?.Invoke(this, new WampCalleeRemoveEventArgs(operation));
        }

        private class WampRegistrationToken : IWampRegistrationSubscriptionToken
        {
            private readonly IWampRpcOperation mOperation;
            private readonly WampProcedureRegistration mRegistration;

            public WampRegistrationToken(IWampRpcOperation operation, WampProcedureRegistration registration)
            {
                mOperation = operation;
                mRegistration = registration;
            }

            public void Dispose()
            {
                mRegistration.RemoveOperation(mOperation);
            }

            public long TokenId => mRegistration.RegistrationId;
        }
    }
}