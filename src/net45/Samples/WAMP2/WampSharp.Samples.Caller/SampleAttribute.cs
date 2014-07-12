using System;

namespace WampSharp.Samples.Caller
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class SampleAttribute : Attribute
    {
        private readonly string mName;

        public SampleAttribute(string name)
        {
            mName = name;
        }

        public string Name
        {
            get
            {
                return mName;
            }
        }
    }
}