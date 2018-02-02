using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    internal abstract class WampAsyncSubject : IWampAsyncSubject
    {
        private static readonly PublishOptions EmptyOptions  =
            new PublishOptions();

        public virtual Task OnNextAsync(IWampEvent value)
        {
            PublishOptions options = value.Options ?? EmptyOptions;
            object[] arguments = value.Arguments;
            IDictionary<string, object> argumentsKeywords = value.ArgumentsKeywords;

            Task result;

            if (argumentsKeywords != null)
            {
                result = Publish(options, arguments, argumentsKeywords);
            }
            else if (arguments != null)
            {
                result = Publish(options, arguments);
            }
            else
            {
                result = Publish(options);
            }

            return result;
        }

        public virtual Task OnErrorAsync(Exception error)
        {
            throw new NotImplementedException();
        }

        public virtual Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        protected abstract Task Publish(PublishOptions options);
        protected abstract Task Publish(PublishOptions options, object[] arguments);
        protected abstract Task Publish(PublishOptions options, object[] arguments,
            IDictionary<string, object> argumentsKeywords);

        public abstract Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<IWampSerializedEvent> observer);
    }
}