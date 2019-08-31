using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal interface IWampTopicPublicationProxy
    {
        Task<long?> Publish(string topicUri, PublishOptions options);
        Task<long?> Publish(string topicUri, PublishOptions options, object[] arguments);
        Task<long?> Publish(string topicUri, PublishOptions options, object[] arguments, IDictionary<string, object> argumentKeywords);
    }
}