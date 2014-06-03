using WampSharp.V1.Cra;

namespace WampSharp.V1.Core.Contracts
{
    /// <summary>
    /// Interface to data regarding a client that is authenticated (or being authenticated).
    /// </summary>
    public interface IWampCraClient
    {
        /// <summary>
        /// Interface to data regarding a client that is authenticated (or being authenticated).
        /// </summary>
        IWampCraAuthenticator CraAuthenticator { get; set; }
    }
}
