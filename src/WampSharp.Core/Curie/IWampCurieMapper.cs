namespace WampSharp.Core.Curie
{
    public interface IWampCurieMapper
    {
        string ResolveCurie(string curie);
        void Map(string curie, string uri);
    }
}