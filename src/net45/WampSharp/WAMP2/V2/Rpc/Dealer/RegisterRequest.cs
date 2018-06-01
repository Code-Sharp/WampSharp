using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class RegisterRequest : IRegisterRequest
    {
        private readonly long mRequestId;

        public RegisterRequest(IWampCallee callee, long requestId)
        {
            Callee = callee;
            mRequestId = requestId;
        }

        public IWampCallee Callee { get; }

        public long RequestId => mRequestId;

        public void Registered(long registrationId)
        {
            Callee.Registered(this.RequestId, registrationId);
        }
    }
}