using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace WampSharp.Owin
{
    /// <summary>
    /// This sample requires Windows 8, .NET 4.5, and Microsoft.Owin.Host.HttpListener.
    /// </summary>
    public class StartupSample
    {
        // Run at startup
        public void Configuration(IAppBuilder app)
        {
            app.Use(UpgradeToWebSockets);
            app.UseWelcomePage();
        }

        // Run once per request
        private Task UpgradeToWebSockets(IOwinContext context, Func<Task> next)
        {
            Action<IDictionary<string, object>, Func<IDictionary<string, object>, Task>> accept = context.Get<Action<IDictionary<string, object>, Func<IDictionary<string, object>, Task>>>("websocket.Accept");

            if (accept == null)
            {
                // Not a websocket request
                return next();
            }

            accept(null, WebSocketEcho);

            return Task.FromResult<object>(null);
        }

        private async Task WebSocketEcho(IDictionary<string, object> websocketContext)
        {
            byte[] buffer = new byte[1024];

            WebSocketWrapper wrapper = new WebSocketWrapper(websocketContext);

            CancellationToken callCancelled = wrapper.CancellationToken;

            WebSocketReceiveResultStruct received = 
                await wrapper.ReceiveAsync(new ArraySegment<byte>(buffer), callCancelled);

            while (wrapper.ClientCloseStatus == null || wrapper.ClientCloseStatus == 0)
            {
                // Echo anything we receive
                await wrapper.SendAsync(new ArraySegment<byte>(buffer, 0, received.Count), received.MessageType, received.EndOfMessage, callCancelled);

                received = await wrapper.ReceiveAsync(new ArraySegment<byte>(buffer), callCancelled).ConfigureAwait(false);
            }

            await wrapper.CloseAsync(wrapper.ClientCloseStatus.Value,
                                     wrapper.ClientCloseDescription,
                                     callCancelled);
        }
    }
}