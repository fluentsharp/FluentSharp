// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class O2ObjectModel : UserControl
    {
        public O2ObjectModel()
        {
            InitializeComponent();
        }

        private void llRefreshFunctionsViewer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            refreshViews();
        }

        private void ascx_O2ObjectModel_Load(object sender, EventArgs e)
        {
            onLoad();
        }

        private void cbHideCSharpGeneratedMethods_CheckedChanged(object sender, EventArgs e)
        {
            refreshO2ObjectModelView(cbHideCSharpGeneratedMethods.Checked);
        }

        private void tbFilterBy_MethodType_KeyUp(object sender, KeyEventArgs e)
        {
            showFilteredMethods(e);
        }        

        private void tbFilterBy_MethodName_KeyUp(object sender, KeyEventArgs e)
        {
            showFilteredMethods(e);
        }

        private void tbFilterBy_ParameterType_KeyUp(object sender, KeyEventArgs e)
        {
            showFilteredMethods(e);
        }

        private void tbFilterBy_ReturnType_KeyUp(object sender, KeyEventArgs e)
        {
            showFilteredMethods(e);
        }

        private void filteredFunctionsViewer__onAfterSelect(object oObject)
        {
            if (oObject is FilteredSignature)
                showSelectedMethodDetails((FilteredSignature)oObject);
            else if (oObject is List<FilteredSignature>)
                showSelectedMethodDetails(((List<FilteredSignature>)oObject)[0]);
        }

        private void tbMethodDetails_Type_Enter(object sender, EventArgs e)
        {
            O2Forms.setClipboardText(tbMethodDetails_Type.Text);
        }

        private void tbMethodDetails_Name_Enter(object sender, EventArgs e)
        {
            O2Forms.setClipboardText(tbMethodDetails_Name.Text);
        }

        private void tbMethodDetails_Parameters_Enter(object sender, EventArgs e)
        {
            O2Forms.setClipboardText(tbMethodDetails_Parameters.Text);
        }

        private void tbMethodDetails_ReturnType_Enter(object sender, EventArgs e)
        {
            O2Forms.setClipboardText(tbMethodDetails_ReturnType.Text);
        }

        private void tbMethodDetails_Signature_Enter(object sender, EventArgs e)
        {
            O2Forms.setClipboardText(tbMethodDetails_Signature.Text);
        }

        private void tbMethodDetails_OriginalSignature_Enter(object sender, EventArgs e)
        {
            O2Forms.setClipboardText(tbMethodDetails_OriginalSignature.Text);
        }

        private void lbMethodDetails_Type_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(tbMethodDetails_Type.Text, DragDropEffects.Copy);
        }

        private void lbMethodDetails_Name_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(tbMethodDetails_Name.Text, DragDropEffects.Copy);
        }

        private void lbMethodDetails_Parameters_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(tbMethodDetails_Parameters.Text, DragDropEffects.Copy);
        }

        private void lbMethodDetails_ReturnType_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(tbMethodDetails_ReturnType.Text, DragDropEffects.Copy);
        }

        private void lbMethodDetails_Signature_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(tbMethodDetails_Signature.Text, DragDropEffects.Copy);
        }

        private void lbMethodDetails_OriginalSignature_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(tbMethodDetails_OriginalSignature.Text, DragDropEffects.Copy);
        }

        private void tvAssembliesLoaded_DragOver(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }

        private void tvAssembliesLoaded_DragDrop(object sender, DragEventArgs e)
        {
            handleDrop(e);
        }
       

        private void tvAssembliesLoaded_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is TreeNode)
            {
                var treeNode = (TreeNode)e.Item;
                tvAssembliesLoaded.SelectedNode = treeNode;
                if (treeNode.Tag != null && treeNode.Tag is Assembly)
                    DoDragDrop((Assembly)treeNode.Tag, DragDropEffects.Copy);
            }
        }

        private void tvExtraAssembliesToLoad_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is TreeNode)
            {                
                var treeNode = (TreeNode)e.Item;
                tvExtraAssembliesToLoad.SelectedNode = treeNode;
                if (treeNode.Tag != null && treeNode.Tag is Assembly)
                    DoDragDrop((Assembly)treeNode.Tag, DragDropEffects.Copy);
            }
        }

        private void filteredFunctionsViewer__onItemDrag(object oObject)
        {
            handleOnItemDrag(oObject);
        }
                  
    }
}
