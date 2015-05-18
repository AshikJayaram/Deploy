
namespace DeployListener.DeployCommands
{
    /// <summary>
    /// Interface to handle SQL deployment request to intermediate folder.
    /// </summary>
    public interface ISqlDeploymentRequestHandler
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        void Handle(DeployRequestDto request);
    }
}
