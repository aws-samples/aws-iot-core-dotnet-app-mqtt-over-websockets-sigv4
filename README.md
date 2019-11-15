
# 1. Overview

There are multiple options available for publishing and subscribing messages with AWS IoT Core. The message broker supports the use of the MQTT protocol to publish and subscribe and the HTTPS protocol to publish. Both protocols are supported through IP version 4 and IP version 6. The message broker also supports MQTT over the WebSocket protocol.

Here is a simple table that shows various protocol and port options available for handshake with AWS IoT Core.

|No |Protocol        |Authentication     |Port    |
|---|----------------|-------------------|------- |
| 1 |MQTT            |ClientCertificate  |8883,443|
| 2 |HTTP            |ClientCertificate  |8443    |
| 3 |HTTP            |Sigv4              |443     |
| 4 |MQTT+WebSocket  |Sigv4              |443     |

More details are available here https://docs.aws.amazon.com/iot/latest/developerguide/protocols.html

In this post, we'll cover the option #4 of leveraging MQTT over Web sockets for communicating with AWS IoT Core by implementing the necessary Sigv4 authentication. 

# 2. AWS IoT .NET Framework app using MQTT Over Websockets and Sigv4
The following sub-sections 2a,2b, and 2c offer guidance on implementing a .NET Framework app that communicates with AWS IoT Core using MQTT protocol over Websockets channel. It also implements AWS Sigv4 authentication.

## 2a. Development environment
- Windows 10 with latest updates
- Visual Studio 2017 with latest updates
- Windows Subsystem for Linux 

## 2b. Visual Studio Solution & Project Walkthrough

Start by opening the solution file located at 'Dotnet win app\awsiotmqttoverwebsocketswinapp\awsiotmqttoverwebsocketswinapp.sln'.

This .NET Framework solution is a WinForms app that allows you to configure account and endpoint specific settings (access keys, secret keys, custom endpoint URL) and publish and subscribe test messages to topics in AWS IoT Core.  The underlying implementation is communicating over the websockets protocol and signing the request with the Sigv4 protocol for authentication.

The solution is implemented using the MVP (Model, View, Presenter) pattern.  The Form.cs class controls the interaction between the view and the presentation layer.  The presentation layer (found in the Presenter folder) passes along the requests to the MQTT library.  This sample uses the MQTTnet library, but any .NET MQTT library will work.  The presentation layer then handles the actual publishing and subscribing to AWS IoT Core and the resulting messages are reflected in the WinForms UI.


## 2c. Compile, Run and Verify messages sent to AWS IoT Core
Compile and run the above code.  Fill out the various fields in the UI and you should see the successful publishing of messages of AWS IoT Core and also successful subscription of messages from AWS IoT Core. 

![](/images/appoutput.png)


# 3. AWS IoT .NET Core app using MQTT Over Websockets and Sigv4
The following sub-sections 3a,3b, and 3c offer guidance on implementing a .NET Core app that communicates with AWS IoT Core using MQTT protocol over Websockets channel. It also implements AWS Sigv4 authentication.

## 3a. Development environment
- Mac OS with latest updates 
- .NET Core 2.1 or higher
- Visual Studio Code 2017 for Mac

## 3b. Visual Studio Solution Walkthrough

Open the solution file located at 'Dotnet core console app\AwsIOTMqttOverWebsockets\AwsIOTMqttOverWebsockets.sln' and start by navigating to the appsettings.json file.  Modify the "host" setting to point to your IoT Core custom endpoint and then substitute your access and secret keys for the other settings in this file.  These settings should not be checked in to source control.

Navigate to the Program.cs file and observe that this sample has two possible actions.  When running the program, entering a "1" causes the program to publish a sample message to the given topic.  Entering a "2" causes the program to subscribe to that topic and print incoming message to the console.

The 'Model\AwsMqttConnection.cs' class demonstrates an implementation of Sigv4 signing for a request made over the websockets (wss) protocol.  This request signature is created and added to the request as the query string.  This signed URI is then used to connect to the AWS IoT Core service to publish and subscribe to messages.

## 3c. Compile, run and verify

Compile and run the code in Visual Studio for Mac.

Select "1" for publishing messages. You should see the status for successful publishing of messages in the console app.

![](/images/publish1.png)

You can also verify that the messages were successfully published by subscribing to the topic in the AWS IoT Test Console.

![](/images/publish2.png)

Run the solution again and select "2" for subscribing messages. You should see confirmation for the successful creation of a subscription for a specific topic.
Publish the messages on the specified topic from AWS IoT Test Console.

![](/images/subscribe1.png)

You should see that the message is successfuly received by the console app.

![](/images/subscribe2.png)

## 4. Conclusion
We have created a .NET Framework WinForms app that publishes messages messages to AWS IoT Core using MQTT protocol over Websockets channel with AWS Sigv4 authentication. The app also implemented logic for subscribing to messages from AWS IoT Core using the same architecture. We also have created another reference implementation using .NET Core to publish messages to to AWS IoT Core using MQTT protocol over Websockets channel with AWS Sigv4 authentication. It also implemented logic for subscription of messages from AWS IoT Core using the same architecture. 

## License

This library is licensed under the Apache 2.0 License. 
