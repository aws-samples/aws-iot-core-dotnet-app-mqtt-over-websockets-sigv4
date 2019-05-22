namespace awsiotmqttoverwebsocketswinapp
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSubscribe = new System.Windows.Forms.Button();
            this.txtSubscribeMessage = new System.Windows.Forms.TextBox();
            this.txtSubscribeTopic = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.lblPubStatus = new System.Windows.Forms.Label();
            this.lblPublishStatus = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPublish = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPublishMessage = new System.Windows.Forms.TextBox();
            this.txtTopicToPublish = new System.Windows.Forms.TextBox();
            this.lblTopic = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblConnectStatus = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.txtAccessKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIotEndpoint = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSubscribe
            // 
            this.btnSubscribe.Location = new System.Drawing.Point(563, 45);
            this.btnSubscribe.Name = "btnSubscribe";
            this.btnSubscribe.Size = new System.Drawing.Size(75, 20);
            this.btnSubscribe.TabIndex = 3;
            this.btnSubscribe.Text = "Subscribe";
            this.btnSubscribe.UseVisualStyleBackColor = true;
            this.btnSubscribe.Click += new System.EventHandler(this.btnSubscribe_Click);
            // 
            // txtSubscribeMessage
            // 
            this.txtSubscribeMessage.Location = new System.Drawing.Point(182, 85);
            this.txtSubscribeMessage.Multiline = true;
            this.txtSubscribeMessage.Name = "txtSubscribeMessage";
            this.txtSubscribeMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSubscribeMessage.Size = new System.Drawing.Size(318, 80);
            this.txtSubscribeMessage.TabIndex = 2;
            // 
            // txtSubscribeTopic
            // 
            this.txtSubscribeTopic.Location = new System.Drawing.Point(182, 35);
            this.txtSubscribeTopic.Name = "txtSubscribeTopic";
            this.txtSubscribeTopic.Size = new System.Drawing.Size(318, 20);
            this.txtSubscribeTopic.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnSubscribe);
            this.panel3.Controls.Add(this.txtSubscribeMessage);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.txtSubscribeTopic);
            this.panel3.Location = new System.Drawing.Point(13, 322);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(775, 192);
            this.panel3.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "TopicToSubscribe";
            // 
            // lblPubStatus
            // 
            this.lblPubStatus.AutoSize = true;
            this.lblPubStatus.Location = new System.Drawing.Point(653, 92);
            this.lblPubStatus.Name = "lblPubStatus";
            this.lblPubStatus.Size = new System.Drawing.Size(0, 13);
            this.lblPubStatus.TabIndex = 7;
            // 
            // lblPublishStatus
            // 
            this.lblPublishStatus.AutoSize = true;
            this.lblPublishStatus.Location = new System.Drawing.Point(638, 92);
            this.lblPublishStatus.Name = "lblPublishStatus";
            this.lblPublishStatus.Size = new System.Drawing.Size(7, 13);
            this.lblPublishStatus.TabIndex = 6;
            this.lblPublishStatus.Text = "\r\n";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(567, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "PublishStatus";
            // 
            // btnPublish
            // 
            this.btnPublish.Location = new System.Drawing.Point(562, 36);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(75, 20);
            this.btnPublish.TabIndex = 4;
            this.btnPublish.Text = "Publish";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Message To Publish";
            // 
            // txtPublishMessage
            // 
            this.txtPublishMessage.Location = new System.Drawing.Point(183, 85);
            this.txtPublishMessage.Name = "txtPublishMessage";
            this.txtPublishMessage.Size = new System.Drawing.Size(318, 20);
            this.txtPublishMessage.TabIndex = 2;
            // 
            // txtTopicToPublish
            // 
            this.txtTopicToPublish.Location = new System.Drawing.Point(183, 37);
            this.txtTopicToPublish.Name = "txtTopicToPublish";
            this.txtTopicToPublish.Size = new System.Drawing.Size(318, 20);
            this.txtTopicToPublish.TabIndex = 1;
            // 
            // lblTopic
            // 
            this.lblTopic.AutoSize = true;
            this.lblTopic.Location = new System.Drawing.Point(34, 37);
            this.lblTopic.Name = "lblTopic";
            this.lblTopic.Size = new System.Drawing.Size(81, 13);
            this.lblTopic.TabIndex = 0;
            this.lblTopic.Text = "TopicToPublish";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblPubStatus);
            this.panel2.Controls.Add(this.lblPublishStatus);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.btnPublish);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtPublishMessage);
            this.panel2.Controls.Add(this.txtTopicToPublish);
            this.panel2.Controls.Add(this.lblTopic);
            this.panel2.Location = new System.Drawing.Point(12, 171);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(776, 129);
            this.panel2.TabIndex = 4;
            // 
            // lblConnectStatus
            // 
            this.lblConnectStatus.AutoSize = true;
            this.lblConnectStatus.Location = new System.Drawing.Point(644, 115);
            this.lblConnectStatus.Name = "lblConnectStatus";
            this.lblConnectStatus.Size = new System.Drawing.Size(79, 13);
            this.lblConnectStatus.TabIndex = 11;
            this.lblConnectStatus.Text = "Not Connected";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(650, 115);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(561, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "ConnectStatus";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(638, 76);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 20);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Secret Key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Access Key";
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Location = new System.Drawing.Point(183, 108);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.PasswordChar = '$';
            this.txtSecretKey.Size = new System.Drawing.Size(318, 20);
            this.txtSecretKey.TabIndex = 5;
            // 
            // txtAccessKey
            // 
            this.txtAccessKey.Location = new System.Drawing.Point(183, 70);
            this.txtAccessKey.Name = "txtAccessKey";
            this.txtAccessKey.PasswordChar = '#';
            this.txtAccessKey.Size = new System.Drawing.Size(318, 20);
            this.txtAccessKey.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(567, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Region";
            // 
            // txtIotEndpoint
            // 
            this.txtIotEndpoint.Location = new System.Drawing.Point(183, 15);
            this.txtIotEndpoint.Name = "txtIotEndpoint";
            this.txtIotEndpoint.Size = new System.Drawing.Size(318, 20);
            this.txtIotEndpoint.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "AWS IOT Endpoint";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtRegion);
            this.panel1.Controls.Add(this.lblConnectStatus);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtSecretKey);
            this.panel1.Controls.Add(this.txtAccessKey);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtIotEndpoint);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 150);
            this.panel1.TabIndex = 3;
            // 
            // txtRegion
            // 
            this.txtRegion.Location = new System.Drawing.Point(638, 22);
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(100, 20);
            this.txtRegion.TabIndex = 12;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 514);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form2";
            this.Text = "AWS Iot Mqtt over Websockets";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSubscribe;
        private System.Windows.Forms.TextBox txtSubscribeMessage;
        private System.Windows.Forms.TextBox txtSubscribeTopic;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblPubStatus;
        private System.Windows.Forms.Label lblPublishStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPublishMessage;
        private System.Windows.Forms.TextBox txtTopicToPublish;
        private System.Windows.Forms.Label lblTopic;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblConnectStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.TextBox txtAccessKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIotEndpoint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRegion;
    }
}