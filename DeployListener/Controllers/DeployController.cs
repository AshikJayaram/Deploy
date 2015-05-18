using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DeployListener.DeployCommands;

namespace DeployListener.Controllers
{
    /// <summary>
    /// Deploy Api Controller
    /// </summary>
    public class DeployController : ApiController
    {
        #region private members

        /// <summary>
        /// The deployment request handler
        /// </summary>
        private readonly IDeploymentRequestHandler deploymentRequestHandler;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DeployController"/> class.
        /// </summary>
        /// <param name="deploymentRequestHandler">The deployment request handler.</param>
        public DeployController(IDeploymentRequestHandler deploymentRequestHandler)
        {
            this.deploymentRequestHandler = deploymentRequestHandler;
        }

        #endregion

        #region method

        /// <summary>
        /// Posts this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post()
        {
            //check if the request does't have Zip file
            if (!HttpContentMultipartExtensions.IsMimeMultipartContent(this.Request.Content))
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Expected Zip folder for deployment");
            }

            //Zip files in the request data will be put into current directory
            string requestBackupPath = Path.Combine(Environment.CurrentDirectory, "RequestZipFiles");
            
            Console.WriteLine(requestBackupPath);

            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(requestBackupPath);

            await
                HttpContentMultipartExtensions.ReadAsMultipartAsync<MultipartFormDataStreamProvider>(
                    this.Request.Content,
                    streamProvider);
            var sourcePath =
                Enumerable.FirstOrDefault<string>(
                    Enumerable.Select((IEnumerable<MultipartFileData>) streamProvider.FileData,
                                      (p => p.LocalFileName)));
            try
            {
                //getting the deploy path
                var deployPath = streamProvider.FormData.Get("Directory");
                //const string deployPath = "C:\\Test"; //for testing locally using fiddler

                //creates the deploy path if it does not exists
                if (!Directory.Exists(deployPath))
                {
                    Directory.CreateDirectory(deployPath);
                }

                this.deploymentRequestHandler.Handle(new DeployRequestDto
                                                         {
                                                             SourcePath = sourcePath,
                                                             DeployPath = deployPath
                                                         });

                //to clear the stream provider folder
                this.ClearStreamProvider(requestBackupPath);
            }
            catch (AggregateException ex)
            {
                DeployLogger.LogMessage.Error(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                DeployLogger.LogMessage.Error(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.OK,
                                                    "Deployment Completed");
        }

        #endregion

        #region private methods

        /// <summary>
        /// Clears the stream provider.
        /// </summary>
        /// <param name="requestBackupPath">The request backup path.</param>
        private void ClearStreamProvider(string requestBackupPath)
        {
            var dir = new DirectoryInfo(requestBackupPath);

            if(Directory.EnumerateFileSystemEntries(requestBackupPath).Any())
            {
                //for deleting all the files
                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }

                //for deleting all the directories
                foreach (var d in dir.GetDirectories())
                {
                    d.Delete(true);
                } 
            }
        }

        #endregion
    }
}
