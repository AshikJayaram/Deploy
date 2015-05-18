using System.Configuration;
using System.Net.Http;
using DeployNowClient.Models;

namespace DeployNowClient.ServerHandlers
{
    /// <summary>
    /// cache clear handler
    /// </summary>
    public class CacheClearHandler
    {
        /// <summary>
        /// Caches the clear handler.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage CacheClearCommand(string url)
        {
            var request = new HttpClient();
            return request.GetAsync(url +"/api/ClearCache").Result;
        }
    }
}