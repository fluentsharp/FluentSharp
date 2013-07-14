// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class ascx_TraceTreeView
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
            this.tvSmartTrace = new System.Windows.Forms.TreeView();
            this.rbSmartTraceFilter_SourceCode = new System.Windows.Forms.RadioButton();
            this.rbSmartTraceFilter_MethodName = new System.Windows.Forms.RadioButton();
            this.rbSmartTraceFilter_Context = new System.Windows.Forms.RadioButton();
            this.cbShowLineInSourceFile = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tvSmartTrace
            // 
            this.tvSmartTrace.AllowDrop = true;
            this.tvSmartTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvSmartTrace.HideSelection = false;
            this.tvSmartTrace.LabelEdit = true;
            this.tvSmartTrace.Location = new System.Drawing.Point(0, 27);
            this.tvSmartTrace.Name = "tvSmartTrace";
            this.tvSmartTrace.Size = new System.Drawing.Size(439, 359);
            this.tvSmartTrace.TabIndex = 24;
            this.tvSmartTrace.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvSmartTrace_AfterLabelEdit);
            this.tvSmartTrace.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvSmartTrace_DragDrop);
            this.tvSmartTrace.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSmartTrace_AfterSelect);
            this.tvSmartTrace.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvSmartTrace_DragEnter);
            this.tvSmartTrace.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvSmartTrace_KeyPress);
            this.tvSmartTrace.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvSmartTrace_KeyUp);
            this.tvSmartTrace.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvSmartTrace_BeforeLabelEdit);
            this.tvSmartTrace.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvSmartTrace_ItemDrag);
            this.tvSmartTrace.DragOver += new System.Windows.Forms.DragEventHandler(this.tvSmartTrace_DragOver);
            // 
            // rbSmartTraceFilter_SourceCode
            // 
            this.rbSmartTraceFilter_SourceCode.AutoSize = true;
            this.rbSmartTraceFilter_SourceCode.Location = new System.Drawing.Point(171, 3);
            this.rbSmartTraceFilter_SourceCode.Name = "rbSmartTraceFilter_SourceCode";
            this.rbSmartTraceFilter_SourceCode.Size = new System.Drawing.Size(87, 17);
            this.rbSmartTraceFilter_SourceCode.TabIndex = 29;
            this.rbSmartTraceFilter_SourceCode.TabStop = true;
            this.rbSmartTraceFilter_SourceCode.Text = "Source Code";
            this.rbSmartTraceFilter_SourceCode.UseVisualStyleBackColor = true;
            this.rbSmartTraceFilter_SourceCode.CheckedChanged += new System.EventHandler(this.rbSmartTraceFilter_SourceCode_CheckedChanged);
            // 
            // rbSmartTraceFilter_MethodName
            // 
            this.rbSmartTraceFilter_MethodName.AutoSize = true;
            this.rbSmartTraceFilter_MethodName.Checked = true;
            this.rbSmartTraceFilter_MethodName.Location = new System.Drawing.Point(3, 3);
            this.rbSmartTraceFilter_MethodName.Name = "rbSmartTraceFilter_MethodName";
            this.rbSmartTraceFilter_MethodName.Size = new System.Drawing.Size(92, 17);
            this.rbSmartTraceFilter_MethodName.TabIndex = 28;
            this.rbSmartTraceFilter_MethodName.TabStop = true;
            this.rbSmartTraceFilter_MethodName.Text = "Method Name";
            this.rbSmartTraceFilter_MethodName.UseVisualStyleBackColor = true;
            this.rbSmartTraceFilter_MethodName.CheckedChanged += new System.EventHandler(this.rbSmartTraceFilter_MethodName_CheckedChanged);
            // 
            // rbSmartTraceFilter_Context
            // 
            this.rbSmartTraceFilter_Context.AutoSize = true;
            this.rbSmartTraceFilter_Context.Location = new System.Drawing.Point(101, 3);
            this.rbSmartTraceFilter_Context.Name = "rbSmartTraceFilter_Context";
            this.rbSmartTraceFilter_Context.Size = new System.Drawing.Size(61, 17);
            this.rbSmartTraceFilter_Context.TabIndex = 27;
            this.rbSmartTraceFilter_Context.Text = "Context";
            this.rbSmartTraceFilter_Context.UseVisualStyleBackColor = true;
            this.rbSmartTraceFilter_Context.CheckedChanged += new System.EventHandler(this.rbSmartTraceFilter_Context_CheckedChanged);
            // 
            // cbShowLineInSourceFile
            // 
            this.cbShowLineInSourceFile.AutoSize = true;
            this.cbShowLineInSourceFile.Location = new System.Drawing.Point(274, 4);
            this.cbShowLineInSourceFile.Name = "cbShowLineInSourceFile";
            this.cbShowLineInSourceFile.Size = new System.Drawing.Size(179, 17);
            this.cbShowLineInSourceFile.TabIndex = 30;
            this.cbShowLineInSourceFile.Text = "Show Trace in Source Code File";
            this.cbShowLineInSourceFile.UseVisualStyleBackColor = true;
            // 
            // ascx_TraceTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbShowLineInSourceFile);
            this.Controls.Add(this.rbSmartTraceFilter_SourceCode);
            this.Controls.Add(this.rbSmartTraceFilter_MethodName);
            this.Controls.Add(this.rbSmartTraceFilter_Context);
            this.Controls.Add(this.tvSmartTrace);
            this.Name = "ascx_TraceTreeView";
            this.Size = new System.Drawing.Size(439, 388);
            this.Load += new System.EventHandler(this.ascx_TraceTreeView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvSmartTrace;
        private System.Windows.Forms.RadioButton rbSmartTraceFilter_SourceCode;
        private System.Windows.Forms.RadioButton rbSmartTraceFilter_MethodName;
        private System.Windows.Forms.RadioButton rbSmartTraceFilter_Context;
        private System.Windows.Forms.CheckBox cbShowLineInSourceFile;
    }
}
