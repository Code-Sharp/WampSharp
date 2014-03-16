using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WampSharp.V2.Rpc
{
    internal class MethodInfoHelper
    {
        private readonly OutputParameter[] mOutOrRefValues;
        private readonly int[] mInputValues;
        private readonly int mLength;

        public MethodInfoHelper(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            mLength = parameters.Length;

            mInputValues =
                parameters.Select((parameter, index) =>
                                  new { parameter, index })
                          .Where(x => !x.parameter.IsOut)
                          .Select(x => x.index)
                          .ToArray();

            mOutOrRefValues =
                parameters
                    .Select((parameter, index) =>
                            new {parameter, index})
                    .Where(x => x.parameter.IsOut ||
                                x.parameter.ParameterType.IsByRef)
                    .Select(x => new OutputParameter(x.parameter.Name, x.index))
                    .ToArray();
        }

        public object[] GetArguments(object[] inputs)
        {
            object[] result = new object[mLength];

            for (int i = 0; i < mInputValues.Length; i++)
            {
                object current = inputs[i];
                int index = mInputValues[i];
                result[index] = current;
            }

            return result;
        }

        public IDictionary<string, object> GetOutOrRefValues(object[] arguments)
        {
            if (mOutOrRefValues.Length == 0)
            {
                return null;
            }

            Dictionary<string, object> result =
                mOutOrRefValues.ToDictionary(x => x.Name,
                                             x => arguments[x.Position]);

            return result;
        }

        private class OutputParameter
        {
            private readonly string mName;
            private readonly int mPosition;

            public OutputParameter(string name, int position)
            {
                mName = name;
                mPosition = position;
            }

            public string Name
            {
                get
                {
                    return mName;
                }
            }

            public int Position
            {
                get
                {
                    return mPosition;
                }
            }
        }
    }
}