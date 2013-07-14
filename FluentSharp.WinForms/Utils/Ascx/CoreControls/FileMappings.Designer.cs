// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class FileMappings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileMappings));
            this.tvFileMappings = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tbExtensionsToShow = new System.Windows.Forms.TextBox();
            this.lbDropHelpInfo = new System.Windows.Forms.Label();
            this.cbRecursiveLoadForFolders = new System.Windows.Forms.CheckBox();
            this.llExtensionFilter_Java = new System.Windows.Forms.LinkLabel();
            this.llExtensionFilter_AllFiles = new System.Windows.Forms.LinkLabel();
            this.llExtensionFilter_DotNet = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbShowFileSizes = new System.Windows.Forms.CheckBox();
            this.cbOnDropClearLoadedFiles = new System.Windows.Forms.CheckBox();
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters = new System.Windows.Forms.CheckBox();
            this.cbOpenFilesOnSelection = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lbStatus = new System.Windows.Forms.ToolStripLabel();
            this.progressBarLoadFiles = new System.Windows.Forms.ToolStripProgressBar();
            this.btRrefresh = new System.Windows.Forms.ToolStripButton();
            this.btClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btLoadFilesFromO2TempDir = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btDragAllFileThatMatchExtensionFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.tbViewFilter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbNumberOfFilesLoaded = new System.Windows.Forms.Label();
            this.lbSelectedFile = new System.Windows.Forms.Label();
            this.llDragSelectedFiles = new System.Windows.Forms.LinkLabel();
            this.lbNumberOfFilesSelected = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvFileMappings
            // 
            this.tvFileMappings.AllowDrop = true;
            this.tvFileMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvFileMappings.FullRowSelect = true;
            this.tvFileMappings.HideSelection = false;
            this.tvFileMappings.ImageIndex = 0;
            this.tvFileMappings.ImageList = this.imageList1;
            this.tvFileMappings.Location = new System.Drawing.Point(3, 57);
            this.tvFileMappings.Name = "tvFileMappings";
            this.tvFileMappings.SelectedImageIndex = 0;
            this.tvFileMappings.ShowNodeToolTips = true;
            this.tvFileMappings.Size = new System.Drawing.Size(316, 201);
            this.tvFileMappings.TabIndex = 0;
            this.tvFileMappings.DoubleClick += new System.EventHandler(this.tvProjectFiles_DoubleClick);
            this.tvFileMappings.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvProjectFiles_DragDrop);
            this.tvFileMappings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvProjectFiles_AfterSelect);
            this.tvFileMappings.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvProjectFiles_DragEnter);
            this.tvFileMappings.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvFileMappings_KeyUp);
            this.tvFileMappings.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvFileMappings_ItemDrag);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Explorer_Folder.ico");
            this.imageList1.Images.SetKeyName(1, "Explorer_File.ico");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "(space delimited)";
            // 
            // tbExtensionsToShow
            // 
            this.tbExtensionsToShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbExtensionsToShow.Location = new System.Drawing.Point(98, 27);
            this.tbExtensionsToShow.Name = "tbExtensionsToShow";
            this.tbExtensionsToShow.Size = new System.Drawing.Size(201, 20);
            this.tbExtensionsToShow.TabIndex = 3;
            this.tbExtensionsToShow.TextChanged += new System.EventHandler(this.tbExtensionsToShow_TextChanged);
            this.tbExtensionsToShow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbExtensionsToShow_KeyPress);
            // 
            // lbDropHelpInfo
            // 
            this.lbDropHelpInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbDropHelpInfo.BackColor = System.Drawing.SystemColors.Window;
            this.lbDropHelpInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDropHelpInfo.Location = new System.Drawing.Point(92, 130);
            this.lbDropHelpInfo.Name = "lbDropHelpInfo";
            this.lbDropHelpInfo.Size = new System.Drawing.Size(149, 65);
            this.lbDropHelpInfo.TabIndex = 4;
            this.lbDropHelpInfo.Text = "To load items, drop Files or Folders here";
            this.lbDropHelpInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbRecursiveLoadForFolders
            // 
            this.cbRecursiveLoadForFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbRecursiveLoadForFolders.AutoSize = true;
            this.cbRecursiveLoadForFolders.Checked = true;
            this.cbRecursiveLoadForFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRecursiveLoadForFolders.Location = new System.Drawing.Point(6, 6);
            this.cbRecursiveLoadForFolders.Name = "cbRecursiveLoadForFolders";
            this.cbRecursiveLoadForFolders.Size = new System.Drawing.Size(153, 17);
            this.cbRecursiveLoadForFolders.TabIndex = 5;
            this.cbRecursiveLoadForFolders.Text = "Recursive Load for Folders";
            this.cbRecursiveLoadForFolders.UseVisualStyleBackColor = true;
            // 
            // llExtensionFilter_Java
            // 
            this.llExtensionFilter_Java.AutoSize = true;
            this.llExtensionFilter_Java.Location = new System.Drawing.Point(61, 6);
            this.llExtensionFilter_Java.Name = "llExtensionFilter_Java";
            this.llExtensionFilter_Java.Size = new System.Drawing.Size(54, 13);
            this.llExtensionFilter_Java.TabIndex = 9;
            this.llExtensionFilter_Java.TabStop = true;
            this.llExtensionFilter_Java.Text = "JAVA files";
            this.llExtensionFilter_Java.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llExtensionFilter_Java_LinkClicked);
            // 
            // llExtensionFilter_AllFiles
            // 
            this.llExtensionFilter_AllFiles.AutoSize = true;
            this.llExtensionFilter_AllFiles.Location = new System.Drawing.Point(6, 6);
            this.llExtensionFilter_AllFiles.Name = "llExtensionFilter_AllFiles";
            this.llExtensionFilter_AllFiles.Size = new System.Drawing.Size(38, 13);
            this.llExtensionFilter_AllFiles.TabIndex = 10;
            this.llExtensionFilter_AllFiles.TabStop = true;
            this.llExtensionFilter_AllFiles.Text = "all files";
            this.llExtensionFilter_AllFiles.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llExtensionFilter_AllFiles_LinkClicked);
            // 
            // llExtensionFilter_DotNet
            // 
            this.llExtensionFilter_DotNet.AutoSize = true;
            this.llExtensionFilter_DotNet.Location = new System.Drawing.Point(121, 6);
            this.llExtensionFilter_DotNet.Name = "llExtensionFilter_DotNet";
            this.llExtensionFilter_DotNet.Size = new System.Drawing.Size(53, 13);
            this.llExtensionFilter_DotNet.TabIndex = 11;
            this.llExtensionFilter_DotNet.TabStop = true;
            this.llExtensionFilter_DotNet.Text = ".NET files";
            this.llExtensionFilter_DotNet.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llExtensionFilter_DotNet_LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 280);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(313, 89);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tbExtensionsToShow);
            this.tabPage1.Controls.Add(this.llExtensionFilter_DotNet);
            this.tabPage1.Controls.Add(this.llExtensionFilter_Java);
            this.tabPage1.Controls.Add(this.llExtensionFilter_AllFiles);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(305, 63);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "extension filters";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbShowFileSizes);
            this.tabPage2.Controls.Add(this.cbOnDropClearLoadedFiles);
            this.tabPage2.Controls.Add(this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters);
            this.tabPage2.Controls.Add(this.cbOpenFilesOnSelection);
            this.tabPage2.Controls.Add(this.cbRecursiveLoadForFolders);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(305, 63);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "config";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbShowFileSizes
            // 
            this.cbShowFileSizes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbShowFileSizes.AutoSize = true;
            this.cbShowFileSizes.Checked = true;
            this.cbShowFileSizes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowFileSizes.Location = new System.Drawing.Point(163, 43);
            this.cbShowFileSizes.Name = "cbShowFileSizes";
            this.cbShowFileSizes.Size = new System.Drawing.Size(100, 17);
            this.cbShowFileSizes.TabIndex = 9;
            this.cbShowFileSizes.Text = "Show File Sizes";
            this.cbShowFileSizes.UseVisualStyleBackColor = true;
            // 
            // cbOnDropClearLoadedFiles
            // 
            this.cbOnDropClearLoadedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOnDropClearLoadedFiles.AutoSize = true;
            this.cbOnDropClearLoadedFiles.Checked = true;
            this.cbOnDropClearLoadedFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOnDropClearLoadedFiles.Location = new System.Drawing.Point(6, 43);
            this.cbOnDropClearLoadedFiles.Name = "cbOnDropClearLoadedFiles";
            this.cbOnDropClearLoadedFiles.Size = new System.Drawing.Size(149, 17);
            this.cbOnDropClearLoadedFiles.TabIndex = 8;
            this.cbOnDropClearLoadedFiles.Text = "On Drop Clear loaded files";
            this.cbOnDropClearLoadedFiles.UseVisualStyleBackColor = true;
            // 
            // cbOnDropOnlyLoadFilesThatMatchExtensionFilters
            // 
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.AutoSize = true;
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.Checked = true;
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.Enabled = false;
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.Location = new System.Drawing.Point(6, 25);
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.Name = "cbOnDropOnlyLoadFilesThatMatchExtensionFilters";
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.Size = new System.Drawing.Size(260, 17);
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.TabIndex = 7;
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.Text = "On Drop only load files that match extension filters";
            this.cbOnDropOnlyLoadFilesThatMatchExtensionFilters.UseVisualStyleBackColor = true;
            // 
            // cbOpenFilesOnSelection
            // 
            this.cbOpenFilesOnSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOpenFilesOnSelection.AutoSize = true;
            this.cbOpenFilesOnSelection.Checked = true;
            this.cbOpenFilesOnSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOpenFilesOnSelection.Location = new System.Drawing.Point(163, 6);
            this.cbOpenFilesOnSelection.Name = "cbOpenFilesOnSelection";
            this.cbOpenFilesOnSelection.Size = new System.Drawing.Size(138, 17);
            this.cbOpenFilesOnSelection.TabIndex = 6;
            this.cbOpenFilesOnSelection.Text = "Open Files on Selection";
            this.cbOpenFilesOnSelection.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus,
            this.progressBarLoadFiles,
            this.btRrefresh,
            this.btClear,
            this.toolStripSeparator3,
            this.btLoadFilesFromO2TempDir,
            this.toolStripSeparator1,
            this.btDragAllFileThatMatchExtensionFilter,
            this.toolStripSeparator2,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(322, 25);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lbStatus
            // 
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(19, 22);
            this.lbStatus.Text = "...";
            // 
            // progressBarLoadFiles
            // 
            this.progressBarLoadFiles.Name = "progressBarLoadFiles";
            this.progressBarLoadFiles.Size = new System.Drawing.Size(40, 22);
            this.progressBarLoadFiles.Step = 5;
            // 
            // btRrefresh
            // 
            this.btRrefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRrefresh.Image = ((System.Drawing.Image)(resources.GetObject("btRrefresh.Image")));
            this.btRrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRrefresh.Name = "btRrefresh";
            this.btRrefresh.Size = new System.Drawing.Size(23, 22);
            this.btRrefresh.Text = "Refresh";
            this.btRrefresh.Click += new System.EventHandler(this.btRrefresh_Click);
            // 
            // btClear
            // 
            this.btClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btClear.Image = ((System.Drawing.Image)(resources.GetObject("btClear.Image")));
            this.btClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(23, 22);
            this.btClear.Text = "Clear loaded files list";
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btLoadFilesFromO2TempDir
            // 
            this.btLoadFilesFromO2TempDir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btLoadFilesFromO2TempDir.Image = ((System.Drawing.Image)(resources.GetObject("btLoadFilesFromO2TempDir.Image")));
            this.btLoadFilesFromO2TempDir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btLoadFilesFromO2TempDir.Name = "btLoadFilesFromO2TempDir";
            this.btLoadFilesFromO2TempDir.Size = new System.Drawing.Size(23, 22);
            this.btLoadFilesFromO2TempDir.Text = "Load files from O2 Temp dir";
            this.btLoadFilesFromO2TempDir.Click += new System.EventHandler(this.btLoadFilesFromO2TempDir_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btDragAllFileThatMatchExtensionFilter
            // 
            this.btDragAllFileThatMatchExtensionFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDragAllFileThatMatchExtensionFilter.Image = ((System.Drawing.Image)(resources.GetObject("btDragAllFileThatMatchExtensionFilter.Image")));
            this.btDragAllFileThatMatchExtensionFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDragAllFileThatMatchExtensionFilter.Name = "btDragAllFileThatMatchExtensionFilter";
            this.btDragAllFileThatMatchExtensionFilter.Size = new System.Drawing.Size(23, 22);
            this.btDragAllFileThatMatchExtensionFilter.Text = "Drag All files that match current extension filter";
            this.btDragAllFileThatMatchExtensionFilter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btDragAllFileThatMatchExtensionFilter_MouseDown);
            this.btDragAllFileThatMatchExtensionFilter.Click += new System.EventHandler(this.btDragAllFileThatMatchExtensionFilter_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 261);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "view filter";
            // 
            // tbViewFilter
            // 
            this.tbViewFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbViewFilter.Location = new System.Drawing.Point(61, 258);
            this.tbViewFilter.Name = "tbViewFilter";
            this.tbViewFilter.Size = new System.Drawing.Size(258, 20);
            this.tbViewFilter.TabIndex = 12;
            this.tbViewFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbViewFilter_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "# of files loaded:";
            // 
            // lbNumberOfFilesLoaded
            // 
            this.lbNumberOfFilesLoaded.AutoSize = true;
            this.lbNumberOfFilesLoaded.Location = new System.Drawing.Point(87, 26);
            this.lbNumberOfFilesLoaded.Name = "lbNumberOfFilesLoaded";
            this.lbNumberOfFilesLoaded.Size = new System.Drawing.Size(13, 13);
            this.lbNumberOfFilesLoaded.TabIndex = 19;
            this.lbNumberOfFilesLoaded.Text = "0";
            // 
            // lbSelectedFile
            // 
            this.lbSelectedFile.AutoSize = true;
            this.lbSelectedFile.Location = new System.Drawing.Point(156, 26);
            this.lbSelectedFile.Name = "lbSelectedFile";
            this.lbSelectedFile.Size = new System.Drawing.Size(10, 13);
            this.lbSelectedFile.TabIndex = 20;
            this.lbSelectedFile.Text = ".";
            // 
            // llDragSelectedFiles
            // 
            this.llDragSelectedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llDragSelectedFiles.AutoSize = true;
            this.llDragSelectedFiles.Location = new System.Drawing.Point(230, 25);
            this.llDragSelectedFiles.Name = "llDragSelectedFiles";
            this.llDragSelectedFiles.Size = new System.Drawing.Size(92, 13);
            this.llDragSelectedFiles.TabIndex = 21;
            this.llDragSelectedFiles.TabStop = true;
            this.llDragSelectedFiles.Text = "drag selected files";
            this.llDragSelectedFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.llDragSelectedFiles_MouseDown);
            // 
            // lbNumberOfFilesSelected
            // 
            this.lbNumberOfFilesSelected.AutoSize = true;
            this.lbNumberOfFilesSelected.Location = new System.Drawing.Point(95, 42);
            this.lbNumberOfFilesSelected.Name = "lbNumberOfFilesSelected";
            this.lbNumberOfFilesSelected.Size = new System.Drawing.Size(13, 13);
            this.lbNumberOfFilesSelected.TabIndex = 22;
            this.lbNumberOfFilesSelected.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "# of files selected:";
            // 
            // FileMappings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbDropHelpInfo);
            this.Controls.Add(this.tvFileMappings);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbNumberOfFilesSelected);
            this.Controls.Add(this.llDragSelectedFiles);
            this.Controls.Add(this.lbSelectedFile);
            this.Controls.Add(this.lbNumberOfFilesLoaded);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbViewFilter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "FileMappings";
            this.Size = new System.Drawing.Size(322, 372);
            this.Load += new System.EventHandler(this.ascx_ProjectFiles_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvFileMappings;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbExtensionsToShow;
        private System.Windows.Forms.Label lbDropHelpInfo;
        private System.Windows.Forms.CheckBox cbRecursiveLoadForFolders;
        private System.Windows.Forms.LinkLabel llExtensionFilter_Java;
        private System.Windows.Forms.LinkLabel llExtensionFilter_AllFiles;
        private System.Windows.Forms.LinkLabel llExtensionFilter_DotNet;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox cbOpenFilesOnSelection;
        private System.Windows.Forms.CheckBox cbOnDropClearLoadedFiles;
        private System.Windows.Forms.CheckBox cbOnDropOnlyLoadFilesThatMatchExtensionFilters;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btRrefresh;
        private System.Windows.Forms.ToolStripButton btClear;
        private System.Windows.Forms.ToolStripButton btLoadFilesFromO2TempDir;
        private System.Windows.Forms.ToolStripButton btDragAllFileThatMatchExtensionFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.CheckBox cbShowFileSizes;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbViewFilter;
        private System.Windows.Forms.ToolStripProgressBar progressBarLoadFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbNumberOfFilesLoaded;
        private System.Windows.Forms.ToolStripLabel lbStatus;
        private System.Windows.Forms.Label lbSelectedFile;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.LinkLabel llDragSelectedFiles;
        private System.Windows.Forms.Label lbNumberOfFilesSelected;
        private System.Windows.Forms.Label label5;
    }
}
