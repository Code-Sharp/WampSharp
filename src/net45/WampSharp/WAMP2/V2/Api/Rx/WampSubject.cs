using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WampSharp.V2
{
    internal abstract class WampSubject : IWampSubject
    {
        private static readonly IDictionary<string, object> EmptyOptions  =
            new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());

        public virtual void OnNext(IWampEvent value)
        {
            IDictionary<string, object> options = value.Options ?? EmptyOptions;
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

        protected abstract void Publish(IDictionary<string, object> options);
        protected abstract void Publish(IDictionary<string, object> options, object[] arguments);
        protected abstract void Publish(IDictionary<string, object> options, object[] arguments, IDictionary<string, object> argumentsKeywords);

        public abstract IDisposable Subscribe(IObserver<IWampSerializedEvent> observer);
    }
}