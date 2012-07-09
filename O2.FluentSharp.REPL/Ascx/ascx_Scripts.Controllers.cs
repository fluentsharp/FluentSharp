// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel.CodeUtils;

namespace O2.External.SharpDevelop.Ascx
{
    public partial class ascx_Scripts
    {
        /*public List<String> lsExtraReferenceAssembliesToAdd = new List<string>
                                                                  {
                                                                      "O2_Kernel.dll",
                                                                      "System.Dll",
                                                                      "System.Core.dll",
                                                                      "System.Data.dll",
                                                                      "System.Drawing.dll",
                                                                      "System.Windows.Forms.dll",
                                                                      "System.Xml.dll",
                                                                      "System.Xml.Linq.dll",                                                                      
                                                                      "System.Configuration.dll",                                                                      
                                                                      "WeifenLuo.WinFormsUI.Docking.dll"
                                                                  };*/

        private bool runOnLoad = true;

        private void onLoad()
        {


            if (false == DesignMode && runOnLoad)
            {

                // set ascx_ScriptsFolder DI variable to point to the current Script Editor|
                scriptsFolder.sourceCodeEditor = sourceCodeEditor;
                /*
                // adding loaded o2 Add-ons
                foreach (String sReference in o2.core.GlobalStaticVars.dO2LoadedDlls.Keys)
                    tbExtraReferencesToAdd.Text += sReference + Environment.NewLine;
                // adding default references*/

                //foreach (string sReference in lsExtraReferenceAssembliesToAdd)
                foreach (string reference in new CompileEngine().getListOfReferencedAssembliesToUse())
                    tbExtraReferencesToAdd.Text += reference + Environment.NewLine;

                //foreach (var sReference in Compile.lsExtraReferencesToAdd)
                //    tbExtraReferencesToAdd.Text += sReference + Environment.NewLine;
                
//                addCurrentAppDomainsDllsAsReferences();
                try
                {
                    sourceCodeEditor.eEnterInSource_Event += asceSourceCodeEditor_eEnterInSource_Event;
                }
                catch (Exception ex)
                {
                    DI.log.error("in controlInitMethods: {0} ", ex.Message);
                }
                
                // configure assembly  invoke default values
                assemblyInvoke.setIsDebuggerAvailable(true);
                assemblyInvoke.setExecuteMethodOnDoubleClick(true);
                assemblyInvoke.setShowAssemblyExecutionPanel(true);
                assemblyInvoke.setOnlyShowStaticMethods(true);
                runOnLoad = false;
            }
        }


        
        //public ascx_ScriptsFolder scriptsFolder;

/*        public void addCurrentAppDomainsDllsAsReferences()
        {
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var assemblyName = Path.GetFileName(assembly.Location);
                if (false == lsExtraReferenceAssembliesToAdd.Contains(assemblyName))
                    if (assemblyName.IndexOf("JetBrains")==-1 && 
                        assemblyName.IndexOf("VisualStudio") == -1 &&
                        assemblyName.IndexOf("SMDiagnostics") == -1) // don't add these assemblies
                        lsExtraReferenceAssembliesToAdd.Add(assemblyName);

            }
        }*/


        public void compileSourceCode()
        {
            if (sourceCodeEditor.partialFileViewMode == false)

                if (this.okThread(delegate { compileSourceCode(); }))
                {
                    var lsExtraReferencesToAdd = new List<string>(); //lsExtraReferenceAssembliesToAdd.ToArray());
                    String sErrorMessages = "";
                    var compileEngine = new CompileEngine();
                    Assembly aCompiledAssembly = null;
                    if (tbExtraReferencesToAdd.Text != "")
                        lsExtraReferencesToAdd.AddRange(tbExtraReferencesToAdd.Text.Split(new[] {Environment.NewLine},
                                                                                          StringSplitOptions.
                                                                                              RemoveEmptyEntries));

                    lbSourceCode_CompilationResult.Items.Clear();
                    var exeMainClass = (rbCreateExe.Checked) ? tbMainClass.Text : "";
                    var outputAssemblyName = ""; // todo expose outputAssemblyName on GUI        

                    //DI.config.addPathToCurrentExecutableEnvironmentPathVariable(DI.config.O2TempDir);

                    sourceCodeEditor.saveSourceCode();
                    var filesToCompile = new List<String>().add(sourceCodeEditor.sPathToFileLoaded);
                    if (compileEngine.compileSourceFiles(filesToCompile, lsExtraReferencesToAdd.ToArray(),
                                                         ref aCompiledAssembly, ref sErrorMessages, false /*verbose*/,
                                                         exeMainClass,
                                                         outputAssemblyName))
                    {
                        // if we only have 1 class in the completed code, set tbMainClass.Text to it
                        if (aCompiledAssembly.GetTypes().Length == 1)
                            tbMainClass.Text = aCompiledAssembly.GetTypes()[0].FullName;
                        lbSourceCode_CompilationResult.ForeColor = Color.Black;
                        lbSourceCode_CompilationResult.Items.Add("No errors");
                        O2Messages.dotNetAssemblyAvailable(aCompiledAssembly.Location);
                        //if (assemblyInvoke != null)
                        //    assemblyInvoke.loadAssembly(aCompiledAssembly, cbAutoExecuteOnMethodCompile.Checked);
                    }
                    else
                    {
                        //assemblyInvoke.setControlsEnableState(false);
                        lbSourceCode_CompilationResult.ForeColor = Color.Red;
                        CompileEngine_WinForms.addErrorsListToListBox(sErrorMessages, lbSourceCode_CompilationResult);
                    }
                    if (cbAutoSaveOnCompile.Checked)
                        sourceCodeEditor.saveSourceCode();
                }
        }

        public void loadSourceCodeFile(String sSourceCodeFileToLoad)
        {
            loadSourceCodeFile(sSourceCodeFileToLoad, true);
        }

        public void loadSourceCodeFile(String sSourceCodeFileToLoad, bool compileSourceCodeFile)
        {
            sourceCodeEditor.loadSourceCodeFile(sSourceCodeFileToLoad);
            if (compileSourceCodeFile)
                compileSourceCode();
            //    tbSourceCode_PathToFileLoaded.Text = sSourceCodeFileToLoad;
        }


        public void saveSourceCodeFile()
        {
            sourceCodeEditor.saveSourceCode();
        }

/*        public void addExtraReferenceAssemblies(String sReference)
        {
            addExtraReferenceAssemblies(new List<String>(new[] { sReference }));
        }*/

/*        public void addExtraReferenceAssemblies(List<String> lsReferences)
        {
            foreach (String sReference in lsReferences)
                if (false == lsExtraReferenceAssembliesToAdd.Contains(sReference))
                    lsExtraReferenceAssembliesToAdd.Add(sReference);
        }*/

        public string getSourceCode()
        {
            return sourceCodeEditor.getSourceCode();
        }

        public void setExeCompilationMode(string varClassName)
        {
            this.okThreadSync(delegate
                                  {
                                      rbCreateExe.Checked = true;
                                      tbMainClass.Text = varClassName;
                                  });
        }


        public void setSelectedLineNumber(string fileName, int lineNumber)
        {
            sourceCodeEditor.setSelectedLineNumber(fileName, lineNumber);
        }

        public void setSelectedLineNumber(int lineNumber)
        {
            sourceCodeEditor.setSelectedLineNumber(lineNumber);
        }
        public int getSelectedLineNumber()
        {
            return sourceCodeEditor.getSelectedLineNumber();
        }

        public string getSelectedLineText()
        {
            return sourceCodeEditor.getSelectedLineText();
        }

    }
}
