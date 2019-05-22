
# 1. Overview

There are multiple options available for publishing and subscribing messages with AWS IOT Core. The message broker supports the use of the MQTT protocol to publish and subscribe and the HTTPS protocol to publish. Both protocols are supported through IP version 4 and IP version 6. The message broker also supports MQTT over the WebSocket protocol.

Here is a simple table that shows various protocol and port options available for handshake with AWS IOT Core.

|No |Protocol        |Authentication     |Port    |
|---|----------------|-------------------|------- |
| 1 |MQTT            |ClientCertificate  |8883,443|
| 2 |HTTP            |ClientCertificate  |8443    |
| 3 |HTTP            |SigV4              |443     |
| 4 |MQTT+WebSocket  |SigV4              |443     |

More details are available here https://docs.aws.amazon.com/iot/latest/developerguide/protocols.html

In this post, we'll cover the option #4 of leveraging MQTT over Web sockets for communicating with AWS IOT Core by implementing the necessary SigV4 authentication. 

# 2. AWS IOT .NET app using MQTT Over Websockets and Sigv4
The following sub-sections 2a,2b,2c,2d and 2e offer guidance on implementing a .NET app that communicates with AWS IOT Core using MQTT protocol over Websockets channel. It also implements AWS Sigv4 authentication.

## 2a. Development environment
- Windows 10 with latest updates
- Visual Studio 2017 with latest updates
- Windows Subsystem for Linux 

## 2b. Visual Studio Solution & Project

Create a windows forms application in visual studio 2017 with name 'awsiotmqttoverwebsocketswinapp'

Add the following nuget package references in the solution.
1.MQTTnet
2.log4net

Let's implement the Model-View-Presenter(MVP) pattern in this win form app for achieving required functionality.

Add a folder named 'Model' and create a class 'AwsMqttConnection.cs' inside that. The implementation of 'AwsMqttConnection.cs' should look like the following.

``` c#

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using awsiotmqttoverwebsocketswinapp.Utils;
using MQTTnet.Client;
using MQTTnet;

namespace awsiotmqttoverwebsocketswinapp.Model
{
    public class AwsMqttConnection     {
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

        public string ProcessReceivedMessages(MqttApplicationMessageReceivedEventArgs e )
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

```

We have completed the creation of Model. Let's move to create View. Create a folder named 'View' and define an interface called 'IAwsIotView.cs' inside it. The definition of the interface 'IAwsIotView.cs' should look like the following.

``` c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awsiotmqttoverwebsocketswinapp.View
{
 public   interface IAwsIotView
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

        string SubscribeMessageText { get; set; }

    }
}


```

We have completed the defintion of View. Let's move to implementation of Presenter. Create a folder called 'Presenter' and implement 'AwsIotPresenter.cs' like the following.

``` c#
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

```

We have completed the implementation of Model, View and Presenter. Let's move to creation of utility classes. Create a folder named 'Utils'.

Implement the following 'ConfigHelper.cs' under Utils folder. This is responsible for reading keys from the app.config file and returning the appropriate values.

``` c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using awsiotmqttoverwebsocketswinapp.Utils;

namespace awsiotmqttoverwebsocketswinapp.Utils
{
    public static class ConfigHelper
    {
        public static string ReadSetting(string key)
        {
            string result="NotFound";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "NotFound";
                
            }
            catch (ConfigurationErrorsException ex)
            {
                Logger.LogError(ex.Message);
                
            }

            return result;
        }
    }
}

```

Implement the following 'HttpHelper.cs' under Utils folder.

``` c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awsiotmqttoverwebsocketswinapp.Utils
{
    public static class HttpHelper
    {
        // The Set of accepted and valid Url characters per RFC3986. Characters outside of this set will be encoded.
        const string ValidUrlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        public static string UrlEncode(string data, bool isPath = false)
        {

            var encoded = new StringBuilder(data.Length * 2);

            try
            { 
                string unreservedChars = String.Concat(ValidUrlCharacters, (isPath ? "/:" : ""));

                foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(data))
                {
                    if (unreservedChars.IndexOf(symbol) != -1)
                        encoded.Append(symbol);
                    else
                        encoded.Append("%").Append(String.Format("{0:X2}", (int)symbol));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

            }
            return encoded.ToString();
        }
    }
}

```

