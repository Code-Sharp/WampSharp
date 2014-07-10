namespace WampSharp.V2.Binding.Transports
{
    public interface IWampWebSocketTransport :
        IWampTransport<string>,
        IWampTransport<byte[]>
    {
    }
}