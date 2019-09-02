using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    internal class WampCraPendingClientDetails
    {
        private const string UTCFormat = "yyyy-MM-ddTHH:mm:ssK";

        public string AuthenticationRole { get; set; }

        public string AuthenticationProvider { get; set; }

        public string AuthenticationMethod => WampAuthenticationMethods.WampCra;

        public string AuthenticationId { get; set; }

        public long SessionId { get; set; }

        public string TimeStamp
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
                $@"{{""nonce"": ""{Nonce}"", ""authprovider"": ""{AuthenticationProvider}"", ""authid"": ""{
                        AuthenticationId
                    }"", ""timestamp"": ""{TimeStamp}"", ""authrole"": ""{AuthenticationRole}"", ""authmethod"": ""{
                        AuthenticationMethod
                    }"", ""session"": {SessionId}}}";
        }
    }
}