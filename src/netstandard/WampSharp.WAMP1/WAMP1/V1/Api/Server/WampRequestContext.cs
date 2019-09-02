using System;
using System.Threading;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Cra;

namespace WampSharp.V1
{
    [Serializable]
    public class WampRequestContext
    {
        #region Static Members

        private static readonly AsyncLocal<WampRequestContext> mCurrent = new AsyncLocal<WampRequestContext>();

        public static WampRequestContext Current
        {
            get => mCurrent.Value;
            internal set => mCurrent.Value = value;
        }

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
