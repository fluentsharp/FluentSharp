// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class ctrl_TableList
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
            this.lvData = new System.Windows.Forms.ListView();
            this.lbTableListTitle = new System.Windows.Forms.Label();
            this.llMakeColumnWithMatchCellWidth = new System.Windows.Forms.LinkLabel();
            this.llClearLoadedItems = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lvData
            // 
            this.lvData.AllowColumnReorder = true;
            this.lvData.AllowDrop = true;
            this.lvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvData.FullRowSelect = true;
            this.lvData.GridLines = true;
            this.lvData.HideSelection = false;
            this.lvData.Location = new System.Drawing.Point(0, 22);
            this.lvData.Name = "lvData";
            this.lvData.Size = new System.Drawing.Size(697, 188);
            //this.lvData.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvData.TabIndex = 0;
            this.lvData.UseCompatibleStateImageBehavior = false;
            this.lvData.View = System.Windows.Forms.View.Details;
            this.lvData.Resize += new System.EventHandler(this.lvData_Resize);
            this.lvData.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvData_DragDrop);
            this.lvData.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvData_DragEnter);
            // 
            // lbTableListTitle
            // 
            this.lbTableListTitle.AutoSize = true;
            this.lbTableListTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTableListTitle.Location = new System.Drawing.Point(4, 4);
            this.lbTableListTitle.Name = "lbTableListTitle";
            this.lbTableListTitle.Size = new System.Drawing.Size(63, 13);
            this.lbTableListTitle.TabIndex = 1;
            this.lbTableListTitle.Text = "Table List";
            // 
            // llMakeColumnWithMatchCellWidth
            // 
            this.llMakeColumnWithMatchCellWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llMakeColumnWithMatchCellWidth.AutoSize = true;
            this.llMakeColumnWithMatchCellWidth.Location = new System.Drawing.Point(575, 5);
            this.llMakeColumnWithMatchCellWidth.Name = "llMakeColumnWithMatchCellWidth";
            this.llMakeColumnWithMatchCellWidth.Size = new System.Drawing.Size(83, 13);
            this.llMakeColumnWithMatchCellWidth.TabIndex = 2;
            this.llMakeColumnWithMatchCellWidth.TabStop = true;
            this.llMakeColumnWithMatchCellWidth.Text = "match cell width";
            this.llMakeColumnWithMatchCellWidth.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llMakeColumnWithMatchCellWidth_LinkClicked);
            // 
            // llClearLoadedItems
            // 
            this.llClearLoadedItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llClearLoadedItems.AutoSize = true;
            this.llClearLoadedItems.Location = new System.Drawing.Point(664, 5);
            this.llClearLoadedItems.Name = "llClearLoadedItems";
            this.llClearLoadedItems.Size = new System.Drawing.Size(30, 13);
            this.llClearLoadedItems.TabIndex = 3;
            this.llClearLoadedItems.TabStop = true;
            this.llClearLoadedItems.Text = "clear";
            this.llClearLoadedItems.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llClearLoadedItems_LinkClicked);
            // 
            // ascx_TableList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.llClearLoadedItems);
            this.Controls.Add(this.llMakeColumnWithMatchCellWidth);
            this.Controls.Add(this.lbTableListTitle);
            this.Controls.Add(this.lvData);
            this.Name = "ascx_TableList";
            this.Size = new System.Drawing.Size(697, 210);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvData;
        private System.Windows.Forms.Label lbTableListTitle;
        private System.Windows.Forms.LinkLabel llMakeColumnWithMatchCellWidth;
        private System.Windows.Forms.LinkLabel llClearLoadedItems;
    }
}
