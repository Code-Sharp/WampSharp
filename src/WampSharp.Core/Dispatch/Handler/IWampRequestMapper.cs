namespace WampSharp.Core.Dispatch.Handler
{
    public interface IWampRequestMapper<TRequest>
    {
        WampMethodInfo Map(TRequest request);
    }
}