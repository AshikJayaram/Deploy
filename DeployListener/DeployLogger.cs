using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace DeployListener
{
    /// <summary>
    /// log4net configurator
    /// </summary>
    public class DeployLogger
    {
        /// <summary>
        /// The log message
        /// </summary>
        public static readonly ILog LogMessage = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Setups the logger configuration. Can be used instead of loading log4net.config file
        /// </summary>
        public static void SetupLogger()
        {
            Hierarchy hierarchy = (Hierarchy) LogManager.GetRepository();

            PatternLayout patternLayout = new DynamicPatternLayout();
            patternLayout.ConversionPattern = "%date{dd MMM yyyy HH:mm:ss} [%thread] %level   -   %message %exception %newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender
                                             {
                                                 // appends the log into same file
                                                 AppendToFile = true,
                                                 File = @"DeployLogs\\Deploylog.log",
                                                 Layout = patternLayout,
                                                 MaxSizeRollBackups = 1000,
                                                 RollingStyle = RollingFileAppender.RollingMode.Date,
                                                 StaticLogFileName = false
                                             };

            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memoryAppender = new MemoryAppender();
            memoryAppender.ActivateOptions();

            hierarchy.Root.AddAppender(memoryAppender);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }
    }
}
