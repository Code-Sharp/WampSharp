using System;

namespace WampSharp.Samples.Callee
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class SampleAttribute : Attribute
    {
        public SampleAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}