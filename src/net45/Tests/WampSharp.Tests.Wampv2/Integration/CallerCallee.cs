using WampSharp.V2;

namespace WampSharp.Tests.Wampv2.Integration
{
    internal class CallerCallee
    {
        public IWampChannel CalleeChannel { get; set; }
        public IWampChannel CallerChannel { get; set; }
        public long CallerSessionId { get; set; }
    }
}