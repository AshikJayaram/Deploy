using DeployNowClient.Models;
using DeployNowClient.ServerHandlers;

namespace DeployNowClient.DeployNowPlugin
{
    /// <summary>
    /// Processess the file copying from Source path to destination path
    /// </summary>
    public class FileProcessor
        : IDeployNowPlugin
    {
        /// <summary>
        /// Determines whether this instance can excecute the specified deploy entity.
        /// </summary>
        /// <param name="deployEntity">The deploy entity.</param>
        /// <returns>
        ///   <c>true</c> if this instance can excecute the specified deploy entity; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExcecute(dynamic deployEntity)
        {
            //checks if the object the is used to perform excecute function is not null
            return ((ServerDetailsModel) deployEntity != null);
        }

        /// <summary>
        /// Excecutes the specified deploy entity.
        /// </summary>
        /// <param name="deployEntity">The deploy entity.</param>
        public void Excecute(dynamic deployEntity)
        {
            //typecasting dynamic model to ServiceDetailsModel
            var serviceDetailsModel = (ServerDetailsModel) deployEntity;

            //get the instance of FileProcess handler class
            var fileProcessHandler = new FileProcessHandler();

            //process XCOPY function
            fileProcessHandler.ProcessXcopy(serviceDetailsModel);

            //delete the old file
            fileProcessHandler.DeleteOldFile(serviceDetailsModel);

            //replace the config file
            fileProcessHandler.ReplaceConfigFile(serviceDetailsModel);
        }
    }
}