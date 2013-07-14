// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class ascx_O2InstallAndTempDirectories
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.directoryWithO2Install = new global::FluentSharp.WinForms.Controls.DirectoryViewer();
            this.tbCurrentO2InstallDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbMessage_DeletingTempFolder = new System.Windows.Forms.Label();
            this.btDeleteTempFolderContents = new System.Windows.Forms.Button();
            this.directoryWithO2TempDir = new global::FluentSharp.WinForms.Controls.DirectoryViewer();
            this.btChangeTempDir = new System.Windows.Forms.Button();
            this.tbCurrentO2TempDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbMessage_O2TempFolderContentsDeleted = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.directoryWithO2Install);
            this.splitContainer1.Panel1.Controls.Add(this.tbCurrentO2InstallDirectory);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lbMessage_O2TempFolderContentsDeleted);
            this.splitContainer1.Panel2.Controls.Add(this.lbMessage_DeletingTempFolder);
            this.splitContainer1.Panel2.Controls.Add(this.btDeleteTempFolderContents);
            this.splitContainer1.Panel2.Controls.Add(this.directoryWithO2TempDir);
            this.splitContainer1.Panel2.Controls.Add(this.btChangeTempDir);
            this.splitContainer1.Panel2.Controls.Add(this.tbCurrentO2TempDir);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(452, 345);
            this.splitContainer1.SplitterDistance = 208;
            this.splitContainer1.TabIndex = 0;
            // 
            // directoryWithO2Install
            // 
            this.directoryWithO2Install._ProcessDroppedObjects = true;
            this.directoryWithO2Install._ShowFileSize = false;
            this.directoryWithO2Install._ShowLinkToUpperFolder = true;
            this.directoryWithO2Install._ViewMode = global::FluentSharp.WinForms.Controls.DirectoryViewer.ViewMode.Simple;
            this.directoryWithO2Install._WatchFolder = false;
            this.directoryWithO2Install.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryWithO2Install.BackColor = System.Drawing.SystemColors.Control;
            this.directoryWithO2Install.ForeColor = System.Drawing.Color.Black;
            this.directoryWithO2Install.Location = new System.Drawing.Point(7, 46);
            this.directoryWithO2Install.Name = "directoryWithO2Install";
            this.directoryWithO2Install.Size = new System.Drawing.Size(194, 294);
            this.directoryWithO2Install.TabIndex = 2;
            // 
            // tbCurrentO2InstallDirectory
            // 
            this.tbCurrentO2InstallDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                                            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentO2InstallDirectory.Location = new System.Drawing.Point(7, 19);
            this.tbCurrentO2InstallDirectory.Name = "tbCurrentO2InstallDirectory";
            this.tbCurrentO2InstallDirectory.Size = new System.Drawing.Size(194, 20);
            this.tbCurrentO2InstallDirectory.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current O2 Install Directory";
            // 
            // lbMessage_DeletingTempFolder
            // 
            this.lbMessage_DeletingTempFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbMessage_DeletingTempFolder.AutoSize = true;
            this.lbMessage_DeletingTempFolder.ForeColor = System.Drawing.Color.Red;
            this.lbMessage_DeletingTempFolder.Location = new System.Drawing.Point(3, 322);
            this.lbMessage_DeletingTempFolder.Name = "lbMessage_DeletingTempFolder";
            this.lbMessage_DeletingTempFolder.Size = new System.Drawing.Size(170, 13);
            this.lbMessage_DeletingTempFolder.TabIndex = 6;
            this.lbMessage_DeletingTempFolder.Text = "Deleting O2 Temp Folder Contents";
            this.lbMessage_DeletingTempFolder.Visible = false;
            // 
            // btDeleteTempFolderContents
            // 
            this.btDeleteTempFolderContents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                                                                                           | System.Windows.Forms.AnchorStyles.Right)));
            this.btDeleteTempFolderContents.Location = new System.Drawing.Point(6, 290);
            this.btDeleteTempFolderContents.Name = "btDeleteTempFolderContents";
            this.btDeleteTempFolderContents.Size = new System.Drawing.Size(227, 26);
            this.btDeleteTempFolderContents.TabIndex = 5;
            this.btDeleteTempFolderContents.Text = "Delete Temp Folder Contents";
            this.btDeleteTempFolderContents.UseVisualStyleBackColor = true;
            this.btDeleteTempFolderContents.Click += new System.EventHandler(this.btDeleteTempFolderContents_Click);
            // 
            // directoryWithO2TempDir
            // 
            this.directoryWithO2TempDir._ProcessDroppedObjects = true;
            this.directoryWithO2TempDir._ShowFileSize = false;
            this.directoryWithO2TempDir._ShowLinkToUpperFolder = true;
            this.directoryWithO2TempDir._ViewMode = global::FluentSharp.WinForms.Controls.DirectoryViewer.ViewMode.Simple;
            this.directoryWithO2TempDir._WatchFolder = true;
            this.directoryWithO2TempDir.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryWithO2TempDir.BackColor = System.Drawing.SystemColors.Control;
            this.directoryWithO2TempDir.ForeColor = System.Drawing.Color.Black;
            this.directoryWithO2TempDir.Location = new System.Drawing.Point(6, 48);
            this.directoryWithO2TempDir.Name = "directoryWithO2TempDir";
            this.directoryWithO2TempDir.Size = new System.Drawing.Size(227, 236);
            this.directoryWithO2TempDir.TabIndex = 4;
            // 
            // btChangeTempDir
            // 
            this.btChangeTempDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btChangeTempDir.Location = new System.Drawing.Point(179, 19);
            this.btChangeTempDir.Name = "btChangeTempDir";
            this.btChangeTempDir.Size = new System.Drawing.Size(54, 23);
            this.btChangeTempDir.TabIndex = 3;
            this.btChangeTempDir.Text = "change";
            this.btChangeTempDir.UseVisualStyleBackColor = true;
            this.btChangeTempDir.Click += new System.EventHandler(this.btChangeTempDir_Click);
            // 
            // tbCurrentO2TempDir
            // 
            this.tbCurrentO2TempDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                                   | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentO2TempDir.Location = new System.Drawing.Point(6, 19);
            this.tbCurrentO2TempDir.Name = "tbCurrentO2TempDir";
            this.tbCurrentO2TempDir.Size = new System.Drawing.Size(167, 20);
            this.tbCurrentO2TempDir.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Current O2 Temp Dir";
            // 
            // lbMessage_O2TempFolderContentsDeleted
            // 
            this.lbMessage_O2TempFolderContentsDeleted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbMessage_O2TempFolderContentsDeleted.AutoSize = true;
            this.lbMessage_O2TempFolderContentsDeleted.ForeColor = System.Drawing.Color.Green;
            this.lbMessage_O2TempFolderContentsDeleted.Location = new System.Drawing.Point(6, 322);
            this.lbMessage_O2TempFolderContentsDeleted.Name = "lbMessage_O2TempFolderContentsDeleted";
            this.lbMessage_O2TempFolderContentsDeleted.Size = new System.Drawing.Size(168, 13);
            this.lbMessage_O2TempFolderContentsDeleted.TabIndex = 7;
            this.lbMessage_O2TempFolderContentsDeleted.Text = "O2 Temp Folder Contents Deleted";
            this.lbMessage_O2TempFolderContentsDeleted.Visible = false;
            // 
            // ascx_O2InstallAndTempDirectories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ascx_O2InstallAndTempDirectories";
            this.Size = new System.Drawing.Size(452, 345);
            this.Load += new System.EventHandler(this.ascx_O2InstallAndTempDirectories_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tbCurrentO2InstallDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btChangeTempDir;
        private System.Windows.Forms.TextBox tbCurrentO2TempDir;
        private System.Windows.Forms.Label label2;
        private global::FluentSharp.WinForms.Controls.DirectoryViewer directoryWithO2Install;
        private global::FluentSharp.WinForms.Controls.DirectoryViewer directoryWithO2TempDir;
        private System.Windows.Forms.Button btDeleteTempFolderContents;
        private System.Windows.Forms.Label lbMessage_DeletingTempFolder;
        private System.Windows.Forms.Label lbMessage_O2TempFolderContentsDeleted;
    }
}