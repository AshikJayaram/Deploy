using DeployListener.DeployCommands;

namespace DeployListener.Helpers
{
    public interface IServerStartHelper
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        void Handle(DeployRequestDto request);
    }
}