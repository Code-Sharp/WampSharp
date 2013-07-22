using System.Reactive.Subjects;

namespace WampSharp.Rpc.Client
{
    public class WampRpcRequest
    {
        public ISubject<object> Task { get; set; }

        public WampRpcCall<object> Request { get; set; } 
    }
}