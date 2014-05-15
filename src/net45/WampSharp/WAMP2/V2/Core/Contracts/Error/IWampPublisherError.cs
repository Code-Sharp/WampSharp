using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampPublisherError<TMessage>
    {
        [WampErrorHandler(WampMessageType.v2Publish)]
        void PublishError(long requestId, TMessage details, string error);

        [WampErrorHandler(WampMessageType.v2Publish)]
        void PublishError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampErrorHandler(WampMessageType.v2Publish)]
        void PublishError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);         
    }
}