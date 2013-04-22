using WampSharp.Core.Contracts;

namespace WampSharp.Core.Listener
{
    public interface IWampClientBuilder<TConnection>
    {
        IWampClient Create(TConnection connection);
    }
}