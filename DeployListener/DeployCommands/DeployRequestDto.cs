using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployListener.DeployCommands
{
    /// <summary>
    /// Deployment request model
    /// </summary>
    public class DeployRequestDto
    {
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        public string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the deploy path.
        /// </summary>
        /// <value>
        /// The deploy path.
        /// </value>
        public string DeployPath { get; set; }

        /// <summary>
        /// Gets or sets the intermediate directory.
        /// </summary>
        /// <value>
        /// The intermediate directory.
        /// </value>
        public string IntermediateDirectory { get; set; }

        /// <summary>
        /// Gets or sets the web config path.
        /// </summary>
        /// <value>
        /// The web config path.
        /// </value>
        public string WebConfigPath { get; set; }
    }
}
