using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Register)]
    public class RegisterOptions : WampDetailsOptions
    {
        public RegisterOptions()
        {
        }

        public RegisterOptions(RegisterOptions other)
        {
            this.DiscloseCaller = other.DiscloseCaller;
        }

        [PropertyName("disclose_caller")]
        public bool? DiscloseCaller { get; set; }
    }
}