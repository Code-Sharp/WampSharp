using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    public interface IWampRegistrationDescriptor
    {
        [WampProcedure("wamp.registration.list")]
        AvailableGroups GetAllRegistrations();

        [WampProcedure("wamp.registration.lookup")]
        long LookupRegistrationId(string procedureUri, RegisterOptions options = null);

        [WampProcedure("wamp.registration.match")]
        long[] GetMatchingRegistrationIds(string procedureUri);

        [WampProcedure("wamp.registration.get")]
        RegistrationDetails GetRegistrationDetails(long registrationId);

        [WampProcedure("wamp.registration.list_callees")]
        long[] GetCalleesIds(long registrationId);

        [WampProcedure("wamp.registration.count_callees")]
        long CountCallees(long registrationId);
    }
}