using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace DeployListener
{
    /// <summary>
    /// Structure map Activator
    /// </summary>
    public class StructureMapActivator
        : IHttpControllerActivator
    {
        #region

        /// <summary>
        /// The ioc container
        /// </summary>
        private readonly IContainer iocContainer;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapActivator"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        public StructureMapActivator(IContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }
        #endregion

        #region methods

        /// <summary>
        /// Creates an <see cref="T:System.Web.Http.Controllers.IHttpController" /> object.
        /// </summary>
        /// <param name="request">The message request.</param>
        /// <param name="controllerDescriptor">The HTTP controller descriptor.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// An <see cref="T:System.Web.Http.Controllers.IHttpController" /> object.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IContainer nestedContainer = this.iocContainer.GetNestedContainer();

            request.RegisterForDispose(nestedContainer);

            return nestedContainer.GetInstance(controllerType) as IHttpController;
        }

        #endregion
    }
}
