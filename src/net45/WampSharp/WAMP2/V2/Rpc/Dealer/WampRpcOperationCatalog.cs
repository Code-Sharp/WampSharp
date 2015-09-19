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
            WampIdMapper<ProcedureRegistration> mapper = 
                new WampIdMapper<ProcedureRegistration>();
 
            mInnerCatalogs = new MatchRpcOperationCatalog[]
            {
                new ExactRpcOperationCatalog(mapper), 
                new PrefixRpcOperationCatalog(mapper),
                new WildcardRpcOperationCatalog(mapper)
            };
        }

        public IWampRegistrationSubscriptionToken Register(IWampRpcOperation operation, RegisterOptions options)
        {
            MatchRpcOperationCatalog catalog = GetInnerCatalog(options);

            return catalog.Register(operation, options);
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

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure)
        {
            InvokePattern(catalog => catalog.Invoke(caller, formatter, details, procedure), procedure);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments)
        {
            InvokePattern(catalog => catalog.Invoke(caller, formatter, details, procedure, arguments), procedure);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     InvocationDetails details,
                                     string procedure,
                                     TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            InvokePattern(catalog => catalog.Invoke(caller, formatter, details, procedure, arguments, argumentsKeywords), procedure);
        }

        private void InvokePattern(Func<MatchRpcOperationCatalog, bool> invokeAction, string procedure)
        {
            bool invoked = false;

            foreach (MatchRpcOperationCatalog catalog in mInnerCatalogs)
            {
                if (invokeAction(catalog))
                {
                    invoked = true;
                    break;
                }
            }

            if (!invoked)
            {
                WampRpcThrowHelper.NoProcedureRegistered(procedure);
            }
        }
    }
}