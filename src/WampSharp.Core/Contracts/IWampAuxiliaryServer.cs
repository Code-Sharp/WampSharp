using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    public interface IWampAuxiliaryServer
    {
        [WampHandler(WampMessageType.Prefix)]
        void Prefix(IWampClient client, string prefix, string uri);         
    }
}