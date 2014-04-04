namespace WampSharp.V2.Client
{
    public interface IWampRealmProxy
    {
        string Name { get; }

        IWampTopicContainerProxy TopicContainer { get; }

        IWampRpcOperationCatalogProxy RpcCatalog { get; }
    }
}