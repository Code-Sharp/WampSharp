using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampCalleeError<TMessage>
    {
        [WampHandler(WampMessageType.v2Register)]
        void RegisterError(long requestId, TMessage details, string error);

        [WampHandler(WampMessageType.v2Register)]
        void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Register)]
        void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);

        [WampHandler(WampMessageType.v2Unregister)]
        void UnregisterError(long requestId, TMessage details, string error);

        [WampHandler(WampMessageType.v2Unregister)]
        void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Unregister)]
        void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}