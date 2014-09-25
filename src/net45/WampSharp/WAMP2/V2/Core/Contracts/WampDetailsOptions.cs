namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// An abstract class for WampDetails/WampOptions types.
    /// </summary>
    public abstract class WampDetailsOptions
    {
        /// <summary>
        /// The original (serialized) received value.
        /// </summary>
        [IgnoreProperty]
        public ISerializedValue OriginalValue
        {
            get; 
            set;
        }
    }
}