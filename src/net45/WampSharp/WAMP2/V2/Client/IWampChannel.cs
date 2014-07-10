using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    public interface IWampChannel
    {
        IWampServerProxy Server { get; }
        
        IWampRealmProxy RealmProxy { get; }
        
        Task Open();
    }
}