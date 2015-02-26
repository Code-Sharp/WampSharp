using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace WampSharp.SignalR
{
    internal class Startup
    {
        private readonly ConnectionListenerSettings mSettings;
        private readonly ISignalRConnectionListenerAdapter mAdapter;

        public Startup(ConnectionListenerSettings settings, ISignalRConnectionListenerAdapter adapter)
        {
            mSettings = settings;
            mAdapter = adapter;
        }

        public void Configuration(IAppBuilder app)
        {
            DefaultDependencyResolver resolver = new DefaultDependencyResolver();

            resolver.Register(typeof (PersistentConnectionListener),
                () => new PersistentConnectionListener(this.mAdapter));

            ConnectionConfiguration connectionConfiguration =
                new ConnectionConfiguration()
                {
                    Resolver = resolver,
                };

            if (mSettings.EnableJSONP)
            {
                connectionConfiguration.EnableJSONP = true;
            }

            app.Map
                (mSettings.PathMatch,
                    map =>
                    {
                        if (mSettings.EnableCors)
                        {
                            map.UseCors(CorsOptions.AllowAll);
                        }

                        map.RunSignalR<PersistentConnectionListener>(connectionConfiguration);
                    });
        }
    }
}