using System.Reactive.Subjects;

namespace WampSharp.Rpc
{
    public class WampRpcRequest
    {
        public ISubject<object> Task { get; set; }

        public WampRpcCall<object> Request { get; set; } 
    }
}