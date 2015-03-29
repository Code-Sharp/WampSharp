using System;
using Castle.Core.Logging;

namespace WampSharp.Core.Logs
{
    public class ReadableConsoleFactory : ILoggerFactory
    {
        private readonly ConsoleFactory mUnderlying;

        public ReadableConsoleFactory() : this(LoggerLevel.Debug)
        {
        }

        public ReadableConsoleFactory(LoggerLevel level)
        {
            mUnderlying = new ConsoleFactory(level);
        }

        public ILogger Create(Type type)
        {
            return mUnderlying.Create(type.Name);
        }

        public ILogger Create(string name)
        {
            return mUnderlying.Create(name);
        }

        public ILogger Create(Type type, LoggerLevel level)
        {
            return mUnderlying.Create(type, level);
        }

        public ILogger Create(string name, LoggerLevel level)
        {
            return mUnderlying.Create(name, level);
        }
    }
}