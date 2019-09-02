using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SystemEx
{
    internal class CompositeAsyncDisposable : ITaskAsyncDisposable, IAsyncDisposable
    {
        private readonly IList<ITaskAsyncDisposable> mDisposables;

        public CompositeAsyncDisposable(IList<IAsyncDisposable> disposables)
        {
            mDisposables =
                disposables.Select(x => ConvertToTaskAsyncDisposable(x))
                           .ToList();
        }

        private ITaskAsyncDisposable ConvertToTaskAsyncDisposable(IAsyncDisposable asyncDisposable)
        {
            if (asyncDisposable is ITaskAsyncDisposable taskAsyncDisposable)
            {
                return taskAsyncDisposable;
            }
            else
            {
                return new TaskAsyncDisposableWrapper(asyncDisposable);
            }
        }

        public Task DisposeAsync()
        {
            List<Task> tasks = new List<Task>();

            foreach (ITaskAsyncDisposable disposable in mDisposables)
            {
                Task disposeTask = disposable.DisposeAsync();
                tasks.Add(disposeTask);
            }

            Task result = Task.WhenAll(tasks);

            return result;
        }

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            Task result = this.DisposeAsync();
            return new ValueTask(result);
        }

        private class TaskAsyncDisposableWrapper : ITaskAsyncDisposable
        {
            private readonly IAsyncDisposable mAsyncDisposable;

            public TaskAsyncDisposableWrapper(IAsyncDisposable asyncDisposable)
            {
                mAsyncDisposable = asyncDisposable;
            }

            public Task DisposeAsync()
            {
                return mAsyncDisposable.DisposeAsync().AsTask();
            }
        }
    }
}