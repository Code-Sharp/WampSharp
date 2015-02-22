using System.Reflection;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    public class CalleeRegistrationInterceptor : ICalleeRegistrationInterceptor
    {
        private readonly RegisterOptions mRegisterOptions;

        public static readonly ICalleeRegistrationInterceptor Default =
            new CalleeRegistrationInterceptor(new RegisterOptions());

        public CalleeRegistrationInterceptor(RegisterOptions registerOptions)
        {
            mRegisterOptions = registerOptions;
        }

        public virtual bool IsCalleeProcedure(MethodInfo method)
        {
            return method.IsDefined(typeof (WampProcedureAttribute));
        }

        public virtual RegisterOptions GetRegisterOptions(MethodInfo method)
        {
            RegisterOptions result = new RegisterOptions(mRegisterOptions);

            return result;
        }

        public virtual string GetProcedureUri(MethodInfo method)
        {
            WampProcedureAttribute attribute =
                method.GetCustomAttribute<WampProcedureAttribute>();

            return attribute.Procedure;
        }
    }
}