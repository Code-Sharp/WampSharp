using System;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Curie;

namespace WampSharp.Auxiliary
{
    public class WampAuxiliaryServer : IWampAuxiliaryServer
    {
        public void Prefix(IWampClient client, string prefix, string uri)
        {
            IWampCurieMapper mapper = client as IWampCurieMapper;

            mapper.Map(prefix, uri);
        }
    }
}