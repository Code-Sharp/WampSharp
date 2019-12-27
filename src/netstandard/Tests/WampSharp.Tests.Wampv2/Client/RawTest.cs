using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;

namespace WampSharp.Tests.Wampv2.Client
{
    public abstract class RawTest<TMessage>
    {
        protected IWampBinding<TMessage> mBinding;
        private readonly IEqualityComparer<TMessage> mEqualityComparer;

        public RawTest(IWampBinding<TMessage> binding, IEqualityComparer<TMessage> equalityComparer)
        {
            mBinding = binding;
            mEqualityComparer = equalityComparer;
        }

        public string TestName { get; set; }

        public abstract void Act();
        public abstract void Assert();

        protected void CompareParameters(object[] expected, object[] actual, string parameterType)
        {
            object[] serializedExpected = 
                expected.Select(x => SerializeArgument(x)).ToArray();

            object[] serializedActual = 
                actual.Select(x => SerializeArgument(x)).ToArray();
            
            NUnit.Framework.Assert.That
                (serializedExpected,
                 Is.EquivalentTo(serializedActual).Using(new ArgumentComparer(mEqualityComparer)),
                 string.Format("Expected {0} parameters were different than actual {0} parameters", parameterType));
        }

        private object SerializeArgument(object x)
        {
            if (x is string || x is long || x is int)
            {
                return x;
            }

            IWampFormatter<TMessage> formatter = mBinding.Formatter;

            if (x is object[] array)
            {
                TMessage[] arguments =
                    array.Select(y => formatter.Serialize(y)).ToArray();

                return arguments;
            }

            return formatter.Serialize(x);
        }

        private class ArgumentComparer : IEqualityComparer
        {
            private readonly IEqualityComparer<TMessage> mEqualityComparer;

            public ArgumentComparer(IEqualityComparer<TMessage> equalityComparer)
            {
                mEqualityComparer = equalityComparer;
            }

            public new bool Equals(object x, object y)
            {
                if (x is TMessage[] && y is TMessage[])
                {
                    IEnumerable<TMessage> first = x as IEnumerable<TMessage>;
                    IEnumerable<TMessage> second = y as IEnumerable<TMessage>;
                    return first.SequenceEqual(second, mEqualityComparer);
                }
                if (x is TMessage && y is TMessage)
                {
                    return mEqualityComparer.Equals((TMessage)x, (TMessage)y);
                }
                else
                {
                    return object.Equals(x, y);
                }
            }

            public int GetHashCode(object obj)
            {
                return 0;
            }
        }
    }
}