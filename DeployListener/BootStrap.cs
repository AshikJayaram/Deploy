using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using DeployListener.DeployCommands;
using DeployListener.Helpers;
using Microsoft.AspNet.SignalR.Hubs;
using StructureMap;

namespace DeployListener
{
    /// <summary>
    /// Configures the API.
    /// </summary>
    public class BootStrap
    {
        #region Private members

        /// <summary>
        /// Gets or sets the ioc container.
        /// </summary>
        /// <value>
        /// The ioc container.
        /// </value>
        public static IContainer IocContainer { get; set; }

        #endregion
        
        /// <summary>
        /// Creates the required folders.
        /// </summary>
        /// <returns></returns>
        public BootStrap CreateRequiredFolders()
        {
            string reqBackupZipFilespath = Path.Combine(Environment.CurrentDirectory, "RequestZipFiles");

            // Create RequestZipFiles directory if it does not exist
            if (!Directory.Exists(reqBackupZipFilespath))
            {
                Directory.CreateDirectory(reqBackupZipFilespath);
            }
            return this;
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <returns></returns>
        public BootStrap ConfigureContainer(HttpConfiguration configuration,IContainer iocContainer = null)
        {
            if (iocContainer == null)
            {
                // No. Create anew one.
                iocContainer = new Container();

                iocContainer.Configure
                    (
                        configure =>
                        {
                            configure.For<IHub>().Use<SignalrHub>();
                            configure.For<IDeploymentRequestHandler>().Use<DeployRequestHandler>();
                            configure.For<ISqlDeploymentRequestHandler>().Use<SqlDeploymentHandler>();
                            configure.For<IClearCacheHelper>().Use<ClearCacheHelper>();
                            configure.For<IServerResetHelper>().Use<ServerResetHelper>();
                            configure.For<IServerStartHelper>().Use<ServerStartHelper>();
                            configure.For<IServerStopHelper>().Use<ServerStopHelper>();
                            configure.For<ISqlScriptExecutor>().Use<SqlScriptExecutor>();
                            configure.For<IClearSourceScriptsHelper>().Use<ClearSourceScriptsHelper>();
                        }
                    );
            }
            HttpRouteCollectionExtensions.MapHttpRoute(configuration.Routes, "Api default", "api/{controller}/{id}", (object)new
            {
                id = RouteParameter.Optional
            });
            configuration.Services.Replace
                (
                    typeof(IHttpControllerActivator),
                    new StructureMapActivator(iocContainer)
                );
            return this;
        }
    }
}
