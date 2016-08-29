using System;
using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    internal abstract class WampSubject : IWampSubject
    {
        private static readonly PublishOptions EmptyOptions  =
            new PublishOptions();

        public virtual void OnNext(IWampEvent value)
        {
            PublishOptions options = value.Options ?? EmptyOptions;
            object[] arguments = value.Arguments;
            IDictionary<string, object> argumentsKeywords = value.ArgumentsKeywords;

            if (argumentsKeywords != null)
            {
                Publish(options, arguments, argumentsKeywords);
            }
            else if (arguments != null)
            {
                Publish(options, arguments);
            }
            else
            {
                Publish(options);
            }
        }

        public virtual void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public virtual void OnCompleted()
        {
            throw new NotImplementedException();
        }

        protected abstract void Publish(PublishOptions options);
        protected abstract void Publish(PublishOptions options, object[] arguments);
        protected abstract void Publish(PublishOptions options, object[] arguments, IDictionary<string, object> argumentsKeywords);

        public abstract IDisposable Subscribe(IObserver<IWampSerializedEvent> observer);
    }
}