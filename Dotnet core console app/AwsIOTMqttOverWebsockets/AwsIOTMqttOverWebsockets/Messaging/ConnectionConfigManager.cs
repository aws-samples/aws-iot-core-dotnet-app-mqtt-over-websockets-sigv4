using System;
using AwsIOTMqttOverWebsockets.Model;
namespace AwsIOTMqttOverWebsockets.Messaging
{
    public static class ConnectionConfigManager
    {
        private static CloudConnectionConfig cloudConnectionConfig;

        public static CloudConnectionConfig  GetConnectionConfig()
        {

            if (cloudConnectionConfig==null)
            {

                cloudConnectionConfig = new CloudConnectionConfig();
            }

            return cloudConnectionConfig;


        }

    }
}
