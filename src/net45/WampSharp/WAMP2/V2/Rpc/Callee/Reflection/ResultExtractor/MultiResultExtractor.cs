using System.Collections.Generic;
using System.Linq;

namespace WampSharp.V2.Rpc
{
    internal class MultiResultExtractor : WampResultExtractor
    {
        public override object[] GetArguments(object result)
        {
            return GetFlattenResult((dynamic) result);
        }

        private object[] GetFlattenResult<T>(ICollection<T> result)
        {
            return result.Cast<object>().ToArray();
        }

        private object[] GetFlattenResult(object result)
        {
            return new object[] {result};
        }
    }
}