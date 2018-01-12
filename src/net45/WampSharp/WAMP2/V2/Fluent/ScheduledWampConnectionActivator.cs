using System.Reactive.Concurrency;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.V2.Fluent
{
    internal class ScheduledWampConnectionActivator : IWampConnectionActivator
    {
        private readonly IWampConnectionActivator mConnectionActivator;
        private readonly IScheduler mScheduler;

        public ScheduledWampConnectionActivator(IWampConnectionActivator connectionActivator, IScheduler scheduler)
        {
            mConnectionActivator = connectionActivator;
            mScheduler = scheduler;
        }

        public IControlledWampConnection<TMessage> Activate<TMessage>(IWampBinding<TMessage> binding)
        {
            IControlledWampConnection<TMessage> connection = mConnectionActivator.Activate(binding);
            ScheduledWampConnection<TMessage> result = new ScheduledWampConnection<TMessage>(connection, mScheduler);
            return result;
        }
    }
}