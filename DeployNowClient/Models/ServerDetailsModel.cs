using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeployNowClient.Models
{
    /// <summary>
    /// Server details model
    /// </summary>
    public class ServerDetailsModel
    {
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the source file location from the source region in build server.
        /// </summary>
        /// <value>
        /// The source file location.
        /// </value>
        public string SourceFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the destination file location from the source region in build server.
        /// </summary>
        /// <value>
        /// The destination file location.
        /// </value>
        public string DestinationFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the source file.
        /// </summary>
        /// <value>
        /// The name of the source file.
        /// </value>
        public string SourceFileName { get; set; }

        /// <summary>
        /// Gets or sets the file limit.
        /// </summary>
        /// <value>
        /// The file limit.
        /// </value>
        public int FileLimit { get; set; }

        /// <summary>
        /// Gets or sets the config file location.
        /// </summary>
        /// <value>
        /// The config file location.
        /// </value>
        public string ConfigFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the server lists.
        /// </summary>
        /// <value>
        /// The server lists.
        /// </value>
        public IList<ServerList> ServerList { get; set; }
    }
}