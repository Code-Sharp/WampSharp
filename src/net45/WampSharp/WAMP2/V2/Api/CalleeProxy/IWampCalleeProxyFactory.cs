using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    internal interface IWampCalleeProxyFactory
    {
        TProxy GetProxy<TProxy>(CallOptions callOptions) where TProxy : class;
    }
}