// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_FunctionsViewer : UserControl
    {
        #region ViewMode enum
        
        public enum ViewDisplayControl
        {
            TreeView,
            ListBox,
            TextBox
        }

        public enum ViewMode
        {            
            byFunctionSignature,
            byProvidedSplitChar
        }

        #endregion
        

     

        public ascx_FunctionsViewer()
        {
            NamespaceDepthValue = 2;
            InitializeComponent();
            onInitialize();
        }
        

        private void ascx_FunctionsViewer_Load(object sender, EventArgs e)
        {
            onLoad();
        }        

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
                showView();
        }

     

        /*private void lBoxListBox_Resize(object sender, EventArgs e)
        {
        }

        private void tvTreeView_Resize(object sender, EventArgs e)
        {
            // tvTreeView.Width = lBoxListBox.Width;
            // tvTreeView.Height = lBoxListBox.Height;
        }*/              

        private void tvTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (raiseOnAfterTreeViewCheck)
                onAfterTreeViewCheck(e.Node, ref bRecursivelyCheckingAllSubNodes, tvTreeView, eNodeEvent_CheckClickEvent);
        }


        private void tvTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tvTreeView.SelectedNode = e.Node;            
            onAfterSelectTreeView(tvTreeView.SelectedNode);
            tvTreeView.Focus();
        }

        

        public TreeView getObject_TreeView()
        {
            return tvTreeView;
        }

        private void tbMaxItemsToShow_TextChanged(object sender, EventArgs e)
        {
            int iNewValue;
            if (Int32.TryParse(tbMaxItemsToShow.Text, out iNewValue))
            {
                iMaxItemsToShow = iNewValue;
                tbMaxItemsToShow.BackColor = Color.White;
                showView();
            }
            else
                tbMaxItemsToShow.BackColor = Color.LightPink;
        }      

        private void colapseAll_Click(object sender, EventArgs e)
        {
            tvTreeView.CollapseAll();
            collapseAll.Enabled = false;
            expandAll.Enabled = true;
        }

        private void expandAll_Click(object sender, EventArgs e)
        {
            tvTreeView.ExpandAll();
            collapseAll.Enabled = true;
            expandAll.Enabled = false;
        }

        private void typeOfControlToShowData_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (typeOfControlToShowData.Text)
            {
                case "TreeView":
                    viewDisplayControl = ViewDisplayControl.TreeView;
                    break;
                case "TextBox":
                    viewDisplayControl = ViewDisplayControl.TextBox;
                    break;
                case "ListBox":
                    viewDisplayControl = ViewDisplayControl.ListBox;
                    break;
            }
            showView();
        }

        private void checkAll_Click(object sender, EventArgs e)
        {
            setTreeViewCheckBoxValue(tvTreeView, true);

        }

        private void uncheckAll_Click(object sender, EventArgs e)
        {
            setTreeViewCheckBoxValue(tvTreeView, false);
        }       

        private void namespaceDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            NamespaceDepthValue = namespaceDepth.SelectedIndex - 1;  // since we have that -1 special value for the nameSpace
            showView();
        }

        private void showConfig_Click(object sender, EventArgs e)
        {
            showHideConfig();
        }

        private void showConfigButton_Click(object sender, EventArgs e)
        {
            showHideConfig();
        }

        private void llHideConfig_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showHideConfig();
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
       //     showView();
        }

        private void tvTreeView_DoubleClick(object sender, EventArgs e)
        {
            onDoubleClickTreeView(tvTreeView.SelectedNode); 
        }

        private void tvTreeView_DragDrop(object sender, DragEventArgs e)
        {
            handleDrop(e);
        }        

        private void tvTreeView_DragEnter(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }

        private void tvTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            onTreeViewBeforeExpand(e.Node);
        }

        private void tvTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            onTreeViewItemDrag(tvTreeView,e);
                
        }
        
        private void llShowCheckboxes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tvTreeView.CheckBoxes = true;
        }

        private void llHideCheckboxes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tvTreeView.CheckBoxes = false;
        }

        private void cbAdvancedViewMode_CheckedChanged(object sender, EventArgs e)
        {
            _AdvancedModeViews = cbAdvancedViewMode.Checked;
        }

        private void tvTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            var currentNode = tvTreeView.GetNodeAt(e.Location);
            if (currentNode != null)
                Callbacks.raiseRegistedCallbacks(_onMouseMove, new[] { currentNode});
        }

        private void tvTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tvTreeView.SelectedNode = e.Node;
        }                     

    }
}
