using WampSharp.Core.Utilities;

namespace WampSharp.V2.Core
{
    internal class WampIdMapper<T> : IdMapperBase<long, T>
    {
        private readonly IWampIdGenerator mGenerator = new WampIdGenerator();

        protected override long GenerateId()
        {
            return mGenerator.Generate();
        }
    }
}