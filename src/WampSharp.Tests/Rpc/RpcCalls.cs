using System.Collections.Generic;
using System.Linq;
using WampSharp.Rpc;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Rpc
{
    public static class RpcCalls
    {
        public static IEnumerable<object[]> SomeCalls
        {
            get
            {
                return Messages.CallMessages
                               .Select(x => new[] {GetRpcCall(x.Arguments)});
            }
        }

        private static WampRpcCall<object> GetRpcCall(MockRaw[] arguments)
        {
            string callId = (string) arguments[0].Value;

            string procUri = (string) arguments[1].Value;

            object[] methodArguments =
                arguments.Skip(2).Select(x => x.Value).ToArray();

            return new WampRpcCall<object>()
                       {
                           Arguments = methodArguments,
                           ProcUri = procUri,
                           ReturnType = typeof (object) // Not sure how we'll do it.
                       };
        }
    }
}