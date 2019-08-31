using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcOperationCatalog : IWampRpcOperationCatalog
    {
        private readonly MatchRpcOperationCatalog[] mInnerCatalogs;

        public WampRpcOperationCatalog()
        {
            // We're passing the mapper to the inner catalogs so ids
            // will be unique among all patterns.
            WampIdMapper<WampProcedureRegistration> mapper = 
                new WampIdMapper<WampProcedureRegistration>();
 
            mInnerCatalogs = new MatchRpcOperationCatalog[]
            {
                new ExactRpcOperationCatalog(mapper), 
                new PrefixRpcOperationCatalog(mapper),
                new WildcardRpcOperationCatalog(mapper)
            };

            foreach (MatchRpcOperationCatalog innerCatalog in mInnerCatalogs)
            {
                innerCatalog.RegistrationAdded += OnRegistrationAdded;
                innerCatalog.RegistrationRemoved += OnRegistrationRemoved;
            }
        }

        public IWampRegistrationSubscriptionToken Register(IWampRpcOperation operation, RegisterOptions options)
        {
            options = options.WithDefaults();

            MatchRpcOperationCatalog catalog = GetInnerCatalog(options);

            return catalog.Register(operation, options);
        }

        public event EventHandler<WampProcedureRegisterEventArgs> RegistrationAdded;

        public event EventHandler<WampProcedureRegisterEventArgs> RegistrationRemoved;

        public IWampRpcOperation GetMatchingOperation(string criteria)
        {
            foreach (MatchRpcOperationCatalog innerCatalog in mInnerCatalogs)
            {
                IWampRpcOperation operation = innerCatalog.GetMatchingOperation(criteria);

                if (operation != null)
                {
                    return operation;
                }
            }

            return null;
        }

        private MatchRpcOperationCatalog GetInnerCatalog(RegisterOptions options)
        {
            MatchRpcOperationCatalog result =
                mInnerCatalogs.FirstOrDefault
                    (innerCatalog => innerCatalog.Handles(options));

            if (result == null)
            {
                throw new WampException(WampErrors.InvalidOptions,
                                        "unknown match type: " + options.Match);
            }
            
            return result;
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure)
        {
            return InvokePattern(catalog => catalog.Invoke(caller, formatter, details, procedure), procedure);
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments)
        {
            return InvokePattern(catalog => catalog.Invoke(caller, formatter, details, procedure, arguments), procedure);
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            return InvokePattern(catalog => catalog.Invoke(caller, formatter, details, procedure, arguments, argumentsKeywords), procedure);
        }

        private IWampCancellableInvocation InvokePattern(Func<MatchRpcOperationCatalog, IWampCancellableInvocation> invokeAction, string procedure)
        {
            foreach (MatchRpcOperationCatalog catalog in mInnerCatalogs)
            {
                IWampCancellableInvocation result = invokeAction(catalog);

                if (result != null)
                {
                    return result;
                }
            }

            WampRpcThrowHelper.NoProcedureRegistered(procedure);

            return null;
        }

        private void OnRegistrationAdded(object sender, WampProcedureRegisterEventArgs e)
        {
            RaiseRegistrationAdded(e);
        }

        private void OnRegistrationRemoved(object sender, WampProcedureRegisterEventArgs e)
        {
            RaiseRegistrationRemoved(e);
        }

        private void RaiseRegistrationAdded(WampProcedureRegisterEventArgs e)
        {
            RegistrationAdded?.Invoke(this, e);
        }

        private void RaiseRegistrationRemoved(WampProcedureRegisterEventArgs e)
        {
            RegistrationRemoved?.Invoke(this, e);
        }
    }
}