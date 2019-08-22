using WampSharp.V2.Binding.Contracts;

namespace WampSharp.RawSocket
{
    public static class SerializerTypeExtensions
    {
        public static string GetSubProtocol(this SerializerType serializerType)
        {
            switch (serializerType)
            {
                case SerializerType.Json:
                    return WampSubProtocols.JsonSubProtocol;
                case SerializerType.MsgPack:
                    return WampSubProtocols.MsgPackSubProtocol;
                case SerializerType.Cbor:
                    return WampSubProtocols.CborSubProtocol;
            }

            return serializerType.ToString();
        }
    }
}