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
            get => mCurrent.Value;
            internal set => mCurrent.Value = value;
        }

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