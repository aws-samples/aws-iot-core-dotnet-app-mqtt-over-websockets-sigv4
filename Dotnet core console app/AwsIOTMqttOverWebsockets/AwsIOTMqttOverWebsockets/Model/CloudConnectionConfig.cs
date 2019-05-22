using System;
using AwsIOTMqttOverWebsockets.Utils;
namespace AwsIOTMqttOverWebsockets.Model
{
    public class CloudConnectionConfig
    {
        public CloudConnectionConfig()
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

       public string TopicToPublish
        {
            get
            {

                return ConfigHelper.ReadSetting("topictopublish");
            }


        }


      public  string TopicToSubscribe
        {
            get
            {

                return ConfigHelper.ReadSetting("topictosubscribe");
            }


        }
        public string MessageToPublish
        {

            get
            {

                return "Message from .NET core console application";
            }
        }

        public string MessageFromSubscribption

        {

            get
            {
                return this.MessageFromSubscribption;
            }

            set
            {
                this.MessageFromSubscribption = value;
            }
        }
    }
}
