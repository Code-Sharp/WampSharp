using System.Net;
using System.Runtime.Serialization;
using vtortola.WebSockets;
using WampSharp.V2.MetaApi;

namespace WampSharp.Vtortola
{
    [DataContract]
    internal class VtortolaTransportDetails : WampTransportDetails
    {
        private readonly string mRemoteEndpoint;
        private readonly VtortolaWebSocketHttpRequest mHttpRequest;
        private readonly VtortolaWebSocketHttpResponse mHttpResponse;

        public VtortolaTransportDetails(WebSocket websocket)
        {
            IPEndPoint remoteEndpoint = websocket.RemoteEndpoint;

            mRemoteEndpoint =
                string.Format("tcp4://{0}:{1}",
                              remoteEndpoint.Address,
                              remoteEndpoint.Port);

            mHttpRequest = new VtortolaWebSocketHttpRequest(websocket.HttpRequest);
            mHttpResponse = new VtortolaWebSocketHttpResponse(websocket.HttpResponse);
        }

        [DataMember(Name = "peer")]
        public string RemoteEndpoint
        {
            get
            {
                return mRemoteEndpoint;
            }
        }

        [DataMember(Name = "http_request")]
        public VtortolaWebSocketHttpRequest HttpRequest
        {
            get
            {
                return mHttpRequest;
            }
        }

        [DataMember(Name = "http_response")]
        public VtortolaWebSocketHttpResponse HttpResponse
        {
            get
            {
                return mHttpResponse;
            }
        }
    }
}