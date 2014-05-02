using System.Collections.Generic;

namespace WampSharp.V2.Rpc
{
    /// <summary>
    /// Indicates how to treat results of type <see cref="ICollection{T}"/>.
    /// </summary>
    public enum CollectionResultTreatment
    {
        /// <summary>
        /// Indicates that result of type <see cref="ICollection{T}"/> are treated as
        /// a single return value.
        /// </summary>
        SingleValue,
        /// <summary>
        /// Indicates that result of type <see cref="ICollection{T}"/> are treated as
        /// a multiple return value - i.e. as the arguments of the result.
        /// </summary>
        Multivalued
    }
}