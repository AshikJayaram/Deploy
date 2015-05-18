using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using DeployListener.DeployCommands;
using DeployListener.Models;
using Oracle.ManagedDataAccess.Client;

namespace DeployListener.Helpers
{
    /// <summary>
    /// Executes the SQL Script
    /// </summary>
    public class SqlScriptExecutor : ISqlScriptExecutor
    {
        #region methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Handle(DeployRequestDto request)
        {
           //Get the connection string
            var sqlInputs = GetConnectionString(request);

            //get the scripts folder
            var directory = request.IntermediateDirectory;

            //get the files in the folder
            string[] fileEntries = Directory.GetFiles(directory);
            foreach (string fileName in fileEntries)
            {
                try
                {
                    string content = string.Format(File.ReadAllText(fileName));
                    using (var oracleConnection = new OracleConnection(sqlInputs.ConnectionString))
                    {
                        oracleConnection.Open();
                        var command = new OracleCommand();
                        var script = content.Replace("\t", " ");
                        script = script.Replace("\n", Environment.NewLine);
                        command.Connection = oracleConnection;
                        command.CommandText = script;
                        var result = command.ExecuteNonQuery();
                        Debug.Write(result);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Write("Exception message: " + ex.Message);
                }
            }
        }

        #endregion

        #region private members

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private SqlExecutionConfigModel GetConnectionString (DeployRequestDto request)
        {
            var sqlInput = new SqlExecutionConfigModel();

            var webConfig = new XmlDocument();
            //string webConfigPath = "C:\\Papillon\\Web.config";

            string webConfigPath = request.WebConfigPath;
            webConfig.Load(webConfigPath);
            var connectionString = webConfig.SelectSingleNode("//connectionStrings");
            if (connectionString != null)
            {
                var sqlConnectionString = connectionString.ChildNodes[0].Attributes[1].Value;
                var index = IndexOfNth(sqlConnectionString, 3);
                sqlInput.ConnectionString = sqlConnectionString.Substring(0, index);
            }
            return sqlInput;
        }

        /// <summary>
        /// Indexes the of NTH.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        private static int IndexOfNth(string connectionString, int count)
        {
            int index = -1;
            const char semicolon = ';';
            for (int i = 0; i < count; i++)
            {
                index = connectionString.IndexOf(semicolon, index + 1);

                if (index == -1) break;
            }
            return index;
        }

        #endregion
    }
}
