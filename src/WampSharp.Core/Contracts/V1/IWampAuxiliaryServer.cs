using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V1
{
    public interface IWampAuxiliaryServer
    {
        [WampHandler(WampMessageType.v1Prefix)]
        void Prefix([WampProxyParameter]IWampClient client, string prefix, string uri);         
    }
}