Implement the following 'Logger.cs' under Utils folder. This is a wrapper for log4net.

``` c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awsiotmqttoverwebsocketswinapp.Utils
{
   public static class Logger
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogInfo(string message)
        {
            log.Info(message);
        }

        public static void LogDebug(string message) {
            log.Debug(message);
        }


        public static void LogError(string message) {
            log.Error(message);
        }


        public static void LogFatal(string message) {
            log.Fatal(message);
        }


        public static void LogWarn(string message) {
            log.Warn(message);
        }


    }
}


```

Implement the following 'Sigvutil.cs' under Utils folder. This performs the necessary heavy lifting for framing AWS Sigv4 authentication requests headers and urls.

``` c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace awsiotmqttoverwebsocketswinapp.Utils
{
    public static class Sigv4util
    {
        public const string ISO8601BasicFormat = "yyyyMMddTHHmmssZ";
        public const string DateStringFormat = "yyyyMMdd";
        public const string EmptyBodySha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
        public static HashAlgorithm CanonicalRequestHashAlgorithm = HashAlgorithm.Create("SHA-256");
        // the name of the keyed hash algorithm used in signing
        public const string HmacSha256 = "HMACSHA256";
        public const string XAmzSignature = "X-Amz-Signature";




      private  static byte[] HmacSHA256(String data, byte[] key)
        {
            String algorithm = "HmacSHA256";
            KeyedHashAlgorithm keyHashAlgorithm = KeyedHashAlgorithm.Create(algorithm);
            keyHashAlgorithm.Key = key;

            return keyHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

       private static byte[] ComputeKeyedHash(string algorithm, byte[] key, byte[] data)
        {
            var kha = KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;
            return kha.ComputeHash(data);
        }

        public static string ToHexString(byte[] data, bool lowerCase)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                for (var i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString(lowerCase ? "x2" : "X2"));
                }

            }
            catch(Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }
            
            return stringBuilder.ToString();
        }

      private  static byte[] getSignatureKey(String key, String dateStamp, String regionName, String serviceName)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + key).ToCharArray());
            byte[] kDate = HmacSHA256(dateStamp, kSecret);
            byte[] kRegion = HmacSHA256(regionName, kDate);
            byte[] kService = HmacSHA256(serviceName, kRegion);
            byte[] kSigning = HmacSHA256("aws4_request", kService);

            return kSigning;
        }

        public static string getSignedurl(string host, string region, string accessKey, string secretKey)
        {
            string requestUrl = "";
            try
            {
                
                DateTime requestDateTime = DateTime.UtcNow;
                string datetime = requestDateTime.ToString(ISO8601BasicFormat, CultureInfo.InvariantCulture);
                var date = requestDateTime.ToString(DateStringFormat, CultureInfo.InvariantCulture);

                string method = ConfigHelper.ReadSetting("method");

                string protocol = ConfigHelper.ReadSetting("protocol");

                string uri = ConfigHelper.ReadSetting("uri");

                string service = ConfigHelper.ReadSetting("service");

                string algorithm = ConfigHelper.ReadSetting("algorithm");

                string credentialScope = date + "/" + region + "/" + service + "/" + "aws4_request";
                string canonicalQuerystring = "X-Amz-Algorithm=" + algorithm;
                canonicalQuerystring += "&X-Amz-Credential=" + HttpHelper.UrlEncode(accessKey + '/' + credentialScope);

                canonicalQuerystring += "&X-Amz-Date=" + datetime;
                canonicalQuerystring += "&X-Amz-Expires=86400";
                canonicalQuerystring += "&X-Amz-SignedHeaders=host";

                string canonicalHeaders = "host:" + host + "\n";

                var canonicalRequest = method + "\n" + uri + "\n" + canonicalQuerystring + "\n" + canonicalHeaders + "\n" + "host" + "\n" + EmptyBodySha256;

                byte[] hashValueCanonicalRequest = CanonicalRequestHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(canonicalRequest));



                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashValueCanonicalRequest.Length; i++)
                {
                    builder.Append(hashValueCanonicalRequest[i].ToString("x2"));
                }

                string byteString = builder.ToString();

                var stringToSign = algorithm + "\n" + datetime + "\n" + credentialScope + "\n" + byteString;
                // compute the signing key
                KeyedHashAlgorithm keyedHashAlgorithm = KeyedHashAlgorithm.Create(HmacSha256);

                keyedHashAlgorithm.Key = getSignatureKey(secretKey, date, region, service);

                var signingKey = keyedHashAlgorithm.Key;

                var signature = ComputeKeyedHash(HmacSha256, signingKey, Encoding.UTF8.GetBytes(stringToSign));
                var signatureString = ToHexString(signature, true);

                canonicalQuerystring += "&X-Amz-Signature=" + signatureString;

                requestUrl = protocol + "://" + host + uri + "?" + canonicalQuerystring;


            }

            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

            }
            

            return requestUrl;
        }
    }
}


```


