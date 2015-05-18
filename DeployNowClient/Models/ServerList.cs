using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeployNowClient.Models
{
    /// <summary>
    /// Server list model
    /// </summary>
    public class ServerList
    {
        /// <summary>
        /// Gets or sets the server URL.
        /// </summary>
        /// <value>
        /// The server URL.
        /// </value>
        public string ServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the deploy directory.
        /// </summary>
        /// <value>
        /// The deploy directory.
        /// </value>
        public string DeployDirectory { get; set; }
    }
}