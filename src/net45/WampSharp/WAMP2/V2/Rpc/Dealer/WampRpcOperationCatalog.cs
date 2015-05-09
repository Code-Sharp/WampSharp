using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcOperationCatalog : IWampRpcOperationCatalog
    {
        private readonly ConcurrentDictionary<string, Registration> mProcedureToRegistration =
            new ConcurrentDictionary<string, Registration>();

        private readonly WampIdMapper<Registration> mRegistrationIdToRegistration =
            new WampIdMapper<Registration>();
        
        private readonly object mLock = new object();

        public IWampRpcOperationRegistrationToken Register(IWampRpcOperation operation, RegisterOptions registerOptions)
        {
            lock (mLock)
            {
                Registration registration =
                    mProcedureToRegistration
                        .GetOrAdd
                        (operation.Procedure,
                         procedureUri => CreateRegistration(registerOptions, procedureUri));

                return registration.Register(operation, registerOptions);                
            }
        }

        private Registration CreateRegistration(RegisterOptions registerOptions, string procedureUri)
        {
            Registration result = new Registration(procedureUri, registerOptions);

            result.Empty += OnRegistrationEmpty;

            long registrationId = mRegistrationIdToRegistration.Add(result);

            result.RegistrationId = registrationId;

            return result;
        }

        private void OnRegistrationEmpty(object sender, EventArgs e)
        {
            Registration registration = sender as Registration;

            if (!registration.HasOperations)
            {
                lock (mLock)
                {
                    if (!registration.HasOperations)
                    {
                        Registration removed;
                        mRegistrationIdToRegistration.TryRemove(registration.RegistrationId, out removed);
                        mProcedureToRegistration.TryRemove(registration.Procedure, out removed);
                        registration.Empty -= OnRegistrationEmpty;
                    }
                }
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationClientCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure)
        {
            Invoke(new WampClientRouterCallbackAdapter(caller, details),
                   formatter,
                   details,
                   procedure);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationClientCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments)
        {
            Invoke(new WampClientRouterCallbackAdapter(caller, details),
                   formatter,
                   details,
                   procedure,
                   arguments);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationClientCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            Invoke(new WampClientRouterCallbackAdapter(caller, details),
                   formatter,
                   details,
                   procedure,
                   arguments,
                   argumentsKeywords);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure)
        {
            IWampRpcOperation operation = TryGetOperation(caller, details, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, details);
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details, string procedure, TMessage[] arguments)
        {
            IWampRpcOperation operation = TryGetOperation(caller, details, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, details, arguments);
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details, string procedure, TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            IWampRpcOperation operation = TryGetOperation(caller, details, procedure);

            if (operation != null)
            {
                IWampRpcOperation<TMessage> casted =
                    CastOperation(operation, formatter);

                casted.Invoke(caller, details, arguments, argumentsKeywords);
            }
        }

        private IWampRpcOperation TryGetOperation(IWampRawRpcOperationRouterCallback caller, InvocationDetails details,
                                                  string procedure)
        {
            Registration registration;

            if (mProcedureToRegistration.TryGetValue(procedure, out registration))
            {
                return registration;
            }
            else
            {
                caller.NoProcedureRegistered(procedure);
                return null;
            }
        }

        private IWampRpcOperation<TMessage> CastOperation<TMessage>
            (IWampRpcOperation operation, IWampFormatter<TMessage> formatter)
        {
            IWampRpcOperation<TMessage> casted = operation as IWampRpcOperation<TMessage>;

            if (casted != null)
            {
                return casted;
            }
            else
            {
                return new CastedRpcOperation<TMessage>(operation, formatter);
            }
        }
    }

    public interface IWampRpcOperationRegistrationToken : IDisposable
    {
        long RegistrationId { get; }
    }

    internal class Registration : IWampRpcOperation
    {
        private readonly string mProcedureUri;
        private readonly RegisterOptions mRegisterOptions;

        private IImmutableList<IWampRpcOperation> mOperations =
            ImmutableList.Create<IWampRpcOperation>();

        private readonly IWampRpcOperationSelector mSelector;
        private readonly object mLock = new object();

        public Registration(string procedureUri, RegisterOptions registerOptions)
        {
            mProcedureUri = procedureUri;
            mSelector = GetOperationSelector(registerOptions.Invoke);
            mRegisterOptions = registerOptions;
        }

        private static IWampRpcOperationSelector GetOperationSelector(string invocationPolicy)
        {
            switch (invocationPolicy)
            {
                case "single":
                case "first":
                    return new FirstOperationSelector();
                case "last":
                    return new LastOperationSelector();
                case "random":
                    return new RandomOperationSelector();
                case "roundrobin":
                    return new RoundrobinOperationSelector();
                default:
                    throw new WampException
                        ("wamp.invalid_options",
                         string.Format("invoke = {0} isn't supported", invocationPolicy));
            }
        }

        public long RegistrationId { get; set; }

        public event EventHandler Empty;

        public IWampRpcOperationRegistrationToken Register(IWampRpcOperation operation, RegisterOptions registerOptions)
        {
            VerifyInvokePoliciesAreCompatible(registerOptions);

            lock (mLock)
            {
                if (mRegisterOptions.Invoke != "single" || !mOperations.Any())
                {
                    if (!mOperations.Contains(operation))
                    {
                        mOperations = mOperations.Add(operation);                        
                    }

                    return new WampRpcOperationRegistrationToken(operation, this);
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
            lock (mLock)
            {
                IWampRpcOperation operation = GetOperation(caller);

                if (operation != null)
                {
                    operation.Invoke(caller, formatter, details);                    
                }
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     TMessage[] arguments)
        {
            lock (mLock)
            {
                IWampRpcOperation operation = GetOperation(caller);

                if (operation != null)
                {
                    operation.Invoke(caller, formatter, details, arguments);
                }
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            lock (mLock)
            {
                IWampRpcOperation operation = GetOperation(caller);

                if (operation != null)
                {
                    operation.Invoke(caller, formatter, details, arguments, argumentsKeywords);
                }
            }
        }

        private IWampRpcOperation GetOperation(IWampRawRpcOperationRouterCallback caller)
        {
            IWampRpcOperation result = mSelector.SelectOperation(mOperations);

            if (result == null)
            {
                caller.NoProcedureRegistered(Procedure);
            }

            return result;
        }

        private class WampRpcOperationRegistrationToken : IWampRpcOperationRegistrationToken
        {
            private readonly IWampRpcOperation mOperation;
            private readonly Registration mRegistration;

            public WampRpcOperationRegistrationToken(IWampRpcOperation operation, Registration registration)
            {
                mOperation = operation;
                mRegistration = registration;
            }

            public void Dispose()
            {
                mRegistration.RemoveOperation(mOperation);
            }

            public long RegistrationId
            {
                get
                {
                    return mRegistration.RegistrationId;
                }
            }
        }
    }

    internal interface IWampRpcOperationSelector
    {
        IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations);
    }

    internal class FirstOperationSelector : IWampRpcOperationSelector
    {
        public IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations)
        {
            return operations.FirstOrDefault();
        }
    }

    internal class LastOperationSelector : IWampRpcOperationSelector
    {
        public IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations)
        {
            return operations.LastOrDefault();
        }
    }

    internal class RandomOperationSelector : IWampRpcOperationSelector
    {
        private readonly Random mRandom = new Random();

        public IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations)
        {
            int index = mRandom.Next(operations.Count);

            return operations.ElementAtOrDefault(index);
        }
    }

    internal class RoundrobinOperationSelector : IWampRpcOperationSelector
    {
        private int mIndex = 0;

        public IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations)
        {
            int count = operations.Count;
            int index = mIndex%count;

            mIndex = (mIndex + 1)%count;

            return operations.ElementAtOrDefault(index);
        }
    }
}