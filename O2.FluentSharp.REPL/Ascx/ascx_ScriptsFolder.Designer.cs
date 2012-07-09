// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using O2.Views.ASCX.CoreControls;

namespace O2.External.SharpDevelop.Ascx
{
    partial class ascx_ScriptsFolder
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
            this.tvSampleScripts = new System.Windows.Forms.TreeView();
            this.scDirectoryAndEditor = new System.Windows.Forms.SplitContainer();
            this.cbOverrideWithDefaultSample = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.directoryWithSourceCodeFiles = new O2.Views.ASCX.CoreControls.ascx_Directory();
            this.scDirectoryAndEditor.Panel1.SuspendLayout();
            this.scDirectoryAndEditor.Panel2.SuspendLayout();
            this.scDirectoryAndEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvSampleScripts
            // 
            this.tvSampleScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvSampleScripts.HideSelection = false;
            this.tvSampleScripts.Location = new System.Drawing.Point(1, 20);
            this.tvSampleScripts.Name = "tvSampleScripts";
            this.tvSampleScripts.Size = new System.Drawing.Size(249, 80);
            this.tvSampleScripts.TabIndex = 0;
            this.tvSampleScripts.DoubleClick += new System.EventHandler(this.tvSampleScripts_DoubleClick);
            this.tvSampleScripts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSampleScripts_AfterSelect);
            // 
            // scDirectoryAndEditor
            // 
            this.scDirectoryAndEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scDirectoryAndEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scDirectoryAndEditor.Location = new System.Drawing.Point(0, 0);
            this.scDirectoryAndEditor.Name = "scDirectoryAndEditor";
            this.scDirectoryAndEditor.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scDirectoryAndEditor.Panel1
            // 
            this.scDirectoryAndEditor.Panel1.Controls.Add(this.cbOverrideWithDefaultSample);
            this.scDirectoryAndEditor.Panel1.Controls.Add(this.label1);
            this.scDirectoryAndEditor.Panel1.Controls.Add(this.tvSampleScripts);
            // 
            // scDirectoryAndEditor.Panel2
            // 
            this.scDirectoryAndEditor.Panel2.Controls.Add(this.directoryWithSourceCodeFiles);
            this.scDirectoryAndEditor.Size = new System.Drawing.Size(257, 586);
            this.scDirectoryAndEditor.SplitterDistance = 130;
            this.scDirectoryAndEditor.TabIndex = 7;
            // 
            // cbOverrideWithDefaultSample
            // 
            this.cbOverrideWithDefaultSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOverrideWithDefaultSample.AutoSize = true;
            this.cbOverrideWithDefaultSample.Location = new System.Drawing.Point(4, 105);
            this.cbOverrideWithDefaultSample.Name = "cbOverrideWithDefaultSample";
            this.cbOverrideWithDefaultSample.Size = new System.Drawing.Size(185, 17);
            this.cbOverrideWithDefaultSample.TabIndex = 2;
            this.cbOverrideWithDefaultSample.Text = "Override With Default Sample File";
            this.cbOverrideWithDefaultSample.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sample Scripts";
            // 
            // directoryWithSourceCodeFiles
            // 
            this.directoryWithSourceCodeFiles._ProcessDroppedObjects = true;
            this.directoryWithSourceCodeFiles._ShowFileSize = false;
            this.directoryWithSourceCodeFiles._ShowLinkToUpperFolder = true;
            this.directoryWithSourceCodeFiles._ViewMode = O2.Views.ASCX.CoreControls.ascx_Directory.ViewMode.Simple_With_LocationBar;
            this.directoryWithSourceCodeFiles._WatchFolder = true;
            this.directoryWithSourceCodeFiles.BackColor = System.Drawing.SystemColors.Control;
            this.directoryWithSourceCodeFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryWithSourceCodeFiles.ForeColor = System.Drawing.Color.Black;
            this.directoryWithSourceCodeFiles.Location = new System.Drawing.Point(0, 0);
            this.directoryWithSourceCodeFiles.Name = "directoryWithSourceCodeFiles";
            this.directoryWithSourceCodeFiles.Size = new System.Drawing.Size(253, 448);
            this.directoryWithSourceCodeFiles.TabIndex = 4;
            // 
            // ascx_ScriptsFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scDirectoryAndEditor);
            this.Name = "ascx_ScriptsFolder";
            this.Size = new System.Drawing.Size(257, 586);
            this.scDirectoryAndEditor.Panel1.ResumeLayout(false);
            this.scDirectoryAndEditor.Panel1.PerformLayout();
            this.scDirectoryAndEditor.Panel2.ResumeLayout(false);
            this.scDirectoryAndEditor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvSampleScripts;
        private ascx_Directory directoryWithSourceCodeFiles;
        private System.Windows.Forms.SplitContainer scDirectoryAndEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbOverrideWithDefaultSample;
    }
}
