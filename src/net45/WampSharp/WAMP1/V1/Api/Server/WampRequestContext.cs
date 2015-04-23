using System;
using WampSharp.V1.Core.Contracts;
#if !PCL
using System.Runtime.Remoting.Messaging;
using WampSharp.V1.Cra;
#endif

namespace WampSharp.V1
{
    public class WampRequestContext
    {
        #region Static Members

#if !PCL
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
#else
        [ThreadStatic]
        private static WampRequestContext mCurrent;

        public static WampRequestContext Current
        {
            get
            {
                return mCurrent;
            }
            internal set
            {
                mCurrent = value;
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

        public string SessionId
        {
            get
            {
                return mClient.SessionId;
            }
        }

#if !PCL
        public IWampCraAuthenticator Authenticator
        {
            get
            {
                return mClient.CraAuthenticator;
            }
        }
#endif

        #endregion
    }
}
