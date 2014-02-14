using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampErrorCallback<TMessage>
    {
        [WampHandler(WampMessageType.v2Error)]
        void Error([WampProxyParameter]IWampClient client, int reqestType, long requestId, TMessage details, string error);

        [WampHandler(WampMessageType.v2Error)]
        void Error([WampProxyParameter]IWampClient client, int reqestType, long requestId, TMessage details, string error, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Error)]
        void Error([WampProxyParameter]IWampClient client, int reqestType, long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}