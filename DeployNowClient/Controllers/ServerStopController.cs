using System.Net;
using System.Net.Http;
using System.Web.Http;
using DeployNowClient.Models;
using DeployNowClient.ServerHandlers;

namespace DeployNowClient.Controllers
{
    /// <summary>
    /// Server stop controller to send request to start the server
    /// </summary>
    public class ServerStopController : ApiController
    {
        /// <summary>
        /// Gets this instance.
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
                serverResponse = new ServerStopHandler().StopServerCommand(server.ServerUrl);
            }
            return serverResponse.StatusCode == HttpStatusCode.OK
                       ? Request.CreateResponse(HttpStatusCode.OK, "Server is stopped")
                       : Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "server stop task incomplete");
        }
    }
}