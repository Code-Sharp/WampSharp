using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WampSharp.Tests.TestHelpers
{
    public class MockRaw
    {
        public MockRaw(object value)
        {
            object[] rawArray = value as object[];

            if (value is MockRaw raw)
            {
                Value = Clone(raw.Value);
            }
            else if (rawArray != null && rawArray.GetType() == typeof(object[]))
            {
                Value = ConvertToMockRawArray(rawArray);
            }
            else
            {
                Value = Clone(value);
            }
        }


        private static object ConvertToMockRawArray(object[] value)
        {
            return value.Select(x => new MockRaw(x)).ToArray();
        }

        private static object Clone(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (TryClone(value, out object clone))
            {
                return clone;
            }
            else
            {
                // Anonymous type
                Type type = value.GetType();

                if (type.IsDefined(typeof(CompilerGeneratedAttribute), true))
                {
                    object[] properties =
                        type.GetProperties()
                            .Select(x => x.GetValue(value, null)).ToArray();

                    return Activator.CreateInstance(type, properties);
                }
            }

            return value;
        }

        private static bool TryClone(object value, out object result)
        {
            result = null;

            if (value is ICloneable cloneable)
            {
                result = cloneable.Clone();
                return true;
            }

            return false;
        }

        public object Value { get; }
    }
}