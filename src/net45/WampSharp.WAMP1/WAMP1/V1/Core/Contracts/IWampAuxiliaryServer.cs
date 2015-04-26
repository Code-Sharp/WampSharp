using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V1.Core.Contracts
{
    /// <summary>
    /// Represents the auxiliary methods of a WAMP server.
    /// </summary>
    public interface IWampAuxiliaryServer
    {
        /// <summary>
        /// A prefix message sent by a client in order to map curies.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="uri">The full uri.</param>
        [WampHandler(WampMessageType.v1Prefix)]
        void Prefix([WampProxyParameter]IWampClient client, string prefix, string uri);         
    }
}