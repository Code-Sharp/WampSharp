using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampCalleeError<TMessage>
    {
        [WampErrorHandler(WampMessageType.v2Register)]
        void RegisterError(long requestId, TMessage details, string error);

        [WampErrorHandler(WampMessageType.v2Register)]
        void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampErrorHandler(WampMessageType.v2Register)]
        void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);

        [WampErrorHandler(WampMessageType.v2Unregister)]
        void UnregisterError(long requestId, TMessage details, string error);

        [WampErrorHandler(WampMessageType.v2Unregister)]
        void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampErrorHandler(WampMessageType.v2Unregister)]
        void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}