using System;
using System.Collections.Generic;

namespace WampSharp.V2.Rpc
{
    [AttributeUsage(AttributeTargets.ReturnValue, Inherited = false, AllowMultiple = false)]
    public sealed class WampResultAttribute : Attribute
    {
        private readonly CollectionResultTreatment mCollectionResultTreatment = CollectionResultTreatment.Multivalued;

        /// <summary>
        /// Creates a new instance of <see cref="WampResultAttribute"/>.
        /// </summary>
        /// <param name="collectionResultTreatment">A value indicating how to treat results of type
        /// <see cref="ICollection{T}"/>.</param>
        public WampResultAttribute(CollectionResultTreatment collectionResultTreatment)
        {
            mCollectionResultTreatment = collectionResultTreatment;
        }

        public CollectionResultTreatment CollectionResultTreatment
        {
            get
            {
                return mCollectionResultTreatment;
            }
        }
    }
}