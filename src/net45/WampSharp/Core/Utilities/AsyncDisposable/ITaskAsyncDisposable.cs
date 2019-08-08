using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SystemEx
{
    /// <summary>
    /// A Task based version of <see cref="IAsyncDisposable"/>.
    /// </summary>
    internal interface ITaskAsyncDisposable
    {
        /// <summary>
        /// <see cref="IDisposable.Dispose"/>
        /// </summary>
        /// <returns>A task that is finished when dispose is done.</returns>
        Task DisposeAsync();
    }
}