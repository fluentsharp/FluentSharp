// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using O2.Kernel.CodeUtils;
using O2.Kernel.InterfacesBaseImpl;
using O2.Views.ASCX;
using O2.Views.ASCX.SourceCodeEdit;

namespace O2.External.SharpDevelop.Ascx
{
    public partial class ascx_Scripts : UserControl
    {        
        public ascx_Scripts()
        {
            InitializeComponent();
            onLoad(); 
        }
      
        public ascx_Scripts(String sFileToOpen) : this()
        {            
            String sTargetDirectory = Path.GetDirectoryName(sFileToOpen);
            if (Directory.Exists(sTargetDirectory))
            {
                scriptsFolder.openDirectory(sTargetDirectory);

                if (File.Exists(sFileToOpen))
                {
                    loadSourceCodeFile(sFileToOpen);
                    compileSourceCode();
                }
                else
                    DI.log.error("Could no find file: {0}", sFileToOpen);
            }
            else
                DI.log.error("Could no find directory: {0}", sTargetDirectory);
        }
       
      
        private void btSourceCode_Compile_Click(object sender, EventArgs e)
        {
            compileSourceCode();
        }

        private void asceSourceCodeEditor_eEnterInSource_Event()
        {
            if (cbSourceCode_AutoCompileOnEnter.Checked)
                compileSourceCode();
        }
        

        /*private void adDirectory_eDirectoryEvent_DoubleClick(string sValue)
        {
            if (File.Exists(sValue))
            {
                loadSourceCodeFile(sValue);
                btSourceCode_Compile_Click(null, null);
            }
        }*/

        /*private void adDirectory_eDirectoryEvent_Click(string sValue)
        {
            if (File.Exists(sValue))
            {
                loadSourceCodeFile(sValue);
            }
        }*/

        public void openDirectory(String sNewDirectory)
        {
            scriptsFolder.getDirectoryWithSourceCodeFiles().openDirectory(sNewDirectory);
        }

        /*private void directoryWithSourceCodeFiles__onDirectoryRefresh(string newPath)
        {
            asceSourceCodeEditor.setDirectoryOfFileLoaded(newPath);
        }*/        

        private void lbSourceCode_CompilationResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            DI.log.info(lbSourceCode_CompilationResult.Text);
            string[] sSplitedLine = lbSourceCode_CompilationResult.Text.Split(':');
            if (sSplitedLine.Length == 4)
            {
                int iLine = Int32.Parse(sSplitedLine[0]);
                sourceCodeEditor.gotoLine(iLine);
            }
        }

        //
        /*    private void loadAssemblyInvokeAsRightO2DockPanel()
        {
            assemblyInvoke = ascx_AssemblyInvoke.loadAsO2DockPanel(DockState.DockRight, "Compiled C# Script Assembly");
        }

        private void loadScriptsFolderAsLeftO2DockPanel()
        {
            scriptsFolder = ascx_ScriptsFolder.loadAsO2DockPanel(DockState.DockLeft, "Scripts Folder");
            scriptsFolder._onSampleScriptsSelect += scriptsFolder__onSampleScriptsSelect;
            scriptsFolder.getDirectoryWithSourceCodeFiles()._onDirectoryRefresh +=
                directoryWithSourceCodeFiles__onDirectoryRefresh;
            scriptsFolder.getDirectoryWithSourceCodeFiles().eDirectoryEvent_Click += adDirectory_eDirectoryEvent_Click;
            scriptsFolder.getDirectoryWithSourceCodeFiles().eDirectoryEvent_DoubleClick +=
                adDirectory_eDirectoryEvent_DoubleClick;
            scriptsFolder.processSelectedSampleScript();
        }*/      

        private void asceSourceCodeEditor_Load(object sender, EventArgs e)
        {
            if (DesignMode == false)
            {
                //  loadAssemblyInvokeAsRightO2DockPanel();
                //  loadScriptsFolderAsLeftO2DockPanel();
            }
        }

        private void rbCreateExe_CheckedChanged(object sender, EventArgs e)
        {
            tbMainClass.Enabled = rbCreateExe.Checked;
        }

        private void rbCreateDll_CheckedChanged(object sender, EventArgs e)
        {
            tbMainClass.Enabled = rbCreateExe.Checked;
        }

        private void ascx_Scripts_Load(object sender, EventArgs e)
        {
            onLoad();
        }


    }
}
