// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class DownloadFile
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
// ReSharper disable ConvertToConstant
// ReSharper disable RedundantDefaultFieldInitializer
        private System.ComponentModel.IContainer components = null;
// ReSharper restore RedundantDefaultFieldInitializer
// ReSharper restore ConvertToConstant

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
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.prgDownload = new System.Windows.Forms.ProgressBar();
            this.btnDownload = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.btnPauseResume = new System.Windows.Forms.Button();
            this.cbCloseWindowWhenDownloadComplete = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(3, 89);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(151, 20);
            this.lblProgress.TabIndex = 16;
            this.lblProgress.Text = "Awaiting Download";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(367, 61);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 15;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(6, 38);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(83, 13);
            this.lblPath.TabIndex = 14;
            this.lblPath.Text = "Download Path:";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(95, 35);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(347, 20);
            this.txtPath.TabIndex = 13;
            // 
            // prgDownload
            // 
            this.prgDownload.Location = new System.Drawing.Point(7, 115);
            this.prgDownload.Name = "prgDownload";
            this.prgDownload.Size = new System.Drawing.Size(436, 23);
            this.prgDownload.TabIndex = 12;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(187, 61);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(93, 23);
            this.btnDownload.TabIndex = 11;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(95, 9);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(347, 20);
            this.txtUrl.TabIndex = 10;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(6, 12);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(83, 13);
            this.lblUrl.TabIndex = 9;
            this.lblUrl.Text = "Download URL:";
            // 
            // btnPauseResume
            // 
            this.btnPauseResume.Enabled = false;
            this.btnPauseResume.Location = new System.Drawing.Point(286, 61);
            this.btnPauseResume.Name = "btnPauseResume";
            this.btnPauseResume.Size = new System.Drawing.Size(75, 23);
            this.btnPauseResume.TabIndex = 17;
            this.btnPauseResume.Text = "Pause";
            this.btnPauseResume.UseVisualStyleBackColor = true;
            this.btnPauseResume.Click += new System.EventHandler(this.btnPauseResume_Click);
            // 
            // cbCloseWindowWhenDownloadComplete
            // 
            this.cbCloseWindowWhenDownloadComplete.AutoSize = true;
            this.cbCloseWindowWhenDownloadComplete.Location = new System.Drawing.Point(7, 143);
            this.cbCloseWindowWhenDownloadComplete.Name = "cbCloseWindowWhenDownloadComplete";
            this.cbCloseWindowWhenDownloadComplete.Size = new System.Drawing.Size(231, 17);
            this.cbCloseWindowWhenDownloadComplete.TabIndex = 18;
            this.cbCloseWindowWhenDownloadComplete.Text = "Close window when download is completed";
            this.cbCloseWindowWhenDownloadComplete.UseVisualStyleBackColor = true;
            // 
            // DownloadFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbCloseWindowWhenDownloadComplete);
            this.Controls.Add(this.btnPauseResume);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.prgDownload);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lblUrl);
            this.Name = "DownloadFile";
            this.Size = new System.Drawing.Size(452, 187);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.ProgressBar prgDownload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Button btnPauseResume;
        private System.Windows.Forms.CheckBox cbCloseWindowWhenDownloadComplete;
    }
}
