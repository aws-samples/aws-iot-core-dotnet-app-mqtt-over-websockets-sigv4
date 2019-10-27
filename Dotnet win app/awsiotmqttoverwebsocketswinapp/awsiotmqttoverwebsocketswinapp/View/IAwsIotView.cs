namespace awsiotmqttoverwebsocketswinapp.View
{
    public interface IAwsIotView
    {
        string HostText { get; set; }

        string RegionText { get; set; }

        string AccessKeyText { get; set; }

        string SecretKeyText { get; set; }

        string TopicToPublishText { get; set; }

        string TopicToSubscribeText { get; set; }

        string PublishStatusLabel { get; set; }

        string ConnectStatusLabel { get; set; }

        string PublishMessageText { get; set; }

        string ReceivedMessageText { get; set; }

        string SubscribeStatusLabel { get; set; }
    }
}
