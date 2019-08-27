using System.Collections.Generic;
using System.Linq;
using CliFx.Attributes;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    [Command("arguments")]
    public class ArgumentsCommand : CalleeCommand<ArgumentsService>
    {
    }

    public interface IArgumentsService
    {
        [WampProcedure("com.arguments.ping")]
        void Ping();

        [WampProcedure("com.arguments.add2")]
        int Add2(int a, int b);

        [WampProcedure("com.arguments.stars")]
        string Stars(string nick = "somebody", int stars = 0);

        [WampProcedure("com.arguments.orders")]
        string[] Orders(string product, int limit = 5);
    }

    public class ArgumentsService : IArgumentsService
    {
        public void Ping()
        {
        }

        public int Add2(int a, int b)
        {
            return a + b;
        }

        public string Stars(string nick = "somebody", int stars = 0)
        {
            return $"{nick} starred {stars}x";
        }

        public string[] Orders(string product, int limit = 5)
        {
            return Enumerable.Range(0, 50).Take(limit).Select(i => $"Product {i}").ToArray();
        }
    }

    public class ArgLenOperation : SyncLocalRpcOperation
    {
        private readonly RpcParameter[] mParameters = new RpcParameter[0];

        public ArgLenOperation()
            : base("com.arguments.arglen")
        {
        }

        protected override object InvokeSync<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords, out IDictionary<string, object> outputs)
        {
            outputs = null;

            int argumentsLength = 0;

            if (arguments != null)
            {
                argumentsLength = arguments.Length;
            }

            int argumentKeyWordsLength = 0;

            if (argumentsKeywords != null)
            {
                argumentKeyWordsLength = argumentsKeywords.Count;
            }

            return new int[] { argumentsLength, argumentKeyWordsLength };
        }

        public override RpcParameter[] Parameters => mParameters;

        public override bool HasResult => true;

        public override CollectionResultTreatment CollectionResultTreatment => CollectionResultTreatment.Multivalued;
    }
}