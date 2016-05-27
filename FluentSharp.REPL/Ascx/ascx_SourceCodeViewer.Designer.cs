namespace FluentSharp.REPL.Controls
{
    partial class ascx_SourceCodeViewer
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
            this.sourceCodeEditor = new FluentSharp.REPL.Controls.SourceCodeEditor();
            this.SuspendLayout();
            // 
            // sourceCodeEditor
            // 
            this.sourceCodeEditor._ShowSearchAndAstDetails = false;
            this.sourceCodeEditor._ShowTopMenu = false;
            this.sourceCodeEditor.AllowDrop = true;
            this.sourceCodeEditor.BackColor = System.Drawing.SystemColors.Control;
            this.sourceCodeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceCodeEditor.ForeColor = System.Drawing.Color.Black;
            this.sourceCodeEditor.Location = new System.Drawing.Point(0, 0);
            this.sourceCodeEditor.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.sourceCodeEditor.Name = "sourceCodeEditor";
            this.sourceCodeEditor.Size = new System.Drawing.Size(1276, 516);
            this.sourceCodeEditor.TabIndex = 0;
            // 
            // ascx_SourceCodeViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sourceCodeEditor);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ascx_SourceCodeViewer";
            this.Size = new System.Drawing.Size(1276, 516);
            this.ResumeLayout(false);

        }

        #endregion

        private SourceCodeEditor sourceCodeEditor;

    }
}
