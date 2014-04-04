using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Rpc
{
    public class SyncMethodInfoRpcOperation : SyncLocalRpcOperation
    {
        private readonly object mInstance;
        private readonly MethodInfo mMethod;
        private readonly MethodInfoHelper mHelper;
        private readonly RpcParameter[] mParameters;
        private readonly bool mHasResult;

        public SyncMethodInfoRpcOperation(object instance, MethodInfo method) :
            base(GetProcedure(method))
        {
            mInstance = instance;
            mMethod = method;

            if (method.ReturnType != typeof (void))
            {
                mHasResult = true;
            }
            else
            {
                mHasResult = false;
            }

            mHelper = new MethodInfoHelper(method);

            mParameters =
                method.GetParameters()
                      .Where(x => !x.IsOut)
                      .Select((parameter, index) =>
                              new RpcParameter(parameter.Name,
                                               parameter.ParameterType,
                                               parameter.DefaultValue,
                                               parameter.HasDefaultValue(),
                                               index))
                      .ToArray();
        }

        private static string GetProcedure(MethodInfo method)
        {
            WampProcedureAttribute procedureAttribute =
                method.GetCustomAttribute<WampProcedureAttribute>(true);

            if (procedureAttribute == null)
            {
                // throw 
            }

            return procedureAttribute.Procedure;
        }

        public override RpcParameter[] Parameters
        {
            get
            {
                return mParameters;
            }
        }

        public override bool HasResult
        {
            get
            {
                return mHasResult;
            }
        }

        protected override object InvokeSync<TMessage>
            (IWampRpcOperationCallback caller, 
            IWampFormatter<TMessage> formatter, 
            TMessage options, 
            TMessage[] arguments, 
            IDictionary<string, TMessage> argumentsKeywords, 
            out IDictionary<string, object> outputs)
        {
             object[] unpacked = 
                 UnpackParameters(formatter, arguments, argumentsKeywords);

            object[] parameters = 
                mHelper.GetArguments(unpacked);

            object result = 
                mMethod.Invoke(mInstance, parameters);

            outputs = mHelper.GetOutOrRefValues(parameters);

            return result;
        }
    }
}