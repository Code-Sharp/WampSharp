using System;
using System.Collections.Generic;

namespace WampSharp.V2
{
    internal abstract class WampSubject : IWampSubject
    {
        public virtual void OnNext(IWampEvent value)
        {
            IDictionary<string, object> details = value.Details;
            object[] arguments = value.Arguments;
            IDictionary<string, object> argumentsKeywords = value.ArgumentsKeywords;

            if (argumentsKeywords != null)
            {
                Publish(details, arguments, argumentsKeywords);
            }
            else if (arguments != null)
            {
                Publish(details, arguments);
            }
            else
            {
                Publish(details);
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

        protected abstract void Publish(IDictionary<string, object> options);
        protected abstract void Publish(IDictionary<string, object> options, object[] arguments);
        protected abstract void Publish(IDictionary<string, object> options, object[] arguments, IDictionary<string, object> argumentsKeywords);

        public abstract IDisposable Subscribe(IObserver<IWampSerializedEvent> observer);
    }
}