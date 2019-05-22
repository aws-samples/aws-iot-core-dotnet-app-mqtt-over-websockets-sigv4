using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;


namespace AwsIOTMqttOverWebsockets.Utils
{
    public static class ConfigHelper
    {
        public static string ReadSetting(string key)
        {
            string result = "NotFound";

            try
            {

            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", true, true)
          .Build();

            result = config[key];

            }
            catch(Exception ex)
            {

                Logger.LogDebug(ex.Message);
            }
            return result;
        }
    }
}
