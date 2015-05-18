using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace DeployListener.DeployCommands
{
    /// <summary>
    /// Handles the deployment phase
    /// </summary>
    public class DeployRequestHandler : IDeploymentRequestHandler
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Handle(DeployRequestDto request)
        {
            DirectoryInfo directory = new DirectoryInfo(request.DeployPath);

            var signalrHub = new SignalrHub();

            //for deleting all the files
            try
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                //for deleting all the directories
                foreach (var dir in directory.GetDirectories())
                {
                    dir.Delete(true);
                }
                DeployLogger.LogMessage.InfoFormat("Deploy folder {0} has been cleared successfully", request.DeployPath);
            }
            catch (AggregateException ex)
            {
                throw ex;
            }

            //Copy only if the direcory is empty
            if (!directory.GetDirectories().Any() && !directory.GetFiles().Any())
            {
                //unzipping and extracting data to the deploy path
                ZipFile.ExtractToDirectory(request.SourcePath, request.DeployPath);
                DeployLogger.LogMessage.InfoFormat("Code has been deployed to {0} folder successfully", request.DeployPath);
            }

            signalrHub.Publish("Deployment" , "Deployment completed successfully");
        }
    }
}
