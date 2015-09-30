using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    public interface IWampRegistrationDescriptor
    {
        /// <summary>
        /// Retrieves registration IDs listed according to match policies.
        /// </summary>
        /// <returns>An object with a list of registration IDs for each match policy.</returns>
        [WampProcedure("wamp.registration.list")]
        AvailableGroups GetAllRegistrations();

        /// <summary>
        /// Obtains the registration (if any) managing a procedure, according to some match policy.
        /// </summary>
        /// <param name="procedureUri">The procedure to lookup the registration for.</param>
        /// <param name="options">Same options as when registering a procedure.</param>
        /// <returns>The ID of the registration managing the procedure, if found, or null.</returns>
        [WampProcedure("wamp.registration.lookup")]
        long? LookupRegistrationId(string procedureUri, RegisterOptions options = null);

        /// <summary>
        /// Obtains the registration best matching a given procedure URI.
        /// </summary>
        /// <param name="procedureUri">The procedure URI to match.</param>
        /// <returns>The ID of best matching registration, or null.</returns>
        [WampProcedure("wamp.registration.match")]
        long? GetBestMatchingRegistrationId(string procedureUri);

        /// <summary>
        /// Retrieves information on a particular registration.
        /// </summary>
        /// <param name="registrationId">The ID of the registration to retrieve.</param>
        /// <returns>Details on the registration.</returns>
        [WampProcedure("wamp.registration.get")]
        RegistrationDetails GetRegistrationDetails(long registrationId);

        /// <summary>
        /// Retrieves a list of session IDs for sessions currently attached to the registration.
        /// </summary>
        /// <param name="registrationId">The ID of the registration to get calles for.</param>
        /// <returns>A list of WAMP session IDs of callees currently attached to the 
        /// registration.</returns>
        [WampProcedure("wamp.registration.list_callees")]
        long[] GetCalleesIds(long registrationId);

        /// <summary>
        /// Obtains the number of sessions currently attached to a registration.
        /// </summary>
        /// <param name="registrationId">The ID of the registration to get the number of callees for.</param>
        /// <returns>The number of callees currently attached to a registration.</returns>
        [WampProcedure("wamp.registration.count_callees")]
        long CountCallees(long registrationId);
    }
}