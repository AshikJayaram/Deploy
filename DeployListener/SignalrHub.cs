using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DeployListener
{
    /// <summary>
    /// creating a signalR hub
    /// </summary>
    [HubName("DeployListener.SignalrHub")]
    public sealed class SignalrHub : Hub , IHub
    {
        #region

        /// <summary>
        /// The hub context
        /// </summary>
        private readonly IHubContext hubContext;

        #endregion

        #region

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalrHub"/> class.
        /// </summary>
        public SignalrHub()
        {
            this.hubContext
                = GlobalHost.ConnectionManager.GetHubContext<SignalrHub>();
        }

        #endregion

        #region

        /// <summary>
        /// Publishes the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        public void Publish(string method, dynamic data)
        {
            var proxy = this.hubContext.Clients.All;
            proxy.Invoke(method, data);
        }

        #endregion
    }
}
