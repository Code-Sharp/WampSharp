using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Logging;
using WampSharp.Core.Logs;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;

namespace WampSharp.V2.Rpc
{
    internal class MethodInfoHelper
    {
        private readonly MethodInfo mMethod;
        private readonly ParameterInfo[] mOutOrRefValues;
        private readonly int[] mInputValues;
        private readonly int mLength;

        public MethodInfoHelper(MethodInfo method)
        {
            mMethod = method;
            ParameterInfo[] parameters = method.GetParameters();

            mLength = parameters.Length;

            mInputValues =
                parameters.Where(x => !x.IsOut)
                          .Select(x => x.Position)
                          .ToArray();

            mOutOrRefValues =
                parameters
                    .Where(x => x.IsOut ||
                                x.ParameterType.IsByRef)
                    .ToArray();
        }

        public MethodInfo Method
        {
            get { return mMethod; }
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

        public object[] GetInputArguments(object[] arguments)
        {
            object[] inputs = new object[mInputValues.Length];

            for (int i = 0; i < mInputValues.Length; i++)
            {
                int index = mInputValues[i];
                object current = arguments[index];
                inputs[index] = current;
            }

            return inputs;
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

        public void PopulateOutOrRefValues<TMessage>(IWampFormatter<TMessage> formatter,
                                                     object[] arguments,
                                                     IDictionary<string, TMessage> outOrRefParameters)
        {
            if (mOutOrRefValues.Length != 0)
            {
                foreach (ParameterInfo parameter in mOutOrRefValues)
                {
                    TMessage currentValue;

                    if (!outOrRefParameters.TryGetValue(parameter.Name, out currentValue))
                    {
                        throw new Exception(string.Format("Argument {0} not found in arguments dictionary",
                            parameter.Name));
                    }
                    else
                    {
                        Type parameterType = parameter.ParameterType.StripByRef();

                        object deserializedValue =
                            formatter.Deserialize(parameterType, currentValue);

                        arguments[parameter.Position] = deserializedValue;
                    }
                }
            }
        }
    }
}