using System;
using System.Threading.Tasks;
using awsiotmqttoverwebsocketswinapp.View;
using awsiotmqttoverwebsocketswinapp.Model;
using MQTTnet.Client;
using MQTTnet;
using awsiotmqttoverwebsocketswinapp.Utils;
using System.Text;
using System.Collections.Generic;

namespace awsiotmqttoverwebsocketswinapp.Presenter
{
    public class AwsIotPresenter
    {
        private readonly IAwsIotView view;

        public IMqttClient mqttClient;
        private IMqttClientOptions mqttClientOptions;
        private string lastSubscribedTopic;
       
        public AwsIotPresenter(IAwsIotView view)
        {
            this.view = view;
        }

        public async Task ConnectToAwsIoT()
        {
            try
            {
                AwsMqttConnection awsMqttConnection = new AwsMqttConnection
                {
                    Host = view.HostText,
                    Region = view.RegionText,
                    AccessKey = view.AccessKeyText,
                    SecretKey = view.SecretKeyText
                };

                string signedRequestUrl = awsMqttConnection.SignRequestUrl();

                var factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient();
                mqttClient.Connected += MqttClient_Connected;
                mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;

                mqttClientOptions = new MqttClientOptionsBuilder()
                        .WithWebSocketServer(signedRequestUrl)
                        .Build();

                await mqttClient.ConnectAsync(mqttClientOptions);
                Logger.LogInfo("Connected successfully .....");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        private void MqttClient_Connected(object sender, MqttClientConnectedEventArgs e)
        {
            view.ConnectStatusLabel = "Connected";
        }

        public async Task PublishMessage(string message, string topic)
        {
            try
            {
                await mqttClient.PublishAsync(topic, message, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce, false);
                Logger.LogInfo($"Published message: {message}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        public async Task SubscribeTo(string topic)
        {
            try
            {
                if (lastSubscribedTopic != topic)
                {
                    if (lastSubscribedTopic != null)
                        await mqttClient.UnsubscribeAsync(lastSubscribedTopic);

                    await mqttClient.SubscribeAsync(topic);
                    Logger.LogInfo($"Subscribed to: {topic}");
                    lastSubscribedTopic = topic;

                    view.SubscribeStatusLabel = $"Subscribed to {topic}";
                }
                else
                {
                    Logger.LogInfo($"Already subscribed to: {topic}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload, 0, e.ApplicationMessage.Payload.Length);

            stringBuilder.AppendLine("### RECEIVED APPLICATION MESSAGE ###");
            stringBuilder.AppendLine("Topic: " + e.ApplicationMessage.Topic);
            stringBuilder.AppendLine("Payload: " + payload);
            stringBuilder.AppendLine("QOS: " + e.ApplicationMessage.QualityOfServiceLevel);
            stringBuilder.AppendLine("QOS Retain: " + e.ApplicationMessage.Retain);
            stringBuilder.AppendLine();
            
            view.ReceivedMessageText = stringBuilder.ToString();
        }
    }
}
