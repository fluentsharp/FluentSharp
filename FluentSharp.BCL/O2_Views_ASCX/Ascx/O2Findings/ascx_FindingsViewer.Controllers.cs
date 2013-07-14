// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.O2Findings;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_FindingsViewer
    {
        public static List<IO2AssessmentLoad> o2AssessmentLoadEngines = new List<IO2AssessmentLoad>();
        public static IO2AssessmentSave o2AssessmentSave;

        public bool runOnLoad = true;
        private const int icon_findingWithNoTrace = 2;
        private const int icon_findingWithTrace = 1;
        private const int icon_folder = 0;
        private const string noFilterStringTagForFilter2 = "(no Filter)";
        private bool currentlyDraggingNodeFromTreeView;
        private int maxNumberOfFindingsToLoad { get; set; }

        public List<IO2Finding> currentO2Findings = new List<IO2Finding>();
        public Callbacks.dMethod_Object eventFindingSelected;
        public event Action<IO2Finding> _onFindingSelected;
        public event Action<IO2Trace> _onTraceSelected;
        public event Action<string> _onFolderSelectEvent;

        public string assessmentName = "";
        //public Dictionary<string, IO2Finding> currentFilteredText = new Dictionary<string, IO2Finding>(); 
        //public Dictionary<IO2Finding, string> currentFilteredTextMappings = new Dictionary<IO2Finding, string>(); 

        private void onLoad()
        {
            if (runOnLoad && DesignMode == false)
            {
                this.invokeOnThread(() =>
                                        {
                                            //   OzasmtMappedToWindowsForms.loadIntoCombox_O2FindingFieldsNames((cbFilter1), "severity");
                                            OzasmtMappedToWindowsForms.loadIntoToolStripCombox_O2FindingFieldsNames(
                                                (cbFilter1), "vulnType");                                                                                        
                                            OzasmtMappedToWindowsForms.loadIntoToolStripCombox_O2FindingFieldsNames(
                                                (cbFilter2), "");                                   // we can't set the default value here
                                            cbFilter2.Items.Insert(0, noFilterStringTagForFilter2); // since this would push it down
                                            cbFilter2.Text = "vulnName";                            // so we add it here
                                            tbSavedFileName.Text = PublicDI.config.TempFileNameInTempDirectory + ".ozasmt";
                                            maxNumberOfFindingsToLoad = 1000;
                                            tbMaxRecordsToShow.Text = maxNumberOfFindingsToLoad.ToString();
                                            laNoAssessmentLoadEnginesLoaded.Visible = (o2AssessmentLoadEngines.Count == 0);
                                            runOnLoad = false;
                                            return false;
                                        });
            }
        }
        
        public Thread loadO2Assessment(string pathToFileToLoad)
        {
            if (Path.GetExtension(pathToFileToLoad) == PublicDI.config.O2FindingsFileExtension)
            {
                var o2Assessment = (IO2Assessment)Serialize.getDeSerializedObjectFromBinaryFile(pathToFileToLoad, typeof(O2Assessment));
                if (o2Assessment != null)
                {
                    PublicDI.log.info("Sucessfuly created O2Findings object from file: {0}", pathToFileToLoad);
                    loadO2Assessment(o2Assessment);
                }
                else
                    PublicDI.log.error("There was a problem deserializing O2Findings  object saved to: {0}", pathToFileToLoad);
                return null;
            }
            else
            {
                var o2AssessmentLoad = OzasmtUtils.getO2AssessmentLoadEngine(pathToFileToLoad, o2AssessmentLoadEngines);
                return loadO2Assessment(o2AssessmentLoad, pathToFileToLoad);
            }
        }

        public Thread loadO2Assessment(IO2AssessmentLoad o2AssessmentLoad, string pathToFileToLoad)
        {
            if (o2AssessmentLoad == null || false == File.Exists(pathToFileToLoad))
            {
                this.invokeOnThread(() => laLoadingDroppedFile.Visible = false);
                return null;
            }
            return O2Thread.mtaThread(() =>
                                          {
                                              this.invokeOnThread(() => laLoadingDroppedFile.Visible = true);

                                              var o2Assemment = new O2Assessment(o2AssessmentLoad, pathToFileToLoad);
                                                  // load this on another thread 
                                              var sync = new AutoResetEvent(false);
                                              this.invokeOnThread(() => // and then complete it on the controls thread
                                                                      {
                                                                          loadO2Assessment(o2Assemment);
                                                                          tbSavedFileName.Text =
                                                                              (cbClearOnOzasmtDrop.Checked)
                                                                                  ? pathToFileToLoad
                                                                                  : PublicDI.config.TempFileNameInTempDirectory + "_" + Path.GetFileName(pathToFileToLoad);
                                                                          laLoadingDroppedFile.Visible = false;
                                                                          sync.Set();
                                                                      });
                                              sync.WaitOne();
                                          });

        }

        public void loadO2Assessment(IO2Assessment o2Assessment)
        {
            this.invokeOnThread(() =>
                                    {
                                        assessmentName = o2Assessment.name;
                                        if (cbClearOnOzasmtDrop.Checked)
                                            clearO2Findings();
                                        loadO2Findings(o2Assessment.o2Findings);
                                    });
        }

        
        public void loadO2Findings(List<IO2Finding> o2Findings)
        {
            loadO2Findings(o2Findings, false);
        }

        public void loadO2Findings(List<IO2Finding> o2Findings, bool clearLoadedFindings)
        {
            this.invokeOnThread(() =>
                                    {                                       
                                        if (o2Findings == null)
                                            return;
                                        if (clearLoadedFindings)
                                            currentO2Findings = new List<IO2Finding>();

                                        if (o2Findings.Count > 10000)
                                        {
                                            PublicDI.log.debug( "NOTE: since are more that 10000 findings to load, for performance reasons, the check for duplicated findings objects was disabled");
                                            currentO2Findings.AddRange(o2Findings);
                                        }
                                        else
                                        {
                                            foreach (IO2Finding o2Finding in o2Findings)
                                                addO2Finding(o2Finding, false);
                                        }
                                        showCurrentO2Findings();                                    
                                    });
        }

        public void addO2Finding(IO2Finding o2Finding)
        {
            this.invokeOnThread(() => addO2Finding(o2Finding, true));
            
        }

        public void addO2Finding(IO2Finding o2Finding, bool refresh)
        {
            try
            {
                if (false == currentO2Findings.Contains(o2Finding))
                    if (cbDontLoadFindingsWithNoTraces.Checked == false || o2Finding.o2Traces.Count > 0)
                        currentO2Findings.Add(o2Finding);
                if (refresh)
                    showCurrentO2Findings();
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in addO2Finding: {0}", ex.Message);
            }
        }

        public void showCurrentO2Findings()
        {
            this.invokeOnThread(() => buildO2FindindTraceTree(ascxTraceTreeView.Visible));
        }

        private void buildO2FindindTraceTree(bool onlyShowFindingsWithTraces)
        {
            this.invokeOnThread(() =>
                                    {
                                        laAlertOnNotAllFindingsShown.Visible = false; 
                                        lbNumberOfFindingsLoaded.ForeColor = Color.Black;
                                        tvFindings.Visible = false;
                                        tvFindings.Nodes.Clear();
                                        tvFindings.Sorted = true;
                                        if (cbFilter2.Text == noFilterStringTagForFilter2)
                                            buildTreeWithOneFilter(onlyShowFindingsWithTraces);
                                        else
                                            buildTreeWithTwoFilters(onlyShowFindingsWithTraces);
                                        canWeStillAddMoreNodes(0, true);
                                        tvFindings.Visible = true;
                                       // Application.DoEvents();         
                                        tvFindings.Refresh();
                                        lbNumberOfFindingsLoaded.Text = getNumberOfFidingsText();
                                    });
        }

        private string getNumberOfFidingsText()
        {
            //var timer = new O2Timer("getNumberOfFidingsText").start();
            var numberOfFindingsLoaded = currentO2Findings.Count;
            int numberOfFindingsInTreeView = getFindingsFromTreeView().Count;
            int numberOfTraces = 0;            
            foreach (var o2Finding in currentO2Findings)
                if (o2Finding.o2Traces.Count > 0)
                    numberOfTraces++;
            //timer.stop();
            return "loaded:{0}, after text filter:{1} ({2} traces) ".format(numberOfFindingsLoaded, numberOfFindingsInTreeView,numberOfTraces);
        }
        

        private void buildTreeWithTwoFilters(bool onlyShowFindingsWithTraces)
        {
        //    currentFilteredTextMappings = new Dictionary<IO2Finding, string>();
            var mappings = new Dictionary<String, List<O2Finding>>();
            var propertyToUseOnRootNode = cbFilter1.Text;
            var propertyToUseOnChildNode = cbFilter2.Text;
            var nodeFilterForRootNode = tbFilter1Text.Text;
            foreach (O2Finding o2Finding in currentO2Findings)
            {
                if (onlyShowFindingsWithTraces == false || o2Finding.o2Traces.Count > 0)
                {
                    string nodeText = calculateTreeNodeText(o2Finding, propertyToUseOnRootNode, nodeFilterForRootNode);                    
                    if (nodeText != "")
                    {
                        if (false == mappings.ContainsKey(nodeText))
                            mappings.Add(nodeText, new List<O2Finding>());
                        mappings[nodeText].Add(o2Finding);
          //              currentFilteredTextMappings.Add(o2Finding, nodeText);
                    }
                }
            }

            foreach (string nodeText in mappings.Keys)
            {
                var childNodes = new List<TreeNode>();                
                foreach (O2Finding o2ChildFinding in mappings[nodeText])
                {
                    var subNodeText = calculateTreeNodeText(o2ChildFinding, propertyToUseOnChildNode,"");
                    if (subNodeText != "")
                    {
                        childNodes.Add(getO2FindingTreeNode(o2ChildFinding, subNodeText, propertyToUseOnChildNode));
                        //if (!canWeStillAddMoreNodes(treeNode.Nodes.Count, false))
                        //    break;
                    }
                }
                TreeNode treeNode = O2Forms.newTreeNode(nodeText, cbFilter1.Text,icon_folder, childNodes);
                //mappings[nodeText]);
                // add child node count
                if (childNodes.Count > 0)
                {
                    treeNode.Text += "  ({0})".format(childNodes.Count);
                    treeNode.Nodes.Add("Dummy node"); // so that we get the + sign
                    tvFindings.Nodes.Add(treeNode);
                }
                if (!canWeStillAddMoreNodes(0, true))
                    break;
            }
        }

        private void buildTreeWithOneFilter(bool onlyShowFindingsWithTraces)
        {
            //currentFilteredTextMappings = new Dictionary<IO2Finding, string>();
            var treeNodeCollectionToAppendFinding = tvFindings.Nodes;
            var propertyToUse = cbFilter1.Text;
            var nodeFilter = tbFilter1Text.Text;
            foreach (O2Finding o2Finding in currentO2Findings)
            {
                if (onlyShowFindingsWithTraces == false || o2Finding.o2Traces.Count > 0)
                {
                    var nodeText = calculateTreeNodeText(o2Finding, propertyToUse, nodeFilter);
                    if (nodeText != "")
                    {
                        treeNodeCollectionToAppendFinding.Add(getO2FindingTreeNode(o2Finding, nodeText,propertyToUse));
                      //  currentFilteredTextMappings.Add(o2Finding, nodeText);
                        if (!canWeStillAddMoreNodes(0, true))
                            break;

                    }
                }
            }
        }

        private static string calculateTreeNodeText(IO2Finding o2Finding, string propertyToUse, string filterToUse)
        {
            string nodeText;
            try
            {
                switch (propertyToUse)
                {
                    case "severity":
                        return OzasmtUtils.getSeverityFromId(o2Finding.severity);

                    case "confidence":
                        return OzasmtUtils.getConfidenceFromId(o2Finding.confidence);

                    case "o2Traces":
                        var allO2Traces = OzasmtUtils.getAllTraces(o2Finding.o2Traces);
                        return (allO2Traces.Keys.Count > 0) ? "# nodes: {0}".format(allO2Traces.Keys.Count) : "";

                    default:
                        nodeText = PublicDI.reflection.getProperty(propertyToUse, o2Finding).ToString();
                        break;
                }
                if (nodeText != "")
                    if (RegEx.findStringInString(nodeText, filterToUse) || nodeText.index(filterToUse) > -1)
                        return nodeText;
                    else
                        return "";
                return nodeText;
            }
            catch (Exception ex)
            {                
                PublicDI.log.error("in calculateTreeNodeText: {0}", ex.Message);
                return "[O2 Error (check logs for details)]"; 
            }
        }
        

        public bool canWeStillAddMoreNodes(int subNodeCount, bool showMessage)
        {
            if (tvFindings.GetNodeCount(true) + subNodeCount > maxNumberOfFindingsToLoad)
            {
                if (showMessage)
                {
                    lbNumberOfFindingsLoaded.Text += " NOTE: Only " + maxNumberOfFindingsToLoad + " loaded";
                    lbNumberOfFindingsLoaded.ForeColor = Color.Red;
                    laAlertOnNotAllFindingsShown.Visible = true;
                }
                return false;
            }
            return true;
        }

        public TreeNode getO2FindingTreeNode( IO2Finding o2Finding,string treeNodeText, string propertyUsed)
        {
            return O2Forms.newTreeNode(treeNodeText, propertyUsed,
                                       (o2Finding.o2Traces.Count > 0)
                                           ? icon_findingWithTrace
                                           : icon_findingWithNoTrace
                                       , o2Finding);
        }

        public void clearO2Findings()
        {            
            currentO2Findings = new List<IO2Finding>();
            this.invokeOnThread(() => tvFindings.Nodes.Clear());
        }

        public void deleteNode(TreeNode nodeToDelete)
        {
            if (nodeToDelete != null)
            {                
                if (nodeToDelete.Tag.GetType().Name == "O2Finding")
                    currentO2Findings.Remove((O2Finding)nodeToDelete.Tag);
                else if (nodeToDelete.Tag is List<TreeNode>)
                    {
                        foreach (TreeNode childNode in (List<TreeNode>)nodeToDelete.Tag)
                            if (childNode.Tag != null && childNode.Tag is IO2Finding) 
                                currentO2Findings.Remove((IO2Finding)childNode.Tag);
                }
                else
                    foreach (TreeNode childNode in nodeToDelete.Nodes)
                        currentO2Findings.Remove((O2Finding)childNode.Tag);

                showCurrentO2Findings();
            }
        }

        public void ApplyTextChangeToNode(TreeNode rootNode, string newText)
        {
            try
            {                
                if (rootNode.Tag.GetType().Name == "O2Finding")
                {
                    var o2Finding = (O2Finding) rootNode.Tag;
                    PublicDI.reflection.setProperty(rootNode.Name, o2Finding, newText);
                    //o2Finding.setField(rootNode.Name, newText);
                    rootNode.Text = newText;
                }
                else if (rootNode.Tag is List<TreeNode>)
                {                                            
                    foreach (TreeNode treeNode in (List<TreeNode>)rootNode.Tag)
                        if (treeNode.Tag != null && treeNode.Tag is IO2Finding)
                        {                            
                            var o2Finding = (O2Finding)treeNode.Tag;
                            if (rootNode.Name == "confidence" || rootNode.Name == "severity")
                                PublicDI.reflection.setProperty(rootNode.Name, o2Finding, byte.Parse(newText));
                            else
                                PublicDI.reflection.setProperty(rootNode.Name, o2Finding, newText);
                        }
                }
                else
                {
                    rootNode.Text = newText;
                    foreach (TreeNode childNode in rootNode.Nodes)
                    {
                        if (childNode.Tag.GetType().Name == "O2Finding")
                        {
                            var o2Finding = (O2Finding)childNode.Tag;
                            PublicDI.reflection.setProperty(rootNode.Name, o2Finding, newText);
                            //o2Finding.setField(rootNode.Name, newText);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                PublicDI.log.error("in ApplyTextChangeToNode: {0}", ex.Message);
            }
        }


        public void registerSelectedFindingEventCallback(Callbacks.dMethod_Object callback)
        {
            eventFindingSelected += callback;
        }        

        public void setViewMode_Simple()
        {
            setFilter1Value("vulnName");
            setFilter2Value(noFilterStringTagForFilter2);
        }

        

        private void handleTreeViewDropEvent(DragEventArgs e)
        {
            O2Thread.mtaThread(() =>
                                   {

                                       this.invokeOnThread(() => laLoadingDroppedFile.Visible = true);
                                        
                                       // see if we dropped an O2 Finding
                                       object o2Finding = Dnd.tryToGetObjectFromDroppedObject(e, typeof (IO2Finding));
                                       if (o2Finding != null)
                                       {
                                           addO2Finding((IO2Finding)o2Finding);
                                       }
                                       else
                                       {
                                           object o2Findings = Dnd.tryToGetObjectFromDroppedObject(e,typeof (List<IO2Finding>));
                                           if (o2Findings != null)
                                           {
                                               loadO2Findings((List<IO2Finding>)o2Findings);
                                               //addO2Findings((O2Finding)o2Finding);                                               
                                           }
                                           else
                                           {
                                               // see if we dropped an O2 Finding
                                               var treeNode =
                                                   (TreeNode) Dnd.tryToGetObjectFromDroppedObject(e, typeof (TreeNode));
                                               if (treeNode != null)
                                               {
                                                   // addTreeNode
                                                   if (treeNode.Tag.GetType().Name == "O2Finding")
                                                       addO2Finding((O2Finding)treeNode.Tag);
                                                   else
                                                       foreach (TreeNode childNode in treeNode.Nodes)
                                                           if (childNode.Tag !=null && childNode.Tag.GetType().Name == "O2Finding")
                                                               addO2Finding((O2Finding)childNode.Tag);                                                   
                                               }
                                               else
                                               {
                                                   // see if we dropped an Ozasmt file to Load
                                                   string fileOrDirectoryToLoad = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
                                                   if (fileOrDirectoryToLoad != "")
                                                   {
                                                       if (Directory.Exists(fileOrDirectoryToLoad))
                                                       {
                                                           // if it is a directly lets deleted the current findings and uncheck cbClearOnOzasmtDrop
                                                           this.invokeOnThread(() =>
                                                                                   {
                                                                                       cbClearOnOzasmtDrop.Checked =
                                                                                           false;
                                                                                       clearO2Findings();
                                                                                   });
                                                           this.invokeOnThread(
                                                               () => laLoadingDroppedFile.Visible = false);
                                                           // in case there is no ozasmt files
                                                           foreach (
                                                               var ozasmtFile in
                                                                   Files.getFilesFromDir_returnFullPath(
                                                                       fileOrDirectoryToLoad, "*.ozasmt"))
                                                               loadO2Assessment(ozasmtFile);
                                                           return;
                                                       }
                                                       else if (File.Exists(fileOrDirectoryToLoad))
                                                       {
                                                           loadO2Assessment(fileOrDirectoryToLoad);
                                                           return;
                                                       }
                                                   }
                                                   else                     // finally try to use it as a filter
                                                   {
                                                       var objectDropped = Dnd.tryToGetObjectFromDroppedObject(e);
                                                       if (objectDropped is FilteredSignature)
                                                           filterByFilteredSignature((FilteredSignature)objectDropped);
                                                       else if (objectDropped is List<FilteredSignature>)
                                                           filterByFilteredSignatures((List<FilteredSignature>)objectDropped);
                                                       else
                                                           setFilter1TextValue(objectDropped.ToString(), true);
                                                   }
                                               }
                                           }
                                       }
                                       this.invokeOnThread(() => laLoadingDroppedFile.Visible = false);
                                   });
        }

        private void saveFindings(IEnumerable<IO2Finding> o2FindingsToSave, bool saveIntoO2BinaryFormat)
        {
            btSaveFindings.Enabled = false;
            btSave.Enabled = false;

            if (o2AssessmentSave == null)
                //PublicDI.log.showMessageBox("Aborting save since there is no O2AssessmentSave Engine configured");
                PublicDI.log.error("Aborting save since there is no O2AssessmentSave Engine configured");
            {

                OzasmtCompatibility.makeCompatibleWithOunceV6(o2FindingsToSave);

                string targetFile = tbSavedFileName.Text;
                var o2Assessment = new O2Assessment();
                o2Assessment.name = assessmentName;
                o2Assessment.o2Findings.AddRange(o2FindingsToSave);
                if (saveIntoO2BinaryFormat)
                {
                    if (Path.GetExtension(targetFile) != PublicDI.config.O2FindingsFileExtension)
                    {
                        targetFile += PublicDI.config.O2FindingsFileExtension;
                        tbSavedFileName.Text = targetFile;
                    }
                    if (o2Assessment.saveAsO2Format(targetFile))
                        lbFileSaved.Visible = true;

                }
                else
                    if (o2Assessment.save(o2AssessmentSave, targetFile))
                        lbFileSaved.Visible = true;
                btSaveFindings.Enabled = true;
                btSave.Enabled = true;
            }
        }

        private void deleteSelectedNode()
        {
            int selectedNodeIndex = tvFindings.SelectedNode.Index;
            deleteNode(tvFindings.SelectedNode);
            if (selectedNodeIndex == tvFindings.Nodes.Count)
                selectedNodeIndex = tvFindings.Nodes.Count - 1;
            if (tvFindings.Nodes.Count > 0 && selectedNodeIndex < tvFindings.Nodes.Count)
            {
                tvFindings.SelectedNode = tvFindings.Nodes[selectedNodeIndex];
            }
            tvFindings.Focus();
        }

        private void onFindingsTreeViewAfterSelect()
        {
            tbSelectedNodeTextValue.Text = tvFindings.SelectedNode.Text;
            
            if (tvFindings.SelectedNode.Tag != null && tvFindings.SelectedNode.Tag is O2Finding)
            {
                var o2Finding = (O2Finding) tvFindings.SelectedNode.Tag;
                Callbacks.raiseRegistedCallbacks(eventFindingSelected, new[] { o2Finding });
                Callbacks.raiseRegistedCallbacks(_onFindingSelected, new[] { o2Finding });
                if (ascxTraceTreeView.Visible)                
                    ascxTraceTreeView.loadO2Finding(o2Finding);
                if (cbShowLineInSourceFile.Checked)                
                    O2Messages.fileOrFolderSelected(o2Finding.file, (int) o2Finding.lineNumber);                

            }
            else 
            {
                var textValue = getSelectedNodeFilter1Text();
                if (textValue != null)
                    Callbacks.raiseRegistedCallbacks(_onFolderSelectEvent, new[] {textValue});
            }
            if (cbOnSelectCopyTreeNodeTextToClipboard.Checked)
            {
                var clipboardText = "";
                foreach (TreeNode node in tvFindings.Nodes)
                    clipboardText += node.Text + Environment.NewLine;
                Clipboard.SetText(clipboardText);
            }
            
        }

        public string getSelectedNodeFilter1Text()
        {
            if (tvFindings.SelectedNode.Tag != null && tvFindings.SelectedNode.Tag is List<TreeNode>)
            {
                var treeNodes = (List<TreeNode>) tvFindings.SelectedNode.Tag;
                if (treeNodes.Count > 0 && treeNodes[0].Tag is IO2Finding)
                {
                    var sampleO2Finding = (IO2Finding) treeNodes[0].Tag;
                    var currentFilter1 = cbFilter1.Text;
                    var propertyValue = PublicDI.reflection.getProperty(currentFilter1, sampleO2Finding);
                    return (propertyValue != null) ? propertyValue.ToString() : "";
                }
            }
            return null;
        }

        private void onFindingsTreeViewItemDrag(ItemDragEventArgs e)
        {
            tvFindings.SelectedNode = (TreeNode)e.Item;
            if (cbAllowDragOfFindings.Checked)
            {                
                currentlyDraggingNodeFromTreeView = true;
                DoDragDrop(getFindingsListFromTreeNodeChildNodes((TreeNode)e.Item), DragDropEffects.Copy);
                currentlyDraggingNodeFromTreeView = false;
            }
        }

        private List<IO2Finding> getFindingsListFromTreeNodeChildNodes(TreeNode treeNode)
        {
            var o2Findings = new List<IO2Finding>();
            if (treeNode.Tag != null)
                if (treeNode.Tag is IO2Finding)
                    o2Findings.Add((IO2Finding)treeNode.Tag);
                else if (treeNode.Tag is List<TreeNode>)
                    foreach (TreeNode childNode in (List<TreeNode>)treeNode.Tag)
                        if (childNode.Tag != null && childNode.Tag is IO2Finding)
                            o2Findings.Add((IO2Finding)childNode.Tag);
            return o2Findings;
        }

        private void expandAll()
        {
            tvFindings.ExpandAll();
        }


        private void collapseAll()
        {
            tvFindings.CollapseAll();
        }

        private void onTreeViewBeforeExpand(TreeNode treeNodeToExpand)
        {
            treeNodeToExpand.Nodes.Clear();
            if (treeNodeToExpand.Tag != null && treeNodeToExpand.Tag is List<TreeNode>)
                foreach (var TreeNode in ((List<TreeNode>)treeNodeToExpand.Tag))
                {
                    treeNodeToExpand.Nodes.Add(TreeNode);
                    if (!canWeStillAddMoreNodes(0, true))
                        break;
                }
        }

        public List<IO2Finding> getFindingsFromTreeView()
        {
            var o2Findings = new List<IO2Finding>();
            foreach (TreeNode rootNode in tvFindings.Nodes)
            {
                if (rootNode.Tag is IO2Finding)
                    o2Findings.Add((IO2Finding) rootNode.Tag);
                    //foreach(TreeNode childNode in rootNode.Nodes)
                    //{                   
                else if (rootNode.Tag is List<TreeNode>)
                {
                    foreach (TreeNode childNode in ((List<TreeNode>) rootNode.Tag))
                        if (childNode.Tag is IO2Finding)                        
                            o2Findings.Add((IO2Finding) childNode.Tag);                                                    
                }                
            }
            return o2Findings;
        }

        private static void openO2FindingOnNewGuiWindow(IO2Finding o2FindingToOpen)
        {
            O2Thread.mtaThread(() =>
                                   {
                                       var findingEditorControlName = string.Format("Findings Editor for: {0}               ({1})", o2FindingToOpen, Guid.NewGuid());
                                       O2Messages.openControlInGUISync(typeof (ascx_FindingEditor), O2DockState.Float,
                                                                       findingEditorControlName);
                                       O2Messages.getAscx(findingEditorControlName, controlObject =>
                                                                                        {
                                                                                            if(controlObject!=null && controlObject is ascx_FindingEditor)
                                                                                            {
                                                                                                var findingEditor = (ascx_FindingEditor)controlObject;
                                                                                                findingEditor.loadO2Finding(o2FindingToOpen);
                                                                                            }

                                                                                        });
                                       // can't use this because we can't  serialized O2Finding (since it has interfaces on it)
                                       /*
                                       var serializedO2Finding = OzasmtUtils.createSerializedXmlStringFromO2Finding(o2FindingToOpen);
                                       O2Messages.executeOnAscx(findingEditorControlName, "loadSerializedO2Finding",
                                                                new[] {serializedO2Finding});
                                        * */
                                   });
        }

        public static Thread openInFloatWindow(string ozasmtFile)
        {
            return openInFloatWindow(ozasmtFile, "Findings Viewer: "  + Path.GetFileName(ozasmtFile));
        }

        public static Thread openInFloatWindow(string ozasmtFile, string controlName)
        {
            var o2AssessmentLoadEngine = OzasmtUtils.getO2AssessmentLoadEngine(ozasmtFile, o2AssessmentLoadEngines);
            if (o2AssessmentLoadEngine != null)
            {
                var o2Assessment = new O2Assessment(o2AssessmentLoadEngine, ozasmtFile);
                if (o2Assessment.o2Findings.Count > 0)
                    return openInFloatWindow(o2Assessment.o2Findings, controlName);
            }
            return null;
        }

        public static Thread openInFloatWindow(IO2Finding o2Finding)
        {
            return openInFloatWindow(new List<IO2Finding>().add(o2Finding));
        }

        public static Thread openInFloatWindow(List<IO2Finding> o2Findings)
        {
            return openInFloatWindow(o2Findings, "Findings Viewer");
        }

        public static Thread openInFloatWindow(List<IO2Finding> o2Findings, string windowTitle)
        {            
            return O2Thread.mtaThread(() =>
                                          {
                                              O2Messages.openControlInGUISync(typeof (ascx_FindingsViewer), O2DockState.Float, windowTitle);
                                              O2Messages.getAscx(windowTitle, guiControl =>
                                                                                  {
                                                                                      if (guiControl != null && guiControl is ascx_FindingsViewer)
                                                                                      {
                                                                                          var findingsViewer = (ascx_FindingsViewer) guiControl;
                                                                                          findingsViewer.loadO2Findings(o2Findings);
                                                                                      }                                                        
                                                                                  });
                                          });            
        }

        public void setTraceTreeViewVisibleStatus(bool traceTreeViewVisibleStatus)
        {
            this.invokeOnThread(
                () =>
                    {
                        scrollBarHorizontalSize.Visible = scrollBarVerticalSize.Visible = 
                            ascxTraceTreeView.Visible = traceTreeViewVisibleStatus;                        
                        showCurrentO2Findings();
                    });
        }

        /*public static void addO2AssessmentLoadEngine(IO2AssessmentLoad o2AssessmentLoad)
        {
            o2AssessmentLoadEngines.Add(o2AssessmentLoad);            
        }*/
        public static void addO2AssessmentSaveEngine_static(IO2AssessmentSave _o2AssessmentSave)
        {
            o2AssessmentSave = _o2AssessmentSave;
        }
        
        public static bool addO2AssessmentLoadEngine_static(IO2AssessmentLoad o2AssessmentLoad)
        {
            foreach (var loadedEngine in o2AssessmentLoadEngines)
                if (loadedEngine.typeFullName() == o2AssessmentLoad.typeFullName())
                    return false;
            o2AssessmentLoadEngines.Add(o2AssessmentLoad);
            return true;
        }

        public void addO2AssessmentLoadEngine(IO2AssessmentLoad o2AssessmentLoad)
        {
            o2AssessmentLoadEngines.Add(o2AssessmentLoad);
            laNoAssessmentLoadEnginesLoaded.Visible = false;
        }

        public TreeView getResultsTreeView()
        {
            return tvFindings;
        }

        public void refreshView()
        {
            showCurrentO2Findings();
        }


        private void onTraceSelected(IO2Trace o2SelectedTrace)
        {
            Callbacks.raiseRegistedCallbacks(_onTraceSelected, new object[] {o2SelectedTrace});
        }        

        public void setFilter1Value(string value)
        {
            setFilter1Value(value, false);
        }

        public void setFilter1Value(string value, bool refreshView)
        {
            if (value == "")
                value = "(no Filter)";
            this.invokeOnThread(
                () =>
                    {
                        cbFilter1.Text = value;
                        if (refreshView)
                            showCurrentO2Findings();
                    });            
        }

        public void setFilter1TextValue(string value, bool refreshView)
        {
            this.invokeOnThread(
                () =>
                {
                    tbFilter1Text.Text = value;
                    if (refreshView)
                        showCurrentO2Findings();
                });
        }
        public void setFilter2Value(string value)
        {
            if (value == "")
                value = "(no Filter)";
            this.invokeOnThread(() => cbFilter2.Text = value);
        }



        private void filterByFilteredSignatures(List<FilteredSignature> filteredSignatures)
        {
            this.invokeOnThread(
                () =>
                    {
                        var filterToUse = cbFilter1.Text;

                        O2Thread.mtaThread(
                            () =>
                                {
                                    var timer = new O2Timer("filterByFilteredSignatures").start();
                                    List<string> listOfFilteredSignatures =
                                        O2Linq.getListOfSignatures(filteredSignatures);
                                    var newListOfO2Findings = new List<IO2Finding>();
                                    foreach (IO2Finding o2Finding in currentO2Findings)
                                        if (listOfFilteredSignatures.Contains(calculateTreeNodeText(o2Finding,
                                                                                                    filterToUse, "")))
                                            newListOfO2Findings.Add(o2Finding);
                                    currentO2Findings = newListOfO2Findings;
                                    timer.stop();
                                    showCurrentO2Findings();

                                });
                    });
        

        /*
                        this.invokeOnThread(
                            () =>
                                {
                    
                                    tvFindings.Visible = false;
                                    var nodesToRemove = new List<TreeNode>();
                                    var nodesToAnalyze = new List<TreeNode>();
                                    foreach (TreeNode treeNode in tvFindings.Nodes)
                                        nodesToAnalyze.Add(treeNode);


                                    nodesToRemove = removeFromTreeNodeCollectionOnFilteredSignatures(nodesToAnalyze,
                                                                                                     listOfFilteredSignatures, currentFilteredTextMappings);


                                    // and update indexes
                            /*        foreach (TreeNode currentTreeNode in tvFindings.Nodes)
                                        if (currentTreeNode.Tag != null && currentTreeNode.Tag is List<TreeNode>)
                                        {
                                            var subTreeNodesCount = ((List<TreeNode>) currentTreeNode.Tag).Count;
                                            if (subTreeNodesCount == 0)
                                                nodesToRemove.Add(currentTreeNode);
                                            else
                                            {
                                                currentTreeNode.Text += "   ... after filter (" + subTreeNodesCount +")";
                                                currentTreeNode.Collapse();
                                            }
                                        }
* /
                                    // remove nodes from current tree
                                    foreach (TreeNode treeNodeToRemove in nodesToRemove)
                                        tvFindings.Nodes.Remove(treeNodeToRemove);

                                    tvFindings.Visible = true;
                                    tvFindings.Refresh();
                                    //    refreshView();
                                });
                    });*/
        }

        private static List<TreeNode> removeFromTreeNodeCollectionOnFilteredSignatures(List<TreeNode> treeNodes, List<string> listOfFilteredSignatures, Dictionary<IO2Finding, string> filteredTextMappings)
        {
            var nodesToRemove = new List<TreeNode>();
            try
            {

                foreach (TreeNode treeNode in treeNodes)
                    if (treeNode.Tag != null)
                    {
                        if (treeNode.Tag is List<TreeNode>)         // for now we only support this search on the 1st filter
                        {
                            // removeFromTreeNodeCollectionOnFilteredSignatures((List<TreeNode>) treeNode.Tag,
                            //                                                  listOfFilteredSignatures, filteredTextMappings);
                            var childTreeNodes = (List<TreeNode>)treeNode.Tag;
                            if (childTreeNodes[0].Tag != null && childTreeNodes[0].Tag is IO2Finding)
                            {
                                var o2Finding = (IO2Finding)childTreeNodes[0].Tag;
                                //}
                                // else
                                //     if (treeNode.Tag is IO2Finding)

                                //var o2Finding = (IO2Finding) treeNode.Tag;

                                var textToSearch = filteredTextMappings[o2Finding];
                                // PublicDI.reflection.getProperty(treeNode.Name, (IO2Finding)treeNode.Tag);
                                if (false == listOfFilteredSignatures.Remove(textToSearch))
                                    //if (false == listOfFilteredSignatures.Contains((string) textToSearch))
                                    nodesToRemove.Add(treeNode);
                                else
                                {
                                }
                            }
                        }

                        /*
                            if (removeFromTreeNodeOnFilteredSignatures(treeNode, listOfFilteredSignatures, nodesToRemove))
                                ;                    */
                        /*
                    {
                        var o2finding = (IO2Finding) treeNode.Tag;
                        if (false == listOfFilteredSignatures.Contains(o2finding.vulnName))
                            nodesToRemove.Add(treeNode);
                    }*/
                    }

                foreach (TreeNode treeNodeToRemove in nodesToRemove)
                    treeNodes.Remove(treeNodeToRemove);
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "removeFromTreeNodeCollectionOnFilteredSignatures");
            }
            return nodesToRemove;
        }

        

        private void filterByFilteredSignature(FilteredSignature filteredSignature)
        {
            setFilter1TextValue(filteredSignature.sSignature, true);
        }

        public void expandAllNodes()
        {
            this.invokeOnThread(() => tvFindings.ExpandAll());
        }
    }
}
