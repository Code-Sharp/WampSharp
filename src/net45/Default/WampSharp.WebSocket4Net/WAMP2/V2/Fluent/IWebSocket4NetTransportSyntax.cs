using System;
using SuperSocket.ClientEngine;

namespace WampSharp.V2.Fluent
{
    /// <summary>
    /// IWebSocket4NetTransportSyntax interface for fluent definitions
    /// </summary>
    public interface IWebSocket4NetTransportSyntax : ChannelFactorySyntax.ITransportSyntax
    {
        /// <summary>
        /// Fluent SetSecurityOptions
        /// </summary>
        /// <param name="configureSecurityOptions"></param>
        /// <returns></returns>
        ChannelFactorySyntax.ITransportSyntax SetSecurityOptions(Action<SecurityOption> configureSecurityOptions);
    }
}