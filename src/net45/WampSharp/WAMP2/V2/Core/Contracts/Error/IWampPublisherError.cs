using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampPublisherError<TMessage>
    {
        [WampHandler(WampMessageType.v2Publish)]
        void PublishError(long requestId, TMessage details, string error);

        [WampHandler(WampMessageType.v2Publish)]
        void PublishError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Publish)]
        void PublishError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);         
    }
}