// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class DropObject
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
// ReSharper disable ConvertToConstant
// ReSharper disable RedundantDefaultFieldInitializer
        private System.ComponentModel.IContainer components = null;
// ReSharper restore RedundantDefaultFieldInitializer
// ReSharper restore ConvertToConstant

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
            this.label1 = new System.Windows.Forms.Label();
            this.lbDropObjectText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "dropObject";
            // 
            // lbDropObjectText
            // 
            this.lbDropObjectText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDropObjectText.Location = new System.Drawing.Point(0, 0);
            this.lbDropObjectText.Name = "lbDropObjectText";
            this.lbDropObjectText.Size = new System.Drawing.Size(136, 21);
            this.lbDropObjectText.TabIndex = 2;
            this.lbDropObjectText.Text = "Drop Content Here!!";
            this.lbDropObjectText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DropObject
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.Controls.Add(this.lbDropObjectText);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "DropObject";
            this.Size = new System.Drawing.Size(136, 21);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DropObject_DragDrop);
            this.DragLeave += new System.EventHandler(this.DropObject_DragLeave);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DropObject_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }        

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbDropObjectText;
    }
}
