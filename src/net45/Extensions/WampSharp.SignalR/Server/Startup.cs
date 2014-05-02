using Microsoft.AspNet.SignalR;
using Owin;

namespace WampSharp.SignalR
{
    internal class Startup
    {
        private readonly ISignalRConnectionListenerAdapter mAdapter;

        public Startup(ISignalRConnectionListenerAdapter adapter)
        {
            mAdapter = adapter;
        }

        public void Configuration(IAppBuilder app)
        {
            DefaultDependencyResolver resolver = new DefaultDependencyResolver();

            resolver.Register(typeof(PersistentConnectionListener),
                () => new PersistentConnectionListener(this.mAdapter));

            app.MapSignalR<PersistentConnectionListener>
                ("", 
                 new ConnectionConfiguration() { Resolver = resolver });
        }
    }
}