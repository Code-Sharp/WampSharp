#if !ASYNC_LOCAL && !PCL
using System.Runtime.Remoting.Messaging;
#else
using System.Threading;
#endif
using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    /// <summary>
    /// Includes information about the current event.
    /// </summary>
    [Serializable]
    public class WampEventContext
    {

        #region Static Members

#if ASYNC_LOCAL
        private static readonly AsyncLocal<WampEventContext> mCurrent = new AsyncLocal<WampEventContext>();

        public static WampEventContext Current
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
        public static WampEventContext Current
        {
            get => (WampEventContext) CallContext.LogicalGetData(typeof (WampEventContext).Name);
            internal set => CallContext.LogicalSetData(typeof (WampEventContext).Name, value);
        }

#else
#endif
        #endregion
        #region Members

        #endregion

        #region Constructor

        internal WampEventContext(long publicationId, EventDetails eventDetails)
        {
            PublicationId = publicationId;
            EventDetails = eventDetails;
        }

        #endregion

        #region Properties

        public EventDetails EventDetails { get; }

        public long PublicationId { get; }

        #endregion
    }
}