using System.Net;
using System.Net.Http;
using System.Web.Http;
using DeployNowClient.Models;
using DeployNowClient.ServerHandlers;

namespace DeployNowClient.Controllers
{
    /// <summary>
    /// cache clear controller
    /// </summary>
    public class CacheClearController : ApiController
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get([FromUri]string region)
        {
            var serverDetails = new ConfigFileReader().ReadConfigFile(new ServerDetailsModel
            {
                Region = region
            });

            var serverResponse = new HttpResponseMessage();

            foreach (var server in serverDetails.ServerList)
            {
                serverResponse = new CacheClearHandler().CacheClearCommand(server.ServerUrl);
            }
            return serverResponse.StatusCode == HttpStatusCode.OK
                       ? Request.CreateResponse(HttpStatusCode.OK, "resdis cache cleared")
                       : Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "cache clear task incomplete");
        }
    }
}