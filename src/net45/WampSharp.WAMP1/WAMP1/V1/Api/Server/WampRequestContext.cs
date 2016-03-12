using System;
using System.Runtime.Remoting.Messaging;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Cra;

namespace WampSharp.V1
{
    [Serializable]
    public class WampRequestContext
    {
        #region Static Members

        public static WampRequestContext Current
        {
            get
            {
                return (WampRequestContext) CallContext.LogicalGetData(typeof (WampRequestContext).Name);
            }
            internal set
            {
                CallContext.LogicalSetData(typeof (WampRequestContext).Name, value);
            }
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

        public string SessionId
        {
            get
            {
                return mClient.SessionId;
            }
        }

        public IWampCraAuthenticator Authenticator
        {
            get
            {
                return mClient.CraAuthenticator;
            }
        }

        #endregion
    }
}
