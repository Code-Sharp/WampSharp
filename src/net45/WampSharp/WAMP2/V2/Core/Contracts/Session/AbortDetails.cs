namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details of an ABORT message.
    /// </summary>
    public class AbortDetails : WampDetailsOptions
    {
        /// <summary>
        /// The message sent upon the ABORT message.
        /// </summary>
        [PropertyName("message")]
        public string Message { get; set; }
    }
}