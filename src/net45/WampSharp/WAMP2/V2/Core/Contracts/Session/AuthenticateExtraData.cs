using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Authenticate)]
    public class AuthenticateExtraData : WampDetailsOptions
    {
    }
}