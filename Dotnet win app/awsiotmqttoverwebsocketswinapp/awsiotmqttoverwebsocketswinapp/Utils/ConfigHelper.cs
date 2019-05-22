using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using awsiotmqttoverwebsocketswinapp.Utils;

namespace awsiotmqttoverwebsocketswinapp.Utils
{
    public static class ConfigHelper
    {
        public static string ReadSetting(string key)
        {
            string result="NotFound";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "NotFound";
                
            }
            catch (ConfigurationErrorsException ex)
            {
                Logger.LogError(ex.Message);
                
            }

            return result;
        }
    }
}
