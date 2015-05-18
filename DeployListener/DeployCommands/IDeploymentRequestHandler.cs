
namespace DeployListener.DeployCommands
{
    /// <summary>
    /// interface defining deployment request handler
    /// </summary>
    public interface IDeploymentRequestHandler
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        void Handle(DeployRequestDto request);
    }
}
