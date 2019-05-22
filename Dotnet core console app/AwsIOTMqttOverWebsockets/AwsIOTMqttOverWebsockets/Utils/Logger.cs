using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using log4net;
using log4net.Config;
namespace AwsIOTMqttOverWebsockets.Utils
{
    public static class Logger
    {
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static bool IsLog4netConfigured;

        public static void LogInfo(string message)
        {
            if (! IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }
            log.Info(message);
        }

        public static void LogDebug(string message)
        {
            if (!IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }

            log.Debug(message);
        }


        public static void LogError(string message)
        {
            if (!IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }
            log.Error(message);
        }


        public static void LogFatal(string message)
        {
            if (!IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }

            log.Fatal(message);
        }


        public static void LogWarn(string message)
        {
            if (!IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }
            log.Warn(message);
        }

        private static void ConfigureLog4Net()
        { 
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            IsLog4netConfigured = true;
        }

    }
}
