namespace WampSharp.Core.Dispatch.Handler
{
    public interface IMethodBuilder<TKey, TMethod>
    {
        TMethod BuildMethod(TKey key);
    }
}