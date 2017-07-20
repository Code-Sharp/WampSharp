using System.Linq;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal static class PublishOptionsExtensions
    {
        public static EventDetails GetEventDetails(this PublishOptions options, string match)
        {
            EventDetails result = new EventDetails();

            PublishOptionsExtended extendedOptions =
                options as PublishOptionsExtended;

            bool disclosePublisher = options.DiscloseMe ?? false;

            if (extendedOptions != null)
            {
                if (disclosePublisher)
                {
                    result.Publisher = extendedOptions.PublisherId;

                    result.AuthenticationId = extendedOptions.AuthenticationId;
                    result.AuthenticationRole = extendedOptions.AuthenticationRole;
                }

                if (match != WampMatchPattern.Exact)
                {
                    result.Topic = extendedOptions.TopicUri;
                }
            }

            return result;
        }

        public static bool IsEligible(this PublishOptions options, IRemoteWampTopicSubscriber subscriber)
        {
            long sessionId = subscriber.SessionId;
            string authId = subscriber.AuthenticationId;
            string authRole = subscriber.AuthenticationRole;

            return (IsEligible(options.Eligible, sessionId, true) ||
                    IsEligible(options.EligibleAuthenticationIds, authId, true) ||
                    IsEligible(options.EligibleAuthenticationRoles, authRole, true)) &&
                   (IsEligible(options.Exclude, sessionId, false) &&
                    IsEligible(options.ExcludeAuthenticationIds, authId, false) &&
                    IsEligible(options.ExcludeAuthenticationRoles, authRole, false));
        }

        private static bool IsEligible<T>(T[] array, T value, bool returnValue)
        {
            if (array == null || array.Length == 0)
            {
                return true;
            }

            if (array.Contains(value))
            {
                return returnValue;
            }
            else
            {
                return !returnValue;
            }
        }
    }
}