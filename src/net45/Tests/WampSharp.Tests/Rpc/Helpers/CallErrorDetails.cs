namespace WampSharp.Tests.Rpc.Helpers
{
    public class CallErrorDetails
    {
        private readonly string mErrorDesc;

        public CallErrorDetails(string errorUri, string errorDesc, object errorDetails)
        {
            ErrorUri = errorUri;
            mErrorDesc = errorDesc;
            ErrorDetails = errorDetails;
        }

        public string ErrorUri { get; }

        public string ErrorDesc => mErrorDesc;

        public object ErrorDetails { get; }
    }
}