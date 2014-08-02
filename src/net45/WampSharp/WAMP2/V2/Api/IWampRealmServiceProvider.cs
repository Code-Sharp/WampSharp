using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents services for a WAMP realm.
    /// </summary>
    public interface IWampRealmServiceProvider
    {
        // TODO: Add overloads for all options
        /// <summary>
        /// Registers an instance of a type having methods decorated with
        /// <see cref="WampProcedureAttribute"/> to the realm.
        /// </summary>
        /// <param name="instance">The instance to register.</param>
        /// <returns>A task that is completed when all methods are registered.</returns>
        Task RegisterCallee(object instance);

        /// <summary>
        /// Gets a proxy of a callee registered in the realm.
        /// </summary>
        /// <typeparam name="TProxy"></typeparam>
        /// <returns>The proxy to the callee.</returns>
        TProxy GetCalleeProxy<TProxy>() where TProxy : class;

        /// <summary>
        /// Gets a <see cref="ISubject{TResult}"/> representing a
        /// WAMP topic in the realm.
        /// </summary>
        /// <param name="topicUri">The WAMP topic uri.</param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns>The requested subject.</returns>
        ISubject<TEvent> GetSubject<TEvent>(string topicUri);

        /// <summary>
        /// Gets a <see cref="IWampSubject"/> representing a WAMP topic
        /// in the realm.
        /// </summary>
        /// <param name="topicUri">The WAMP topic uri.</param>
        /// <returns>The requested subject.</returns>
        IWampSubject GetSubject(string topicUri);
    }
}