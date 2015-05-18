using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DeployNowClient.Models;

namespace DeployNowClient.Controllers
{
    /// <summary>
    /// Server configuration controller
    /// </summary>
    public class ServerConfigController : ApiController
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            var serverConfigDetails = new ConfigFileReader().GetAllServerDetailsFromConfig();
            return serverConfigDetails.Any()
                ? Request.CreateResponse(HttpStatusCode.OK, serverConfigDetails)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "No results found");
        }

        /// <summary>
        /// Puts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public HttpResponseMessage Put(ServerDetailsModel input)
        {
            var configFileWriter = new ConfigFileWriter().UpdateConfigFile(input);
            return configFileWriter 
                ? Request.CreateResponse(HttpStatusCode.Created, "Updated successfully") 
                : Request.CreateErrorResponse(HttpStatusCode.NotModified, "Config file not modified");
        }
    }
}

//{
//          Region= "DEV",
//          SourceFileLocation= "C:\\BUILDS",
//          DestinationFileLocation= "C:\\DEV",
//          SourceFileName= null,
//          FileLimit= 10,
//          ConfigFileLocation= "",
//          ServerList= new List<ServerList>()
//                          {
//                              new ServerList
//                                  {
//                                      ServerUrl = "http://10.157.84.4:8172/",
//                                      DeployDirectory= "D:\\PapillonDEVMaster"
//                                  }
//                          }
// }