using System.Reflection;
#if !PCL
using Castle.Core.Internal;
#endif
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

        public bool IsCalleeMember(MemberInfo member)
        {
            return member.IsDefined(typeof(WampMemberAttributeBase));
        }

        public virtual RegisterOptions GetRegisterOptions(MemberInfo member)
        {
            RegisterOptions result = new RegisterOptions(mRegisterOptions);

            return result;
        }

        public virtual string GetProcedureUri(MemberInfo member)
        {
            WampMemberAttributeBase attribute =
                member.GetCustomAttribute<WampMemberAttributeBase>();

            if (!string.IsNullOrEmpty(attribute.Procedure))
                return attribute.Procedure;

            return member.Name;
        }
    }
}