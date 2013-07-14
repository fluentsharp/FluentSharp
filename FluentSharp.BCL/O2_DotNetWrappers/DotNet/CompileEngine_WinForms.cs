using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{
    public class CompileEngine_WinForms
    {
		public static void addErrorsListToListBox(ListBox lbSourceCode_CompilationResult, StringBuilder sbErrorMessage)
        {
            lbSourceCode_CompilationResult.invokeOnThread(
                () =>
                {
                    if (sbErrorMessage == null)
                        return;
                    lbSourceCode_CompilationResult.Items.Clear();
                    addErrorsListToListBox(sbErrorMessage.ToString(), lbSourceCode_CompilationResult);
                });
        }
        // note that this functionality was moved into a TreeView (in the current version of SourceCodeEditor)        
        public static void addErrorsListToListBox(string sErrorMessages, ListBox lbSourceCode_CompilationResult)
        {
            if (sErrorMessages == null)
                return;

            String[] sSplitedErrorMessage = sErrorMessages.Split(new[] { Environment.NewLine },
                                                                 StringSplitOptions.RemoveEmptyEntries);
            foreach (string sSplitMessage in sSplitedErrorMessage)
            {
                // this doesn't really work from here due to multi-threading issues when loading up the page
                /*string[] sSplitedLine = sSplitMessage.Split(new [] {"::"}, StringSplitOptions.None);
                if (sSplitedLine.Length == 5)
                {
                    int iLine = Int32.Parse(sSplitedLine[0]);
                    int iColumn = Int32.Parse(sSplitedLine[1]);
                    O2Messages.fileErrorHighlight(sSplitedLine[4], iLine, iColumn);                    
                }*/
                if (false == sSplitMessage.Contains("conflicts with the imported type"))        // ignore these warning which mostlikely will be caused by adding extra files (via O2) to this script
                    lbSourceCode_CompilationResult.Items.Add(sSplitMessage);
            }
        }

		public static void addErrorsListToTreeView(TreeView tvCompilationErrors, StringBuilder sbErrorMessage)
        {
            tvCompilationErrors.invokeOnThread(
             () =>
             {
                 if (sbErrorMessage == null)
                     return;
                 tvCompilationErrors.Nodes.Clear();
                 addErrorsListToTreeView(sbErrorMessage.ToString(), tvCompilationErrors);
             });
        }

        private static void addErrorsListToTreeView(string errorMessages, TreeView tvCompilationErrors)
        {
            if (errorMessages == null)
                return;

            String[] sSplitedErrorMessage = errorMessages.Split(new[] { Environment.NewLine },
                                                                 StringSplitOptions.RemoveEmptyEntries);
            foreach (string sSplitMessage in sSplitedErrorMessage)
            {
                var newNode = O2Forms.newTreeNode(tvCompilationErrors.Nodes, sSplitMessage, 0, sSplitMessage);
                newNode.ToolTipText = sSplitMessage;

                if (sSplitMessage.Contains("conflicts with the imported type"))
                    newNode.ForeColor = Color.Gray;
                else
                    newNode.ForeColor = Color.Red;
            }
        }

        public static Thread loadAssesmblyDataIntoTreeView(Assembly aAssemblyToLoad, TreeView tvTargetTreeView,
                                                         Label lbLastMethodExecuted, bool bOnlyShowStaticMethods)
        {
            tvTargetTreeView.Visible = false;
            tvTargetTreeView.Nodes.Clear();
            tvTargetTreeView.Sorted = true;
            int iTypesAdded = 0;

            return O2Thread.mtaThread(() =>
            {
                try
                {
                    var treeNodesToAdd = new List<TreeNode>();
                    foreach (Type tType in aAssemblyToLoad.GetTypes())
                    {
                        if ((iTypesAdded++) % 500 == 0)
                            PublicDI.log.info("{0} types processed", iTypesAdded);
                        //vars.set_(tType.Name, tType); // set global variable of compiled code
                        //Callbacks.raiseEvent_ScriptCompiledSuccessfully(tType.Name);                
                        TreeNode tnType = O2Forms.newTreeNode(tType.Name, tType.Name, 1, tType);
                        foreach (
                            MethodInfo mMethod in
                                tType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly |
                                ((bOnlyShowStaticMethods) ? BindingFlags.Static : BindingFlags.Static | BindingFlags.Instance)))
                        {
                            if (mMethod.Name == lbLastMethodExecuted.Text)
                                lbLastMethodExecuted.Tag = mMethod;
                            //TreeNode tnMethod = O2Forms.newTreeNode(mMethod.Name, mMethod.Name, 2, mMethod);
                            TreeNode tnMethod =
                                O2Forms.newTreeNode(
                                    new FilteredSignature(mMethod).getReflectorView(),
                                    mMethod.Name, 2, mMethod);
                            tnType.Nodes.Add(tnMethod);
                        }
                        if (tnType.Nodes.Count > 0)
                            treeNodesToAdd.Add(tnType);
                        //O2Forms.addNodeToTreeNodeCollection(tvTargetTreeView, tvTargetTreeView.Nodes, tnType);      // thread safe way to add nodes                                                                                    
                    }
                    PublicDI.log.info("{0} types processed , now loading them into treeView", iTypesAdded);
                    tvTargetTreeView.invokeOnThread(() =>
                    {
                        foreach (var treeNode in treeNodesToAdd)
                            tvTargetTreeView.Nodes.Add(treeNode);
                        PublicDI.log.info("All nodes loaded");
                        if (tvTargetTreeView.Nodes.Count > 0)
                            tvTargetTreeView.Nodes[0].Expand();
                        tvTargetTreeView.Visible = true;
                    });

                }
                catch (Exception ex)
                {
                    PublicDI.log.ex(ex, "in loadAssesmblyDataIntoTreeView");
                }
            });


            //if (tvTargetTreeView.GetNodeCount(true) < 20)
            //    tvTargetTreeView.ExpandAll();
            //tvTargetTreeView.Visible = true;
        }

        public static void addExtraFileReferencesToSelectedNode(TreeView treeView, string file)
        {
            if (treeView != null)
                treeView.invokeOnThread(
                    () =>
                    {
                        addExtraFileReferencesToTreeNode(treeView.SelectedNode, file);
                    });
        }

        public static void addExtraFileReferencesToTreeNode(TreeNode treeNode, string file)
        {
            if (treeNode != null && File.Exists(file))
            {
                treeNode.Nodes.Clear();
                // this will get the list of files to compile (which includes the extra files referenced in the source code that we want to add to this treeview)
                var filesToCompile = new List<string>();
				filesToCompile.Add(file);
                CompileEngine.addSourceFileOrFolderIncludedInSourceCode(filesToCompile, new List<string>() , new List<string>());
                filesToCompile.Remove(file);
                foreach (var extraFile in filesToCompile)
                {
                    O2Forms.newTreeNode(treeNode, Path.GetFileName(extraFile), 5, extraFile);
                }
                treeNode.ExpandAll();
            }
        }
		
    }
}