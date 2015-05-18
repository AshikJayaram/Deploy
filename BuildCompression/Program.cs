using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace BuildCompression
{
    /// <summary>
    /// Build compression
    /// </summary>
    public class Program
    {
        #region Private instance fields

        /// <summary>
        /// The verion number of each zipped file. 
        /// </summary>
        private static Version Ver;

        /// <summary>
        /// The XML doc with application configuration.
        /// </summary>
        private static XmlDocument XmlDoc;

        /// <summary>
        /// The source location of the build.
        /// </summary>
        private static string SourceLocation;

        /// <summary>
        /// The destination location to zip and copy the build.
        /// </summary>
        private static string DestinationLocation;

        /// <summary>
        /// The build version used to read the xml node.
        /// </summary>
        private static XmlNode BuildVersion;

        /// <summary>
        /// The no of builds that can exist in the destination folder.
        /// </summary>
        private static int NoOfBuilds;

        /// <summary>
        /// The zip file name
        /// </summary>
        private static string zipFileName;
        #endregion

        public static void Main(string[] args)
        {
            ConfigureApplication(args);
            GetZipFileName(args);
            CopyFilesAndZip(zipFileName);
            DeleteOldFiles();
        }

        /// <summary>
        /// Gets the name of the zip file. 
        /// </summary>
        /// <returns></returns>
        private static void GetZipFileName(string[] args)
        {
            using (var client = new HttpClient())
            {
                var uri = GetFromConfig("Server") + GetFromConfig("BuildNumber") + "projectName=" + args[2] +
                          "&buildDefinationName=" + GetFromConfig("BuildName");
                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content;
                    zipFileName = data.ReadAsStringAsync().Result;
                    var fileName = JsonConvert.DeserializeObject<String>(zipFileName);
                    zipFileName = fileName + ".zip";
                }
            }
        }

        /// <summary>
        /// Copies the files and zip.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private static void CopyFilesAndZip(string fileName)
        {
            ZipFile.CreateFromDirectory(SourceLocation, DestinationLocation + "\\" + fileName);
        }

        /// <summary>
        /// Deletes the old files from destination folder.
        /// </summary>
        private static void DeleteOldFiles()
        {
            int fCount = Directory.GetFiles(DestinationLocation, "*.zip", SearchOption.AllDirectories).Length;
            if (fCount > NoOfBuilds)
            {
                var dir = new DirectoryInfo(DestinationLocation);
                var oldFileName =
                    dir.EnumerateFiles("*.zip", SearchOption.AllDirectories).OrderBy(d => d.CreationTime).First();
                File.Delete(DestinationLocation + "\\" + oldFileName);
            }
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="args">The args.</param>
        private static void ConfigureApplication(string[] args)
        {
            XmlDoc = new XmlDocument();
            //XmlDoc.Load("C:\\BUILDS\\BuildCompressionConfig.xml");
            //BuildVersion = XmlDoc.SelectSingleNode("//BuildVersion");
            SourceLocation = args[0];
            DestinationLocation = args[1];
            NoOfBuilds = 10;
            //Int32.TryParse(XmlDoc.SelectSingleNode("//NoOfBuilds").InnerText, out NoOfBuilds);
        }

        /// <summary>
        /// Gets from config.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static string GetFromConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
