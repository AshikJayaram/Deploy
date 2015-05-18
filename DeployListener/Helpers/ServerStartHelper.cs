using System.Diagnostics;
using DeployListener.DeployCommands;

namespace DeployListener.Helpers
{
    /// <summary>
    /// To start the server using the command iisreset /start
    /// </summary>
    public class ServerStartHelper : IServerStartHelper
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Handle(DeployRequestDto request)
        {
            var processInfo = new ProcessStartInfo
            {
                Arguments = "/start",
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "iisreset"
            };
            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();
                process.WaitForExit();
            }
        }
    }
}
