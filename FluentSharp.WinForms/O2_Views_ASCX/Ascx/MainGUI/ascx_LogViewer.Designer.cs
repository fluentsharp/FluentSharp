// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class ascx_LogViewer
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
			this.rtbDebugMessages = new System.Windows.Forms.RichTextBox();
			this.btClearDebugView = new System.Windows.Forms.Button();
			this.btAddNewLineToDebugWindiw = new System.Windows.Forms.Button();
			this.cbErrorMessages = new System.Windows.Forms.CheckBox();
			this.cbDebugMessages = new System.Windows.Forms.CheckBox();
			this.cbInfoMessages = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.llGCCollect = new System.Windows.Forms.LinkLabel();
			this.lbMemoryUsed = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// rtbDebugMessages
			// 
			this.rtbDebugMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rtbDebugMessages.Location = new System.Drawing.Point(0, 17);
			this.rtbDebugMessages.Name = "rtbDebugMessages";
			this.rtbDebugMessages.Size = new System.Drawing.Size(447, 107);
			this.rtbDebugMessages.TabIndex = 27;
			this.rtbDebugMessages.Text = "";
			this.rtbDebugMessages.WordWrap = false;
			this.rtbDebugMessages.TextChanged += new System.EventHandler(this.rtbDebugMessages_TextChanged);
			// 
			// btClearDebugView
			// 
			this.btClearDebugView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btClearDebugView.BackColor = System.Drawing.Color.White;
			this.btClearDebugView.ForeColor = System.Drawing.Color.Black;
			this.btClearDebugView.Location = new System.Drawing.Point(2, 127);
			this.btClearDebugView.Name = "btClearDebugView";
			this.btClearDebugView.Size = new System.Drawing.Size(13, 19);
			this.btClearDebugView.TabIndex = 31;
			this.btClearDebugView.Text = "X";
			this.btClearDebugView.UseVisualStyleBackColor = false;
			this.btClearDebugView.Click += new System.EventHandler(this.btClearDebugView_Click);
			// 
			// btAddNewLineToDebugWindiw
			// 
			this.btAddNewLineToDebugWindiw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btAddNewLineToDebugWindiw.BackColor = System.Drawing.Color.White;
			this.btAddNewLineToDebugWindiw.ForeColor = System.Drawing.Color.Black;
			this.btAddNewLineToDebugWindiw.Location = new System.Drawing.Point(21, 127);
			this.btAddNewLineToDebugWindiw.Name = "btAddNewLineToDebugWindiw";
			this.btAddNewLineToDebugWindiw.Size = new System.Drawing.Size(13, 19);
			this.btAddNewLineToDebugWindiw.TabIndex = 30;
			this.btAddNewLineToDebugWindiw.Text = ".";
			this.btAddNewLineToDebugWindiw.UseVisualStyleBackColor = false;
			this.btAddNewLineToDebugWindiw.Click += new System.EventHandler(this.btAddNewLineToDebugWindiw_Click);
			// 
			// cbErrorMessages
			// 
			this.cbErrorMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbErrorMessages.AutoSize = true;
			this.cbErrorMessages.Checked = true;
			this.cbErrorMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbErrorMessages.ForeColor = System.Drawing.Color.Black;
			this.cbErrorMessages.Location = new System.Drawing.Point(52, 130);
			this.cbErrorMessages.Name = "cbErrorMessages";
			this.cbErrorMessages.Size = new System.Drawing.Size(48, 17);
			this.cbErrorMessages.TabIndex = 32;
			this.cbErrorMessages.Text = "Error";
			this.cbErrorMessages.UseVisualStyleBackColor = true;
			this.cbErrorMessages.CheckedChanged += new System.EventHandler(this.cbErrorMessages_CheckedChanged);
			// 
			// cbDebugMessages
			// 
			this.cbDebugMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbDebugMessages.AutoSize = true;
			this.cbDebugMessages.Checked = true;
			this.cbDebugMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbDebugMessages.ForeColor = System.Drawing.Color.Black;
			this.cbDebugMessages.Location = new System.Drawing.Point(119, 130);
			this.cbDebugMessages.Name = "cbDebugMessages";
			this.cbDebugMessages.Size = new System.Drawing.Size(58, 17);
			this.cbDebugMessages.TabIndex = 33;
			this.cbDebugMessages.Text = "Debug";
			this.cbDebugMessages.UseVisualStyleBackColor = true;
			this.cbDebugMessages.CheckedChanged += new System.EventHandler(this.cbDebugMessages_CheckedChanged);
			// 
			// cbInfoMessages
			// 
			this.cbInfoMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbInfoMessages.AutoSize = true;
			this.cbInfoMessages.Checked = true;
			this.cbInfoMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbInfoMessages.ForeColor = System.Drawing.Color.Black;
			this.cbInfoMessages.Location = new System.Drawing.Point(196, 130);
			this.cbInfoMessages.Name = "cbInfoMessages";
			this.cbInfoMessages.Size = new System.Drawing.Size(44, 17);
			this.cbInfoMessages.TabIndex = 34;
			this.cbInfoMessages.Text = "Info";
			this.cbInfoMessages.UseVisualStyleBackColor = true;
			this.cbInfoMessages.CheckedChanged += new System.EventHandler(this.cbInfoMessages_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(-2, 1);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 35;
			this.label1.Text = "Log Viewer";
			// 
			// llGCCollect
			// 
			this.llGCCollect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.llGCCollect.AutoSize = true;
			this.llGCCollect.Location = new System.Drawing.Point(390, 131);
			this.llGCCollect.Name = "llGCCollect";
			this.llGCCollect.Size = new System.Drawing.Size(57, 13);
			this.llGCCollect.TabIndex = 36;
			this.llGCCollect.TabStop = true;
			this.llGCCollect.Text = "GC Collect";
			this.llGCCollect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llGCCollect_LinkClicked);
			// 
			// lbMemoryUsed
			// 
			this.lbMemoryUsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbMemoryUsed.AutoSize = true;
			this.lbMemoryUsed.Location = new System.Drawing.Point(246, 131);
			this.lbMemoryUsed.Name = "lbMemoryUsed";
			this.lbMemoryUsed.Size = new System.Drawing.Size(19, 13);
			this.lbMemoryUsed.TabIndex = 37;
			this.lbMemoryUsed.Text = "....";
			this.lbMemoryUsed.Click += new System.EventHandler(this.lbMemoryUsed_Click);
			// 
			// ascx_LogViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.Controls.Add(this.lbMemoryUsed);
			this.Controls.Add(this.llGCCollect);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbInfoMessages);
			this.Controls.Add(this.cbDebugMessages);
			this.Controls.Add(this.cbErrorMessages);
			this.Controls.Add(this.btClearDebugView);
			this.Controls.Add(this.btAddNewLineToDebugWindiw);
			this.Controls.Add(this.rtbDebugMessages);
			this.Location = new System.Drawing.Point(20, 600);
			this.Name = "ascx_LogViewer";
			this.Size = new System.Drawing.Size(450, 150);
			this.Load += new System.EventHandler(this.ascx_LogViewer_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbDebugMessages;
        private System.Windows.Forms.Button btClearDebugView;
        private System.Windows.Forms.Button btAddNewLineToDebugWindiw;
        private System.Windows.Forms.CheckBox cbErrorMessages;
        private System.Windows.Forms.CheckBox cbDebugMessages;
        private System.Windows.Forms.CheckBox cbInfoMessages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel llGCCollect;
		private System.Windows.Forms.Label lbMemoryUsed;
    }
}