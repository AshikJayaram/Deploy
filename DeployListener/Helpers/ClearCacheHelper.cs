using DeployListener.DeployCommands;
using ServiceStack.Redis;

namespace DeployListener.Helpers
{
    /// <summary>
    /// To clear the redis cache
    /// </summary>
    public class ClearCacheHelper : IClearCacheHelper
    {
        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Handle(DeployRequestDto request)
        {
            var client = new RedisClient();
            client.FlushAll();
        }
    }
}
