using System;
using Castle.Core.Logging;

namespace WampSharp.Core.Logs
{
    public static class WampLoggerFactory
    {
        private static ILoggerFactory mFactory = new NullLogFactory();

        public static ILogger Create(Type type)
        {
            return mFactory.Create(type);
        }

        public static ILogger Create(string name)
        {
            return mFactory.Create(name);
        }

        public static ILogger Create(Type type, LoggerLevel level)
        {
            return mFactory.Create(type, level);
        }

        public static ILogger Create(string name, LoggerLevel level)
        {
            return mFactory.Create(name, level);
        }

        public static ILoggerFactory Factory
        {
            get { return mFactory; }
            set { mFactory = value; }
        }
    }
}