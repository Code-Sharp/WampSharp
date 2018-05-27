using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Fleck;
using WampSharp.V2.MetaApi;

namespace WampSharp.Fleck
{
    [DataContract]
    internal class FleckTransportDetails : WampTransportDetails
    {
        private readonly IWebSocketConnectionInfo mConnectionInfo;

        public FleckTransportDetails(IWebSocketConnectionInfo connectionInfo)
        {
            this.Type = "fleck_websocket";

            mConnectionInfo = connectionInfo;

            Peer = string.Format("tcp4://{0}:{1}",
                                  mConnectionInfo.ClientIpAddress,
                                  mConnectionInfo.ClientPort);
        }

        [DataMember(Name = "peer")]
        public string Peer { get; }

        [DataMember(Name = "cookies")]
        public IDictionary<string, string> Cookies
        {
            get
            {
                return mConnectionInfo.Cookies;
            }
        }

        [DataMember(Name = "http_headers")]
        public IDictionary<string, string> Headers
        {
            get
            {
                return mConnectionInfo.Headers;
            }
        }

        [DataMember(Name = "fleck_connection_id")]
        public Guid Id
        {
            get
            {
                return mConnectionInfo.Id;
            }
        }

        [DataMember(Name = "protocol")]
        public string NegotiatedSubProtocol
        {
            get
            {
                return mConnectionInfo.NegotiatedSubProtocol;
            }
        }
    }
}