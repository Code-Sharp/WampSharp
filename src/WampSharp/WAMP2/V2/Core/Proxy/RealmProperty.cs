using Castle.DynamicProxy;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Core.Proxy
{
    internal class RealmProperty<TMessage> where TMessage : class
    {
        private IWampRealm<TMessage> mRealm;

        public IInterceptor Getter
        {
            get
            {
                return new RealmGetPropertyInterceptor(this);
            }
        }

        public IInterceptor Setter
        {
            get
            {
                return new RealmSetPropertyInterceptor(this);
            }
        }

        public class RealmGetPropertyInterceptor : IInterceptor
        {
            private readonly RealmProperty<TMessage> mParent;

            public RealmGetPropertyInterceptor(RealmProperty<TMessage> parent)
            {
                mParent = parent;
            }

            public void Intercept(IInvocation invocation)
            {
                invocation.ReturnValue = mParent.mRealm;
            }
        }

        public class RealmSetPropertyInterceptor : IInterceptor
        {
            private readonly RealmProperty<TMessage> mParent;

            public RealmSetPropertyInterceptor(RealmProperty<TMessage> parent)
            {
                mParent = parent;
            }

            public void Intercept(IInvocation invocation)
            {
                mParent.mRealm = (IWampRealm<TMessage>)invocation.Arguments[0];
            }
        }
    }
}