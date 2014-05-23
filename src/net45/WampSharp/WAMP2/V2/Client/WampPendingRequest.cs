using System.Threading.Tasks;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Client
{
    internal class WampPendingRequest<TMessage> : WampPendingRequestBase<TMessage, bool>
    {
        public WampPendingRequest(IWampFormatter<TMessage> formatter) : base(formatter)
        {
        }

        public void Complete()
        {
            this.Complete(true);
        }

        public Task Task
        {
            get { return base.Task; }
        }
    }

    internal class WampPendingRequest<TMessage, TResult> : WampPendingRequestBase<TMessage, TResult>
    {
        public WampPendingRequest(IWampFormatter<TMessage> formatter)
            : base(formatter)
        {
        }

        public void Complete(TResult result)
        {
            base.Complete(result);
        }

        public Task<TResult> Task
        {
            get { return base.Task; }
        }
    }    
}