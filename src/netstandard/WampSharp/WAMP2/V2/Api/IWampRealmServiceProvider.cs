using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents services for a WAMP realm.
    /// </summary>
    public interface IWampRealmServiceProvider
    {
        /// <summary>
        /// Registers an instance of a type having methods decorated with
        /// <see cref="WampProcedureAttribute"/> to the realm.
        /// </summary>
        /// <param name="instance">The instance to register.</param>
        /// <returns>A task that is completed when all methods are registered - its result is a
        /// <see cref="IAsyncDisposable"/>- disposing it will unregister the instance.</returns>
        Task<IAsyncDisposable> RegisterCallee(object instance);

        /// <summary>
        /// Registers an instance of a type having methods decorated with
        /// <see cref="WampProcedureAttribute"/> to the realm.
        /// </summary>
        /// <param name="instance">The instance to register.</param>
        /// <param name="interceptor">An object which allows registration customization.</param>
        /// <returns>A task that is completed when all methods are registered - its result is a
        /// <see cref="IAsyncDisposable"/>- disposing it will unregister the instance.</returns>
        Task<IAsyncDisposable> RegisterCallee(object instance, ICalleeRegistrationInterceptor interceptor);

        /// <summary>
        /// Registers an instance of a type having methods decorated with
        /// <see cref="WampProcedureAttribute"/> to the realm.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="instanceProvider">A delegate that creates an instance of the service type per call.</param>
        /// <returns>A task that is completed when all methods are registered - its result is a
        /// <see cref="IAsyncDisposable"/>- disposing it will unregister the instance.</returns>
        Task<IAsyncDisposable> RegisterCallee(Type serviceType, Func<object> instanceProvider);

        /// <summary>
        /// Registers an instance of a type having methods decorated with
        /// <see cref="WampProcedureAttribute"/> to the realm.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="instanceProvider">A delegate that creates an instance of the service type per call.</param>
        /// <param name="interceptor">An object which allows registration customization.</param>
        /// <returns>A task that is completed when all methods are registered - its result is a
        /// <see cref="IAsyncDisposable"/>- disposing it will unregister the instance.</returns>
        Task<IAsyncDisposable> RegisterCallee(Type serviceType, Func<object> instanceProvider, ICalleeRegistrationInterceptor interceptor);

        /// <summary>
        /// Registers an instance of a type having methods decorated with
        /// <see cref="WampProcedureAttribute"/> to the realm.
        /// </summary>
        /// <param name="instanceProvider">A delegate that creates an instance per call.</param>
        /// <returns>A task that is completed when all methods are registered - its result is a
        /// <see cref="IAsyncDisposable"/>- disposing it will unregister the instance.</returns>
        Task<IAsyncDisposable> RegisterCallee<TService>(Func<TService> instanceProvider) where TService : class;

        /// <summary>
        /// Registers an instance of a type having methods decorated with
        /// <see cref="WampProcedureAttribute"/> to the realm.
        /// </summary>
        /// <param name="instanceProvider">A delegate that creates an instance per call.</param>
        /// <param name="interceptor">An object which allows registration customization.</param>
        /// <returns>A task that is completed when all methods are registered - its result is a
        /// <see cref="IAsyncDisposable"/>- disposing it will unregister the instance.</returns>
        Task<IAsyncDisposable> RegisterCallee<TService>(Func<TService> instanceProvider, ICalleeRegistrationInterceptor interceptor) where TService : class;

        /// <summary>
        /// Gets a proxy of a callee registered in the realm.
        /// </summary>
        /// <typeparam name="TProxy"></typeparam>
        /// <returns>The proxy to the callee.</returns>
        TProxy GetCalleeProxy<TProxy>() where TProxy : class;

        /// <summary>
        /// Gets a proxy of a callee registered in the realm.
        /// </summary>
        /// <param name="interceptor">An object which allows call customization.</param>
        /// <typeparam name="TProxy"></typeparam>
        /// <returns>The proxy to the callee.</returns>
        TProxy GetCalleeProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class;

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

        /// <summary>
        /// Registers an instance of a type having events decorated with
        /// <see cref="WampTopicAttribute"/> to the realm.
        /// </summary>
        /// <param name="instance">The instance to register.</param>
        /// <returns>A disposable - disposing it will unregister the realm from the events of the instance.</returns>
        IDisposable RegisterPublisher(object instance);

        /// <summary>
        /// Registers an instance of a type having events decorated with
        /// <see cref="WampTopicAttribute"/> to the realm.
        /// </summary>
        /// <param name="instance">The instance to register.</param>
        /// <param name="interceptor">An object which allows registration customization.</param>
        /// <returns>A disposable - disposing it will unregister the realm from the events of the instance.</returns>
        IDisposable RegisterPublisher(object instance, IPublisherRegistrationInterceptor interceptor);

        /// <summary>
        /// Registers an instance of a type having methods handlers decorated with
        /// <see cref="WampTopicAttribute"/> to the realm.
        /// </summary>
        /// <param name="instance">The instance to register.</param>
        /// <returns>A Task that is finished when SUBSCRIBE is complete - its result is a
        /// <see cref="IAsyncDisposable"/>- disposing it will unsubscribe from the topic.</returns>
        Task<IAsyncDisposable> RegisterSubscriber(object instance);

        /// <summary>
        /// Registers an instance of a type having methods handlers decorated with
        /// <see cref="WampTopicAttribute"/> to the realm.
        /// </summary>
        /// <param name="instance">The instance to register.</param>
        /// <param name="interceptor">An object which allows registration customization.</param>
        /// <returns>A Task that is finished when SUBSCRIBE is complete - its result is a
        /// <see cref="IAsyncDisposable"/>- disposing it will unsubscribe from the topic.</returns>
        Task<IAsyncDisposable> RegisterSubscriber(object instance, ISubscriberRegistrationInterceptor interceptor);

        /// <summary>
        /// Gets a <see cref="ISubject{TTuple}"/> representing a
        /// WAMP topic in the realm.
        /// </summary>
        /// <param name="topicUri">The WAMP topic uri.</param>
        /// <param name="tupleConverter">An interface responsible for converting <see cref="IWampEvent"/>s into <typeparamref name="TTuple"/>s
        /// and vice versa</param>
        /// <returns>The requested subject.</returns>
        ISubject<TTuple> GetSubject<TTuple>(string topicUri, IWampEventValueTupleConverter<TTuple> tupleConverter);
    }
}