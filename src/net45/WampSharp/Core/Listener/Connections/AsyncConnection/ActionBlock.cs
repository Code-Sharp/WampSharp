using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using WampSharp.Core.Utilities;

namespace WampSharp.Core.Listener
{
#if TPL
    internal class ActionBlock<TInput>
    {
        private readonly System.Threading.Tasks.Dataflow.ActionBlock<TInput> mBlock;

        public ActionBlock(Func<TInput, Task> action)
        {
            mBlock = new System.Threading.Tasks.Dataflow.ActionBlock<TInput>(action);
        }

        public void Complete()
        {
            mBlock.Complete();
        }

        public bool Post(TInput item)
        {
            return mBlock.Post(item);
        }

        public Task Completion => mBlock.Completion;
    }
#else

    /// <summary>
    /// A workaround until tpl dataflow is compatible with mono.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    internal class ActionBlock<TInput>
    {
        private readonly Subject<TInput> mSubject = new Subject<TInput>();
        private readonly Task mCompletion;

        public ActionBlock(Func<TInput, Task> action)
        {
            mCompletion =
                mSubject
                    .ToAsyncEnumerable()
                    .SelectMany(x => action(x)
                                    .ContinueWithNull()
                                    .ToAsyncEnumerable())
                    .Count();
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

#endif
    }