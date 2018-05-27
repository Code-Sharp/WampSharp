using System.Net;
using System.Runtime.Serialization;
using vtortola.WebSockets;
using WampSharp.V2.MetaApi;

namespace WampSharp.Vtortola
{
    [DataContract]
    internal class VtortolaTransportDetails : WampTransportDetails
    {
        private readonly VtortolaWebSocketHttpResponse mHttpResponse;

        public VtortolaTransportDetails(WebSocket websocket)
        {
            IPEndPoint remoteEndpoint = websocket.RemoteEndpoint;

            RemoteEndpoint =
                $"tcp4://{remoteEndpoint.Address}:{remoteEndpoint.Port}";

            HttpRequest = new VtortolaWebSocketHttpRequest(websocket.HttpRequest);
            mHttpResponse = new VtortolaWebSocketHttpResponse(websocket.HttpResponse);
        }

        [DataMember(Name = "peer")]
        public string RemoteEndpoint { get; }

        [DataMember(Name = "http_request")]
        public VtortolaWebSocketHttpRequest HttpRequest { get; }

        [DataMember(Name = "http_response")]
        public VtortolaWebSocketHttpResponse HttpResponse => mHttpResponse;
    }
}