namespace WampSharp.V2.Rpc
{
    internal class SingleResultExtractor : WampResultExtractor
    {
        public override object[] GetArguments(object result)
        {
            return new object[] {result};
        }
    }
}