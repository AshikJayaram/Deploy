using System.Linq;
using DeployNowClient.Models;
using DeployNowClient.ServerHandlers;

namespace DeployNowClient.DeployNowPlugin
{
    /// <summary>
    /// Process to excecte serevr stop task
    /// </summary>
    public class ServerStopProcessor
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
            var serverDetailsModel = (ServerDetailsModel) deployEntity;

            return (serverDetailsModel != null
                && serverDetailsModel.Region != null
                && serverDetailsModel.ServerList.Any());
        }

        /// <summary>
        /// Excecutes the specified deploy entity.
        /// </summary>
        /// <param name="deployEntity">The deploy entity.</param>
        public void Excecute(dynamic deployEntity)
        {
            //ToDo:yet to complete the entire task
            var serverDetailsModel = (ServerDetailsModel)deployEntity;

            var serverStopHandler = new ServerStopHandler();

            foreach (var server in serverDetailsModel.ServerList)
            {
                serverStopHandler.StopServerCommand(server.ServerUrl);
            }
        }
    }
}