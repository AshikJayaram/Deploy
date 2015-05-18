using System.Net;
using System.Net.Http;
using System.Web.Http;
using DeployNowClient.Models;
using DeployNowClient.ServerHandlers;

namespace DeployNowClient.Controllers
{
    /// <summary>
    /// Server start controller to send request to start the server
    /// </summary>
    public class ServerStartController : ApiController
    {
        /// <summary>
        /// get the response from the server after server start task is completed
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get([FromUri] string region)
        {
            var serverDetails = new ConfigFileReader().ReadConfigFile(new ServerDetailsModel
            {
                Region = region
            });

            var serverResponse = new HttpResponseMessage();

            foreach (var server in serverDetails.ServerList)
            {
                serverResponse = new ServerStartHandler().StartServerCommand(server.ServerUrl);
            }

            return serverResponse.StatusCode == HttpStatusCode.OK
                       ? Request.CreateResponse(HttpStatusCode.OK, "Server is started")
                       : Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "server start task incomplete");
        }
    }
}