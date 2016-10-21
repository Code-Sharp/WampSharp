using System;

namespace WampSharp.V2.Rpc
{
    public interface ICalleeSettings
    {
        WampRpcRuntimeException ConvertExceptionToRuntimeException(Exception exception);
    }

    public class CalleeSettings : ICalleeSettings
    {
        public bool IncludeExceptionStackTrace { get; set; }

        public WampRpcRuntimeException ConvertExceptionToRuntimeException(Exception exception)
        {
            WampRpcRuntimeException result = new WampRpcRuntimeException(exception.Message);

            if (IncludeExceptionStackTrace)
            {
                result.ArgumentsKeywords["stackTrace"] = exception.StackTrace;
            }

            return result;
        }
    }
}