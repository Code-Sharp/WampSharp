#if CASTLE
using Castle.DynamicProxy;
using WampSharp.V2.Binding;

namespace WampSharp.V2.Core.Proxy
{
    internal class BindingPropertyInterceptor<TMessage> : IInterceptor
    {
        private readonly IWampBinding<TMessage> mBinding;

        public BindingPropertyInterceptor(IWampBinding<TMessage> binding)
        {
            mBinding = binding;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = mBinding;
        }
    }
}
#endif