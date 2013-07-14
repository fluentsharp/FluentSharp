// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class ascx_WorkingOnTask
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
            this.lbTaskCurrentlyWorkingOn = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lbTimeElapsed = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbTaskCurrentlyWorkingOn
            // 
            this.lbTaskCurrentlyWorkingOn.AutoSize = true;
            this.lbTaskCurrentlyWorkingOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTaskCurrentlyWorkingOn.Location = new System.Drawing.Point(3, 9);
            this.lbTaskCurrentlyWorkingOn.Name = "lbTaskCurrentlyWorkingOn";
            this.lbTaskCurrentlyWorkingOn.Size = new System.Drawing.Size(129, 24);
            this.lbTaskCurrentlyWorkingOn.TabIndex = 0;
            this.lbTaskCurrentlyWorkingOn.Text = "Current Task";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(7, 38);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(352, 18);
            this.progressBar.TabIndex = 1;
            // 
            // lbTimeElapsed
            // 
            this.lbTimeElapsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTimeElapsed.AutoSize = true;
            this.lbTimeElapsed.Location = new System.Drawing.Point(362, 40);
            this.lbTimeElapsed.Name = "lbTimeElapsed";
            this.lbTimeElapsed.Size = new System.Drawing.Size(35, 13);
            this.lbTimeElapsed.TabIndex = 2;
            this.lbTimeElapsed.Text = "0 Sec";
            // 
            // ascx_WorkingOnTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lbTimeElapsed);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lbTaskCurrentlyWorkingOn);
            this.Name = "ascx_WorkingOnTask";
            this.Size = new System.Drawing.Size(411, 95);
            this.Load += new System.EventHandler(this.ascx_WorkingOnTask_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTaskCurrentlyWorkingOn;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lbTimeElapsed;
    }
}