using System.Reflection;

namespace WampSharp.V2.Rpc
{
    internal class MethodInfoRpcOperation
    {
        public static string GetProcedure(MethodInfo method)
        {
            WampProcedureAttribute procedureAttribute =
                method.GetCustomAttribute<WampProcedureAttribute>(true);

            if (procedureAttribute == null)
            {
                // throw 
            }

            return procedureAttribute.Procedure;
        }
    }
}