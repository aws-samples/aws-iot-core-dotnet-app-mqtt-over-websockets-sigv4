using System;
using AwsIOTMqttOverWebsockets.Model;
using AwsIOTMqttOverWebsockets.Messaging;
using System.Threading;

namespace AwsIOTMqttOverWebsockets
{
    class Program 
    {
       



        static void Main(string[] args)
        {
           

            CloudConnectionConfig cloudConnectionConfig = ConnectionConfigManager.GetConnectionConfig();

            CloudConnector cloudConnector = new CloudConnector(cloudConnectionConfig);



            Console.WriteLine("Enter 1 for Publish. 2 for subscribe.");


            string input = Console.ReadLine();
           
            if (input=="1")
            {

                cloudConnector.PublishMessage();
            }

            else if (input=="2")
            {
                cloudConnector.SubscribeMessage();
            }



            Console.ReadLine();


        }

      


    }
}
