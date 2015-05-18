using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeployNowClient.ServerHandlers
{
    /// <summary>
    /// server stop handler
    /// </summary>
    public class ServerStopHandler
    {
        /// <summary>
        /// Stops the server command.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public HttpResponseMessage StopServerCommand(string url)
        {
            var request = new HttpClient();
            return request.GetAsync(url + "/api/ServerReset?action=STOP").Result;
        }
    }
}