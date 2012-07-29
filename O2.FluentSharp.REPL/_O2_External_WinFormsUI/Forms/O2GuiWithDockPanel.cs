// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.Windows;
using O2.External.WinFormsUI;
using O2.Interfaces.Views;
using O2.Kernel;
using O2.Views.ASCX.Ascx.MainGUI;
using O2.Views.ASCX.classes.MainGUI;
using O2.Views.ASCX.Forms;
using WeifenLuo.WinFormsUI.Docking;
using O2.Kernel.InterfacesBaseImpl;

namespace O2.External.WinFormsUI.Forms
{
    public partial class O2GuiWithDockPanel : Form
    {
        //private string guiXmlFormat = Path.Combine(PublicDI.o2CorLibConfig.O2TempDir, "GuiFormat.xml");

        public O2GuiWithDockPanel()
        {
            InitializeComponent();
            if (DesignMode == false)
            {
                // set the logRedirection in the  publicDI log so that all messages go to the GUIs Log (and visible by the LogViewer)
                PublicDI.log.LogRedirectionTarget = new WinFormsUILog(); // first create the one we are going to use locally                
                KO2MessageQueue.getO2KernelQueue().onMessages += o2MessageQueue_onMessages;
                O2AscxGUI.o2GuiWithDockPanel = this;
                cbAutoSendLogsOnClose.Checked = false; // ClickOnceDeployment.isApplicationBeingExecutedViaClickOnceDeployment();
            }
        }

        
        /*      private void button1_Click(object sender, EventArgs e)
        {
            
        }*/

        

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }


        private void O2GuiWithDockPanel_FormClosing(object sender, FormClosingEventArgs e)
        {                                    
            if (cbAutoSendLogsOnClose.Checked)
                sendEmailToO2Support("On Form Closing", "Closed at: " + DateTime.Now.ToShortTimeString(), false);
            PublicDI.log.LogRedirectionTarget = null;

            KO2MessageQueue.getO2KernelQueue().onMessages -= o2MessageQueue_onMessages;
            O2AscxGUI.o2GuiWithDockPanel = null; // reset this value since this GUI is not available anymore
            O2AscxGUI.guiClosed.Set();            // set flag to the treads on WaitFor() can continute
            PublicDI.log.info("O2GuiWithDockPanel form Closing"); // this log entry should now go to the Debug View            
        }

        private void requestHelpFromO2SupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void dockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
        }

        private void reLaunchThisO2ModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void closeExitO2ModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

/*        private void relauchThisO2ModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Processes.reLaunchCurrentProcess();
        }*/

        private void requestHelpFromO2SupportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReportBug.showGui(this);            
        }


        private void textToemailSupport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
                sendEmailToO2SupportUsingTextInMenuBar();
        }

        private void btSendCommentToO2Support_Click(object sender, EventArgs e)
        {
            sendEmailToO2SupportUsingTextInMenuBar();
        }

       
        private void sendMessageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sendEmailToO2Support("From O2 GUI Tool bar", tbTextToemailSupport.Text);
        }

        private void setDefaultEmailO2SupportMessage()
        {
            tbTextToemailSupport.Text = PublicDI.sEmailDefaultTextFromO2Gui;
        }

        private void O2GuiWithDockPanel_Load(object sender, EventArgs e)
        {
            setDefaultEmailO2SupportMessage();
        }

        private void tbTextToemailSupport_TextChanged(object sender, EventArgs e)
        {
            if (tbTextToemailSupport.Text != PublicDI.sEmailDefaultTextFromO2Gui)
                tbTextToemailSupport.BackColor = Color.LightGreen;
            else
                tbTextToemailSupport.BackColor = Color.LightPink;
        }

        private void tbTextToemailSupport_KeyDown(object sender, KeyEventArgs e)
        {
            if (tbTextToemailSupport.Text == PublicDI.sEmailDefaultTextFromO2Gui)
                tbTextToemailSupport.Text = "";
            if (e.KeyCode == Keys.Enter)
            {
                sendEmailToO2Support("From O2 GUI Tool bar (using Enter)", tbTextToemailSupport.Text);
            }
        }

        


        private void toolStripTextBoxForMailServer_TextChanged(object sender, EventArgs e)
        {
            PublicDI.sEmailHost = toolStripTextBoxForMailServer.Text;
            toolStripTextBoxForMailServer.BackColor = Color.LightGreen;
        }

        private void toolStripTextBoxForMailServer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char) Keys.Enter)
                sendMessageToolStripMenuItem1_Click(null, null);
        }

        private void dockPanel_DragDrop(object sender, DragEventArgs e)
        {
            tryToLoadFileInMainDocumentArea(e);
        }


        private void dockPanel_DragEnter(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }

        private void menuStripForO2GuiWithDocPanel_DragDrop(object sender, DragEventArgs e)
        {
            tryToLoadFileInMainDocumentArea(e);
        }

        private void menuStripForO2GuiWithDocPanel_DragEnter(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }

        private void O2GuiWithDockPanel_DragEnter(object sender, DragEventArgs e)
        {
            Dnd.setEffect(e);
        }

        private void O2GuiWithDockPanel_DragDrop(object sender, DragEventArgs e)
        {
            tryToLoadFileInMainDocumentArea(e);
        }

       

        private void ozasmtQuerytoViewAndFilterOzasmtFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PublicDI.windowsForms.openAscx(typeof (ascx_OzasmtQuery), false, "O2 Tool - Ozasmt Query");
        }


        private void findingsEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PublicDI.windowsForms.openAscx(typeof (ascx_FindingEditor), true, "O2 Tool - Finding Editor");
        }

        private void currentTempDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PublicDI.windowsForms.openAscx(typeof (ascx_Directory), true, "O2 Temp Directory");
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLogViewerControl();            
        }

        

        private void findingsViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PublicDI.windowsForms.openAscx(typeof (ascx_FindingsViewer), true, "O2 Tool - Findings Viewer");
        }

        private void cirViewertoViewCirDataFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
