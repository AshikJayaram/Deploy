using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using DeployNowClient.Models;

namespace DeployNowClient.ServerHandlers
{
    /// <summary>
    /// File Process handler
    /// </summary>
    public class FileProcessHandler
    {
        /// <summary>
        /// Processes the xcopy.
        /// </summary>
        /// <param name="serverDetails">The server details.</param>
        public void ProcessXcopy(ServerDetailsModel serverDetails)
        {
            // Use ProcessStartInfo class
            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            //Give the name as Xcopy
            startInfo.FileName = "xcopy";
            //make the window Hidden
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //Send the Source and destination as Arguments to the process
            startInfo.Arguments = "\"" + serverDetails.SourceFileName + "\"" + " " + "\"" + serverDetails.DestinationFileLocation + "\"" + @" /e /y /I";
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    exeProcess.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Replaces the config file.
        /// </summary>
        /// <param name="serverDetails">The server details.</param>
        public void ReplaceConfigFile(ServerDetailsModel serverDetails)
        {
            var dir = new DirectoryInfo(serverDetails.DestinationFileLocation);
            FileInfo fileDetails = dir.EnumerateFiles("*.zip", SearchOption.AllDirectories).First();
            ZipArchive archive = ZipFile.Open(fileDetails.FullName, ZipArchiveMode.Update);
            //Gets the Web.config file for the server.
            IEnumerable<FileInfo> configFiles = dir.EnumerateFiles("*.config", SearchOption.AllDirectories);
            if (configFiles.Any())
            {
                FileInfo configFileDetails = dir.EnumerateFiles("*.config", SearchOption.AllDirectories).First();
                archive.Entries.First(p => p.Name == "Web.config").Delete();
                if (archive.Entries.Any(p => p.Name == "Web.Debug.config"))
                {
                    archive.Entries.First(p => p.Name == "Web.Debug.config").Delete();
                }
                if (archive.Entries.Any(p => p.Name == "Web.Release.config"))
                {
                    archive.Entries.First(p => p.Name == "Web.Release.config").Delete();
                }
                //Copies the web.config file to the zip file.
                archive.CreateEntryFromFile(configFileDetails.FullName, "Web.config");
            }
            archive.Dispose();
        }

        /// <summary>
        /// Deletes the old file.
        /// </summary>
        /// <param name="serverDetails">The server details.</param>
        public void DeleteOldFile(ServerDetailsModel serverDetails)
        {
            var dir = new DirectoryInfo(serverDetails.DestinationFileLocation);
            int fCount = Directory.GetFiles(serverDetails.DestinationFileLocation, "*.zip", SearchOption.AllDirectories).Length;
            if (fCount > serverDetails.FileLimit)
            {
                FileInfo oldFileName =
                    dir.EnumerateFiles("*.zip", SearchOption.AllDirectories).OrderBy(d => d.CreationTime).First();
                File.Delete(serverDetails.DestinationFileLocation + "\\" + oldFileName);
            }
        }
    }
}