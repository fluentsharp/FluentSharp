// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.O2Findings;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_FindingsViewer : UserControl
    {

        public ascx_FindingsViewer()
        {
            InitializeComponent();            
        }        

        public ascx_FindingsViewer(string ozasmtFile) 
            : this()
        {
            onLoad();
            loadO2Assessment(ozasmtFile);
        }

        private void ascx_FindingsViewer_Load(object sender, EventArgs e)
        {
            onLoad();
        }
        private void tvFindings_DragDrop(object sender, DragEventArgs e)
        {

            handleTreeViewDropEvent(e);
        }
      
        private void tvFindings_DragEnter(object sender, DragEventArgs e)
        {
            if (false == currentlyDraggingNodeFromTreeView)
                e.Effect = DragDropEffects.Copy;
        }
        

        private void cbFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            showCurrentO2Findings();
        }

        private void cbFilter1_SelectedIndexChanged(object sender, EventArgs e)
        {
            showCurrentO2Findings();
        }

        private void btSaveFindings_Click(object sender, EventArgs e)
        {
            if (cbSaveFilteredFindings.Checked)
                saveFindings(getFindingsFromTreeView(),cbSaveIntoO2BinaryFormat.Checked);
            else
                saveFindings(currentO2Findings, cbSaveIntoO2BinaryFormat.Checked);
        }

        


        private void tbSavedFileName_TextChanged(object sender, EventArgs e)
        {
        }

        private void tvFindings_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                deleteSelectedNode();
            }
        }

        

        private void tbSelectedValue_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplyTextChangeToNode(tvFindings.SelectedNode, tbSelectedNodeTextValue.Text);
            }
        }

        private void btApplyTextChange_Click(object sender, EventArgs e)
        {
            ApplyTextChangeToNode(tvFindings.SelectedNode, tbSelectedNodeTextValue.Text);
        }

        private void tvFindings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            onFindingsTreeViewAfterSelect();
        }


        private void tvFindings_ItemDrag(object sender, ItemDragEventArgs e)
        {
            onFindingsTreeViewItemDrag(e);
        }        

      
        
        private void btSaveFindings_Leave(object sender, EventArgs e)
        {
            lbFileSaved.Visible = false;
        }

        private void tvFindings_MouseDoubleClick(object sender, MouseEventArgs e)
        {            
            if (tvFindings.SelectedNode != null && tvFindings.SelectedNode.Tag is O2Finding)
            {
                //openInFloatWindow((O2Finding) tvFindings.SelectedNode.Tag);                
                openO2FindingOnNewGuiWindow((O2Finding) tvFindings.SelectedNode.Tag);                    

            }
        }                
        
        private void tbMaxRecordsToShow_TextChanged(object sender, EventArgs e)
        {
            try
            {
                maxNumberOfFindingsToLoad = Int32.Parse(tbMaxRecordsToShow.Text);
                tbMaxRecordsToShow.BackColor = Color.White;
            }
            catch (Exception)
            {
                PublicDI.log.error("could not convert {0} to an int" + tbMaxRecordsToShow.Text);
                tbMaxRecordsToShow.BackColor = Color.LightPink;
            }
        }

        private void btConfig_Click(object sender, EventArgs e)
        {
            groupBoxConfigSaveAndEdit.Visible = !groupBoxConfigSaveAndEdit.Visible;
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            showCurrentO2Findings();
        }

        private void btClearAll_Click(object sender, EventArgs e)
        {
            clearO2Findings();
            showCurrentO2Findings();
        }

        private void btViewSmartTraces_Click(object sender, EventArgs e)
        {
            var traceTreeViewVisibleStatus = !ascxTraceTreeView.Visible; ;
            setTraceTreeViewVisibleStatus(traceTreeViewVisibleStatus);
        }        

        private void btExpandAll_Click(object sender, EventArgs e)
        {
            expandAll();
        }
        
        private void btCollapseAll_Click(object sender, EventArgs e)
        {
            collapseAll();
        }

        private void laFilter2Name_Click(object sender, EventArgs e)
        {
            cbFilter2.Text = ascx_FindingsViewer.noFilterStringTagForFilter2;       // small helper to make the filter2 into the no filter mode
        }

        private void tbFilter1Text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                showCurrentO2Findings();
        }

        private void tvFindings_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            onTreeViewBeforeExpand(e.Node);
        }

        private void llDragAllFindings_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(currentO2Findings, DragDropEffects.Copy);
        }        

        private void llDragFilteredFindings_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(getFindingsFromTreeView(), DragDropEffects.Copy);
        }

        private void ascxTraceTreeView__onTraceSelected(IO2Trace o2SelectedTrace)
        {
            onTraceSelected(o2SelectedTrace);
        }

        private void tsFindingsViewer_DragDrop(object sender, DragEventArgs e)
        {
            handleTreeViewDropEvent(e);
        }        

        private void tsFindingsViewer_DragEnter(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }

        private void cbFilter1_Click(object sender, EventArgs e)
        {

        }

        private void llDragSelectedNodeText_MouseDown(object sender, MouseEventArgs e)
        {
            var textValue = getSelectedNodeFilter1Text();
            if (textValue != null)
                DoDragDrop(textValue, DragDropEffects.Copy);
        }

        private void scrollBarVerticalSize_Scroll(object sender, ScrollEventArgs e)
        {
            var difference = ascxTraceTreeView.Height - scrollBarVerticalSize.Value;
            ascxTraceTreeView.Height = scrollBarVerticalSize.Value;
            ascxTraceTreeView.Top += difference;
        }

        private void scrollBarHorizontalSize_Scroll(object sender, ScrollEventArgs e)
        {
            var difference = ascxTraceTreeView.Width - scrollBarHorizontalSize.Value;
            ascxTraceTreeView.Width = scrollBarHorizontalSize.Value;
            ascxTraceTreeView.Left += difference;
        }

        private void ascxTraceTreeView_Load(object sender, EventArgs e)
        {
           
        }

        private void ascxTraceTreeView_SizeChanged(object sender, EventArgs e)
        {
            scrollBarVerticalSize.Maximum = ascxTraceTreeView.Height + 20;
            scrollBarVerticalSize.Value = ascxTraceTreeView.Height;            

            scrollBarHorizontalSize.Maximum = ascxTraceTreeView.Width + 20;
            scrollBarHorizontalSize.Value = ascxTraceTreeView.Width;
        }

        private void btSave_Click(object sender, EventArgs e)
        {           
            saveFindings(currentO2Findings,cbSaveIntoO2BinaryFormat.Checked);            
        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            PublicDI.log.info("Select file to open");
            var openFileDialog = new OpenFileDialog();                        
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PublicDI.log.info("Loading file: {0}", openFileDialog.FileName);
                loadO2Assessment(openFileDialog.FileName);
                openFileDialog.Dispose();
            }
            //var fileOrFolder = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
            //loadO2Assessment(fileOrFolder);
        }


        private bool showNoEnginesLoadedAlert = true;
        private bool simpleViewMode = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _ShowNoEnginesLoadedAlert
        {
            get { return showNoEnginesLoadedAlert; }
            set
            {
                showNoEnginesLoadedAlert = value;
                laNoAssessmentLoadEnginesLoaded.Visible = showNoEnginesLoadedAlert;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _SimpleViewMode
        {
            get { return simpleViewMode; }
            set 
            { 
                simpleViewMode = value;
                setSimpleViewMode();
            }
        }

        private void setSimpleViewMode()
        {
            btOpenFile.Visible = simpleViewMode;
            btSaveFindings.Visible = simpleViewMode;
            btConfig.Visible = simpleViewMode;
        }

                  
     
    }
}
