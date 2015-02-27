using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace WampSharp
{
    /// <summary>
    /// A workaround until tpl dataflow is compatible with mono.
    /// see http://stackoverflow.com/questions/28708400/actionblock-framework-4-rx-alternative
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    internal class ActionBlock<TInput>
    {
        private readonly Subject<TInput> mSubject = new Subject<TInput>();
        private readonly Task mCompletion;

        public ActionBlock(Func<TInput, Task> action)
        {
            mCompletion =
                mSubject.Select(x => Observable.FromAsync(() => action(x)))
                    .Concat()
                    .Count()
                    .ToTask();
        }

        public Task Completion
        {
            get
            {
                return mCompletion;
            }
        }

        public void Post(TInput item)
        {
            mSubject.OnNext(item);
        }

        public void Complete()
        {
            mSubject.OnCompleted();
        }
    }
}