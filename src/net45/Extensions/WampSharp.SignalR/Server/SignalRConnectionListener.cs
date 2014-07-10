using System;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Services;
using Microsoft.Owin.Hosting.Starter;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.SignalR
{
    public class SignalRConnectionListener<TMessage> : IWampConnectionListener<TMessage>, IDisposable
    {
        private readonly string mUrl;
        private readonly SignalRConnectionListenerAdapter<TMessage> mAdapter;
        private IDisposable mDisposable;

        public SignalRConnectionListener(string url, IWampTextBinding<TMessage> binding)
        {
            mUrl = url;
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

            var starter = services.GetService<IHostingStarter>();

            mDisposable = starter.Start(options);
        }

        public void Dispose()
        {
            mDisposable.Dispose();
        }
    }
}