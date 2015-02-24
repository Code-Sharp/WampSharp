using System;
using System.Linq;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.CalleeProxy
{
    internal class SingleValueExtractor : IOperationResultExtractor
    {
        private readonly Type mReturnType;
        private readonly bool mHasReturnValue;

        public SingleValueExtractor(Type returnType, bool hasReturnValue)
        {
            mReturnType = returnType;
            mHasReturnValue = hasReturnValue;
        }

        public object GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            if (!mHasReturnValue || !arguments.Any())
            {
                return null;
            }
            else
            {
                object result = formatter.Deserialize(mReturnType, arguments[0]);
                return result;
            }
        }
    }
}