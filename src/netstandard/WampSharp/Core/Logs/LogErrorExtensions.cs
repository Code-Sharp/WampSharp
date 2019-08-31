using System;

namespace WampSharp.Logging
{
    public static class LogErrorExtensions
    {
        public static void ErrorFormat(this ILog log, Exception ex, string message, params object[] arguments)
        {
            log.Log(LogLevel.Error, () => message, ex, arguments);
        }

        public static void Error(this ILog log, string message, Exception ex)
        {
            log.Log(LogLevel.Error, () => message, ex);
        }
    }
}