using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.Core.Contracts;

namespace WampSharp.Api
{
    interface IWampChannel
    {
        TProxy GetRpcClient<TProxy>() where TProxy : class;

        IWampServer GetServer<TMessage>(IWampClient<TMessage> client);
    }

    class WampChannel : IWampChannel
    {
        public TProxy GetRpcClient<TProxy>() where TProxy : class
        {
            throw new NotImplementedException();
        }

        public IWampServer GetServer<TMessage>(IWampClient<TMessage> client)
        {
            throw new NotImplementedException();
        }
    }
}
