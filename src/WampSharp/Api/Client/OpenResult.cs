namespace WampSharp
{
    using System;

    public class OpenResult
    {
        public Exception Exception { get; set; }

        public static OpenResult Default()
        {
            return new OpenResult();
        }
        
        public OpenResult()
        {
        }

        public OpenResult(Exception exception)
        {
            Exception = exception;
        }
    }
}