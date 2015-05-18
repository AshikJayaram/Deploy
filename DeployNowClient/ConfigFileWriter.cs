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
    /// writes the json file
    /// </summary>
    public class ConfigFileWriter
    {
        /// <summary>
        /// Updates the config file.
        /// </summary>
        /// <param name="serverModels">The server models.</param>
        /// <returns></returns>
        public bool UpdateConfigFile(ServerDetailsModel serverModels)
        {
            const string path = @"C:\\Shared\\Dev\\DeployNow\\DeployNowClient\\Config.json";

            var configList = JsonConvert.DeserializeObject<List<ServerDetailsModel>>(File.ReadAllText(path));

            foreach (var config in configList.Where(config => serverModels.Region != null && config.Region == serverModels.Region))
            {
                //mapping the previous values to modified value
                config.ServerList = serverModels.ServerList;
            }
            var output = Newtonsoft.Json.JsonConvert.SerializeObject(configList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, output);
            return true;
        }
    }
}