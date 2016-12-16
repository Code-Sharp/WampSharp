using Owin;
using WampSharp.Binding;
using WampSharp.V2;

namespace WampSharp.Owin
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            WampHost host = new WampHost();

            host.RegisterTransport(new OwinWebSocketTransport(app),
                                   new JTokenJsonBinding(),
                                   new JTokenMsgpackBinding());

            // Only serve files requested by name.
            app.UseStaticFiles("/complex");

            app.UseWelcomePage();

            host.Open();
        }
    }
}