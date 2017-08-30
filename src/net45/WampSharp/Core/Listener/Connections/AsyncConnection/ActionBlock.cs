using System;
using System.Threading.Tasks;

namespace WampSharp.Core.Listener
{
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

        public Task Completion
        {
            get { return mBlock.Completion; }
        }
    }

    }