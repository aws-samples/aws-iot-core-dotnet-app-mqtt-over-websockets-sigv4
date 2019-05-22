using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using awsiotmqttoverwebsocketswinapp.View;
using awsiotmqttoverwebsocketswinapp.Model;
using MQTTnet.Client;
using MQTTnet;
using awsiotmqttoverwebsocketswinapp.Utils;

namespace awsiotmqttoverwebsocketswinapp.Presenter
{
    public class AwsIotPresenter
    {
        IAwsIotView view1;
        public IMqttClient mqttClient;
        private IMqttClientOptions mqttClientOptions;
       

        public AwsIotPresenter(IAwsIotView view)
        {

            view1 = view;

        }

        public async void MakeConnection()
        {
            try
            {

            
            AwsMqttConnection awsMqttConnection = new AwsMqttConnection();
            awsMqttConnection.Host = view1.HostText;
            awsMqttConnection.Region = view1.RegionText;
            awsMqttConnection.AccessKey = view1.AccessKeyText;
            awsMqttConnection.SecretKey = view1.SecretKeyText;
            awsMqttConnection.ClientId = new Guid();

            string requestUrl = awsMqttConnection.GetRequesturl();

            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();


            mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithWebSocketServer(requestUrl)
                    .Build();

            mqttClient.Connected += MqttClient_Connected;

            await mqttClient.ConnectAsync(mqttClientOptions);

            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }
        }

        private void MqttClient_Connected(object sender, MqttClientConnectedEventArgs e)
        {


            view1.ConnectStatusLabel = "Connected";



        }

        public async void PublishMessage()
        {
            try
            { 
            await mqttClient.PublishAsync(view1.TopicToPublishText, view1.PublishMessageText, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce, false);
            }
            catch(Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }
        }

        public async void SubscribeMessage()
        {
            
            try
            {

            
            mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;

            await mqttClient.SubscribeAsync(view1.TopicToSubscribeText);
            }
            catch(Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }
        }

        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            AwsMqttConnection awsMqttConnection = new AwsMqttConnection();
            view1.SubscribeMessageText = awsMqttConnection.ProcessReceivedMessages(e);
        }
    }
}
