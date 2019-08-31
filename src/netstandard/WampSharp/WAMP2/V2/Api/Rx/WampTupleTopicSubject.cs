using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WampSharp.V2
{
    internal class WampTupleTopicSubject<TTuple> : ISubject<TTuple>
    {
        private readonly IWampSubject mSubject;
        private readonly IWampEventValueTupleConverter<TTuple> mConverter;
        private readonly IObservable<TTuple> mObservable;

        public WampTupleTopicSubject(IWampSubject subject, IWampEventValueTupleConverter<TTuple> converter)
        {
            mSubject = subject;
            mConverter = converter;

            mObservable = 
                mSubject.Select(x => mConverter.ToTuple(x));
        }

        public void OnNext(TTuple value)
        {
            IWampEvent wampEvent = mConverter.ToEvent(value);
            mSubject.OnNext(wampEvent);
        }

        public void OnError(Exception error)
        {
            mSubject.OnError(error);
        }

        public void OnCompleted()
        {
            mSubject.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<TTuple> observer)
        {
            return mObservable.Subscribe(observer);
        }
    }
}