// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.O2Findings;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_FindingEditor : UserControl
    {        
        public ascx_FindingEditor()
        {
            InitializeComponent();            
        }
    
        private void ascx_FindingEditor_Load(object sender, EventArgs e)
        {
            onLoad();
        }
                   
        private void llSaveAndDrag_MouseDown(object sender, MouseEventArgs e)
        {
            saveCurrentO2Finding();
            DoDragDrop(currentO2Finding, DragDropEffects.Copy);
        }

        private void llSaveAndDrag_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        private void llSaveAndDrag_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }

        private void btSaveChanges_Click(object sender, EventArgs e)
        {
            saveCurrentO2Finding();
            btSaveChangesToTrace.Visible = false;
        }

        private void dgvTraceDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {            
            if (cbAutoSaveChanges.Checked)
            {
                saveCurrentO2Trace();
            }
            else            
                btSaveChangesToTrace.Visible = true;                
        }        
    
        private void dgvFindingsDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cbAutoSaveChanges.Checked)
                saveCurrentO2Finding();
        }
    
        private void cbCurrentO2TraceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCellWithCurrentO2TraceText("traceType").Value = cbCurrentO2TraceType.Text;
            btSaveChangesToTrace.Visible = true;      
        }

        private void llRefreshCurrentO2Trace_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ascxTraceTreeView.showO2TraceTree();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void dgvFindingsDetails_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dgvFindingsDetails_DragDrop(object sender, DragEventArgs e)
        {
            handleDragDrop(e);
        }
       

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }
        

        private void cbAutoSaveChanges_CheckedChanged(object sender, EventArgs e)
        {
            updateVisibilityOfSaveButton();
        }
 
        private void btNewFinding_Click(object sender, EventArgs e)
        {
            newO2Finding();
        }

        private void cbMoveTraces_CheckedChanged(object sender, EventArgs e)
        {
            ascxTraceTreeView.bMoveTraces = cbMoveTraces.Checked;
        }

        private void btInsertNewTrace_Click(object sender, EventArgs e)
        {
            ascxTraceTreeView.insertNewO2Trace();
        }

        private void btAppendNewTRace_Click(object sender, EventArgs e)
        {
            ascxTraceTreeView.appendNewO2Trace();
        }

        private void btDeleteTraceNode_Click(object sender, EventArgs e)
        {
            if (ascxTraceTreeView.selectedNodeTag != null)
            {
                OzasmtGlue.deleteO2Trace((List<IO2Trace>) currentO2Finding.o2Traces, ascxTraceTreeView.o2Trace);
                ascxTraceTreeView.showO2TraceTree();
            }
        }

        private void btSaveChangesToTrace_Click(object sender, EventArgs e)
        {
            saveCurrentO2Trace();
        }

    }
}
