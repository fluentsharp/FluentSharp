// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class ascx_FunctionsViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ascx_FunctionsViewer));
            this.tvTreeView = new System.Windows.Forms.TreeView();
            this.ilClassesAndMethods = new System.Windows.Forms.ImageList(this.components);
            this.lBoxListBox = new System.Windows.Forms.ListBox();
            this.ilSolutionsProjecstFiles = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lbViewName = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tbNotAllDataShown = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.checkAll = new System.Windows.Forms.ToolStripButton();
            this.uncheckAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lbFilter = new System.Windows.Forms.ToolStripLabel();
            this.tbFilter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.namespaceDepth = new System.Windows.Forms.ToolStripComboBox();
            this.showConfig = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showConfigButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.configGroupBox = new System.Windows.Forms.GroupBox();
            this.cbAdvancedViewMode = new System.Windows.Forms.CheckBox();
            this.llHideCheckboxes = new System.Windows.Forms.LinkLabel();
            this.llShowCheckboxes = new System.Windows.Forms.LinkLabel();
            this.llHideConfig = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.typeOfControlToShowData = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMaxItemsToShow = new System.Windows.Forms.TextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.collapseAll = new System.Windows.Forms.ToolStripButton();
            this.expandAll = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.configGroupBox.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvTreeView
            // 
            this.tvTreeView.AllowDrop = true;
            this.tvTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvTreeView.CheckBoxes = true;
            this.tvTreeView.HideSelection = false;
            this.tvTreeView.ImageIndex = 0;
            this.tvTreeView.ImageList = this.ilClassesAndMethods;
            this.tvTreeView.Location = new System.Drawing.Point(3, 28);
            this.tvTreeView.Name = "tvTreeView";
            this.tvTreeView.SelectedImageIndex = 0;
            this.tvTreeView.ShowNodeToolTips = true;
            this.tvTreeView.Size = new System.Drawing.Size(692, 345);
            this.tvTreeView.TabIndex = 7;
            this.tvTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTreeView_AfterCheck);
            this.tvTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTreeView_BeforeExpand);
            this.tvTreeView.DoubleClick += new System.EventHandler(this.tvTreeView_DoubleClick);
            this.tvTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvTreeView_DragDrop);
            this.tvTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTreeView_AfterSelect);
            this.tvTreeView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tvTreeView_MouseMove);
            this.tvTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvTreeView_DragEnter);
            this.tvTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvTreeView_NodeMouseClick);
            this.tvTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvTreeView_ItemDrag);
            // 
            // ilClassesAndMethods
            // 
            this.ilClassesAndMethods.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilClassesAndMethods.ImageStream")));
            this.ilClassesAndMethods.TransparentColor = System.Drawing.Color.Transparent;
            this.ilClassesAndMethods.Images.SetKeyName(0, "class");
            this.ilClassesAndMethods.Images.SetKeyName(1, "method_member");
            this.ilClassesAndMethods.Images.SetKeyName(2, "method_static");
            this.ilClassesAndMethods.Images.SetKeyName(3, "smartTrace");
            this.ilClassesAndMethods.Images.SetKeyName(4, "string.ico");
            this.ilClassesAndMethods.Images.SetKeyName(5, "redBall.ico");
            this.ilClassesAndMethods.Images.SetKeyName(6, "engineWithTick.ico");
            this.ilClassesAndMethods.Images.SetKeyName(7, "sort_descending.ico");
            this.ilClassesAndMethods.Images.SetKeyName(8, "sort_ascending.ico");
            this.ilClassesAndMethods.Images.SetKeyName(9, "Edit_copy.ico");
            this.ilClassesAndMethods.Images.SetKeyName(10, "Findings_Medium.ico");
            this.ilClassesAndMethods.Images.SetKeyName(11, "Findings_High.ico");
            this.ilClassesAndMethods.Images.SetKeyName(12, "Findings_Info.ico");
            // 
            // lBoxListBox
            // 
            this.lBoxListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lBoxListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lBoxListBox.FormattingEnabled = true;
            this.lBoxListBox.Location = new System.Drawing.Point(3, 27);
            this.lBoxListBox.Name = "lBoxListBox";
            this.lBoxListBox.Size = new System.Drawing.Size(692, 338);
            this.lBoxListBox.Sorted = true;
            this.lBoxListBox.TabIndex = 6;
            // 
            // ilSolutionsProjecstFiles
            // 
            this.ilSolutionsProjecstFiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSolutionsProjecstFiles.ImageStream")));
            this.ilSolutionsProjecstFiles.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSolutionsProjecstFiles.Images.SetKeyName(0, "Explorer_allApplications.ico");
            this.ilSolutionsProjecstFiles.Images.SetKeyName(1, "Explorer_Application.ico");
            this.ilSolutionsProjecstFiles.Images.SetKeyName(2, "Explorer_Project.ico");
            this.ilSolutionsProjecstFiles.Images.SetKeyName(3, "Explorer_Folder.ico");
            this.ilSolutionsProjecstFiles.Images.SetKeyName(4, "Explorer_File.ico");
            this.ilSolutionsProjecstFiles.Images.SetKeyName(5, "ExpandAll.ico");
            this.ilSolutionsProjecstFiles.Images.SetKeyName(6, "CollapseAll.ico");
            this.ilSolutionsProjecstFiles.Images.SetKeyName(7, "PatternLibrary_Delete.ico");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbViewName,
            this.toolStripSeparator4,
            this.tbNotAllDataShown,
            this.toolStripSeparator5,
            this.checkAll,
            this.uncheckAll,
            this.toolStripSeparator1,
            this.lbFilter,
            this.tbFilter,
            this.toolStripSeparator3,
            this.toolStripLabel3,
            this.namespaceDepth,
            this.showConfig,
            this.toolStripSeparator2,
            this.showConfigButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(698, 25);
            this.toolStrip1.TabIndex = 70;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lbViewName
            // 
            this.lbViewName.BackColor = System.Drawing.SystemColors.Control;
            this.lbViewName.Name = "lbViewName";
            this.lbViewName.Size = new System.Drawing.Size(88, 22);
            this.lbViewName.Text = "Functions Viewer";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tbNotAllDataShown
            // 
            this.tbNotAllDataShown.BackColor = System.Drawing.SystemColors.Control;
            this.tbNotAllDataShown.Name = "tbNotAllDataShown";
            this.tbNotAllDataShown.Size = new System.Drawing.Size(19, 22);
            this.tbNotAllDataShown.Text = "...";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // checkAll
            // 
            this.checkAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.checkAll.Image = ((System.Drawing.Image)(resources.GetObject("checkAll.Image")));
            this.checkAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.checkAll.Name = "checkAll";
            this.checkAll.Size = new System.Drawing.Size(23, 22);
            this.checkAll.Text = "Check All";
            this.checkAll.Click += new System.EventHandler(this.checkAll_Click);
            // 
            // uncheckAll
            // 
            this.uncheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.uncheckAll.Image = ((System.Drawing.Image)(resources.GetObject("uncheckAll.Image")));
            this.uncheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uncheckAll.Name = "uncheckAll";
            this.uncheckAll.Size = new System.Drawing.Size(23, 22);
            this.uncheckAll.Text = "Un Check All";
            this.uncheckAll.Click += new System.EventHandler(this.uncheckAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lbFilter
            // 
            this.lbFilter.Name = "lbFilter";
            this.lbFilter.Size = new System.Drawing.Size(33, 22);
            this.lbFilter.Text = "filter:";
            // 
            // tbFilter
            // 
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(60, 25);
            this.tbFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbFilter_KeyPress);
            this.tbFilter.TextChanged += new System.EventHandler(this.tbFilter_TextChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(94, 22);
            this.toolStripLabel3.Text = "Namespace Depth";
            // 
            // namespaceDepth
            // 
            this.namespaceDepth.DropDownHeight = 300;
            this.namespaceDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.namespaceDepth.IntegralHeight = false;
            this.namespaceDepth.Items.AddRange(new object[] {
            "-1  (show all functions)",
            "0 (show all types)",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11"});
            this.namespaceDepth.Name = "namespaceDepth";
            this.namespaceDepth.Size = new System.Drawing.Size(150, 25);
            this.namespaceDepth.SelectedIndexChanged += new System.EventHandler(this.namespaceDepth_SelectedIndexChanged);
            // 
            // showConfig
            // 
            this.showConfig.BackColor = System.Drawing.SystemColors.Control;
            this.showConfig.Name = "showConfig";
            this.showConfig.Size = new System.Drawing.Size(64, 22);
            this.showConfig.Text = "show config";
            this.showConfig.Click += new System.EventHandler(this.showConfig_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // showConfigButton
            // 
            this.showConfigButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showConfigButton.Image = ((System.Drawing.Image)(resources.GetObject("showConfigButton.Image")));
            this.showConfigButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showConfigButton.Name = "showConfigButton";
            this.showConfigButton.Size = new System.Drawing.Size(29, 22);
            this.showConfigButton.Text = "Show Config";
            this.showConfigButton.Click += new System.EventHandler(this.showConfigButton_Click);
            // 
            // configGroupBox
            // 
            this.configGroupBox.Controls.Add(this.cbAdvancedViewMode);
            this.configGroupBox.Controls.Add(this.llHideCheckboxes);
            this.configGroupBox.Controls.Add(this.llShowCheckboxes);
            this.configGroupBox.Controls.Add(this.llHideConfig);
            this.configGroupBox.Controls.Add(this.label3);
            this.configGroupBox.Controls.Add(this.typeOfControlToShowData);
            this.configGroupBox.Controls.Add(this.label2);
            this.configGroupBox.Controls.Add(this.tbMaxItemsToShow);
            this.configGroupBox.Controls.Add(this.toolStrip2);
            this.configGroupBox.Location = new System.Drawing.Point(316, 27);
            this.configGroupBox.Name = "configGroupBox";
            this.configGroupBox.Size = new System.Drawing.Size(308, 109);
            this.configGroupBox.TabIndex = 71;
            this.configGroupBox.TabStop = false;
            this.configGroupBox.Text = "Config options";
            this.configGroupBox.Visible = false;
            // 
            // cbAdvancedViewMode
            // 
            this.cbAdvancedViewMode.AutoSize = true;
            this.cbAdvancedViewMode.Checked = true;
            this.cbAdvancedViewMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAdvancedViewMode.Location = new System.Drawing.Point(9, 83);
            this.cbAdvancedViewMode.Name = "cbAdvancedViewMode";
            this.cbAdvancedViewMode.Size = new System.Drawing.Size(131, 17);
            this.cbAdvancedViewMode.TabIndex = 10;
            this.cbAdvancedViewMode.Text = "Advanced View Mode";
            this.cbAdvancedViewMode.UseVisualStyleBackColor = true;
            this.cbAdvancedViewMode.CheckedChanged += new System.EventHandler(this.cbAdvancedViewMode_CheckedChanged);
            // 
            // llHideCheckboxes
            // 
            this.llHideCheckboxes.AutoSize = true;
            this.llHideCheckboxes.Location = new System.Drawing.Point(215, 64);
            this.llHideCheckboxes.Name = "llHideCheckboxes";
            this.llHideCheckboxes.Size = new System.Drawing.Size(88, 13);
            this.llHideCheckboxes.TabIndex = 9;
            this.llHideCheckboxes.TabStop = true;
            this.llHideCheckboxes.Text = "hide checkboxes";
            this.llHideCheckboxes.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llHideCheckboxes_LinkClicked);
            // 
            // llShowCheckboxes
            // 
            this.llShowCheckboxes.AutoSize = true;
            this.llShowCheckboxes.Location = new System.Drawing.Point(211, 44);
            this.llShowCheckboxes.Name = "llShowCheckboxes";
            this.llShowCheckboxes.Size = new System.Drawing.Size(96, 13);
            this.llShowCheckboxes.TabIndex = 8;
            this.llShowCheckboxes.TabStop = true;
            this.llShowCheckboxes.Text = "show  checkboxes";
            this.llShowCheckboxes.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowCheckboxes_LinkClicked);
            // 
            // llHideConfig
            // 
            this.llHideConfig.AutoSize = true;
            this.llHideConfig.Location = new System.Drawing.Point(270, 21);
            this.llHideConfig.Name = "llHideConfig";
            this.llHideConfig.Size = new System.Drawing.Size(32, 13);
            this.llHideConfig.TabIndex = 7;
            this.llHideConfig.TabStop = true;
            this.llHideConfig.Text = "close";
            this.llHideConfig.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llHideConfig_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Show data using";
            // 
            // typeOfControlToShowData
            // 
            this.typeOfControlToShowData.FormattingEnabled = true;
            this.typeOfControlToShowData.Items.AddRange(new object[] {
            "TreeView",
            "ListBox",
            "TextBox"});
            this.typeOfControlToShowData.Location = new System.Drawing.Point(109, 61);
            this.typeOfControlToShowData.Name = "typeOfControlToShowData";
            this.typeOfControlToShowData.Size = new System.Drawing.Size(100, 21);
            this.typeOfControlToShowData.TabIndex = 5;
            this.typeOfControlToShowData.SelectedIndexChanged += new System.EventHandler(this.typeOfControlToShowData_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Max Items to Show";
            // 
            // tbMaxItemsToShow
            // 
            this.tbMaxItemsToShow.Location = new System.Drawing.Point(109, 41);
            this.tbMaxItemsToShow.Name = "tbMaxItemsToShow";
            this.tbMaxItemsToShow.Size = new System.Drawing.Size(51, 20);
            this.tbMaxItemsToShow.TabIndex = 3;
            this.tbMaxItemsToShow.TextChanged += new System.EventHandler(this.tbMaxItemsToShow_TextChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseAll,
            this.expandAll});
            this.toolStrip2.Location = new System.Drawing.Point(3, 16);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(302, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // collapseAll
            // 
            this.collapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.collapseAll.Image = ((System.Drawing.Image)(resources.GetObject("collapseAll.Image")));
            this.collapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collapseAll.Name = "collapseAll";
            this.collapseAll.Size = new System.Drawing.Size(23, 22);
            this.collapseAll.Text = "toolStripButton1";
            this.collapseAll.Click += new System.EventHandler(this.colapseAll_Click);
            // 
            // expandAll
            // 
            this.expandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.expandAll.Image = ((System.Drawing.Image)(resources.GetObject("expandAll.Image")));
            this.expandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.expandAll.Name = "expandAll";
            this.expandAll.Size = new System.Drawing.Size(23, 22);
            this.expandAll.Text = "toolStripButton2";
            this.expandAll.Click += new System.EventHandler(this.expandAll_Click);
            // 
            // ascx_FunctionsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.configGroupBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tvTreeView);
            this.Controls.Add(this.lBoxListBox);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "ascx_FunctionsViewer";
            this.Size = new System.Drawing.Size(698, 377);
            this.Load += new System.EventHandler(this.ascx_FunctionsViewer_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.configGroupBox.ResumeLayout(false);
            this.configGroupBox.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvTreeView;
        private System.Windows.Forms.ListBox lBoxListBox;
        private System.Windows.Forms.ImageList ilClassesAndMethods;
        private System.Windows.Forms.ImageList ilSolutionsProjecstFiles;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel tbNotAllDataShown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lbViewName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel showConfig;
        private System.Windows.Forms.ToolStripComboBox namespaceDepth;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.GroupBox configGroupBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox typeOfControlToShowData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMaxItemsToShow;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton collapseAll;
        private System.Windows.Forms.ToolStripButton expandAll;
        private System.Windows.Forms.LinkLabel llHideConfig;
        private System.Windows.Forms.ToolStripLabel lbFilter;
        private System.Windows.Forms.ToolStripTextBox tbFilter;
        private System.Windows.Forms.ToolStripDropDownButton showConfigButton;
        private System.Windows.Forms.LinkLabel llShowCheckboxes;
        private System.Windows.Forms.LinkLabel llHideCheckboxes;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton checkAll;
        private System.Windows.Forms.ToolStripButton uncheckAll;
        private System.Windows.Forms.CheckBox cbAdvancedViewMode;
    }
}
