using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.TeamFoundation.Build.Client;
using DeployNowClient.Models;

namespace DeployNowClient.Controllers
{
    public class ChangeSetController : ApiController
    {
        //
        // GET: /api/ChangeSet?projectName=Papillon&buildNumber=Papillon.Dev.DeployToDev_20150331.2

        public List<ChangeSet> Get(string projectName, string buildNumber)
        {
            //var buildDefinationName = "Papillon.Dev.DeployToDev";
            //var buildNumber = "Papillon.Dev.DeployToDev_20150331.2";
            //var projectName = "Papillon";
            string url = ConfigurationManager.AppSettings["TFSServerUrl"];
            var changeSets = new List<ChangeSet>();
              var changeSetList=new TFSConnector.TfsConnector().Connect(url, projectName).GetAssocitaedChangeSets(buildNumber);
            changeSetList.ForEach(p=> changeSets.Add(new ChangeSet
                                                           {
                                                               Id = p.ChangesetId,
                                                               Comment = p.Comment
                                                           }));
            return changeSets;

        }

    }
}
