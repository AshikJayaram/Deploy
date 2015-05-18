using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DeployNowClient.Models;

namespace DeployNowClient.DeployNowPlugin
{
    /// <summary>
    /// processes moving code to given server lists
    /// </summary>
    public class DeployProcessor
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
                && serverDetailsModel.ServerList != null
                && serverDetailsModel.ServerList.Any());
        }

        /// <summary>
        /// Excecutes the specified deploy entity.
        /// </summary>
        /// <param name="deployEntity">The deploy entity.</param>
        public async void Excecute(dynamic deployEntity)
        {
            //typecasting dynamic model to ServiceDetailsModel
            var serverDetails = (ServerDetailsModel)deployEntity;

            foreach (var server in serverDetails.ServerList)
            {
                await MoveCodeToServer(server,serverDetails.DestinationFileLocation);
            }
        }

        /// <summary>
        /// Moves the code to server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="destinationLocation">The destination location.</param>
        /// <returns></returns>
        private Task MoveCodeToServer(ServerList server,string destinationLocation)
        {
            var dir = new DirectoryInfo(destinationLocation);
            FileInfo fileDetails = dir.EnumerateFiles("*.zip", SearchOption.AllDirectories).First();
            var client = new HttpClient();
            var message = new HttpRequestMessage();
            var content = new MultipartFormDataContent();
            var filestream = new FileStream(fileDetails.FullName, FileMode.Open);
            string fileName = Path.GetFileName(fileDetails.FullName);
            content.Add(new StreamContent(filestream), "file", fileName);
            content.Add(new StringContent(server.DeployDirectory), "Directory");
            message.Method = HttpMethod.Post;
            message.Content = content;
            message.RequestUri = new Uri(server.ServerUrl + "/api/Deploy");
            return client.SendAsync(message).ContinueWith(task => { task.Result.EnsureSuccessStatusCode(); });
        }
    }
}