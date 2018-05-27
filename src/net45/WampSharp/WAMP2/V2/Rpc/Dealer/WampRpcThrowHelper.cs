using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal static class WampRpcThrowHelper
    {
        public static void NoProcedureRegistered(string procedure)
        {
            string errorMessage = $"no procedure '{procedure}' registered";

            throw new WampException(WampErrors.NoSuchProcedure, errorMessage);
        }
    }
}