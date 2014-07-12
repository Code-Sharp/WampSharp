namespace WampSharp.V2.CalleeProxy
{
    internal interface IWampCalleeProxyFactory
    {
        TProxy GetProxy<TProxy>() where TProxy : class;
    }
}