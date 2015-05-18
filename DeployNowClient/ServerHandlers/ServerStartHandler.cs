using System.Configuration;
using System.Net.Http;

namespace DeployNowClient.ServerHandlers
{
    /// <summary>
    /// server start handler
    /// </summary>
    public class ServerStartHandler
    {
        /// <summary>
        /// Starts the server command.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage StartServerCommand(string url)
        {
            var request = new HttpClient();
            return request.GetAsync(url + "/api/ServerReset?action=START").Result;
        }
    }
}