// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using System.IO;
using FluentSharp.WinForms;
using FluentSharp.WinForms.Controls;
using FluentSharp.WinForms.Utils;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.REPL.Controls
{
    public partial class ascx_ScriptsFolder
    {
        public static string sDefaultO2Scripts = @"_o2_Scripts\";

        //public event Callbacks.dMethod_String _onSampleScriptsSelect;
        public Dictionary<string, string> sampleScripts;

        //readonly O2SampleScripts o2SampleScripts = new O2SampleScripts();

        public SourceCodeEditor sourceCodeEditor { get; set;}           // External DI property

        public void openDirectory(string sTargetDirectory)
        {
            directoryWithSourceCodeFiles.openDirectory(sTargetDirectory);
        }

        public DirectoryViewer getDirectoryWithSourceCodeFiles()
        {
            return directoryWithSourceCodeFiles;
        }

        public void processSelectedSampleScript()
        {
            if (tvSampleScripts.SelectedNode != null && tvSampleScripts.SelectedNode.Tag != null)
            {
                string tempScriptFile = createTempScriptFile(directoryWithSourceCodeFiles.getCurrentDirectory(), tvSampleScripts.SelectedNode.Text, tvSampleScripts.SelectedNode.Tag.ToString());

                openSourceCodeFile(tempScriptFile);
                                
            }
        }

        private void openSourceCodeFile(string sourceCodeFileToOpen)
        {
            // if the sourceCodeEditor has not been DI, then raise a global message, other wise, open the file directly on it
            if (sourceCodeEditor != null)
                sourceCodeEditor.loadSourceCodeFile(sourceCodeFileToOpen);
            else
                O2Messages.fileOrFolderSelected(sourceCodeFileToOpen); ;
        }

        private string createTempScriptFile(string targetDirectory, string scriptName, string scriptContents)
        {
            return createTempScriptFile(Path.Combine(targetDirectory, scriptName), scriptContents);
        }

        private string createTempScriptFile(string tempScriptFile, string scriptContents)
        {
            this.invokeOnThread(() =>
                                    {
                                        if (cbOverrideWithDefaultSample.Checked || !File.Exists(tempScriptFile))
                                        {
                                            Files.WriteFileContent(tempScriptFile, scriptContents);
                                        }
                                    });
            return tempScriptFile;
        }

        private void loadDefaultScriptLocation()
        {
            directoryWithSourceCodeFiles.openDirectory(Path.Combine(PublicDI.config.CurrentExecutableDirectory,
                                                                    ascx_ScriptsFolder.sDefaultO2Scripts));
        }

        

        public void loadSampleScripts()
        {
            "[loadSampleScripts] REMOVED DUE to bug in O2SampleScripts ctor".error();
          //  loadSampleScripts(o2SampleScripts, true);
        }

        public void loadSampleScripts(object referenceObjectWithSampleScripts, bool clearLoadedScriptsList)
        {
            this.invokeOnThread(
                () =>
                {
                    if (clearLoadedScriptsList)
                        tvSampleScripts.Nodes.Clear();
                    sampleScripts = SampleScripts.getDictionaryWithSampleScripts(referenceObjectWithSampleScripts);
                    foreach (var scriptName in sampleScripts.Keys)
                        O2Forms.newTreeNode(tvSampleScripts.Nodes, scriptName, 1, sampleScripts[scriptName]);

                    if (tvSampleScripts.Nodes.Count > 0)
                        tvSampleScripts.SelectedNode = tvSampleScripts.Nodes[0];
                });
        }
        
        public List<string> getSampleScriptNames()
        {
            return new List<string> ( sampleScripts .Keys);
        }

        public string getSampleScriptContent(string scriptName)
        {
            if (sampleScripts.ContainsKey(scriptName))
                return sampleScripts[scriptName];
            return "";
        }

        public void selectSampleScript(string sampleScriptName)
        {
            tvSampleScripts.okThreadSync(delegate
                                             {
                                                 var scriptNode = tvSampleScripts.Nodes[sampleScriptName];
                                                 if (scriptNode != null)
                                                     tvSampleScripts.SelectedNode = scriptNode;
                                             });            
        }

        public void clearLoadedScriptsList()
        {
            throw new System.NotImplementedException();
        }
    }
}
