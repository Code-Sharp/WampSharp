using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V1
{
    /// <summary>
    /// Represents the auxiliary methods of a WAMP client.
    /// </summary>
    public interface IWampAuxiliaryClient
    {
        /// <summary>
        /// Server-to-client WELCOME message.
        /// </summary>
        /// <param name="sessionId">The client's session id.</param>
        /// <param name="protocolVersion">The WAMP protocol version the server speaks.</param>
        /// <param name="serverIdent">A string the server may use to disclose it's version, software, platform or identity.</param>
        [WampHandler(WampMessageType.v1Welcome)]
        void Welcome(string sessionId, int protocolVersion, string serverIdent);

        /// <summary>
        /// Gets the given WAMP client's session id.
        /// </summary>
        string SessionId { get; }
    }
}