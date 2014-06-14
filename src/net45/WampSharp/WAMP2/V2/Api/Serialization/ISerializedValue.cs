namespace WampSharp.V2
{
    public interface ISerializedValue
    {
        T Deserialize<T>();
    }
}