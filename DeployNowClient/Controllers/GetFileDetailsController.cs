using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DeployNowClient.Models;

namespace DeployNowClient.Controllers
{
    /// <summary>
    /// Controller for getting the file information in destination folder.
    /// </summary>
    public class GetFileDetailsController : ApiController
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            var buildRegion = GetFileDetails("BUILDS");
            var devRegion = GetFileDetails("DEV");
            var stRegion = GetFileDetails("ST");
            var uatRegion = GetFileDetails("UAT");
            var files = new List<FileDetailsModel>();
            if (buildRegion != null) files.AddRange(buildRegion);
            if (devRegion != null) files.AddRange(devRegion);
            if (stRegion != null) files.AddRange(stRegion);
            if (uatRegion != null) files.AddRange(uatRegion);
            return base.Request.CreateResponse(HttpStatusCode.OK, files);
        }

        /// <summary>
        /// Gets the specified region.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns></returns>
        public HttpResponseMessage Get([FromUri]string region)
        {
            var files = GetFileDetails(region);
            return base.Request.CreateResponse(HttpStatusCode.OK, files);
        }

        private List<FileDetailsModel> GetFileDetails(string location)
        {
            var dir = new DirectoryInfo(GetFromConfig(location));
            var allFiles =
                dir.EnumerateFiles("*.zip", SearchOption.AllDirectories).OrderByDescending(d => d.CreationTime);
            if(!allFiles.Any())
            {
                return null;
            }
            return allFiles.Select(f => new FileDetailsModel
            {
                FileName = f.Name,
                CreationTime = f.CreationTime,
                Region = f.Directory.Name
            }).ToList();
        } 

        /// <summary>
        /// Gets from config.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string GetFromConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}