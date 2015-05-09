using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal static class WampRawRpcOperationRouterCallbackExtensions
    {
        private static readonly Dictionary<string, object> mEmptyDetails = new Dictionary<string, object>();

        private static readonly IWampFormatter<object> ObjectFormatter = WampObjectFormatter.Value;

        public static void NoProcedureRegistered(this IWampRawRpcOperationRouterCallback caller, string procedure)
        {
            string errorMessage = String.Format("no procedure '{0}' registered", procedure);

            caller.Error(ObjectFormatter, mEmptyDetails,
                         WampErrors.NoSuchProcedure,
                         new object[] { errorMessage });
        }
    }
}