## 2c. App.Config file changes

The app.config file should have the following configuration settings to support app specific configurations and also required configurations to make lognet logging to work.

``` XML
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1" />
  </startup>
  <appSettings>
    <add key="method" value="GET" />
    <add key="protocol" value="wss" />
    <add key="uri" value="/mqtt" />
    <add key="service" value="iotdevicegateway" />
    <add key="algorithm" value="AWS4-HMAC-SHA256" />
    <add key="ClientSettingsProvider.ServiceUri" value="" />
  </appSettings>
  <system.web>
    <membership defaultProvider="ClientAuthenticationMembershipProvider">
      <providers>
        <add name="ClientAuthenticationMembershipProvider" type="System.Web.ClientServices.Providers.ClientFormsAuthenticationMembershipProvider, System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" serviceUri="" />
      </providers>
    </membership>
    <roleManager defaultProvider="ClientRoleProvider" enabled="true">
      <providers>
        <add name="ClientRoleProvider" type="System.Web.ClientServices.Providers.ClientRoleProvider, System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" serviceUri="" cacheTimeout="86400" />
      </providers>
    </roleManager>
  </system.web>
  
  <log4net>
    <root>
      <level value="ALL" />
      <appender-ref ref="MyAppender" />
      <appender-ref ref="RollingFileAppender" />
    </root>
    <appender name="MyAppender" type="log4net.Appender.ConsoleAppender">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date %level %logger - %message%newline" />
      </layout>
    </appender>
    <appender name="MyFileAppender" type="log4net.Appender.FileAppender">
      <file value="application.log" />
      <appendToFile value="true" />
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date %level %logger - %message%newline" />
      </layout>
    </appender>
    <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
      <file value="C:\\SundarTest2\\rolling.log" />
      <appendToFile value="true" />
      <rollingStyle value="Size" />
      <maxSizeRollBackups value="5" />
      <maximumFileSize value="10MB" />
      <staticLogFileName value="true" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %level %logger - %message%newline" />
      </layout>
    </appender>
  </log4net>
</configuration>

```

## 2d. AssemblyInfo.cs file changes for log4net configuration

Add the following configuration entry in AssemblyInfo.cs. This is to make the log4net to refer app.config for all the logging related configurations.

``` c#

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
```
## 2e. Completed and working code

The completed and working code for this reference is available in the folder folder 'Dotnet win app' of this repository. You can also download it from there.

## 2f. Compile, Run and Verify messages sent to AWS IOT Core
Compile and run the above code. You should see the successful publishment of messages of AWS Iot core and also successful subscription of messages from AWS Iot core. 

![](/images/appoutput.jpg)

This completes the reference implementation in .NET framework.

# 3. AWS IOT .NET core app using MQTT Over Websockets and Sigv4
The following sub-sections 3a,3b,3c,3d and 3e offer guidance on implementing a .NET core app that communicates with AWS IOT Core using MQTT protocol over Websockets channel. It also implements AWS Sigv4 authentication.

## 3a. Development environment
- Mac OS with latest updates 
- .NET Core 2.1 or higher
- Visual Studio Code 2017 for Mac

## 3b. Create Console application project in Dotnetcore

Create a .NET core console application project in Visual Studio for Mac and name it as 'AwsIOTMqttOverWebsockets'.

Add the following five Nuget package references to the solution.
- log4net
- MQTTnet
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.FileExtensions
- Microsoft.Extensions.Configuration.Json

