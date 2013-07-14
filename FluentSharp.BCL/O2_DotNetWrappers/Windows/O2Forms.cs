// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{
    public class O2Forms
    {
        public static Form loadAscxAsMainForm(Type tAscxTypeToLoad)
        {
            return loadAscxAsMainForm(tAscxTypeToLoad, true);
        }

        public static Form loadAscxAsMainForm(Type tAscxTypeToLoad, bool showAsApplication)
        {
            try
            {
                var controlToLoad = (Control)PublicDI.reflection.createObjectUsingDefaultConstructor(tAscxTypeToLoad);
                return loadAscxAsMainForm(controlToLoad, showAsApplication);
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex);                
            }
            return null;
        }

        public static Form loadAscxAsMainForm(Control controlToLoad, bool loadAsApplication)
        {
            if (controlToLoad != null)
            {
                try
                {
                    var fForm = new Form();
					fForm.Height = controlToLoad.Height;
					fForm.Width = controlToLoad.Width;
                    fForm.set_H2Icon();
                    controlToLoad.Dock = DockStyle.Fill;
                    fForm.Controls.Add(controlToLoad);
                    if (loadAsApplication)
                        Application.Run(fForm);
                    else
                        fForm.Show();
                    return fForm;
                }
                catch (Exception ex)
                {
                    PublicDI.log.ex(ex);
                }
                
            }
            return null;
        }

      

        public static String getClipboardText()
        {
            return Clipboard.GetText();
        }

        public static void setClipboardText(String text)
        {
            try
            {
                if (text != null)
                {
                    PublicDI.log.info("Setting clipboard text to: {0}", text);
                    Clipboard.SetText(text);
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in setClipboardText: {0}", ex.Message);
            }
        }

        public static void copyListBoxItemsToClipboard(ListBox.ObjectCollection lbListBoxObjectCollection)
        {
            var sbClipboardData = new StringBuilder();
            foreach (Object oItem in lbListBoxObjectCollection)
                sbClipboardData.Append(oItem + Environment.NewLine);
            Clipboard.SetText(sbClipboardData.ToString());
            PublicDI.log.debug("Copied list into clipboard. {0} items, size:{1}", lbListBoxObjectCollection.Count,
                         sbClipboardData.Length);
        }

        public static void copyListBoxItemsToClipboard(ListBox lbTargetListBox)
        {
            var sbClipboardData = new StringBuilder();
            foreach (Object oItem in lbTargetListBox.Items)
                sbClipboardData.Append(oItem + Environment.NewLine);
            Clipboard.SetText(sbClipboardData.ToString());
            PublicDI.log.debug("Copied list into clipboard. Size:{0}", sbClipboardData.Length);
        }

        public static void copyTreeViewNodesToClipboard(TreeNodeCollection tncTreeNodes)
        {
            var sbClipboardData = new StringBuilder();
            foreach (TreeNode tnTreeNode in tncTreeNodes)
                sbClipboardData.Append(tnTreeNode.Text + Environment.NewLine);
            Clipboard.SetText(sbClipboardData.ToString());
            PublicDI.log.debug("Copied list into clipboard. Size:{0}", sbClipboardData.Length);
        }

        public static void copyStringListToClipboard(List<String> lsListString)
        {
            var sbClipboardData = new StringBuilder();
            foreach (String sItem in lsListString)
                sbClipboardData.Append(sItem + Environment.NewLine);
            Clipboard.SetText(sbClipboardData.ToString());
            PublicDI.log.debug("Copied list into clipboard. Size:{0}", sbClipboardData.Length);
        }

        public static void setCheckBoxStatusForAllParentNodes_recursive(TreeNode tnTreeNode, bool bCbecked)
        {
            if (tnTreeNode.Parent != null)
            {
                tnTreeNode.Parent.Checked = bCbecked;
                setCheckBoxStatusForAllParentNodes_recursive(tnTreeNode.Parent, bCbecked);
            }
        }

        public static void setCheckBoxStatusForAllTreeNodeChilds_recursive(TreeNodeCollection tncTreeNodeCollection,
                                                                           bool bCbecked)
        {
            if (tncTreeNodeCollection != null)
                foreach (TreeNode tnTreeNode in tncTreeNodeCollection)
                {
                    tnTreeNode.Checked = bCbecked;
                    setCheckBoxStatusForAllTreeNodeChilds_recursive(tnTreeNode.Nodes, bCbecked);
                }
        }

        public static void calculateListOfCurrentlySelectedClasses_Recursive(TreeNodeCollection tncTreeNodeCollection,
                                                                             List<TreeNode> ltnCurrentlySelectedClasses)
        {
            if (tncTreeNodeCollection != null)
                foreach (TreeNode tnTreeNode in tncTreeNodeCollection)
                {
                    if (tnTreeNode.Checked)
                        ltnCurrentlySelectedClasses.Add(tnTreeNode);
                    calculateListOfCurrentlySelectedClasses_Recursive(tnTreeNode.Nodes, ltnCurrentlySelectedClasses);
                }
        }

        public static int addToDataGridView_Column(DataGridView dgvDataGridView, String sColumnName, int iWidth)
        {
            int iNewColumnId = dgvDataGridView.Columns.Add(sColumnName, sColumnName);
            if (iWidth == -1)
                dgvDataGridView.Columns[iNewColumnId].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            else
                dgvDataGridView.Columns[iNewColumnId].Width = iWidth;
            return iNewColumnId;
        }

        public static int addToDataGridView_Row(DataGridView dgvDataGridView, Object oTagObject, object[] sRowCells)
        {
            int iNewRowId = dgvDataGridView.Rows.Add(sRowCells);
            DataGridViewRow dgvr = dgvDataGridView.Rows[iNewRowId];
            dgvr.Tag = oTagObject;
            return iNewRowId;
        }

        // add a cache so that we don't process the same file more than once

        public static void removeSelectedNodeFromTreeView(TreeView tvTargetTreeView)
        {
            if (tvTargetTreeView.SelectedNode != null)
                tvTargetTreeView.Nodes.Remove(tvTargetTreeView.SelectedNode);
        }



        public static List<String> getStringListWithAllParentNodesName(TreeNode tnTreeNodeToProcess)
        {
            var ltnParentNodes = new List<String>();
            TreeNode tnCurrentNode = tnTreeNodeToProcess;
            while (tnCurrentNode.Parent != null)
            {
                ltnParentNodes.Add(tnCurrentNode.Parent.Name);
                tnCurrentNode = tnCurrentNode.Parent;
            }
            return ltnParentNodes;
        }

        public static List<String> getStringListWithAllParentNodesText(TreeNode tnTreeNodeToProcess)
        {
            var ltnParentNodes = new List<String>();
            TreeNode tnCurrentNode = tnTreeNodeToProcess;
            while (tnCurrentNode.Parent != null)
            {
                ltnParentNodes.Add(tnCurrentNode.Parent.Text);
                tnCurrentNode = tnCurrentNode.Parent;
            }
            return ltnParentNodes;
        }

        public static List<TreeNode> getTreeNodeListWithAllParentNodes(TreeNode tnTreeNodeToProcess)
        {
            var ltnParentNodes = new List<TreeNode>();
            TreeNode tnCurrentNode = tnTreeNodeToProcess;
            while (tnCurrentNode.Parent != null)
            {
                ltnParentNodes.Add(tnCurrentNode.Parent);
                tnCurrentNode = tnCurrentNode.Parent;
            }
            return ltnParentNodes;
        }

        /*public static List<String> getStringListWithAllNodesText(List<TreeNode> nodes)
        {
            var nodesText = new List<String>();
            foreach(var node in nodes)
                nodesText.Add(node.Text);
            return nodesText;
        }*/

        public static List<TreeNode> getListWithAllNodesFromTreeView(TreeNodeCollection tncNodes)
        {
            var ltnNodes = new List<TreeNode>();
            foreach (TreeNode tnNode in tncNodes)
            {
                ltnNodes.Add(tnNode);
                ltnNodes.AddRange(getListWithAllNodesFromTreeView(tnNode.Nodes));
            }
            return ltnNodes;
        }

        public static TreeNode getRootNode(TreeNode tnTreeNode)
        {
            if (tnTreeNode.Parent != null)
                return getRootNode(tnTreeNode.Parent);
            return tnTreeNode;
        }

        public static void populateTreeViewWithDirectoryContents(TreeView tvTargetTreeView, String sPathToProcess,
                                                                 bool bRecursive, bool bOrganizeByFileType)
        {
            if (Directory.Exists(sPathToProcess))
            {
                foreach (String sFile in Directory.GetFiles(sPathToProcess))
                {
                    TreeNode tnFile = newTreeNode(Path.GetFileName(sFile), sFile, 4, sFile);
                    if (bOrganizeByFileType)
                    {
                        String sFileExtension = Path.GetExtension(sFile);
                        if (sFileExtension == "")
                            sFileExtension = " ";
                        //  tvTargetTreeView.Nodes[sFileExtension] doesn't work when sFileExtension = "";
                        if (tvTargetTreeView.Nodes[sFileExtension] == null)
                            tvTargetTreeView.Nodes.Add(newTreeNode(sFileExtension, sFileExtension, 3, null));
                        tvTargetTreeView.Nodes[sFileExtension].Nodes.Add(tnFile);
                    }
                    else
                        tvTargetTreeView.Nodes.Add(tnFile);
                }
                if (bRecursive)
                    foreach (string sDirectory in Directory.GetDirectories(sPathToProcess))
                        populateTreeViewWithDirectoryContents(tvTargetTreeView, sDirectory, true /*bRecursive*/,
                                                              bOrganizeByFileType);
            }
        }

        public static void loadTreeViewWith_AssembliesInCurrentAppDomain(TreeView treeview, List<Assembly> assembliesAlreadyLoaded,bool showCheckBoxes)
        {
            treeview.Nodes.Clear();
            foreach (var assemblyInAppDomain in PublicDI.reflection.getAssembliesInCurrentAppDomain())
            {
                try
                {
                    var assemblyLoaded = false;
                    foreach (var assemblyAlreadyLoaded in assembliesAlreadyLoaded)
                    {
                        if (assemblyAlreadyLoaded.FullName == assemblyInAppDomain.FullName)
                        {
                            assemblyLoaded = true;
                            break;
                        }
                    }
                    //if (false == dontAdd)
                    //{
                    var newNode = newTreeNode(treeview.Nodes, assemblyInAppDomain.GetName().Name, 0, assemblyInAppDomain);
                    if (!string.IsNullOrEmpty(assemblyInAppDomain.Location))
                    {
                        var pdbFile = assemblyInAppDomain.Location.Replace(Path.GetExtension(assemblyInAppDomain.Location), ".pdb");
                        if (File.Exists(pdbFile))
                            newNode.ForeColor = Color.DarkGreen;
                    }
                    if (assemblyLoaded)
                        newNode.Checked = true;
                    //}
                }
                catch (Exception ex)
                {
                    ex.log("in loadTreeViewWith_AssembliesInCurrentAppDomain");
                }
            }
            if (showCheckBoxes)
                treeview.CheckBoxes = true;
        }

        public static void loadTreeViewWithDirectoriesAndFiles(TreeView tvTargetTreeView, String sDirectoryToProcess,
                                                               TextBox tbCurrentLoadedDirectory)
        {
            loadTreeViewWithDirectoriesAndFiles(tvTargetTreeView, sDirectoryToProcess, "*.*", tbCurrentLoadedDirectory);
        }

        public static void loadTreeViewWithDirectoriesAndFiles(TreeView tvTargetTreeView, String sDirectoryToProcess,
                                                               String sFileFilter, TextBox tbCurrentLoadedDirectory)
        {
            loadTreeViewWithDirectoriesAndFiles(tvTargetTreeView, sDirectoryToProcess, sFileFilter,
                                                tbCurrentLoadedDirectory, false,false /* hideFiles */);
        }

        public static void loadTreeViewWithDirectoriesAndFiles(TreeView tvTargetTreeView, String sDirectoryToProcess,
                                                               String sFileFilter, TextBox tbCurrentLoadedDirectory,
                                                               bool bShowFileSize, bool hideFiles)
        {
            try
            {
                if (tvTargetTreeView.InvokeRequired)
                    tvTargetTreeView.Invoke(
                        new EventHandler(
                            delegate
                                {
                                    loadTreeViewWithDirectoriesAndFiles(tvTargetTreeView, sDirectoryToProcess,
                                                                        sFileFilter, tbCurrentLoadedDirectory,
                                                                        bShowFileSize, hideFiles);
                                }));
                else
                {
                    if (sDirectoryToProcess != "")
                    {
                        tvTargetTreeView.Visible = false;
                        tvTargetTreeView.Nodes.Clear();
                        String sPreviousDirectory = Path.GetFullPath(Path.Combine(sDirectoryToProcess, ".."));
                        if (Directory.Exists(sPreviousDirectory))
                            tvTargetTreeView.Nodes.Add(newTreeNode("..", sPreviousDirectory, 2, sPreviousDirectory));

                        foreach (String sDirectory in Directory.GetDirectories(sDirectoryToProcess))
                        {
                            tvTargetTreeView.Nodes.Add(newTreeNode(Path.GetFileName(sDirectory), sDirectory, 0,
                                                                   sDirectory));
                        }

                        foreach (String sFile in Directory.GetFiles(sDirectoryToProcess, sFileFilter))
                        {
                            if (hideFiles)
                            {
                                var newNode = newTreeNode(Path.GetFileName(sFile), null, 1, null);
                                newNode.ForeColor = Color.Gray;
                                tvTargetTreeView.Nodes.Add(newNode);
                            }
                            else
                            {
                                if (bShowFileSize)
                                    tvTargetTreeView.Nodes.Add(
                                        newTreeNode("{0}     :     {1}k".format(Path.GetFileName(sFile),
                                                                                Files_WinForms.getFileSize(Path.Combine(sDirectoryToProcess, sFile)) / 1024),
                                            sFile, 1, sFile));
                                else
                                    tvTargetTreeView.Nodes.Add(newTreeNode(Path.GetFileName(sFile), sFile, 1, sFile));
                            }
                        }
                        tvTargetTreeView.Visible = true;

                        //   if (null != lCurrentLoadedDirectory)
                        //       lCurrentLoadedDirectory.Text = Path.GetFileName(sDirectoryToProcess);
                    }
                    else
                        PublicDI.log.error(
                            "in loadTreeViewWithDirectoriesAndFiles, sDirectoryToProcess != \"\", this value must be provided");
                }               
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in loadTreeViewWithDirectoriesAndFiles: {0}", ex.Message);
            }
        }

        public static void dataGridView_Utils_MaxColumnsWidth(DataGridView dgvToProcess)
        {
            if (dgvToProcess.IsDisposed)
                return;
            int icolumns = dgvToProcess.Columns.Count;
            for (int i = 0; i < icolumns; i++)
                dgvToProcess.Columns[i].Width = (dgvToProcess.Width - 100)/icolumns;
        }

        public static void loadComboBoxWithFilesFromDir(ComboBox cbComboBoxToPopulate, String sDirectory,
                                                        String sSearchPattern)
        {
            cbComboBoxToPopulate.Items.Clear();
            if (Directory.Exists(sDirectory))
            {
                foreach (String sFile in Directory.GetFiles(sDirectory, sSearchPattern))
                    cbComboBoxToPopulate.Items.Add(Path.GetFileName(sFile));
            }
            if (cbComboBoxToPopulate.Items.Count > 0)
                cbComboBoxToPopulate.SelectedIndex = 0;
        }

        public static void loadListBoxWithFilesFromDir(ListBox lbListBoxToPopulate, String sDirectory)
        {
            loadListBoxWithFilesFromDir(lbListBoxToPopulate, sDirectory, "*.*");
        }

        public static void loadListBoxWithFilesFromDir(ListBox lbListBoxToPopulate, String sDirectory,
                                                       String sSearchPattern)
        {
            // invoke in a thread safe way
            new loadListBoxWithFilesFromDir_ThreadSafe(lbListBoxToPopulate, sDirectory, sSearchPattern).populateListBox();
        }

        // this is generic so that I can use it for multiple control ListBox, ComboBox etc.. 
        public static void populateControlItemCollectionWithArray(Object oLiveObject, String[] asArray)
        {
            // get live instance of control's Items collection
            object oItems = oLiveObject.GetType().InvokeMember("Items", BindingFlags.GetProperty, null, oLiveObject,
                                                               null);

            // for each item in the array provided invoke the add methods from the oItems collection
            foreach (String sItem in asArray)
                oItems.GetType().InvokeMember("Add",
                                              BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
                                              null, oItems, new object[] {sItem});
        }

        // this is generic so that I can use it for multiple control ListBox, ComboBox etc.. 
        public static void populateControlItemCollectionWithArray(Control oLiveObject, UInt32[] auArray)
        {
            // get live instance of control's Items collection
            object oItems = oLiveObject.GetType().InvokeMember("Items", BindingFlags.GetProperty, null, oLiveObject,
                                                               null);

            // for each item in the array provided invoke the add methods from the oItems collection
            foreach (UInt32 uItem in auArray)
                oItems.GetType().InvokeMember("Add",
                                              BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
                                              null, oItems, new object[] {uItem});
        }

        // this is a more refined version of the above method: public static void populateControlItemCollectionWithArray(Object oLiveObject, String[] asArray)
        public static int populateWindowsControlWithList(Control targetControl, object itemsToAdd)
        {
            if (targetControl.okThread(delegate { populateWindowsControlWithList(targetControl, itemsToAdd); }))
            {
                object targetCollection = null;
                switch (targetControl.GetType().Name)
                {
                    case "ListBox":
                    case "ComboBox":
                    case "ListView":
                        targetCollection = PublicDI.reflection.getProperty("Items", targetControl);
                        break;
                    case "TreeView":
                        targetCollection = PublicDI.reflection.getProperty("Nodes", targetControl);
                        break;
                    default:
                        PublicDI.log.error("in populateWindowsControlWithList, unsupported object type: {0}",
                                     targetControl.GetType().Name);
                        break;
                }
                if (targetCollection != null)
                {
                    PublicDI.reflection.invokeMethod_InstanceStaticPublicNonPublic(targetCollection, "Clear", new object[0]);
                    foreach (object itemToAdd in (IEnumerable) itemsToAdd)
                    {
                        switch (targetControl.GetType().Name)
                        {
                            case "ListBox":
                            case "ComboBox":
                                PublicDI.reflection.invokeMethod_InstanceStaticPublicNonPublic(targetCollection, "Add",
                                                                                         new[] {itemToAdd});
                                break;
                            case "ListView":
                                var listViewItem = new ListViewItem(itemToAdd.ToString());
								listViewItem.Tag = itemToAdd;
								listViewItem.Name = itemToAdd.ToString();
                                PublicDI.reflection.invokeMethod_InstanceStaticPublicNonPublic(targetCollection, "Add",
                                                                                         new[] {listViewItem});
                                break;
                            case "TreeView":
                                TreeNode treeNode = newTreeNode(itemToAdd.ToString(), itemToAdd.ToString(), 0, itemToAdd);
                                PublicDI.reflection.invokeMethod_InstanceStaticPublicNonPublic(targetCollection, "Add",
                                                                                         new[] {treeNode});
                                break;
                            default:
                                PublicDI.log.error("in populateWindowsControlWithList, unsupported object type: {0}",
                                             targetControl.GetType().Name);
                                break;
                        }
                    }
                    return
                        (int)
                        PublicDI.reflection.invokeMethod_InstanceStaticPublicNonPublic(targetCollection, "get_Count", null);
                }
            }
            return 0;
        }




        // we need to do this because of the threading issues related with cross thread object access
        public static void setTextBoxValue_ThreadSafeWay(String sTextValue, TextBox tTargetTextBox)
        {
            var tSetTextBoxValue = new Thread(new setTextThread(sTextValue, tTargetTextBox).setDataReceivedTextBox);
            tSetTextBoxValue.Start();
        }

        public static void loadFileContentsIntoDataGridView(String sPathToFileToLoad, DataGridView dgvTargetDataGridView,
                                                            bool bShowItemsInReverseOrder)
        {
            new loadLogFileContentsIntoDataGridView_ThreadSafe(sPathToFileToLoad, dgvTargetDataGridView,
                                                               bShowItemsInReverseOrder).load();
        }


        public static void addRowToDataGridViewThreadSafe(DataGridView dgvTargetDataGridView, Object[] oRowItems)
        {
            var tNewThread = new Thread(new addRowToDataGridViewThread(dgvTargetDataGridView, oRowItems).addRow);
            tNewThread.Start();
        }


        // generic method that will invoke the provided method in a thread save way
        public static void executeMethodThreadSafe(Control cTargetControl, String sMethodName)
        {
            executeMethodThreadSafe(cTargetControl, cTargetControl, sMethodName, new object[] {});
        }

        // generic method that will invoke the provided method in a thread save way
        public static void executeMethodThreadSafe(Control cTargetControl, Object oTargetObject, String sMethodName,
                                                   Object[] oMethodParams)
        {
            new Thread(
                new executeMethod_inControl_Thread(cTargetControl, oTargetObject, sMethodName, oMethodParams).
                    invokeMethod).Start();
        }


        // generic method that will invoke the setText
        public static void setObjectTextValueThreadSafe(String sTextValue, Object oTargetObject)
        {
            var tNewThread = new Thread(new setObjectThread(sTextValue, oTargetObject).setText);
            tNewThread.Start();
        }


        public static TreeNode newTreeNode(TreeNode newNode, string text)
        {
            return newTreeNode(newNode, text, null);
        }

        public static TreeNode newTreeNode(TreeNode targetNode, string text, object tag)
        {
            return newTreeNode(targetNode, text, text, 0, tag);
        }

        public static TreeNode newTreeNode(TreeNodeCollection targetNodeCollection, string text, int imageIndex,object tag)
        
        {
            return newTreeNode(targetNodeCollection, text, imageIndex, tag, false);
        }

        public static TreeNode newTreeNode(TreeNodeCollection targetNodeCollection, string text, int imageIndex, object tag, bool addDummyNode)
        {
            var treeNode = newTreeNode(targetNodeCollection, text, text, imageIndex, tag);
            if (addDummyNode)
                treeNode.Nodes.Add("DymmyNode");
            return treeNode;
        }

        public static TreeNode newTreeNode(TreeNode targetNode, string text, int imageIndex, object tag)
        {
            return newTreeNode(targetNode.Nodes, text, imageIndex, tag);
        }        

        public static TreeNode newTreeNode(TreeNode targetNode, string text, string name, int imageIndex, object tag)
        {
            return newTreeNode(targetNode.Nodes, text, name, imageIndex, tag);
        }

        public static TreeNode newTreeNode(TreeNodeCollection targetNodeCollection, string text)
        {
            return newTreeNode(targetNodeCollection, text, 0, null);
        }

        public static TreeNode newTreeNode(TreeNodeCollection targetNodeCollection, string text, string name,
                                           int imageIndex, object tag)
        {
            TreeNode newNode = newTreeNode(text, name, imageIndex, tag);
            targetNodeCollection.Add(newNode);
            return newNode;
        }

        // create a new node object
        public static TreeNode newTreeNode(string text, string name, int imageIndex, object tag)
        {
            var tnNewNode = new TreeNode();
			tnNewNode.Name = name;
			tnNewNode.Text = text;
            tnNewNode.ImageIndex = tnNewNode.SelectedImageIndex = imageIndex;
            tnNewNode.Tag = tag;
            tnNewNode.ForeColor = Color.Black;  // to handle the weird 'treeView with 1 Node makes the TreeNode Text  white' bug
            return tnNewNode;
        }

        public static void newDataGridViewTextBoxColumn(DataGridView dgvTargetDataGridView, String sName,
                                                        String sHeaderValue, bool bReadOnly, Type tValueType)
        {
            dgvTargetDataGridView.Columns.Add(newDataGridViewTextBoxColumn(sName, sHeaderValue, bReadOnly, tValueType));
        }

        public static DataGridViewTextBoxColumn newDataGridViewTextBoxColumn(String sName, String sHeaderValue,
                                                                             bool bReadOnly, Type tValueType)
        {
            var dgvcNewTextBoxColumn = new DataGridViewTextBoxColumn();
			dgvcNewTextBoxColumn.ValueType = tValueType;
			dgvcNewTextBoxColumn.ReadOnly = bReadOnly;
			dgvcNewTextBoxColumn.HeaderText = sHeaderValue;
			dgvcNewTextBoxColumn.Name = sName;
            return dgvcNewTextBoxColumn;
        }

        public static DataGridViewComboBoxCell newDataGridViewComboBoxCell_forBooleanValues(bool bDefaultValue,
                                                                                            bool bReadOnly)
        {
            const string sTrue = "true";
            const string sFalse = "false";
            var dgvcNewComboBoxCell = new DataGridViewComboBoxCell();
            dgvcNewComboBoxCell.ReadOnly = bReadOnly;
            dgvcNewComboBoxCell.Items.AddRange(new[] {sTrue, sFalse});
            dgvcNewComboBoxCell.Value = bDefaultValue ? sTrue : sFalse;
            dgvcNewComboBoxCell.Tag = typeof (Boolean);
            return dgvcNewComboBoxCell;
        }

        public static String askUserForDirectory(String sStartDir)
        {
            try
            {
                using (var fbdFolderBrowserDialog = new FolderBrowserDialog())
                {
                	fbdFolderBrowserDialog.SelectedPath = sStartDir;
                    DialogResult drDialogResult = fbdFolderBrowserDialog.ShowDialog();
                    return drDialogResult == DialogResult.OK ? fbdFolderBrowserDialog.SelectedPath : "";
                }
            }
            catch (Exception ex)
            {
                ex.log("in askUserForDirectory");
                return "";
            }
        }
        public static String askUserForFileToOpen()
        {
            return askUserForFileToOpen(Environment.CurrentDirectory,"");
        }

        public static String askUserForFileToOpen(String proposedFolder)
        {
            return askUserForFileToOpen(proposedFolder, "");
        }
    
        public static String askUserForFileToOpen(String proposedFolder, string filter)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = proposedFolder;
                if (filter.valid())
                    openFileDialog.Filter = filter;
                DialogResult drDialogResult = openFileDialog.ShowDialog();
                if (drDialogResult == DialogResult.OK)
                    return openFileDialog.FileName;
            }
            catch (Exception ex)
            {
                ex.log("in askUserForFileToOpen");
            }
            return "";
        }

        public static String askUserForFileToSave(String proposedFolder)
        {
            return askUserForFileToSave(proposedFolder, "");
        }

        public static String askUserForFileToSave(string proposedFolder, string proposedFile)
        {
            try
            {
                string extension = Path.GetExtension(proposedFile);
               	var openFileDialog = new SaveFileDialog();
				openFileDialog.Filter = "{0} files (*{0})|*{0}".format(extension);
				openFileDialog.FileName = proposedFile;
				openFileDialog.InitialDirectory = proposedFolder;
                DialogResult drDialogResult = openFileDialog.ShowDialog();
                if (drDialogResult == DialogResult.OK)
                    return openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    ex.log("in askUserForFileToSave");
                    return "";
                }
            return "";
        }

        public static Form loadAscxControlAsMdiChild(Control cAscxControlToLoad, String sFormTitle)
        {
            return null;
        }

        public static Form loadAscxControlAsMdiChild(Type tFormHost, Type tTypeToLoad, Form fMdiParent,
                                                     String sFormTitle, String sParameter)
        {
            Object oForm = PublicDI.reflection.getInstance(tFormHost);
            if (oForm != null)
            {
                var fHostForm = (Form) oForm;
                fHostForm.MdiParent = fMdiParent;
                bool bResult = PublicDI.reflection.invokeMethod_returnSucess(fHostForm, "loadAscxControl",
                                                                       new Object[]
                                                                           {tTypeToLoad, sFormTitle, sParameter});
                if (fHostForm.Location.X != 0) // cases when the control wants to define were it is loaded
                {
                    fHostForm.StartPosition = FormStartPosition.Manual;
                    fHostForm.Location = fHostForm.Location;
                }
                if (bResult)
                {
                    fHostForm.Show();
                    return fHostForm;
                }
            }
            return null;
        }

        public static TreeNode getTreeNodeAtDroppedOverPoint(TreeView tvTreeView, int iDroppedX, int iDroppedY)
        {
            return tvTreeView.invokeOnThread(
                () =>{
                        Point pPointToString = tvTreeView.PointToScreen(tvTreeView.Location);

                        int iAdjustedX = tvTreeView.Left + iDroppedX - pPointToString.X;
                        int iAdjustedY = tvTreeView.Top + iDroppedY - pPointToString.Y;

                        //PublicDI.log.info("x:{0} y:{1}   - {2}, {3}", x, y, tvCurrentFilters.Left, tvCurrentFilters.Top);
                        return tvTreeView.GetNodeAt(iAdjustedX, iAdjustedY);
                    });
        }

        public static TreeView selectTreeNodeAtDroppedOverPoint(TreeView tvTreeView, int iDroppedX, int iDroppedY)
        {
            return tvTreeView.invokeOnThread(
                () =>{
                        TreeNode tnDraggedTarget = getTreeNodeAtDroppedOverPoint(tvTreeView, iDroppedX, iDroppedY);
                        if (tnDraggedTarget != null)
                            tvTreeView.SelectedNode = tnDraggedTarget;
                        return tvTreeView;
                    });
        }

        public static Control findParentThatHostsControl(Control controlToFind)
        {
            return controlToFind.invokeOnThread(
                () =>{
                        if (controlToFind != null)
                        {
                            IEnumerable<Control> results = from Control control in controlToFind.Parent.Controls
                                                           where control == controlToFind
                                                           select controlToFind.Parent;
                            if (results.Count() == 1)
                                return results.First();
                            findParentThatHostsControl(controlToFind.Parent);
                        }
                        return null;
                });
        }
       

        #region Nested type: addRowToDataGridViewThread

        private class addRowToDataGridViewThread
        {
            private readonly DataGridView dgvTargetDataGridView;
            private readonly Object[] oRowItems;

            public addRowToDataGridViewThread(DataGridView dgvTargetDataGridView, Object[] oRowItems)
            {
                this.dgvTargetDataGridView = dgvTargetDataGridView;
                this.oRowItems = oRowItems;
            }

            public void addRow()
            {
                try
                {
                    if (dgvTargetDataGridView.InvokeRequired)
                        dgvTargetDataGridView.Invoke(new addRowCallback(addRow));
                    else
                        dgvTargetDataGridView.Rows.Add(oRowItems);
                }
                catch (Exception ex)
                {
                    PublicDI.log.debug("in addRowToDataGridViewThread.addRow: {0}", ex.Message);
                }
            }

            #region Nested type: addRowCallback

            private delegate void addRowCallback();

            #endregion
        }

        #endregion

        #region Nested type: executeMethod_inControl_Thread

        private class executeMethod_inControl_Thread
        {
            private readonly Control cTargetControl;
            private readonly Object[] oMethodParams;
            private readonly Object oTargetObject;
            private readonly String sMethodName;


            public executeMethod_inControl_Thread(Control cTargetControl, Object oTargetObject, String sMethodName,
                                                  Object[] oMethodParams)
            {
                this.cTargetControl = cTargetControl;
                this.oTargetObject = oTargetObject;
                this.sMethodName = sMethodName;
                this.oMethodParams = oMethodParams;
            }

            public void invokeMethod()
            {
                if (cTargetControl.InvokeRequired)
                {
                    cTargetControl.Invoke(new dCallback(invokeMethod));
                    //cTargetControl.Invoke
                    //dCallback dInvokeMethod = new dCallback(invokeMethod);
                    //reflection.invokeMethod_returnSucess(oTargetObject, "Invoke", new object[] { dInvokeMethod });                        
                }
                else
                {
                    PublicDI.reflection.invokeMethod_returnSucess(oTargetObject, sMethodName, oMethodParams);
                }
            }

            #region Nested type: dCallback

            private delegate void dCallback();

            #endregion
        }

        #endregion

        #region Nested type: loadListBoxWithFilesFromDir_ThreadSafe

        private class loadListBoxWithFilesFromDir_ThreadSafe
        {
            private readonly ListBox lbListBoxToPopulate;
            private readonly String sDirectory;
            private readonly String sSearchPattern;

            public loadListBoxWithFilesFromDir_ThreadSafe(ListBox lbListBoxToPopulate, String sDirectory,
                                                          String sSearchPattern)
            {
                this.lbListBoxToPopulate = lbListBoxToPopulate;
                this.sDirectory = sDirectory;
                this.sSearchPattern = sSearchPattern;
            }

            public void populateListBox()
            {
                try
                {
                    if (lbListBoxToPopulate.InvokeRequired)
                    {
                        populateCallback pcPopulcateCallback = populateListBox;
                        lbListBoxToPopulate.Invoke(pcPopulcateCallback);
                    }
                    else
                    {
                        lbListBoxToPopulate.Items.Clear();
                        if (Directory.Exists(sDirectory))
                        {
                            foreach (String sFile in Directory.GetFiles(sDirectory, sSearchPattern))
                                lbListBoxToPopulate.Items.Add(Path.GetFileName(sFile));
                        }
                    }
                }
                catch (Exception ex)
                {
                    PublicDI.log.error("in loadListBoxWithFilesFromDir_ThreadSafe.populateListBox:  {0}", ex.Message);
                }
            }

            #region Nested type: populateCallback

            private delegate void populateCallback();

            #endregion
        }

        #endregion

        #region Nested type: loadLogFileContentsIntoDataGridView_ThreadSafe

        private class loadLogFileContentsIntoDataGridView_ThreadSafe
        {
            private readonly bool bShowItemsInReverseOrder;
            private readonly DataGridView dgvTargetDataGridView;
            private readonly String sPathToFileToLoad;

            public loadLogFileContentsIntoDataGridView_ThreadSafe(String sPathToFileToLoad,
                                                                  DataGridView dgvTargetDataGridView,
                                                                  bool bShowItemsInReverseOrder)
            {
                this.dgvTargetDataGridView = dgvTargetDataGridView;
                this.sPathToFileToLoad = sPathToFileToLoad;
                this.bShowItemsInReverseOrder = bShowItemsInReverseOrder;
            }

            public void load()
            {
                if (dgvTargetDataGridView.InvokeRequired)
                {
                    loadCallback lcLoadCallback = load;
                    dgvTargetDataGridView.Invoke(lcLoadCallback);
                }
                else
                {
                    int iMaxRowsToShow = 1000; // make this a global seeting
                    DateTime dtStart = DateTime.Now;
                    String sContentsOfLogFile = Files.getFileContents(sPathToFileToLoad);
                    String[] sSplittedFileContents = sContentsOfLogFile.Split(new[] {Environment.NewLine},
                                                                              StringSplitOptions.RemoveEmptyEntries);
                    PublicDI.log.debug("There are {0} entries in the log file, only the first {1} will be shown",
                                 sSplittedFileContents.Length, iMaxRowsToShow);


                    dgvTargetDataGridView.Columns.Clear();
                    dgvTargetDataGridView.Columns.Add("Line", "Line");
                    // ReSharper disable PossibleNullReferenceException
                    dgvTargetDataGridView.Columns["Line"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    // ReSharper restore PossibleNullReferenceException

                    foreach (String sLine in sSplittedFileContents)
                    {
                        String sFilteredLine = sLine.Replace("\t", "").Replace("\r", "").Replace(Environment.NewLine, "");
                        if (bShowItemsInReverseOrder)
                            dgvTargetDataGridView.Rows.Insert(0, sFilteredLine);
                        else
                            dgvTargetDataGridView.Rows.Add(sFilteredLine);
                        if (iMaxRowsToShow-- < 1)
                        {
                            PublicDI.log.debug("iMaxRowsToShow reached, aborting data load");
                            break;
                        }
                    }
                    dataGridView_Utils_MaxColumnsWidth(dgvTargetDataGridView);
                    TimeSpan tsTime = DateTime.Now - dtStart;
                    PublicDI.log.info("Loaded file into DataGridView in  {0}.{1} seconds", tsTime.Seconds.ToString(),
                                tsTime.Milliseconds.ToString());
                }
            }

            #region Nested type: loadCallback

            private delegate void loadCallback();

            #endregion
        }

        #endregion

        #region Nested type: setObjectThread

        private class setObjectThread
        {
            private readonly Object oTargetObject;
            private readonly String sTextValue;

            public setObjectThread(String sTextValue, Object oTargetObject)
            {
                this.sTextValue = sTextValue;
                this.oTargetObject = oTargetObject;
            }

            public void setText()
            {
                Object oInvokeRequired = PublicDI.reflection.getProperty("InvokeRequired", oTargetObject);
                if (oInvokeRequired != null)
                {
                    var bInvokeRequired = (bool) oInvokeRequired;
                    if (bInvokeRequired)
                    {
                        dCallback dSetText = setText;
                        PublicDI.reflection.invokeMethod_returnSucess(oTargetObject, "Invoke", new object[] {dSetText});
                        //this.oTargetObject.Invoke(dSetText);
                    }
                    else
                    {
                        PublicDI.reflection.invokeMethod_returnSucess(oTargetObject, "set_Text", new object[] {sTextValue});
                    }
                }
            }

            #region Nested type: dCallback

            private delegate void dCallback();

            #endregion
        }

        #endregion

        #region Nested type: setTextThread

        private class setTextThread
        {
            private readonly String sTextValue;
            private readonly TextBox tTargetTextBox;

            public setTextThread(String sTextValue, TextBox tTargetTextBox)
            {
                this.sTextValue = sTextValue;
                this.tTargetTextBox = tTargetTextBox;
            }

            // from MSDN example

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            public void setDataReceivedTextBox()
            {
                try
                {
                    if (tTargetTextBox.InvokeRequired)
                    {
                        SetTextCallback d = setDataReceivedTextBox;
                        tTargetTextBox.Invoke(d);
                    }
                    else
                    {
                        tTargetTextBox.Text = sTextValue;
                        //this.tTargetTextBox.AppendText(sTextValue + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                   PublicDI.log.error("in setDataReceivedTextBox: {0}", ex.Message);
                }
            }

            #region Nested type: SetTextCallback

            private delegate void SetTextCallback();

            #endregion
        }

        #endregion

        public static void addNodeToTreeNodeCollection(Control controlInCorrectThread, TreeNodeCollection targetTreeNodeCollection, TreeNode newNode)
        {
            addNodeToTreeNodeCollection(controlInCorrectThread, targetTreeNodeCollection, newNode, -1);
        }
        /// <summary>
        /// Async Thread safe way to add nodes to TreeViews
        /// </summary>       
        public static void addNodeToTreeNodeCollection(Control controlInCorrectThread, TreeNodeCollection targetTreeNodeCollection, TreeNode newNode, int maxNodesToAdd)
        {
            //            if (controlInCorrectThread.okThread(delegate { addNodeToTreeNodeCollection(controlInCorrectThread,targetTreeNodeCollection, newNode); }))
            controlInCorrectThread.invokeOnThread(
                () =>
                {
                    if (maxNodesToAdd == -1 || targetTreeNodeCollection.Count < maxNodesToAdd)
                        if (Thread.CurrentThread.IsAlive)
                            targetTreeNodeCollection.Add(newNode);
                });
        }

        public static void closeForm(Form formToClose)
        {            
            if (formToClose != null)
                if(formToClose.okThread(delegate{closeForm(formToClose);}))
                {
                    PublicDI.log.info("Closing Form: {0} or type  {1}" , formToClose.Text, formToClose.GetType().FullName);
                    formToClose.Close();
                }
        }

        public static void closeParentForm(ContainerControl controlToClose)
        {
            if (controlToClose!= null)
                if (controlToClose.okThread(delegate { closeParentForm(controlToClose); }))
                {
                    if (controlToClose.ParentForm == null)
                        PublicDI.log.error(
                            "in O2Forms.closeParentForm the requested control to close had no parent Form: {0}",
                            controlToClose.ToString());
                    else
                        controlToClose.ParentForm.Close();
                }
        }

        public static void expandNodes(TreeView targetTreeView)
        {
            if (targetTreeView != null)
                targetTreeView.invokeOnThread(
                    ()=>
                        {
                           foreach(TreeNode node in targetTreeView.Nodes)
                               node.Expand();
                        });                   
        }

        public static TreeNode cloneTreeNode(TreeNode treeNode)
        {
            if (treeNode == null)
                return null;
            return (TreeNode) treeNode.Clone();            
        }

        public static void SetFocusOnControl(Control targetControl, int sleepBeforeFocus)
        {
            // first start a separate thread to wait before jumping into the targetControl to set the focus
            O2Thread.mtaThread(
                ()=>
                    {
                    Thread.Sleep(sleepBeforeFocus);
                    targetControl.invokeOnThread(()=>targetControl.Focus());
                    });
        }
           
		public static void setAscxPosition(object control, int top, int left)
		{
			setAscxPosition(control, top, left, -1, -1);
		}
		
		public static void setAscxPosition(object control, int top, int left, int width, int height)
        {        	
        	if (control != null & control is Control)
        	{
        		var parentForm =(Form)O2Forms.findParentThatHostsControl((Control)control);
        		setAscxPosition(parentForm,top, left,width,height);
        	}
        }
        
        public static void setAscxPosition(Form form, int top, int left, int width, int height)
        {
        	PublicDI.log.info("Setting form position: {0} , {1}.{2} x {3}.{4}", form.Name,top, left,width,height);
        	form.Top = top;
        	form.Left = left;
        	form.Width = (width > 0) ? width : form.Width;
        	form.Height = (height >0) ? height : form.Height;
        }

        public static void setToolTipText(TreeView treeView, TreeNode treeNode, string toolTipText)
        {
            treeView.invokeOnThread(
                () => treeNode.ToolTipText = toolTipText);
        }

        public static void setTreeNodeColor(TreeView treeView, TreeNode treeNode, Color color)
        {
            if (treeNode != null)
                treeView.invokeOnThread(
                    () => treeNode.ForeColor = color);
        }
    }
}
