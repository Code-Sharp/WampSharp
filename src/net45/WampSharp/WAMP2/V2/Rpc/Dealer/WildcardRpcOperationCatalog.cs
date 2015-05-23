using System.Collections.Generic;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WildcardRpcOperationCatalog : MatchRpcOperationCatalog
    {
        private readonly IDictionary<string, WildCardMatcher> mWildCardToEvaluator =
            new SwapDictionary<string, WildCardMatcher>();

        public WildcardRpcOperationCatalog(WampIdMapper<ProcedureRegistration> mapper) : 
            base(mapper)
        {
        }

        public override bool Handles(RegisterOptions options)
        {
            return options.Match == "wildcard";
        }

        protected override void OnRegistrationAdded(string procedureUri)
        {
            mWildCardToEvaluator[procedureUri] = new WildCardMatcher(procedureUri);
        }

        protected override void OnRegistrationRemoved(string procedureUri)
        {
            mWildCardToEvaluator.Remove(procedureUri);
        }

        protected override IWampRpcOperation GetMatchingOperation(string criteria)
        {
            string[] uriParts = criteria.Split('.');

            // Finds the first wildcard that can match the criteria.
            // TODO: change the implementation to the "most selective wildcard",
            // TODO: when this is finally defined.
            foreach (var wildcardToEvaluator in mWildCardToEvaluator)
            {
                string wildcard = wildcardToEvaluator.Key;
                WildCardMatcher evaluator = wildcardToEvaluator.Value;

                if (evaluator.IsMatched(uriParts))
                {
                    return GetOperationByUri(wildcard);
                }
            }

            return null;
        }
    }
}