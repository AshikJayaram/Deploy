using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace DeployListener.DeployCommands
{
    public class SqlDeploymentHandler : ISqlDeploymentRequestHandler
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Handle(DeployRequestDto request)
        {
            DirectoryInfo directory = new DirectoryInfo(request.IntermediateDirectory);

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

            }
            catch (AggregateException ex)
            {
                throw ex;
            }

            //Copy only if the direcory is empty
            if (!directory.GetDirectories().Any() && !directory.GetFiles().Any())
            {
                //unzipping and extracting data to the deploy path
                ZipFile.ExtractToDirectory(request.SourcePath, request.IntermediateDirectory);
            }

            signalrHub.Publish("SqlDeployment", "Sql Scripts deployed to intermediate folder successfully.");
        }
    }
}
