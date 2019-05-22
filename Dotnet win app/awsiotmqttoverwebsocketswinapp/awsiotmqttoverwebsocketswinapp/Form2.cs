using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using awsiotmqttoverwebsocketswinapp.View;
using awsiotmqttoverwebsocketswinapp.Presenter;
using awsiotmqttoverwebsocketswinapp.Utils;


namespace awsiotmqttoverwebsocketswinapp
{
    public partial class Form2 : Form, IAwsIotView
    {
        private delegate void AppendSubscribeMessageDelegate(string message);
        private delegate void UpdateConnectLabelDelegate(string message);
        private delegate void UpdatePublishLabelDelegate(string message);
        private StringBuilder subscribeMessage = new StringBuilder();

        private AwsIotPresenter presenter1;

        public Form2()
        {
            InitializeComponent();
            Logger.LogInfo("Initialized");
        }

        string IAwsIotView.HostText
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
        string IAwsIotView.RegionText
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

        string IAwsIotView.AccessKeyText
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

        string IAwsIotView.SecretKeyText
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

        string IAwsIotView.TopicToPublishText
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

        string IAwsIotView.TopicToSubscribeText
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

        string IAwsIotView.ConnectStatusLabel
        {
            get
            {
                return lblConnectStatus.Text;
            }

            set
            {
                
                lblConnectStatus.Invoke(new UpdateConnectLabelDelegate(UpdateConnectLabel), value);
            }
        }

        string IAwsIotView.PublishStatusLabel
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

        string IAwsIotView.PublishMessageText
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


        string IAwsIotView.SubscribeMessageText
        {
            get
            {
                return txtSubscribeMessage.Text;
            }

            set
            {
                // txtSubscribeMessage.Text = value;
                txtSubscribeMessage.Invoke(new AppendSubscribeMessageDelegate(AppendSubscribeMessage), value);
            }
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (presenter1 == null)
            {
                presenter1 = new AwsIotPresenter(this);

            }

            presenter1.MakeConnection();

        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (presenter1 == null)
            {
                presenter1 = new AwsIotPresenter(this);

            }

            if (presenter1.mqttClient == null)
            {
                presenter1.MakeConnection();
            }

            presenter1.PublishMessage();
           
            lblPubStatus.Text = "Published";
        }

        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            if (presenter1 == null)
            {
                presenter1 = new AwsIotPresenter(this);

            }
            
            if (presenter1.mqttClient == null)
            {
                presenter1.MakeConnection();
            }

            presenter1.SubscribeMessage();


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
