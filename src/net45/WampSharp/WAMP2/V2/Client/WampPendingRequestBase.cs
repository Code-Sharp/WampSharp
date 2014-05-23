using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class WampPendingRequestBase<TMessage, TResult>
    {
        private readonly TaskCompletionSource<TResult> mTaskCompletionSource = new TaskCompletionSource<TResult>();
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampPendingRequestBase(IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
        }

        public long RequestId
        {
            get;
            set;
        }

        protected Task<TResult> Task
        {
            get { return mTaskCompletionSource.Task; }
        }

        protected void Complete(TResult result)
        {
            mTaskCompletionSource.SetResult(result);
        }

        private IDictionary<string, object> DeserializeDictionary(TMessage details)
        {
            return mFormatter.Deserialize<IDictionary<string, object>>(details);
        }

        // TODO: Don't repeat yourself
        public void Error(TMessage details, string error)
        {
            IDictionary<string, object> deserializedDetails =
                DeserializeDictionary(details);

            SetException(new WampException(deserializedDetails, error));
        }

        public void Error(TMessage details, string error, TMessage[] arguments)
        {
            IDictionary<string, object> deserializedDetails =
                DeserializeDictionary(details);

            object[] castedArguments = arguments.Cast<object>().ToArray();

            SetException(new WampException(deserializedDetails, error, castedArguments));
        }

        public void Error(TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            IDictionary<string, object> deserializedDetails =
                DeserializeDictionary(details);

            object[] castedArguments = arguments.Cast<object>().ToArray();

            IDictionary<string, object> deserializedArgumentKeywords =
                DeserializeDictionary(argumentsKeywords);

            SetException(new WampException(deserializedDetails,
                                           error,
                                           castedArguments,
                                           deserializedArgumentKeywords));
        }

        private void SetException(WampException exception)
        {
            mTaskCompletionSource.SetException(exception);
        }
    }
}