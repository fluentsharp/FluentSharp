// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_FunctionsViewer
    {
        #region Events
        public event Callbacks.dMethod_Object _onDrop;
        public event Callbacks.dMethod_Object _onAfterSelect;
        public event Callbacks.dMethod_Object _onDoubleClick;
        public event Callbacks.dMethod_Object _onItemDrag;
        public event Action<TreeNode> _onMouseMove;        
        
        public Callbacks.dMethod_String eNodeEvent_AfterSelect;
        public Callbacks.dMethod_ListString eNodeEvent_CheckClickEvent;
        #endregion

        public List<FilteredSignature> selectedFilteredSignatures = new List<FilteredSignature>();
        public FilteredSignature selectedFilteredSignature;
        public Char cSplitChar = '.'; // char that will be used to split the string into the tree view

        private bool bRecursivelyCheckingAllSubNodes;
        private bool raiseOnAfterTreeViewCheck = true;
        public bool bRemoveEmptyRootNodes = true;
        


        public int iMaxItemsToShow = 25000;
        public IEnumerable<String> lsSignatures = new List<string>();
        public String sViewName = "";

        public TreeNode tnSelectedNode;
        public ViewMode vmViewMode = ViewMode.byFunctionSignature; // default to signature view
        public ViewDisplayControl viewDisplayControl = ViewDisplayControl.TreeView;

        public int NamespaceDepthValue { get; set; }

        public int numberOfUniqueStrings;

        public bool runOnLoad = true;

        private void onInitialize()
        {
            this.invokeOnThread(
                () =>
                    {
                        
                    });
        }

        private void onLoad()
        {            
            if (false == DesignMode && runOnLoad)
            {
                namespaceDepth.SelectedIndex = 2;
                typeOfControlToShowData.Text = viewDisplayControl.ToString();
                tvTreeView.CheckBoxes = false;
                tbMaxItemsToShow.Text = iMaxItemsToShow.ToString();
                //     tvTreeView_Resize(null, null);                
                runOnLoad = false;
                showView();
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _AdvancedModeViews
        {
            set
            {
                lbFilter.Visible = value;
                tbFilter.Visible = value;
                //lBoxListBox.Visible = value;
                toolStripSeparator3.Visible = value;
                checkAll.Visible = value;
                uncheckAll.Visible = value;
                cbAdvancedViewMode.Checked = value;
            }
            get
            {
                return cbAdvancedViewMode.Checked;
            }

        }

        public void setNamespaceDepth(Int32 iDepth)
        {
            this.invokeOnThread(
                () =>
                    {
                        NamespaceDepthValue = iDepth;
                        namespaceDepth.SelectedIndex = (iDepth < namespaceDepth.Items.Count - 1)
                                                           ? iDepth + 1
                                                           : namespaceDepth.Items.Count - 1;
                    });
        
    }

       

        private void onDoubleClickTreeView(TreeNode selectedNode)
        {
            if (selectedNode != null)
                Callbacks.raiseRegistedCallbacks(_onDoubleClick, new[] { selectedNode.Tag });
        }

        public void onAfterSelectTreeView(TreeNode selectedNode)
        {
            try
            {
                if (selectedNode != null)
                {
                    // populate selectedFilteredSignature object
                    if (selectedNode.Tag != null && selectedNode.Tag.GetType().Name == "FilteredSignature")
                        selectedFilteredSignature = (FilteredSignature) selectedNode.Tag;
                    else
                        selectedFilteredSignature = null;

                    // get fucntion's signature
                    var signature = (selectedFilteredSignature!=null)
                                        ?
                                            selectedFilteredSignature.sOriginalSignature
                                        :                                            
                                            selectedNode.Name;

                    // raise callbacks
                    Callbacks.raiseRegistedCallbacks(eNodeEvent_AfterSelect, new object[] { signature });

                    Callbacks.raiseRegistedCallbacks(_onAfterSelect, new [] { selectedNode.Tag });
                    
                    

                    // need to manual handle this backcolor stuff because we lose it when multiple function's viewers are used at the same time
                    /*            if (selectedNode != null)
                                {
                                    selectedNode.BackColor = Color.White;
                                    selectedNode.ForeColor = Color.Black;
                                }
                                //tnSelectedNode = tvTreeView.SelectedNode;
                                tvTreeView.SelectedNode.BackColor = Color.DarkBlue;
                                tnSelectedNode.ForeColor = Color.White;*/

                    

                    /*if (onTreNodeAfterSelect != null)
                        foreach (Delegate dDelegate in onTreNodeAfterSelect.GetInvocationList())
                        {
                            if (selectedNode.Tag != null &&
                                selectedNode.Tag.GetType().Name == "FilteredSignature")
                                dDelegate.DynamicInvoke(new[]
                                                            {
                                                                ((FilteredSignature) selectedNode.Tag).sOriginalSignature
                                                            });
                            else
                                dDelegate.DynamicInvoke(new[] { selectedNode.Text });

                        }*/
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("In tvTreeView_AfterSelect: {0}", ex.Message);
            }
        }

        private void onAfterTreeViewCheck(TreeNode checkedTreeNode, ref bool _bRecursivelyCheckingAllSubNodes, TreeView targetTreeView, Callbacks.dMethod_ListString onTreeViewCheckClick)
        {
            if (false == _bRecursivelyCheckingAllSubNodes)
            {                
                bRecursivelyCheckingAllSubNodes = true;                
                O2Forms.setCheckBoxStatusForAllTreeNodeChilds_recursive(checkedTreeNode.Nodes, checkedTreeNode.Checked);
                O2Forms.setCheckBoxStatusForAllParentNodes_recursive(checkedTreeNode, false);

                var ltnCurrentlySelectedClasses = new List<TreeNode>();
                O2Forms.calculateListOfCurrentlySelectedClasses_Recursive(targetTreeView.Nodes,ltnCurrentlySelectedClasses);
                // clear this variable                                                          
                selectedFilteredSignatures = new List<FilteredSignature>();
                if (ltnCurrentlySelectedClasses.Count > 0)
                {
                    foreach (TreeNode tnTreeNode in ltnCurrentlySelectedClasses)
                    {
                        if (tnTreeNode.Tag != null)
                            switch (tnTreeNode.Tag.GetType().Name)
                            {
                                case "FilteredSignature":
                                    var fsFilteredSig = (FilteredSignature) tnTreeNode.Tag;
                                    if (false == selectedFilteredSignatures.Contains(fsFilteredSig))
                                        selectedFilteredSignatures.Add(fsFilteredSig);

                                    break;
                                case "List`1":
                                    //Type tTyp = tnTreeNode.Tag.GetType();
                                    var lfsFilteredSignatures = (List<FilteredSignature>) tnTreeNode.Tag;
                                    selectedFilteredSignatures.AddRange(lfsFilteredSignatures);
                                    break;
                                default:
                                    break;
                            }
                    }
                }
                
                // fire sevent with selected signatures
                Callbacks.raiseRegistedCallbacks(_onAfterSelect, new object[] {selectedFilteredSignatures});            
                // create  list of signatures and fire event
                var lsCurrentlySelectedClasses = new List<String>();
                foreach (var fsFilteredSignature in selectedFilteredSignatures)
                    if (false == lsCurrentlySelectedClasses.Contains(fsFilteredSignature.sOriginalSignature))
                        lsCurrentlySelectedClasses.Add(fsFilteredSignature.sOriginalSignature);
                
                if (onTreeViewCheckClick != null)
                    onTreeViewCheckClick.Invoke(lsCurrentlySelectedClasses);


                bRecursivelyCheckingAllSubNodes = false;
            }
        }

        public void setCheckBoxesState(bool newState)
        {
            this.invokeOnThread(() => tvTreeView.CheckBoxes = newState);
        }
        
        public void setViewName(String _viewName)
        {
            sViewName = _viewName;
            lbViewName.Text = sViewName;
        }

        public void setSplitChar(Char _splitChar)
        {
            cSplitChar = _splitChar;
            vmViewMode = ViewMode.byProvidedSplitChar;
            // if we a splitchar is provided is because the desired ViewMode is ViewMode.byProvidedSplitChar
        }

        public void setViewMode(ViewMode _viewMode)
        {
            vmViewMode = _viewMode;
        }

        public void setMaxItemsToShow(Int32 _maxItemsToShow)
        {
            iMaxItemsToShow = _maxItemsToShow;
        }

        public Thread showSignatures(IEnumerable<String> _signatures)
        {
            if (lsSignatures != null)
            {
                lsSignatures = O2Linq.getUniqueSortedListOfStrings(_signatures, ref numberOfUniqueStrings);
                return showView();
            }
            return null;
        }

        public Thread loadView()
        {
            //lBoxListBox.Visible = rbView_ListBox.Checked;
            //tvTreeView.Visible = !lBoxListBox.Visible;
            return showView();
        }

        public void applyFilter(String sRegEx)
        {
        }

        public Thread showView()
        {                      
            if (runOnLoad)   // don't run any searches until the control is loaded
                return null;
            return (Thread)this.invokeOnThread(
                () =>
                //if (this.okThread(delegate { showView(); }))
                    {                                                
                        lbViewName.Text = "Showing " + numberOfUniqueStrings + " Items";
                        //tvTreeView.Visible = false;
                        //lBoxListBox.Visible = false;
                        switch (viewDisplayControl)
                        {
                            case ViewDisplayControl.ListBox:
                                lBoxListBox.Items.Clear();
                                tvTreeView.Visible = false;
                                lBoxListBox.Visible = true;
                                lBoxListBox.Items.AddRange(limitItemsToShow(lsSignatures).ToArray());
                                //lBoxListBox.BringToFront();                    
                                break;
                            case ViewDisplayControl.TreeView:
                                tvTreeView.Visible = true;
                                lBoxListBox.Visible = false;
                                //showListOnTreeView(limitItemsToShow(lsSignatures));
                                return showListOnTreeView(lsSignatures);                                
                        }
                        return null;                        
                    });                        
        }

        public IEnumerable<String> limitItemsToShow(IEnumerable<String> lsTargetList)
        {
            if (iMaxItemsToShow > numberOfUniqueStrings)
                return lsTargetList;


            PublicDI.log.error(
                "The {0} list has more items that the current MaxToView. only showing the first {1} out of {2}",
                sViewName, iMaxItemsToShow, numberOfUniqueStrings);
            tbNotAllDataShown.Visible = true;
            return lsTargetList.Take(iMaxItemsToShow);
            /*var lsLimitedList = new List<string>();
            for (int i = 0; i < iMaxItemsToShow; i++)
                lsLimitedList.Add(lsTargetList[i]);
            return lsLimitedList;*/
        }

        private void populateTreeViewUsingViewMode_byProvidedSplitChar(IEnumerable<string> lsList,
                                                                       TreeView tvTempTreeView,
                                                                       int iDepth)
        {
            var dItemsBrokenByDepth = new Dictionary<String, List<String>>();

            foreach (String sItem in lsList)
            {
                if (NamespaceDepthValue == -1)
                {
                    if (RegEx.findStringInString(sItem, tbFilter.Text))
                        //if (dItemsParsed.ContainsKey(sItem))
                        tvTempTreeView.Nodes.Add(O2Forms.newTreeNode(sItem, sItem, 3, sItem));
                }
                else
                {
                    String sBeforeString = "";
                    String sAfterString = "";
                    // split the string on the provided split char
                    String[] sSplitedString = sItem.Split(new[] { cSplitChar });
                    /*// ensure that the splidebar maximum value is at least one more than the maximum number of splitted items
                    if (sSplitedString.Length > tBarNameSpaceDepth.Maximum)
                        tBarNameSpaceDepth.Maximum = sSplitedString.Length + 1;*/

                    for (int i = 0; i < sSplitedString.Length; i++)
                        if (i < iDepth)
                        {
                            if (i < sSplitedString.Length - 1)
                                sBeforeString += sSplitedString[i] + cSplitChar;
                            else
                                sBeforeString += sSplitedString[i];
                        }
                        else
                            sAfterString += cSplitChar + sSplitedString[i];


                    //if (iSplitPoint > -1)                    
                    //    String sBeforeString = sItem.Substring(0, iSplitPoint);
                    //    String sAfterString = sItem.Substring(iSplitPoint);
                    if (false == dItemsBrokenByDepth.ContainsKey(sBeforeString))
                        dItemsBrokenByDepth.Add(sBeforeString, new List<string>());
                    dItemsBrokenByDepth[sBeforeString].Add(sAfterString);
                }
            }

            foreach (String sBeforeString in dItemsBrokenByDepth.Keys)
            {
                const int iLettersToShowInTopLevelNodeText = 30;
                String sNodeText = (sBeforeString.Length < iLettersToShowInTopLevelNodeText)
                                       ? sBeforeString
                                       : "..." +
                                         sBeforeString.Substring(sBeforeString.Length - iLettersToShowInTopLevelNodeText);
                var lsTopLevelFiles = new List<string>();
                foreach (string sFile in dItemsBrokenByDepth[sBeforeString])
                    lsTopLevelFiles.Add(sBeforeString + sFile);
                TreeNode tnNewTreeNode = O2Forms.newTreeNode(sNodeText, sBeforeString, 0, lsTopLevelFiles);
                tvTempTreeView.Nodes.Add(tnNewTreeNode);
                //        addCategoryToTreeNodeCollection(sBeforeString, ref tncNodes, null);
                foreach (String sAfterString in dItemsBrokenByDepth[sBeforeString])
                {
                    TreeNodeCollection tncNodes = tnNewTreeNode.Nodes;

                    String[] sSplittedAfterString = sAfterString.Split(new[] { cSplitChar },
                                                                       StringSplitOptions.RemoveEmptyEntries);
                    if (sSplittedAfterString.Length == 1)
                    {
                        tncNodes.Add(O2Forms.newTreeNode(sSplittedAfterString[0], sSplittedAfterString[0], 3,
                                                         new List<String>()));
                        ((List<String>)tncNodes[sSplittedAfterString[0]].Tag).Add(sBeforeString +
                                                                                   sSplittedAfterString[0]);
                    }
                    else
                    {
                        String sCurrentPath = "";
                        for (int iItem = 0; iItem < sSplittedAfterString.Length; iItem++)
                        {
                            if (iItem < sSplittedAfterString.Length - 1)
                            {
                                sCurrentPath += cSplitChar + sSplittedAfterString[iItem];
                                TreeNodeCollection tncCurrentNode = tncNodes;
                                addCategoryToTreeNodeCollection(sSplittedAfterString[iItem], sCurrentPath, ref tncNodes,
                                                                new List<String>());
                                ((List<String>)tncCurrentNode[sCurrentPath].Tag).Add(sBeforeString + sAfterString);
                            }
                            else
                            {
                                tncNodes.Add(O2Forms.newTreeNode(sSplittedAfterString[iItem],
                                                                 sSplittedAfterString[iItem], 3, new List<String>()));
                                ((List<String>)tncNodes[sSplittedAfterString[iItem]].Tag).Add(sBeforeString +
                                                                                               sAfterString);
                            }
                        }
                    }
                }
                tnNewTreeNode.Expand(); // auto expand the top level nodes
                //tnNewTreeNode.Nodes.Add(sAfterString);
            }
        }

        private static Thread populateTreeViewUsingViewMode_byFunctionSignature(IEnumerable<string> lsList,
                                                                       TreeView targetTreeView,
                                                                       int namespaceDepthValue, string textFilter, int iMaxItemsToShow)
        {
            return O2Thread.mtaThread(
                () =>
                {
                    targetTreeView.invokeOnThread(() => targetTreeView.Visible = false);
                    try
                    {
                        var dItemsParsed = new Dictionary<String, FilteredSignature>();
                        foreach (String sItem in lsList)
                        {
                            //if (false == dItemsParsed.ContainsKey(sItem))
                            {
                                if (textFilter == "" || RegEx.findStringInString(sItem, textFilter))
                                    dItemsParsed.Add(sItem, new FilteredSignature(sItem));
                            }
                            //else
                            //    PublicDI.log.error("Something's wrong in showListOnTreeView, lsList had repeated key:{0}", sItem);
                        }
                        var dItemsBrokenByClassDepth = new Dictionary<String, List<FilteredSignature>>();

                        foreach (String sItem in lsList)
                        {

                            if (namespaceDepthValue == -1)
                            {
                                if (textFilter == "" || RegEx.findStringInString(sItem, textFilter))
                                    if (dItemsParsed.ContainsKey(sItem))
                                        // add node ASync and on the correct thread
                                        O2Forms.addNodeToTreeNodeCollection(targetTreeView, targetTreeView.Nodes,
                                                                  O2Forms.newTreeNode(
                                                                      dItemsParsed[sItem].sSignature, sItem,
                                                                      3, dItemsParsed[sItem]), iMaxItemsToShow);
                            }
                            else
                            {
                                if (dItemsParsed.ContainsKey(sItem))
                                {
                                    String sClassNameToShow =
                                        dItemsParsed[sItem].getClassName_Rev(namespaceDepthValue);
                                    if (sClassNameToShow == "")
                                        sClassNameToShow = dItemsParsed[sItem].sFunctionClass;
                                    /*  var sClassNameToConsolidate = (sClassNameToShow == "")
                                                                 ? dItemsParsed[sItem].sFunctionClass
                                                                 : dItemsParsed[sItem].sFunctionClass.Replace(
                                                                       sClassNameToShow, "");*/
                                    if (false == dItemsBrokenByClassDepth.ContainsKey(sClassNameToShow))
                                        dItemsBrokenByClassDepth.Add(sClassNameToShow, new List<FilteredSignature>());
                                    //String sSignatureToShow = sClassNameToConsolidate + "__" + dItemsParsed[sItem].sFunctionNameAndParams;
                                    dItemsBrokenByClassDepth[sClassNameToShow].Add(dItemsParsed[sItem]);
                                }
                            }

                            // add calculated results 
                            //     TreeNodeCollection tncToAddFunction_ = targetTreeView.Nodes;
                        }
                            foreach (String sClass in dItemsBrokenByClassDepth.Keys)
                            {
                                var filteredSignatures = dItemsBrokenByClassDepth[sClass];
                                TreeNode tnNewTreeNode = O2Forms.newTreeNode(sClass, sClass, 0,
                                                                             filteredSignatures);
                                if (filteredSignatures.Count > 0)
                                    tnNewTreeNode.Nodes.Add("DummyNode");
                                // add node ASync and on the correct thread
                                O2Forms.addNodeToTreeNodeCollection(targetTreeView, targetTreeView.Nodes, tnNewTreeNode, iMaxItemsToShow);
                                //tncToAddFunction.Add(tnNewTreeNode);


                            }


                            // remove empty nodes

                            /*       if (false && bRemoveEmptyRootNodes && NamespaceDepthValue > -1)
                    {
                        var tnTreeNodesToRemove = new List<TreeNode>();
                        foreach (TreeNode tnTreeNode in tvTempTreeView.Nodes)
                            if (tnTreeNode.Nodes.Count == 0)
                                tnTreeNodesToRemove.Add(tnTreeNode);
                        foreach (TreeNode tnTreeNode in tnTreeNodesToRemove)
                            tvTempTreeView.Nodes.Remove(tnTreeNode);
                    }*/
                        

                        var numberOfUniqueStrings = lsList.Count();
                        if (numberOfUniqueStrings > iMaxItemsToShow)
                        {
                            var message = "This view has more items that the current MaxToView. only showing the first {0} out of {1}".format(iMaxItemsToShow, numberOfUniqueStrings);
                            PublicDI.log.error(message);
                            targetTreeView.add_Node(message);                            
                        }
                    }
                    catch (Exception ex)
                    {
                        PublicDI.log.error("in populateTreeViewUsingViewMode_byFunctionSignature: {0}", ex.Message);
                    }

                    targetTreeView.invokeOnThread(
                        () =>
                        {
                            targetTreeView.Visible = true;
                            // the code below tries to solve a weird GUI problem that happens when there is only one child Node (which is invible until the user clicks on it))                                
                            if (targetTreeView.Nodes.Count == 1)
                            {
                                targetTreeView.Nodes[0].Expand();
                                //                                    targetTreeView.SelectedNode = targetTreeView.Nodes[0];
                                //var dummyNode = targetTreeView.Nodes.Add("dUMMYN node");
                                //targetTreeView.Nodes.Remove(dummyNode);
                            }
                        });
                });
        }

        private void onTreeViewBeforeExpand(TreeNode currentTreeNode)
        {            
            if (currentTreeNode.Tag != null && currentTreeNode.Tag is List<FilteredSignature>)
            {
                currentTreeNode.Nodes.Clear();
                var sClass = currentTreeNode.Name;
                var filteredSignatures = (List<FilteredSignature>) currentTreeNode.Tag;
                //var textFilter = tbFilter.Text;    

                // first build the dictionary with this level's node class data
                var dictionaryWithNodeData = new Dictionary<string, List<FilteredSignature>>();
                // and this node's functions
                var listWithFunctionsToAdd = new List<FilteredSignature>();

                foreach (FilteredSignature filteredSignature in filteredSignatures)
                {
                    //TreeNodeCollection tncNodes = tnNewTreeNode.Nodes;                                    
                    if (sClass == "")
                        sClass = "<no class def>";
                   // if (sClass != "")
                   // {
                        String sRestOfClassToShow = filteredSignature.sFunctionClass.Replace(sClass, "");
                        if (sRestOfClassToShow != "")
                        {
                            if (sRestOfClassToShow[0] == cSplitChar)
                                sRestOfClassToShow = sRestOfClassToShow.Substring(1);
                            var firstIndexOfSplitChar = sRestOfClassToShow.IndexOf(cSplitChar);
                            var nodeText = (firstIndexOfSplitChar > -1)
                                               ? sRestOfClassToShow.Substring(0, firstIndexOfSplitChar)
                                               : sRestOfClassToShow;
                            if (false == dictionaryWithNodeData.ContainsKey(nodeText))
                                dictionaryWithNodeData.Add(nodeText, new List<FilteredSignature>());
                            dictionaryWithNodeData[nodeText].Add(filteredSignature);
                        }

                            /*if (sRestOfClassToShow != "")
                        {
                            if (sRestOfClassToShow[0] == cSplitChar)
                                sRestOfClassToShow = sRestOfClassToShow.Substring(1);
                            foreach (String sClassItem in sRestOfClassToShow.Split(cSplitChar))
                                // check, I think this will merge subsclass with the same name
                            {

                                TreeNodeCollection tncNodes = currentTreeNode.Nodes;
                                addCategoryToTreeNodeCollection(sClassItem, sClassItem,
                                                                ref tncNodes,
                                                                fsSignatureToShow);
                            }
                        }*/

                        else if (false == filteredSignature.bIsClass)
                        {
                            listWithFunctionsToAdd.Add(filteredSignature);
                            // add node ASync and on the correct thread

                            /*O2Forms.addNodeToTreeNodeCollection(targetTreeView, targetTreeView.Nodes,
                                                                newTreeNode);*/
                        }
                        
                   // }
                }
                // now that we have the list calculated, add them as subnodes:

                foreach(var key in dictionaryWithNodeData.Keys)
                {
                    var nodeText = sClass + cSplitChar + key;
                    var thisNodeSignatures = dictionaryWithNodeData[key];
                    var newNode = O2Forms.newTreeNode(key, nodeText, 1, thisNodeSignatures);                    
                    currentTreeNode.Nodes.Add(newNode);
                    if (thisNodeSignatures.Count > 0 || listWithFunctionsToAdd.Count > 0)
                        newNode.Nodes.Add("Dummy node");

                }

                foreach (var function in listWithFunctionsToAdd)
                //if (textFilter == "" || RegEx.findStringInString(fsSignatureToShow.sFunctionNameAndParams,
                //                                                       textFilter))
                {
                    var nodeText = function.sFunctionNameAndParamsAndReturnClass; //sClass + cSplitChar + key;
                    var newTreeNode =
                        O2Forms.newTreeNode(nodeText,
                                            nodeText, 3,
                                            function);
                    currentTreeNode.Nodes.Add(newTreeNode);
                }
            }
        }




        private Thread showListOnTreeView(IEnumerable<String> lsList)
        {            
            //  lsSignatures.Sort();
            //lsList.OrderBy();
            
     /*       var tvTempTreeView = new TreeView { ImageList = tvTreeView.ImageList };
            //  tvTempTreeView.Resize += new EventHandler(tvTreeView_Resize);
            tvTempTreeView.AfterCheck += tvTreeView_AfterCheck;
            tvTempTreeView.AfterSelect += tvTreeView_AfterSelect;
            tvTempTreeView.Anchor = tvTreeView.Anchor;
            tvTempTreeView.Top = tvTreeView.Top;
            tvTempTreeView.Left = tvTreeView.Left;
            tvTempTreeView.Width = tvTreeView.Width;
            tvTempTreeView.Height = tvTreeView.Height;
            tvTempTreeView.CheckBoxes = tvTreeView.CheckBoxes;
            tvTempTreeView.Scrollable = tvTreeView.Scrollable;*/
            
            tvTreeView.Nodes.Clear();            
            tvTreeView.Sorted= true;
            
            tbNotAllDataShown.Visible = false;

            if (lsList.Count() == 0)
                return null;


            var textFilter = tbFilter.Text.Trim();
            var namespaceDepthVaLue = NamespaceDepthValue;
            switch (vmViewMode)
            {
                case ViewMode.byFunctionSignature:
                    return populateTreeViewUsingViewMode_byFunctionSignature(lsList, tvTreeView, namespaceDepthVaLue, textFilter, iMaxItemsToShow);

                case ViewMode.byProvidedSplitChar:
                    populateTreeViewUsingViewMode_byProvidedSplitChar(lsList, tvTreeView, NamespaceDepthValue);
                    break;
            }
            return null;
            //tvTempTreeView.Sort();
            //if (tvTreeView.CheckBoxes)
            //{
                //  forms.setCheckBoxStatusForAllTreeNodeChilds_recursive(tvTempTreeView.Nodes, true);
            //}            
//            tvTempTreeView.Visible = true;
        //    Controls.Remove(tvTreeView);
        //    Controls.Add(tvTempTreeView);
        //    tvTreeView = tvTempTreeView;
        //    tvTempTreeView.SendToBack();
        }

        private static void addCategoryToTreeNodeCollection(String sCategoryText, String sCategoryName,
                                                            ref TreeNodeCollection tncTargetTreeNodeCollection,
                                                            Object oTreeNodeTag)
        {
            if (sCategoryName == "")
                return;
            if (false == tncTargetTreeNodeCollection.ContainsKey(sCategoryName))
            {
                tncTargetTreeNodeCollection.Add(O2Forms.newTreeNode(sCategoryText, sCategoryName, 1, oTreeNodeTag));
            }
            tncTargetTreeNodeCollection = tncTargetTreeNodeCollection[sCategoryName].Nodes;
        }


        private void setTreeViewCheckBoxValue(TreeView targetTreeView, bool value)
        {
            raiseOnAfterTreeViewCheck = false;
            foreach (var treeNode in O2Forms.getListWithAllNodesFromTreeView(targetTreeView.Nodes))
                treeNode.Checked = value;
            raiseOnAfterTreeViewCheck = true;
            targetTreeView.Nodes[0].Checked = value;  // we need to do this and it will triger the onAfterTreeViewCheck event            
        }

        public void showHideConfig()
        {
            configGroupBox.Visible = !configGroupBox.Visible;
            showConfig.Text = (configGroupBox.Visible) ? "show config" : "hide config";
                
        }

        private void handleDrop(DragEventArgs e)
        {
            var droppedObject = Dnd.tryToGetObjectFromDroppedObject(e);
            if (droppedObject is List<string>)
                showSignatures((List<string>)droppedObject);
            else
            {
                var isFileOrDir = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
                if (isFileOrDir!="")                    
                        Callbacks.raiseRegistedCallbacks(_onDrop, new []{isFileOrDir});
                else
                    Callbacks.raiseRegistedCallbacks(_onDrop, new []{droppedObject});
            }                                
        }

        private void onTreeViewItemDrag(TreeView targetTreeView, ItemDragEventArgs e)
        {
            object objectToDrag = null;
            targetTreeView.SelectedNode = (TreeNode) e.Item;
            if (targetTreeView.CheckBoxes)
                objectToDrag = selectedFilteredSignatures;
            else if (e.Item is TreeNode)
                objectToDrag = ((TreeNode) e.Item).Tag;
            if (_onItemDrag != null)
                Callbacks.raiseRegistedCallbacks(_onItemDrag, new[] {objectToDrag});
            else
                DoDragDrop(objectToDrag, DragDropEffects.Copy);
        }

        public void doTreeViewDragDrop(object objectToDrag)
        {
            tvTreeView.invokeOnThread(
                () => tvTreeView.DoDragDrop(objectToDrag, DragDropEffects.Copy));

        }

        public void expandNodes()
        {
            tvTreeView.invokeOnThread(
                () =>
                    {
                        foreach (TreeNode node in tvTreeView.Nodes)
                            node.Expand();
                    });
        }

        public void clearLoadedSignatures()
        {
            tvTreeView.invokeOnThread(
                () =>
                    {
                        tvTreeView.Nodes.Clear();
                        lsSignatures = new List<string>();
                        return default(object);                      // make this a Sync operation
                    });
            
            
        }        
    }
}
