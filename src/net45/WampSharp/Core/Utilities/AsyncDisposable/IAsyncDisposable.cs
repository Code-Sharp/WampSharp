using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace SystemEx
{
    public interface IAsyncDisposable
    {
        Task DisposeAsync();
    }
}