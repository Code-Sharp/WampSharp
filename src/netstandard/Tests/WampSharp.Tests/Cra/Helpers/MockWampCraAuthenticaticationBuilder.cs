using WampSharp.V1.Cra;

namespace WampSharp.Tests.Cra.Helpers
{
    public class MockWampCraAuthenticaticationBuilder<TMessage> : WampCraAuthenticaticatorBuilder<TMessage>
    {
        public override WampCraAuthenticator<TMessage> BuildAuthenticator(string clientSessionId)
        {
            MockWampCraAuthenticator<TMessage> result = new MockWampCraAuthenticator<TMessage>(clientSessionId, Formatter, RpcMetadataCatalog, TopicContainer);
            result.AllowAnonymousAccess = AllowAnonymous;
            result.RequireSalted = RequireSalted;
            return result;
        }

        public bool RequireSalted { get; set; } = true;
        public bool AllowAnonymous { get; set; }
    }
}