using System;

namespace WampSharp.Auxiliary.Client
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