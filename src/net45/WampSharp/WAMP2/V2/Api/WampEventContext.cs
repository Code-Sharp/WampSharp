using System;
using System.Threading;
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