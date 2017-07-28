using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Testament
{
    public interface IWampTestamentService
    {
        /// <summary>
        /// Add a testament to the current session.
        /// </summary>
        /// <param name="topic">The topic to publish to.</param>
        /// <param name="args">Arguments for the publication.</param>
        /// <param name="kwargs">Keyword arguments for the publication.</param>
        /// <param name="publish_options">The publish options for the publication.</param>
        /// <param name="scope">The scope of the testament, either "detatched" or "destroyed".</param>
        [WampProcedure("wamp.session.add_testament")]
        void AddTestament(string topic,
                          object[] args,
                          IDictionary<string, object> kwargs,
                          PublishOptions publish_options = null,
                          string scope = WampTestamentScope.Destroyed);

        /// <summary>
        /// Flush the testaments of a given scope.
        /// </summary>
        /// <param name="scope">The scope to flush, either "detatched" or "destroyed".</param>
        /// <returns>The number of flushed testament events.</returns>
        [WampProcedure("wamp.session.flush_testaments")]
        int FlushTestaments(string scope = WampTestamentScope.Destroyed);
    }
}