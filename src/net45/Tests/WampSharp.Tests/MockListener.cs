using System;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;

namespace WampSharp.Tests
{
    class MockListener<TMessage> : IWampConnectionListener<TMessage>, ISubject<IWampConnection<TMessage>>
    {
        private readonly Subject<IWampConnection<TMessage>> mSubject = new Subject<IWampConnection<TMessage>>();

        public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
        {
            return mSubject.Subscribe(observer);
        }

        public void OnNext(IWampConnection<TMessage> value)
        {
            mSubject.OnNext(value);

            // Yuck

            if (value is IControlledWampConnection<TMessage> casted)
            {
                casted.Connect();
            }
        }

        public void OnError(Exception error)
        {
            mSubject.OnError(error);
        }

        public void OnCompleted()
        {
            mSubject.OnCompleted();
        }
    }
}