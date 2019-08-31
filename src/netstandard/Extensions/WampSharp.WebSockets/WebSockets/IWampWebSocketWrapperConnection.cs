using System.Threading.Tasks;

namespace WampSharp.WebSockets
{
    public interface IWampWebSocketWrapperConnection
    {
        IClientWebSocketWrapper ClientWebSocket { get; }
        Task RunAsync();
    }
}