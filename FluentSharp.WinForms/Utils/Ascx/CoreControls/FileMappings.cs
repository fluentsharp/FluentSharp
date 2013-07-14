// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;


namespace FluentSharp.WinForms.Controls
{
    public partial class FileMappings : UserControl
    {
        
        private void tvProjectFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void tvProjectFiles_DragDrop(object sender, DragEventArgs e)
        {
            handleDrop(e);
        }                

        private void tvProjectFiles_DoubleClick(object sender, EventArgs e)
        {
            if (tvFileMappings.SelectedNode != null)
            {
                string selectedItem = tvFileMappings.SelectedNode.Text;
                Callbacks.raiseRegistedCallbacks(eventDoubleClick, new object[] {selectedItem});
            }
        }

        private void tvProjectFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvFileMappings.SelectedNode.Tag != null)
            {
                if (tvFileMappings.SelectedNode.Tag is string)
                {
                    lbSelectedFile.Text = tvFileMappings.SelectedNode.Text;
                    string selectedFile = tvFileMappings.SelectedNode.Tag.ToString();
                    Callbacks.raiseRegistedCallbacks(eventAfterSelect, new object[] { selectedFile });
                    if (cbOpenFilesOnSelection.Checked)
                        O2Messages.fileOrFolderSelected(selectedFile);
                }
                else
                    lbSelectedFile.Text = tvFileMappings.SelectedNode.Text;
            }
        }

        private void ascx_ProjectFiles_Load(object sender, EventArgs e)
        {
            onLoad();
        }

        private void tbExtensionsToShow_TextChanged(object sender, EventArgs e)
        {
            setExtensionsToShow_internal(tbExtensionsToShow.Text);
        }

        private void tvFileMappings_ItemDrag(object sender, ItemDragEventArgs e)
        {
            tvFileMappings.SelectedNode = (TreeNode) e.Item;
            if (tvFileMappings.SelectedNode != null && tvFileMappings.SelectedNode.Tag != null)
                DoDragDrop(tvFileMappings.SelectedNode.Tag, DragDropEffects.Copy);
        }

                private void llExtensionFilter_AllFiles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbExtensionsToShow.Text = extensionFilter_AllFiles;
            applyColorsToRootNodes();
            //showMappingsOnTreeView();
        }

        private void llExtensionFilter_Java_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbExtensionsToShow.Text = extensionFilter_JavaFiles;
            applyColorsToRootNodes();
            //showMappingsOnTreeView();
        }

        private void llExtensionFilter_DotNet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbExtensionsToShow.Text = extensionFilter_DotNetFiles;
            applyColorsToRootNodes();
            //showMappingsOnTreeView();
        }
        

        private void tbExtensionsToShow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                applyColorsToRootNodes();

        }
        

        private void tvFileMappings_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                if (deleteFilesMappedToFromSelectedNodeTag((TreeView)sender))
                    showMappingsOnTreeView(tbViewFilter.Text);
        }

        private void btRrefresh_Click(object sender, EventArgs e)
        {
            showMappingsOnTreeView(tbViewFilter.Text);
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            clearMappings();
            showMappingsOnTreeView(tbViewFilter.Text);
        }

        private void btLoadFilesFromO2TempDir_Click(object sender, EventArgs e)
        {
            clearMappings();
            loadFilesFromFolder(PublicDI.config.O2TempDir, tbViewFilter.Text);
            lbDropHelpInfo.Visible = false;
        }

        private void btDragAllFileThatMatchExtensionFilter_Click(object sender, EventArgs e)
        {

        }
        

        private void btDragAllFileThatMatchExtensionFilter_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(getFilesThatMatchCurrentExtensionFilter(), DragDropEffects.Copy);
        }

        private void tbViewFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                showMappingsOnTreeView(tbViewFilter.Text);                
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tvFileMappings.ExpandAll();
        }

        private void llDragSelectedFiles_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(getFilesThatMatchCurrentExtensionFilter(), DragDropEffects.Copy);
        }                
    }
}
