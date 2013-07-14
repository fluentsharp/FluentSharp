// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.O2Findings;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_TraceTreeView
    {
        public event Action<IO2Trace> _onTraceSelected;
        public event Callbacks.dMethod_Object onTreeNodeAfterSelect;
        public event Callbacks.dMethod_String onTreeNodeAfterLabelEdit;
        public bool runOnLoad = true;
        public bool DontSelectNodeOnLoad { get; set; }
        public IO2Trace o2Trace          { get; set; }
        public IO2Finding o2Finding      { get; set; }
        public object selectedNodeTag { get; set; }
        public TreeNode selectedNode { get; set; }
        public TreeView traceTreeView { get; set; }
        public bool bMoveTraces { get; set; }

        private string tracePropertyToUseAsNodeText = "signature";


        private void onLoad()
        {
            if (runOnLoad)
            {
                traceTreeView = tvSmartTrace;
                bMoveTraces = true;
                runOnLoad = false;
            }
        }

        public void loadO2Finding(IO2Finding _o2Finding)        
        {
            Thread_Invoke_ExtensionMethods.invokeOnThread((Control)this, () =>
                                    {
                                        o2Finding = _o2Finding;
                                        showO2TraceTree();
                                        return default(object);
                                    });
        }

        public void showO2TraceTree()
        {
            Thread_Invoke_ExtensionMethods.invokeOnThread(tvSmartTrace, () =>
                                            {
                                                if (o2Finding != null)
                                                {
                                                    tvSmartTrace.Visible = false;
                                                    tvSmartTrace.Nodes.Clear();
                                                    foreach (IO2Trace chilldO2Trace in o2Finding.o2Traces)
                                                        loadO2TraceIntoTreeNodeCollection(chilldO2Trace, tvSmartTrace.Nodes);
                                                    tvSmartTrace.ExpandAll();
                                                    if (DontSelectNodeOnLoad.isFalse())
                                                        if (tvSmartTrace.Nodes.Count > 0 && tvSmartTrace.SelectedNode == null)
                                                            tvSmartTrace.SelectedNode = tvSmartTrace.Nodes[0];
                                                    tvSmartTrace.Visible = true;
                                                }
                                                return default(object);
                                            });
        }

        public void loadO2TraceIntoTreeNodeCollection(IO2Trace o2TraceToLoad, TreeNodeCollection treeNodeCollection)
        {
            string nodeText = getNodeText(o2TraceToLoad);

            TreeNode newNode = O2Forms.newTreeNode(nodeText, nodeText, 0, o2TraceToLoad);
            newNode.ForeColor = OzasmtUtils.getTraceColorBasedOnTraceType(o2TraceToLoad);
            treeNodeCollection.Add(newNode);
            if (o2TraceToLoad == o2Trace)
                tvSmartTrace.SelectedNode = newNode;
            foreach (O2Trace childO2Trace in o2TraceToLoad.childTraces)
                loadO2TraceIntoTreeNodeCollection(childO2Trace, newNode.Nodes);
        }

        private string getNodeText(IO2Trace o2TraceToLoad)
        {
            string nodeText = "";
            if (tracePropertyToUseAsNodeText == "SourceCode")
            {
                nodeText = Files_WinForms.getLineFromSourceCode(o2TraceToLoad.file, o2TraceToLoad.lineNumber);
                if (nodeText == "")
                    nodeText = "[no source code available]";
                //Files.getLineFromSourceCode(o2TraceToLoad.file, o2TraceToLoad.lineNumber) :
            }
            else
                nodeText = PublicDI.reflection.getProperty(tracePropertyToUseAsNodeText, o2TraceToLoad).ToString();
            if (nodeText == "")
                nodeText = (o2TraceToLoad.signature != "") ? o2TraceToLoad.signature : o2TraceToLoad.method;

            // hack to deal with encoded quotes
            nodeText = nodeText.Replace("&quot;","\"");
            return nodeText;
        }

        public void insertNewO2Trace()
        {
            if (o2Finding.o2Traces.Count == 0)
                o2Finding.o2Traces = new List<IO2Trace>().add(new O2Trace("new root trace"));
            else if (tvSmartTrace.SelectedNode != null && tvSmartTrace.SelectedNode.Tag.GetType().Name == "O2Trace")
            {
                var newO2Trace = new O2Trace("insertedNewTrace");
                if (tvSmartTrace.SelectedNode.Parent != null)
                {
                    var parentNodeTrace = (IO2Trace)tvSmartTrace.SelectedNode.Parent.Tag;
                    var curentNodeTrace = (IO2Trace)tvSmartTrace.SelectedNode.Tag;
                    newO2Trace.childTraces.Add(curentNodeTrace);
                    parentNodeTrace.childTraces.Add(newO2Trace);
                    parentNodeTrace.childTraces.Remove(curentNodeTrace);
                }
                else
                {
                    var curentNodeTrace = (IO2Trace)tvSmartTrace.SelectedNode.Tag;
                    newO2Trace.childTraces.Add(curentNodeTrace);
                    o2Finding.o2Traces.Remove(curentNodeTrace);
                    o2Finding.o2Traces.Add(newO2Trace);
                }
                //tvSmartTrace.SelectedNode.Tag = newO2Trace;
                o2Trace = newO2Trace;


                //currentO2Trace.childTraces.Add(((O2Trace)tvSmartTrace.SelectedNode.Tag));
                //tvSmartTrace.SelectedNode.Tag = currentO2Trace;
                //.childTraces.Add(currentO2Trace);
            }
            showO2TraceTree();
        }

        public void appendNewO2Trace()
        {
            if (o2Finding.o2Traces.Count == 0)
                o2Finding.o2Traces = new List<IO2Trace>().add(new O2Trace("new root trace"));
            else if (tvSmartTrace.SelectedNode != null && tvSmartTrace.SelectedNode.Tag.GetType().Name == "O2Trace")
            {
                o2Trace = new O2Trace("newChildTrace");
                ((O2Trace)tvSmartTrace.SelectedNode.Tag).childTraces.Add(o2Trace);
            }
            showO2TraceTree();
        }

        private void handleDragDrop(DragEventArgs e)
        {
            var droppedObject = Dnd.tryToGetObjectFromDroppedObject(e);

            switch (droppedObject.GetType().Name)
            { 
                case "O2Trace":
                    var droppedTrace = (IO2Trace)droppedObject;
                    var selectedO2Trace = o2Trace;
                    if (selectedO2Trace == droppedTrace)
                        PublicDI.log.error("on tvSmartTrace_DragDrop: It is not possible to drop a trace on it seft");
                    else if (bMoveTraces &&
                             (OzasmtSearch.isO2TraceAChildTraceOfO2Trace(selectedO2Trace, droppedTrace)))
                    {
                        // if we draged into a parent, we need to make a copy first, then copy it then delete the original
                        IO2Trace copiedO2Trace = OzasmtCopy.createCopy(droppedTrace);
                        OzasmtGlue.deleteO2Trace(o2Finding.o2Traces, droppedTrace);
                        selectedO2Trace.childTraces.Add(copiedO2Trace);
                    }
                    else if (bMoveTraces &&
                             OzasmtSearch.isO2TraceAChildTraceOfO2Trace(droppedTrace, selectedO2Trace))
                    {
                        PublicDI.log.error(
                            "on tvSmartTrace_DragDrop: Could not move trace since it is not possible to drop a trace into its own child node");
                    }
                    else
                    {
                        IO2Trace copiedO2Trace = OzasmtCopy.createCopy(droppedTrace);

                        if (bMoveTraces)
                            OzasmtGlue.deleteO2Trace(o2Finding.o2Traces, droppedTrace);
                        selectedO2Trace.childTraces.Add(copiedO2Trace);
                    }
                    showO2TraceTree();
                    break;
                case "O2Finding":
                    loadO2Finding((O2Finding)droppedObject);
                    break;
                case"TreeNode":
                    var tagObject = ((TreeNode) droppedObject).Tag;
                    if (tagObject != null)
                        if (tagObject is O2Finding)
                            loadO2Finding((O2Finding) tagObject);
                    break;

            }

            /*       var droppedTrace2 = (O2Trace)Dnd.tryToGetObjectFromDroppedObject(e, typeof(O2Trace));
            if (droppedTrace2 != null)
            {
            
            }
            else
            {
                var droppedO2Finding = (O2Finding)Dnd.tryToGetObjectFromDroppedObject(e, typeof(O2Finding));
                if (droppedO2Finding != null)
                    loadO2Finding(droppedO2Finding);
                else
                {
                    droppedO2Finding = (O2Finding)Dnd.tryToGetObjectFromDroppedObject(e, typeof(TreeNode));

                }
            }*/
            
        }


        internal void expandAll()
        {
            this.invokeOnThread(() => traceTreeView.ExpandAll());
        }

        internal void collapseAll()
        {
            this.invokeOnThread(() => traceTreeView.CollapseAll());
        }

        private void onTreeViewAfterSelect(bool bShowLineInSourceFile)
        {
            if (traceTreeView.SelectedNode != null)
            {
                selectedNode = traceTreeView.SelectedNode;
                selectedNodeTag = traceTreeView.SelectedNode.Tag;
                Callbacks.raiseRegistedCallbacks(onTreeNodeAfterSelect, new[] { selectedNodeTag });
                if (selectedNodeTag is O2Trace)
                {
                    o2Trace = (O2Trace) selectedNodeTag;
                    Callbacks.raiseRegistedCallbacks(_onTraceSelected, new[] { o2Trace });
                    if (bShowLineInSourceFile)
                    {
                        raiseShowLineInSourceCodeFile(o2Trace.file, (int) o2Trace.lineNumber);                        
                    }
                }                
                traceTreeView.Focus();
            }
            
        }

        private void raiseShowLineInSourceCodeFile(string fileName, int lineNumber)
        {
            O2Thread.mtaThread(
                () =>
                    {
                        var thread = O2Messages.fileOrFolderSelected(fileName, lineNumber);
                        thread.Join(); // wait for O2MessageExecution
                        traceTreeView.invokeOnThread(()=> traceTreeView.Focus());
                    });
        }
    }
}
