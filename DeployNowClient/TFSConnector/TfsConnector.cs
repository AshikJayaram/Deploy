using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;

namespace DeployNowClient.TFSConnector
{
    /// <summary>
    /// Connects to TFS and give an options to get build details.
    /// </summary>
    public class TfsConnector
    {
        /// <summary>
        /// The build detail spec
        /// </summary>
        private IBuildDetailSpec buildDetailSpec;

        /// <summary>
        /// The build server
        /// </summary>
        private IBuildServer buildServer;

        /// <summary>
        /// Connects the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <returns></returns>
        public TfsConnector Connect(string url, string projectName)
        {
            var tfs = new TfsTeamProjectCollection(new Uri(url));

            this.buildServer = (IBuildServer) tfs.GetService(typeof (IBuildServer));
            this.buildDetailSpec = this.buildServer.CreateBuildDetailSpec(projectName);

            return this;
        }

        /// <summary>
        /// Gets the build number for current build.
        /// </summary>
        /// <param name="buildDefinationName">Name of the build defination.</param>
        /// <returns></returns>
        public string GetBuildNumberForCurrentBuild(string buildDefinationName)
        {
            buildDetailSpec.DefinitionSpec.Name = buildDefinationName;
            buildDetailSpec.QueryOrder = BuildQueryOrder.StartTimeAscending;
            buildDetailSpec.Status = BuildStatus.InProgress;
            var results = buildServer.QueryBuilds(buildDetailSpec);
            var latest = results.Builds.FirstOrDefault();
            return latest != null ? latest.BuildNumber : String.Empty;
        }

        /// <summary>
        /// Gets the associtaed change sets.
        /// </summary>
        /// <param name="buildNumber">The build number.</param>
        /// <returns></returns>
        public List<IChangesetSummary> GetAssocitaedChangeSets(string buildNumber)
        {
            this.buildDetailSpec.BuildNumber = buildNumber;
            var results = buildServer.QueryBuilds(buildDetailSpec);
            var latest = results.Builds.FirstOrDefault();
            return(latest != null
                                                  ? InformationNodeConverters.GetAssociatedChangesets(latest.Information)
                                                  : Enumerable.Empty<IChangesetSummary>().ToList());
        }
    }
}