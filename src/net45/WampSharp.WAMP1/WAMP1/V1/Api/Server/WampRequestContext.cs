using System;
#if !NETSTANDARD2_0
using System.Runtime.Remoting.Messaging;
#endif
using System.Threading;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Cra;

namespace WampSharp.V1
{
    [Serializable]
    public class WampRequestContext
    {
        #region Static Members

#if !NETSTANDARD2_0
        public static WampRequestContext Current
        {
            get => (WampRequestContext) CallContext.LogicalGetData(typeof (WampRequestContext).Name);
            internal set => CallContext.LogicalSetData(typeof (WampRequestContext).Name, value);
        }
#else
        private static readonly AsyncLocal<WampRequestContext> mCurrent = new AsyncLocal<WampRequestContext>();

        public static WampRequestContext Current
        {
            get
            {
                return mCurrent.Value;
            }
            internal set
            {
                mCurrent.Value = value;
            }
        }
#endif

        #endregion

        #region Members

        private readonly IWampClient mClient;

        #endregion

        #region Constructor

        internal WampRequestContext(IWampClient client)
        {
            mClient = client;
        }

        #endregion

        #region Properties

        public string SessionId => mClient.SessionId;

        public IWampCraAuthenticator Authenticator => mClient.CraAuthenticator;

        #endregion
    }
}
