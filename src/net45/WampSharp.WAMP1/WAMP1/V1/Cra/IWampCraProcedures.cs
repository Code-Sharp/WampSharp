using System.Collections.Generic;
using WampSharp.V1.Rpc;

namespace WampSharp.V1.Cra
{
    /// <summary>
    /// Contract for v1 WAMP-CRA RPC Calls.
    /// </summary>
    public interface IWampCraProcedures
    {
        /// <summary>
        /// RPC endpoint for clients to initiate the authentication handshake.
        /// </summary>
        /// <param name="authKey">Authentication key, such as user name or application name.</param>
        /// <param name="extra">Extra data for salting the secret. Possible key values 'salt' (required,
        /// otherwise the secret is unsalted), 'iterations' (1000 default), and/or 'keylen' (32 default).</param>
        /// <returns>
        /// Authentication challenge. The client will need to create an authentication signature from
        /// this. The type WampCraChallenge can be deserialized from this.
        /// </returns>        
        [WampRpcMethod("http://api.wamp.ws/procedure#authreq")]
        string AuthReq(string authKey, IDictionary<string, string> extra);

        /// <summary>
        /// RPC endpoint for clients to actually authenticate after requesting authentication and
        /// computing a signature from the authentication challenge.
        /// </summary>
        /// <param name="signature">The signature.</param>
        /// <returns>
        /// A set of permissions the client is granted when authentication was successful.
        /// </returns>        
        [WampRpcMethod("http://api.wamp.ws/procedure#auth")]
        WampCraPermissions Auth(string signature);
    }
}
