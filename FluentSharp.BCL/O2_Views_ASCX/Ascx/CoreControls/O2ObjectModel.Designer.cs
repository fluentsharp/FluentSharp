// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Controls
{
    partial class O2ObjectModel
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
            this.tcO2ObjectModel = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbMethodDetails_OriginalSignature = new System.Windows.Forms.TextBox();
            this.lbMethodDetails_OriginalSignature = new System.Windows.Forms.Label();
            this.tbMethodDetails_Signature = new System.Windows.Forms.TextBox();
            this.lbMethodDetails_Signature = new System.Windows.Forms.Label();
            this.tbMethodDetails_ReturnType = new System.Windows.Forms.TextBox();
            this.lbMethodDetails_ReturnType = new System.Windows.Forms.Label();
            this.tbMethodDetails_Parameters = new System.Windows.Forms.TextBox();
            this.lbMethodDetails_Parameters = new System.Windows.Forms.Label();
            this.tbMethodDetails_Name = new System.Windows.Forms.TextBox();
            this.lbMethodDetails_Name = new System.Windows.Forms.Label();
            this.tbMethodDetails_Type = new System.Windows.Forms.TextBox();
            this.lbMethodDetails_Type = new System.Windows.Forms.Label();
            this.tbFilterBy_ReturnType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbFilterBy_ParameterType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFilterBy_MethodName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFilterBy_MethodType = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tvAssembliesLoaded = new System.Windows.Forms.TreeView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tvExtraAssembliesToLoad = new System.Windows.Forms.TreeView();
            this.cbPerformRegExSearch = new System.Windows.Forms.CheckBox();
            this.cbHideCSharpGeneratedMethods = new System.Windows.Forms.CheckBox();
            this.llRefreshFunctionsViewer = new System.Windows.Forms.LinkLabel();
            this.filteredFunctionsViewer = new Controls.ascx_FunctionsViewer();
            this.functionsViewer = new Controls.ascx_FunctionsViewer();
            this.tcO2ObjectModel.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcO2ObjectModel
            // 
            this.tcO2ObjectModel.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tcO2ObjectModel.Controls.Add(this.tabPage3);
            this.tcO2ObjectModel.Controls.Add(this.tabPage1);
            this.tcO2ObjectModel.Controls.Add(this.tabPage2);
            this.tcO2ObjectModel.Controls.Add(this.tabPage4);
            this.tcO2ObjectModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcO2ObjectModel.Location = new System.Drawing.Point(0, 0);
            this.tcO2ObjectModel.Name = "tcO2ObjectModel";
            this.tcO2ObjectModel.SelectedIndex = 0;
            this.tcO2ObjectModel.Size = new System.Drawing.Size(484, 325);
            this.tcO2ObjectModel.TabIndex = 51;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.filteredFunctionsViewer);
            this.tabPage3.Controls.Add(this.tbFilterBy_ReturnType);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.tbFilterBy_ParameterType);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.tbFilterBy_MethodName);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.tbFilterBy_MethodType);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(476, 299);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Find Method";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tbMethodDetails_OriginalSignature);
            this.groupBox1.Controls.Add(this.lbMethodDetails_OriginalSignature);
            this.groupBox1.Controls.Add(this.tbMethodDetails_Signature);
            this.groupBox1.Controls.Add(this.lbMethodDetails_Signature);
            this.groupBox1.Controls.Add(this.tbMethodDetails_ReturnType);
            this.groupBox1.Controls.Add(this.lbMethodDetails_ReturnType);
            this.groupBox1.Controls.Add(this.tbMethodDetails_Parameters);
            this.groupBox1.Controls.Add(this.lbMethodDetails_Parameters);
            this.groupBox1.Controls.Add(this.tbMethodDetails_Name);
            this.groupBox1.Controls.Add(this.lbMethodDetails_Name);
            this.groupBox1.Controls.Add(this.tbMethodDetails_Type);
            this.groupBox1.Controls.Add(this.lbMethodDetails_Type);
            this.groupBox1.Location = new System.Drawing.Point(7, 236);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 60);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Method (select textbox to copy contents to clipboard)";
            // 
            // tbMethodDetails_OriginalSignature
            // 
            this.tbMethodDetails_OriginalSignature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMethodDetails_OriginalSignature.Location = new System.Drawing.Point(393, 37);
            this.tbMethodDetails_OriginalSignature.Name = "tbMethodDetails_OriginalSignature";
            this.tbMethodDetails_OriginalSignature.Size = new System.Drawing.Size(67, 20);
            this.tbMethodDetails_OriginalSignature.TabIndex = 12;
            this.tbMethodDetails_OriginalSignature.Enter += new System.EventHandler(this.tbMethodDetails_OriginalSignature_Enter);
            // 
            // lbMethodDetails_OriginalSignature
            // 
            this.lbMethodDetails_OriginalSignature.AutoSize = true;
            this.lbMethodDetails_OriginalSignature.Location = new System.Drawing.Point(324, 40);
            this.lbMethodDetails_OriginalSignature.Name = "lbMethodDetails_OriginalSignature";
            this.lbMethodDetails_OriginalSignature.Size = new System.Drawing.Size(63, 13);
            this.lbMethodDetails_OriginalSignature.TabIndex = 11;
            this.lbMethodDetails_OriginalSignature.Text = "Original Sig:";
            this.lbMethodDetails_OriginalSignature.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbMethodDetails_OriginalSignature_MouseDown);
            // 
            // tbMethodDetails_Signature
            // 
            this.tbMethodDetails_Signature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMethodDetails_Signature.Location = new System.Drawing.Point(393, 17);
            this.tbMethodDetails_Signature.Name = "tbMethodDetails_Signature";
            this.tbMethodDetails_Signature.Size = new System.Drawing.Size(67, 20);
            this.tbMethodDetails_Signature.TabIndex = 10;
            this.tbMethodDetails_Signature.Enter += new System.EventHandler(this.tbMethodDetails_Signature_Enter);
            // 
            // lbMethodDetails_Signature
            // 
            this.lbMethodDetails_Signature.AutoSize = true;
            this.lbMethodDetails_Signature.Location = new System.Drawing.Point(324, 20);
            this.lbMethodDetails_Signature.Name = "lbMethodDetails_Signature";
            this.lbMethodDetails_Signature.Size = new System.Drawing.Size(52, 13);
            this.lbMethodDetails_Signature.TabIndex = 9;
            this.lbMethodDetails_Signature.Text = "Signature";
            this.lbMethodDetails_Signature.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbMethodDetails_Signature_MouseDown);
            // 
            // tbMethodDetails_ReturnType
            // 
            this.tbMethodDetails_ReturnType.Location = new System.Drawing.Point(219, 37);
            this.tbMethodDetails_ReturnType.Name = "tbMethodDetails_ReturnType";
            this.tbMethodDetails_ReturnType.Size = new System.Drawing.Size(100, 20);
            this.tbMethodDetails_ReturnType.TabIndex = 8;
            this.tbMethodDetails_ReturnType.Enter += new System.EventHandler(this.tbMethodDetails_ReturnType_Enter);
            // 
            // lbMethodDetails_ReturnType
            // 
            this.lbMethodDetails_ReturnType.AutoSize = true;
            this.lbMethodDetails_ReturnType.Location = new System.Drawing.Point(148, 40);
            this.lbMethodDetails_ReturnType.Name = "lbMethodDetails_ReturnType";
            this.lbMethodDetails_ReturnType.Size = new System.Drawing.Size(69, 13);
            this.lbMethodDetails_ReturnType.TabIndex = 7;
            this.lbMethodDetails_ReturnType.Text = "Return Type:";
            this.lbMethodDetails_ReturnType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbMethodDetails_ReturnType_MouseDown);
            // 
            // tbMethodDetails_Parameters
            // 
            this.tbMethodDetails_Parameters.Location = new System.Drawing.Point(219, 17);
            this.tbMethodDetails_Parameters.Name = "tbMethodDetails_Parameters";
            this.tbMethodDetails_Parameters.Size = new System.Drawing.Size(100, 20);
            this.tbMethodDetails_Parameters.TabIndex = 6;
            this.tbMethodDetails_Parameters.Enter += new System.EventHandler(this.tbMethodDetails_Parameters_Enter);
            // 
            // lbMethodDetails_Parameters
            // 
            this.lbMethodDetails_Parameters.AutoSize = true;
            this.lbMethodDetails_Parameters.Location = new System.Drawing.Point(148, 20);
            this.lbMethodDetails_Parameters.Name = "lbMethodDetails_Parameters";
            this.lbMethodDetails_Parameters.Size = new System.Drawing.Size(63, 13);
            this.lbMethodDetails_Parameters.TabIndex = 5;
            this.lbMethodDetails_Parameters.Text = "Parameters:";
            this.lbMethodDetails_Parameters.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbMethodDetails_Parameters_MouseDown);
            // 
            // tbMethodDetails_Name
            // 
            this.tbMethodDetails_Name.Location = new System.Drawing.Point(47, 37);
            this.tbMethodDetails_Name.Name = "tbMethodDetails_Name";
            this.tbMethodDetails_Name.Size = new System.Drawing.Size(100, 20);
            this.tbMethodDetails_Name.TabIndex = 4;
            this.tbMethodDetails_Name.Enter += new System.EventHandler(this.tbMethodDetails_Name_Enter);
            // 
            // lbMethodDetails_Name
            // 
            this.lbMethodDetails_Name.AutoSize = true;
            this.lbMethodDetails_Name.Location = new System.Drawing.Point(7, 40);
            this.lbMethodDetails_Name.Name = "lbMethodDetails_Name";
            this.lbMethodDetails_Name.Size = new System.Drawing.Size(38, 13);
            this.lbMethodDetails_Name.TabIndex = 3;
            this.lbMethodDetails_Name.Text = "Name:";
            this.lbMethodDetails_Name.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbMethodDetails_Name_MouseDown);
            // 
            // tbMethodDetails_Type
            // 
            this.tbMethodDetails_Type.Location = new System.Drawing.Point(47, 17);
            this.tbMethodDetails_Type.Name = "tbMethodDetails_Type";
            this.tbMethodDetails_Type.Size = new System.Drawing.Size(100, 20);
            this.tbMethodDetails_Type.TabIndex = 2;
            this.tbMethodDetails_Type.Enter += new System.EventHandler(this.tbMethodDetails_Type_Enter);
            // 
            // lbMethodDetails_Type
            // 
            this.lbMethodDetails_Type.AutoSize = true;
            this.lbMethodDetails_Type.Location = new System.Drawing.Point(7, 20);
            this.lbMethodDetails_Type.Name = "lbMethodDetails_Type";
            this.lbMethodDetails_Type.Size = new System.Drawing.Size(34, 13);
            this.lbMethodDetails_Type.TabIndex = 0;
            this.lbMethodDetails_Type.Text = "Type:";
            this.lbMethodDetails_Type.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbMethodDetails_Type_MouseDown);
            // 
            // tbFilterBy_ReturnType
            // 
            this.tbFilterBy_ReturnType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterBy_ReturnType.Location = new System.Drawing.Point(376, 21);
            this.tbFilterBy_ReturnType.Name = "tbFilterBy_ReturnType";
            this.tbFilterBy_ReturnType.Size = new System.Drawing.Size(97, 20);
            this.tbFilterBy_ReturnType.TabIndex = 7;
            this.tbFilterBy_ReturnType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbFilterBy_ReturnType_KeyUp);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(376, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Return Type";
            // 
            // tbFilterBy_ParameterType
            // 
            this.tbFilterBy_ParameterType.Location = new System.Drawing.Point(270, 21);
            this.tbFilterBy_ParameterType.Name = "tbFilterBy_ParameterType";
            this.tbFilterBy_ParameterType.Size = new System.Drawing.Size(100, 20);
            this.tbFilterBy_ParameterType.TabIndex = 5;
            this.tbFilterBy_ParameterType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbFilterBy_ParameterType_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Parameter Type";
            // 
            // tbFilterBy_MethodName
            // 
            this.tbFilterBy_MethodName.Location = new System.Drawing.Point(109, 21);
            this.tbFilterBy_MethodName.Name = "tbFilterBy_MethodName";
            this.tbFilterBy_MethodName.Size = new System.Drawing.Size(155, 20);
            this.tbFilterBy_MethodName.TabIndex = 3;
            this.tbFilterBy_MethodName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbFilterBy_MethodName_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Method";
            // 
            // tbFilterBy_MethodType
            // 
            this.tbFilterBy_MethodType.Location = new System.Drawing.Point(4, 21);
            this.tbFilterBy_MethodType.Name = "tbFilterBy_MethodType";
            this.tbFilterBy_MethodType.Size = new System.Drawing.Size(100, 20);
            this.tbFilterBy_MethodType.TabIndex = 1;
            this.tbFilterBy_MethodType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbFilterBy_MethodType_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.functionsViewer);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(476, 299);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "All Methods & Classes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tvAssembliesLoaded);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(476, 299);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Assemblies Loaded";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tvAssembliesLoaded
            // 
            this.tvAssembliesLoaded.AllowDrop = true;
            this.tvAssembliesLoaded.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvAssembliesLoaded.Location = new System.Drawing.Point(3, 3);
            this.tvAssembliesLoaded.Name = "tvAssembliesLoaded";
            this.tvAssembliesLoaded.Size = new System.Drawing.Size(470, 293);
            this.tvAssembliesLoaded.TabIndex = 0;
            this.tvAssembliesLoaded.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvAssembliesLoaded_DragDrop);
            this.tvAssembliesLoaded.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvAssembliesLoaded_ItemDrag);
            this.tvAssembliesLoaded.DragOver += new System.Windows.Forms.DragEventHandler(this.tvAssembliesLoaded_DragOver);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.cbPerformRegExSearch);
            this.tabPage4.Controls.Add(this.cbHideCSharpGeneratedMethods);
            this.tabPage4.Controls.Add(this.llRefreshFunctionsViewer);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(476, 299);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "config";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tvExtraAssembliesToLoad);
            this.groupBox2.Location = new System.Drawing.Point(6, 79);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(464, 214);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Also load the following .NET Assemblies ";
            // 
            // tvExtraAssembliesToLoad
            // 
            this.tvExtraAssembliesToLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvExtraAssembliesToLoad.Location = new System.Drawing.Point(3, 16);
            this.tvExtraAssembliesToLoad.Name = "tvExtraAssembliesToLoad";
            this.tvExtraAssembliesToLoad.Size = new System.Drawing.Size(458, 195);
            this.tvExtraAssembliesToLoad.TabIndex = 5;
            this.tvExtraAssembliesToLoad.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvExtraAssembliesToLoad_ItemDrag);
            // 
            // cbPerformRegExSearch
            // 
            this.cbPerformRegExSearch.AutoSize = true;
            this.cbPerformRegExSearch.Checked = true;
            this.cbPerformRegExSearch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPerformRegExSearch.Location = new System.Drawing.Point(6, 32);
            this.cbPerformRegExSearch.Name = "cbPerformRegExSearch";
            this.cbPerformRegExSearch.Size = new System.Drawing.Size(241, 17);
            this.cbPerformRegExSearch.TabIndex = 4;
            this.cbPerformRegExSearch.Text = "Perform RegEx search (vs direct string match)";
            this.cbPerformRegExSearch.UseVisualStyleBackColor = true;
            // 
            // cbHideCSharpGeneratedMethods
            // 
            this.cbHideCSharpGeneratedMethods.AutoSize = true;
            this.cbHideCSharpGeneratedMethods.Checked = true;
            this.cbHideCSharpGeneratedMethods.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHideCSharpGeneratedMethods.Location = new System.Drawing.Point(6, 9);
            this.cbHideCSharpGeneratedMethods.Name = "cbHideCSharpGeneratedMethods";
            this.cbHideCSharpGeneratedMethods.Size = new System.Drawing.Size(162, 17);
            this.cbHideCSharpGeneratedMethods.TabIndex = 2;
            this.cbHideCSharpGeneratedMethods.Text = "Hide C# Generated Methods";
            this.cbHideCSharpGeneratedMethods.UseVisualStyleBackColor = true;
            this.cbHideCSharpGeneratedMethods.CheckedChanged += new System.EventHandler(this.cbHideCSharpGeneratedMethods_CheckedChanged);
            // 
            // llRefreshFunctionsViewer
            // 
            this.llRefreshFunctionsViewer.AutoSize = true;
            this.llRefreshFunctionsViewer.Location = new System.Drawing.Point(6, 59);
            this.llRefreshFunctionsViewer.Name = "llRefreshFunctionsViewer";
            this.llRefreshFunctionsViewer.Size = new System.Drawing.Size(178, 13);
            this.llRefreshFunctionsViewer.TabIndex = 1;
            this.llRefreshFunctionsViewer.TabStop = true;
            this.llRefreshFunctionsViewer.Text = "refresh (i.e. rebuild O2 Object model)";
            this.llRefreshFunctionsViewer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llRefreshFunctionsViewer_LinkClicked);
            // 
            // filteredFunctionsViewer
            // 
            this.filteredFunctionsViewer._AdvancedModeViews = false;
            this.filteredFunctionsViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filteredFunctionsViewer.BackColor = System.Drawing.SystemColors.Control;
            this.filteredFunctionsViewer.ForeColor = System.Drawing.Color.Black;
            this.filteredFunctionsViewer.Location = new System.Drawing.Point(3, 48);
            this.filteredFunctionsViewer.Name = "filteredFunctionsViewer";
            this.filteredFunctionsViewer.NamespaceDepthValue = 2;
            this.filteredFunctionsViewer.Size = new System.Drawing.Size(470, 189);
            this.filteredFunctionsViewer.TabIndex = 8;
            this.filteredFunctionsViewer._onItemDrag += new Callbacks.dMethod_Object(this.filteredFunctionsViewer__onItemDrag);
            this.filteredFunctionsViewer._onAfterSelect += new Callbacks.dMethod_Object(this.filteredFunctionsViewer__onAfterSelect);
            // 
            // functionsViewer
            // 
            this.functionsViewer._AdvancedModeViews = true;
            this.functionsViewer.BackColor = System.Drawing.SystemColors.Control;
            this.functionsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.functionsViewer.ForeColor = System.Drawing.Color.Black;
            this.functionsViewer.Location = new System.Drawing.Point(3, 3);
            this.functionsViewer.Name = "functionsViewer";
            this.functionsViewer.NamespaceDepthValue = 2;
            this.functionsViewer.Size = new System.Drawing.Size(470, 293);
            this.functionsViewer.TabIndex = 0;
            // 
            // O2ObjectModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcO2ObjectModel);
            this.Name = "O2ObjectModel";
            this.Size = new System.Drawing.Size(484, 325);
            this.Load += new System.EventHandler(this.ascx_O2ObjectModel_Load);
            this.tcO2ObjectModel.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcO2ObjectModel;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.ascx_FunctionsViewer functionsViewer;
        private System.Windows.Forms.LinkLabel llRefreshFunctionsViewer;
        private System.Windows.Forms.CheckBox cbHideCSharpGeneratedMethods;
        private System.Windows.Forms.TreeView tvAssembliesLoaded;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox tbFilterBy_ReturnType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbFilterBy_ParameterType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFilterBy_MethodName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFilterBy_MethodType;
        private System.Windows.Forms.Label label1;
        private Controls.ascx_FunctionsViewer filteredFunctionsViewer;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbMethodDetails_Type;
        private System.Windows.Forms.Label lbMethodDetails_Type;
        private System.Windows.Forms.TextBox tbMethodDetails_OriginalSignature;
        private System.Windows.Forms.Label lbMethodDetails_OriginalSignature;
        private System.Windows.Forms.TextBox tbMethodDetails_Signature;
        private System.Windows.Forms.Label lbMethodDetails_Signature;
        private System.Windows.Forms.TextBox tbMethodDetails_ReturnType;
        private System.Windows.Forms.Label lbMethodDetails_ReturnType;
        private System.Windows.Forms.TextBox tbMethodDetails_Parameters;
        private System.Windows.Forms.Label lbMethodDetails_Parameters;
        private System.Windows.Forms.TextBox tbMethodDetails_Name;
        private System.Windows.Forms.Label lbMethodDetails_Name;
        private System.Windows.Forms.CheckBox cbPerformRegExSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView tvExtraAssembliesToLoad;
    }
}
