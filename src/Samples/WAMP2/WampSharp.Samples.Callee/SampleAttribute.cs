using System;

namespace WampSharp.Samples.Callee
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
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