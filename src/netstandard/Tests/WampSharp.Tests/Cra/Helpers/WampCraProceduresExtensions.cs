using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Cra;
using WampSharp.Core.Serialization;
using WampSharp.V1.Cra;

namespace WampSharp.Tests.Cra.Helpers
{
    internal static class WampCraProceduresExtensions
    {
        public static WampCraPermissions Authenticate(this IWampCraProcedures proxy, IWampFormatter<JToken> formatter, string authKey, IDictionary<string, string> authExtra, string authSecret)
        {
            string challenge = proxy.AuthReq(authKey, authExtra);
            
            if (string.IsNullOrEmpty(authKey))
            {
                return proxy.Auth(null);
            }

            WampCraChallenge info = formatter.Deserialize<WampCraChallenge>(JObject.Parse(challenge));
            
            string sig = WampCraHelpers.AuthSignature(challenge, authSecret, info.authextra);
            
            return proxy.Auth(sig);
        }     
    }
}