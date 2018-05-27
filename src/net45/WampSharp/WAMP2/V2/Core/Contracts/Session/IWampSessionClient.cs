using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSessionClient
    {
        [WampHandler(WampMessageType.v2Challenge)]
        void Challenge(string authMethod, ChallengeDetails extra);

        [WampHandler(WampMessageType.v2Welcome)]
        void Welcome(long session, WelcomeDetails details);

        [WampHandler(WampMessageType.v2Abort)]
        void Abort(AbortDetails details, string reason);

        [WampHandler(WampMessageType.v2Goodbye)]
        void Goodbye(GoodbyeDetails details, string reason);
    }
}