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


        public static async Task<IAsyncDisposable> ToAsyncDisposableTask(
            this IEnumerable<Task<IAsyncDisposable>> disposableTasks)
        {
            IAsyncDisposable[] allDisposables =
                await Task.WhenAll(disposableTasks);

            IAsyncDisposable result = new CompositeAsyncDisposable(allDisposables);

            return result;
        }


    }
}