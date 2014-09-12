using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Newtonsoft
{
    internal class JsonPropertyNameContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty result = base.CreateProperty(member, memberSerialization);

            PropertyNameAttribute attribute = member.GetCustomAttribute<PropertyNameAttribute>();

            if (attribute == null)
            {
                result.Ignored = true;
            }
            else
            {
                result.PropertyName = attribute.PropertyName;
            }

            return result;
        }
    }
}