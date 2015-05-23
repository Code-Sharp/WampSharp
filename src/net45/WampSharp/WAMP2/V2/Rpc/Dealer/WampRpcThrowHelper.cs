using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal static class WampRpcThrowHelper
    {
        public static void NoProcedureRegistered(string procedure)
        {
            string errorMessage = String.Format("no procedure '{0}' registered", procedure);

            throw new WampException(WampErrors.NoSuchProcedure, errorMessage);
        }
    }
}