namespace WampSharp.V2.Core.Contracts
{
    public abstract class WampOptionsDetails
    {
        public ISerializedValue OriginalValue
        {
            get; 
            internal set;
            // I hate internal members but this time it is too hard
        }
    }
}