// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class ReportBug
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tBoxFromEmail = new System.Windows.Forms.TextBox();
            this.tbSubject = new System.Windows.Forms.TextBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbFirstCheckToMailServer = new System.Windows.Forms.Label();
            this.lbMailServerConnectErrorMessage = new System.Windows.Forms.Label();
            this.lbMailServerLabel = new System.Windows.Forms.Label();
            this.tbMailServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tBoxToEmail = new System.Windows.Forms.TextBox();
            this.btSendMessage = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btRefresh_LogViewerData = new System.Windows.Forms.Button();
            this.rtbLogViewContentsToSend = new System.Windows.Forms.RichTextBox();
            this.pbScreenShotToSend = new System.Windows.Forms.PictureBox();
            this.btRefresh_ScreenShot = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbScreenShotToSend)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subject";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "From (email)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Message";
            // 
            // tBoxFromEmail
            // 
            this.tBoxFromEmail.BackColor = System.Drawing.Color.Coral;
            this.tBoxFromEmail.Location = new System.Drawing.Point(72, 30);
            this.tBoxFromEmail.Name = "tBoxFromEmail";
            this.tBoxFromEmail.Size = new System.Drawing.Size(258, 20);
            this.tBoxFromEmail.TabIndex = 3;
            this.tBoxFromEmail.TextChanged += new System.EventHandler(this.tBoxFromEmail_TextChanged);
            // 
            // tbSubject
            // 
            this.tbSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSubject.BackColor = System.Drawing.Color.Coral;
            this.tbSubject.Location = new System.Drawing.Point(72, 52);
            this.tbSubject.Name = "tbSubject";
            this.tbSubject.Size = new System.Drawing.Size(711, 20);
            this.tbSubject.TabIndex = 4;
            this.tbSubject.TextChanged += new System.EventHandler(this.tbSubject_TextChanged);
            // 
            // tbMessage
            // 
            this.tbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMessage.BackColor = System.Drawing.Color.Coral;
            this.tbMessage.Location = new System.Drawing.Point(72, 74);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMessage.Size = new System.Drawing.Size(711, 132);
            this.tbMessage.TabIndex = 6;
            this.tbMessage.TextChanged += new System.EventHandler(this.tbMessage_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.lbFirstCheckToMailServer);
            this.splitContainer1.Panel1.Controls.Add(this.lbMailServerConnectErrorMessage);
            this.splitContainer1.Panel1.Controls.Add(this.lbMailServerLabel);
            this.splitContainer1.Panel1.Controls.Add(this.tbMailServer);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.tBoxToEmail);
            this.splitContainer1.Panel1.Controls.Add(this.btSendMessage);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.tbMessage);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.tbSubject);
            this.splitContainer1.Panel1.Controls.Add(this.tBoxFromEmail);
            this.splitContainer1.Panel1.ForeColor = System.Drawing.Color.Black;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(885, 424);
            this.splitContainer1.SplitterDistance = 219;
            this.splitContainer1.TabIndex = 7;
            // 
            // lbFirstCheckToMailServer
            // 
            this.lbFirstCheckToMailServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFirstCheckToMailServer.AutoSize = true;
            this.lbFirstCheckToMailServer.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbFirstCheckToMailServer.Location = new System.Drawing.Point(714, 10);
            this.lbFirstCheckToMailServer.Name = "lbFirstCheckToMailServer";
            this.lbFirstCheckToMailServer.Size = new System.Drawing.Size(172, 13);
            this.lbFirstCheckToMailServer.TabIndex = 14;
            this.lbFirstCheckToMailServer.Text = "Checking Mail Server online Status";
            // 
            // lbMailServerConnectErrorMessage
            // 
            this.lbMailServerConnectErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMailServerConnectErrorMessage.AutoSize = true;
            this.lbMailServerConnectErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMailServerConnectErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.lbMailServerConnectErrorMessage.Location = new System.Drawing.Point(473, 31);
            this.lbMailServerConnectErrorMessage.Name = "lbMailServerConnectErrorMessage";
            this.lbMailServerConnectErrorMessage.Size = new System.Drawing.Size(398, 13);
            this.lbMailServerConnectErrorMessage.TabIndex = 11;
            this.lbMailServerConnectErrorMessage.Text = "Error: Could not connected to mail server,  please enter another one:";
            this.lbMailServerConnectErrorMessage.Visible = false;
            // 
            // lbMailServerLabel
            // 
            this.lbMailServerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMailServerLabel.AutoSize = true;
            this.lbMailServerLabel.Location = new System.Drawing.Point(653, 9);
            this.lbMailServerLabel.Name = "lbMailServerLabel";
            this.lbMailServerLabel.Size = new System.Drawing.Size(60, 13);
            this.lbMailServerLabel.TabIndex = 9;
            this.lbMailServerLabel.Text = "Mail Server";
            this.lbMailServerLabel.Visible = false;
            // 
            // tbMailServer
            // 
            this.tbMailServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMailServer.BackColor = System.Drawing.Color.Gray;
            this.tbMailServer.Location = new System.Drawing.Point(720, 8);
            this.tbMailServer.Name = "tbMailServer";
            this.tbMailServer.Size = new System.Drawing.Size(151, 20);
            this.tbMailServer.TabIndex = 10;
            this.tbMailServer.Visible = false;
            this.tbMailServer.TextChanged += new System.EventHandler(this.tbMailServer_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "To";
            // 
            // tBoxToEmail
            // 
            this.tBoxToEmail.BackColor = System.Drawing.Color.Gray;
            this.tBoxToEmail.Location = new System.Drawing.Point(72, 8);
            this.tBoxToEmail.Name = "tBoxToEmail";
            this.tBoxToEmail.Size = new System.Drawing.Size(179, 20);
            this.tBoxToEmail.TabIndex = 8;
            // 
            // btSendMessage
            // 
            this.btSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSendMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSendMessage.Location = new System.Drawing.Point(787, 74);
            this.btSendMessage.Name = "btSendMessage";
            this.btSendMessage.Size = new System.Drawing.Size(91, 91);
            this.btSendMessage.TabIndex = 0;
            this.btSendMessage.Text = "Send Message";
            this.btSendMessage.UseVisualStyleBackColor = true;
            this.btSendMessage.Click += new System.EventHandler(this.btSendMessage_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel1.Controls.Add(this.btRefresh_LogViewerData);
            this.splitContainer2.Panel1.Controls.Add(this.rtbLogViewContentsToSend);
            this.splitContainer2.Panel1.ForeColor = System.Drawing.Color.Black;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel2.Controls.Add(this.pbScreenShotToSend);
            this.splitContainer2.Panel2.Controls.Add(this.btRefresh_ScreenShot);
            this.splitContainer2.Panel2.ForeColor = System.Drawing.Color.Black;
            this.splitContainer2.Size = new System.Drawing.Size(885, 201);
            this.splitContainer2.SplitterDistance = 418;
            this.splitContainer2.TabIndex = 0;
            // 
            // btRefresh_LogViewerData
            // 
            this.btRefresh_LogViewerData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRefresh_LogViewerData.Location = new System.Drawing.Point(351, 1);
            this.btRefresh_LogViewerData.Name = "btRefresh_LogViewerData";
            this.btRefresh_LogViewerData.Size = new System.Drawing.Size(60, 23);
            this.btRefresh_LogViewerData.TabIndex = 2;
            this.btRefresh_LogViewerData.Text = "Refresh";
            this.btRefresh_LogViewerData.UseVisualStyleBackColor = true;
            this.btRefresh_LogViewerData.Click += new System.EventHandler(this.btRefresh_LogViewerData_Click);
            // 
            // rtbLogViewContentsToSend
            // 
            this.rtbLogViewContentsToSend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLogViewContentsToSend.Location = new System.Drawing.Point(3, 28);
            this.rtbLogViewContentsToSend.Name = "rtbLogViewContentsToSend";
            this.rtbLogViewContentsToSend.Size = new System.Drawing.Size(408, 166);
            this.rtbLogViewContentsToSend.TabIndex = 1;
            this.rtbLogViewContentsToSend.Text = "";
            // 
            // pbScreenShotToSend
            // 
            this.pbScreenShotToSend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbScreenShotToSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbScreenShotToSend.Location = new System.Drawing.Point(3, 28);
            this.pbScreenShotToSend.Name = "pbScreenShotToSend";
            this.pbScreenShotToSend.Size = new System.Drawing.Size(453, 166);
            this.pbScreenShotToSend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbScreenShotToSend.TabIndex = 4;
            this.pbScreenShotToSend.TabStop = false;
            // 
            // btRefresh_ScreenShot
            // 
            this.btRefresh_ScreenShot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRefresh_ScreenShot.ForeColor = System.Drawing.Color.Black;
            this.btRefresh_ScreenShot.Location = new System.Drawing.Point(398, 1);
            this.btRefresh_ScreenShot.Name = "btRefresh_ScreenShot";
            this.btRefresh_ScreenShot.Size = new System.Drawing.Size(60, 23);
            this.btRefresh_ScreenShot.TabIndex = 3;
            this.btRefresh_ScreenShot.Text = "Refresh";
            this.btRefresh_ScreenShot.UseVisualStyleBackColor = true;
            this.btRefresh_ScreenShot.Click += new System.EventHandler(this.btRefresh_ScreenShot_Click);
            // 
            // ReportBug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(885, 424);
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ReportBug";
            this.Text = "Ask for Help from O2 Support, Report a Bug or Send Comment";
            this.Load += new System.EventHandler(this.ReportBug_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbScreenShotToSend)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBoxFromEmail;
        private System.Windows.Forms.TextBox tbSubject;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btSendMessage;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox rtbLogViewContentsToSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tBoxToEmail;
        private System.Windows.Forms.Label lbMailServerConnectErrorMessage;
        private System.Windows.Forms.Label lbMailServerLabel;
        private System.Windows.Forms.TextBox tbMailServer;
        private System.Windows.Forms.Label lbFirstCheckToMailServer;
        private System.Windows.Forms.Button btRefresh_LogViewerData;
        private System.Windows.Forms.Button btRefresh_ScreenShot;
        private System.Windows.Forms.PictureBox pbScreenShotToSend;
    }
}