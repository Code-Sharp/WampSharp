using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace WampSharp.V2
{
    internal class WampTopicAsyncSubject<T> : IAsyncSubject<T>
    {
        private readonly IWampAsyncSubject mSubject;
        private readonly IAsyncObservable<T> mObservable;

        public WampTopicAsyncSubject(IWampAsyncSubject subject)
        {
            mSubject = subject;

            mObservable =
                subject.Where(x => x.Arguments != null && x.Arguments.Any())
                       .Select(x => x.Arguments[0].Deserialize<T>());
        }

        public Task OnNextAsync(T value)
        {
            return mSubject.OnNextAsync(new WampEvent {Arguments = new object[] {value}});
        }

        public Task OnErrorAsync(Exception error)
        {
            throw new NotImplementedException();
        }

        public Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            return mObservable.SubscribeAsync(observer);
        }
    }
}