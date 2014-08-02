using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.V2
{
    /// <summary>
    /// Extension methods for <see cref="IWampHost"/>.
    /// </summary>
    public static class WampHostExtensions
    {
        /// <summary>
        /// Registers a given transport for a given host.
        /// </summary>
        /// <param name="host">The given host.</param>
        /// <param name="transport">The given transport to register.</param>
        /// <param name="binding">The given bindings to activate support with the given transport.</param>
        public static void RegisterTransport(this IWampHost host, IWampTransport transport,
                                             params IWampBinding[] binding)
        {
            host.RegisterTransport(transport, binding);
        }
    }
}