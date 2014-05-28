using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.Cra;

namespace WampSharp.Core.Contracts.V1
{
    /// <summary>
    /// Interface to data regarding a client that is authenticated (or being authenticated).
    /// </summary>
    public interface IWampCraClient
    {
        /// <summary>
        /// Interface to data regarding a client that is authenticated (or being authenticated).
        /// </summary>
        IWampCraAuthenticator CraAuthenticator { get; set; }
    }
}
