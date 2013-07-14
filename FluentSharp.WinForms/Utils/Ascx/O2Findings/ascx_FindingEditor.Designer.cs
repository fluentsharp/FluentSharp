// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.WinForms.Controls
{
    partial class ascx_FindingEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
// ReSharper disable RedundantDefaultFieldInitializer
// ReSharper disable ConvertToConstant
        private System.ComponentModel.IContainer components = null;
// ReSharper restore ConvertToConstant
// ReSharper restore RedundantDefaultFieldInitializer

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ascx_FindingEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvFindingsDetails = new System.Windows.Forms.DataGridView();
            this.panelFindingDetails = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ascxTraceTreeView = new ascx_TraceTreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cbCurrentO2TraceType = new System.Windows.Forms.ToolStripComboBox();
            this.btSaveChangesToTrace = new System.Windows.Forms.ToolStripButton();
            this.btInsertNewTrace = new System.Windows.Forms.ToolStripButton();
            this.btAppendNewTRace = new System.Windows.Forms.ToolStripButton();
            this.btDeleteTraceNode = new System.Windows.Forms.ToolStripButton();
            this.dgvTraceDetails = new System.Windows.Forms.DataGridView();
            this.cbMoveTraces = new System.Windows.Forms.CheckBox();
            this.llRefreshCurrentO2Trace = new System.Windows.Forms.LinkLabel();
            this.btSaveChanges = new System.Windows.Forms.Button();
            this.llSaveAndDrag = new System.Windows.Forms.Label();
            this.cbAutoSaveChanges = new System.Windows.Forms.CheckBox();
            this.btNewFinding = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.dgvFindingsDetails)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.dgvTraceDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(0, 31);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvFindingsDetails);
            this.splitContainer1.Panel1.Controls.Add(this.panelFindingDetails);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(554, 402);
            this.splitContainer1.SplitterDistance = 248;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvFindingsDetails
            // 
            this.dgvFindingsDetails.AllowDrop = true;
            this.dgvFindingsDetails.AllowUserToAddRows = false;
            this.dgvFindingsDetails.AllowUserToDeleteRows = false;
            this.dgvFindingsDetails.AllowUserToOrderColumns = true;
            this.dgvFindingsDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFindingsDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFindingsDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvFindingsDetails.Location = new System.Drawing.Point(0, 0);
            this.dgvFindingsDetails.Name = "dgvFindingsDetails";
            this.dgvFindingsDetails.Size = new System.Drawing.Size(244, 398);
            this.dgvFindingsDetails.TabIndex = 2;
            this.dgvFindingsDetails.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFindingsDetails_CellValueChanged);
            this.dgvFindingsDetails.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvFindingsDetails_DragEnter);
            this.dgvFindingsDetails.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvFindingsDetails_DragDrop);
            // 
            // panelFindingDetails
            // 
            this.panelFindingDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFindingDetails.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panelFindingDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFindingDetails.Location = new System.Drawing.Point(1, 20);
            this.panelFindingDetails.Name = "panelFindingDetails";
            this.panelFindingDetails.Size = new System.Drawing.Size(241, 371);
            this.panelFindingDetails.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ascxTraceTreeView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer2.Panel2.Controls.Add(this.dgvTraceDetails);
            this.splitContainer2.Panel2.Controls.Add(this.cbMoveTraces);
            this.splitContainer2.Panel2MinSize = 150;
            this.splitContainer2.Size = new System.Drawing.Size(302, 402);
            this.splitContainer2.SplitterDistance = 242;
            this.splitContainer2.TabIndex = 0;
            // 
            // ascxTraceTreeView
            // 
            this.ascxTraceTreeView.bMoveTraces = true;
            this.ascxTraceTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ascxTraceTreeView.Location = new System.Drawing.Point(0, 0);
            this.ascxTraceTreeView.Name = "ascxTraceTreeView";
            this.ascxTraceTreeView.o2Finding = null;
            this.ascxTraceTreeView.o2Trace = null;
            this.ascxTraceTreeView.selectedNode = null;
            this.ascxTraceTreeView.selectedNodeTag = null;
            this.ascxTraceTreeView.Size = new System.Drawing.Size(298, 238);
            this.ascxTraceTreeView.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbCurrentO2TraceType,
            this.btSaveChangesToTrace,
            this.btInsertNewTrace,
            this.btAppendNewTRace,
            this.btDeleteTraceNode});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(298, 25);
            this.toolStrip1.TabIndex = 28;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cbCurrentO2TraceType
            // 
            this.cbCurrentO2TraceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrentO2TraceType.Name = "cbCurrentO2TraceType";
            this.cbCurrentO2TraceType.Size = new System.Drawing.Size(80, 25);
            this.cbCurrentO2TraceType.SelectedIndexChanged += new System.EventHandler(this.cbCurrentO2TraceType_SelectedIndexChanged);
            // 
            // btSaveChangesToTrace
            // 
            this.btSaveChangesToTrace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSaveChangesToTrace.Image = ((System.Drawing.Image)(resources.GetObject("btSaveChangesToTrace.Image")));
            this.btSaveChangesToTrace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSaveChangesToTrace.Name = "btSaveChangesToTrace";
            this.btSaveChangesToTrace.Size = new System.Drawing.Size(23, 22);
            this.btSaveChangesToTrace.Text = "Save Changes To Trace Details";
            this.btSaveChangesToTrace.Click += new System.EventHandler(this.btSaveChangesToTrace_Click);
            // 
            // btInsertNewTrace
            // 
            this.btInsertNewTrace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btInsertNewTrace.Image = ((System.Drawing.Image)(resources.GetObject("btInsertNewTrace.Image")));
            this.btInsertNewTrace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btInsertNewTrace.Name = "btInsertNewTrace";
            this.btInsertNewTrace.Size = new System.Drawing.Size(23, 22);
            this.btInsertNewTrace.Text = "InsertNewTrace";
            this.btInsertNewTrace.Click += new System.EventHandler(this.btInsertNewTrace_Click);
            // 
            // btAppendNewTRace
            // 
            this.btAppendNewTRace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAppendNewTRace.Image = ((System.Drawing.Image)(resources.GetObject("btAppendNewTRace.Image")));
            this.btAppendNewTRace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAppendNewTRace.Name = "btAppendNewTRace";
            this.btAppendNewTRace.Size = new System.Drawing.Size(23, 22);
            this.btAppendNewTRace.Text = "toolStripButton1";
            this.btAppendNewTRace.Click += new System.EventHandler(this.btAppendNewTRace_Click);
            // 
            // btDeleteTraceNode
            // 
            this.btDeleteTraceNode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btDeleteTraceNode.Image = ((System.Drawing.Image)(resources.GetObject("btDeleteTraceNode.Image")));
            this.btDeleteTraceNode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDeleteTraceNode.Name = "btDeleteTraceNode";
            this.btDeleteTraceNode.Size = new System.Drawing.Size(23, 22);
            this.btDeleteTraceNode.Text = "Delete Trace Node";
            this.btDeleteTraceNode.Click += new System.EventHandler(this.btDeleteTraceNode_Click);
            // 
            // dgvTraceDetails
            // 
            this.dgvTraceDetails.AllowUserToAddRows = false;
            this.dgvTraceDetails.AllowUserToDeleteRows = false;
            this.dgvTraceDetails.AllowUserToOrderColumns = true;
            this.dgvTraceDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTraceDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTraceDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvTraceDetails.Location = new System.Drawing.Point(3, 30);
            this.dgvTraceDetails.Name = "dgvTraceDetails";
            this.dgvTraceDetails.Size = new System.Drawing.Size(292, 99);
            this.dgvTraceDetails.TabIndex = 3;
            this.dgvTraceDetails.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTraceDetails_CellValueChanged);
            // 
            // cbMoveTraces
            // 
            this.cbMoveTraces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbMoveTraces.AutoSize = true;
            this.cbMoveTraces.Checked = true;
            this.cbMoveTraces.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMoveTraces.Location = new System.Drawing.Point(3, 134);
            this.cbMoveTraces.Name = "cbMoveTraces";
            this.cbMoveTraces.Size = new System.Drawing.Size(174, 17);
            this.cbMoveTraces.TabIndex = 26;
            this.cbMoveTraces.Text = "Move Traces (same trace drop)";
            this.cbMoveTraces.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.cbMoveTraces.UseVisualStyleBackColor = true;
            this.cbMoveTraces.CheckedChanged += new System.EventHandler(this.cbMoveTraces_CheckedChanged);
            // 
            // llRefreshCurrentO2Trace
            // 
            this.llRefreshCurrentO2Trace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llRefreshCurrentO2Trace.AutoSize = true;
            this.llRefreshCurrentO2Trace.Location = new System.Drawing.Point(420, 9);
            this.llRefreshCurrentO2Trace.Name = "llRefreshCurrentO2Trace";
            this.llRefreshCurrentO2Trace.Size = new System.Drawing.Size(39, 13);
            this.llRefreshCurrentO2Trace.TabIndex = 24;
            this.llRefreshCurrentO2Trace.TabStop = true;
            this.llRefreshCurrentO2Trace.Text = "refresh";
            this.llRefreshCurrentO2Trace.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llRefreshCurrentO2Trace_LinkClicked);
            // 
            // btSaveChanges
            // 
            this.btSaveChanges.Location = new System.Drawing.Point(3, 4);
            this.btSaveChanges.Name = "btSaveChanges";
            this.btSaveChanges.Size = new System.Drawing.Size(89, 23);
            this.btSaveChanges.TabIndex = 2;
            this.btSaveChanges.Text = "Save Changes";
            this.btSaveChanges.UseVisualStyleBackColor = true;
            this.btSaveChanges.Click += new System.EventHandler(this.btSaveChanges_Click);
            // 
            // llSaveAndDrag
            // 
            this.llSaveAndDrag.AutoSize = true;
            this.llSaveAndDrag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llSaveAndDrag.ForeColor = System.Drawing.Color.Blue;
            this.llSaveAndDrag.Location = new System.Drawing.Point(98, 9);
            this.llSaveAndDrag.Name = "llSaveAndDrag";
            this.llSaveAndDrag.Size = new System.Drawing.Size(75, 13);
            this.llSaveAndDrag.TabIndex = 4;
            this.llSaveAndDrag.Text = "save and drag";
            this.llSaveAndDrag.MouseLeave += new System.EventHandler(this.llSaveAndDrag_MouseLeave);
            this.llSaveAndDrag.MouseDown += new System.Windows.Forms.MouseEventHandler(this.llSaveAndDrag_MouseDown);
            this.llSaveAndDrag.MouseHover += new System.EventHandler(this.llSaveAndDrag_MouseHover);
            // 
            // cbAutoSaveChanges
            // 
            this.cbAutoSaveChanges.AutoSize = true;
            this.cbAutoSaveChanges.Location = new System.Drawing.Point(179, 9);
            this.cbAutoSaveChanges.Name = "cbAutoSaveChanges";
            this.cbAutoSaveChanges.Size = new System.Drawing.Size(180, 17);
            this.cbAutoSaveChanges.TabIndex = 5;
            this.cbAutoSaveChanges.Text = "Auto Save (on content changes)";
            this.cbAutoSaveChanges.UseVisualStyleBackColor = true;
            this.cbAutoSaveChanges.CheckedChanged += new System.EventHandler(this.cbAutoSaveChanges_CheckedChanged);
            // 
            // btNewFinding
            // 
            this.btNewFinding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btNewFinding.Location = new System.Drawing.Point(465, 3);
            this.btNewFinding.Name = "btNewFinding";
            this.btNewFinding.Size = new System.Drawing.Size(89, 23);
            this.btNewFinding.TabIndex = 6;
            this.btNewFinding.Text = "New Finding";
            this.btNewFinding.UseVisualStyleBackColor = true;
            this.btNewFinding.Click += new System.EventHandler(this.btNewFinding_Click);
            // 
            // ascx_FindingEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btNewFinding);
            this.Controls.Add(this.cbAutoSaveChanges);
            this.Controls.Add(this.llSaveAndDrag);
            this.Controls.Add(this.btSaveChanges);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.llRefreshCurrentO2Trace);
            this.Name = "ascx_FindingEditor";
            this.Size = new System.Drawing.Size(558, 436);
            this.Load += new System.EventHandler(this.ascx_FindingEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            //((System.ComponentModel.ISupportInitialize)(this.dgvFindingsDetails)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            //((System.ComponentModel.ISupportInitialize)(this.dgvTraceDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvFindingsDetails;
        private System.Windows.Forms.DataGridView dgvTraceDetails;
        private System.Windows.Forms.Button btSaveChanges;
        private System.Windows.Forms.Label llSaveAndDrag;
        private System.Windows.Forms.Panel panelFindingDetails;
        private System.Windows.Forms.CheckBox cbAutoSaveChanges;
        private System.Windows.Forms.LinkLabel llRefreshCurrentO2Trace;
        private System.Windows.Forms.CheckBox cbMoveTraces;
        private System.Windows.Forms.Button btNewFinding;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox cbCurrentO2TraceType;
        private ascx_TraceTreeView ascxTraceTreeView;
        private System.Windows.Forms.ToolStripButton btInsertNewTrace;
        private System.Windows.Forms.ToolStripButton btAppendNewTRace;
        private System.Windows.Forms.ToolStripButton btDeleteTraceNode;
        private System.Windows.Forms.ToolStripButton btSaveChangesToTrace;
    }
}
