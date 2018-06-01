using System;
using System.Collections.Generic;

namespace WampSharp.V2.Rpc
{
    [AttributeUsage(AttributeTargets.ReturnValue, Inherited = false, AllowMultiple = false)]
    public sealed class WampResultAttribute : Attribute
    {

        /// <summary>
        /// Creates a new instance of <see cref="WampResultAttribute"/>.
        /// </summary>
        /// <param name="collectionResultTreatment">A value indicating how to treat results of type
        /// <see cref="ICollection{T}"/>.</param>
        public WampResultAttribute(CollectionResultTreatment collectionResultTreatment)
        {
            CollectionResultTreatment = collectionResultTreatment;
        }

        public CollectionResultTreatment CollectionResultTreatment { get; } = CollectionResultTreatment.Multivalued;
    }
}