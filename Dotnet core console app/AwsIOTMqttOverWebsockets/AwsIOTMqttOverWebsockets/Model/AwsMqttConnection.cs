using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwsIOTMqttOverWebsockets.Utils;
using MQTTnet.Client;
using MQTTnet;
namespace AwsIOTMqttOverWebsockets.Model
{
    public class AwsMqttConnection
    {
        public string Host { get; set; }

        public string Region { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public string SubscribeTopic { get; set; }

        public string PublishTopic { get; set; }


        public Guid ClientId { get; set; }

        public string RequestUrl { get; set; }

        public string GetRequesturl()
        {
            string requestUrl = Sigv4util.getSignedurl(Host, Region, AccessKey, SecretKey);
            return requestUrl;

        }

        public string ProcessReceivedMessages(MqttApplicationMessageReceivedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string payload = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload, 0, e.ApplicationMessage.Payload.Length);

            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("### RECEIVED APPLICATION MESSAGE ###");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Topic " + e.ApplicationMessage.Topic);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Pay load" + payload);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("QOS " + e.ApplicationMessage.QualityOfServiceLevel);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("QOS " + "Retain " + e.ApplicationMessage.Retain);

            return stringBuilder.ToString();
        }
    }
}
