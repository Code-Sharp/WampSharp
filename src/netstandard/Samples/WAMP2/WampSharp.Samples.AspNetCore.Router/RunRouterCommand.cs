using System.IO;
using System.Net;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using WampSharp.AspNetCore.RawSocket;
using WampSharp.AspNetCore.WebSockets.Server;
using WampSharp.Binding;
using WampSharp.V2;

namespace WampSharp.Samples.AspNetCore.Router
{
    [Command("run", Description = "Runs a WAMP router, powered by ASP.NET Core.")]
    public class RunRouterCommand : ICommand
    {
        [CommandOption("port", 'p', Description = "The port to listen on for requests. Default value is 8080.", IsRequired = false)]
        public int Port { get; set; } = 8080;

        public async Task ExecuteAsync(IConsole console)
        {
            WampHost wampHost = new WampHost();

            JTokenJsonBinding jsonBinding = new JTokenJsonBinding();
            JTokenCborBinding cborBinding = new JTokenCborBinding();
            JTokenMessagePackBinding messagePackBinding = new JTokenMessagePackBinding();

            IWebHost host =
                WebHost.CreateDefaultBuilder()
                       .UseKestrel(options =>
                                   {
                                       options.Listen(IPAddress.Loopback, Port,
                                                      builder =>
                                                      {
                                                          // Log all of the http bytes as they are sent and received
                                                          builder.UseConnectionLogging();

                                                          // Configure RawSocket transport
                                                          wampHost
                                                              .RegisterTransport(new AspNetCoreRawSocketTransport(builder),
                                                                                 jsonBinding,
                                                                                 cborBinding,
                                                                                 messagePackBinding);
                                                      });
                                   })
                       .Configure(app =>
                                  {
                                      app.Map("/ws",
                                              builder =>
                                              {
                                                  builder.UseWebSockets();

                                                  // Configure WebSockets transport
                                                  wampHost
                                                      .RegisterTransport(new AspNetCoreWebSocketTransport(builder, null),
                                                                         jsonBinding,
                                                                         cborBinding,
                                                                         messagePackBinding);
                                              });

                                      app.UseStaticFiles(new StaticFileOptions
                                                         {
                                                             FileProvider =
                                                                 new
                                                                     PhysicalFileProvider(Path
                                                                                              .Combine(Directory.GetCurrentDirectory(),
                                                                                                       "static")),
                                                             RequestPath = ""
                                                         });

                                      app.UseDirectoryBrowser(new DirectoryBrowserOptions
                                                              {
                                                                  FileProvider =
                                                                      new
                                                                          PhysicalFileProvider(Path
                                                                                                   .Combine(Directory.GetCurrentDirectory(),
                                                                                                            "static")),
                                                                  RequestPath = ""
                                                              });

                                      wampHost.Open();
                                  })
                       .Build();

            await host.RunAsync().ConfigureAwait(false);
        }
    }
}