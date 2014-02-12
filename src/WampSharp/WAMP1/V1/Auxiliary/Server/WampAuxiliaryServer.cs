using WampSharp.V1.Core.Contracts.V1;
using WampSharp.V1.Core.Curie;

namespace WampSharp.V1.Auxiliary.Server
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