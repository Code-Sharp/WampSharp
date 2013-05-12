using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V1
{
    public interface IWampAuxiliaryClient
    {
        [WampHandler(WampMessageType.v1Welcome)]
        void Welcome(string sessionId, int protocolVersion, string serverIdent);

        string SessionId { get; }
    }
}