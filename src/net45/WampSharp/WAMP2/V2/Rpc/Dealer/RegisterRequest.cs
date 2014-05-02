using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class RegisterRequest : IRegisterRequest
    {
        private readonly IWampCallee mCallee;
        private readonly long mRequestId;

        public RegisterRequest(IWampCallee callee, long requestId)
        {
            mCallee = callee;
            mRequestId = requestId;
        }

        public IWampCallee Callee
        {
            get
            {
                return mCallee;
            }
        }

        public long RequestId
        {
            get
            {
                return mRequestId;
            }
        }

        public void Registered(long registrationId)
        {
            Callee.Registered(this.RequestId, registrationId);
        }
    }
}