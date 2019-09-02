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
            get => mCurrent.Value;
            internal set => mCurrent.Value = value;
        }

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