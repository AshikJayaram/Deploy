using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DeployNowClient.Models;
using DeployNowClient.ServerHandlers;

namespace DeployNowClient.Controllers
{
    /// <summary>
    /// Controller for moving the code to the designated folder and also calling the deploy listener service.
    /// </summary>
    public class PromoteCodeController : ApiController
    {
        /// <summary>
        /// The destination location
        /// </summary>
        private string DestinationLocation;

        /// <summary>
        /// Posts the specified promote code.
        /// </summary>
        /// <param name="promoteCode">The promote code.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post(PromoteCodeModel promoteCode)
        {
            if (String.IsNullOrEmpty(promoteCode.Region))
            {
                return base.Request.CreateResponse(HttpStatusCode.BadRequest, "Please specify the region!");
            }

            string sourceFileName = SelectSourceAndDestination(promoteCode);

            var serverDetails = new ConfigFileReader().ReadConfigFile(new ServerDetailsModel
                                                {
                                                    Region = promoteCode.Region
                                                });

            serverDetails.SourceFileName = sourceFileName;

            //copy destination location to a private variable
            this.DestinationLocation = serverDetails.DestinationFileLocation;
            
            if (!Directory.Exists(serverDetails.DestinationFileLocation))
                Directory.CreateDirectory(serverDetails.DestinationFileLocation);

            var fileProcess = new FileProcessHandler();

            //Call a method to perform Xcopy
            fileProcess.ProcessXcopy(serverDetails);
            fileProcess.DeleteOldFile(serverDetails);
            fileProcess.ReplaceConfigFile(serverDetails);

            //perform the deployment to all servers deployment 
            foreach (var server in serverDetails.ServerList)
            {
                await MoveCodeToServer(server);
            }

            return Request.CreateResponse(HttpStatusCode.Created, "Deployment Completed");
        }

        /// <summary>
        /// Selects the source and destination.
        /// </summary>
        /// <param name="promoteCode">The promote code.</param>
        private static string SelectSourceAndDestination(PromoteCodeModel promoteCode)
        {
            var configFileReader = new ConfigFileReader();

            ServerDetailsModel serverDetails = configFileReader.ReadConfigFile(new ServerDetailsModel
                                                {
                                                    Region = promoteCode.Region
                                                });

            var sourceFileName = serverDetails.SourceFileLocation + "\\" + promoteCode.FileName;

            return sourceFileName;
        }

        /// <summary>
        /// Moves the code to server.
        /// </summary>
        /// <returns></returns>
        private Task MoveCodeToServer(ServerList server)
        {
            //ToDo: Make a call to the deploy now listener.
            var dir = new DirectoryInfo(DestinationLocation);
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