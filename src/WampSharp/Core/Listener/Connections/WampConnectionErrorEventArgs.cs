using System;

namespace WampSharp.Core.Listener
{
    public class WampConnectionErrorEventArgs : EventArgs
    {
        private readonly Exception mException;

        public WampConnectionErrorEventArgs(Exception exception)
        {
            mException = exception;
        }

        public Exception Exception
        {
            get
            {
                return mException;
            }
        }
    }
}