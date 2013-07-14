// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.O2Findings;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_TraceTreeView : UserControl
    {
        public ascx_TraceTreeView()
        {
            InitializeComponent();
        }
        private void tvSmartTrace_DragDrop(object sender, DragEventArgs e)
        {
            handleDragDrop(e);
        }

        private void tvSmartTrace_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void tvSmartTrace_ItemDrag(object sender, ItemDragEventArgs e)
        {
            tvSmartTrace.SelectedNode = (TreeNode)e.Item;
            Application.DoEvents();
            DoDragDrop(tvSmartTrace.SelectedNode.Tag, DragDropEffects.Copy);
        }

        private void tvSmartTrace_DragOver(object sender, DragEventArgs e)
        {
            O2Forms.selectTreeNodeAtDroppedOverPoint(tvSmartTrace, e.X, e.Y);
        }

        private void tvSmartTrace_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && tvSmartTrace.SelectedNode != null)
            {
                OzasmtGlue.deleteO2Trace(o2Finding.o2Traces, (IO2Trace)tvSmartTrace.SelectedNode.Tag);
                showO2TraceTree();
            }
        }

        private void tvSmartTrace_KeyPress(object sender, KeyPressEventArgs e)
        {
            //DataGridViewCell cellWithCurrentO2TraceSignatureText = getCellWithCurrentO2TraceSignatureText();
            // var signature = cellWithCurrentO2TraceSignatureText.Value.ToString();

            switch (e.KeyChar)
            {
                case (char)8: // del key

                    return;
                    //if (signature.Length > 0)
                    //   signature = signature.Substring(0, signature.Length - 1);                    
                case '\r':
                    appendNewO2Trace();
                    break;
                default:
                    //signature += e.KeyChar.ToString();

                    // tvSmartTrace.SelectedNode.Text = signature;
                    if (tvSmartTrace.SelectedNode!=null)
                        tvSmartTrace.SelectedNode.BeginEdit();
                    break;
            }

            //cellWithCurrentO2TraceSignatureText.Value = signature;
            //e.Handled = true;   
        }

        private void tvSmartTrace_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            Callbacks.raiseRegistedCallbacks(onTreeNodeAfterLabelEdit, new[] {e.Label});            
        }

        private void tvSmartTrace_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
        }

        private void tvSmartTrace_AfterSelect(object sender, TreeViewEventArgs e)
        {
            onTreeViewAfterSelect(cbShowLineInSourceFile.Checked);
        }
        

        private void ascx_TraceTreeView_Load(object sender, EventArgs e)
        {
            onLoad();
        }

        private void rbSmartTraceFilter_MethodName_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSmartTraceFilter_MethodName.Checked)
                tracePropertyToUseAsNodeText = "signature";
            showO2TraceTree();
        }

        private void rbSmartTraceFilter_Context_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSmartTraceFilter_Context.Checked)
                tracePropertyToUseAsNodeText = "context";
            showO2TraceTree();
        }

        private void rbSmartTraceFilter_SourceCode_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSmartTraceFilter_SourceCode.Checked)
                tracePropertyToUseAsNodeText = "SourceCode";
            showO2TraceTree();
        }
        
    }
}
