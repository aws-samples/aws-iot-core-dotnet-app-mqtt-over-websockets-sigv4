using System;
using System.Text;
using awsiotmqttoverwebsocketswinapp.View;
using awsiotmqttoverwebsocketswinapp.Presenter;
using awsiotmqttoverwebsocketswinapp.Utils;

namespace awsiotmqttoverwebsocketswinapp
{
    public partial class Form : System.Windows.Forms.Form, IAwsIotView
    {
        private delegate void AppendSubscribeMessageDelegate(string message);
        private delegate void UpdateConnectLabelDelegate(string message);
        private delegate void UpdatePublishLabelDelegate(string message);
        private readonly StringBuilder subscribeMessage = new StringBuilder();

        private readonly AwsIotPresenter presenter;

        public Form()
        {
            InitializeComponent();
            Logger.LogInfo("Initialized");

            presenter = new AwsIotPresenter(this);
        }

        public string HostText
        {
            get
            {
               return txtIotEndpoint.Text;
            }

            set
            {
                txtIotEndpoint.Text = value;
            }
        }
        public string RegionText
        {
            get
            {
                return txtRegion.Text;
            }
            set
            {
                txtRegion.Text = value;
            }
        }

        public string AccessKeyText
        {
            get
            {
                return txtAccessKey.Text;
            }
            set
            {
                txtAccessKey.Text = value;
            }
        }

        public string SecretKeyText
        {
            get
            {
                return txtSecretKey.Text;
            }
            set
            {
                txtSecretKey.Text = value;
            }
        }

        public string TopicToPublishText
        {
            get
            {
                return txtTopicToPublish.Text;
            }

            set
            {
                txtTopicToPublish.Text = value;
            }
        }

        public string TopicToSubscribeText
        {
            get
            {
                return txtSubscribeTopic.Text;
            }

            set
            {
                txtSubscribeTopic.Text = value;
            }
        }

        public string ConnectStatusLabel
        {
            get
            {
                return lblConnectStatus.Text;
            }

            set
            {
                if (InvokeRequired)
                {
                    lblConnectStatus.Invoke(new UpdateConnectLabelDelegate(UpdateConnectLabel), value);
                }
                else
                {
                    lblConnectStatus.Text = value;
                }
            }
        }

        public string PublishStatusLabel
        {
            get
            {
                return lblPubStatus.Text;
            }

            set
            {
                lblPubStatus.Text = value;
            }
        }

        public string SubscribeStatusLabel
        {
            get
            {
                return lblSubscriptionStatus.Text;
            }

            set
            {
                lblSubscriptionStatus.Text = value;
            }
        }

        public string PublishMessageText
        {
            get
            {
                return txtPublishMessage.Text;
            }

            set
            {
                txtPublishMessage.Text = value;
            }
        }

        public string ReceivedMessageText
        {
            get
            {
                return txtSubscribeMessage.Text;
            }

            set
            {
                if (InvokeRequired)
                {
                    txtSubscribeMessage.Invoke(new AppendSubscribeMessageDelegate(AppendSubscribeMessage), value);
                }
                else
                {
                    txtSubscribeMessage.AppendText(value);
                }
            }
        }
        private async void btnConnect_Click(object sender, EventArgs e)
        {            
            await presenter.ConnectToAwsIoT();
        }

        private async void btnPublish_Click(object sender, EventArgs e)
        {
            if (presenter.mqttClient == null)
            {
                await presenter.ConnectToAwsIoT();
            }

            await presenter.PublishMessage(PublishMessageText, TopicToPublishText);

            PublishStatusLabel = $"Published to {TopicToPublishText}";
        }

        private async void btnSubscribe_Click(object sender, EventArgs e)
        {            
            if (presenter.mqttClient == null)
            {
                await presenter.ConnectToAwsIoT();
            }

            await presenter.SubscribeTo(TopicToSubscribeText);
        }

        private void AppendSubscribeMessage(string message)
        {
            subscribeMessage.Append(message);
            txtSubscribeMessage.AppendText(subscribeMessage.ToString());
        }

        private void UpdateConnectLabel(string message)
        {
            lblConnectStatus.Text = message;
        }
    }
}
