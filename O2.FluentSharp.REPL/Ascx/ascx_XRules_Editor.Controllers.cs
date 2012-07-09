// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using O2.Core.XRules.XRulesEngine;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.O2Misc;
using O2.DotNetWrappers.Windows;
using O2.External.SharpDevelop.Ascx;
using O2.Kernel;

namespace O2.Core.XRules.Ascx
{
    partial class ascx_XRules_Editor
    {
        private bool runOnLoad = true;        

        public Dictionary<string, TabPage> filesLoaded = new Dictionary<string, TabPage>();  

        private void onLoad()
        {
            if (DesignMode == false && runOnLoad)
            {
                tabPage_WithSearchForScript.add_TreeViewWithFilter(PublicDI.config.LocalScriptsFolder, PublicDI.config.LocalScriptsFolder.files(true, "*.cs", "*.h2", "*.o2"))
                                           .afterSelect<string>((file)=>loadFile(file,true));
                loadXRuleDatabase();                
                runOnLoad = false;
                this.Text += " " + clr.details();
            }

        }

        public void loadXRuleDatabase()
        {						
            directoryWithXRulesDatabase.openDirectory(PublicDI.config.LocalScriptsFolder);
            directoryWithLocalXRules.openDirectory(PublicDI.config.LocallyDevelopedScriptsFolder.createDir());            
            loadXRulesTemplates(lbCurrentXRulesTemplates);
        }

        public static void loadXRulesTemplates(ListBox lbTargetListBox)
        {
            lbTargetListBox.invokeOnThread(
                () =>
                {
                    lbTargetListBox.Items.Clear();
                    if (PublicDI.config.ScriptsTemplatesFolder.dirExists())
                    {
                        lbTargetListBox.Items.AddRange(
                            Files.getFilesFromDir(PublicDI.config.ScriptsTemplatesFolder).ToArray());
                        if (lbTargetListBox.Items.Count > 0)
                            lbTargetListBox.SelectedIndex = 0;
                    }
                });
        }
        public void openXRule(string fileOrDir)
        {
            loadFile(fileOrDir,false);
        }

        public string loadFile(string fileOrDir)
        {
            return loadFile(fileOrDir, false);
        }
        public string loadFile(string fileOrDir, bool compileLoadedFile)
        {
            var fileToOpen = "";
            if (File.Exists(fileOrDir))
                fileToOpen = fileOrDir;
            else if (Directory.Exists(fileOrDir))
                return ""; // no suport for dirs
            else
            {
                fileToOpen = Path.Combine(directoryWithXRulesDatabase.getCurrentDirectory(), fileOrDir);
                if (false == File.Exists(fileToOpen))
                    return "";
            }
            this.invokeOnThread(() =>
                    {

                        var fileName = Path.GetFileName(fileToOpen);
                        // check if there is already a tab with this file loaded
                        if (filesLoaded.ContainsKey(fileToOpen))
                            tcTabControlWithRulesSource.SelectedTab = filesLoaded[fileToOpen];
                        else
                        {
                            var newTabPage = new TabPage(fileName);
                            tcTabControlWithRulesSource.TabPages.Add(newTabPage);
                            loadSourceCodeFileIntoTab(fileToOpen, newTabPage, compileLoadedFile);
                            
                            if (tcTabControlWithRulesSource.TabPages.Contains(tpNoRulesLoaded))
                                tcTabControlWithRulesSource.TabPages.Remove(tpNoRulesLoaded);

                            

                            // check again (sometimes there can be a race condition on double click
                            if (filesLoaded.ContainsKey(fileToOpen))
                                tcTabControlWithRulesSource.TabPages.Remove(newTabPage);
                            else
                            {
                                // make it the selected tab
                                tcTabControlWithRulesSource.SelectedTab = newTabPage;
                                // finally add to dictionary                                
                                filesLoaded.Add(fileToOpen, newTabPage);
                            }                            
                        }
                    });
            return fileToOpen;
        }
        

        private void loadSourceCodeFileIntoTab(string fileToOpen, TabPage tabPage, bool compileLoadedFile)
        {
            var sourceCodeEditor = new ascx_SourceCodeEditor();
            sourceCodeEditor.Dock = DockStyle.Fill;
            tabPage.Controls.Add(sourceCodeEditor);
            sourceCodeEditor.loadSourceCodeFile(fileToOpen.Trim());
            if (compileLoadedFile)
                sourceCodeEditor.compileSourceCode();
        }

        public void openSourceDirectory(string directoryToOpen)
        {
            directoryWithXRulesDatabase.openDirectory(directoryToOpen);
        }

        public List<String> getXRulesSourceFilesInCurrentDirectory()
        {
            return directoryWithXRulesDatabase.getFiles();
        }

        public string createNewRuleFromTemplate(string templateToUse, string newRuleName)
        {            
            if (File.Exists(templateToUse) == false)
                PublicDI.log.error("In createNewRuleFromTemplate, could not find template file: {0}", templateToUse);
            else
            {
                //var newRuleFile = Path.Combine(XRules_Config.PathTo_XRulesDatabase_fromLocalDisk, newRuleName);
                var newRuleFile = Path.Combine(directoryWithLocalXRules.getCurrentDirectory(), newRuleName); // TODO: move directoryWithLocalXRules.getCurrentDirectory() to the upper level
                if (Path.GetExtension(newRuleFile) != ".cs")
                    newRuleFile += ".cs";
                if (false == File.Exists(newRuleFile))
                {
                    Files.WriteFileContent(newRuleFile, Files.getFileContents(templateToUse));
                    if (File.Exists(newRuleFile))
                        return newRuleFile;
                }
            }
            return "";
        }

        public void reloadFile(ascx_SourceCodeEditor sourceCodeEditor)
        {
            sourceCodeEditor.reloadCurrentFile();
        }

        private void removeFileInTab(TabPage tabToRemove)
        {
            foreach(var loadedFile in filesLoaded)
                if (loadedFile.Value == tabToRemove)
                {
                    tcTabControlWithRulesSource.TabPages.Remove(loadedFile.Value);
                    filesLoaded.Remove(loadedFile.Key);
                    return;
                }            
        }
    }
}
