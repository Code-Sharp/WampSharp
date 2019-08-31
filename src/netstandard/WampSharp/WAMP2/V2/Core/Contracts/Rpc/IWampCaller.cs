using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampCaller : IWampCaller<object>, IWampError<object>
    {
    }

    public interface IWampCaller<TMessage>
    {
        [WampHandler(WampMessageType.v2Result)]
        void Result(long requestId, ResultDetails details);

        [WampHandler(WampMessageType.v2Result)]
        void Result(long requestId, ResultDetails details, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Result)]
        void Result(long requestId, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}