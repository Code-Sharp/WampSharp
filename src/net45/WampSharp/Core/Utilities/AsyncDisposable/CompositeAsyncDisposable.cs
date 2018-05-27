using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SystemEx
{
    internal class CompositeAsyncDisposable : IAsyncDisposable
    {
        private readonly IList<IAsyncDisposable> mDisposables;

        public CompositeAsyncDisposable(IList<IAsyncDisposable> disposables)
        {
            mDisposables = disposables;
        }

        public Task DisposeAsync()
        {
            List<Task> tasks = new List<Task>();

            foreach (IAsyncDisposable disposable in mDisposables)
            {
                Task disposeTask = disposable.DisposeAsync();
                tasks.Add(disposeTask);
            }

#if !NET40
            Task result = Task.WhenAll(tasks);
#else
            IObservable<Unit> whenAll = 
                from currentTask in tasks.ToObservable()
                from unit in currentTask.ToObservable()
                select unit;

            Task result = whenAll.ToTask();
#endif

            return result;
        }
    }
}