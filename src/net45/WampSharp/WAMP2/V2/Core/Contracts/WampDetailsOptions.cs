namespace WampSharp.V2.Core.Contracts
{
    public abstract class WampDetailsOptions
    {
        [IgnoreProperty]
        public ISerializedValue OriginalValue
        {
            get; 
            set;
        }
    }
}