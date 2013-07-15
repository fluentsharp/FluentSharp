// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using FluentSharp.WinForms;
using FluentSharp.WinForms.Utils;
using FluentSharp.CoreLib.API;
using FluentSharp.SharpDevelopEditor.Resources;

namespace FluentSharp.REPL.Controls
{
    public partial class ascx_AssemblyInvoke : UserControl
    {        

        public ascx_AssemblyInvoke()
        {
            InitializeComponent();
            btExecuteMethodWithoutDebugger.Enabled = false;
            btExecuteAssemblyUnderDebug.Enabled = false;
            btDebugMethod.Enabled = false;
            KO2MessageQueue.getO2KernelQueue().onMessages += ascx_AssemblyInvoke_onMessages;
            setControlImages();
        }

        public void setControlImages()
        {            
            this.btDebugMethod.Image = ImagesForControls.btDebugMethod_Image;            
            this.btExecuteAssemblyUnderDebug.Image = ImagesForControls.btExecuteAssemblyUnderDebug_Image;
            this.btExecuteMethodWithoutDebugger.Image = ImagesForControls.btExecuteMethodWithoutDebugger_Image;            
            this.btShowAssemblyExecutionPanel.Image = ImagesForControls.btShowAssemblyExecutionPanel_Image;            
        }

        private void tvSourceCode_CompiledAssembly_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            dgvSourceCode_SelectedMethodParameters.Enabled = false;            
            onTreeViewAfterSelect(e.Node);
        }                       
               

        public void checkAutoExecutionOfLastInvokedMethod()
        {
            if (autoExecuteLastMethod && lbLastMethodExecuted.Tag != null)
            {
                PublicDI.log.info("checkAutoExecutionOfLastInvokedMethod not implemented in this version");
                /*         Compile.executeMethod((MethodInfo) lbLastMethodExecuted.Tag, dgvSourceCode_SelectedMethodParameters,
                                      tbSourceCode_InvocationResult, dgvSourceCode_InvocationResult, this);
        */ 
            }
        }
        

        private void tvSourceCode_CompiledAssembly_DoubleClick(object sender, EventArgs e)
        {        
            if (executeMethodOnDoubleClick)
                executeMethod();
        }        

        private void tvSourceCode_CompiledAssembly_DragDrop(object sender, DragEventArgs e)
        {
            loadAssembly(Dnd.tryToGetFileOrDirectoryFromDroppedObject(e));
        }


        private void tvSourceCode_CompiledAssembly_DragEnter(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }

        private void llAbortAssemblyLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            killThreadUsedToPopulateTargetList();
            tvSourceCode_CompiledAssembly.Visible = true;
        }

        private void executeMethodUnderDebug_Click(object sender, EventArgs e)
        {
            executeAndDebugAssembly();
        }
        
        private void btExecuteMethodWithoutDebugger_Click(object sender, EventArgs e)
        {
            executeMethod();
        }

        private void btDebugMethod_Click(object sender, EventArgs e)
        {            
            createStandAloneExeAndDebugMethod(tbAppDomainDlls.Text);
        }
      
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void cbExecuteOnDoubleClick_CheckedChanged(object sender, EventArgs e)
        {
            executeMethodOnDoubleClick = rbExecuteOnDoubleClick.Checked;
        }
        

        private void cbOnlyShowStaticMethods_CheckedChanged(object sender, EventArgs e)
        {
            onlyShowStaticMethods = cbOnlyShowStaticMethods.Checked;
        }

        public void setExecuteMethodOnDoubleClick(bool value)
        {
            this.invokeOnThread(() => rbExecuteOnDoubleClick.Checked = value);
        }

        public void setOnlyShowStaticMethods(bool value)
        {
            this.invokeOnThread(() => rbExecuteOnDoubleClick.Checked = value);
        }

        private void btShowAssemblyExecutionPanel_Click(object sender, EventArgs e)
        {
            setShowAssemblyExecutionPanel(!showAssemblyExecutionPanel);
        }

        private void llExecutionThreads_Refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            executionThreads_Refresh();
        }
        
        private void llExecutionThreads_EndThread_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (tvExecutionThreads.SelectedNode != null && tvExecutionThreads.SelectedNode.Tag is Thread)
                stopExecutionThread((Thread)tvExecutionThreads.SelectedNode.Tag);
        }

        private void tvExecutionThreads_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var currentNode = e.Node;
            if (currentNode != null)
            {
                if (currentNode.Tag is Thread)
                    showThreadDetailsOnTreeNode(currentNode, (Thread) currentNode.Tag);
                else if (currentNode.Tag is ProcessThread)
                    showThreadDetailsOnTreeNode(currentNode, (ProcessThread)currentNode.Tag);
            }
        }

        private void llShowO2Threads_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            showO2Threads();
        }
      

    }
}
