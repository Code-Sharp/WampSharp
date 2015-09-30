using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class ExactRpcOperationCatalog : MatchRpcOperationCatalog
    {
        public ExactRpcOperationCatalog(WampIdMapper<WampProcedureRegistration> mapper) :
            base(mapper)
        {
        }

        public override bool Handles(RegisterOptions options)
        {
            return options.Match == WampMatchPattern.Exact;
        }

        public override IWampRpcOperation GetMatchingOperation(string criteria)
        {
            return GetOperationByUri(criteria);
        }
    }
}