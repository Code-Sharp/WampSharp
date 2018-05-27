using System;

namespace WampSharp.Samples.Caller
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class SampleAttribute : Attribute
    {
        public SampleAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}