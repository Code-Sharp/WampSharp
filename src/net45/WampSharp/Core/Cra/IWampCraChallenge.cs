namespace WampSharp.Core.Cra
{
    public interface IWampCraChallenge
    {
        string Salt { get; }
        int? Iterations { get; }
        int? KeyLength { get; }
    }
}