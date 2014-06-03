using System;
using System.Collections.Generic;

namespace WampSharp.V1.Cra
{
    /// <summary>
    /// This type is returned to the client from a call to IWampCra.authreq().  The client uses the
    /// JSON as sent in addition to the contents of authextra (if present) to sign the challenge.
    /// </summary>
    /// <remarks>As this is defined as part of WAMP-CRA (v1), it should not be changed.</remarks>
    public class WampCraChallenge
    {
        public WampCraChallenge(string authId, string authKey, DateTime timestamp, string sessionId, IDictionary<string, string> extra, WampCraPermissions permissions = null, IDictionary<string, string> authextra = null)
        {
            this.authid = authId;
            this.authkey = authKey;
            this.timestamp = timestamp;
            this.sessionid = sessionId;
            this.extra = extra;
            this.permissions = permissions ?? new WampCraPermissions();
            this.authextra = authextra;
        }

        public IDictionary<string, string> extra { get; private set; }
        public DateTime timestamp { get; private set; }
        public string authid { get; private set; }
        public string authkey { get; private set; }
        public string sessionid { get; private set; }
        public IDictionary<string, string> authextra { get; private set; }
        public WampCraPermissions permissions { get; private set; }
    }
}