Create a folder named 'Utils' and add a class called  ConfigHelper.cs with the following implementation. This takes care of reading from appsettings.json file.

``` c#
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;


namespace AwsIOTMqttOverWebsockets.Utils
{
    public static class ConfigHelper
    {
        public static string ReadSetting(string key)
        {
            string result = "NotFound";

            try
            {

            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", true, true)
          .Build();

            result = config[key];

            }
            catch(Exception ex)
            {

                Logger.LogDebug(ex.Message);
            }
            return result;
        }
    }
}

``` 


Create a class called 'HttpHelper.cs' in the 'Utils' folder with the following implementation. 

``` c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsIOTMqttOverWebsockets.Utils
{
    public static class HttpHelper
    {
        // The Set of accepted and valid Url characters per RFC3986. Characters outside of this set will be encoded.
        const string ValidUrlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        public static string UrlEncode(string data, bool isPath = false)
        {

            var encoded = new StringBuilder(data.Length * 2);

            try
            {
                string unreservedChars = String.Concat(ValidUrlCharacters, (isPath ? "/:" : ""));

                foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(data))
                {
                    if (unreservedChars.IndexOf(symbol) != -1)
                        encoded.Append(symbol);
                    else
                        encoded.Append("%").Append(String.Format("{0:X2}", (int)symbol));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

            }
            return encoded.ToString();
        }
    }
}

``` 

Now, we have added HttpHelper.cs and ConfigHelper.cs in the utils folder. Add a class named 'Logger.cs' with the following implementation. This is a wrapper class for working with Log4net.

``` c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using log4net;
using log4net.Config;
namespace AwsIOTMqttOverWebsockets.Utils
{
    public static class Logger
    {
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static bool IsLog4netConfigured;

        public static void LogInfo(string message)
        {
            if (! IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }
            log.Info(message);
        }

        public static void LogDebug(string message)
        {
            if (!IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }

            log.Debug(message);
        }


        public static void LogError(string message)
        {
            if (!IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }
            log.Error(message);
        }


        public static void LogFatal(string message)
        {
            if (!IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }

            log.Fatal(message);
        }


        public static void LogWarn(string message)
        {
            if (!IsLog4netConfigured)
            {
                ConfigureLog4Net();
            }
            log.Warn(message);
        }

        private static void ConfigureLog4Net()
        { 
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            IsLog4netConfigured = true;
        }

    }
}

```

Then, create a class 'Sigvutil.cs' in the 'Utils' folder with the following implementation. This performs the required heavy lifting for making Aws Sigv4 requests.

