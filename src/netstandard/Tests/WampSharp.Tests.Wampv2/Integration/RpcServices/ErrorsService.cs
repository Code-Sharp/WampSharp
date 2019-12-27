using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcServices
{
    public class ErrorsService
    {
        [WampProcedure("com.myapp.sqrt")]
        public int Sqrt(int x)
        {
            if (x == 0)
            {
                throw new WampRpcRuntimeException("don't ask folly questions;)");
            }
            else
            {
                if (x < 0)
                {
                    // Math.Sqrt doesn't throw exceptions for negative numbers.
                    throw new WampRpcRuntimeException("The square root of a negative number is non real", "x");
                }
                else
                {
                    return (int) Math.Sqrt(x);
                }
            }
        }

        [WampProcedure("com.myapp.checkname")]
        public void CheckName(string name)
        {
            if (new[] {"foo", "bar"}.Contains(name))
            {
                throw new WampException("com.myapp.error.reserved");
            }

            if (name.ToLower() != name.ToUpper())
            {
                throw new WampException("com.myapp.error.mixed_case", name.ToLower(), name.ToUpper());
            }

            if ((name.Length < 3) || (name.Length > 10))
            {
                object[] arguments = new object[] {};

                IDictionary<string, object> argumentKeywords =
                    new Dictionary<string, object>()
                        {
                            {"min", 3},
                            {"max", 10}
                        };

                throw new WampException("com.myapp.error.invalid_length", arguments, argumentKeywords);
            }
        }

        [WampProcedure("com.myapp.compare")]
        public void Compare(int a, int b)
        {
            if (a < b)
            {
                object[] arguments = new object[] {b - a};
                throw new AppException1(arguments); // AppException1 : WampException.
            }
        }

        public class AppException1 : WampException
        {
            private const string AppErrorUri = "com.myapp.error1";

            public AppException1(params object[] arguments)
                : base(AppErrorUri, arguments)
            {
            }

            public AppException1(object[] arguments, IDictionary<string, object> argumentsKeywords) :
                base(AppErrorUri, arguments, argumentsKeywords)
            {
            }

            public AppException1(IDictionary<string, object> details, object[] arguments,
                                 IDictionary<string, object> argumentsKeywords)
                : base(details, AppErrorUri, arguments, argumentsKeywords)
            {
            }

            public AppException1(IDictionary<string, object> details, string message,
                                 IDictionary<string, object> argumentsKeywords) :
                                     base(details, AppErrorUri, message, argumentsKeywords)
            {
            }

            public AppException1(IDictionary<string, object> details, object[] arguments,
                                 IDictionary<string, object> argumentsKeywords, string message, Exception inner) :
                                     base(details, AppErrorUri, arguments, argumentsKeywords, message, inner)
            {
            }
        }
    }
}