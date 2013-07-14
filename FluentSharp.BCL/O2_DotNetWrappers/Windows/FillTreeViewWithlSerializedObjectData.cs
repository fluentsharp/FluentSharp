// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{
    public class FillTreeViewWithSerializedObjectData
    {
        #region addMode enum

        public enum addMode
        {
            ObjectValue,
            ObjectProperties
        }

        #endregion

        public bool bAcceptBeforeExpandEvents = true;
        // use to prevent multiple refreshs caused by the BeforeExpand event

        private bool bSimulateMode; // use this to calculate how many objects will be added
        public bool bUpdatingNode; // used to prevent multiple updates of the same node
        private int iItems; //local var to count the number of items loaded

        private Object oRootObject; // will contain the object loaded
        private ProgressBar pbProgressBar; // assigned progress bar (this is optional)
        private TreeView tvTargetTreeView; // target treeviw
        public UInt32 uDefaultMaxArrayRecordsToFetch = 20; // don't fetch all array records by default

        public UInt32 uRecursiveDepth = 3;
        // with 3 this will auto load 2 level of Nodes so that the user gets a feel for what is there


        /// <summary>
        /// define working vars
        /// </summary>
        /// <param name="_rootObject"></param>
        /// <param name="_targetTreeView"></param>
        public void init(Object _rootObject, TreeView _targetTreeView)
        {
            init(_rootObject, _targetTreeView, null);
        }

        public void init(Object _rootObject, TreeView _targetTreeView, ProgressBar _progressBar)
        {
            oRootObject = _rootObject;
            tvTargetTreeView = _targetTreeView;
            pbProgressBar = _progressBar;
        }

        /// <summary>
        /// reload treeview
        /// </summary>
        public void refresh()
        {
            PublicDI.log.debug("Refreshing TreeView");
            loadObjectInTreeView();
        }

        /// <summary>
        /// Load root object in TreeView
        /// </summary>
        public void loadObjectInTreeView()
        {
            // make sure the objects are defined
            if (null != oRootObject && null != tvTargetTreeView)
            {
                Application.DoEvents();
                // clears treeview and sets it to sort the nodes
                tvTargetTreeView.Nodes.Clear();
                tvTargetTreeView.Sort();
                if (pbProgressBar != null)
                {
                    // calculates how many items will be added
                    calculateNumberOfItemsForObjectAndConfigureProgressBar();
                }
                // starts recursive loading of nodes in addMode.ObjectValue (which means that the first node will be the name of teh current object
                loadObjectInTreeNodeCollection(oRootObject, "", "", "", tvTargetTreeView.Nodes, uRecursiveDepth,
                                               addMode.ObjectValue);
                // expand 1 level nodes (so that the user see what is next)
                // bAcceptBeforeExpandEvents = false;
                foreach (TreeNode tnNode in tvTargetTreeView.Nodes)
                    tnNode.Expand();
                //            fixXsdNodeNameIssue(this.tvTargetTreeView.Nodes,"");

                //   bAcceptBeforeExpandEvents = true;                
            }
            else
                PublicDI.log.error("The Object to loaded and the target TreeView have not been defined");
        }

        // hack to fix the xsd creation long names (where each name contains all element names up to it (which makes it very hard to read)
/*        private void fixXsdNodeNameIssue(TreeNodeCollection tncTargetNodeCollection, String sParentText)
        {
            if (tncTargetNodeCollection != null)
                foreach (TreeNode tnNode in tncTargetNodeCollection)
                {
                    // PublicDI.log.info(tnNode.Text);
                    //if (tnNode.Parent != null && tnNode.Parent.Parent != null)
                    if (sParentText != "")
                    {
                        // String sParentText = tnNode.Parent.Text.Substring(0,tnNode.Parent.Parent.Text).Trim();
                        tnNode.Text = tnNode.Text.Replace(sParentText, "");
                        //if (tnNode.Parent.Parent != null)
                        //    tnNode.Text = tnNode.Text.Replace(tnNode.Parent.Parent.Text, "...parentParent...");
                    }
                    if (tnNode.Parent != null && (tnNode.Parent.Parent != null))
                    {
                        int iIndexOfBracket = tnNode.Parent.Parent.Text.IndexOf('(');
                        if (iIndexOfBracket > -1)
                            sParentText = tnNode.Parent.Parent.Text.Substring(0, iIndexOfBracket).Trim();
                        else
                            sParentText = tnNode.Parent.Text;
                        //    .Substring(0, tnNode.Parent.Parent.Text).Trim();
                    }
                    / *    if (tnNode.Parent != null)
                    {
                        int iIndexOfBracket = tnNode.Parent.Text.IndexOf('(');
                        if (iIndexOfBracket > -1)
                            sParentText = tnNode.Parent.Text.Substring(0,iIndexOfBracket).Trim();
                        else
                            sParentText = tnNode.Parent.Text;
                    }* /
                    //  tnNode.ExpandAll();
                    fixXsdNodeNameIssue(tnNode.Nodes, sParentText);
                }
        }
*/
        // recursive method to load objects in nodes
        public void loadObjectInTreeNodeCollection(Object oObjectToLoad, String sNodeName, String sParentNodeName,
                                                   String sParentParentNodeName,
                                                   TreeNodeCollection tncTargetNodeCollection, UInt32 uRecursiveLevel,
                                                   addMode amAddMode)
        {
            UInt32 uMaxArrayRecordsToFetch = uDefaultMaxArrayRecordsToFetch;

            if (null != oObjectToLoad && // there is nothing to load
                0 < uRecursiveLevel--) // don't continue recursion              
            {
                switch (oObjectToLoad.GetType().Name)
                {
                        //all these basic types can be processed in the same since the .ToString() correctly gets the value
                    case "String":
                    case "Int32":
                    case "Int64":
                    case "UInt32":
                    case "UInt64":
                    case "Byte":
                    case "Boolean":
                        const int iStringImageType = 2;
                        TreeNode tnNewNodeForObject = getNewNode(sNodeName + " = " + oObjectToLoad, "", iStringImageType,
                                                                 null);
                        if (null != tnNewNodeForObject)
                            tncTargetNodeCollection.Add(tnNewNodeForObject);
                        break;

                    default:
                        {
                            switch (amAddMode)
                            {
                                case addMode.ObjectValue:
                                    // means we are loading the value of the current object                                     

                                    Int32 iObjectImageType = 0; // 'class' icon
                                    if (oObjectToLoad.GetType().IsArray)
                                        // if it is an array, dont' show it if it doesn't have any items
                                    {
                                        iObjectImageType = 1; // 'array' icon
                                        int iCount = ((Object[]) oObjectToLoad).Length;
                                        if (iCount > uMaxArrayRecordsToFetch)
                                            PublicDI.log.error(
                                                "The number of items in the {0}[{1}] array is bigger than the current uMaxArrayRecordsToFetch: {2}",
                                                oObjectToLoad.GetType().Name, iCount, uMaxArrayRecordsToFetch);
                                        if (iCount == 0)
                                            break;
                                    }

                                    // get the name of the node and if it not set, get its value from the type name
                                    String sNewNodeName = sNodeName;
                                    if (sNewNodeName == "")
                                    {
                                        sNewNodeName = oObjectToLoad.GetType().Name;
                                    }
                                    if (sNewNodeName == "CommonIRDumpCommonIRClassMethodsClassMemberFunctionVariable")
                                    {
                                    }
                                    PublicDI.log.info("{0}   -   {1}   - {2}   ", sNewNodeName, sParentNodeName,
                                                sParentParentNodeName);

                                    if (sParentParentNodeName != "")
                                        sNewNodeName = sNewNodeName.Replace(sParentParentNodeName, "");
                                    // remove parent's parent name to make it easier to read (XML Serialization objects have their parents in their name)

                                    if (sParentNodeName != "")
                                        sNewNodeName = sNewNodeName.Replace(sParentNodeName, "");
                                    // remove parent name to make it easier to read (XML Serialization objects have their parents in their name)
                                    // give the user a visual representation of how many subnodes exist in this node
                                    sNewNodeName += getNumberOfSubNodes(oObjectToLoad);

                                    // create a new node for this objects's data (puting the object valuer in the new TreeNode's Tag)
                                    TreeNode tnNewNode = getNewNode(sNewNodeName, oObjectToLoad.GetType().Name,
                                                                    iObjectImageType, oObjectToLoad);
                                    if (tnNewNode != null)
                                    {
                                        // add it to the current tncTargetNodeCollection
                                        tncTargetNodeCollection.Add(tnNewNode);
                                        // move the tncTargetNodeCollection pointer to the new node 
                                        tncTargetNodeCollection = tnNewNode.Nodes;
                                    }

                                    // and call addObjectPropertiesToTreeNodeCollection to add this node's properties (the iRecursiveLevel controls as sub nodes (uRecursiveLevel controls how deep this one goes)
                                    addObjectPropertiesToTreeNodeCollection(oObjectToLoad, tncTargetNodeCollection,
                                                                            uRecursiveLevel, sParentNodeName);

                                    break;
                                case addMode.ObjectProperties:
                                    // we are loading the object's properties (which are usually the Xml attributes Elements
                                    if (oObjectToLoad.GetType().IsArray) // if it is an array we need to add all items
                                    {
                                        foreach (Object oObjectInArray in (Object[]) oObjectToLoad)
                                            if (0 == uMaxArrayRecordsToFetch--) // until we reach our limit 
                                                break;
                                            else
                                                // invoke loadObjectInTreeNodeCollection recursively (note how now we use addMode.ObjectValue since we only want to add this node)
                                            {
//                                                loadObjectInTreeNodeCollection(oObjectInArray, "", oObjectToLoad.GetType().Name,sParentNodeName, tncTargetNodeCollection, uRecursiveLevel, addMode.ObjectValue);
                                                loadObjectInTreeNodeCollection(oObjectInArray, "", sParentNodeName,
                                                                               sParentParentNodeName,
                                                                               tncTargetNodeCollection, uRecursiveLevel,
                                                                               addMode.ObjectValue);
                                            }
                                    }
                                    else
                                        // if it is not an array let's add all its properties to teh current TreeNode Collection
                                        addObjectPropertiesToTreeNodeCollection(oObjectToLoad, tncTargetNodeCollection,
                                                                                uRecursiveLevel, sParentNodeName);

                                    break;
                            }
                        }
                        break;
                }
            }
        }

        private void addObjectPropertiesToTreeNodeCollection(Object oObjectToLoad,
                                                             TreeNodeCollection tncTargetNodeCollection,
                                                             UInt32 uRecursiveLevel, String sParentNodeName)
        {
            if (oObjectToLoad.GetType().IsArray)
                // check if it is an array since we don't want to add's the .NET Framework's System.Array Properties                
                return;

            // get all properties for this object
            foreach (PropertyInfo piProperty in oObjectToLoad.GetType().GetProperties())
            {
                try
                {
                    // get live instance of the current property
                    Object oObjectValue = oObjectToLoad.GetType().InvokeMember(piProperty.Name,
                                                                               BindingFlags.GetProperty, null,
                                                                               oObjectToLoad, new object[] {});
                    // invoke loadObjectInTreeNodeCollection now also using addMode.ObjectValue since we just want to add this object value 

                    //loadObjectInTreeNodeCollection(oObjectValue, piProperty.Name, oObjectToLoad.GetType().Name, tncTargetNodeCollection, uRecursiveLevel, addMode.ObjectValue);
                    loadObjectInTreeNodeCollection(oObjectValue, piProperty.Name, oObjectToLoad.GetType().Name,
                                                   sParentNodeName, tncTargetNodeCollection, uRecursiveLevel,
                                                   addMode.ObjectValue);
                }
                catch (Exception e)
                {
                    PublicDI.log.error(
                        " In loadObjectInTreeNode  foreach (PropertyInfo piProperty in tObjectToLoad.GetProperties()): {0}",
                        e.Message);
                }
            }
        }

        // create a new node object
        private TreeNode getNewNode(String sText, String sName, Int32 iImageIndex, Object oObject)
        {
            //iItems++;
            if (bSimulateMode)
            {
                iItems++;
                return null;
            }

            {
                incProgressbar();
                var tnNewNode = new TreeNode();
				tnNewNode.Name = sName;
				tnNewNode.Text = sText;
                tnNewNode.ImageIndex = tnNewNode.SelectedImageIndex = iImageIndex;
                tnNewNode.Tag = oObject;
                tnNewNode.ForeColor = Color.Black;  // to handle the weird 'treeView with 1 Node makes the TreeNode Text  white' bug
                return tnNewNode;
            }
        }

        // update all Root Nodes
        public void treeNode_UpdateRootNodes(bool bExpandNodeAfterLoading)
        {
            foreach (TreeNode tnNode in tvTargetTreeView.Nodes)
                treeNode_UpdateNode(tnNode, bExpandNodeAfterLoading);
        }

        // invoked when the user expands or clicks on a node
        public void treeNode_UpdateNode(TreeNode tnSelectedTreeNode, bool bExpandNodeAfterLoading)
        {
            if (bAcceptBeforeExpandEvents)
            {
                if (pbProgressBar != null)
                {
                    iItems = 0;
                    bSimulateMode = true;
                    treeNode_UpdateNode(tnSelectedTreeNode, false, bExpandNodeAfterLoading);
                    if (iItems > 0)
                    {
                        pbProgressBar.Value = 0;
                        pbProgressBar.Maximum = iItems;
                    }
                    bSimulateMode = false;
                }
                treeNode_UpdateNode(tnSelectedTreeNode, false, bExpandNodeAfterLoading);
            }
        }

        // use direcly when a reload is required (for example when the max number of array items has changed
        public void treeNode_UpdateNode(TreeNode tnSelectedTreeNode, bool bForceUpdate, bool bExpandNodeAfterLoading)
        {
            if (bUpdatingNode == false)
                // prevents infinite loops since the tnSelectedTreeNode.Expand(); will also trigger this method
            {
                bUpdatingNode = true;
                if (bForceUpdate) // remove existing nodes if we are updating this node
                    tnSelectedTreeNode.Nodes.Clear();
                if (tnSelectedTreeNode.Nodes.Count == 0) // Only load each object once                          
                {
                    // invoke the recursive methods loadObjectInTreeNodeCollection now with addMode.ObjectProperties since we want to add all its properties
                    loadObjectInTreeNodeCollection(tnSelectedTreeNode.Tag, "", "", "", tnSelectedTreeNode.Nodes,
                                                   uRecursiveDepth, addMode.ObjectProperties);
                }
                else
                    foreach (TreeNode tnSubNodes in tnSelectedTreeNode.Nodes)
                    {
                        // calculate Parent and ParentParent Text value
                        String sParentText = (tnSubNodes.Parent == null) ? "" : tnSubNodes.Parent.Text;
                        if (sParentText.IndexOf('(') > -1)
                            sParentText = sParentText.Substring(0, sParentText.IndexOf('(')).Trim();
                        /* String sParentParentText = (tnSubNodes.Parent == null || tnSubNodes.Parent.Parent == null)
                                                       ? ""
                                                       : tnSubNodes.Parent.Parent.Text;*/
                        /*if (sParentParentText.IndexOf('(') > -1)
                            sParentParentText = sParentParentText.Substring(0, sParentParentText.IndexOf('(')).Trim();
                        */
                        if (tnSubNodes.Nodes.Count == 0) // Only load each object once                         
                            loadObjectInTreeNodeCollection(tnSubNodes.Tag, "", sParentText, "", tnSubNodes.Nodes,
                                                           uRecursiveDepth - 1, addMode.ObjectProperties);
                    }
                if (bExpandNodeAfterLoading)
                    tnSelectedTreeNode.Expand();
                bUpdatingNode = false;
            }
        }

        private void calculateNumberOfItemsForObjectAndConfigureProgressBar()
        {
            iItems = 0;
            if (pbProgressBar != null)
            {
                PublicDI.log.debug("calculating size of conversion");
                bSimulateMode = true;
                // calculate number of items by performing the same actions in bSimulateMode = true
                loadObjectInTreeNodeCollection(oRootObject, "", "{----}", "{----}", tvTargetTreeView.Nodes,
                                               uRecursiveDepth, addMode.ObjectValue);
                tvTargetTreeView.Nodes.Clear();
                bSimulateMode = false;
                // set progressbar values
                pbProgressBar.Value = 0;
                pbProgressBar.Maximum = iItems;
            }
            iItems = 0;
            PublicDI.log.debug("# items to load: {0}", iItems);
        }

        private void incProgressbar()
        {
            if (pbProgressBar != null)
            {
                // update ProgressBar
                if (pbProgressBar.Value < pbProgressBar.Maximum)
                {
                    pbProgressBar.Value++;
                    Application.DoEvents();
                }
                //            else
                //                PublicDI.log.error("Wrong 'items to process' count since Value = pbProgressBar.Maximum : {0} {1} ", pbProgressBar.Value, iItems++);
            }
        }


        private static String getNumberOfSubNodes(Object oObject)
        {
            UInt32 uNunberOfSubNodes;
            if (oObject.GetType().IsArray)
                uNunberOfSubNodes = (UInt32) ((Object[]) oObject).Length;
            else
                uNunberOfSubNodes = (UInt32) oObject.GetType().GetProperties().Length;
            return "   (" + uNunberOfSubNodes + ")";
        }

        public UInt32 getRecursiveDepth()
        {
            return uRecursiveDepth;
        }

        public UInt32 getMaxNumberOfArrayRecordsToFetch()
        {
            return uDefaultMaxArrayRecordsToFetch;
        }

        public void setRecursiveDepth(UInt32 uNewRecursiveDepth)
        {
            uRecursiveDepth = 1 + uNewRecursiveDepth;
            PublicDI.log.debug("Configured recursive depth to: {0}", uNewRecursiveDepth);
        }

        public void setMaxNumberOfArrayRecordsToFetch(UInt32 uMaxNumber)
        {
            uDefaultMaxArrayRecordsToFetch = uMaxNumber;
            PublicDI.log.debug("Configured maximum number of array records to fetch to: {0}", uMaxNumber);
        }


        // addMode defines what will be added: an object name or its properties (this creates is required for the recursive call to work)
    }
}
