using System.Reactive.Concurrency;

namespace WampSharp.V2.Fluent
{
    public static class WampChannelObserveOnSyntaxExtensions
    {
        public static ChannelFactorySyntax.IObserveOnSyntax ObserveOn
            (this ChannelFactorySyntax.ISerializationSyntax serializationSyntax, IScheduler scheduler)
        {
            return InnerObserveOn(serializationSyntax, scheduler);
        }

        public static ChannelFactorySyntax.IObserveOnSyntax ObserveOn
            (this ChannelFactorySyntax.IAuthenticationSyntax serializationSyntax, IScheduler scheduler)
        {
            return InnerObserveOn(serializationSyntax, scheduler);
        }

        private static ChannelFactorySyntax.IObserveOnSyntax InnerObserveOn<TSyntax>
            (this TSyntax serializationSyntax, IScheduler scheduler)
            where TSyntax : ChannelFactorySyntax.ISyntaxState, 
            ChannelFactorySyntax.IBuildableSyntax
        {
            ChannelState state = serializationSyntax.State;

            state.ConnectionActivator =
                new ScheduledWampConnectionActivator(state.ConnectionActivator, scheduler);

            return state;
        }
    }
}