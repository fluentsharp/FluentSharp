// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class ascx_Task
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.taskGroupBox = new System.Windows.Forms.GroupBox();
            this.lbExecutionSecondsCounter = new System.Windows.Forms.Label();
            this.lbTaskStatus = new System.Windows.Forms.Label();
            this.lbResultText = new System.Windows.Forms.Label();
            this.lbResulsObject = new System.Windows.Forms.Label();
            this.lbSourceObject = new System.Windows.Forms.Label();
            this.llConfig = new System.Windows.Forms.LinkLabel();
            this.taskProgressBar = new System.Windows.Forms.ProgressBar();
            this.llStartStop = new System.Windows.Forms.LinkLabel();
            this.taskGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // taskGroupBox
            // 
            this.taskGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                              | System.Windows.Forms.AnchorStyles.Left)
                                                                             | System.Windows.Forms.AnchorStyles.Right)));
            this.taskGroupBox.Controls.Add(this.lbExecutionSecondsCounter);
            this.taskGroupBox.Controls.Add(this.lbTaskStatus);
            this.taskGroupBox.Controls.Add(this.lbResultText);
            this.taskGroupBox.Controls.Add(this.lbResulsObject);
            this.taskGroupBox.Controls.Add(this.lbSourceObject);
            this.taskGroupBox.Controls.Add(this.llConfig);
            this.taskGroupBox.Controls.Add(this.taskProgressBar);
            this.taskGroupBox.Controls.Add(this.llStartStop);
            this.taskGroupBox.Location = new System.Drawing.Point(4, 1);
            this.taskGroupBox.Name = "taskGroupBox";
            this.taskGroupBox.Size = new System.Drawing.Size(198, 165);
            this.taskGroupBox.TabIndex = 0;
            this.taskGroupBox.TabStop = false;
            this.taskGroupBox.Text = "taskName";
            // 
            // lbExecutionSecondsCounter
            // 
            this.lbExecutionSecondsCounter.Location = new System.Drawing.Point(136, 14);
            this.lbExecutionSecondsCounter.Name = "lbExecutionSecondsCounter";
            this.lbExecutionSecondsCounter.Size = new System.Drawing.Size(28, 14);
            this.lbExecutionSecondsCounter.TabIndex = 8;
            this.lbExecutionSecondsCounter.Text = "0 s";
            this.lbExecutionSecondsCounter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbTaskStatus
            // 
            this.lbTaskStatus.Location = new System.Drawing.Point(11, 29);
            this.lbTaskStatus.Name = "lbTaskStatus";
            this.lbTaskStatus.Size = new System.Drawing.Size(119, 12);
            this.lbTaskStatus.TabIndex = 7;
            this.lbTaskStatus.Text = "...";
            // 
            // lbResultText
            // 
            this.lbResultText.Location = new System.Drawing.Point(164, 14);
            this.lbResultText.Name = "lbResultText";
            this.lbResultText.Size = new System.Drawing.Size(28, 14);
            this.lbResultText.TabIndex = 6;
            this.lbResultText.Text = "...";
            this.lbResultText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbResulsObject
            // 
            this.lbResulsObject.AutoSize = true;
            this.lbResulsObject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lbResulsObject.Location = new System.Drawing.Point(86, 63);
            this.lbResulsObject.Name = "lbResulsObject";
            this.lbResulsObject.Size = new System.Drawing.Size(69, 13);
            this.lbResulsObject.TabIndex = 5;
            this.lbResulsObject.Text = "results object";
            this.lbResulsObject.Visible = false;            
            // 
            // lbSourceObject
            // 
            this.lbSourceObject.AllowDrop = true;
            this.lbSourceObject.AutoSize = true;
            this.lbSourceObject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbSourceObject.Location = new System.Drawing.Point(9, 63);
            this.lbSourceObject.Name = "lbSourceObject";
            this.lbSourceObject.Size = new System.Drawing.Size(71, 13);
            this.lbSourceObject.TabIndex = 4;
            this.lbSourceObject.Text = "source object";           
            // 
            // llConfig
            // 
            this.llConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llConfig.AutoSize = true;
            this.llConfig.LinkColor = System.Drawing.Color.DarkGray;
            this.llConfig.Location = new System.Drawing.Point(156, 28);
            this.llConfig.Name = "llConfig";
            this.llConfig.Size = new System.Drawing.Size(36, 13);
            this.llConfig.TabIndex = 3;
            this.llConfig.TabStop = true;
            this.llConfig.Text = "config";
            this.llConfig.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llConfig_LinkClicked);
            // 
            // taskProgressBar
            // 
            this.taskProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                                | System.Windows.Forms.AnchorStyles.Right)));
            this.taskProgressBar.Location = new System.Drawing.Point(12, 14);
            this.taskProgressBar.Name = "taskProgressBar";
            this.taskProgressBar.Size = new System.Drawing.Size(127, 14);
            this.taskProgressBar.TabIndex = 2;
            // 
            // llStartStop
            // 
            this.llStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llStartStop.AutoSize = true;
            this.llStartStop.Location = new System.Drawing.Point(161, 63);
            this.llStartStop.Name = "llStartStop";
            this.llStartStop.Size = new System.Drawing.Size(27, 13);
            this.llStartStop.TabIndex = 0;
            this.llStartStop.TabStop = true;
            this.llStartStop.Text = "start";
            this.llStartStop.Visible = false;
            this.llStartStop.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llStartStop_LinkClicked);
            // 
            // ascx_Task
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.taskGroupBox);
            this.Name = "ascx_Task";
            this.Size = new System.Drawing.Size(205, 169);            
            this.taskGroupBox.ResumeLayout(false);
            this.taskGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox taskGroupBox;
        private System.Windows.Forms.ProgressBar taskProgressBar;
        private System.Windows.Forms.LinkLabel llStartStop;
        private System.Windows.Forms.LinkLabel llConfig;
        private System.Windows.Forms.Label lbResulsObject;
        private System.Windows.Forms.Label lbSourceObject;
        private System.Windows.Forms.Label lbResultText;
        private System.Windows.Forms.Label lbTaskStatus;
        private System.Windows.Forms.Label lbExecutionSecondsCounter;
    }
}
