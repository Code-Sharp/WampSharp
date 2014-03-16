using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public abstract class LocalRpcOperation : IWampRpcOperation
    {
        private static readonly object[] mEmptyResult = new object[0];

        private readonly string mProcedure;

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

        public void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options)
        {
            InnerInvoke(caller, formatter, options, null, null);
        }

        public void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments)
        {
            InnerInvoke(caller, formatter, options, arguments, null);
        }

        public void Invoke<TMessage>(IWampRpcOperationCallback caller,
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

        protected void CallResult(IWampRpcOperationCallback caller, object result, IDictionary<string, object> outputs)
        {
            IDictionary<string, object> details =
                new Dictionary<string, object>();

            object[] resultArguments = mEmptyResult;

            if (this.HasResult)
            {
                resultArguments = new object[] { result };
            }

            if (outputs != null)
            {
                caller.Result(details, resultArguments, outputs);
            }
            else if (!this.HasResult)
            {
                caller.Result(details);
            }
            else
            {
                caller.Result(details, resultArguments);
            }
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

        private static Exception NameError(string details)
        {
            throw new WampException(WampErrors.InvalidArgument, details);
        }

        private static Exception PositionError(int position)
        {
            throw new WampException(WampErrors.InvalidArgument, position);
        }

        protected abstract void InnerInvoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}