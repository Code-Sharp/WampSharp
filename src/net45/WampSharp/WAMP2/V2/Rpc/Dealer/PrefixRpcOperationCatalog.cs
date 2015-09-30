using System.Linq;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class PrefixRpcOperationCatalog : MatchRpcOperationCatalog
    {
        public PrefixRpcOperationCatalog(WampIdMapper<WampProcedureRegistration> mapper) : 
            base(mapper)
        {
        }

        public override bool Handles(RegisterOptions options)
        {
            return options.Match == WampMatchPattern.Prefix;
        }

        public override IWampRpcOperation GetMatchingOperation(string criteria)
        {
            IWampRpcOperation currentOperation = null;
            int currentLength = 0;

            foreach (IWampRpcOperation operation in 
                this.Operations.Where(x => criteria.StartsWith(x.Procedure)))
            {
                string currentProcedure = operation.Procedure;

                if (currentProcedure.Length > currentLength)
                {
                    currentLength = currentProcedure.Length;
                    currentOperation = operation;
                }
            }

            return currentOperation;
        }
    }
}