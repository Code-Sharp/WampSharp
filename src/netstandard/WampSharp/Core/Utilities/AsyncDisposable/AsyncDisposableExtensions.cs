using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SystemEx
{
    internal static class AsyncDisposableExtensions
    {

#if ASYNC
        public static async Task<IAsyncDisposable> ToAsyncDisposableTask(
            this IEnumerable<Task<IAsyncDisposable>> disposableTasks)
        {
            IAsyncDisposable[] allDisposables =
                await Task.WhenAll(disposableTasks).ConfigureAwait(false);

            IAsyncDisposable result = new CompositeAsyncDisposable(allDisposables);

            return result;
        }
#else
        public static Task<IAsyncDisposable> ToAsyncDisposableTask(this IEnumerable<Task<IAsyncDisposable>> disposableTasks)
        {
            IObservable<IAsyncDisposable> tasksAsObservables =
                from task in disposableTasks.ToObservable()
                from asyncDisposable in task
                select asyncDisposable;

            var merged =
                tasksAsObservables.ToList();

            var observableResult =
                merged.Select(x => new CompositeAsyncDisposable(x))
                    .Cast<IAsyncDisposable>();

            Task<IAsyncDisposable> result =
                observableResult.ToTask();

            return result;
        }
#endif

    }
}