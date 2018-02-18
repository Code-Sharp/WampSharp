using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace WampSharp.V2
{
    internal class WampTupleTopicAsyncSubject<TTuple> : IAsyncSubject<TTuple>
    {
        private readonly IWampAsyncSubject mSubject;
        private readonly IWampEventValueTupleConverter<TTuple> mConverter;
        private readonly IAsyncObservable<TTuple> mObservable;

        public WampTupleTopicAsyncSubject(IWampAsyncSubject subject, IWampEventValueTupleConverter<TTuple> converter)
        {
            mSubject = subject;
            mConverter = converter;

            mObservable =
                mSubject.Select(x => mConverter.ToTuple(x));
        }

        public Task OnNextAsync(TTuple value)
        {
            IWampEvent wampEvent = mConverter.ToEvent(value);
            return mSubject.OnNextAsync(wampEvent);
        }

        public Task OnErrorAsync(Exception error)
        {
            return mSubject.OnErrorAsync(error);
        }

        public Task OnCompletedAsync()
        {
            return mSubject.OnCompletedAsync();
        }

        public Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<TTuple> observer)
        {
            return mObservable.SubscribeAsync(observer);
        }
    }
}