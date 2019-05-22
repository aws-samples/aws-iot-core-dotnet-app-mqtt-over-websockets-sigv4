using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Client;
using MQTTnet;
using AwsIOTMqttOverWebsockets.Utils;
using AwsIOTMqttOverWebsockets.Model;
using System.Threading;

namespace AwsIOTMqttOverWebsockets.Messaging
{
    public class CloudConnector
    {
        private CloudConnectionConfig cloudConnectionConfig;
        public IMqttClient mqttClient;
        private IMqttClientOptions mqttClientOptions;
        private bool IsConnected;


        public CloudConnector(CloudConnectionConfig cloudConnectionConfig1)
        {
            cloudConnectionConfig = cloudConnectionConfig1;
        }

        public void ConnectToAwsIOT()
        {
            try
            {


                AwsMqttConnection awsMqttConnection = new AwsMqttConnection();
                awsMqttConnection.Host = cloudConnectionConfig.Host;
                awsMqttConnection.Region = cloudConnectionConfig.Region;
                awsMqttConnection.AccessKey = cloudConnectionConfig.AccessKey;
                awsMqttConnection.SecretKey = cloudConnectionConfig.SecretKey;

                awsMqttConnection.ClientId = new Guid();

                string requestUrl = awsMqttConnection.GetRequesturl();

                var factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient();


                mqttClientOptions = new MqttClientOptionsBuilder()
                        .WithWebSocketServer(requestUrl)
                        .Build();



                mqttClient.ConnectAsync(mqttClientOptions).Wait();
                IsConnected = true;
                Logger.LogInfo("Connected successfully .....");

            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }
        }

     

        public  async void PublishMessage()
        {
            try
            {

                if (mqttClient==null)
                {
                    ConnectToAwsIOT();

                    
                }

                if (IsConnected)

                { 
                await mqttClient.PublishAsync(cloudConnectionConfig.TopicToPublish, cloudConnectionConfig.MessageToPublish, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce, false);


                    Logger.LogInfo("Message published successfully");
                }

                else
                {

                    Logger.LogInfo("Waiting for connection to complete");
                }
            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }
        }

        public  async void SubscribeMessage()
        {

            try
            {

                if (mqttClient == null)
                {
                    ConnectToAwsIOT();

                }

              

                if (IsConnected)

                {
                    string topic = cloudConnectionConfig.TopicToSubscribe;
                    mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;

                    await mqttClient.SubscribeAsync(cloudConnectionConfig.TopicToSubscribe);
                Logger.LogInfo("subscribed");

                }

                else
                {
                    Logger.LogInfo("Waiting for connection to complete");
                }
            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }
        }

        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            AwsMqttConnection awsMqttConnection = new AwsMqttConnection();
            string message=awsMqttConnection.ProcessReceivedMessages(e);
            Logger.LogInfo(message);
        }

    }
}
