// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using FluentSharp.CoreLib.API;

namespace FluentSharp.REPL.Controls
{
    partial class ascx_XRules_Editor
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
            this.scTopLevel = new System.Windows.Forms.SplitContainer();
            this.cbShowCodeDependencies = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tbFileToOpen = new System.Windows.Forms.ToolStripTextBox();
            this.llReloadXRules = new System.Windows.Forms.ToolStripLabel();
            this.directoryWithLocalXRules = new global::FluentSharp.WinForms.Controls.DirectoryViewer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btCreateRuleFromTemplate = new System.Windows.Forms.Button();
            this.tbNewRuleName = new System.Windows.Forms.TextBox();
            this.lbCurrentXRulesTemplates = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl_WithLocalScripts = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.directoryWithXRulesDatabase = new global::FluentSharp.WinForms.Controls.DirectoryViewer();
            this.tabPage_WithSearchForScript = new System.Windows.Forms.TabPage();
            this.scRightPanel = new System.Windows.Forms.SplitContainer();
            this.llReloadSelectedSourceCodeFile = new System.Windows.Forms.LinkLabel();
            this.llRemoveSelectedSourceCodeFile = new System.Windows.Forms.LinkLabel();
            this.tcTabControlWithRulesSource = new System.Windows.Forms.TabControl();
            this.tpNoRulesLoaded = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.scTopLevel.Panel1.SuspendLayout();
            this.scTopLevel.Panel2.SuspendLayout();
            this.scTopLevel.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl_WithLocalScripts.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.scRightPanel.Panel1.SuspendLayout();
            this.scRightPanel.SuspendLayout();
            this.tcTabControlWithRulesSource.SuspendLayout();
            this.tpNoRulesLoaded.SuspendLayout();
            this.SuspendLayout();
            // 
            // scTopLevel
            // 
            this.scTopLevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scTopLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTopLevel.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scTopLevel.Location = new System.Drawing.Point(0, 0);
            this.scTopLevel.Name = "scTopLevel";
            // 
            // scTopLevel.Panel1
            // 
            this.scTopLevel.Panel1.Controls.Add(this.cbShowCodeDependencies);
            this.scTopLevel.Panel1.Controls.Add(this.splitContainer1);
            // 
            // scTopLevel.Panel2
            // 
            this.scTopLevel.Panel2.Controls.Add(this.scRightPanel);
            this.scTopLevel.Size = new System.Drawing.Size(897, 460);
            this.scTopLevel.SplitterDistance = 410;
            this.scTopLevel.TabIndex = 10;
            // 
            // cbShowCodeDependencies
            // 
            this.cbShowCodeDependencies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbShowCodeDependencies.AutoSize = true;
            this.cbShowCodeDependencies.Location = new System.Drawing.Point(3, 438);
            this.cbShowCodeDependencies.Name = "cbShowCodeDependencies";
            this.cbShowCodeDependencies.Size = new System.Drawing.Size(150, 17);
            this.cbShowCodeDependencies.TabIndex = 6;
            this.cbShowCodeDependencies.Text = "Show code dependencies";
            this.cbShowCodeDependencies.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(411, 436);
            this.splitContainer1.SplitterDistance = 281;
            this.splitContainer1.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.toolStrip2);
            this.groupBox3.Controls.Add(this.directoryWithLocalXRules);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(411, 281);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "XRules (From LOCAL XRules database)";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.tbFileToOpen,
            this.llReloadXRules});
            this.toolStrip2.Location = new System.Drawing.Point(3, 16);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(405, 25);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "toolStrip2";
            this.toolStrip2.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip2_ItemClicked);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(60, 22);
            this.toolStripLabel2.Text = "Open File:";
            // 
            // tbFileToOpen
            // 
            this.tbFileToOpen.Name = "tbFileToOpen";
            this.tbFileToOpen.Size = new System.Drawing.Size(100, 25);
            this.tbFileToOpen.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbFileToOpen_KeyUp);
            // 
            // llReloadXRules
            // 
            this.llReloadXRules.IsLink = true;
            this.llReloadXRules.Name = "llReloadXRules";
            this.llReloadXRules.Size = new System.Drawing.Size(78, 22);
            this.llReloadXRules.Text = "reload XRules";
            this.llReloadXRules.Click += new System.EventHandler(this.llReloadXRules_Click);
            // 
            // directoryWithLocalXRules
            // 
            this.directoryWithLocalXRules._FileFilter = "*.*";
            this.directoryWithLocalXRules._HandleDrop = true;
            this.directoryWithLocalXRules._HideFiles = false;
            this.directoryWithLocalXRules._ProcessDroppedObjects = true;
            this.directoryWithLocalXRules._ShowFileContentsOnTopTip = false;
            this.directoryWithLocalXRules._ShowFileSize = false;
            this.directoryWithLocalXRules._ShowLinkToUpperFolder = true;
            this.directoryWithLocalXRules._ViewMode = global::FluentSharp.WinForms.Controls.DirectoryViewer.ViewMode.Simple_With_LocationBar;
            this.directoryWithLocalXRules._WatchFolder = true;
            this.directoryWithLocalXRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryWithLocalXRules.BackColor = System.Drawing.SystemColors.Control;
            this.directoryWithLocalXRules.ForeColor = System.Drawing.Color.Black;
            this.directoryWithLocalXRules.Location = new System.Drawing.Point(3, 44);
            this.directoryWithLocalXRules.Name = "directoryWithLocalXRules";
            this.directoryWithLocalXRules.Size = new System.Drawing.Size(405, 157);
            this.directoryWithLocalXRules.TabIndex = 0;
            this.directoryWithLocalXRules._onDirectoryClick += new Callbacks.dMethod_String(this.directoryWithLocalXRules__onDirectoryClick);
            this.directoryWithLocalXRules._onDirectoryDoubleClick += new Callbacks.dMethod_String(this.directoryWithLocalXRules__onDirectoryDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btCreateRuleFromTemplate);
            this.groupBox1.Controls.Add(this.tbNewRuleName);
            this.groupBox1.Controls.Add(this.lbCurrentXRulesTemplates);
            this.groupBox1.Location = new System.Drawing.Point(1, 207);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 74);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New Rule from template";
            // 
            // btCreateRuleFromTemplate
            // 
            this.btCreateRuleFromTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCreateRuleFromTemplate.Location = new System.Drawing.Point(291, 46);
            this.btCreateRuleFromTemplate.Name = "btCreateRuleFromTemplate";
            this.btCreateRuleFromTemplate.Size = new System.Drawing.Size(116, 23);
            this.btCreateRuleFromTemplate.TabIndex = 2;
            this.btCreateRuleFromTemplate.Text = "create rule";
            this.btCreateRuleFromTemplate.UseVisualStyleBackColor = true;
            this.btCreateRuleFromTemplate.Click += new System.EventHandler(this.btCreateRuleFromTemplate_Click);
            // 
            // tbNewRuleName
            // 
            this.tbNewRuleName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNewRuleName.Location = new System.Drawing.Point(291, 13);
            this.tbNewRuleName.Name = "tbNewRuleName";
            this.tbNewRuleName.Size = new System.Drawing.Size(116, 20);
            this.tbNewRuleName.TabIndex = 1;
            // 
            // lbCurrentXRulesTemplates
            // 
            this.lbCurrentXRulesTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCurrentXRulesTemplates.FormattingEnabled = true;
            this.lbCurrentXRulesTemplates.Location = new System.Drawing.Point(6, 13);
            this.lbCurrentXRulesTemplates.Name = "lbCurrentXRulesTemplates";
            this.lbCurrentXRulesTemplates.Size = new System.Drawing.Size(278, 56);
            this.lbCurrentXRulesTemplates.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tabControl_WithLocalScripts);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(411, 151);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "XRules (From O2\'s database)";
            // 
            // tabControl_WithLocalScripts
            // 
            this.tabControl_WithLocalScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_WithLocalScripts.Controls.Add(this.tabPage1);
            this.tabControl_WithLocalScripts.Controls.Add(this.tabPage_WithSearchForScript);
            this.tabControl_WithLocalScripts.Location = new System.Drawing.Point(7, 19);
            this.tabControl_WithLocalScripts.Name = "tabControl_WithLocalScripts";
            this.tabControl_WithLocalScripts.SelectedIndex = 0;
            this.tabControl_WithLocalScripts.Size = new System.Drawing.Size(396, 126);
            this.tabControl_WithLocalScripts.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.directoryWithXRulesDatabase);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(388, 100);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "See all scripts ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // directoryWithXRulesDatabase
            // 
            this.directoryWithXRulesDatabase._FileFilter = "*.*";
            this.directoryWithXRulesDatabase._HandleDrop = false;
            this.directoryWithXRulesDatabase._HideFiles = false;
            this.directoryWithXRulesDatabase._ProcessDroppedObjects = true;
            this.directoryWithXRulesDatabase._ShowFileContentsOnTopTip = false;
            this.directoryWithXRulesDatabase._ShowFileSize = false;
            this.directoryWithXRulesDatabase._ShowLinkToUpperFolder = true;
            this.directoryWithXRulesDatabase._ViewMode = global::FluentSharp.WinForms.Controls.DirectoryViewer.ViewMode.Simple_With_LocationBar;
            this.directoryWithXRulesDatabase._WatchFolder = true;
            this.directoryWithXRulesDatabase.BackColor = System.Drawing.SystemColors.Control;
            this.directoryWithXRulesDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryWithXRulesDatabase.ForeColor = System.Drawing.Color.Black;
            this.directoryWithXRulesDatabase.Location = new System.Drawing.Point(3, 3);
            this.directoryWithXRulesDatabase.Name = "directoryWithXRulesDatabase";
            this.directoryWithXRulesDatabase.Size = new System.Drawing.Size(382, 94);
            this.directoryWithXRulesDatabase.TabIndex = 0;
            this.directoryWithXRulesDatabase._onDirectoryClick += new Callbacks.dMethod_String(this.directoryWithXRulesDatabase__onDirectoryClick);
            this.directoryWithXRulesDatabase._onDirectoryDoubleClick += new Callbacks.dMethod_String(this.directoryWithXRulesDatabase__onDirectoryDoubleClick);
            // 
            // tabPage_WithSearchForScript
            // 
            this.tabPage_WithSearchForScript.Location = new System.Drawing.Point(4, 22);
            this.tabPage_WithSearchForScript.Name = "tabPage_WithSearchForScript";
            this.tabPage_WithSearchForScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_WithSearchForScript.Size = new System.Drawing.Size(388, 100);
            this.tabPage_WithSearchForScript.TabIndex = 1;
            this.tabPage_WithSearchForScript.Text = "Search for script";
            this.tabPage_WithSearchForScript.UseVisualStyleBackColor = true;
            // 
            // scRightPanel
            // 
            this.scRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scRightPanel.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scRightPanel.Location = new System.Drawing.Point(0, 0);
            this.scRightPanel.Name = "scRightPanel";
            this.scRightPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scRightPanel.Panel1
            // 
            this.scRightPanel.Panel1.Controls.Add(this.llReloadSelectedSourceCodeFile);
            this.scRightPanel.Panel1.Controls.Add(this.llRemoveSelectedSourceCodeFile);
            this.scRightPanel.Panel1.Controls.Add(this.tcTabControlWithRulesSource);
            this.scRightPanel.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel1_Paint);
            this.scRightPanel.Panel2Collapsed = true;
            this.scRightPanel.Size = new System.Drawing.Size(479, 456);
            this.scRightPanel.SplitterDistance = 406;
            this.scRightPanel.TabIndex = 0;
            // 
            // llReloadSelectedSourceCodeFile
            // 
            this.llReloadSelectedSourceCodeFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llReloadSelectedSourceCodeFile.AutoSize = true;
            this.llReloadSelectedSourceCodeFile.Location = new System.Drawing.Point(518, 2);
            this.llReloadSelectedSourceCodeFile.Name = "llReloadSelectedSourceCodeFile";
            this.llReloadSelectedSourceCodeFile.Size = new System.Drawing.Size(95, 13);
            this.llReloadSelectedSourceCodeFile.TabIndex = 2;
            this.llReloadSelectedSourceCodeFile.TabStop = true;
            this.llReloadSelectedSourceCodeFile.Text = "reload selected file";
            this.llReloadSelectedSourceCodeFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llReloadSelectedSourceCodeFile_LinkClicked);
            // 
            // llRemoveSelectedSourceCodeFile
            // 
            this.llRemoveSelectedSourceCodeFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llRemoveSelectedSourceCodeFile.AutoSize = true;
            this.llRemoveSelectedSourceCodeFile.Location = new System.Drawing.Point(370, 4);
            this.llRemoveSelectedSourceCodeFile.Name = "llRemoveSelectedSourceCodeFile";
            this.llRemoveSelectedSourceCodeFile.Size = new System.Drawing.Size(101, 13);
            this.llRemoveSelectedSourceCodeFile.TabIndex = 1;
            this.llRemoveSelectedSourceCodeFile.TabStop = true;
            this.llRemoveSelectedSourceCodeFile.Text = "remove selected file";
            this.llRemoveSelectedSourceCodeFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llRemoveSelectedSourceCodeFile_LinkClicked);
            // 
            // tcTabControlWithRulesSource
            // 
            this.tcTabControlWithRulesSource.Controls.Add(this.tpNoRulesLoaded);
            this.tcTabControlWithRulesSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTabControlWithRulesSource.Location = new System.Drawing.Point(0, 0);
            this.tcTabControlWithRulesSource.Name = "tcTabControlWithRulesSource";
            this.tcTabControlWithRulesSource.SelectedIndex = 0;
            this.tcTabControlWithRulesSource.Size = new System.Drawing.Size(479, 456);
            this.tcTabControlWithRulesSource.TabIndex = 0;
            // 
            // tpNoRulesLoaded
            // 
            this.tpNoRulesLoaded.Controls.Add(this.label1);
            this.tpNoRulesLoaded.Location = new System.Drawing.Point(4, 22);
            this.tpNoRulesLoaded.Name = "tpNoRulesLoaded";
            this.tpNoRulesLoaded.Padding = new System.Windows.Forms.Padding(3);
            this.tpNoRulesLoaded.Size = new System.Drawing.Size(471, 430);
            this.tpNoRulesLoaded.TabIndex = 0;
            this.tpNoRulesLoaded.Text = "no rules loaded";
            this.tpNoRulesLoaded.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(185, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 96);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose rule to edit from XRules Database";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ascx_XRules_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scTopLevel);
            this.Name = "ascx_XRules_Editor";
            this.Size = new System.Drawing.Size(897, 460);
            this.Load += new System.EventHandler(this.ascx_XRules_Editor_Load);
            this.scTopLevel.Panel1.ResumeLayout(false);
            this.scTopLevel.Panel1.PerformLayout();
            this.scTopLevel.Panel2.ResumeLayout(false);
            this.scTopLevel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl_WithLocalScripts.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.scRightPanel.Panel1.ResumeLayout(false);
            this.scRightPanel.Panel1.PerformLayout();
            this.scRightPanel.ResumeLayout(false);
            this.tcTabControlWithRulesSource.ResumeLayout(false);
            this.tpNoRulesLoaded.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scTopLevel;
        private global::FluentSharp.WinForms.Controls.DirectoryViewer directoryWithXRulesDatabase;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btCreateRuleFromTemplate;
        private System.Windows.Forms.TextBox tbNewRuleName;
        private System.Windows.Forms.ListBox lbCurrentXRulesTemplates;
        private System.Windows.Forms.TabControl tcTabControlWithRulesSource;
        private System.Windows.Forms.TabPage tpNoRulesLoaded;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox3;
        private global::FluentSharp.WinForms.Controls.DirectoryViewer directoryWithLocalXRules;
        private System.Windows.Forms.LinkLabel llRemoveSelectedSourceCodeFile;
		private System.Windows.Forms.LinkLabel llReloadSelectedSourceCodeFile;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel llReloadXRules;
        private System.Windows.Forms.ToolStripTextBox tbFileToOpen;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.SplitContainer scRightPanel;        
        private System.Windows.Forms.TabControl tabControl_WithLocalScripts;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage_WithSearchForScript;
		private System.Windows.Forms.CheckBox cbShowCodeDependencies;
    }
}
