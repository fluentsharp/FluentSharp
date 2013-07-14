// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class DirectoryViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryViewer));
            this.tvDirectory = new System.Windows.Forms.TreeView();
            this.directoryMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewDirectoryName = new System.Windows.Forms.ToolStripTextBox();
            this.miCreateDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRenameSelectedItem = new System.Windows.Forms.ToolStripTextBox();
            this.deleteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allowDragAndDropToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilDirectoriesAndFiles = new System.Windows.Forms.ImageList(this.components);
            this.btSelectDirectory = new System.Windows.Forms.Button();
            this.btCreateDirectory = new System.Windows.Forms.Button();
            this.tbNewDirectoryName = new System.Windows.Forms.TextBox();
            this.tbNewFileName = new System.Windows.Forms.TextBox();
            this.btCreateFile = new System.Windows.Forms.Button();
            this.btRefresh = new System.Windows.Forms.Button();
            this.btDeleteFile = new System.Windows.Forms.Button();
            this.tbCurrentDirectoryName = new System.Windows.Forms.TextBox();
            this.cbWatchFolder = new System.Windows.Forms.CheckBox();
            this.cbMoveOnDrag = new System.Windows.Forms.CheckBox();
            this.scAddressAndRest = new System.Windows.Forms.SplitContainer();
            this.scViewerAndSettings = new System.Windows.Forms.SplitContainer();
            this.createFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directoryMenu.SuspendLayout();
            this.scAddressAndRest.Panel1.SuspendLayout();
            this.scAddressAndRest.Panel2.SuspendLayout();
            this.scAddressAndRest.SuspendLayout();
            this.scViewerAndSettings.Panel1.SuspendLayout();
            this.scViewerAndSettings.Panel2.SuspendLayout();
            this.scViewerAndSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvDirectory
            // 
            this.tvDirectory.AllowDrop = true;
            this.tvDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvDirectory.ContextMenuStrip = this.directoryMenu;
            this.tvDirectory.FullRowSelect = true;
            this.tvDirectory.HideSelection = false;
            this.tvDirectory.ImageIndex = 0;
            this.tvDirectory.ImageList = this.ilDirectoriesAndFiles;
            this.tvDirectory.Location = new System.Drawing.Point(3, -1);
            this.tvDirectory.Name = "tvDirectory";
            this.tvDirectory.SelectedImageIndex = 0;
            this.tvDirectory.ShowNodeToolTips = true;
            this.tvDirectory.Size = new System.Drawing.Size(179, 225);
            this.tvDirectory.TabIndex = 18;
            this.tvDirectory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDirectory_AfterSelect);
            this.tvDirectory.Click += new System.EventHandler(this.tvDirectory_Click);
            this.tvDirectory.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvDirectory_DragDrop);
            this.tvDirectory.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvDirectory_DragEnter);
            this.tvDirectory.DoubleClick += new System.EventHandler(this.tvDirectory_DoubleClick);
            this.tvDirectory.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvDirectory_KeyUp);
            this.tvDirectory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvDirectory_MouseDown);
            this.tvDirectory.MouseEnter += new System.EventHandler(this.tvDirectory_MouseEnter);
            this.tvDirectory.MouseHover += new System.EventHandler(this.tvDirectory_MouseHover);
            // 
            // directoryMenu
            // 
            this.directoryMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createDirectoryToolStripMenuItem,
            this.renameFileToolStripMenuItem,
            this.deleteFileToolStripMenuItem,
            this.deleteFolderToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.allowDragAndDropToolStripMenuItem,
            this.createFileToolStripMenuItem});
            this.directoryMenu.Name = "contextMenuStrip1";
            this.directoryMenu.Size = new System.Drawing.Size(187, 180);
            // 
            // createDirectoryToolStripMenuItem
            // 
            this.createDirectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNewDirectoryName,
            this.miCreateDirectory});
            this.createDirectoryToolStripMenuItem.Name = "createDirectoryToolStripMenuItem";
            this.createDirectoryToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.createDirectoryToolStripMenuItem.Text = "Create Directory";
            // 
            // miNewDirectoryName
            // 
            this.miNewDirectoryName.Name = "miNewDirectoryName";
            this.miNewDirectoryName.Size = new System.Drawing.Size(180, 23);
            this.miNewDirectoryName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.miNewDirectoryName_KeyUp);
            // 
            // miCreateDirectory
            // 
            this.miCreateDirectory.Name = "miCreateDirectory";
            this.miCreateDirectory.Size = new System.Drawing.Size(256, 22);
            this.miCreateDirectory.Text = "Create Directory (put name above)";
            this.miCreateDirectory.Click += new System.EventHandler(this.miCreateDirectory_Click);
            // 
            // renameFileToolStripMenuItem
            // 
            this.renameFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbRenameSelectedItem});
            this.renameFileToolStripMenuItem.Name = "renameFileToolStripMenuItem";
            this.renameFileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.renameFileToolStripMenuItem.Text = "Rename File";
            // 
            // tbRenameSelectedItem
            // 
            this.tbRenameSelectedItem.Name = "tbRenameSelectedItem";
            this.tbRenameSelectedItem.Size = new System.Drawing.Size(100, 23);
            this.tbRenameSelectedItem.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbRenameSelectedItem_KeyUp);
            // 
            // deleteFileToolStripMenuItem
            // 
            this.deleteFileToolStripMenuItem.Name = "deleteFileToolStripMenuItem";
            this.deleteFileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.deleteFileToolStripMenuItem.Text = "Delete File";
            this.deleteFileToolStripMenuItem.Click += new System.EventHandler(this.deleteFileToolStripMenuItem_Click);
            // 
            // deleteFolderToolStripMenuItem
            // 
            this.deleteFolderToolStripMenuItem.Name = "deleteFolderToolStripMenuItem";
            this.deleteFolderToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.deleteFolderToolStripMenuItem.Text = "Delete Folder";
            this.deleteFolderToolStripMenuItem.Click += new System.EventHandler(this.deleteFolderToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // allowDragAndDropToolStripMenuItem
            // 
            this.allowDragAndDropToolStripMenuItem.Name = "allowDragAndDropToolStripMenuItem";
            this.allowDragAndDropToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.allowDragAndDropToolStripMenuItem.Text = "Allow Drag And Drop";
            this.allowDragAndDropToolStripMenuItem.Click += new System.EventHandler(this.allowDragAndDropToolStripMenuItem_Click);
            // 
            // ilDirectoriesAndFiles
            // 
            this.ilDirectoriesAndFiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilDirectoriesAndFiles.ImageStream")));
            this.ilDirectoriesAndFiles.TransparentColor = System.Drawing.Color.Transparent;
            this.ilDirectoriesAndFiles.Images.SetKeyName(0, "Explorer_Folder.ico");
            this.ilDirectoriesAndFiles.Images.SetKeyName(1, "Explorer_File.ico");
            this.ilDirectoriesAndFiles.Images.SetKeyName(2, "project_sourceRoot.ico");
            this.ilDirectoriesAndFiles.Images.SetKeyName(3, "Edit_remove.ico");
            this.ilDirectoriesAndFiles.Images.SetKeyName(4, "refresh_active.ico");
            this.ilDirectoriesAndFiles.Images.SetKeyName(5, "accessories-text-editor.png");
            // 
            // btSelectDirectory
            // 
            this.btSelectDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelectDirectory.Location = new System.Drawing.Point(152, 0);
            this.btSelectDirectory.Name = "btSelectDirectory";
            this.btSelectDirectory.Size = new System.Drawing.Size(30, 19);
            this.btSelectDirectory.TabIndex = 21;
            this.btSelectDirectory.Text = "....";
            this.btSelectDirectory.UseVisualStyleBackColor = true;
            this.btSelectDirectory.Click += new System.EventHandler(this.btSelectDirectory_Click);
            // 
            // btCreateDirectory
            // 
            this.btCreateDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCreateDirectory.Location = new System.Drawing.Point(112, 48);
            this.btCreateDirectory.Name = "btCreateDirectory";
            this.btCreateDirectory.Size = new System.Drawing.Size(71, 20);
            this.btCreateDirectory.TabIndex = 24;
            this.btCreateDirectory.Text = "Create Dir";
            this.btCreateDirectory.UseVisualStyleBackColor = true;
            this.btCreateDirectory.Click += new System.EventHandler(this.btCreateDirectory_Click);
            // 
            // tbNewDirectoryName
            // 
            this.tbNewDirectoryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNewDirectoryName.Location = new System.Drawing.Point(3, 48);
            this.tbNewDirectoryName.Name = "tbNewDirectoryName";
            this.tbNewDirectoryName.Size = new System.Drawing.Size(103, 20);
            this.tbNewDirectoryName.TabIndex = 25;
            // 
            // tbNewFileName
            // 
            this.tbNewFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNewFileName.Location = new System.Drawing.Point(3, 22);
            this.tbNewFileName.Name = "tbNewFileName";
            this.tbNewFileName.Size = new System.Drawing.Size(104, 20);
            this.tbNewFileName.TabIndex = 27;
            // 
            // btCreateFile
            // 
            this.btCreateFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCreateFile.Location = new System.Drawing.Point(113, 22);
            this.btCreateFile.Name = "btCreateFile";
            this.btCreateFile.Size = new System.Drawing.Size(70, 20);
            this.btCreateFile.TabIndex = 26;
            this.btCreateFile.Text = "Create File";
            this.btCreateFile.UseVisualStyleBackColor = true;
            this.btCreateFile.Click += new System.EventHandler(this.btCreateFile_Click);
            // 
            // btRefresh
            // 
            this.btRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRefresh.ImageKey = "refresh_active.ico";
            this.btRefresh.ImageList = this.ilDirectoriesAndFiles;
            this.btRefresh.Location = new System.Drawing.Point(112, 0);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(30, 19);
            this.btRefresh.TabIndex = 23;
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // btDeleteFile
            // 
            this.btDeleteFile.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btDeleteFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDeleteFile.ImageKey = "Edit_remove.ico";
            this.btDeleteFile.ImageList = this.ilDirectoriesAndFiles;
            this.btDeleteFile.Location = new System.Drawing.Point(3, 1);
            this.btDeleteFile.Name = "btDeleteFile";
            this.btDeleteFile.Size = new System.Drawing.Size(30, 19);
            this.btDeleteFile.TabIndex = 22;
            this.btDeleteFile.UseVisualStyleBackColor = true;
            this.btDeleteFile.Click += new System.EventHandler(this.btDeleteFile_Click);
            // 
            // tbCurrentDirectoryName
            // 
            this.tbCurrentDirectoryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentDirectoryName.BackColor = System.Drawing.Color.Silver;
            this.tbCurrentDirectoryName.Location = new System.Drawing.Point(3, 3);
            this.tbCurrentDirectoryName.Name = "tbCurrentDirectoryName";
            this.tbCurrentDirectoryName.Size = new System.Drawing.Size(179, 20);
            this.tbCurrentDirectoryName.TabIndex = 28;
            this.tbCurrentDirectoryName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCurrentDirectoryName_KeyPress);
            // 
            // cbWatchFolder
            // 
            this.cbWatchFolder.AutoSize = true;
            this.cbWatchFolder.Location = new System.Drawing.Point(3, 71);
            this.cbWatchFolder.Name = "cbWatchFolder";
            this.cbWatchFolder.Size = new System.Drawing.Size(90, 17);
            this.cbWatchFolder.TabIndex = 29;
            this.cbWatchFolder.Text = "Watch Folder";
            this.cbWatchFolder.UseVisualStyleBackColor = true;
            this.cbWatchFolder.CheckedChanged += new System.EventHandler(this.cbMonitorFolder_CheckedChanged);
            // 
            // cbMoveOnDrag
            // 
            this.cbMoveOnDrag.AutoSize = true;
            this.cbMoveOnDrag.Location = new System.Drawing.Point(90, 71);
            this.cbMoveOnDrag.Name = "cbMoveOnDrag";
            this.cbMoveOnDrag.Size = new System.Drawing.Size(94, 17);
            this.cbMoveOnDrag.TabIndex = 30;
            this.cbMoveOnDrag.Text = "Move on Drag";
            this.cbMoveOnDrag.UseVisualStyleBackColor = true;
            // 
            // scAddressAndRest
            // 
            this.scAddressAndRest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scAddressAndRest.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scAddressAndRest.Location = new System.Drawing.Point(0, 0);
            this.scAddressAndRest.Name = "scAddressAndRest";
            this.scAddressAndRest.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scAddressAndRest.Panel1
            // 
            this.scAddressAndRest.Panel1.Controls.Add(this.tbCurrentDirectoryName);
            this.scAddressAndRest.Panel1MinSize = 20;
            // 
            // scAddressAndRest.Panel2
            // 
            this.scAddressAndRest.Panel2.Controls.Add(this.scViewerAndSettings);
            this.scAddressAndRest.Size = new System.Drawing.Size(186, 345);
            this.scAddressAndRest.SplitterDistance = 20;
            this.scAddressAndRest.TabIndex = 31;
            // 
            // scViewerAndSettings
            // 
            this.scViewerAndSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scViewerAndSettings.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scViewerAndSettings.Location = new System.Drawing.Point(0, 0);
            this.scViewerAndSettings.Name = "scViewerAndSettings";
            this.scViewerAndSettings.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scViewerAndSettings.Panel1
            // 
            this.scViewerAndSettings.Panel1.Controls.Add(this.tvDirectory);
            // 
            // scViewerAndSettings.Panel2
            // 
            this.scViewerAndSettings.Panel2.Controls.Add(this.tbNewFileName);
            this.scViewerAndSettings.Panel2.Controls.Add(this.cbMoveOnDrag);
            this.scViewerAndSettings.Panel2.Controls.Add(this.btRefresh);
            this.scViewerAndSettings.Panel2.Controls.Add(this.btCreateFile);
            this.scViewerAndSettings.Panel2.Controls.Add(this.btDeleteFile);
            this.scViewerAndSettings.Panel2.Controls.Add(this.btSelectDirectory);
            this.scViewerAndSettings.Panel2.Controls.Add(this.cbWatchFolder);
            this.scViewerAndSettings.Panel2.Controls.Add(this.tbNewDirectoryName);
            this.scViewerAndSettings.Panel2.Controls.Add(this.btCreateDirectory);
            this.scViewerAndSettings.Size = new System.Drawing.Size(186, 321);
            this.scViewerAndSettings.SplitterDistance = 227;
            this.scViewerAndSettings.TabIndex = 0;
            // 
            // createFileToolStripMenuItem
            // 
            this.createFileToolStripMenuItem.Name = "createFileToolStripMenuItem";
            this.createFileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.createFileToolStripMenuItem.Text = "Create File";
            this.createFileToolStripMenuItem.Click += new System.EventHandler(this.createFileToolStripMenuItem_Click);
            // 
            // DirectoryViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.scAddressAndRest);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "DirectoryViewer";
            this.Size = new System.Drawing.Size(186, 345);
            this.Load += new System.EventHandler(this.ascx_Directory_Load);
            this.directoryMenu.ResumeLayout(false);
            this.scAddressAndRest.Panel1.ResumeLayout(false);
            this.scAddressAndRest.Panel1.PerformLayout();
            this.scAddressAndRest.Panel2.ResumeLayout(false);
            this.scAddressAndRest.ResumeLayout(false);
            this.scViewerAndSettings.Panel1.ResumeLayout(false);
            this.scViewerAndSettings.Panel2.ResumeLayout(false);
            this.scViewerAndSettings.Panel2.PerformLayout();
            this.scViewerAndSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvDirectory;
        private System.Windows.Forms.ImageList ilDirectoriesAndFiles;
        private System.Windows.Forms.Button btSelectDirectory;
        private System.Windows.Forms.Button btDeleteFile;
        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.Button btCreateDirectory;
        private System.Windows.Forms.TextBox tbNewDirectoryName;
        private System.Windows.Forms.TextBox tbNewFileName;
        private System.Windows.Forms.Button btCreateFile;
        private System.Windows.Forms.TextBox tbCurrentDirectoryName;
        private System.Windows.Forms.CheckBox cbWatchFolder;
        private System.Windows.Forms.CheckBox cbMoveOnDrag;
        private System.Windows.Forms.SplitContainer scAddressAndRest;
        private System.Windows.Forms.SplitContainer scViewerAndSettings;
        private System.Windows.Forms.ContextMenuStrip directoryMenu;
        private System.Windows.Forms.ToolStripMenuItem createDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox miNewDirectoryName;
        private System.Windows.Forms.ToolStripMenuItem miCreateDirectory;
        private System.Windows.Forms.ToolStripMenuItem renameFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox tbRenameSelectedItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allowDragAndDropToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFileToolStripMenuItem;
    }
}
