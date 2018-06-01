using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcServices
{
    public class LongValueTuplesService
    {
        [WampProcedure("com.myapp.get_long_positional_tuple")]
        [return: WampResult(CollectionResultTreatment.Multivalued)]
        public object[] GetLongTuple(string name)
        {
            object[] result =
                Enumerable.Range(0, 10).Select(i => $"{name} {i}")
                          .Concat(new object[] {name.Length})
                          .ToArray();

            return result;
        }

        public class KeywordTupleOperation : SyncLocalRpcOperation
        {
            public KeywordTupleOperation() : base("com.myapp.get_long_keyword_tuple")
            {
            }

            public override RpcParameter[] Parameters => new RpcParameter[] {new RpcParameter(typeof(string), 0) };

            public override bool HasResult => false;

            public override CollectionResultTreatment CollectionResultTreatment => CollectionResultTreatment.SingleValue;

            protected override object InvokeSync<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
                                                           InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords,
                                                           out IDictionary<string, object> outputs)
            {
                object[] unpacked =
                    this.UnpackParameters(formatter, arguments, argumentsKeywords)
                    .ToArray();

                string name = (string) unpacked[0];

                outputs = new Dictionary<string, object>();

                outputs["length"] = name.Length;

                for (int i = 0; i < 10; i++)
                {
                    string argumentName = "item" + (i + 1);
                    outputs[argumentName] = $"{name} {i}";
                }

                return null;
            }
        }
    }
}