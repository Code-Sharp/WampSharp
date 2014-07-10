using System.Collections.Generic;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.V2
{
    public interface IWampTransportDefinition
    {
        IWampTransport Transport { get; }
        ICollection<IWampBinding> Bindings { get; }
    }

    public class WampTransportDefinition : IWampTransportDefinition
    {
        public WampTransportDefinition()
        {
            Bindings = new List<IWampBinding>();
        }

        public IWampTransport Transport { get; set; }
        public ICollection<IWampBinding> Bindings { get; set; }

    }
}