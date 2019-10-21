using System;
using AwsIOTMqttOverWebsockets.Utils;

namespace AwsIOTMqttOverWebsockets.Model
{
    public class CloudConnectionConfig
    {
        private static readonly Lazy<CloudConnectionConfig> lazy = new Lazy<CloudConnectionConfig>(() => new CloudConnectionConfig());

        public static CloudConnectionConfig Instance { get { return lazy.Value; } }

        private CloudConnectionConfig()
        {
        }

        public string Host
        {
            get
            {
                return ConfigHelper.ReadSetting("host");
            }
        }

        public string Region
        {
            get
            {
                return ConfigHelper.ReadSetting("region");
            }
        }

        public string AccessKey
        {
            get
            {

                return ConfigHelper.ReadSetting("accesskey");
            }
        }

        public string SecretKey
        {
            get
            {
                return ConfigHelper.ReadSetting("secretkey");
            }
        }
    }
}
