using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using DeployListener.DeployCommands;
using DeployListener.Helpers;

namespace DeployListener.Controllers
{
    /// <summary>
    /// Controller to clear cache
    /// </summary>
    public class ClearCacheController : ApiController
    {
        #region private members

        /// <summary>
        /// The deployment request handler
        /// </summary>
        private readonly IClearCacheHelper clearCacheHelper;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearCacheController"/> class.
        /// </summary>
        /// <param name="clearCacheHelper">The clear cache helper.</param>
        public ClearCacheController(IClearCacheHelper clearCacheHelper)
        {
            this.clearCacheHelper = clearCacheHelper;
        }

        #endregion
        #region RESTful methods
        /// <summary>
        /// Gets the specified action.
        /// </summary>
        public HttpResponseMessage Get( )
        {
            DeployLogger.LogMessage.Info("Request for cache clear");
            this.clearCacheHelper.Handle(new DeployRequestDto());

            return this.Request.CreateResponse(HttpStatusCode.OK, "Cache clear task completed");
        }
        #endregion

    }
}
