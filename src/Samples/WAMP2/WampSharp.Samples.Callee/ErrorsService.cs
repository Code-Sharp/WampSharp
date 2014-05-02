using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    [Sample("errors")]
    public class ErrorsService
    {
        [WampProcedure("com.myapp.sqrt")]
        public int Sqrt(int x)
        {
            if (x == 0)
            {
                var dummy = new Dictionary<string, string>();
                throw new WampException("wamp.error.runtime_error",
                                        dummy,
                                        "don't ask folly questions;)");
                // TODO: this doesn't work. fix this.
            }
            else
            {
                return (int)Math.Sqrt(x);
            }
        }

        [WampProcedure("com.myapp.checkname")]
        public void CheckName(string name)
        {
            if (new[] { "foo", "bar" }.Contains(name))
            {
                var dummy = new Dictionary<string, string>();
                throw new WampException("com.myapp.error.reserved", dummy);
            }

            if (name.ToLower() != name.ToUpper())
            {
                var dummy = new Dictionary<string, string>();
                object[] arguments = new object[] { name.ToLower(), name.ToUpper() };
                throw new WampException("com.myapp.error.mixed_case", dummy);
                // TODO: add arguments support.
            }

            if ((name.Length < 3) || (name.Length > 10))
            {
                var dummy = new Dictionary<string, string>();
                object[] arguments = new object[] { };
                IDictionary<string, object> argumentKeywords =
                    new Dictionary<string, object>()
                        {
                            {"min", 3},
                            {"max", 10}
                        };

                throw new WampException("com.myapp.error.invalid_length", dummy);
                // TODO: add argument keywords support.
            }
        }

        [WampProcedure("com.myapp.compare")]
        public void Compare(int a, int b)
        {
            if (a < b)
            {
                var dummy = new Dictionary<string, string>();
                object[] arguments = new object[] { b - a };
                throw new WampException("com.myapp.error1", dummy); // AppException1 : WampException.
            }
        }
    }
}