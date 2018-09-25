using System.Threading.Tasks;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents a WAMP client session.
    /// </summary>
    public interface IWampChannel
    {
        /// <summary>
        /// Gets a proxy of the realm, this session is connected to.
        /// </summary>
        IWampRealmProxy RealmProxy { get; }
        
        /// <summary>
        /// Tries to establish a connection to the WAMP router.
        /// </summary>
        /// <returns>A task that is complete when the connection is
        /// established.</returns>
        Task Open();

        /// <summary>
        /// Closes the session violently.
        /// </summary>
        void Close();

        /// <summary>
        /// Closes the session.
        /// </summary>
        /// <param name="reason">The close reason.</param>
        /// <param name="details">The close details.</param>
        /// <returns>A task that completes upon a GOODBYE message from the router.</returns>
        Task<GoodbyeMessage> Close(string reason, GoodbyeDetails details);
    }
}