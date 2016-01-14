#if !PCL
using System.Runtime.Remoting.Messaging;
#endif
using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    /// <summary>
    /// Includes information about the current event.
    /// </summary>
#if !PCL
    [Serializable]
#endif
    public class WampEventContext
    {

        #region Static Members

#if !PCL 
        public static WampEventContext Current
        {
            get
            {
                return (WampEventContext) CallContext.LogicalGetData(typeof (WampEventContext).Name);
            }
            internal set
            {
                CallContext.LogicalSetData(typeof (WampEventContext).Name, value);
            }
        }
#else
        [ThreadStatic]
        private static WampEventContext mCurrent;

        public static WampEventContext Current
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

        private readonly long mPublicationId;
        private readonly EventDetails mEventDetails;

        #endregion

        #region Constructor

        internal WampEventContext(long publicationId, EventDetails eventDetails)
        {
            mPublicationId = publicationId;
            mEventDetails = eventDetails;
        }

        #endregion

        #region Properties

        public EventDetails EventDetails
        {
            get
            {
                return mEventDetails;
            }
        }

        public long PublicationId
        {
            get
            {
                return mPublicationId;
            }
        }

        #endregion
    }
}