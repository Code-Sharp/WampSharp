using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public abstract class LocalRpcOperation : IWampRpcOperation
    {
        private static readonly object[] mEmptyResult = new object[0];

        private readonly string mProcedure;

        protected readonly static IWampFormatter<object> ObjectFormatter =
            new WampObjectFormatter();

        protected LocalRpcOperation(string procedure)
        {
            mProcedure = procedure;
        }

        public string Procedure
        {
            get
            {
                return mProcedure;
            }
        }

        public abstract RpcParameter[] Parameters
        {
            get;
        }

        public abstract bool HasResult
        {
            get;
        }

        /// <summary>
        /// Returns a value indicating whether to treat an ICollection{T} result
        /// as the arguments yield argument. (If false, treats an ICollection{T} result
        /// value as a single argument).
        /// </summary>
        public abstract CollectionResultTreatment CollectionResultTreatment
        {
            get;
        }

        public void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage details)
        {
            InnerInvoke(caller, formatter, details, null, null);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments)
        {
            InnerInvoke(caller, formatter, options, arguments, null);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationCallback caller,
                                     IWampFormatter<TMessage> formatter,
                                     TMessage options,
                                     TMessage[] arguments,
                                     TMessage argumentsKeywords)
        {
            IDictionary<string, TMessage> nameToParameterValue =
                formatter.Deserialize<IDictionary<string, TMessage>>
                    (argumentsKeywords);

            InnerInvoke(caller, formatter, options, arguments, nameToParameterValue);
        }

        protected void CallResult(IWampRawRpcOperationCallback caller, object result, IDictionary<string, object> outputs)
        {
            IDictionary<string, object> details =
                new Dictionary<string, object>();

            object[] resultArguments = mEmptyResult;

            if (this.HasResult)
            {
                if (this.CollectionResultTreatment == CollectionResultTreatment.Multivalued)
                {
                    resultArguments = GetFlattenResult((dynamic) result);
                }
                else
                {
                    resultArguments = new object[] {result};
                }
            }

            if (outputs != null)
            {
                caller.Result(ObjectFormatter, details, resultArguments, outputs);
            }
            else if (!this.HasResult)
            {
                caller.Result(ObjectFormatter, details);
            }
            else
            {
                caller.Result(ObjectFormatter, details, resultArguments);
            }
        }

        private object[] GetFlattenResult<T>(ICollection<T> result)
        {
            return result.Cast<object>().ToArray();
        }

        private object[] GetFlattenResult(object result)
        {
            return new object[] {result};
        }

        protected object[] UnpackParameters<TMessage>(IWampFormatter<TMessage> formatter,
                                                      TMessage[] arguments,
                                                      IDictionary<string, TMessage> argumentsKeywords)
        {
            IEnumerable<object> positional = Enumerable.Empty<object>();
            IEnumerable<object> named = Enumerable.Empty<object>();

            int positionalArguments = 0;
            
            if (arguments != null)
            {
                positionalArguments = arguments.Length;

                positional =
                    Parameters.Take(positionalArguments)
                              .Zip(arguments, (parameter, value) => new {parameter, value})
                              .Select(x => GetPositionalParameterValue(formatter, x.parameter, x.value));
            }

            named = Parameters.Skip(positionalArguments)
                              .Select(parameter => GetNamedParameterValue(formatter, parameter, argumentsKeywords));

            object[] result = positional.Concat(named).ToArray();

            return result;
        }

        private object ConvertParameter<TMessage>(IWampFormatter<TMessage> formatter, RpcParameter parameter, TMessage value)
        {
            return formatter.Deserialize(parameter.Type, value);
        }


        private object ConvertNamedParameter<TMessage>(IWampFormatter<TMessage> formatter,
                                                       RpcParameter parameter,
                                                       TMessage value)
        {
            try
            {
                return ConvertParameter(formatter, parameter, value);
            }
            catch (Exception ex)
            {
                throw NameError(parameter.Name);
            }
        }

        private object GetPositionalParameterValue<TMessage>(IWampFormatter<TMessage> formatter,
                                                             RpcParameter parameter,
                                                             TMessage value)
        {
            try
            {
                return ConvertParameter(formatter, parameter, value);
            }
            catch (Exception ex)
            {
                throw PositionError(parameter.Position);
            }
        }

        private object GetNamedParameterValue<TMessage>(IWampFormatter<TMessage> formatter, RpcParameter parameter, IDictionary<string, TMessage> argumentKeywords)
        {
            if (parameter.Name == null)
            {
                throw PositionError(parameter.Position);
            }
            else
            {
                TMessage value;

                if (argumentKeywords != null && 
                    argumentKeywords.TryGetValue(parameter.Name, out value))
                {
                    return ConvertNamedParameter(formatter, parameter, value);
                }
                else if (parameter.HasDefaultValue)
                {
                    return parameter.DefaultValue;
                }
                else
                {
                    throw NameError(parameter.Name);
                }
            }
        }

        private static Exception NameError(string name)
        {
            throw new WampException(WampErrors.InvalidArgument, "argument name: " + name);
        }

        private static Exception PositionError(int position)
        {
            throw new WampException(WampErrors.InvalidArgument, "argument position: " + position);
        }

        protected abstract void InnerInvoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}