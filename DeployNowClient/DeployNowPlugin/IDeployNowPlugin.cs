namespace DeployNowClient.DeployNowPlugin
{
    /// <summary>
    /// deploy now plugin interface that is used to process all functionalities that happens while deployment
    /// </summary>
    public interface IDeployNowPlugin
    {
        #region methods

        /// <summary>
        /// Determines whether this instance can excecute the specified deploy entity.
        /// </summary>
        /// <param name="deployEntity">The deploy entity.</param>
        /// <returns>
        ///   <c>true</c> if this instance can excecute the specified deploy entity; otherwise, <c>false</c>.
        /// </returns>
        bool CanExcecute(dynamic deployEntity);

        /// <summary>
        /// Excecutes the specified deploy entity.
        /// </summary>
        /// <param name="deployEntity">The deploy entity.</param>
        void Excecute(dynamic deployEntity);

        #endregion
    }
}
