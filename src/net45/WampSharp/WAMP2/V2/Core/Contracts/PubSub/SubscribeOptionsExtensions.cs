namespace WampSharp.V2.Core.Contracts
{
    public static class SubscribeOptionsExtensions
    {
        public static SubscribeOptions WithDefaults(this SubscribeOptions options)
        {
            SubscribeOptions result = new SubscribeOptions(options);
            result.Match = result.Match ?? WampMatchPattern.Default;
            return result;
        }
    }
}