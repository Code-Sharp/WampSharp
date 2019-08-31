using System.Collections.Generic;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.V2
{
    internal class WampTransportDefinition
    {
        public WampTransportDefinition()
        {
            Bindings = new List<IWampBinding>();
        }

        public IWampTransport Transport { get; set; }
        public ICollection<IWampBinding> Bindings { get; set; }

    }
}