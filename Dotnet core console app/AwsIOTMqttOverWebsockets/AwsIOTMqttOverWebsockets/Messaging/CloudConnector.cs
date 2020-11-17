using System;
using MQTTnet.Client;
using MQTTnet;
using AwsIOTMqttOverWebsockets.Utils;
using AwsIOTMqttOverWebsockets.Model;
using System.Threading.Tasks;
using System.Text;
using MQTTnet.Protocol;

namespace AwsIOTMqttOverWebsockets.Messaging
{
    public class CloudConnector
    {
        private CloudConnectionConfig cloudConnectionConfig;
        private IMqttClient mqttClient;
        private IMqttClientOptions mqttClientOptions;
        private bool isSubscribed;

        public CloudConnector(CloudConnectionConfig cloudConnectionConfig)
        {
            this.cloudConnectionConfig = cloudConnectionConfig;
            isSubscribed = false;
        }

        public async Task ConnectToAwsIoT()
        {
            try
            {
                AwsMqttConnection awsMqttConnection = new AwsMqttConnection
                {
                    Host = cloudConnectionConfig.Host,
                    Region = cloudConnectionConfig.Region,
                    AccessKey = cloudConnectionConfig.AccessKey,
                    SecretKey = cloudConnectionConfig.SecretKey,
                    SessionToken = cloudConnectionConfig.SessionToken
                };

                string signedRequestUrl = awsMqttConnection.SignRequestUrl();

                var factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient();

                mqttClientOptions = new MqttClientOptionsBuilder()
                        .WithWebSocketServer(signedRequestUrl)
                        .Build();

                await mqttClient.ConnectAsync(mqttClientOptions);
                Logger.LogInfo("Connected successfully .....");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task PublishMessage(string message, string topic)
        {
            try
            {
                await mqttClient.PublishAsync(topic, message, MqttQualityOfServiceLevel.AtLeastOnce, false);
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
                if (!isSubscribed)
                {
                    mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;

                    await mqttClient.SubscribeAsync(topic);
                    Logger.LogInfo($"Subscribed to: {topic}");
                    isSubscribed = true;
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
            string payload = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload, 0, e.ApplicationMessage.Payload.Length);

            stringBuilder.AppendLine("### RECEIVED APPLICATION MESSAGE ###");
            stringBuilder.AppendLine("Topic: " + e.ApplicationMessage.Topic);
            stringBuilder.AppendLine("Payload: " + payload);
            stringBuilder.AppendLine("QOS: " + e.ApplicationMessage.QualityOfServiceLevel);
            stringBuilder.AppendLine("QOS Retain: " + e.ApplicationMessage.Retain);
            stringBuilder.AppendLine();

            Logger.LogInfo(stringBuilder.ToString());
        }
    }
}