using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.Rpc
{


    public interface IWampRpcClientFactory
    {
        TProxy GetClient<TProxy>() where TProxy : class;
    }
}