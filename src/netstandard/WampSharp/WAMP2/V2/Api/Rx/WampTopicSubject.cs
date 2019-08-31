using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WampSharp.V2
{
    internal class WampTopicSubject<T> : ISubject<T>
    {
        private readonly IWampSubject mSubject;
        private readonly IObservable<T> mObservable;

        public WampTopicSubject(IWampSubject subject)
        {
            mSubject = subject;

            mObservable =
                subject.Where(x => x.Arguments != null && x.Arguments.Any())
                       .Select(x => x.Arguments[0].Deserialize<T>());
        }

        public void OnNext(T value)
        {
            mSubject.OnNext(new WampEvent {Arguments = new object[] {value}});
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return mObservable.Subscribe(observer);
        }
    }
}