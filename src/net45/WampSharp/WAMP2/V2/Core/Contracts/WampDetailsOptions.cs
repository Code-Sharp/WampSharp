using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// An abstract class for WampDetails/WampOptions types.
    /// </summary>
    [DataContract]
    public abstract class WampDetailsOptions
    {
        /// <summary>
        /// The original (serialized) received value.
        /// </summary>
        [IgnoreDataMember]
        public ISerializedValue OriginalValue
        {
            get; 
            set;
        }
    }
}