using System;
using System.Collections.Generic;
using System.Threading;
using WampSharp.Core.Cra;
using WampSharp.Core.Serialization;
using WampSharp.V1.Core.Listener;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.Rpc;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1.Cra
{
    /// <summary>
    /// WAMP-CRA is a cryptographically strong challenge response authentication protocol based on
    /// HMAC-SHA256. The protocol performs in-band authentication of WAMP clients to WAMP servers.
    /// WAMP-CRA does not introduce any new WAMP protocol level message types, but implements the
    /// authentication handshake via standard WAMP RPCs with well-known procedure URIs and signatures.
    /// </summary>
    /// <seealso cref="T:WampSharp.V1.Cra.IWampCraProcedures"/>
    /// <seealso cref="T:WampSharp.V1.Cra.IWampCraAuthenticator"/>
    public abstract class WampCraAuthenticator<TMessage> : IWampCraProcedures, IWampCraAuthenticator
    {

        #region Fields
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly IWampTopicContainer mTopicContainer;
        private WampCraPendingAuth mPendingAuth;
        private readonly IWampRpcMetadataCatalog mMetadataCatalog;
        private static readonly IWampSessionIdGenerator mIdGenerator = new WampSessionIdGenerator();

        #endregion

        #region Constructor

        /// <summary>
        /// Specialized constructor for use only by derived classes.
        /// </summary>
        /// <param name="clientSessionId">Identifier for the client session.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="metadataCatalog">The metadata catalog.</param>
        /// <param name="topicContainer">The topic container.</param>
        protected WampCraAuthenticator(string clientSessionId, IWampFormatter<TMessage> formatter,
            IWampRpcMetadataCatalog metadataCatalog, IWampTopicContainer topicContainer)
        {
            ClientSessionId = clientSessionId;
            mFormatter = formatter;
            mMetadataCatalog = metadataCatalog;
            mTopicContainer = topicContainer;
            CraPermissionsMapper = new WampCraPermissionsMapper();
        }

        #endregion

        #region IWampCraAuthenticator implementation

        /// <summary>
        /// The authKey provided by the client during the WAMP-CRA authentication request.
        /// </summary>
        /// <seealso cref="P:WampSharp.V1.Cra.IWampCraAuthenticator.AuthKey"/>
        public string AuthKey { get; private set; }

        /// <summary>
        /// Gets the sessionId of the connected client.
        /// </summary>
        /// <seealso cref="P:WampSharp.V1.Cra.IWampCraAuthenticator.ClientSessionId"/>
        public string ClientSessionId { get; }

        /// <summary>
        /// Gets a value indicating whether the user identified by AuthKey is successfully authenticated.
        /// </summary>
        /// <seealso cref="P:WampSharp.V1.Cra.IWampCraAuthenticator.IsAuthenticated"/>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Gets a value indicating whether we are waiting for an auth call after an authreq.
        /// </summary>
        public bool IsAuthenticationPending => (mPendingAuth != null);

        /// <summary>
        /// Gets the permissions that were sent to the client following a successful authreq call.
        /// </summary>
        public WampCraPermissionsMapper CraPermissionsMapper { get; }

        /// <summary>
        /// Gets all RPC methods in this collection.
        /// </summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process all RPC methods in this collection.
        /// </returns>
        public IEnumerable<IWampRpcMethod> GetAllRpcMethods()
        {
            return mMetadataCatalog.GetAllRpcMethods();
        }

        /// <summary>
        /// Gets topic container.
        /// </summary>
        /// <returns>The topic container.</returns>
        public IWampTopicContainer GetTopicContainer()
        {
            return mTopicContainer;
        }

        #endregion

        #region IWampCraProcedures implementation

        /// <summary>
        /// RPC endpoint for clients to initiate the authentication handshake.
        /// </summary>
        /// <seealso cref="M:WampSharp.Cra.IWampCraProcedures.AuthReq(string,IDictionary{string,string})"/>
        public string AuthReq(string authKey, IDictionary<string, string> extra)
        {
            ValidateAuthReqStatus(authKey);

            string authSecret = GetAuthReqSecret(authKey);

            // each authentication request gets a unique authid, which can only be used (later) once!
            string authid = mIdGenerator.Generate();

            //check extra
            if (extra == null)
            {
                extra = new Dictionary<string, string>();
            }

            Dictionary<string, string> extraAuth = new Dictionary<string, string>(extra);

            WampCraPermissions permissions = GetAuthReqPermissions(authKey, extraAuth);

            WampCraChallenge info =
                new WampCraChallenge(authid, authKey, DateTime.UtcNow, ClientSessionId, extra, permissions, extraAuth);

            AuthKey = authKey;

            if (string.IsNullOrEmpty(authKey))
            {
                // anonymous session
                mPendingAuth = new WampCraPendingAuth(info, null, permissions);
                return null;
            }

            // authenticated session
            string infoser = mFormatter.Serialize(info).ToString();
            string sig = WampCraHelpers.AuthSignature(infoser, authSecret, info.authextra);
            mPendingAuth = new WampCraPendingAuth(info, sig, permissions);
            return infoser;
        }

        private void ValidateAuthReqStatus(string authKey)
        {
            // check authentication state
            if (IsAuthenticated)
            {
                ThrowHelper.AlreadyAuthenticated();
            }

            if (IsAuthenticationPending)
            {
                ThrowHelper.AuthenticationAlreadyRequested();
            }

            // check authKey
            if (string.IsNullOrEmpty(authKey) && !AllowAnonymous)
            {
                ThrowHelper.AuthenticationAsAnonymousForbidden();
            }
        }

        private string GetAuthReqSecret(string authKey)
        {
            string authSecret = null;
            if (!string.IsNullOrEmpty(authKey))
            {
                //if we are anon, no need to get authSecret
                try
                {
                    authSecret = GetAuthSecret(authKey);

                    if (string.IsNullOrEmpty(authSecret))
                    {
                        ThrowHelper.AuthenticationKeyDoesNotExist(authKey);
                    }
                }
                catch (WampRpcCallException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    ThrowHelper.AuthenticationKeyDoesNotExist(authKey, e);
                }
            }
            return authSecret;
        }

        private WampCraPermissions GetAuthReqPermissions(string authKey, Dictionary<string, string> extraAuth)
        {
            WampCraPermissions permissions;

            try
            {
                permissions = GetAuthPermissions(authKey, extraAuth);
            }
            catch (WampRpcCallException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new WampRpcCallException("http://api.wamp.ws/error#auth-permissions-error", e.Message, null);
            }

            if (permissions == null)
            {
                permissions = new WampCraPermissions();
            }

            return permissions;
        }

        /// <summary>
        /// RPC endpoint for clients to actually authenticate after requesting authentication and
        /// computing a signature from the authentication challenge.
        /// </summary>
        /// <seealso cref="M:WampSharp.Cra.IWampCraProcedures.Auth(string)"/>
        public WampCraPermissions Auth(string signature)
        {
            ValidateAuthState();

            CheckSignature(signature);
            // At this point, the client has successfully authenticated!

            // Get the permissions we determined earlier
            WampCraPermissions perms = mPendingAuth.Permissions;

            // Delete auth request and mark client as authenticated
            string authKey = mPendingAuth.AuthInfo.authkey;
            IsAuthenticated = true;
            mPendingAuth = null;

            // TODO: If implementing a pending auth timeout, here is where you would cancel it 
            // TODO: (_clientAuthTimeoutCall in autobahn python)

            RaiseOnAuthenticated(authKey, perms);

            // Return permissions to client
            CraPermissionsMapper.AddPermissions(perms);
            return perms;
        }

        private void ValidateAuthState()
        {
            // Check authentication state
            if (IsAuthenticated)
            {
                ThrowHelper.AlreadyAuthenticated();
            }
            if (!IsAuthenticationPending)
            {
                ThrowHelper.NoAuthenticationRequested();
            }
        }

        private void CheckSignature(string signature)
        {
            // Check signature
            if (!string.Equals(mPendingAuth.Signature, signature, StringComparison.Ordinal))
            {
                // Delete pending authentication, so that no retries are possible. authid is only valid for 1 try!!
                mPendingAuth = null;

                // Notify the client of failed authentication, but only after a random,
                // exponentially distributed delay. this (further) protects against
                // timing attacks
                Random random = new Random();
                double randNum = random.Next(1, 101)/100.0d;
                double failDelaySecs = Math.Log(1.0d - randNum)/((1.0d/0.8d)*-1.0d); //mean = 0.8 secs
                Thread.Sleep(Convert.ToInt32(failDelaySecs*1000.0d));
                ThrowHelper.InvalidSignature();
            }
        }

        private void RaiseOnAuthenticated(string authKey, WampCraPermissions perms)
        {
            // Fire authentication callback
            try
            {
                OnAuthenticated(authKey, perms);
            }
            catch (WampRpcCallException)
            {
                IsAuthenticated = false;
                throw;
            }
            catch (Exception e)
            {
                IsAuthenticated = false;
                ThrowHelper.InvalidSignature(e);
            }
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Gets a value indicating whether we allow anonymous logins. An anonymous login is one where
        /// the authKey is null or an empty string.
        /// </summary>
        public abstract bool AllowAnonymous { get; }

        /// <summary>
        /// Gets the secret value for the supplied auth key used to hash the challenge.  Return null if
        /// the key is not found.
        /// </summary>
        /// <param name="authKey">Authentication key, such as a user name or application name.</param>
        /// <returns>The authentication secret.</returns>
        public abstract string GetAuthSecret(string authKey);

        /// <summary>
        /// Called to get the permissions the user identified by @authKey BEFORE the user is
        /// authenticated. A copy of the client's request 'extra' is provided, and you can alter it here.
        /// This will be passed back to the client as WampCraChallenge.extraAuth and MUST be what is
        /// actually used for generating the secret.
        /// </summary>
        /// <remarks>
        /// Here is a good place to check/fix something in @extra that is out of whack with the request,
        /// like capping the number of iterations, or specifying a different (stricter, presumably)
        /// salting mechanism. I really don't know or understand the purpose of sending available
        /// endpoints to an unauthenticated user.  These are only used as part of the challenge, and are
        /// regurgitated to you in OnAuthenticated. I see no reason why you have to return anything here
        /// (yes, null is fine), and you can do all the work in OnAuthenticated(), including returning
        /// the real list of permissions.
        /// </remarks>
        /// <param name="authKey">Authentication key, such as user name or application name.</param>
        /// <param name="extra">Extra data for salting the secret. Possible key values 'salt' (required,
        /// otherwise the secret is unsalted), 'iterations' (1000 default), and/or 'keylen' (32 default).
        /// You may change these here and the client must use this method of signing.</param>
        /// <returns>The permissions of the user if successfully authenticated.</returns>
        public abstract WampCraPermissions GetAuthPermissions(string authKey, IDictionary<string, string> extra);

        /// <summary>
        /// Called after the user is successfully authenticated.  This call is the last opportunity for
        /// you to set the WampPermissions, as these will be used to determine rpc and pub/sub operations
        /// for the remainder of this session.
        /// </summary>
        /// <remarks>
        /// NOTE: Throwing an exception here could put you in a bad state.  We'll send an error to the
        /// client, and make them unauthenticated in this case.
        /// </remarks>
        /// <param name="authKey">Authentication key, such as user name or application name.</param>
        /// <param name="permissions">The permissions initially granted in the call to
        /// GetAuthPermissions(). You could change these, and the updated copy is what will be returned
        /// to the client from its call to auth().</param>
        public abstract void OnAuthenticated(string authKey, WampCraPermissions permissions);

        #endregion

        #region ThrowHelper

        private static class ThrowHelper
        {
            public static void AuthenticationKeyDoesNotExist(string authKey, Exception exception = null)
            {
                string errorDesc = $"authentication key '{authKey}' does not exist.";

                if (exception != null)
                {
                    errorDesc = $"{errorDesc} {exception.Message}";
                }

                throw new WampRpcCallException("http://api.wamp.ws/error#no-such-authkey",
                    errorDesc, null);
            }

            public static void AlreadyAuthenticated()
            {
                throw new WampRpcCallException("http://api.wamp.ws/error#already-authenticated", "already authenticated",
                    null);
            }

            public static void AuthenticationAsAnonymousForbidden()
            {
                throw new WampRpcCallException("http://api.wamp.ws/error#anonymous-auth-forbidden",
                    "authentication as anonymous forbidden", null);
            }

            public static void AuthenticationAlreadyRequested()
            {
                throw new WampRpcCallException("http://api.wamp.ws/error#authentication-already-requested",
                    "authentication request already issues - authentication pending", null);
            }

            public static void NoAuthenticationRequested()
            {
                throw new WampRpcCallException("http://api.wamp.ws/error#no-authentication-requested",
                    "no authentication previously requested", null);
            }

            public static void InvalidSignature()
            {
                throw new WampRpcCallException("http://api.wamp.ws/error#invalid-signature",
                    "signature for authentication request is invalid", null);
            }

            public static void InvalidSignature(Exception e)
            {
                throw new WampRpcCallException("http://api.wamp.ws/error#invalid-signature",
                                               $"internal error: {e.Message}", null);
            }
        }

        #endregion
    }
}