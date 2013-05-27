using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WampSharp.Tests.TestHelpers
{
    public class MockRaw
    {
        private readonly object mValue;

        public MockRaw(object value)
        {
            MockRaw raw = value as MockRaw;
            
            if (raw != null)
            {
                mValue = Clone(raw.Value);
            }
            else
            {
                mValue = Clone(value);                
            }
        }

        public object Clone(object value)
        {
            if (value == null)
            {
                return null;
            }

            ICloneable cloneable = value as ICloneable;

            if (cloneable != null)
            {
                return cloneable.Clone();
            }
            else
            {
                // Anonymous type
                Type type = value.GetType();

                if (type.IsDefined(typeof (CompilerGeneratedAttribute), true))
                {
                    object[] properties =
                        type.GetProperties()
                            .Select(x => x.GetValue(value)).ToArray();

                    return Activator.CreateInstance(type, properties);
                }
            }

            return value;
        }

        public object Value
        {
            get
            {
                return mValue;
            }
        }
    }
}