using System.Collections.Generic;

namespace WampSharp.PubSub.Server
{
    public class WampNotification<TMessage>
    {
        private readonly TMessage mEvent;
        private readonly ICollection<string> mExcluded;
        private readonly ICollection<string> mEligible;

        public WampNotification(TMessage @event) :
            this(@event, new string[0], new string[0])
        {
        }

        public WampNotification(TMessage @event, ICollection<string> excluded, ICollection<string> eligible)
        {
            mEvent = @event;
            mExcluded = excluded;
            mEligible = eligible;
        }

        public TMessage Event
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