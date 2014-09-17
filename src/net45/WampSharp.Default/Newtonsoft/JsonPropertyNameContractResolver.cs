using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Newtonsoft
{
    public class JsonPropertyNameContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty result = base.CreateProperty(member, memberSerialization);

            PropertyNameAttribute attribute = member.GetCustomAttribute<PropertyNameAttribute>();

            if (attribute != null)
            {
                result.PropertyName = attribute.PropertyName;
            }

            IgnorePropertyAttribute ignoreAttribute = member.GetCustomAttribute<IgnorePropertyAttribute>();

            if (ignoreAttribute != null)
            {
                result.Ignored = true;
            }

            return result;
        }
    }

}