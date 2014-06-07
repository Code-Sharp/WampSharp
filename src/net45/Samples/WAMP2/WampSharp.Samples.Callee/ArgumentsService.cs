using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    [Sample("arguments")]
    public class ArgumentsService
    {
        [WampProcedure("com.arguments.ping")]
        public void Ping()
        {
        }

        [WampProcedure("com.arguments.add2")]
        public int Add2(int a, int b)
        {
            return a + b;
        }

        [WampProcedure("com.arguments.stars")]
        public string Stars(string nick = "somebody", int stars = 0)
        {
            return string.Format("{0} starred {1}x", nick, stars);
        }

        [WampProcedure("com.arguments.orders")]
        public string[] Orders(string product, int limit = 5)
        {
            return Enumerable.Range(0, 50).Take(limit).Select(i => string.Format("Product {0}", i)).ToArray();
        }

        public class ArgLenOperation : SyncLocalRpcOperation
        {
            private readonly RpcParameter[] mParameters = new RpcParameter[0];

            public ArgLenOperation()
                : base("com.arguments.arglen")
            {
            }

            protected override object InvokeSync<TMessage>(IWampRawRpcOperationCallback caller,
                                                           IWampFormatter<TMessage> formatter, TMessage options,
                                                           TMessage[] arguments,
                                                           IDictionary<string, TMessage> argumentsKeywords,
                                                           out IDictionary<string, object> outputs)
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

            public override RpcParameter[] Parameters
            {
                get { return mParameters; }
            }

            public override bool HasResult
            {
                get { return true; }
            }

            public override CollectionResultTreatment CollectionResultTreatment
            {
                get
                {
                    return CollectionResultTreatment.Multivalued;
                }
            }
        }
    }
}