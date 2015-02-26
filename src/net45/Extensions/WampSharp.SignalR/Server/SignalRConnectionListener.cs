using System;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Services;
using Microsoft.Owin.Hosting.Starter;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.SignalR
{
    public class SignalRConnectionListener<TMessage> : IWampConnectionListener<TMessage>, IDisposable,
        ISignalRListener
    {
        private readonly string mUrl;
        private readonly SignalRConnectionListenerAdapter<TMessage> mAdapter;
        private IDisposable mDisposable;
        private readonly ConnectionListenerSettings mSettings;

        public SignalRConnectionListener(string url, IWampTextBinding<TMessage> binding,
            bool enableCors = true, bool enableJSONP = true, string pathMatch = "")
        {
            mUrl = url;

            mSettings = new ConnectionListenerSettings
            {
                PathMatch = pathMatch,
                EnableCors = enableCors, 
                EnableJSONP = enableJSONP
            };

            mAdapter = new SignalRConnectionListenerAdapter<TMessage>(binding);
        }

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
        {
            return mAdapter.Subscribe(observer);
        }

        public void Open()
        {
            var services = (ServiceProvider) ServicesFactory.Create();
            var options = new StartOptions(mUrl);

            services.Add(typeof (ISignalRConnectionListenerAdapter),
                         () => mAdapter);

            services.Add(typeof(ConnectionListenerSettings),
                         () => mSettings);

            var starter = services.GetService<IHostingStarter>();

            mDisposable = starter.Start(options);
        }

        public void Dispose()
        {
            mDisposable.Dispose();
        }
    }
}