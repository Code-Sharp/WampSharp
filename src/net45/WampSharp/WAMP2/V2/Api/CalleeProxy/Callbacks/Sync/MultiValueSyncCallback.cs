using System;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class MultiValueSyncCallback : SyncCallback
    {
        public MultiValueSyncCallback(MethodInfoHelper methodInfoHelper, object[] arguments)
            : base(methodInfoHelper, arguments)
        {
        }

        protected override void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            if (!arguments.Any())
            {
                SetResult(null);
            }
            else
            {
                Type arrayElementType = mMethodInfoHelper.Method.ReturnType.GetElementType();

                Array result = Array.CreateInstance(arrayElementType, arguments.Length);

                for (int i = 0; i < arguments.Length; i++)
                {
                    TMessage current = arguments[i];
                    var currentDeserialized = formatter.Deserialize(arrayElementType, current);
                    result.SetValue(currentDeserialized, i);
                }

                SetResult(result);
            }
        }
    }
}