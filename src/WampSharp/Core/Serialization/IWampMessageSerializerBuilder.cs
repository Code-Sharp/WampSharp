namespace WampSharp.Core.Serialization
{
    public interface IWampMessageSerializerBuilder
    {
        TProxy GetSerializer<TProxy>() where TProxy : class;
    }
}