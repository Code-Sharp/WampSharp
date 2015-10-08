namespace WampSharp.Core.Cra
{
    /// <summary>
    /// Represents details of a WAMP-CRA challenge.
    /// </summary>
    public interface IWampCraChallenge
    {
        /// <summary>
        /// Gets the salt to use in order to compute the signature, sent to the user that upon CHALLENGE
        /// message. (in challenge.extra)
        /// </summary>
        string Salt { get; }

        /// <summary>
        /// Gets the number of iterations to use in order to compute the signature, 
        /// sent to the user that upon CHALLENGE
        /// message. (in challenge.extra)
        /// </summary>
        int? Iterations { get; }
 
        /// <summary>
        /// Gets the keylength to use in order to compute the signature, 
        /// sent to the user that upon CHALLENGE
        /// message. (in challenge.extra)
        /// </summary>
        int? KeyLength { get; }
    }
}