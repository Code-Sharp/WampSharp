using WampSharp.Cra;

namespace WampSharp.Tests.Cra.Helpers
{
    public class MockWampCraAuthenticaticationBuilder<TMessage> : WampCraAuthenticaticatorBuilder<TMessage>
    {
        private bool mRequireSalted = true;

        public override WampCraAuthenticator<TMessage> BuildAuthenticator(string clientSessionId)
        {
            MockWampCraAuthenticator<TMessage> result = new MockWampCraAuthenticator<TMessage>(clientSessionId, Formatter, RpcMetadataCatalog, TopicContainer);
            result.AllowAnonymousAccess = AllowAnonymous;
            result.RequireSalted = RequireSalted;
            return result;
        }

        public bool RequireSalted
        {
            get { return mRequireSalted; }
            set { mRequireSalted = value; }
        }

        public bool AllowAnonymous { get; set; }
    }
}