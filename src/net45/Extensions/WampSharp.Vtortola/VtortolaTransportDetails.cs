using vtortola.WebSockets;
using WampSharp.V2.MetaApi;

namespace WampSharp.Vtortola
{
    internal class VtortolaTransportDetails : WampTransportDetails
    {
        private readonly string mRemoteEndpoint;
        private readonly VtortolaWebSocketHttpRequest mHttpRequest;
        private readonly VtortolaWebSocketHttpResponse mHttpResponse;

        public VtortolaTransportDetails(WebSocket websocket)
        {
            mRemoteEndpoint = websocket.RemoteEndpoint.Address.ToString();
            mHttpRequest = new VtortolaWebSocketHttpRequest(websocket.HttpRequest);
            mHttpResponse = new VtortolaWebSocketHttpResponse(websocket.HttpResponse);
        }

        public string RemoteEndpoint
        {
            get
            {
                return mRemoteEndpoint;
            }
        }

        public VtortolaWebSocketHttpRequest HttpRequest
        {
            get
            {
                return mHttpRequest;
            }
        }

        public VtortolaWebSocketHttpResponse HttpResponse
        {
            get
            {
                return mHttpResponse;
            }
        }
    }
}