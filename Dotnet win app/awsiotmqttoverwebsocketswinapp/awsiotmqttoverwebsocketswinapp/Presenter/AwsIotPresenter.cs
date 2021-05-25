using System;
using System.Threading.Tasks;
using awsiotmqttoverwebsocketswinapp.View;
using awsiotmqttoverwebsocketswinapp.Model;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using MQTTnet.Server;
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
        private IManagedMqttClient managedMqttClientPublisher;
        //private IManagedMqttClient managedMqttClientSubscriber;

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

                var options = new MqttClientOptions
                {
                    CleanSession = true,
                    KeepAlivePeriod = TimeSpan.FromSeconds(5)
                };

                mqttClientOptions = new MqttClientOptionsBuilder()
                        .WithWebSocketServer(signedRequestUrl)
                        .WithCleanSession(true)
                        .WithKeepAlivePeriod(TimeSpan.FromSeconds(5))
                        .Build();

                this.managedMqttClientPublisher = factory.CreateManagedMqttClient();
                this.managedMqttClientPublisher.UseApplicationMessageReceivedHandler(this.HandleReceivedApplicationMessage);
                this.managedMqttClientPublisher.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnPublisherConnected);
                this.managedMqttClientPublisher.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnPublisherDisconnected);

                await this.managedMqttClientPublisher.StartAsync(
                new ManagedMqttClientOptions
                {
                    ClientOptions = mqttClientOptions

                });


            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        private void OnPublisherConnected(MqttClientConnectedEventArgs e)
        {
            view.ConnectStatusLabel = "Connected";
        }

        private void OnPublisherDisconnected(MqttClientDisconnectedEventArgs x)
        {
            Logger.LogInfo("Publisher Disconnected");
            view.ConnectStatusLabel = "Connection Lost";
        }

        public async Task PublishMessage(string message, string topic)
        {
            try
            {
                if (this.managedMqttClientPublisher != null)
                {
                    await this.managedMqttClientPublisher.PublishAsync(topic, message, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce, false);
                    Logger.LogInfo($"Published message: {message}");
                }
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
                        await this.managedMqttClientPublisher.UnsubscribeAsync(lastSubscribedTopic);

                    await this.managedMqttClientPublisher.SubscribeAsync(topic);
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

        private void HandleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs e)
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