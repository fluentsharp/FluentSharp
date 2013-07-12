// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)



namespace FluentSharp.REPL.Controls
{
    partial class ascx_Scripts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ascx_Scripts));
            this.scSourceCodeAndCompile = new System.Windows.Forms.SplitContainer();
            this.sourceCodeEditor = new SourceCodeEditor();
            this.tbCompile = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbMainClass = new System.Windows.Forms.TextBox();
            this.lbSourceCode_CompilationResult = new System.Windows.Forms.ListBox();
            this.rbCreateExe = new System.Windows.Forms.RadioButton();
            this.rbCreateDll = new System.Windows.Forms.RadioButton();
            this.btSourceCode_Compile = new System.Windows.Forms.Button();
            this.ilNamepasceClassMethods = new System.Windows.Forms.ImageList(this.components);
            this.tpReferences = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.tbExtraReferencesToAdd = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbSourceCode_AutoCompileOnEnter = new System.Windows.Forms.CheckBox();
            this.cbAutoSaveOnCompile = new System.Windows.Forms.CheckBox();
            this.cbAutoExecuteOnMethodCompile = new System.Windows.Forms.CheckBox();
            this.ilDirectoriesAndFiles = new System.Windows.Forms.ImageList(this.components);
            this.scTopLevel = new System.Windows.Forms.SplitContainer();
            this.scScriptsFolderAndScriptsEditor = new System.Windows.Forms.SplitContainer();
            this.scriptsFolder = new ascx_ScriptsFolder();
            this.scSourceCodeAndCompile.Panel1.SuspendLayout();
            this.scSourceCodeAndCompile.Panel2.SuspendLayout();
            this.scSourceCodeAndCompile.SuspendLayout();
            this.tbCompile.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tpReferences.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.scTopLevel.Panel1.SuspendLayout();
            this.scTopLevel.SuspendLayout();
            this.scScriptsFolderAndScriptsEditor.Panel1.SuspendLayout();
            this.scScriptsFolderAndScriptsEditor.Panel2.SuspendLayout();
            this.scScriptsFolderAndScriptsEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // scSourceCodeAndCompile
            // 
            this.scSourceCodeAndCompile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scSourceCodeAndCompile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSourceCodeAndCompile.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scSourceCodeAndCompile.Location = new System.Drawing.Point(0, 0);
            this.scSourceCodeAndCompile.Name = "scSourceCodeAndCompile";
            this.scSourceCodeAndCompile.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scSourceCodeAndCompile.Panel1
            // 
            this.scSourceCodeAndCompile.Panel1.Controls.Add(this.sourceCodeEditor);
            // 
            // scSourceCodeAndCompile.Panel2
            // 
            this.scSourceCodeAndCompile.Panel2.Controls.Add(this.tbCompile);
            this.scSourceCodeAndCompile.Panel2MinSize = 100;
            this.scSourceCodeAndCompile.Size = new System.Drawing.Size(350, 437);
            this.scSourceCodeAndCompile.SplitterDistance = 322;
            this.scSourceCodeAndCompile.TabIndex = 5;
            // 
            // sourceCodeEditor
            // 
            this.sourceCodeEditor._ShowSearchAndAstDetails = true;
            this.sourceCodeEditor._ShowTopMenu = true;
            this.sourceCodeEditor.AllowDrop = true;
            this.sourceCodeEditor.BackColor = System.Drawing.SystemColors.Control;
            this.sourceCodeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceCodeEditor.ForeColor = System.Drawing.Color.Black;
            this.sourceCodeEditor.Location = new System.Drawing.Point(0, 0);
            this.sourceCodeEditor.Name = "sourceCodeEditor";
            this.sourceCodeEditor.Size = new System.Drawing.Size(346, 318);
            this.sourceCodeEditor.TabIndex = 15;
            this.sourceCodeEditor.Load += new System.EventHandler(this.asceSourceCodeEditor_Load);
            // 
            // tbCompile
            // 
            this.tbCompile.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tbCompile.Controls.Add(this.tabPage1);
            this.tbCompile.Controls.Add(this.tpReferences);
            this.tbCompile.Controls.Add(this.tabPage2);
            this.tbCompile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCompile.Location = new System.Drawing.Point(0, 0);
            this.tbCompile.Name = "tbCompile";
            this.tbCompile.SelectedIndex = 0;
            this.tbCompile.Size = new System.Drawing.Size(346, 107);
            this.tbCompile.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbMainClass);
            this.tabPage1.Controls.Add(this.lbSourceCode_CompilationResult);
            this.tabPage1.Controls.Add(this.rbCreateExe);
            this.tabPage1.Controls.Add(this.rbCreateDll);
            this.tabPage1.Controls.Add(this.btSourceCode_Compile);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(338, 81);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Compile options";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbMainClass
            // 
            this.tbMainClass.Location = new System.Drawing.Point(179, 50);
            this.tbMainClass.Name = "tbMainClass";
            this.tbMainClass.Size = new System.Drawing.Size(88, 20);
            this.tbMainClass.TabIndex = 11;
            // 
            // lbSourceCode_CompilationResult
            // 
            this.lbSourceCode_CompilationResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSourceCode_CompilationResult.FormattingEnabled = true;
            this.lbSourceCode_CompilationResult.HorizontalScrollbar = true;
            this.lbSourceCode_CompilationResult.Location = new System.Drawing.Point(273, 4);
            this.lbSourceCode_CompilationResult.Name = "lbSourceCode_CompilationResult";
            this.lbSourceCode_CompilationResult.ScrollAlwaysVisible = true;
            this.lbSourceCode_CompilationResult.Size = new System.Drawing.Size(127, 69);
            this.lbSourceCode_CompilationResult.TabIndex = 4;
            this.lbSourceCode_CompilationResult.SelectedIndexChanged += new System.EventHandler(this.lbSourceCode_CompilationResult_SelectedIndexChanged);
            // 
            // rbCreateExe
            // 
            this.rbCreateExe.AutoSize = true;
            this.rbCreateExe.Location = new System.Drawing.Point(57, 50);
            this.rbCreateExe.Name = "rbCreateExe";
            this.rbCreateExe.Size = new System.Drawing.Size(113, 17);
            this.rbCreateExe.TabIndex = 10;
            this.rbCreateExe.Text = "EXE  main class ->";
            this.rbCreateExe.UseVisualStyleBackColor = true;
            this.rbCreateExe.CheckedChanged += new System.EventHandler(this.rbCreateExe_CheckedChanged);
            // 
            // rbCreateDll
            // 
            this.rbCreateDll.AutoSize = true;
            this.rbCreateDll.Checked = true;
            this.rbCreateDll.Location = new System.Drawing.Point(6, 50);
            this.rbCreateDll.Name = "rbCreateDll";
            this.rbCreateDll.Size = new System.Drawing.Size(45, 17);
            this.rbCreateDll.TabIndex = 9;
            this.rbCreateDll.TabStop = true;
            this.rbCreateDll.Text = "DLL";
            this.rbCreateDll.UseVisualStyleBackColor = true;
            this.rbCreateDll.CheckedChanged += new System.EventHandler(this.rbCreateDll_CheckedChanged);
            // 
            // btSourceCode_Compile
            // 
            this.btSourceCode_Compile.BackColor = System.Drawing.SystemColors.Control;
            this.btSourceCode_Compile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSourceCode_Compile.ForeColor = System.Drawing.Color.Black;
            this.btSourceCode_Compile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btSourceCode_Compile.ImageKey = "Findings_API.ico";
            this.btSourceCode_Compile.ImageList = this.ilNamepasceClassMethods;
            this.btSourceCode_Compile.Location = new System.Drawing.Point(6, 6);
            this.btSourceCode_Compile.Name = "btSourceCode_Compile";
            this.btSourceCode_Compile.Size = new System.Drawing.Size(261, 38);
            this.btSourceCode_Compile.TabIndex = 0;
            this.btSourceCode_Compile.Text = "Compile Source Code";
            this.btSourceCode_Compile.UseVisualStyleBackColor = false;
            this.btSourceCode_Compile.Click += new System.EventHandler(this.btSourceCode_Compile_Click);
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
            // tpReferences
            // 
            this.tpReferences.Controls.Add(this.label2);
            this.tpReferences.Controls.Add(this.tbExtraReferencesToAdd);
            this.tpReferences.Location = new System.Drawing.Point(4, 4);
            this.tpReferences.Name = "tpReferences";
            this.tpReferences.Padding = new System.Windows.Forms.Padding(3);
            this.tpReferences.Size = new System.Drawing.Size(338, 81);
            this.tpReferences.TabIndex = 2;
            this.tpReferences.Text = "References";
            this.tpReferences.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Compile-time references:";
            // 
            // tbExtraReferencesToAdd
            // 
            this.tbExtraReferencesToAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbExtraReferencesToAdd.Location = new System.Drawing.Point(132, 5);
            this.tbExtraReferencesToAdd.Multiline = true;
            this.tbExtraReferencesToAdd.Name = "tbExtraReferencesToAdd";
            this.tbExtraReferencesToAdd.Size = new System.Drawing.Size(200, 70);
            this.tbExtraReferencesToAdd.TabIndex = 15;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbSourceCode_AutoCompileOnEnter);
            this.tabPage2.Controls.Add(this.cbAutoSaveOnCompile);
            this.tabPage2.Controls.Add(this.cbAutoExecuteOnMethodCompile);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(338, 81);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Config settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cbSourceCode_AutoCompileOnEnter
            // 
            this.cbSourceCode_AutoCompileOnEnter.AutoSize = true;
            this.cbSourceCode_AutoCompileOnEnter.Checked = true;
            this.cbSourceCode_AutoCompileOnEnter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSourceCode_AutoCompileOnEnter.Location = new System.Drawing.Point(6, 3);
            this.cbSourceCode_AutoCompileOnEnter.Name = "cbSourceCode_AutoCompileOnEnter";
            this.cbSourceCode_AutoCompileOnEnter.Size = new System.Drawing.Size(134, 17);
            this.cbSourceCode_AutoCompileOnEnter.TabIndex = 9;
            this.cbSourceCode_AutoCompileOnEnter.Text = "Auto Compile on \'enter\'";
            this.cbSourceCode_AutoCompileOnEnter.UseVisualStyleBackColor = true;
            // 
            // cbAutoSaveOnCompile
            // 
            this.cbAutoSaveOnCompile.AutoSize = true;
            this.cbAutoSaveOnCompile.Checked = true;
            this.cbAutoSaveOnCompile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoSaveOnCompile.Enabled = false;
            this.cbAutoSaveOnCompile.Location = new System.Drawing.Point(6, 22);
            this.cbAutoSaveOnCompile.Name = "cbAutoSaveOnCompile";
            this.cbAutoSaveOnCompile.Size = new System.Drawing.Size(130, 17);
            this.cbAutoSaveOnCompile.TabIndex = 11;
            this.cbAutoSaveOnCompile.Text = "Auto Save on compile";
            this.cbAutoSaveOnCompile.UseVisualStyleBackColor = true;
            // 
            // cbAutoExecuteOnMethodCompile
            // 
            this.cbAutoExecuteOnMethodCompile.Enabled = false;
            this.cbAutoExecuteOnMethodCompile.Location = new System.Drawing.Point(6, 41);
            this.cbAutoExecuteOnMethodCompile.Name = "cbAutoExecuteOnMethodCompile";
            this.cbAutoExecuteOnMethodCompile.Size = new System.Drawing.Size(170, 40);
            this.cbAutoExecuteOnMethodCompile.TabIndex = 12;
            this.cbAutoExecuteOnMethodCompile.Text = "Auto execute previous method on compile";
            this.cbAutoExecuteOnMethodCompile.UseVisualStyleBackColor = true;
            // 
            // ilDirectoriesAndFiles
            // 
            this.ilDirectoriesAndFiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilDirectoriesAndFiles.ImageStream")));
            this.ilDirectoriesAndFiles.TransparentColor = System.Drawing.Color.Transparent;
            this.ilDirectoriesAndFiles.Images.SetKeyName(0, "Explorer_Folder.ico");
            this.ilDirectoriesAndFiles.Images.SetKeyName(1, "Explorer_File.ico");
            this.ilDirectoriesAndFiles.Images.SetKeyName(2, "project_sourceRoot.ico");
            // 
            // scTopLevel
            // 
            this.scTopLevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scTopLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTopLevel.Location = new System.Drawing.Point(0, 0);
            this.scTopLevel.Name = "scTopLevel";
            // 
            // scTopLevel.Panel1
            // 
            this.scTopLevel.Panel1.Controls.Add(this.scScriptsFolderAndScriptsEditor);
            this.scTopLevel.Size = new System.Drawing.Size(826, 437);
            this.scTopLevel.SplitterDistance = 531;
            this.scTopLevel.TabIndex = 6;
            // 
            // scScriptsFolderAndScriptsEditor
            // 
            this.scScriptsFolderAndScriptsEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scScriptsFolderAndScriptsEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scScriptsFolderAndScriptsEditor.Location = new System.Drawing.Point(0, 0);
            this.scScriptsFolderAndScriptsEditor.Name = "scScriptsFolderAndScriptsEditor";
            // 
            // scScriptsFolderAndScriptsEditor.Panel1
            // 
            this.scScriptsFolderAndScriptsEditor.Panel1.Controls.Add(this.scriptsFolder);
            // 
            // scScriptsFolderAndScriptsEditor.Panel2
            // 
            this.scScriptsFolderAndScriptsEditor.Panel2.Controls.Add(this.scSourceCodeAndCompile);
            this.scScriptsFolderAndScriptsEditor.Size = new System.Drawing.Size(531, 437);
            this.scScriptsFolderAndScriptsEditor.SplitterDistance = 177;
            this.scScriptsFolderAndScriptsEditor.TabIndex = 6;
            // 
            // scriptsFolder
            // 
            this.scriptsFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptsFolder.Location = new System.Drawing.Point(0, 0);
            this.scriptsFolder.Name = "scriptsFolder";
            this.scriptsFolder.Size = new System.Drawing.Size(173, 433);
            this.scriptsFolder.sourceCodeEditor = null;
            this.scriptsFolder.TabIndex = 0;
            // 
            // ascx_Scripts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.scTopLevel);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "ascx_Scripts";
            this.Size = new System.Drawing.Size(826, 437);
            this.Load += new System.EventHandler(this.ascx_Scripts_Load);
            this.scSourceCodeAndCompile.Panel1.ResumeLayout(false);
            this.scSourceCodeAndCompile.Panel2.ResumeLayout(false);
            this.scSourceCodeAndCompile.ResumeLayout(false);
            this.tbCompile.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tpReferences.ResumeLayout(false);
            this.tpReferences.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.scTopLevel.Panel1.ResumeLayout(false);
            this.scTopLevel.ResumeLayout(false);
            this.scScriptsFolderAndScriptsEditor.Panel1.ResumeLayout(false);
            this.scScriptsFolderAndScriptsEditor.Panel2.ResumeLayout(false);
            this.scScriptsFolderAndScriptsEditor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scSourceCodeAndCompile;
        private System.Windows.Forms.Button btSourceCode_Compile;
        private System.Windows.Forms.ImageList ilDirectoriesAndFiles;
        private System.Windows.Forms.ImageList ilNamepasceClassMethods;
        private SourceCodeEditor sourceCodeEditor;
        private System.Windows.Forms.ListBox lbSourceCode_CompilationResult;
        private System.Windows.Forms.RadioButton rbCreateExe;
        private System.Windows.Forms.RadioButton rbCreateDll;
        private System.Windows.Forms.TextBox tbMainClass;
        private System.Windows.Forms.TabControl tbCompile;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox cbSourceCode_AutoCompileOnEnter;
        private System.Windows.Forms.CheckBox cbAutoSaveOnCompile;
        private System.Windows.Forms.CheckBox cbAutoExecuteOnMethodCompile;
        private System.Windows.Forms.TabPage tpReferences;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbExtraReferencesToAdd;
        private System.Windows.Forms.SplitContainer scTopLevel;
        //private ascx_AssemblyInvoke assemblyInvoke;
        private System.Windows.Forms.SplitContainer scScriptsFolderAndScriptsEditor;
        private ascx_ScriptsFolder scriptsFolder;

    }
}
