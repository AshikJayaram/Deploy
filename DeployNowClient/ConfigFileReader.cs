using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DeployNowClient.Models;
using Newtonsoft.Json;

namespace DeployNowClient
{
    /// <summary>
    /// Config File Reader
    /// </summary>
    public class ConfigFileReader
    {
        /// <summary>
        /// Reads the config file.
        /// </summary>
        /// <param name="serverDetailsRequest">The server details request.</param>
        /// <returns></returns>
        public ServerDetailsModel ReadConfigFile(ServerDetailsModel serverDetailsRequest)
        {
            //string path = Path.Combine(Environment.CurrentDirectory, "Config.json");
            //for testing purpose only
            const string path = @"D:\\DeployNowClient\\Config.json";
            
            var configList = JsonConvert.DeserializeObject<List<ServerDetailsModel>>(File.ReadAllText(path));


            var serverDetailsResponse = configList.FirstOrDefault(p => p.Region == serverDetailsRequest.Region);

            return (ServerDetailsModel)serverDetailsResponse;
        }

        /// <summary>
        /// Gets the URL from config.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns></returns>
        public static IList<string> GetUrlFromConfig(string region)
        {
            var serverUrls = new List<string>();
            //string path = Path.Combine(Environment.CurrentDirectory, "Config.json");
            //for testing purpose only
            const string path = @"D:\\DeployNowClient\\Config.json";

            var configList = JsonConvert.DeserializeObject<List<ServerDetailsModel>>(File.ReadAllText(path));

            var serverDetailsResponse = configList.FirstOrDefault(p => p.Region == region);

            if (serverDetailsResponse != null)
            {
                serverUrls.AddRange(serverDetailsResponse.ServerList.Select(server => server.ServerUrl));
                return serverUrls.Any() ? serverUrls : null;
            }
            return null;
        }
    }
}