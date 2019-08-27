using System;
using CliFx.Attributes;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    [Command("timeservice")]
    public class TimeServiceCommand : CalleeCommand<TimeService>
    {
    }
    
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