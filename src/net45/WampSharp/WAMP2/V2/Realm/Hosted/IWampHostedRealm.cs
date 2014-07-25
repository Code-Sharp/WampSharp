using System;

namespace WampSharp.V2.Realm
{
    public interface IWampHostedRealm : IWampRealm
    {
        event EventHandler<WampSessionEventArgs> SessionCreated;
        event EventHandler<WampSessionCloseEventArgs> SessionClosed;

        IWampRealmServiceProvider Services { get; }         
    }
}