``` c#

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AwsIOTMqttOverWebsockets.Utils
{
    public static class Sigv4util
    {
        public const string ISO8601BasicFormat = "yyyyMMddTHHmmssZ";
        public const string DateStringFormat = "yyyyMMdd";
        public const string EmptyBodySha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
        public static HashAlgorithm CanonicalRequestHashAlgorithm = HashAlgorithm.Create("SHA-256");
        // the name of the keyed hash algorithm used in signing
        public const string HmacSha256 = "HMACSHA256";
        public const string XAmzSignature = "X-Amz-Signature";




        private static byte[] HmacSHA256(String data, byte[] key)
        {
            String algorithm = "HmacSHA256";
            KeyedHashAlgorithm keyHashAlgorithm = KeyedHashAlgorithm.Create(algorithm);
            keyHashAlgorithm.Key = key;

            return keyHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private static byte[] ComputeKeyedHash(string algorithm, byte[] key, byte[] data)
        {
            var kha = KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;
            return kha.ComputeHash(data);
        }

        public static string ToHexString(byte[] data, bool lowerCase)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                for (var i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString(lowerCase ? "x2" : "X2"));
                }

            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }

            return stringBuilder.ToString();
        }

        private static byte[] getSignatureKey(String key, String dateStamp, String regionName, String serviceName)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + key).ToCharArray());
            byte[] kDate = HmacSHA256(dateStamp, kSecret);
            byte[] kRegion = HmacSHA256(regionName, kDate);
            byte[] kService = HmacSHA256(serviceName, kRegion);
            byte[] kSigning = HmacSHA256("aws4_request", kService);

            return kSigning;
        }

        public static string getSignedurl(string host, string region, string accessKey, string secretKey)
        {
            string requestUrl = "";
            try
            {

                DateTime requestDateTime = DateTime.UtcNow;
                string datetime = requestDateTime.ToString(ISO8601BasicFormat, CultureInfo.InvariantCulture);
                var date = requestDateTime.ToString(DateStringFormat, CultureInfo.InvariantCulture);

                string method = ConfigHelper.ReadSetting("method");

                string protocol = ConfigHelper.ReadSetting("protocol");

                string uri = ConfigHelper.ReadSetting("uri");

                string service = ConfigHelper.ReadSetting("service");

                string algorithm = ConfigHelper.ReadSetting("algorithm");

                string credentialScope = date + "/" + region + "/" + service + "/" + "aws4_request";
                string canonicalQuerystring = "X-Amz-Algorithm=" + algorithm;
                canonicalQuerystring += "&X-Amz-Credential=" + HttpHelper.UrlEncode(accessKey + '/' + credentialScope);

                canonicalQuerystring += "&X-Amz-Date=" + datetime;
                canonicalQuerystring += "&X-Amz-Expires=86400";
                canonicalQuerystring += "&X-Amz-SignedHeaders=host";

                string canonicalHeaders = "host:" + host + "\n";

                var canonicalRequest = method + "\n" + uri + "\n" + canonicalQuerystring + "\n" + canonicalHeaders + "\n" + "host" + "\n" + EmptyBodySha256;

                byte[] hashValueCanonicalRequest = CanonicalRequestHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(canonicalRequest));



                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashValueCanonicalRequest.Length; i++)
                {
                    builder.Append(hashValueCanonicalRequest[i].ToString("x2"));
                }

                string byteString = builder.ToString();

                var stringToSign = algorithm + "\n" + datetime + "\n" + credentialScope + "\n" + byteString;
                // compute the signing key
                KeyedHashAlgorithm keyedHashAlgorithm = KeyedHashAlgorithm.Create(HmacSha256);

                keyedHashAlgorithm.Key = getSignatureKey(secretKey, date, region, service);

                var signingKey = keyedHashAlgorithm.Key;

                var signature = ComputeKeyedHash(HmacSha256, signingKey, Encoding.UTF8.GetBytes(stringToSign));
                var signatureString = ToHexString(signature, true);

                canonicalQuerystring += "&X-Amz-Signature=" + signatureString;

                requestUrl = protocol + "://" + host + uri + "?" + canonicalQuerystring;


            }

            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

            }


            return requestUrl;
        }
    }
}

```

Now we have completed creation of required classes in the 'Utils' folder. Let's move to creation of required 'Model' for this solution. Create a folder named 'Model' and add a class 'CloudConnectionConfig.cs' with the following implementation. This defines the domain model for cloud connection with required attributes.

``` c#
using System;
using AwsIOTMqttOverWebsockets.Utils;
namespace AwsIOTMqttOverWebsockets.Model
{
    public class CloudConnectionConfig
    {
        public CloudConnectionConfig()
        {
        }

       public string Host
        {
            get
            {
                return ConfigHelper.ReadSetting("host");


            }

        }
       public string Region
        {
            get
            {
                return ConfigHelper.ReadSetting("region");




            }
        }

       public string AccessKey
        {
            get
            {

                return ConfigHelper.ReadSetting("accesskey");
            }


        }

       public string SecretKey

        {
            get
            {

                return ConfigHelper.ReadSetting("secretkey");
            }


        }

       public string TopicToPublish
        {
            get
            {

                return ConfigHelper.ReadSetting("topictopublish");
            }


        }


      public  string TopicToSubscribe
        {
            get
            {

                return ConfigHelper.ReadSetting("topictosubscribe");
            }


        }
        public string MessageToPublish
        {

            get
            {

                return "Message from .NET core console application";
            }
        }

        public string MessageFromSubscribption

        {

            get
            {
                return this.MessageFromSubscribption;
            }

            set
            {
                this.MessageFromSubscribption = value;
            }
        }
    }
}


```

Also, create another class 'AwsMqttConnection.cs' in 'Model' folder with following implementation.

``` c#
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


```

