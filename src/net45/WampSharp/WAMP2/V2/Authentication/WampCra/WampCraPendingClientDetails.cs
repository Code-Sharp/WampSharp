#if WAMPCRA
using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    internal class WampCraPendingClientDetails
    {
        private const string UTCFormat = "yyyy-MM-ddTHH:mm:ssK";

        public string AuthenticationRole { get; set; }

        public string AuthenticationProvider { get; set; }

        public string AuthenticationMethod
        {
            get { return WampAuthenticationMethods.WampCra; }
        }

        public string AuthenticationId { get; set; }

        public long SessionId { get; set; }

        public string Timestamp
        {
            get
            {
                DateTime date = DateTime.UtcNow;
                return date.ToString(UTCFormat);
            }
        }

        public string Nonce
        {
            get
            {
                byte[] byteArray = Guid.NewGuid().ToByteArray();
                return Convert.ToBase64String(byteArray);
            }
        }

        public override string ToString()
        {
            return
                string.Format(
                    @"{{""nonce"": ""{0}"", ""authprovider"": ""{1}"", ""authid"": ""{2}"", ""timestamp"": ""{3}"", ""authrole"": ""{4}"", ""authmethod"": ""{5}"", ""session"": {6}}}",
                    Nonce,
                    AuthenticationProvider,
                    AuthenticationId,
                    Timestamp,
                    AuthenticationRole,
                    AuthenticationMethod,
                    SessionId);
        }
    }
}

#endif