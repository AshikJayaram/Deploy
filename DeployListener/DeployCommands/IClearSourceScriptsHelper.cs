using System;

namespace DeployListener.DeployCommands
{
    public interface IClearSourceScriptsHelper
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        void Handle(DeployRequestDto request);
    }
}
