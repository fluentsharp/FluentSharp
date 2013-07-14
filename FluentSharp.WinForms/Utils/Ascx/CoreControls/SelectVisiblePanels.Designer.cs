// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class SelectVisiblePanels
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
            this.gbVisibleControls = new System.Windows.Forms.GroupBox();
            this.cb_View_4 = new System.Windows.Forms.CheckBox();
            this.cb_View_3 = new System.Windows.Forms.CheckBox();
            this.cb_View_2 = new System.Windows.Forms.CheckBox();
            this.cb_View_1 = new System.Windows.Forms.CheckBox();
            this.gbVisibleControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbVisibleControls
            // 
            this.gbVisibleControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                                  | System.Windows.Forms.AnchorStyles.Right)));
            this.gbVisibleControls.BackColor = System.Drawing.Color.Transparent;
            this.gbVisibleControls.Controls.Add(this.cb_View_4);
            this.gbVisibleControls.Controls.Add(this.cb_View_3);
            this.gbVisibleControls.Controls.Add(this.cb_View_2);
            this.gbVisibleControls.Controls.Add(this.cb_View_1);
            this.gbVisibleControls.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbVisibleControls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gbVisibleControls.Location = new System.Drawing.Point(1, 3);
            this.gbVisibleControls.Name = "gbVisibleControls";
            this.gbVisibleControls.Size = new System.Drawing.Size(567, 35);
            this.gbVisibleControls.TabIndex = 101;
            this.gbVisibleControls.TabStop = false;
            this.gbVisibleControls.Enter += new System.EventHandler(this.gbVisibleControls_Enter);
            // 
            // cb_View_4
            // 
            this.cb_View_4.AutoSize = true;
            this.cb_View_4.Location = new System.Drawing.Point(408, 14);
            this.cb_View_4.Name = "cb_View_4";
            this.cb_View_4.Size = new System.Drawing.Size(32, 17);
            this.cb_View_4.TabIndex = 97;
            this.cb_View_4.Text = "4";
            this.cb_View_4.UseVisualStyleBackColor = true;
            this.cb_View_4.CheckedChanged += new System.EventHandler(this.cb_View_4_CheckedChanged);
            // 
            // cb_View_3
            // 
            this.cb_View_3.AutoSize = true;
            this.cb_View_3.Location = new System.Drawing.Point(276, 14);
            this.cb_View_3.Name = "cb_View_3";
            this.cb_View_3.Size = new System.Drawing.Size(32, 17);
            this.cb_View_3.TabIndex = 96;
            this.cb_View_3.Text = "3";
            this.cb_View_3.UseVisualStyleBackColor = true;
            this.cb_View_3.CheckedChanged += new System.EventHandler(this.cb_View_3_CheckedChanged);
            // 
            // cb_View_2
            // 
            this.cb_View_2.AutoSize = true;
            this.cb_View_2.Location = new System.Drawing.Point(144, 14);
            this.cb_View_2.Name = "cb_View_2";
            this.cb_View_2.Size = new System.Drawing.Size(32, 17);
            this.cb_View_2.TabIndex = 94;
            this.cb_View_2.Text = "2";
            this.cb_View_2.UseVisualStyleBackColor = true;
            this.cb_View_2.CheckedChanged += new System.EventHandler(this.cb_View_2_CheckedChanged);
            // 
            // cb_View_1
            // 
            this.cb_View_1.AutoSize = true;
            this.cb_View_1.Checked = true;
            this.cb_View_1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_View_1.Location = new System.Drawing.Point(12, 14);
            this.cb_View_1.Name = "cb_View_1";
            this.cb_View_1.Size = new System.Drawing.Size(32, 17);
            this.cb_View_1.TabIndex = 95;
            this.cb_View_1.Text = "1";
            this.cb_View_1.UseVisualStyleBackColor = true;
            this.cb_View_1.CheckedChanged += new System.EventHandler(this.cb_View_1_CheckedChanged);
            // 
            // SelectVisiblePanels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.gbVisibleControls);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "SelectVisiblePanels";
            this.Size = new System.Drawing.Size(570, 40);
            this.Load += new System.EventHandler(this.ascx_SelectVisiblePanels_Load);
            this.gbVisibleControls.ResumeLayout(false);
            this.gbVisibleControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbVisibleControls;
        private System.Windows.Forms.CheckBox cb_View_1;
        private System.Windows.Forms.CheckBox cb_View_3;
        private System.Windows.Forms.CheckBox cb_View_4;
        private System.Windows.Forms.CheckBox cb_View_2;
    }
}
