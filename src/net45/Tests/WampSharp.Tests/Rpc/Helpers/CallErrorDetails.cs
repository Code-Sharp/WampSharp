namespace WampSharp.Tests.Rpc.Helpers
{
    public class CallErrorDetails
    {
        private string mErrorUri;
        private string mErrorDesc;
        private object mErrorDetails;

        public CallErrorDetails(string errorUri, string errorDesc, object errorDetails)
        {
            mErrorUri = errorUri;
            mErrorDesc = errorDesc;
            mErrorDetails = errorDetails;
        }

        public string ErrorUri
        {
            get { return mErrorUri; }
        }

        public string ErrorDesc
        {
            get { return mErrorDesc; }
        }

        public object ErrorDetails
        {
            get { return mErrorDetails; }
        }
    }
}