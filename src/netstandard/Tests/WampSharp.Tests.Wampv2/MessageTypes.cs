using WampSharp.Core.Message;

namespace WampSharp.Tests.Wampv2
{
    public class MessageTypes
    {
        public static WampMessageType[] Rpc => new WampMessageType[]
                                               {
                                                   WampMessageType.v2Register, 
                                                   WampMessageType.v2Registered, 
                                                   WampMessageType.v2Unregister, 
                                                   WampMessageType.v2Unregistered, 
                                                   WampMessageType.v2Invocation, 
                                                   WampMessageType.v2Yield, 
                                                   WampMessageType.v2Call, 
                                                   WampMessageType.v2Result, 
                                                   WampMessageType.v2Cancel, 
                                                   WampMessageType.v2Interrupt, 
                                                   WampMessageType.v2Error, 
                                               };
    }
}