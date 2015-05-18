
namespace DeployListener.Models
{
    /// <summary>
    /// SqlExecutionConfigModel
    /// </summary>
    public class SqlExecutionConfigModel
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the input file.
        /// </summary>
        /// <value>
        /// The name of the input file.
        /// </value>
        public string InputFileName { get; set; }
    }
}
