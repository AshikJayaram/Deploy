using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DeployNowClient.Models;

namespace DeployNowClient.DeployNowPlugin
{
    /// <summary>
    /// Prerequisites that are needed before deployment process
    /// </summary>
    public class DeployPreRequisite
        : IDeployNowPlugin
    {
        #region methods

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
            var promoteCodeModel = (PromoteCodeModel) deployEntity;

            return (promoteCodeModel != null
                && promoteCodeModel.Region != null);
        }

        /// <summary>
        /// Excecutes the specified deploy entity.
        /// </summary>
        /// <param name="deployEntity">The deploy entity.</param>S
        public void Excecute(dynamic deployEntity)
        {
            var promoteCodeModel = (PromoteCodeModel) deployEntity;
            
            //initialize config file reader
            var configFileReader = new ConfigFileReader();

            //read config file based on region givenS
            ServerDetailsModel serverDetails = configFileReader.ReadConfigFile(new ServerDetailsModel
            {
                Region = promoteCodeModel.Region
            });

            //get the source file name by concatenating sourcelocation and filename
            serverDetails.SourceFileName = serverDetails.SourceFileLocation + "\\" + promoteCodeModel.FileName;

            //create the destination directory if uit does now exists
            if (!Directory.Exists(serverDetails.DestinationFileLocation))
                Directory.CreateDirectory(serverDetails.DestinationFileLocation);
        }

        #endregion
    }
}