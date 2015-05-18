using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DeployNowClient.Controllers
{
    public class BuildNumberController : ApiController
    {
        //
        // GET: /api/BuildNumber?projectName=Papillon&buildDefinationName=Papillon.Dev.DeployToDev

        public string Get(string projectName, string buildDefinationName)
        {
            //var buildDefinationName = "Papillon.Dev.DeployToDev";
            //var buildNumber = "Papillon.Dev.DeployToDev_20150331.2";
            //var projectName = "Papillon";
            string url = ConfigurationManager.AppSettings["TFSServerUrl"];
            return
                new TFSConnector.TfsConnector().Connect(url, projectName).GetBuildNumberForCurrentBuild(buildDefinationName);
        }

    }
}
