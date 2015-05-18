using System;
using System.IO;

namespace DeployListener.DeployCommands
{
    /// <summary>
    /// Deletes the sql scripts from the source server.
    /// This ensures that the same scripts will not be executed again.
    /// </summary>
    public class ClearSourceScriptsHelper : IClearSourceScriptsHelper
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Handle(DeployRequestDto request)
        {
            var directory = new DirectoryInfo(request.SourcePath);

            var signalrHub = new SignalrHub();

            //for deleting all the sql script files
            try
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (AggregateException ex)
            {
                throw ex;
            }

           signalrHub.Publish("SqlSourceClear", "Sql scripts cleared from the source location.");
        }
    }
}
