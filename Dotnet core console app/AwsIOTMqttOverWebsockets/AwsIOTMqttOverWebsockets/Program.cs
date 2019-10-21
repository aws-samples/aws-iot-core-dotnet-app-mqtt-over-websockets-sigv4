using System;
using AwsIOTMqttOverWebsockets.Model;
using AwsIOTMqttOverWebsockets.Messaging;
using System.Threading.Tasks;

namespace AwsIOTMqttOverWebsockets
{
    class Program 
    {
        static async Task Main(string[] args)
        {
            CloudConnectionConfig cloudConnectionConfig = CloudConnectionConfig.Instance;
            CloudConnector cloudConnector = new CloudConnector(cloudConnectionConfig);

            string topic = "mytest/topic";
            string message = "Test message";
            int i = 0;

            await cloudConnector.ConnectToAwsIoT();
            while (true) 
            {
                Console.WriteLine("Enter 1 for publish. 2 for subscribe.");
                string input = Console.ReadLine();
            
                if (input == "1")
                {
                    await cloudConnector.PublishMessage($"{message} {i}", topic);
                }
                else if (input == "2")
                {
                    await cloudConnector.SubscribeTo(topic);
                }
                
                i++;
            }
        }
    }
}