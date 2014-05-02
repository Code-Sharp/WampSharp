using System.Threading.Tasks;

namespace WampSharp.V2.Client
{
    internal interface IWampTopicPublicationProxy
    {
        Task<long> Publish(string topicUri, object options);
        Task<long> Publish(string topicUri, object options, object[] arguments);
        Task<long> Publish(string topicUri, object options, object[] arguments, object argumentKeywords);
    }
}