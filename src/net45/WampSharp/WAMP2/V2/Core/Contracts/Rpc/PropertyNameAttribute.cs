using System;

namespace WampSharp.V2.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class PropertyNameAttribute : Attribute
    {
        private readonly string mPropertyName;

        public PropertyNameAttribute(string propertyName)
        {
            mPropertyName = propertyName;
        }

        public string PropertyName
        {
            get { return mPropertyName; }
        }
    }
}