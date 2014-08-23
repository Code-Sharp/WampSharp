using System;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class SingleValueSyncCallback : SyncCallback
    {
        public SingleValueSyncCallback(MethodInfoHelper methodInfoHelper, object[] arguments)
            : base(methodInfoHelper, arguments)
        {
        }

        protected override void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            if (!mMethodInfoHelper.Method.HasReturnValue() || !arguments.Any())
            {
                SetResult(null);
            }
            else
            {
                Type deserializedType = mMethodInfoHelper.Method.ReturnType;
                object deserialized = formatter.Deserialize(deserializedType, arguments[0]);
                SetResult(deserialized);
            }
        }
    }
}