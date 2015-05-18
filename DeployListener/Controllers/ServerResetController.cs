using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using DeployListener.DeployCommands;
using DeployListener.Helpers;

namespace DeployListener.Controllers
{
    /// <summary>
    /// Controller to start/stop/reset server
    /// </summary>
    public class ServerResetController : ApiController
    {
        #region Private members

        /// <summary>
        /// The server reset helper
        /// </summary>
        private readonly IServerResetHelper serverResetHelper;

        /// <summary>
        /// The server start helper
        /// </summary>
        private readonly IServerStartHelper serverStartHelper;

        /// <summary>
        /// The server stop helper
        /// </summary>
        private readonly IServerStopHelper serverStopHelper;

        #endregion

        #region Constructor

        public ServerResetController(
            IServerResetHelper serverResetHelper,
            IServerStartHelper serverStartHelper,
            IServerStopHelper serverStopHelper
            )
        {
            this.serverResetHelper = serverResetHelper;
            this.serverStartHelper = serverStartHelper;
            this.serverStopHelper = serverStopHelper;
        }

        #endregion
        #region RESTful methods

        /// <summary>
        /// Gets the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        public HttpResponseMessage Get([FromUri]string action)
        {
            switch(action)
            {
                case "START":
                    this.serverStartHelper.Handle(new DeployRequestDto());
                    break;

                case "STOP":
                    this.serverStopHelper.Handle(new DeployRequestDto());
                    break;

                default: 
                    this.serverResetHelper.Handle(new DeployRequestDto());
                    break;
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, "Server " + action + " task Completed");
        }
        #endregion
    }
}
