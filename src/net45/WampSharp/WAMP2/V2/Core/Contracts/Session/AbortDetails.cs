namespace WampSharp.V2.Core.Contracts
{
    public class AbortDetails : WampDetailsOptions
    {
        [PropertyName("message")]
        public string Message { get; set; }
    }
}