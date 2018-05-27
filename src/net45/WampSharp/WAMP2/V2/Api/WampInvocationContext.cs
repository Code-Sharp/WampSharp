#if !ASYNC_LOCAL && !PCL
using System.Runtime.Remoting.Messaging;
#endif

using System;
using System.Threading;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    /// <summary>
    /// Includes information about the current invocation.
    /// </summary>
    [Serializable]
    public class WampInvocationContext
    {
        #region Static Members

#if ASYNC_LOCAL
        private static readonly AsyncLocal<WampInvocationContext> mCurrent = new AsyncLocal<WampInvocationContext>();

        public static WampInvocationContext Current
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
#elif !PCL
        public static WampInvocationContext Current
        {
            get => (WampInvocationContext)CallContext.LogicalGetData(typeof(WampInvocationContext).Name);
            internal set => CallContext.LogicalSetData(typeof(WampInvocationContext).Name, value);
        }

#else
#endif
        #endregion
        #region Members

        #endregion

        #region Constructor

        internal WampInvocationContext(InvocationDetails invocationDetails)
        {
            InvocationDetails = invocationDetails;
        }

        #endregion

        #region Properties

        public InvocationDetails InvocationDetails { get; }

        #endregion
    }
}