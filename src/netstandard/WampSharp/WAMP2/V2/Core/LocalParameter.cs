using System;
using System.Reflection;

namespace WampSharp.V2.Core
{
    public class LocalParameter
    {
        private readonly bool mHasDefaultValue;

        public LocalParameter(Type type, int position) :
            this(null, type, null, false, position)
        {
        }

        public LocalParameter(Type type, object defaultValue, int position) :
            this(null, type, defaultValue, true, position)
        {
        }

        public LocalParameter(string name, Type type, int position) :
            this(name, type, null, false, position)
        {
        }

        public LocalParameter(string name, Type type, object defaultValue, int position) :
            this(name, type, defaultValue, true, position)
        {
        }

        public LocalParameter(string name, Type type, object defaultValue, bool hasDefaultValue, int position)
        {
            Name = name;
            Type = type.StripByRef();
            mHasDefaultValue = hasDefaultValue;

            if (hasDefaultValue)
            {
                DefaultValue = defaultValue;
            }

            Position = position;
        }

        public LocalParameter(ParameterInfo parameter) :
            this(parameter.Name,
                parameter.ParameterType,
                parameter.DefaultValue,
                parameter.HasDefaultValue,
                parameter.Position)
        {
        }

        public string Name { get; }

        public Type Type { get; }

        public object DefaultValue { get; }

        public bool HasDefaultValue => mHasDefaultValue;

        public int Position { get; }
    }
}