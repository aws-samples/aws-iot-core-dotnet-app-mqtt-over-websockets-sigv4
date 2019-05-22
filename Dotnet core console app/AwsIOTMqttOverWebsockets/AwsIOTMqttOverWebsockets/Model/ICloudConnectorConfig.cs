using System;
namespace AwsIOTMqttOverWebsockets.Model
{
    public interface ICloudConnectorConfig
    {
        string Host {get;}
        string Region {get;}
        string AccessKey {get;}
        string SecretKey {get;}
        string  TopicToPublish {get;}
        string  TopicToSubscribe{get;}

    }
}
