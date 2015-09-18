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
            this.Type = "fleck-websocket";
            mConnectionInfo = connectionInfo;
        }

        public string SubProtocol
        {
            get
            {
                return mConnectionInfo.SubProtocol;
            }
        }

        public string Origin
        {
            get
            {
                return mConnectionInfo.Origin;
            }
        }

        public string Host
        {
            get
            {
                return mConnectionInfo.Host;
            }
        }

        public string Path
        {
            get
            {
                return mConnectionInfo.Path;
            }
        }

        public string ClientIpAddress
        {
            get
            {
                return mConnectionInfo.ClientIpAddress;
            }
        }

        public int ClientPort
        {
            get
            {
                return mConnectionInfo.ClientPort;
            }
        }

        public IDictionary<string, string> Cookies
        {
            get
            {
                return mConnectionInfo.Cookies;
            }
        }

        public IDictionary<string, string> Headers
        {
            get
            {
                return mConnectionInfo.Headers;
            }
        }

        public Guid Id
        {
            get
            {
                return mConnectionInfo.Id;
            }
        }

        public string NegotiatedSubProtocol
        {
            get
            {
                return mConnectionInfo.NegotiatedSubProtocol;
            }
        }
    }
}