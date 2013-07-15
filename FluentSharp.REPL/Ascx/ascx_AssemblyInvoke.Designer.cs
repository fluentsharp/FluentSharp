// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using FluentSharp.SharpDevelopEditor.Resources;
namespace FluentSharp.REPL.Controls
{
    partial class ascx_AssemblyInvoke
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ascx_AssemblyInvoke));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.scTopLevel = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.tvSourceCode_CompiledAssembly = new System.Windows.Forms.TreeView();
            this.ilNamepasceClassMethods = new System.Windows.Forms.ImageList(this.components);
            this.llAbortAssemblyLoad = new System.Windows.Forms.LinkLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rbExecuteOnDoubleClick = new System.Windows.Forms.CheckBox();
            this.cbOnlyShowStaticMethods = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbAppDomainDlls = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbLastMethodExecuted = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tvExecutionThreads = new System.Windows.Forms.TreeView();
            this.llExecutionThreads_EndThread = new System.Windows.Forms.LinkLabel();
            this.llExecutionThreads_Refresh = new System.Windows.Forms.LinkLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btExecuteMethodWithoutDebugger = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btExecuteAssemblyUnderDebug = new System.Windows.Forms.ToolStripButton();
            this.btDebugMethod = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btShowAssemblyExecutionPanel = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvSourceCode_SelectedMethodParameters = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvSourceCode_InvocationResult = new System.Windows.Forms.DataGridView();
            this.tbSourceCode_InvocationResult = new System.Windows.Forms.TextBox();
            this.llShowO2Threads = new System.Windows.Forms.LinkLabel();
            this.scTopLevel.Panel1.SuspendLayout();
            this.scTopLevel.Panel2.SuspendLayout();
            this.scTopLevel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.dgvSourceCode_SelectedMethodParameters)).BeginInit();
            this.groupBox1.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.dgvSourceCode_InvocationResult)).BeginInit();
            this.SuspendLayout();
            // 
            // scTopLevel
            // 
            this.scTopLevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scTopLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTopLevel.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scTopLevel.Location = new System.Drawing.Point(0, 0);
            this.scTopLevel.Name = "scTopLevel";
            this.scTopLevel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scTopLevel.Panel1
            // 
            this.scTopLevel.Panel1.Controls.Add(this.tabControl1);
            this.scTopLevel.Panel1.Controls.Add(this.toolStrip1);
            // 
            // scTopLevel.Panel2
            // 
            this.scTopLevel.Panel2.Controls.Add(this.splitContainer1);
            this.scTopLevel.Panel2MinSize = 150;
            this.scTopLevel.Size = new System.Drawing.Size(317, 403);
            this.scTopLevel.SplitterDistance = 249;
            this.scTopLevel.TabIndex = 12;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(304, 213);
            this.tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.tvSourceCode_CompiledAssembly);
            this.tabPage1.Controls.Add(this.llAbortAssemblyLoad);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(296, 187);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Loaded Assembly details";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(177, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Chose class and method to Execute";
            // 
            // tvSourceCode_CompiledAssembly
            // 
            this.tvSourceCode_CompiledAssembly.AllowDrop = true;
            this.tvSourceCode_CompiledAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvSourceCode_CompiledAssembly.FullRowSelect = true;
            this.tvSourceCode_CompiledAssembly.HideSelection = false;
            this.tvSourceCode_CompiledAssembly.ImageIndex = 0;
            this.tvSourceCode_CompiledAssembly.ImageList = this.ilNamepasceClassMethods;
            this.tvSourceCode_CompiledAssembly.Location = new System.Drawing.Point(5, 19);
            this.tvSourceCode_CompiledAssembly.Name = "tvSourceCode_CompiledAssembly";
            this.tvSourceCode_CompiledAssembly.SelectedImageIndex = 0;
            this.tvSourceCode_CompiledAssembly.Size = new System.Drawing.Size(288, 162);
            this.tvSourceCode_CompiledAssembly.TabIndex = 9;
            this.tvSourceCode_CompiledAssembly.DoubleClick += new System.EventHandler(this.tvSourceCode_CompiledAssembly_DoubleClick);
            this.tvSourceCode_CompiledAssembly.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvSourceCode_CompiledAssembly_DragDrop);
            this.tvSourceCode_CompiledAssembly.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSourceCode_CompiledAssembly_AfterSelect);
            this.tvSourceCode_CompiledAssembly.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvSourceCode_CompiledAssembly_DragEnter);
            // 
            // ilNamepasceClassMethods
            // 
            this.ilNamepasceClassMethods.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilNamepasceClassMethods.ImageStream")));
            this.ilNamepasceClassMethods.TransparentColor = System.Drawing.Color.Transparent;
            this.ilNamepasceClassMethods.Images.SetKeyName(0, "package.ico");
            this.ilNamepasceClassMethods.Images.SetKeyName(1, "class.ico");
            this.ilNamepasceClassMethods.Images.SetKeyName(2, "method.ico");
            this.ilNamepasceClassMethods.Images.SetKeyName(3, "Findings_API.ico");
            // 
            // llAbortAssemblyLoad
            // 
            this.llAbortAssemblyLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llAbortAssemblyLoad.AutoSize = true;
            this.llAbortAssemblyLoad.Location = new System.Drawing.Point(243, 3);
            this.llAbortAssemblyLoad.Name = "llAbortAssemblyLoad";
            this.llAbortAssemblyLoad.Size = new System.Drawing.Size(50, 13);
            this.llAbortAssemblyLoad.TabIndex = 11;
            this.llAbortAssemblyLoad.TabStop = true;
            this.llAbortAssemblyLoad.Text = "stop load";
            this.llAbortAssemblyLoad.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llAbortAssemblyLoad_LinkClicked);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rbExecuteOnDoubleClick);
            this.tabPage2.Controls.Add(this.cbOnlyShowStaticMethods);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.tbAppDomainDlls);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.lbLastMethodExecuted);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(296, 187);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Config";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rbExecuteOnDoubleClick
            // 
            this.rbExecuteOnDoubleClick.AutoSize = true;
            this.rbExecuteOnDoubleClick.Location = new System.Drawing.Point(6, 80);
            this.rbExecuteOnDoubleClick.Name = "rbExecuteOnDoubleClick";
            this.rbExecuteOnDoubleClick.Size = new System.Drawing.Size(181, 17);
            this.rbExecuteOnDoubleClick.TabIndex = 21;
            this.rbExecuteOnDoubleClick.Text = "Execute method on Double Click";
            this.rbExecuteOnDoubleClick.UseVisualStyleBackColor = true;
            this.rbExecuteOnDoubleClick.CheckedChanged += new System.EventHandler(this.cbExecuteOnDoubleClick_CheckedChanged);
            // 
            // cbOnlyShowStaticMethods
            // 
            this.cbOnlyShowStaticMethods.AutoSize = true;
            this.cbOnlyShowStaticMethods.Location = new System.Drawing.Point(5, 11);
            this.cbOnlyShowStaticMethods.Name = "cbOnlyShowStaticMethods";
            this.cbOnlyShowStaticMethods.Size = new System.Drawing.Size(148, 17);
            this.cbOnlyShowStaticMethods.TabIndex = 9;
            this.cbOnlyShowStaticMethods.Text = "Only show Static methods";
            this.cbOnlyShowStaticMethods.UseVisualStyleBackColor = true;
            this.cbOnlyShowStaticMethods.CheckedChanged += new System.EventHandler(this.cbOnlyShowStaticMethods_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "AppDomain Dlls:";
            // 
            // tbAppDomainDlls
            // 
            this.tbAppDomainDlls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAppDomainDlls.Location = new System.Drawing.Point(89, 34);
            this.tbAppDomainDlls.Multiline = true;
            this.tbAppDomainDlls.Name = "tbAppDomainDlls";
            this.tbAppDomainDlls.Size = new System.Drawing.Size(204, 18);
            this.tbAppDomainDlls.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Last method executed:";
            // 
            // lbLastMethodExecuted
            // 
            this.lbLastMethodExecuted.AutoSize = true;
            this.lbLastMethodExecuted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLastMethodExecuted.Location = new System.Drawing.Point(113, 54);
            this.lbLastMethodExecuted.Name = "lbLastMethodExecuted";
            this.lbLastMethodExecuted.Size = new System.Drawing.Size(15, 13);
            this.lbLastMethodExecuted.TabIndex = 7;
            this.lbLastMethodExecuted.Text = "..";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.llShowO2Threads);
            this.tabPage3.Controls.Add(this.tvExecutionThreads);
            this.tabPage3.Controls.Add(this.llExecutionThreads_EndThread);
            this.tabPage3.Controls.Add(this.llExecutionThreads_Refresh);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(296, 187);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Executing Threads";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tvExecutionThreads
            // 
            this.tvExecutionThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvExecutionThreads.Location = new System.Drawing.Point(0, 20);
            this.tvExecutionThreads.Name = "tvExecutionThreads";
            this.tvExecutionThreads.Size = new System.Drawing.Size(293, 151);
            this.tvExecutionThreads.TabIndex = 15;
            this.tvExecutionThreads.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvExecutionThreads_BeforeExpand);
            // 
            // llExecutionThreads_EndThread
            // 
            this.llExecutionThreads_EndThread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llExecutionThreads_EndThread.AutoSize = true;
            this.llExecutionThreads_EndThread.Location = new System.Drawing.Point(238, 3);
            this.llExecutionThreads_EndThread.Name = "llExecutionThreads_EndThread";
            this.llExecutionThreads_EndThread.Size = new System.Drawing.Size(58, 13);
            this.llExecutionThreads_EndThread.TabIndex = 14;
            this.llExecutionThreads_EndThread.TabStop = true;
            this.llExecutionThreads_EndThread.Text = "end thread";
            this.llExecutionThreads_EndThread.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llExecutionThreads_EndThread_LinkClicked);
            // 
            // llExecutionThreads_Refresh
            // 
            this.llExecutionThreads_Refresh.AutoSize = true;
            this.llExecutionThreads_Refresh.Location = new System.Drawing.Point(-3, 4);
            this.llExecutionThreads_Refresh.Name = "llExecutionThreads_Refresh";
            this.llExecutionThreads_Refresh.Size = new System.Drawing.Size(39, 13);
            this.llExecutionThreads_Refresh.TabIndex = 13;
            this.llExecutionThreads_Refresh.TabStop = true;
            this.llExecutionThreads_Refresh.Text = "refresh";
            this.llExecutionThreads_Refresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llExecutionThreads_Refresh_LinkClicked);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btExecuteMethodWithoutDebugger,
            this.toolStripSeparator2,
            this.btExecuteAssemblyUnderDebug,
            this.btDebugMethod,
            this.toolStripSeparator1,
            this.btShowAssemblyExecutionPanel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(313, 25);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // btExecuteMethodWithoutDebugger
            // 
            this.btExecuteMethodWithoutDebugger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btExecuteMethodWithoutDebugger.Image = ((System.Drawing.Image)(resources.GetObject("btExecuteMethodWithoutDebugger.Image")));
            this.btExecuteMethodWithoutDebugger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btExecuteMethodWithoutDebugger.Name = "btExecuteMethodWithoutDebugger";
            this.btExecuteMethodWithoutDebugger.Size = new System.Drawing.Size(23, 22);
            this.btExecuteMethodWithoutDebugger.Text = "Execute Method (without Debugger)";
            this.btExecuteMethodWithoutDebugger.Click += new System.EventHandler(this.btExecuteMethodWithoutDebugger_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btExecuteAssemblyUnderDebug
            // 
            this.btExecuteAssemblyUnderDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btExecuteAssemblyUnderDebug.Image = ((System.Drawing.Image)(resources.GetObject("btExecuteAssemblyUnderDebug.Image")));
            this.btExecuteAssemblyUnderDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btExecuteAssemblyUnderDebug.Name = "btExecuteAssemblyUnderDebug";
            this.btExecuteAssemblyUnderDebug.Size = new System.Drawing.Size(23, 22);
            this.btExecuteAssemblyUnderDebug.Text = "Execute Assembly under Debug Mode";
            this.btExecuteAssemblyUnderDebug.Click += new System.EventHandler(this.executeMethodUnderDebug_Click);
            // 
            // btDebugMethod
            // 
            this.btDebugMethod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDebugMethod.Image = ((System.Drawing.Image)(resources.GetObject("btDebugMethod.Image")));
            this.btDebugMethod.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDebugMethod.Name = "btDebugMethod";
            this.btDebugMethod.Size = new System.Drawing.Size(23, 22);
            this.btDebugMethod.Text = "Execute Selected Method under Debug Mode";
            this.btDebugMethod.Click += new System.EventHandler(this.btDebugMethod_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btShowAssemblyExecutionPanel
            // 
            this.btShowAssemblyExecutionPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btShowAssemblyExecutionPanel.Image = ((System.Drawing.Image)(resources.GetObject("btShowAssemblyExecutionPanel.Image")));
            this.btShowAssemblyExecutionPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btShowAssemblyExecutionPanel.Name = "btShowAssemblyExecutionPanel";
            this.btShowAssemblyExecutionPanel.Size = new System.Drawing.Size(23, 22);
            this.btShowAssemblyExecutionPanel.Text = "Show Assembly Execution Panel";
            this.btShowAssemblyExecutionPanel.Click += new System.EventHandler(this.btShowAssemblyExecutionPanel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(313, 146);
            this.splitContainer1.SplitterDistance = 59;
            this.splitContainer1.TabIndex = 12;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvSourceCode_SelectedMethodParameters);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(313, 59);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Method Parameters";
            // 
            // dgvSourceCode_SelectedMethodParameters
            // 
            this.dgvSourceCode_SelectedMethodParameters.AllowUserToAddRows = false;
            this.dgvSourceCode_SelectedMethodParameters.AllowUserToDeleteRows = false;
            this.dgvSourceCode_SelectedMethodParameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSourceCode_SelectedMethodParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSourceCode_SelectedMethodParameters.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSourceCode_SelectedMethodParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSourceCode_SelectedMethodParameters.Location = new System.Drawing.Point(3, 16);
            this.dgvSourceCode_SelectedMethodParameters.Name = "dgvSourceCode_SelectedMethodParameters";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvSourceCode_SelectedMethodParameters.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvSourceCode_SelectedMethodParameters.Size = new System.Drawing.Size(307, 40);
            this.dgvSourceCode_SelectedMethodParameters.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvSourceCode_InvocationResult);
            this.groupBox1.Controls.Add(this.tbSourceCode_InvocationResult);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Return Data";
            // 
            // dgvSourceCode_InvocationResult
            // 
            this.dgvSourceCode_InvocationResult.AllowUserToAddRows = false;
            this.dgvSourceCode_InvocationResult.AllowUserToDeleteRows = false;
            this.dgvSourceCode_InvocationResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSourceCode_InvocationResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSourceCode_InvocationResult.Location = new System.Drawing.Point(3, 16);
            this.dgvSourceCode_InvocationResult.Name = "dgvSourceCode_InvocationResult";
            this.dgvSourceCode_InvocationResult.ReadOnly = true;
            this.dgvSourceCode_InvocationResult.Size = new System.Drawing.Size(307, 64);
            this.dgvSourceCode_InvocationResult.TabIndex = 10;
            // 
            // tbSourceCode_InvocationResult
            // 
            this.tbSourceCode_InvocationResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSourceCode_InvocationResult.Location = new System.Drawing.Point(3, 16);
            this.tbSourceCode_InvocationResult.Multiline = true;
            this.tbSourceCode_InvocationResult.Name = "tbSourceCode_InvocationResult";
            this.tbSourceCode_InvocationResult.Size = new System.Drawing.Size(307, 64);
            this.tbSourceCode_InvocationResult.TabIndex = 7;
            this.tbSourceCode_InvocationResult.WordWrap = false;
            // 
            // llShowO2Threads
            // 
            this.llShowO2Threads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llShowO2Threads.AutoSize = true;
            this.llShowO2Threads.Location = new System.Drawing.Point(-3, 172);
            this.llShowO2Threads.Name = "llShowO2Threads";
            this.llShowO2Threads.Size = new System.Drawing.Size(87, 13);
            this.llShowO2Threads.TabIndex = 16;
            this.llShowO2Threads.TabStop = true;
            this.llShowO2Threads.Text = "show O2 threads";
            this.llShowO2Threads.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowO2Threads_LinkClicked);
            // 
            // ascx_AssemblyInvoke
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scTopLevel);
            this.Name = "ascx_AssemblyInvoke";
            this.Size = new System.Drawing.Size(317, 403);
            this.scTopLevel.Panel1.ResumeLayout(false);
            this.scTopLevel.Panel1.PerformLayout();
            this.scTopLevel.Panel2.ResumeLayout(false);
            this.scTopLevel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            //((System.ComponentModel.ISupportInitialize)(this.dgvSourceCode_SelectedMethodParameters)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            //((System.ComponentModel.ISupportInitialize)(this.dgvSourceCode_InvocationResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scTopLevel;
        private System.Windows.Forms.Label lbLastMethodExecuted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvSourceCode_SelectedMethodParameters;
        private System.Windows.Forms.DataGridView dgvSourceCode_InvocationResult;
        private System.Windows.Forms.TextBox tbSourceCode_InvocationResult;
        private System.Windows.Forms.ImageList ilNamepasceClassMethods;
        private System.Windows.Forms.CheckBox cbOnlyShowStaticMethods;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btExecuteAssemblyUnderDebug;
        private System.Windows.Forms.ToolStripButton btExecuteMethodWithoutDebugger;
        private System.Windows.Forms.ToolStripButton btDebugMethod;
        private System.Windows.Forms.TextBox tbAppDomainDlls;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TreeView tvSourceCode_CompiledAssembly;
        private System.Windows.Forms.LinkLabel llAbortAssemblyLoad;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox rbExecuteOnDoubleClick;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btShowAssemblyExecutionPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel llExecutionThreads_EndThread;
        private System.Windows.Forms.LinkLabel llExecutionThreads_Refresh;
        private System.Windows.Forms.TreeView tvExecutionThreads;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.LinkLabel llShowO2Threads;
    }
}
