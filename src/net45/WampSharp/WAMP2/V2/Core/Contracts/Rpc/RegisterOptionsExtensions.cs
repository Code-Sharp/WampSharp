namespace WampSharp.V2.Core.Contracts
{
    public static class RegisterOptionsExtensions
    {
        public static RegisterOptions WithDefaults(this RegisterOptions options)
        {
            RegisterOptions result = new RegisterOptions(options);
            result.Invoke = result.Invoke ?? WampInvokePolicy.Default;
            result.Match = result.Match ?? WampMatchPattern.Default;
            return result;
        }
    }
}