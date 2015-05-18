using System;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Configuration;

namespace DeployListener
{
    /// <summary>
    /// Service factory to start and stop the windows service
    /// </summary>
    public class ServiceFactory
    {
        /// <summary>
        /// The server
        /// </summary>
        private HttpSelfHostServer Server;

        /// <summary>
        /// The config
        /// </summary>
        public static HttpSelfHostConfiguration Config;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            string baseAddress = ConfigurationManager.AppSettings["HostAddress"];

            //configure the service running at the host address defined in app.config
            HttpSelfHostConfiguration httpHostConfig = new HttpSelfHostConfiguration(baseAddress);

            // define the maximum recieved message size
            httpHostConfig.MaxReceivedMessageSize = long.MaxValue;

            //transfer mode is made streamed where it buffers the message headers and expose the message body as stream
            httpHostConfig.TransferMode = TransferMode.Streamed;

            //Always include error details.
            httpHostConfig.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            HttpSelfHostConfiguration configuration = httpHostConfig;

            Config = httpHostConfig;
 
            // Configure web api controller
            new BootStrap().ConfigureContainer(configuration).CreateRequiredFolders();

            //configure log4net logger 
            DeployLogger.SetupLogger();

            DeployLogger.LogMessage.Info("Logger initiated");
            this.Server = new HttpSelfHostServer(configuration);
            this.Server.OpenAsync().Wait();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            this.Server.CloseAsync().Wait();
        }
    }
}
