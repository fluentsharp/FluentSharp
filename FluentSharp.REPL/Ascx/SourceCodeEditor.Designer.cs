// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using FluentSharp.SharpDevelopEditor.Resources;
namespace FluentSharp.REPL.Controls
{
    partial class SourceCodeEditor
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
            this.menuStripForSourceEdition = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeSelectedMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoCompileEvery10SecondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableCodeCompleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCompileCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLocalFileMappingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addBreakpointOnCurrentLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cboxVRuler = new System.Windows.Forms.CheckBox();
            this.cboxInvalidLines = new System.Windows.Forms.CheckBox();
            this.cboxHRuler = new System.Windows.Forms.CheckBox();
            this.cboxEOLMarkers = new System.Windows.Forms.CheckBox();
            this.cboxSpaces = new System.Windows.Forms.CheckBox();
            this.cboxTabs = new System.Windows.Forms.CheckBox();
            this.cboxLineNumbers = new System.Windows.Forms.CheckBox();
            this.lbSource_CodeFileSaved = new System.Windows.Forms.Label();
            this.lbSourceCode_UnsavedChanges = new System.Windows.Forms.Label();
            this.btSave = new System.Windows.Forms.Button();
            this.tbSourceCode_FileLoaded = new System.Windows.Forms.TextBox();
            this.tbTextSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbSearch_textNotFound = new System.Windows.Forms.Label();
            this.tbSourceCode_DirectoryOfFileLoaded = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbFileLoaded = new System.Windows.Forms.Label();
            this.toolStripWithSourceCodeActions = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tstbTextSearch = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tbShowO2ObjectModel = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.btSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btSaveFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.lbExecuteOnEngine = new System.Windows.Forms.ToolStripLabel();
            this.cbExternalEngineToUse = new System.Windows.Forms.ToolStripComboBox();
            this.btExecuteOnExternalEngine = new System.Windows.Forms.ToolStripButton();
            this.btShowHidePythonLogExecutionOutputData = new System.Windows.Forms.ToolStripButton();
            this.lbCompileCode = new System.Windows.Forms.ToolStripLabel();
            this.btCompileCode = new System.Windows.Forms.ToolStripButton();
            this.tsCompileStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.lbExecuteCode = new System.Windows.Forms.ToolStripLabel();
            this.cboxCompliledSourceCodeMethods = new System.Windows.Forms.ToolStripComboBox();
            this.btExecuteSelectedMethod = new System.Windows.Forms.ToolStripButton();
            this.btDebugMethod = new System.Windows.Forms.ToolStripButton();
            this.lbCompilationErrors = new System.Windows.Forms.ToolStripLabel();
            this.btShowHideCompilationErrors = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btDragAssemblyCreated = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btSeachAndViewAst = new System.Windows.Forms.ToolStripButton();
            this.btSelectedLineHistory = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btShowLogs = new System.Windows.Forms.ToolStripButton();
            this.lbSampleScripts = new System.Windows.Forms.ToolStripLabel();
            this.cBoxSampleScripts = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btOpenFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.groupBoxWithFileAndSaveSettings = new System.Windows.Forms.GroupBox();
            this.llPutFilePathInClipboard = new System.Windows.Forms.LinkLabel();
            this.cbAutoTryToFixSourceCodeFileReferences = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.llReload = new System.Windows.Forms.LinkLabel();
            this.llCurrenlyLoadedObjectModel = new System.Windows.Forms.LinkLabel();
            this.tbMaxLoadSize = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbPartialFileView = new System.Windows.Forms.ListBox();
            this.tbExecutionHistoryOrLog = new System.Windows.Forms.TextBox();
            this.tvCompilationErrors = new System.Windows.Forms.TreeView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tcSourceInfo = new System.Windows.Forms.TabControl();
            this.scCodeAndAst = new System.Windows.Forms.SplitContainer();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tecSourceCode = new ICSharpCode.TextEditor.TextEditorControl();
            this.menuStripForSourceEdition.SuspendLayout();
            this.toolStripWithSourceCodeActions.SuspendLayout();
            this.groupBoxWithFileAndSaveSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tcSourceInfo.SuspendLayout();
            this.scCodeAndAst.Panel1.SuspendLayout();
            this.scCodeAndAst.Panel2.SuspendLayout();
            this.scCodeAndAst.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripForSourceEdition
            // 
            this.menuStripForSourceEdition.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileToolStripMenuItem,
            this.executeSelectedMethodToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openFileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.compileSettingsToolStripMenuItem,
            this.enableCodeCompleteToolStripMenuItem,
            this.clearCompileCacheToolStripMenuItem,
            this.clearLocalFileMappingsToolStripMenuItem});
            this.menuStripForSourceEdition.Name = "menuStripForSourceEdition";
            this.menuStripForSourceEdition.Size = new System.Drawing.Size(210, 246);
            this.menuStripForSourceEdition.Opening += new System.ComponentModel.CancelEventHandler(this.menuStripForSourceEdition_Opening);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.compileToolStripMenuItem.Text = "Compile";
            this.compileToolStripMenuItem.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
            // 
            // executeSelectedMethodToolStripMenuItem
            // 
            this.executeSelectedMethodToolStripMenuItem.Name = "executeSelectedMethodToolStripMenuItem";
            this.executeSelectedMethodToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.executeSelectedMethodToolStripMenuItem.Text = "Execute Selected Method";
            this.executeSelectedMethodToolStripMenuItem.Click += new System.EventHandler(this.executeSelectedMethodToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem
            // 
            this.enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem.Name = "enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem";
            this.enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem.Size = new System.Drawing.Size(405, 22);
            this.enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem.Text = "Enable or Disable Auto Backup on Compile Sucess (for CSharp)";
            this.enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem.Click += new System.EventHandler(this.enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem_Click);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoCompileEvery10SecondsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // autoCompileEvery10SecondsToolStripMenuItem
            // 
            this.autoCompileEvery10SecondsToolStripMenuItem.CheckOnClick = true;
            this.autoCompileEvery10SecondsToolStripMenuItem.Name = "autoCompileEvery10SecondsToolStripMenuItem";
            this.autoCompileEvery10SecondsToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.autoCompileEvery10SecondsToolStripMenuItem.Text = "Auto Compile every 10 seconds";
            this.autoCompileEvery10SecondsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.autoCompileEvery10SecondsToolStripMenuItem_CheckedChanged);
            // 
            // compileSettingsToolStripMenuItem
            // 
            this.compileSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem});
            this.compileSettingsToolStripMenuItem.Name = "compileSettingsToolStripMenuItem";
            this.compileSettingsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.compileSettingsToolStripMenuItem.Text = "Compile Settings";
            // 
            // listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem
            // 
            this.listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem.Name = "listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem";
            this.listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem.Size = new System.Drawing.Size(442, 22);
            this.listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem.Text = "List (in LogView) the current assembly references automatically added";
            this.listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem.Click += new System.EventHandler(this.listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem_Click);
            // 
            // enableCodeCompleteToolStripMenuItem
            // 
            this.enableCodeCompleteToolStripMenuItem.Name = "enableCodeCompleteToolStripMenuItem";
            this.enableCodeCompleteToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.enableCodeCompleteToolStripMenuItem.Text = "Enable Code Complete";
            this.enableCodeCompleteToolStripMenuItem.Click += new System.EventHandler(this.enableCodeCompleteToolStripMenuItem_Click);
            // 
            // clearCompileCacheToolStripMenuItem
            // 
            this.clearCompileCacheToolStripMenuItem.Name = "clearCompileCacheToolStripMenuItem";
            this.clearCompileCacheToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.clearCompileCacheToolStripMenuItem.Text = "Clear Compile Cache";
            this.clearCompileCacheToolStripMenuItem.Click += new System.EventHandler(this.clearCompileCacheToolStripMenuItem_Click);
            // 
            // clearLocalFileMappingsToolStripMenuItem
            // 
            this.clearLocalFileMappingsToolStripMenuItem.Name = "clearLocalFileMappingsToolStripMenuItem";
            this.clearLocalFileMappingsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.clearLocalFileMappingsToolStripMenuItem.Text = "Clear Local File Mappings";
            this.clearLocalFileMappingsToolStripMenuItem.Click += new System.EventHandler(this.clearLocalFileMappingsToolStripMenuItem_Click);
            // 
            // addBreakpointOnCurrentLineToolStripMenuItem
            // 
            this.addBreakpointOnCurrentLineToolStripMenuItem.Name = "addBreakpointOnCurrentLineToolStripMenuItem";
            this.addBreakpointOnCurrentLineToolStripMenuItem.Size = new System.Drawing.Size(353, 22);
            this.addBreakpointOnCurrentLineToolStripMenuItem.Text = "Add Breakpoint on current line";
            this.addBreakpointOnCurrentLineToolStripMenuItem.Click += new System.EventHandler(this.addBreakpointOnCurrentLineToolStripMenuItem_Click);
            // 
            // cboxVRuler
            // 
            this.cboxVRuler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxVRuler.AutoSize = true;
            this.cboxVRuler.Location = new System.Drawing.Point(258, 37);
            this.cboxVRuler.Name = "cboxVRuler";
            this.cboxVRuler.Size = new System.Drawing.Size(58, 17);
            this.cboxVRuler.TabIndex = 32;
            this.cboxVRuler.Text = "VRuler";
            this.cboxVRuler.UseVisualStyleBackColor = true;
            this.cboxVRuler.CheckedChanged += new System.EventHandler(this.cboxVRuler_CheckedChanged);
            // 
            // cboxInvalidLines
            // 
            this.cboxInvalidLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxInvalidLines.AutoSize = true;
            this.cboxInvalidLines.Location = new System.Drawing.Point(14, 37);
            this.cboxInvalidLines.Name = "cboxInvalidLines";
            this.cboxInvalidLines.Size = new System.Drawing.Size(85, 17);
            this.cboxInvalidLines.TabIndex = 31;
            this.cboxInvalidLines.Text = "Invalid Lines";
            this.cboxInvalidLines.UseVisualStyleBackColor = true;
            this.cboxInvalidLines.CheckedChanged += new System.EventHandler(this.cboxInvalidLines_CheckedChanged);
            // 
            // cboxHRuler
            // 
            this.cboxHRuler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxHRuler.AutoSize = true;
            this.cboxHRuler.Location = new System.Drawing.Point(193, 37);
            this.cboxHRuler.Name = "cboxHRuler";
            this.cboxHRuler.Size = new System.Drawing.Size(59, 17);
            this.cboxHRuler.TabIndex = 30;
            this.cboxHRuler.Text = "HRuler";
            this.cboxHRuler.UseVisualStyleBackColor = true;
            this.cboxHRuler.CheckedChanged += new System.EventHandler(this.cboxHRuler_CheckedChanged);
            // 
            // cboxEOLMarkers
            // 
            this.cboxEOLMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxEOLMarkers.AutoSize = true;
            this.cboxEOLMarkers.Location = new System.Drawing.Point(105, 37);
            this.cboxEOLMarkers.Name = "cboxEOLMarkers";
            this.cboxEOLMarkers.Size = new System.Drawing.Size(88, 17);
            this.cboxEOLMarkers.TabIndex = 29;
            this.cboxEOLMarkers.Text = "EOL Markers";
            this.cboxEOLMarkers.UseVisualStyleBackColor = true;
            this.cboxEOLMarkers.CheckedChanged += new System.EventHandler(this.cboxEOLMarkers_CheckedChanged);
            // 
            // cboxSpaces
            // 
            this.cboxSpaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxSpaces.AutoSize = true;
            this.cboxSpaces.Location = new System.Drawing.Point(193, 21);
            this.cboxSpaces.Name = "cboxSpaces";
            this.cboxSpaces.Size = new System.Drawing.Size(62, 17);
            this.cboxSpaces.TabIndex = 28;
            this.cboxSpaces.Text = "Spaces";
            this.cboxSpaces.UseVisualStyleBackColor = true;
            this.cboxSpaces.CheckedChanged += new System.EventHandler(this.cboxSpaces_CheckedChanged);
            // 
            // cboxTabs
            // 
            this.cboxTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxTabs.AutoSize = true;
            this.cboxTabs.Location = new System.Drawing.Point(105, 21);
            this.cboxTabs.Name = "cboxTabs";
            this.cboxTabs.Size = new System.Drawing.Size(50, 17);
            this.cboxTabs.TabIndex = 27;
            this.cboxTabs.Text = "Tabs";
            this.cboxTabs.UseVisualStyleBackColor = true;
            this.cboxTabs.CheckedChanged += new System.EventHandler(this.cboxTabs_CheckedChanged);
            // 
            // cboxLineNumbers
            // 
            this.cboxLineNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxLineNumbers.AutoSize = true;
            this.cboxLineNumbers.Location = new System.Drawing.Point(14, 21);
            this.cboxLineNumbers.Name = "cboxLineNumbers";
            this.cboxLineNumbers.Size = new System.Drawing.Size(91, 17);
            this.cboxLineNumbers.TabIndex = 26;
            this.cboxLineNumbers.Text = "Line Numbers";
            this.cboxLineNumbers.UseVisualStyleBackColor = true;
            this.cboxLineNumbers.CheckedChanged += new System.EventHandler(this.cboxLineNumbers_CheckedChanged);
            // 
            // lbSource_CodeFileSaved
            // 
            this.lbSource_CodeFileSaved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSource_CodeFileSaved.AutoSize = true;
            this.lbSource_CodeFileSaved.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSource_CodeFileSaved.ForeColor = System.Drawing.Color.Green;
            this.lbSource_CodeFileSaved.Location = new System.Drawing.Point(619, 45);
            this.lbSource_CodeFileSaved.Name = "lbSource_CodeFileSaved";
            this.lbSource_CodeFileSaved.Size = new System.Drawing.Size(74, 15);
            this.lbSource_CodeFileSaved.TabIndex = 35;
            this.lbSource_CodeFileSaved.Text = "File Saved";
            this.lbSource_CodeFileSaved.Visible = false;
            // 
            // lbSourceCode_UnsavedChanges
            // 
            this.lbSourceCode_UnsavedChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSourceCode_UnsavedChanges.AutoSize = true;
            this.lbSourceCode_UnsavedChanges.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSourceCode_UnsavedChanges.ForeColor = System.Drawing.Color.Red;
            this.lbSourceCode_UnsavedChanges.Location = new System.Drawing.Point(609, 45);
            this.lbSourceCode_UnsavedChanges.Name = "lbSourceCode_UnsavedChanges";
            this.lbSourceCode_UnsavedChanges.Size = new System.Drawing.Size(96, 15);
            this.lbSourceCode_UnsavedChanges.TabIndex = 34;
            this.lbSourceCode_UnsavedChanges.Text = "Unsaved Data";
            this.lbSourceCode_UnsavedChanges.Visible = false;
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.BackColor = System.Drawing.SystemColors.Control;
            this.btSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btSave.ImageKey = "Project_save.ico";
            this.btSave.Location = new System.Drawing.Point(598, 19);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(123, 23);
            this.btSave.TabIndex = 36;
            this.btSave.Text = "Save Source Code";
            this.btSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btSave.UseVisualStyleBackColor = false;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // tbSourceCode_FileLoaded
            // 
            this.tbSourceCode_FileLoaded.AllowDrop = true;
            this.tbSourceCode_FileLoaded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSourceCode_FileLoaded.BackColor = System.Drawing.Color.White;
            this.tbSourceCode_FileLoaded.ForeColor = System.Drawing.Color.Black;
            this.tbSourceCode_FileLoaded.Location = new System.Drawing.Point(463, 20);
            this.tbSourceCode_FileLoaded.Name = "tbSourceCode_FileLoaded";
            this.tbSourceCode_FileLoaded.Size = new System.Drawing.Size(126, 20);
            this.tbSourceCode_FileLoaded.TabIndex = 37;
            this.tbSourceCode_FileLoaded.TextChanged += new System.EventHandler(this.tbSourceCode_FileLoaded_TextChanged);
            this.tbSourceCode_FileLoaded.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSourceCode_FileLoaded_DragDrop);
            this.tbSourceCode_FileLoaded.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbSourceCode_FileLoaded_DragEnter);
            this.tbSourceCode_FileLoaded.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSourceCode_FileLoaded_KeyPress);
            // 
            // tbTextSearch
            // 
            this.tbTextSearch.Location = new System.Drawing.Point(6, 26);
            this.tbTextSearch.Name = "tbTextSearch";
            this.tbTextSearch.Size = new System.Drawing.Size(267, 20);
            this.tbTextSearch.TabIndex = 39;
            this.tbTextSearch.TextChanged += new System.EventHandler(this.tbTextSearch_TextChanged);
            this.tbTextSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTextSearch_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Text Search (case sensitive)";
            // 
            // lbSearch_textNotFound
            // 
            this.lbSearch_textNotFound.AutoSize = true;
            this.lbSearch_textNotFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSearch_textNotFound.ForeColor = System.Drawing.Color.Red;
            this.lbSearch_textNotFound.Location = new System.Drawing.Point(279, 28);
            this.lbSearch_textNotFound.Name = "lbSearch_textNotFound";
            this.lbSearch_textNotFound.Size = new System.Drawing.Size(98, 15);
            this.lbSearch_textNotFound.TabIndex = 41;
            this.lbSearch_textNotFound.Text = "Text not found";
            this.lbSearch_textNotFound.Visible = false;
            // 
            // tbSourceCode_DirectoryOfFileLoaded
            // 
            this.tbSourceCode_DirectoryOfFileLoaded.AllowDrop = true;
            this.tbSourceCode_DirectoryOfFileLoaded.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSourceCode_DirectoryOfFileLoaded.BackColor = System.Drawing.Color.White;
            this.tbSourceCode_DirectoryOfFileLoaded.ForeColor = System.Drawing.Color.Black;
            this.tbSourceCode_DirectoryOfFileLoaded.Location = new System.Drawing.Point(79, 19);
            this.tbSourceCode_DirectoryOfFileLoaded.Name = "tbSourceCode_DirectoryOfFileLoaded";
            this.tbSourceCode_DirectoryOfFileLoaded.Size = new System.Drawing.Size(367, 20);
            this.tbSourceCode_DirectoryOfFileLoaded.TabIndex = 42;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(449, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "\\";
            // 
            // lbFileLoaded
            // 
            this.lbFileLoaded.AutoSize = true;
            this.lbFileLoaded.Location = new System.Drawing.Point(6, 24);
            this.lbFileLoaded.Name = "lbFileLoaded";
            this.lbFileLoaded.Size = new System.Drawing.Size(65, 13);
            this.lbFileLoaded.TabIndex = 44;
            this.lbFileLoaded.Text = "File Loaded:";
            this.lbFileLoaded.Click += new System.EventHandler(this.lbFileLoaded_Click);
            // 
            // toolStripWithSourceCodeActions
            // 
            this.toolStripWithSourceCodeActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tstbTextSearch,
            this.toolStripSeparator7,
            this.tbShowO2ObjectModel,
            this.toolStripLabel3,
            this.btSettings,
            this.toolStripSeparator1,
            this.btSaveFile,
            this.toolStripSeparator3,
            this.lbExecuteOnEngine,
            this.cbExternalEngineToUse,
            this.btExecuteOnExternalEngine,
            this.btShowHidePythonLogExecutionOutputData,
            this.lbCompileCode,
            this.btCompileCode,
            this.tsCompileStripSeparator,
            this.lbExecuteCode,
            this.cboxCompliledSourceCodeMethods,
            this.btExecuteSelectedMethod,
            this.btDebugMethod,
            this.lbCompilationErrors,
            this.btShowHideCompilationErrors,
            this.toolStripSeparator6,
            this.btDragAssemblyCreated,
            this.toolStripSeparator2,
            this.btSeachAndViewAst,
            this.btSelectedLineHistory,
            this.toolStripSeparator4,
            this.btShowLogs,
            this.lbSampleScripts,
            this.cBoxSampleScripts,
            this.toolStripSeparator5,
            this.btOpenFile,
            this.toolStripSeparator8,
            this.toolStripSeparator9,
            this.toolStripSeparator10});
            this.toolStripWithSourceCodeActions.Location = new System.Drawing.Point(0, 0);
            this.toolStripWithSourceCodeActions.Name = "toolStripWithSourceCodeActions";
            this.toolStripWithSourceCodeActions.Size = new System.Drawing.Size(963, 27);
            this.toolStripWithSourceCodeActions.TabIndex = 45;
            this.toolStripWithSourceCodeActions.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(41, 24);
            this.toolStripLabel1.Text = "search";
            // 
            // tstbTextSearch
            // 
            this.tstbTextSearch.Name = "tstbTextSearch";
            this.tstbTextSearch.Size = new System.Drawing.Size(100, 27);
            this.tstbTextSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tstbTextSearch_KeyDown);
            this.tstbTextSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tstbTextSearch_KeyUp);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
            // 
            // tbShowO2ObjectModel
            // 
            this.tbShowO2ObjectModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbShowO2ObjectModel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbShowO2ObjectModel.Name = "tbShowO2ObjectModel";
            this.tbShowO2ObjectModel.Size = new System.Drawing.Size(23, 24);
            this.tbShowO2ObjectModel.Text = "Show O2 Object Model";
            this.tbShowO2ObjectModel.Click += new System.EventHandler(this.tbShowO2ObjectModel_Click);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(44, 24);
            this.toolStripLabel3.Text = "config:";
            // 
            // btSettings
            // 
            this.btSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSettings.Name = "btSettings";
            this.btSettings.Size = new System.Drawing.Size(23, 24);
            this.btSettings.Text = "Settings, loaded file details and save into another file";
            this.btSettings.Click += new System.EventHandler(this.settings_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btSaveFile
            // 
            this.btSaveFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSaveFile.Enabled = false;
            this.btSaveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSaveFile.Name = "btSaveFile";
            this.btSaveFile.Size = new System.Drawing.Size(23, 24);
            this.btSaveFile.Text = "Save file";
            this.btSaveFile.Click += new System.EventHandler(this.btSaveFile_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // lbExecuteOnEngine
            // 
            this.lbExecuteOnEngine.Name = "lbExecuteOnEngine";
            this.lbExecuteOnEngine.Size = new System.Drawing.Size(106, 24);
            this.lbExecuteOnEngine.Text = "execute on engine:";
            this.lbExecuteOnEngine.Visible = false;
            // 
            // cbExternalEngineToUse
            // 
            this.cbExternalEngineToUse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbExternalEngineToUse.DropDownWidth = 80;
            this.cbExternalEngineToUse.Name = "cbExternalEngineToUse";
            this.cbExternalEngineToUse.Size = new System.Drawing.Size(85, 27);
            this.cbExternalEngineToUse.Visible = false;
            // 
            // btExecuteOnExternalEngine
            // 
            this.btExecuteOnExternalEngine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btExecuteOnExternalEngine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btExecuteOnExternalEngine.Name = "btExecuteOnExternalEngine";
            this.btExecuteOnExternalEngine.Size = new System.Drawing.Size(23, 24);
            this.btExecuteOnExternalEngine.Text = "Execute On External Engine Script";
            this.btExecuteOnExternalEngine.Visible = false;
            this.btExecuteOnExternalEngine.Click += new System.EventHandler(this.btExecutePythonScript_Click);
            // 
            // btShowHidePythonLogExecutionOutputData
            // 
            this.btShowHidePythonLogExecutionOutputData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btShowHidePythonLogExecutionOutputData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btShowHidePythonLogExecutionOutputData.Name = "btShowHidePythonLogExecutionOutputData";
            this.btShowHidePythonLogExecutionOutputData.Size = new System.Drawing.Size(23, 24);
            this.btShowHidePythonLogExecutionOutputData.Text = "Hide/Show Python Execution Output Data";
            this.btShowHidePythonLogExecutionOutputData.Visible = false;
            this.btShowHidePythonLogExecutionOutputData.Click += new System.EventHandler(this.btShowHidePythonLogExecutionOutputData_Click);
            // 
            // lbCompileCode
            // 
            this.lbCompileCode.Name = "lbCompileCode";
            this.lbCompileCode.Size = new System.Drawing.Size(82, 24);
            this.lbCompileCode.Text = "compile code:";
            this.lbCompileCode.Click += new System.EventHandler(this.toolStripLabel2_Click);
            // 
            // btCompileCode
            // 
            this.btCompileCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btCompileCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btCompileCode.Name = "btCompileCode";
            this.btCompileCode.Size = new System.Drawing.Size(23, 24);
            this.btCompileCode.Text = "Compile Source Code";
            this.btCompileCode.Click += new System.EventHandler(this.compile_Click);
            // 
            // tsCompileStripSeparator
            // 
            this.tsCompileStripSeparator.Name = "tsCompileStripSeparator";
            this.tsCompileStripSeparator.Size = new System.Drawing.Size(6, 27);
            // 
            // lbExecuteCode
            // 
            this.lbExecuteCode.Name = "lbExecuteCode";
            this.lbExecuteCode.Size = new System.Drawing.Size(79, 24);
            this.lbExecuteCode.Text = "execute code:";
            this.lbExecuteCode.Visible = false;
            // 
            // cboxCompliledSourceCodeMethods
            // 
            this.cboxCompliledSourceCodeMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxCompliledSourceCodeMethods.DropDownWidth = 250;
            this.cboxCompliledSourceCodeMethods.Name = "cboxCompliledSourceCodeMethods";
            this.cboxCompliledSourceCodeMethods.Size = new System.Drawing.Size(150, 27);
            this.cboxCompliledSourceCodeMethods.Visible = false;
            this.cboxCompliledSourceCodeMethods.SelectedIndexChanged += new System.EventHandler(this.cboxCompliledSourceCodeMethods_SelectedIndexChanged);
            this.cboxCompliledSourceCodeMethods.Click += new System.EventHandler(this.cboxCompliledSourceCodeMethods_Click);
            this.cboxCompliledSourceCodeMethods.TextChanged += new System.EventHandler(this.cboxCompliledSourceCodeMethods_TextChanged);
            // 
            // btExecuteSelectedMethod
            // 
            this.btExecuteSelectedMethod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btExecuteSelectedMethod.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btExecuteSelectedMethod.Name = "btExecuteSelectedMethod";
            this.btExecuteSelectedMethod.Size = new System.Drawing.Size(23, 24);
            this.btExecuteSelectedMethod.Text = "Execute Selected Method";
            this.btExecuteSelectedMethod.Visible = false;
            this.btExecuteSelectedMethod.Click += new System.EventHandler(this.executeSelectedMethod_Click);
            // 
            // btDebugMethod
            // 
            this.btDebugMethod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDebugMethod.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDebugMethod.Name = "btDebugMethod";
            this.btDebugMethod.Size = new System.Drawing.Size(23, 24);
            this.btDebugMethod.Text = "Execute Selected Method under Debug Mode";
            this.btDebugMethod.Visible = false;
            this.btDebugMethod.Click += new System.EventHandler(this.btDebugMethod_Click);
            // 
            // lbCompilationErrors
            // 
            this.lbCompilationErrors.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCompilationErrors.ForeColor = System.Drawing.Color.Red;
            this.lbCompilationErrors.Name = "lbCompilationErrors";
            this.lbCompilationErrors.Size = new System.Drawing.Size(111, 13);
            this.lbCompilationErrors.Text = "Compilation Errors";
            this.lbCompilationErrors.Visible = false;
            // 
            // btShowHideCompilationErrors
            // 
            this.btShowHideCompilationErrors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btShowHideCompilationErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btShowHideCompilationErrors.Name = "btShowHideCompilationErrors";
            this.btShowHideCompilationErrors.Size = new System.Drawing.Size(23, 4);
            this.btShowHideCompilationErrors.Text = "Show / Hide compilation errors";
            this.btShowHideCompilationErrors.Visible = false;
            this.btShowHideCompilationErrors.Click += new System.EventHandler(this.btShowHideCompilationErrors_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator6.Click += new System.EventHandler(this.toolStripSeparator6_Click);
            // 
            // btDragAssemblyCreated
            // 
            this.btDragAssemblyCreated.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDragAssemblyCreated.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDragAssemblyCreated.Name = "btDragAssemblyCreated";
            this.btDragAssemblyCreated.Size = new System.Drawing.Size(23, 4);
            this.btDragAssemblyCreated.Text = "Drag Assembly Created";
            this.btDragAssemblyCreated.Visible = false;
            this.btDragAssemblyCreated.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btDragAssemblyCreated_MouseDown);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btSeachAndViewAst
            // 
            this.btSeachAndViewAst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSeachAndViewAst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSeachAndViewAst.Name = "btSeachAndViewAst";
            this.btSeachAndViewAst.Size = new System.Drawing.Size(23, 4);
            this.btSeachAndViewAst.Text = "Search and AST details view";
            this.btSeachAndViewAst.Click += new System.EventHandler(this.btSeachAndViewAst_Click);
            // 
            // btSelectedLineHistory
            // 
            this.btSelectedLineHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSelectedLineHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSelectedLineHistory.Name = "btSelectedLineHistory";
            this.btSelectedLineHistory.Size = new System.Drawing.Size(23, 4);
            this.btSelectedLineHistory.Text = "View Selected Line History";
            this.btSelectedLineHistory.Click += new System.EventHandler(this.btSelectedLineHistory_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btShowLogs
            // 
            this.btShowLogs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btShowLogs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btShowLogs.Name = "btShowLogs";
            this.btShowLogs.Size = new System.Drawing.Size(23, 4);
            this.btShowLogs.Text = "Show Logs";
            this.btShowLogs.Click += new System.EventHandler(this.showLogs_Click);
            // 
            // lbSampleScripts
            // 
            this.lbSampleScripts.Name = "lbSampleScripts";
            this.lbSampleScripts.Size = new System.Drawing.Size(82, 15);
            this.lbSampleScripts.Text = "sample scripts";
            this.lbSampleScripts.Visible = false;
            // 
            // cBoxSampleScripts
            // 
            this.cBoxSampleScripts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxSampleScripts.Name = "cBoxSampleScripts";
            this.cBoxSampleScripts.Size = new System.Drawing.Size(121, 23);
            this.cBoxSampleScripts.Visible = false;
            this.cBoxSampleScripts.SelectedIndexChanged += new System.EventHandler(this.cBoxSampleScripts_SelectedIndexChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btOpenFile
            // 
            this.btOpenFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(23, 4);
            this.btOpenFile.Text = "Open File";
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // groupBoxWithFileAndSaveSettings
            // 
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.llPutFilePathInClipboard);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.cbAutoTryToFixSourceCodeFileReferences);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.groupBox1);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.llReload);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.llCurrenlyLoadedObjectModel);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.tbMaxLoadSize);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.label4);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.lbFileLoaded);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.tbSourceCode_DirectoryOfFileLoaded);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.btSave);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.lbSource_CodeFileSaved);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.label2);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.lbSourceCode_UnsavedChanges);
            this.groupBoxWithFileAndSaveSettings.Controls.Add(this.tbSourceCode_FileLoaded);
            this.groupBoxWithFileAndSaveSettings.Location = new System.Drawing.Point(10, 0);
            this.groupBoxWithFileAndSaveSettings.Name = "groupBoxWithFileAndSaveSettings";
            this.groupBoxWithFileAndSaveSettings.Size = new System.Drawing.Size(727, 134);
            this.groupBoxWithFileAndSaveSettings.TabIndex = 46;
            this.groupBoxWithFileAndSaveSettings.TabStop = false;
            this.groupBoxWithFileAndSaveSettings.Text = "Settings, loaded file details and save into another file";
            this.groupBoxWithFileAndSaveSettings.Visible = false;
            // 
            // llPutFilePathInClipboard
            // 
            this.llPutFilePathInClipboard.AutoSize = true;
            this.llPutFilePathInClipboard.Location = new System.Drawing.Point(597, 69);
            this.llPutFilePathInClipboard.Name = "llPutFilePathInClipboard";
            this.llPutFilePathInClipboard.Size = new System.Drawing.Size(120, 13);
            this.llPutFilePathInClipboard.TabIndex = 53;
            this.llPutFilePathInClipboard.TabStop = true;
            this.llPutFilePathInClipboard.Text = "put file path in Clipboard";
            this.llPutFilePathInClipboard.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llPutFilePathInClipboard_LinkClicked);
            // 
            // cbAutoTryToFixSourceCodeFileReferences
            // 
            this.cbAutoTryToFixSourceCodeFileReferences.AutoSize = true;
            this.cbAutoTryToFixSourceCodeFileReferences.Checked = true;
            this.cbAutoTryToFixSourceCodeFileReferences.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoTryToFixSourceCodeFileReferences.Location = new System.Drawing.Point(367, 109);
            this.cbAutoTryToFixSourceCodeFileReferences.Name = "cbAutoTryToFixSourceCodeFileReferences";
            this.cbAutoTryToFixSourceCodeFileReferences.Size = new System.Drawing.Size(236, 17);
            this.cbAutoTryToFixSourceCodeFileReferences.TabIndex = 52;
            this.cbAutoTryToFixSourceCodeFileReferences.Text = "Auto Try to Fix Source Code File References";
            this.cbAutoTryToFixSourceCodeFileReferences.UseVisualStyleBackColor = true;
            this.cbAutoTryToFixSourceCodeFileReferences.CheckedChanged += new System.EventHandler(this.cbAutoTryToFixSourceCodeFileReferences_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboxLineNumbers);
            this.groupBox1.Controls.Add(this.cboxInvalidLines);
            this.groupBox1.Controls.Add(this.cboxTabs);
            this.groupBox1.Controls.Add(this.cboxEOLMarkers);
            this.groupBox1.Controls.Add(this.cboxSpaces);
            this.groupBox1.Controls.Add(this.cboxHRuler);
            this.groupBox1.Controls.Add(this.cboxVRuler);
            this.groupBox1.Location = new System.Drawing.Point(9, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 60);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SharpDevelop editor Preferences";
            // 
            // llReload
            // 
            this.llReload.AutoSize = true;
            this.llReload.Location = new System.Drawing.Point(662, 110);
            this.llReload.Name = "llReload";
            this.llReload.Size = new System.Drawing.Size(55, 13);
            this.llReload.TabIndex = 48;
            this.llReload.TabStop = true;
            this.llReload.Text = "reload File";
            this.llReload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llReload_LinkClicked);
            // 
            // llCurrenlyLoadedObjectModel
            // 
            this.llCurrenlyLoadedObjectModel.AutoSize = true;
            this.llCurrenlyLoadedObjectModel.Location = new System.Drawing.Point(609, 90);
            this.llCurrenlyLoadedObjectModel.Name = "llCurrenlyLoadedObjectModel";
            this.llCurrenlyLoadedObjectModel.Size = new System.Drawing.Size(108, 13);
            this.llCurrenlyLoadedObjectModel.TabIndex = 47;
            this.llCurrenlyLoadedObjectModel.TabStop = true;
            this.llCurrenlyLoadedObjectModel.Text = "drag C# object model";
            this.llCurrenlyLoadedObjectModel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.llCurrenlyLoadedObjectModel_MouseDown);
            // 
            // tbMaxLoadSize
            // 
            this.tbMaxLoadSize.AllowDrop = true;
            this.tbMaxLoadSize.BackColor = System.Drawing.Color.White;
            this.tbMaxLoadSize.ForeColor = System.Drawing.Color.Black;
            this.tbMaxLoadSize.Location = new System.Drawing.Point(110, 43);
            this.tbMaxLoadSize.Name = "tbMaxLoadSize";
            this.tbMaxLoadSize.Size = new System.Drawing.Size(93, 20);
            this.tbMaxLoadSize.TabIndex = 46;
            this.tbMaxLoadSize.TextChanged += new System.EventHandler(this.tbMaxLoadSize_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Max Load Size (kb)";
            // 
            // lbPartialFileView
            // 
            this.lbPartialFileView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPartialFileView.FormattingEnabled = true;
            this.lbPartialFileView.Location = new System.Drawing.Point(0, -2);
            this.lbPartialFileView.Name = "lbPartialFileView";
            this.lbPartialFileView.Size = new System.Drawing.Size(957, 407);
            this.lbPartialFileView.TabIndex = 47;
            this.lbPartialFileView.Visible = false;
            // 
            // tbExecutionHistoryOrLog
            // 
            this.tbExecutionHistoryOrLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbExecutionHistoryOrLog.Location = new System.Drawing.Point(573, 0);
            this.tbExecutionHistoryOrLog.Multiline = true;
            this.tbExecutionHistoryOrLog.Name = "tbExecutionHistoryOrLog";
            this.tbExecutionHistoryOrLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbExecutionHistoryOrLog.Size = new System.Drawing.Size(364, 378);
            this.tbExecutionHistoryOrLog.TabIndex = 48;
            this.tbExecutionHistoryOrLog.Visible = false;
            this.tbExecutionHistoryOrLog.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbExecutionHistoryOrLog_MouseDoubleClick);
            // 
            // tvCompilationErrors
            // 
            this.tvCompilationErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tvCompilationErrors.Location = new System.Drawing.Point(574, 0);
            this.tvCompilationErrors.Name = "tvCompilationErrors";
            this.tvCompilationErrors.ShowNodeToolTips = true;
            this.tvCompilationErrors.Size = new System.Drawing.Size(361, 209);
            this.tvCompilationErrors.TabIndex = 50;
            this.tvCompilationErrors.Visible = false;
            this.tvCompilationErrors.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCompilationErrors_AfterSelect);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbTextSearch);
            this.tabPage1.Controls.Add(this.lbSearch_textNotFound);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(142, 20);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Text Search";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tcSourceInfo
            // 
            this.tcSourceInfo.Controls.Add(this.tabPage1);
            this.tcSourceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSourceInfo.Location = new System.Drawing.Point(0, 0);
            this.tcSourceInfo.Name = "tcSourceInfo";
            this.tcSourceInfo.SelectedIndex = 0;
            this.tcSourceInfo.Size = new System.Drawing.Size(150, 46);
            this.tcSourceInfo.TabIndex = 52;
            // 
            // scCodeAndAst
            // 
            this.scCodeAndAst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scCodeAndAst.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scCodeAndAst.Location = new System.Drawing.Point(3, 30);
            this.scCodeAndAst.Name = "scCodeAndAst";
            this.scCodeAndAst.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scCodeAndAst.Panel1
            // 
            this.scCodeAndAst.Panel1.Controls.Add(this.groupBoxWithFileAndSaveSettings);
            this.scCodeAndAst.Panel1.Controls.Add(this.tvCompilationErrors);
            this.scCodeAndAst.Panel1.Controls.Add(this.tbExecutionHistoryOrLog);
            this.scCodeAndAst.Panel1.Controls.Add(this.tecSourceCode);
            this.scCodeAndAst.Panel1.Controls.Add(this.lbPartialFileView);
            // 
            // scCodeAndAst.Panel2
            // 
            this.scCodeAndAst.Panel2.Controls.Add(this.tcSourceInfo);
            this.scCodeAndAst.Panel2Collapsed = true;
            this.scCodeAndAst.Size = new System.Drawing.Size(960, 406);
            this.scCodeAndAst.SplitterDistance = 291;
            this.scCodeAndAst.TabIndex = 53;
            // 
            // tecSourceCode
            // 
            this.tecSourceCode.AllowDrop = true;
            this.tecSourceCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tecSourceCode.ContextMenuStrip = this.menuStripForSourceEdition;
            this.tecSourceCode.IsIconBarVisible = true;
            this.tecSourceCode.IsReadOnly = false;
            this.tecSourceCode.Location = new System.Drawing.Point(0, 0);
            this.tecSourceCode.Name = "tecSourceCode";
            this.tecSourceCode.ShowEOLMarkers = true;
            this.tecSourceCode.ShowSpaces = true;
            this.tecSourceCode.ShowTabs = true;
            this.tecSourceCode.Size = new System.Drawing.Size(956, 403);
            this.tecSourceCode.TabIndex = 17;
            this.toolTip1.SetToolTip(this.tecSourceCode, "Source Code Editor");
            this.tecSourceCode.Load += new System.EventHandler(this.tecSourceCode_Load);
            this.tecSourceCode.DragDrop += new System.Windows.Forms.DragEventHandler(this.tecSourceCode_DragDrop);
            this.tecSourceCode.DragEnter += new System.Windows.Forms.DragEventHandler(this.tecSourceCode_DragEnter);
            this.tecSourceCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tecSourceCode_KeyPress);
            this.tecSourceCode.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tecSourceCode_MouseClick);
            this.tecSourceCode.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tecSourceCode_MouseMove);
            // 
            // SourceCodeEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.scCodeAndAst);
            this.Controls.Add(this.toolStripWithSourceCodeActions);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "SourceCodeEditor";
            this.Size = new System.Drawing.Size(963, 439);
            this.Load += new System.EventHandler(this.SourceCodeEditor_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.SourceCodeEditor_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.SourceCodeEditor_DragEnter);
            this.menuStripForSourceEdition.ResumeLayout(false);
            this.toolStripWithSourceCodeActions.ResumeLayout(false);
            this.toolStripWithSourceCodeActions.PerformLayout();
            this.groupBoxWithFileAndSaveSettings.ResumeLayout(false);
            this.groupBoxWithFileAndSaveSettings.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tcSourceInfo.ResumeLayout(false);
            this.scCodeAndAst.Panel1.ResumeLayout(false);
            this.scCodeAndAst.Panel1.PerformLayout();
            this.scCodeAndAst.Panel2.ResumeLayout(false);
            this.scCodeAndAst.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ICSharpCode.TextEditor.TextEditorControl tecSourceCode;
        private System.Windows.Forms.CheckBox cboxVRuler;
        private System.Windows.Forms.CheckBox cboxInvalidLines;
        private System.Windows.Forms.CheckBox cboxHRuler;
        private System.Windows.Forms.CheckBox cboxEOLMarkers;
        private System.Windows.Forms.CheckBox cboxSpaces;
        private System.Windows.Forms.CheckBox cboxTabs;
        private System.Windows.Forms.CheckBox cboxLineNumbers;
        private System.Windows.Forms.Label lbSource_CodeFileSaved;
        private System.Windows.Forms.Label lbSourceCode_UnsavedChanges;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TextBox tbSourceCode_FileLoaded;
        private System.Windows.Forms.TextBox tbTextSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbSearch_textNotFound;
        private System.Windows.Forms.TextBox tbSourceCode_DirectoryOfFileLoaded;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbFileLoaded;
        private System.Windows.Forms.ToolStrip toolStripWithSourceCodeActions;
        private System.Windows.Forms.GroupBox groupBoxWithFileAndSaveSettings;
        private System.Windows.Forms.ToolStripButton btSettings;
        private System.Windows.Forms.ToolStripButton btCompileCode;
        private System.Windows.Forms.ToolStripLabel lbExecuteCode;
        private System.Windows.Forms.ToolStripComboBox cboxCompliledSourceCodeMethods;
        private System.Windows.Forms.ToolStripButton btExecuteSelectedMethod;
        private System.Windows.Forms.ToolStripButton btShowLogs;
        private System.Windows.Forms.ToolStripLabel lbCompileCode;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel lbSampleScripts;
        private System.Windows.Forms.ToolStripComboBox cBoxSampleScripts;
        private System.Windows.Forms.ToolStripLabel lbCompilationErrors;
        private System.Windows.Forms.ToolStripSeparator tsCompileStripSeparator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TextBox tbMaxLoadSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lbPartialFileView;
        private System.Windows.Forms.ToolStripButton btSelectedLineHistory;
        private System.Windows.Forms.TextBox tbExecutionHistoryOrLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.LinkLabel llCurrenlyLoadedObjectModel;
        private System.Windows.Forms.LinkLabel llReload;
        private System.Windows.Forms.ToolStripButton btSaveFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripButton btDragAssemblyCreated;
        private System.Windows.Forms.ToolStripButton btExecuteOnExternalEngine;
        private System.Windows.Forms.ToolStripLabel lbExecuteOnEngine;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btShowHidePythonLogExecutionOutputData;
        private System.Windows.Forms.ToolStripComboBox cbExternalEngineToUse;
        private System.Windows.Forms.CheckBox cbAutoTryToFixSourceCodeFileReferences;
        private System.Windows.Forms.ToolStripButton btShowHideCompilationErrors;
        private System.Windows.Forms.ToolStripButton tbShowO2ObjectModel;
        private System.Windows.Forms.ContextMenuStrip menuStripForSourceEdition;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btOpenFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem executeSelectedMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoCompileEvery10SecondsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addBreakpointOnCurrentLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btDebugMethod;
        private System.Windows.Forms.TreeView tvCompilationErrors;    
        private System.Windows.Forms.ToolStripMenuItem compileSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listinLogViewCurrentAssemblyRefernecesAutomaticallyAddedToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tcSourceInfo;
        private System.Windows.Forms.SplitContainer scCodeAndAst;
        private System.Windows.Forms.ToolStripButton btSeachAndViewAst;
        private System.Windows.Forms.LinkLabel llPutFilePathInClipboard;
        private System.Windows.Forms.ToolStripMenuItem enableCodeCompleteToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStripMenuItem enableOrDisableAutoBackupOnCompileSucessforCSharpToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tstbTextSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripMenuItem clearCompileCacheToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearLocalFileMappingsToolStripMenuItem;
    }
}
