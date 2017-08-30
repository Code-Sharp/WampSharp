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


        #endregion

        #region Members

        private readonly InvocationDetails mInvocationDetails;

        #endregion

        #region Constructor

        internal WampInvocationContext(InvocationDetails invocationDetails)
        {
            mInvocationDetails = invocationDetails;
        }

        #endregion

        #region Properties

        public InvocationDetails InvocationDetails
        {
            get
            {
                return mInvocationDetails;
            }
        }

        #endregion
    }
}