Now we have created required classes for 'Model'. Let's proceed to create a folder named 'Messaging' and a class 'ConnectionConfigManager.cs' with the followign implementation.

``` c#

using System;
using AwsIOTMqttOverWebsockets.Model;
namespace AwsIOTMqttOverWebsockets.Messaging
{
    public static class ConnectionConfigManager
    {
        private static CloudConnectionConfig cloudConnectionConfig;

        public static CloudConnectionConfig  GetConnectionConfig()
        {

            if (cloudConnectionConfig==null)
            {

                cloudConnectionConfig = new CloudConnectionConfig();
            }

            return cloudConnectionConfig;


        }

    }
}


```

This is reponsible for returnign the required connection details for intiating connection with AWS Iot Core. Then, create a class 'CloudConnector.cs' with the following implementation.

``` c#

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


```

Finally make the necessary changes to 'Program.cs' to start the communication with AWS Iot Core.

``` c#

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



```


## 3c. AppSettings.json file configuration

The .NET core console applications does not rely on 'app.config' for application configuration, unlike .NET framework console applications. It needs 'appsettings.json', instead of 'app.config' file. Add an 'appsettings.json' like below and replace relevant details for your applications.

``` json
{
  "method": "GET",
  "protocol": "wss",
  "uri": "/mqtt",
  "service": "iotdevicegateway",
  "algorithm": "AWS4-HMAC-SHA256",
  "host": "youtiotendpoint.iot.us-east-1.amazonaws.com",
  "region": "us-east-1",
  "accesskey": "youraccesskey",
  "secretkey": "yoursecretkey",
  "topictopublish": "sundar/topic",
  "topictosubscribe": "sundar/topic2"

}



```


## 3d. Configuration for Log4net

Implement the following 'log4net.config' file to configure Log4net to perform logging for this application. 

``` XML

<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
  </configSections>  
  <log4net>
    <root>
      <level value="ALL" />
      <appender-ref ref="MyAppender" />
      <appender-ref ref="RollingFileAppender" />
    </root>
    <appender name="MyAppender" type="log4net.Appender.ConsoleAppender">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date %level %logger - %message%newline" />
      </layout>
    </appender>
    <appender name="MyFileAppender" type="log4net.Appender.FileAppender">
      <file value="applicationnew.log" />
      <appendToFile value="true" />
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date %level %logger - %message%newline" />
      </layout>
    </appender>
    <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
      <file value="rolling.log" />
      <appendToFile value="true" />
      <rollingStyle value="Size" />
      <maxSizeRollBackups value="5" />
      <maximumFileSize value="10MB" />
      <staticLogFileName value="true" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %level %logger - %message%newline" />
      </layout>
    </appender>
  </log4net>
</configuration>

```

This configuration file logs to both console output and also to file named 'applicationnew.log' in the same directory of the solution.

## 3e. Completed and working code

The completed and working code for this solution is available in the folder 'Dotnet core console app' of this repository. You can also refer that.


## 3f. Compile, run and verify

Compile and run the code in Visual Studio for Mac.

Select "1" for publishing messages. You should see status for successful publishing of messages in Console app.
<p align="center">
<img src="/images/publish1.png">
</p>

You can also verify that in AWS Iot Test Console.

<p align="center">
<img src="/images/publish2.png">
</p>


Run the solution again and select "2" for subscribing messages. You should see confirmation for sucessful creation of subscription for a specific topic.
Publish the messages on the specified topic from AWS Iot Test Console.

<p align="center">
<img src="/images/subscribe1.png">
</p>

You should see that message successfuly received by Console app.

<p align="center">
<img src="/images/subscribe2.png">
</p>

## 4. Conclusion
We have created a .NET framework win forms app that publishes messages messages to AWS IOT Core using MQTT protocol over Websockets channel with AWS Sigv4 authentication. The app also implemented logic for subscribing messages from AWS Iot Core using the same architecture. We also have created another reference implementation using .NET Core to publish messages to to AWS IOT Core using MQTT protocol over Websockets channel with AWS Sigv4 authentication. It also implemented logic for subscription of messages from AWS IOT Core using the same architecture. This completes the post.


## License

This library is licensed under the Apache 2.0 License. 
