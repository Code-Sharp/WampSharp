using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// An abstract class for WampDetails/WampOptions types.
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class WampDetailsOptions
    {
        [NonSerialized]
        private ISerializedValue mOriginalValue;

        /// <summary>
        /// The original (serialized) received value.
        /// </summary>
        [IgnoreDataMember]
        public ISerializedValue OriginalValue
        {
            get => mOriginalValue;
            set => mOriginalValue = value;
        }
    }
}