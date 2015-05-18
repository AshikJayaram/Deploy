using System.Diagnostics;
using DeployListener.DeployCommands;

namespace DeployListener.Helpers
{
    /// <summary>
    /// To stop the server using the command iisreset /stop
    /// </summary>
    public class ServerStopHelper : IServerStopHelper
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Handle(DeployRequestDto request)
        {
            var processInfo = new ProcessStartInfo
            {
                Arguments = "/stop",
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
