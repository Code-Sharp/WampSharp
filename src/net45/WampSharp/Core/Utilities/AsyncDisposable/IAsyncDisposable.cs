#if !ASYNC_DISPOSABLE
using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SystemEx
{
    /// <summary>
    /// Represents a <see cref="IDisposable"/> which its <see cref="IDisposable.Dispose"/>
    /// method is async.
    /// </summary>
    public interface IAsyncDisposable
    {
        /// <summary>
        /// <see cref="IDisposable.Dispose"/>
        /// </summary>
        /// <returns>A task that is finished when dispose is done.</returns>
        ValueTask DisposeAsync();
    }
}
#endif