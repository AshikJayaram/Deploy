using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DeployListener.DeployCommands;
using DeployListener.Helpers;

namespace DeployListener.Controllers
{
    /// <summary>
    /// Api Controller to deploy and execute the SQL scripts.
    /// </summary>
    public class SqlExecutionController : ApiController
    {
        #region private members

        /// <summary>
        /// The SQL script executor
        /// </summary>
        private readonly ISqlScriptExecutor sqlScriptExecutor;

        /// <summary>
        /// The deployment request handler
        /// </summary>
        private readonly IDeploymentRequestHandler deploymentRequestHandler;

        /// <summary>
        /// The deployment request handler
        /// </summary>
        private readonly ISqlDeploymentRequestHandler sqlDeploymentRequestHandler;

        /// <summary>
        /// The clear source scripts helper
        /// </summary>
        private readonly IClearSourceScriptsHelper clearSourceScriptsHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExecutionController"/> class.
        /// </summary>
        /// <param name="sqlScriptExecutor">The SQL script executor.</param>
        /// <param name="deploymentRequestHandler">The deployment request handler.</param>
        /// <param name="sqlDeploymentRequestHandler">The SQL deployment request handler.</param>
        /// <param name="clearSourceScriptsHelper">The clear source scripts helper.</param>
        public SqlExecutionController(ISqlScriptExecutor sqlScriptExecutor,
            IDeploymentRequestHandler deploymentRequestHandler, ISqlDeploymentRequestHandler sqlDeploymentRequestHandler,
            IClearSourceScriptsHelper clearSourceScriptsHelper)
        {
            this.sqlScriptExecutor = sqlScriptExecutor;
            this.deploymentRequestHandler = deploymentRequestHandler;
            this.sqlDeploymentRequestHandler = sqlDeploymentRequestHandler;
            this.clearSourceScriptsHelper = clearSourceScriptsHelper;
        }

        #endregion

        #region RESTful methods
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
            string sqlBackUpPath = Path.Combine(Environment.CurrentDirectory, "RequestZipFiles");
            Console.WriteLine(sqlBackUpPath);

            var streamProvider = new MultipartFormDataStreamProvider(sqlBackUpPath);

            await
                HttpContentMultipartExtensions.ReadAsMultipartAsync<MultipartFormDataStreamProvider>(
                    this.Request.Content,
                    streamProvider);

            var sourcePath =
                Enumerable.FirstOrDefault<string>(
                    Enumerable.Select((IEnumerable<MultipartFileData>)streamProvider.FileData,
                                      (p => p.LocalFileName)));
            try
            {
                //getting the deploy path
                var deployPath = streamProvider.FormData.Get("Directory");
                var tempDirectory = streamProvider.FormData.Get("TempDirectory");
                var webConfigPath = streamProvider.FormData.Get("WebConfigPath");

                //var deployPath = "C:\\Papillon\\DEVSQLScripts";
                //var tempDirectory = "C:\\SQLScripts";
                var deployRequest = new DeployRequestDto
                                        {
                                            SourcePath = sourcePath,
                                            DeployPath = deployPath,
                                            IntermediateDirectory = tempDirectory,
                                            WebConfigPath = webConfigPath
                                        };

                //creates the temporary directory to store the sql files if it does not exists.
                if (!Directory.Exists(tempDirectory))
                {
                    Directory.CreateDirectory(tempDirectory);
                }

                // Store the SQL files in the temporary directory.
                this.sqlDeploymentRequestHandler.Handle(deployRequest);

                //Execute all the files available in intermediate directory.
                this.sqlScriptExecutor.Handle(deployRequest);

                //creates the deploy path if it does not exists
                if (!Directory.Exists(deployPath))
                {
                    Directory.CreateDirectory(deployPath);
                }

                // Store the SQL files in the destination.
                this.deploymentRequestHandler.Handle(deployRequest);

                //Deletes the sql scripts from the source server.
                this.clearSourceScriptsHelper.Handle(deployRequest);

                return (Request.CreateResponse(HttpStatusCode.OK, "SQL Scripts Deployed and Executed Successfully."));
            }
            catch (AggregateException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}