//            PublicDI.windowsForms.openAscx(typeof (ascx_CirViewer_CirData), false, "O2 Tool - Cir Viewer");
        }

        private void unziputilToUnzipFilesOnUsingDragAndDropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //          PublicDI.windowsForms.openAscx(typeof (ascx_Unzip), true, "O2 Tool - Unzip files");
        }

        /*  private void o2ReflectorveryBetaVersionOfAnBuiltInNETReflectorToolToolStripMenuItem_Click(object sender,
                                                                                                  EventArgs e)
        {
            PublicDI.windowsForms.openAscx(typeof (ascx_O2Reflector), false, "O2 Tool - O2 Reflector");
        }
        */

        private void webAutomationusesFirefoxGeckoWebBrowserControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //        PublicDI.windowsForms.openAscx(typeof (ascx_WebAutomation), false, "Web Automation using Firefox Gecko Web Browser Control");
        }

        /*      private void sourceCodeViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PublicDI.windowsForms.openAscx(typeof (ascx_SourceCodeEditor), false, "File Viewer");
        }

        private void sourceCodeEditorwriteAndExecuteDynamicCCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PublicDI.windowsForms.openAscx(typeof (ascx_Scripts), false, "C# Scripts Editor");
        }

        private void editThisO2ModuleStartUpXmlConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ascx_SourceCodeEditor.loadFile(SpringExec.getCurrentModuleXmlConfigFile().Replace("vshost.", ""));
        }

        private void dynamicallyInvokeO2sInternalClassesAndMethodsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ascx_AssemblyInvoke.loadAsO2DockPanel(DockState.Float, "O2 Object Model").loadAssembly(
                PublicDI.reflection.getCurrentAssembly(), false);
        }*/

        private void webAutomationusesFirefoxGeckoWebBrowserControlToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
//            PublicDI.windowsForms.openAscx(typeof (ascx_WebAutomation), false, "Web automation - using Firefox Gecko engine");
        }

        private void fileMappingsfilteredByFileTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
//            PublicDI.windowsForms.openAscx(typeof (ascx_FileMappings), false, "File Mappings");
        }

/*        private void assemblyObjectInvocationshouldWorkWithMostNetAssesmbliesToolStripMenuItem_Click(object sender,
                                                                                                     EventArgs e)
        {
            PublicDI.windowsForms.openAscx(typeof (ascx_AssemblyInvoke), false, "Generic Assembly Invoke");
        }*/

        private void crashO2HeyYouKnowYouWantToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PublicDI.log.info("Crashing O2 :)");
            throw new Exception("How to crash O2 in 1 click!");
        }

        private void relauchThisO2ModuleInTempDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Processes.startCurrentProcessInTempFolder();
        }

        private void whichDirectoryIsThisO2ModuleRunningFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PublicDI.log.showMessageBox(PublicDI.config.CurrentExecutableDirectory, "This O2 module is running from:",
            //                      MessageBoxButtons.OK);
            O2AscxGUI.openAscxASync(typeof (ascx_O2InstallAndTempDirectories), O2DockState.Float, "O2 Install and Temp Directories");

        }

		private void editThisO2ModuleStartUpXmlConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

        private void o2CREPLScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            open.scriptEditor_MtaThread();
        }

        private void o2DevelopmentEnviromentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open.devEnvironment_MtaThread();
        }
        

    }
}
