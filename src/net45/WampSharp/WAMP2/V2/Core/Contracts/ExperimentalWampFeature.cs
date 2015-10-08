using System;

namespace WampSharp.V2.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ExperimentalWampFeatureAttribute : Attribute
    {
    }
}