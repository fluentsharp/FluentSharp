// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.Controls
{
    partial class ascx_FindingsViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ascx_FindingsViewer));
            this.tvFindings = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tbSavedFileName = new System.Windows.Forms.TextBox();
            this.btSaveFindings = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btApplyTextChange = new System.Windows.Forms.Button();
            this.tbSelectedNodeTextValue = new System.Windows.Forms.TextBox();
            this.cbClearOnOzasmtDrop = new System.Windows.Forms.CheckBox();
            this.cbDontLoadFindingsWithNoTraces = new System.Windows.Forms.CheckBox();
            this.llDragAllFindings = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SaveFindings = new System.Windows.Forms.TabPage();
            this.cbSaveIntoO2BinaryFormat = new System.Windows.Forms.CheckBox();
            this.cbSaveFilteredFindings = new System.Windows.Forms.CheckBox();
            this.lbFileSaved = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cbOnSelectCopyTreeNodeTextToClipboard = new System.Windows.Forms.CheckBox();
            this.tbMaxRecordsToShow = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cbAllowDragOfFindings = new System.Windows.Forms.CheckBox();
            this.cbShowLineInSourceFile = new System.Windows.Forms.CheckBox();
            this.tsFindingsViewer = new System.Windows.Forms.ToolStrip();
            this.btViewSmartTraces = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbFilter1 = new System.Windows.Forms.ToolStripComboBox();
            this.tbFilter1Text = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.laFilter2Name = new System.Windows.Forms.ToolStripLabel();
            this.cbFilter2 = new System.Windows.Forms.ToolStripComboBox();
            this.llDragSelectedNodeText = new System.Windows.Forms.ToolStripLabel();
            this.laAlertOnNotAllFindingsShown = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btRefresh = new System.Windows.Forms.ToolStripButton();
            this.btClearAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btExpandAll = new System.Windows.Forms.ToolStripButton();
            this.btCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btSave = new System.Windows.Forms.ToolStripButton();
            this.btConfig = new System.Windows.Forms.ToolStripButton();
            this.btOpenFile = new System.Windows.Forms.ToolStripButton();
            this.groupBoxConfigSaveAndEdit = new System.Windows.Forms.GroupBox();
            this.laLoadingDroppedFile = new System.Windows.Forms.Label();
            this.lbNumberOfFindingsLoaded = new System.Windows.Forms.Label();
            this.laNoAssessmentLoadEnginesLoaded = new System.Windows.Forms.Label();
            this.llDragFilteredFindings = new System.Windows.Forms.LinkLabel();
            this.scrollBarHorizontalSize = new System.Windows.Forms.HScrollBar();
            this.scrollBarVerticalSize = new System.Windows.Forms.VScrollBar();
            this.ascxTraceTreeView = new ascx_TraceTreeView();
            this.tabControl1.SuspendLayout();
            this.SaveFindings.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tsFindingsViewer.SuspendLayout();
            this.groupBoxConfigSaveAndEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvFindings
            // 
            this.tvFindings.AllowDrop = true;
            this.tvFindings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvFindings.HideSelection = false;
            this.tvFindings.ImageIndex = 0;
            this.tvFindings.ImageList = this.imageList1;
            this.tvFindings.Location = new System.Drawing.Point(0, 28);
            this.tvFindings.Name = "tvFindings";
            this.tvFindings.SelectedImageIndex = 0;
            this.tvFindings.Size = new System.Drawing.Size(916, 341);
            this.tvFindings.TabIndex = 2;
            this.tvFindings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvFindings_MouseDoubleClick);
            this.tvFindings.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvFindings_BeforeExpand);
            this.tvFindings.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvFindings_DragDrop);
            this.tvFindings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFindings_AfterSelect);
            this.tvFindings.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvFindings_DragEnter);
            this.tvFindings.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvFindings_KeyUp);
            this.tvFindings.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvFindings_ItemDrag);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Explorer_Folder.ico");
            this.imageList1.Images.SetKeyName(1, "SmartTrace.ico");
            this.imageList1.Images.SetKeyName(2, "Findings_TypeII.ico");
            // 
            // tbSavedFileName
            // 
            this.tbSavedFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSavedFileName.Location = new System.Drawing.Point(61, 14);
            this.tbSavedFileName.Name = "tbSavedFileName";
            this.tbSavedFileName.Size = new System.Drawing.Size(551, 20);
            this.tbSavedFileName.TabIndex = 3;
            this.tbSavedFileName.TextChanged += new System.EventHandler(this.tbSavedFileName_TextChanged);
            // 
            // btSaveFindings
            // 
            this.btSaveFindings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveFindings.Location = new System.Drawing.Point(462, 38);
            this.btSaveFindings.Name = "btSaveFindings";
            this.btSaveFindings.Size = new System.Drawing.Size(87, 23);
            this.btSaveFindings.TabIndex = 4;
            this.btSaveFindings.Text = "Save";
            this.btSaveFindings.UseVisualStyleBackColor = true;
            this.btSaveFindings.Click += new System.EventHandler(this.btSaveFindings_Click);
            this.btSaveFindings.Leave += new System.EventHandler(this.btSaveFindings_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Filename:";
            // 
            // btApplyTextChange
            // 
            this.btApplyTextChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btApplyTextChange.Location = new System.Drawing.Point(406, 33);
            this.btApplyTextChange.Name = "btApplyTextChange";
            this.btApplyTextChange.Size = new System.Drawing.Size(60, 23);
            this.btApplyTextChange.TabIndex = 6;
            this.btApplyTextChange.Text = "Apply";
            this.btApplyTextChange.UseVisualStyleBackColor = true;
            this.btApplyTextChange.Click += new System.EventHandler(this.btApplyTextChange_Click);
            // 
            // tbSelectedNodeTextValue
            // 
            this.tbSelectedNodeTextValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSelectedNodeTextValue.Location = new System.Drawing.Point(6, 33);
            this.tbSelectedNodeTextValue.Name = "tbSelectedNodeTextValue";
            this.tbSelectedNodeTextValue.Size = new System.Drawing.Size(394, 20);
            this.tbSelectedNodeTextValue.TabIndex = 3;
            this.tbSelectedNodeTextValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSelectedValue_KeyUp);
            // 
            // cbClearOnOzasmtDrop
            // 
            this.cbClearOnOzasmtDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbClearOnOzasmtDrop.AutoSize = true;
            this.cbClearOnOzasmtDrop.Checked = true;
            this.cbClearOnOzasmtDrop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbClearOnOzasmtDrop.Location = new System.Drawing.Point(3, 9);
            this.cbClearOnOzasmtDrop.Name = "cbClearOnOzasmtDrop";
            this.cbClearOnOzasmtDrop.Size = new System.Drawing.Size(190, 17);
            this.cbClearOnOzasmtDrop.TabIndex = 10;
            this.cbClearOnOzasmtDrop.Text = "Clear Findings on Ozasmt File Drop";
            this.cbClearOnOzasmtDrop.UseVisualStyleBackColor = true;
            // 
            // cbDontLoadFindingsWithNoTraces
            // 
            this.cbDontLoadFindingsWithNoTraces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbDontLoadFindingsWithNoTraces.AutoSize = true;
            this.cbDontLoadFindingsWithNoTraces.Location = new System.Drawing.Point(3, 26);
            this.cbDontLoadFindingsWithNoTraces.Name = "cbDontLoadFindingsWithNoTraces";
            this.cbDontLoadFindingsWithNoTraces.Size = new System.Drawing.Size(196, 17);
            this.cbDontLoadFindingsWithNoTraces.TabIndex = 11;
            this.cbDontLoadFindingsWithNoTraces.Text = "Dont Load Findings With No Traces";
            this.cbDontLoadFindingsWithNoTraces.UseVisualStyleBackColor = true;
            // 
            // llDragAllFindings
            // 
            this.llDragAllFindings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.llDragAllFindings.AutoSize = true;
            this.llDragAllFindings.Location = new System.Drawing.Point(822, 372);
            this.llDragAllFindings.Name = "llDragAllFindings";
            this.llDragAllFindings.Size = new System.Drawing.Size(94, 13);
            this.llDragAllFindings.TabIndex = 14;
            this.llDragAllFindings.TabStop = true;
            this.llDragAllFindings.Text = "Drag ALL Findings";
            this.llDragAllFindings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.llDragAllFindings_MouseDown);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SaveFindings);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(632, 102);
            this.tabControl1.TabIndex = 15;
            // 
            // SaveFindings
            // 
            this.SaveFindings.Controls.Add(this.cbSaveIntoO2BinaryFormat);
            this.SaveFindings.Controls.Add(this.btSaveFindings);
            this.SaveFindings.Controls.Add(this.cbSaveFilteredFindings);
            this.SaveFindings.Controls.Add(this.lbFileSaved);
            this.SaveFindings.Controls.Add(this.label2);
            this.SaveFindings.Controls.Add(this.tbSavedFileName);
            this.SaveFindings.Location = new System.Drawing.Point(4, 22);
            this.SaveFindings.Name = "SaveFindings";
            this.SaveFindings.Size = new System.Drawing.Size(624, 76);
            this.SaveFindings.TabIndex = 2;
            this.SaveFindings.Text = "Save Findings";
            this.SaveFindings.UseVisualStyleBackColor = true;
            // 
            // cbSaveIntoO2BinaryFormat
            // 
            this.cbSaveIntoO2BinaryFormat.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbSaveIntoO2BinaryFormat.Location = new System.Drawing.Point(193, 35);
            this.cbSaveIntoO2BinaryFormat.Name = "cbSaveIntoO2BinaryFormat";
            this.cbSaveIntoO2BinaryFormat.Size = new System.Drawing.Size(116, 38);
            this.cbSaveIntoO2BinaryFormat.TabIndex = 8;
            this.cbSaveIntoO2BinaryFormat.Text = "Save into O2 Binary File Format";
            this.cbSaveIntoO2BinaryFormat.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbSaveIntoO2BinaryFormat.UseVisualStyleBackColor = true;
            // 
            // cbSaveFilteredFindings
            // 
            this.cbSaveFilteredFindings.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbSaveFilteredFindings.Location = new System.Drawing.Point(62, 35);
            this.cbSaveFilteredFindings.Name = "cbSaveFilteredFindings";
            this.cbSaveFilteredFindings.Size = new System.Drawing.Size(148, 38);
            this.cbSaveFilteredFindings.TabIndex = 7;
            this.cbSaveFilteredFindings.Text = "Save Filtered Findings (uncheck to Save all findings)";
            this.cbSaveFilteredFindings.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbSaveFilteredFindings.UseVisualStyleBackColor = true;
            // 
            // lbFileSaved
            // 
            this.lbFileSaved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFileSaved.AutoSize = true;
            this.lbFileSaved.Location = new System.Drawing.Point(555, 43);
            this.lbFileSaved.Name = "lbFileSaved";
            this.lbFileSaved.Size = new System.Drawing.Size(57, 13);
            this.lbFileSaved.TabIndex = 6;
            this.lbFileSaved.Text = "File Saved";
            this.lbFileSaved.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btApplyTextChange);
            this.tabPage2.Controls.Add(this.tbSelectedNodeTextValue);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(509, 76);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Edit Node Text";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Node text:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cbOnSelectCopyTreeNodeTextToClipboard);
            this.tabPage3.Controls.Add(this.tbMaxRecordsToShow);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.cbAllowDragOfFindings);
            this.tabPage3.Controls.Add(this.cbDontLoadFindingsWithNoTraces);
            this.tabPage3.Controls.Add(this.cbClearOnOzasmtDrop);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(509, 76);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Options";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cbOnSelectCopyTreeNodeTextToClipboard
            // 
            this.cbOnSelectCopyTreeNodeTextToClipboard.AutoSize = true;
            this.cbOnSelectCopyTreeNodeTextToClipboard.Location = new System.Drawing.Point(212, 43);
            this.cbOnSelectCopyTreeNodeTextToClipboard.Name = "cbOnSelectCopyTreeNodeTextToClipboard";
            this.cbOnSelectCopyTreeNodeTextToClipboard.Size = new System.Drawing.Size(235, 17);
            this.cbOnSelectCopyTreeNodeTextToClipboard.TabIndex = 51;
            this.cbOnSelectCopyTreeNodeTextToClipboard.Text = "On Select, copy TreeNode Text to clipboard";
            this.cbOnSelectCopyTreeNodeTextToClipboard.UseVisualStyleBackColor = true;
            // 
            // tbMaxRecordsToShow
            // 
            this.tbMaxRecordsToShow.Location = new System.Drawing.Point(307, 12);
            this.tbMaxRecordsToShow.Name = "tbMaxRecordsToShow";
            this.tbMaxRecordsToShow.Size = new System.Drawing.Size(42, 20);
            this.tbMaxRecordsToShow.TabIndex = 50;
            this.tbMaxRecordsToShow.TextChanged += new System.EventHandler(this.tbMaxRecordsToShow_TextChanged);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(209, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(97, 31);
            this.label14.TabIndex = 49;
            this.label14.Text = "Maximum number of Findings to load ";
            // 
            // cbAllowDragOfFindings
            // 
            this.cbAllowDragOfFindings.AutoSize = true;
            this.cbAllowDragOfFindings.Checked = true;
            this.cbAllowDragOfFindings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllowDragOfFindings.Location = new System.Drawing.Point(3, 43);
            this.cbAllowDragOfFindings.Name = "cbAllowDragOfFindings";
            this.cbAllowDragOfFindings.Size = new System.Drawing.Size(131, 17);
            this.cbAllowDragOfFindings.TabIndex = 48;
            this.cbAllowDragOfFindings.Text = "Allow Drag of Findings";
            this.cbAllowDragOfFindings.UseVisualStyleBackColor = true;
            // 
            // cbShowLineInSourceFile
            // 
            this.cbShowLineInSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbShowLineInSourceFile.AutoSize = true;
            this.cbShowLineInSourceFile.Location = new System.Drawing.Point(3, 371);
            this.cbShowLineInSourceFile.Name = "cbShowLineInSourceFile";
            this.cbShowLineInSourceFile.Size = new System.Drawing.Size(190, 17);
            this.cbShowLineInSourceFile.TabIndex = 51;
            this.cbShowLineInSourceFile.Text = "Show Findings in Source Code File";
            this.cbShowLineInSourceFile.UseVisualStyleBackColor = true;
            // 
            // tsFindingsViewer
            // 
            this.tsFindingsViewer.AllowDrop = true;
            this.tsFindingsViewer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btViewSmartTraces,
            this.toolStripSeparator6,
            this.toolStripLabel1,
            this.cbFilter1,
            this.tbFilter1Text,
            this.toolStripSeparator5,
            this.laFilter2Name,
            this.cbFilter2,
            this.llDragSelectedNodeText,
            this.laAlertOnNotAllFindingsShown,
            this.toolStripSeparator3,
            this.btRefresh,
            this.btClearAll,
            this.toolStripSeparator2,
            this.btExpandAll,
            this.btCollapseAll,
            this.toolStripSeparator4,
            this.btSave,
            this.btConfig,
            this.btOpenFile});
            this.tsFindingsViewer.Location = new System.Drawing.Point(0, 0);
            this.tsFindingsViewer.Name = "tsFindingsViewer";
            this.tsFindingsViewer.Size = new System.Drawing.Size(919, 25);
            this.tsFindingsViewer.TabIndex = 18;
            this.tsFindingsViewer.Text = "toolStrip1";
            this.tsFindingsViewer.DragEnter += new System.Windows.Forms.DragEventHandler(this.tsFindingsViewer_DragEnter);
            this.tsFindingsViewer.DragDrop += new System.Windows.Forms.DragEventHandler(this.tsFindingsViewer_DragDrop);
            // 
            // btViewSmartTraces
            // 
            this.btViewSmartTraces.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btViewSmartTraces.Image = ((System.Drawing.Image)(resources.GetObject("btViewSmartTraces.Image")));
            this.btViewSmartTraces.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btViewSmartTraces.Name = "btViewSmartTraces";
            this.btViewSmartTraces.Size = new System.Drawing.Size(23, 22);
            this.btViewSmartTraces.Text = "View Smart Traces";
            this.btViewSmartTraces.Click += new System.EventHandler(this.btViewSmartTraces_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(46, 22);
            this.toolStripLabel1.Text = "filter #1";
            // 
            // cbFilter1
            // 
            this.cbFilter1.DropDownHeight = 460;
            this.cbFilter1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter1.DropDownWidth = 100;
            this.cbFilter1.IntegralHeight = false;
            this.cbFilter1.Name = "cbFilter1";
            this.cbFilter1.Size = new System.Drawing.Size(80, 25);
            this.cbFilter1.Sorted = true;
            this.cbFilter1.SelectedIndexChanged += new System.EventHandler(this.cbFilter1_SelectedIndexChanged);
            this.cbFilter1.Click += new System.EventHandler(this.cbFilter1_Click);
            // 
            // tbFilter1Text
            // 
            this.tbFilter1Text.Name = "tbFilter1Text";
            this.tbFilter1Text.Size = new System.Drawing.Size(50, 25);
            this.tbFilter1Text.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbFilter1Text_KeyPress);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // laFilter2Name
            // 
            this.laFilter2Name.Name = "laFilter2Name";
            this.laFilter2Name.Size = new System.Drawing.Size(50, 22);
            this.laFilter2Name.Text = "filter #2:";
            this.laFilter2Name.Click += new System.EventHandler(this.laFilter2Name_Click);
            // 
            // cbFilter2
            // 
            this.cbFilter2.DropDownHeight = 460;
            this.cbFilter2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter2.DropDownWidth = 100;
            this.cbFilter2.IntegralHeight = false;
            this.cbFilter2.Name = "cbFilter2";
            this.cbFilter2.Size = new System.Drawing.Size(80, 25);
            this.cbFilter2.Sorted = true;
            this.cbFilter2.SelectedIndexChanged += new System.EventHandler(this.cbFilter2_SelectedIndexChanged);
            // 
            // llDragSelectedNodeText
            // 
            this.llDragSelectedNodeText.IsLink = true;
            this.llDragSelectedNodeText.Name = "llDragSelectedNodeText";
            this.llDragSelectedNodeText.Size = new System.Drawing.Size(122, 22);
            this.llDragSelectedNodeText.Text = "drag selected node text";
            this.llDragSelectedNodeText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.llDragSelectedNodeText_MouseDown);
            // 
            // laAlertOnNotAllFindingsShown
            // 
            this.laAlertOnNotAllFindingsShown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.laAlertOnNotAllFindingsShown.ForeColor = System.Drawing.Color.Red;
            this.laAlertOnNotAllFindingsShown.Name = "laAlertOnNotAllFindingsShown";
            this.laAlertOnNotAllFindingsShown.Size = new System.Drawing.Size(160, 22);
            this.laAlertOnNotAllFindingsShown.Text = "Note: Not all findings shown";
            this.laAlertOnNotAllFindingsShown.Visible = false;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btRefresh
            // 
            this.btRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btRefresh.Image")));
            this.btRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(23, 22);
            this.btRefresh.Text = "Refresh";
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // btClearAll
            // 
            this.btClearAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btClearAll.Image = ((System.Drawing.Image)(resources.GetObject("btClearAll.Image")));
            this.btClearAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btClearAll.Name = "btClearAll";
            this.btClearAll.Size = new System.Drawing.Size(23, 22);
            this.btClearAll.Text = "Clear (remove all loaded findings)";
            this.btClearAll.Click += new System.EventHandler(this.btClearAll_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btExpandAll
            // 
            this.btExpandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("btExpandAll.Image")));
            this.btExpandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btExpandAll.Name = "btExpandAll";
            this.btExpandAll.Size = new System.Drawing.Size(23, 22);
            this.btExpandAll.Text = "Expand All";
            this.btExpandAll.Click += new System.EventHandler(this.btExpandAll_Click);
            // 
            // btCollapseAll
            // 
            this.btCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("btCollapseAll.Image")));
            this.btCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btCollapseAll.Name = "btCollapseAll";
            this.btCollapseAll.Size = new System.Drawing.Size(23, 22);
            this.btCollapseAll.Text = "Collapse All";
            this.btCollapseAll.Click += new System.EventHandler(this.btCollapseAll_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btSave
            // 
            this.btSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSave.Image = ((System.Drawing.Image)(resources.GetObject("btSave.Image")));
            this.btSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(23, 22);
            this.btSave.Text = "Save current loaded files";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btConfig
            // 
            this.btConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btConfig.Image = ((System.Drawing.Image)(resources.GetObject("btConfig.Image")));
            this.btConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btConfig.Name = "btConfig";
            this.btConfig.Size = new System.Drawing.Size(23, 22);
            this.btConfig.Text = "Config, Save && Edit";
            this.btConfig.Click += new System.EventHandler(this.btConfig_Click);
            // 
            // btOpenFile
            // 
            this.btOpenFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("btOpenFile.Image")));
            this.btOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(23, 22);
            this.btOpenFile.Text = "Open File";
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // groupBoxConfigSaveAndEdit
            // 
            this.groupBoxConfigSaveAndEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConfigSaveAndEdit.Controls.Add(this.tabControl1);
            this.groupBoxConfigSaveAndEdit.Location = new System.Drawing.Point(278, 28);
            this.groupBoxConfigSaveAndEdit.Name = "groupBoxConfigSaveAndEdit";
            this.groupBoxConfigSaveAndEdit.Size = new System.Drawing.Size(638, 121);
            this.groupBoxConfigSaveAndEdit.TabIndex = 16;
            this.groupBoxConfigSaveAndEdit.TabStop = false;
            this.groupBoxConfigSaveAndEdit.Text = "Config, Save && Edit";
            this.groupBoxConfigSaveAndEdit.Visible = false;
            // 
            // laLoadingDroppedFile
            // 
            this.laLoadingDroppedFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.laLoadingDroppedFile.AutoSize = true;
            this.laLoadingDroppedFile.BackColor = System.Drawing.SystemColors.Window;
            this.laLoadingDroppedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.laLoadingDroppedFile.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.laLoadingDroppedFile.Location = new System.Drawing.Point(338, 183);
            this.laLoadingDroppedFile.Name = "laLoadingDroppedFile";
            this.laLoadingDroppedFile.Size = new System.Drawing.Size(296, 29);
            this.laLoadingDroppedFile.TabIndex = 21;
            this.laLoadingDroppedFile.Text = "Loading Dropped File ...";
            this.laLoadingDroppedFile.Visible = false;
            // 
            // lbNumberOfFindingsLoaded
            // 
            this.lbNumberOfFindingsLoaded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbNumberOfFindingsLoaded.AutoSize = true;
            this.lbNumberOfFindingsLoaded.Location = new System.Drawing.Point(211, 372);
            this.lbNumberOfFindingsLoaded.Name = "lbNumberOfFindingsLoaded";
            this.lbNumberOfFindingsLoaded.Size = new System.Drawing.Size(16, 13);
            this.lbNumberOfFindingsLoaded.TabIndex = 22;
            this.lbNumberOfFindingsLoaded.Text = "...";
            // 
            // laNoAssessmentLoadEnginesLoaded
            // 
            this.laNoAssessmentLoadEnginesLoaded.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.laNoAssessmentLoadEnginesLoaded.BackColor = System.Drawing.SystemColors.Window;
            this.laNoAssessmentLoadEnginesLoaded.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.laNoAssessmentLoadEnginesLoaded.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.laNoAssessmentLoadEnginesLoaded.Location = new System.Drawing.Point(338, 135);
            this.laNoAssessmentLoadEnginesLoaded.Name = "laNoAssessmentLoadEnginesLoaded";
            this.laNoAssessmentLoadEnginesLoaded.Size = new System.Drawing.Size(249, 106);
            this.laNoAssessmentLoadEnginesLoaded.TabIndex = 23;
            this.laNoAssessmentLoadEnginesLoaded.Text = "There are no O2 Assessment Load Engines configured!";
            this.laNoAssessmentLoadEnginesLoaded.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llDragFilteredFindings
            // 
            this.llDragFilteredFindings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.llDragFilteredFindings.AutoSize = true;
            this.llDragFilteredFindings.Location = new System.Drawing.Point(707, 372);
            this.llDragFilteredFindings.Name = "llDragFilteredFindings";
            this.llDragFilteredFindings.Size = new System.Drawing.Size(109, 13);
            this.llDragFilteredFindings.TabIndex = 24;
            this.llDragFilteredFindings.TabStop = true;
            this.llDragFilteredFindings.Text = "Drag Filtered Findings";
            this.llDragFilteredFindings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.llDragFilteredFindings_MouseDown);
            // 
            // scrollBarHorizontalSize
            // 
            this.scrollBarHorizontalSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollBarHorizontalSize.Location = new System.Drawing.Point(845, 355);
            this.scrollBarHorizontalSize.Name = "scrollBarHorizontalSize";
            this.scrollBarHorizontalSize.Size = new System.Drawing.Size(38, 10);
            this.scrollBarHorizontalSize.SmallChange = 10;
            this.scrollBarHorizontalSize.TabIndex = 53;
            this.scrollBarHorizontalSize.Visible = false;
            this.scrollBarHorizontalSize.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollBarHorizontalSize_Scroll);
            // 
            // scrollBarVerticalSize
            // 
            this.scrollBarVerticalSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollBarVerticalSize.Location = new System.Drawing.Point(884, 327);
            this.scrollBarVerticalSize.Name = "scrollBarVerticalSize";
            this.scrollBarVerticalSize.Size = new System.Drawing.Size(10, 38);
            this.scrollBarVerticalSize.SmallChange = 10;
            this.scrollBarVerticalSize.TabIndex = 52;
            this.scrollBarVerticalSize.Visible = false;
            this.scrollBarVerticalSize.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollBarVerticalSize_Scroll);
            // 
            // ascxTraceTreeView
            // 
            this.ascxTraceTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ascxTraceTreeView.bMoveTraces = true;
            this.ascxTraceTreeView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ascxTraceTreeView.Location = new System.Drawing.Point(505, 151);
            this.ascxTraceTreeView.Name = "ascxTraceTreeView";
            this.ascxTraceTreeView.o2Finding = null;
            this.ascxTraceTreeView.o2Trace = null;
            this.ascxTraceTreeView.selectedNode = null;
            this.ascxTraceTreeView.selectedNodeTag = null;
            this.ascxTraceTreeView.Size = new System.Drawing.Size(392, 218);
            this.ascxTraceTreeView.TabIndex = 20;
            this.ascxTraceTreeView.Visible = false;
            this.ascxTraceTreeView.Load += new System.EventHandler(this.ascxTraceTreeView_Load);
            this.ascxTraceTreeView.SizeChanged += new System.EventHandler(this.ascxTraceTreeView_SizeChanged);
            this.ascxTraceTreeView._onTraceSelected += new Action<IO2Trace>(this.ascxTraceTreeView__onTraceSelected);
            // 
            // ascx_FindingsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scrollBarHorizontalSize);
            this.Controls.Add(this.scrollBarVerticalSize);
            this.Controls.Add(this.cbShowLineInSourceFile);
            this.Controls.Add(this.groupBoxConfigSaveAndEdit);
            this.Controls.Add(this.llDragFilteredFindings);
            this.Controls.Add(this.laNoAssessmentLoadEnginesLoaded);
            this.Controls.Add(this.lbNumberOfFindingsLoaded);
            this.Controls.Add(this.laLoadingDroppedFile);
            this.Controls.Add(this.ascxTraceTreeView);
            this.Controls.Add(this.llDragAllFindings);
            this.Controls.Add(this.tsFindingsViewer);
            this.Controls.Add(this.tvFindings);
            this.Name = "ascx_FindingsViewer";
            this.Size = new System.Drawing.Size(919, 388);
            this.Load += new System.EventHandler(this.ascx_FindingsViewer_Load);
            this.tabControl1.ResumeLayout(false);
            this.SaveFindings.ResumeLayout(false);
            this.SaveFindings.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tsFindingsViewer.ResumeLayout(false);
            this.tsFindingsViewer.PerformLayout();
            this.groupBoxConfigSaveAndEdit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvFindings;
        private System.Windows.Forms.TextBox tbSavedFileName;
        private System.Windows.Forms.Button btSaveFindings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox tbSelectedNodeTextValue;
        private System.Windows.Forms.Button btApplyTextChange;
        private System.Windows.Forms.CheckBox cbClearOnOzasmtDrop;
        private System.Windows.Forms.CheckBox cbDontLoadFindingsWithNoTraces;
        private System.Windows.Forms.LinkLabel llDragAllFindings;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage SaveFindings;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbFileSaved;
        private System.Windows.Forms.CheckBox cbAllowDragOfFindings;
        private System.Windows.Forms.TextBox tbMaxRecordsToShow;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ToolStrip tsFindingsViewer;
        private System.Windows.Forms.ToolStripButton btConfig;
        private System.Windows.Forms.ToolStripComboBox cbFilter1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel laFilter2Name;
        private System.Windows.Forms.ToolStripComboBox cbFilter2;
        private System.Windows.Forms.GroupBox groupBoxConfigSaveAndEdit;
        private System.Windows.Forms.ToolStripButton btRefresh;
        private System.Windows.Forms.ToolStripButton btClearAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btViewSmartTraces;
        private ascx_TraceTreeView ascxTraceTreeView;
        private System.Windows.Forms.ToolStripButton btExpandAll;
        private System.Windows.Forms.ToolStripButton btCollapseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripTextBox tbFilter1Text;
        private System.Windows.Forms.CheckBox cbSaveFilteredFindings;
        private System.Windows.Forms.Label laLoadingDroppedFile;
        private System.Windows.Forms.Label lbNumberOfFindingsLoaded;
        private System.Windows.Forms.ToolStripLabel laAlertOnNotAllFindingsShown;
        private System.Windows.Forms.Label laNoAssessmentLoadEnginesLoaded;
        private System.Windows.Forms.LinkLabel llDragFilteredFindings;
        private System.Windows.Forms.CheckBox cbShowLineInSourceFile;
        private System.Windows.Forms.ToolStripLabel llDragSelectedNodeText;
        private System.Windows.Forms.HScrollBar scrollBarHorizontalSize;
        private System.Windows.Forms.VScrollBar scrollBarVerticalSize;
        private System.Windows.Forms.ToolStripButton btSave;
        private System.Windows.Forms.CheckBox cbOnSelectCopyTreeNodeTextToClipboard;
        private System.Windows.Forms.ToolStripButton btOpenFile;
        private System.Windows.Forms.CheckBox cbSaveIntoO2BinaryFormat;
    }
}
