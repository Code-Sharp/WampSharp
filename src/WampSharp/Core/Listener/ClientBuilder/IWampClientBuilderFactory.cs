namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Builds a <see cref="IWampClientBuilder{TMessage,TClient}"/> corresponding 
    /// for a given <see cref="IWampClientContainer{TMessage,TClient}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    /// <remarks>
    /// This interface solves a problem that two objects are circular dependent
    /// in readonly fields.
    /// </remarks>
    public interface IWampClientBuilderFactory<TMessage, TClient>
    {
        /// <summary>
        /// Creates a <see cref="IWampClientBuilder{TMessage,TClient}"/>
        /// given a <see cref="IWampClientContainer{TMessage,TClient}"/>.
        /// </summary>
        /// <param name="container">The given <see cref="IWampClientContainer{TMessage,TClient}"/>.</param>
        /// <returns>The created <see cref="IWampClientBuilder{TMessage,TClient}"/>.</returns>
        IWampClientBuilder<TMessage, TClient> GetClientBuilder(IWampClientContainer<TMessage, TClient> container);
    }
}