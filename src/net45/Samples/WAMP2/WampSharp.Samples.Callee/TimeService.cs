using System;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    [Sample("timeservice")]
    public class TimeService
    {
        [WampProcedure("com.timeservice.now")]
        public string UtcNow()
        {
            DateTime date = DateTime.UtcNow;
            return date.ToString("yyyy-MM-ddTHH:mm:ssK");
        }
    }
}