using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeployListener
{
    /// <summary>
    /// Api controller to check whether the service is running
    /// </summary>
    public class PingController : ApiController
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST

            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();// Get the IP
            Console.WriteLine(ServiceFactory.Config.BaseAddress);
            return this.Request.CreateResponse(HttpStatusCode.OK,
                                               "Deploy Listener service is running and ready for deployment at host " + myIP + " and Port " + ServiceFactory.Config.BaseAddress.Port);
        }
    }
}