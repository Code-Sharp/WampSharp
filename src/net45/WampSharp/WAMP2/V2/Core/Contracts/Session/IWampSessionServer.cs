using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Handles WAMP2 session server messages.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampSessionServer<TMessage>
    {
        [WampHandler(WampMessageType.v2Hello)]
        void Hello([WampProxyParameter] IWampSessionClient client, string realm, HelloDetails details);

        [WampHandler(WampMessageType.v2Abort)]
        void Abort([WampProxyParameter] IWampSessionClient client, AbortDetails details, string reason);

        [WampHandler(WampMessageType.v2Authenticate)]
        void Authenticate([WampProxyParameter]IWampSessionClient client, string signature, AuthenticateExtraData extra);

        [WampHandler(WampMessageType.v2Goodbye)]
        void Goodbye([WampProxyParameter] IWampSessionClient client, GoodbyeDetails details, string reason);

        #region Non-WAMP messages

        /// <summary>
        /// Occurs when a new client connects.
        /// </summary>
        /// <param name="client">The new connected client.</param>
        void OnNewClient(IWampClientProxy<TMessage> client);

        /// <summary>
        /// Occurs when a client disconnects.
        /// </summary>
        /// <param name="client">The disconnected client.</param>
        void OnClientDisconnect(IWampClientProxy<TMessage> client);

        #endregion
    }
}