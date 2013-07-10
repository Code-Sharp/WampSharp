using System.Collections.Generic;

namespace WampSharp.PubSub.Server
{
    public class WampNotification
    {
        private readonly object mEvent;
        private readonly ICollection<string> mExcluded;
        private readonly ICollection<string> mEligible;
        private static readonly string[] mEmptyStringArray = new string[0];

        public WampNotification(object @event) :
            this(@event, mEmptyStringArray, mEmptyStringArray)
        {
        }

        public WampNotification(object @event, ICollection<string> excluded, ICollection<string> eligible)
        {
            mEvent = @event;
            mExcluded = excluded;
            mEligible = eligible;
        }

        public object Event
        {
            get
            {
                return mEvent;
            }
        }

        public ICollection<string> Excluded
        {
            get
            {
                return mExcluded;
            }
        }

        public ICollection<string> Eligible
        {
            get
            {
                return mEligible;
            }
        }
    }
}