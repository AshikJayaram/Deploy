using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DeployNowClient.Models;
using TestingDeployment.SetupTests.Tests;

namespace DeployNowClient.Controllers
{
    /// <summary>
    /// Api controller to invoke iut test cases
    /// </summary>
    public class IutController : ApiController
    {
        /// <summary>
        /// Gets the specified region.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns></returns>
        public HttpResponseMessage Get([FromUri]string region)
        {
            var serverDetails = new ConfigFileReader().ReadConfigFile(new ServerDetailsModel
            {
                Region = region
            });

            foreach (var server in serverDetails.ServerList)
            {
                using (var iutSetup = new SetupTestWithPage(server.ServerUrl))
                {
                    iutSetup.OpenProjectWebPage();
                    iutSetup.Dispose();
                }
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, "Tests successfully launched");
        }
    }
}