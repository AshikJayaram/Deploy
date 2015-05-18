using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DeployNowClient.Models;

namespace DeployNowClient.Controllers
{
    /// <summary>
    /// Controller for moving the sql scripts to the designated folder.
    /// </summary>
    public class PromoteSqlScriptsController : ApiController
    {
        private string DestinationLocation;
        private string SourceLocation;
        private string ServerDeploymentDirectory;
        private string IntermediateDirectory;
        private string WebConfigPath;

        /// <summary>
        /// Posts the specified promote code.
        /// </summary>
        /// <param name="promoteCode">The promote code.</param>
        /// <returns></returns>
        public HttpResponseMessage Post(PromoteCodeModel promoteCode)
        {
            if (String.IsNullOrEmpty(promoteCode.Region))
            {
                return base.Request.CreateResponse(HttpStatusCode.BadRequest, "Please specify the region.");
            }

            SelectSourceAndDestination(promoteCode);
            GetConnectionString(promoteCode.Region);

            if (!Directory.Exists(DestinationLocation))
                Directory.CreateDirectory(DestinationLocation);

            CopyFilesAndZip("\\Scripts.zip");

            MoveCodeToServer();
            return base.Request.CreateResponse(HttpStatusCode.Created, "Copy was successful.");
        }

        /// <summary>
        /// Selects the source and destination.
        /// </summary>
        /// <param name="promoteCode">The promote code.</param>
        private void SelectSourceAndDestination(PromoteCodeModel promoteCode)
        {
            if (promoteCode.Region == "DEV")
            {
                SourceLocation = GetFromConfig("SqlBUILDS");
                DestinationLocation = GetFromConfig("SqlDEV");
                ServerDeploymentDirectory = GetFromConfig("SqlDevDeploymentDirectory");
                IntermediateDirectory = GetFromConfig("SqlTempDirectory");
            }
            if (promoteCode.Region == "ST")
            {
                SourceLocation = GetFromConfig("SqlDEV");
                DestinationLocation = GetFromConfig("SqlST");
                ServerDeploymentDirectory = GetFromConfig("SqlSTDeploymentDirectory");
                IntermediateDirectory = GetFromConfig("SqlTempDirectory");
            }
            if (promoteCode.Region == "UAT")
            {
                SourceLocation = GetFromConfig("SqlST");
                DestinationLocation = GetFromConfig("SqlUAT");
                ServerDeploymentDirectory = GetFromConfig("SqlSTDeploymentDirectory");
                IntermediateDirectory = GetFromConfig("SqlTempDirectory");
            }
        }

        /// <summary>
        /// Moves the code to server.
        /// </summary>
        /// <returns></returns>
        private void MoveCodeToServer()
        {
            //ToDo: Make a call to the deploy now listener.
            var dir = new DirectoryInfo(DestinationLocation);
            var fileDetails = dir.EnumerateFiles("*.zip", SearchOption.AllDirectories).First();
            var client = new HttpClient();
            var message = new HttpRequestMessage();
            var content = new MultipartFormDataContent();
            var filestream = new FileStream(fileDetails.FullName, FileMode.Open);
            var fileName = Path.GetFileName(fileDetails.FullName);
            content.Add(new StreamContent(filestream), "file", fileName);
            content.Add(new StringContent(ServerDeploymentDirectory), "Directory");
            content.Add(new StringContent(IntermediateDirectory), "TempDirectory");
            content.Add(new StringContent(WebConfigPath), "WebConfigPath");
            message.Method = HttpMethod.Post;
            message.Content = content;
            message.RequestUri = new Uri(GetFromConfig("SqlExecutionListener"));
            client.SendAsync(message).ContinueWith(task =>
            {
                task.Result.EnsureSuccessStatusCode();

            });
        }

        /// <summary>
        /// Gets from config.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string GetFromConfig(string key)
        { 
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Copies the files and zip.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private void CopyFilesAndZip(string fileName)
        {
            ZipFile.CreateFromDirectory(SourceLocation, DestinationLocation + fileName);
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="region">The region.</param>
        private void GetConnectionString(string region)
        {
            var fileReader = new ConfigFileReader();
            var serverDetails = fileReader.ReadConfigFile
                (new ServerDetailsModel
                     {
                         Region = region
                     });
            WebConfigPath = serverDetails.ServerList.FirstOrDefault().DeployDirectory + "\\Web.config";
        }
    }
}