using System;
using System.Threading.Tasks;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Rpc.Server
{
    /// <summary>
    /// Represents a hosted rpc service's method.
    /// </summary>
    public interface IWampRpcMethod
    {
        /// <summary>
        /// The method's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The method's proc uri.
        /// </summary>
        string ProcUri { get; }

        /// <summary>
        /// Invokes the method asynchronously.
        /// </summary>
        /// <param name="client">The <see cref="IWampClient"/> making the call.</param>
        /// <param name="parameters">The parameters to invoke with.</param>
        /// <returns>A task representing the result.</returns>
        Task<object> InvokeAsync(IWampClient client, object[] parameters);

        /// <summary>
        /// The types of the method parameters.
        /// </summary>
        Type[] Parameters { get; }

        /// <summary>
        /// Invokes the method syncronously.
        /// </summary>
        /// <param name="client">The <see cref="IWampClient"/> making the call. </param>
        /// <param name="parameters">The parameters to invoke with.</param>
        /// <returns>The result of the method.</returns>
        object Invoke(IWampClient client, object[] parameters);
    }
}