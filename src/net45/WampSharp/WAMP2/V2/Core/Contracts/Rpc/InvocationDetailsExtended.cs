namespace WampSharp.V2.Core.Contracts
{
    public class InvocationDetailsExtended : InvocationDetails
    {
        public InvocationDetailsExtended()
        {
        }

        public InvocationDetailsExtended(InvocationDetailsExtended details) :
            base(details)
        {
            CallerOptions = details.CallerOptions;
        }

        [IgnoreProperty]
        public CallOptions CallerOptions { get; set; }

        [IgnoreProperty]
        public long? CallerSession { get; set; }
    }
}