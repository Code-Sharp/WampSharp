using System.IO.Pipelines;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using WampSharp.RawSocket;

namespace WampSharp.AspNetCore.RawSocket
{
    public class SocketData
    {
        public ConnectionContext ConnectionContext { get; }
        public Handshake Handshake { get; }
        public Handshake HandshakeResponse { get; }

        public Task ReadTask { get; internal set; }
        public PipeReader Reader { get; }

        public SocketData(ConnectionContext connectionContext, Handshake handshake, Handshake handshakeResponse, PipeReader reader)
        {
            ConnectionContext = connectionContext;
            Handshake = handshake;
            HandshakeResponse = handshakeResponse;
            Reader = reader;
        }
    }
}