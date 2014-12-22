using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms
{    
    public static class WinForms_ExtensionMethods_TreeView
    {
		public static TreeView  treeView(this TreeNode treeNode)
        {
            if (treeNode.isNull())
                return null;
            return treeNode.TreeView;
        }
        public static TreeView  treeView(this List<TreeNode> treeNodes)
        {
            if (treeNodes.notNull() && treeNodes.size() > 0)
                return treeNodes[0].TreeView;
            return null;					 // this could cause problems 
        }        
        public static TreeView  add_TreeView(this Control control)
        {
            return (TreeView) control.invokeOnThread(
                                  () =>{
                                          	var treeView = new TreeView();
											treeView.Dock = DockStyle.Fill;
                                          	control.Controls.Add(treeView);
                                          	//change the default dehaviour of treeviews of not selecting on mouse click (big problem when using right click) 
                                          	treeView.NodeMouseClick += (sender, e) => { treeView.SelectedNode = e.Node; };
                                          	treeView.HideSelection = false;
                                          	return treeView;
                                      	});
        }
		public static TreeView	add_TreeView(this Control control, ref TreeView treeView)
		{
			return treeView = control.add_TreeView();
		}

        public static TreeNode  add_Node(this TreeView treeView, TreeNode rootNode, string nodeText, Color textColor)
        {
            TreeNode newNode = treeView.add_Node(rootNode, nodeText); //, nodeText,0,textColor,null);
            newNode.ForeColor = textColor;
            return newNode;
        }
        public static TreeNode  add_Node(this TreeView treeView, string nodeText, int imageId, Color color, object nodeTag)
        {
            return (TreeNode) treeView.invokeOnThread((()
                                                       =>
                                                           {
                                                               TreeNode treeNode = treeView.add_Node(nodeText, nodeTag);
                                                               treeNode.ForeColor = color;
                                                               treeNode.ImageIndex = imageId;
                                                               treeNode.SelectedImageIndex = imageId;
                                                               return treeNode;
                                                           }));
        }
        public static TreeNode  add_Node(this TreeView treeView, string nodeText, object nodeTag)
        {
            return (TreeNode) treeView.invokeOnThread((()=>{
																var treeNode = new TreeNode();
																treeNode.Tag = nodeTag;
																treeNode.Text = nodeText;
																treeNode.Name = nodeText;                                                            
                                                               	treeView.Nodes.Add(treeNode);
                                                               //treeView.Refresh();
                                                               	return treeNode;
                                                           }));
        }
        public static int       add_Node(this TreeView treeView, TreeNode treeNode)
        {
            return (int) treeView.invokeOnThread(()=>{ return treeView.Nodes.Add(treeNode); });
        }
        public static TreeNode  add_Node(this TreeView treeView, string nodeText)
        {
            return (TreeNode)treeView.invokeOnThread(() => { return treeView.Nodes.Add(nodeText); });
        }
        public static TreeNode  add_Node(this TreeView treeView, TreeNode treeNode, string nodeText)
        {
            return (TreeNode) treeView.invokeOnThread(() => { return O2Forms.newTreeNode(treeNode.Nodes, nodeText, 0, null, false); });
        }
        public static TreeNode  add_Node(this TreeView treeView, TreeNode treeNode, string nodeText, object nodeTag, bool addDummyNode)
        {
            return (TreeNode) treeView.invokeOnThread((()
                                                       =>
                                                           {
                                                               TreeNode newNode = O2Forms.newTreeNode(treeNode.Nodes,
                                                                                                      nodeText, 0,
                                                                                                      nodeTag);
                                                               if (addDummyNode)
                                                                   newNode.Nodes.Add("DummyNode_1");
                                                               return newNode;
                                                           }));
        }
        public static TreeNode  add_Node(this TreeView treeView, string nodeText, object nodeTag, bool addDummyNode)
        {
            return (TreeNode) treeView.invokeOnThread((()
                                                       =>
                                                           {
                                                               TreeNode treeNode = treeView.add_Node(nodeText, nodeTag);
                                                               if (addDummyNode)
                                                                   treeNode.Nodes.Add("DummyNode_2");
                                                               return treeNode;
                                                           }));
        }
        public static TreeNode  add_Node(this TreeView treeView, object tag)
        {
            if (treeView != null)
                return treeView.add_Node((tag != null) ? tag.ToString() : "" ,tag);
            return null;
        }
        public static TreeNode  add_Node(this TreeNode treeNode, object textAndTag)
        {
            if (treeNode != null && textAndTag != null)
                return treeNode.add_Node(textAndTag.str(), textAndTag);
            return null;
        }
        public static TreeNode  add_Node(this TreeNode treeNode, string text, object tag)
        {
            if (treeNode != null)
                return treeNode.treeView().add_Node(treeNode, text, tag, false);
            return null;
        }
        public static TreeNode  add_Node(this TreeNode treeNode, string text, object tag, bool addDummyChildNode)
        {
            if (treeNode != null)
                return treeNode.treeView().add_Node(treeNode, text, tag, addDummyChildNode);
            return null;
        }

        public static TreeNode  add_Node(this TreeNodeCollection treeNodeCollection, string nodeText, object tag)
        {
            if (treeNodeCollection != null)
                return treeNodeCollection.parentTreeNode().add_Node(nodeText, tag);
            return null;
        }
        public static TreeNode  add_Nodes(this TreeNode treeNode, IEnumerable collection)
        {
            if (treeNode != null)            
                foreach (var item in collection)
                    treeNode.add_Node(item);
            return treeNode;
        }
        public static TreeView  add_Nodes(this TreeView treeView, IEnumerable collection)
        {
            return treeView.add_Nodes(collection, false, "");
        }        
        public static TreeView  add_Nodes(this TreeView treeView, IEnumerable collection, bool clearTreeView)
        {
            return treeView.add_Nodes(collection, clearTreeView, "");
        }
        public static TreeView  add_Nodes(this TreeView treeView, IEnumerable collection, bool clearTreeView, string filter)
        {
            return (TreeView)treeView.invokeOnThread(
                () =>
                {
                    if (clearTreeView)
                        treeView.clear();
                    foreach (var item in collection)
                        if (filter == "" || item.str().regEx(filter))
                            treeView.add_Node(item.str(), item);                    
                    return treeView;
                });
        }
        public static TreeView  add_Nodes(this TreeView treeView, IDictionary dictionary, bool clearTreeView, string filter)
        {
            return (TreeView)treeView.invokeOnThread(
                () =>
                {
                    if (clearTreeView)
                        treeView.clear();
                    foreach (var key in dictionary.Keys)
                    {
                        var text = key.str();
                        if (filter == "" || text.regEx(filter))
                            treeView.add_Node(text, dictionary[key]);
                    }
                    return treeView;
                });
        }
        public static TreeView  add_Nodes(this TreeView treeView, Dictionary<string, List<string>> dictionary)
		{
			foreach(var item in dictionary)
			{
				var nodeText = "{0}    ({1} items)".format(item.Key, item.Value.size());
				treeView.add_Node(nodeText, item.Value).add_Nodes(item.Value);
			}
			return treeView;
		}
        public static TreeView  add_Nodes(this TreeView treeView, params string[] nodes)
        {
            return treeView.add_Nodes(nodes.toList());
        }

        public static TreeView  add_Nodes<T>(this TreeView treeView, Dictionary<string, List<T>> items)
        {
            return treeView.add_Nodes(items, -1, null);
        }
        public static TreeView  add_Nodes<T>(this TreeView treeView, Dictionary<string, List<T>> items, int maxNodeTextSize, ProgressBar progressBar)
        {
            return treeView.rootNode().add_Nodes(items, maxNodeTextSize, progressBar).treeView();
        }
        public static TreeNode  add_Nodes<T>(this TreeNode treeNode, Dictionary<string, List<T>> items)
        {
            return treeNode.add_Nodes(items, -1, null);
        }
        public static TreeNode  add_Nodes<T>(this TreeNode treeNode, Dictionary<string, List<T>> items, int maxNodeTextSize, ProgressBar progressBar)
        {
            treeNode.treeView().invokeOnThread(
                () =>
                {
                    progressBar.maximum(items.size());
                    //for performance reasons create the treeNode here (good for the cases where we need to add 10,000+ nodes
                    var nodesToAdd = new List<TreeNode>();
                    foreach (var item in items)
                    {
                        var nodeText = (maxNodeTextSize > 1 && item.Key.size() > maxNodeTextSize)
                                            ? item.Key.Substring(0, maxNodeTextSize).add("...")
                                            : item.Key;
                        TreeNode newNode = new TreeNode();
						newNode.Name = nodeText;
						newNode.Text = nodeText;
                        newNode.ImageIndex = newNode.SelectedImageIndex = 0;
                        newNode.Tag = item.Value;                        
                        if (item.Value.size() > 1)
                            newNode.Nodes.Add("DummyNode_1");
                        nodesToAdd.Add(newNode);                        
                        progressBar.increment(1);                        
                    }
                    treeNode.Nodes.AddRange(nodesToAdd.ToArray());
                });
            return treeNode;

        }
        public static TreeNode  add_Nodes<T, T1>(this TreeNode treeNode, Dictionary<T, List<T1>> dictionary)
        {
            foreach (var item in dictionary)
            {
                var keyNode = treeNode.add_Node(item.Key);
                foreach (var listItem in item.Value)
                    keyNode.add_Node(listItem);
            }
            return treeNode;
        }
        
		public static TreeView  add_Nodes<T>(this TreeView treeView, IEnumerable<T> collection, bool addDummyNode)
		{
				
			treeView.rootNode().add_Nodes(collection, (item)=> item.str() ,(item)=> item,(item)=> addDummyNode);			
			return treeView;
		}		
		public static TreeNode  add_Nodes<T>(this TreeNode treeNode, IEnumerable<T> collection, bool addDummyNode)
		{
			return treeNode.add_Nodes(collection, (item)=> item.str() ,(item)=> item, (item)=> addDummyNode);						
		}				
		public static TreeView  add_Nodes<T>(this TreeView treeView, IEnumerable<T> collection, Func<T, bool> getAddDummyNode)
		{
			treeView.rootNode().add_Nodes(collection, (item)=> item.str() ,(item)=> item, getAddDummyNode);			
			return treeView;
		}		
		public static TreeNode  add_Nodes<T>(this TreeNode treeNode, IEnumerable<T> collection, Func<T, bool> getAddDummyNode)
		{
			return treeNode.add_Nodes(collection, (item)=> item.str() ,(item)=> item, getAddDummyNode);						
		}				
		public static TreeView  add_Nodes<T>(this TreeView treeView, IEnumerable<T> collection, Func<T,string> getNodeName)
		{
			treeView.rootNode().add_Nodes(collection, getNodeName,(item)=> item,(item)=> false);			
			return treeView;
		}				
		public static TreeNode  add_Nodes<T>(this TreeNode treeNode, IEnumerable<T> collection, Func<T,string> getNodeName)
		{
			return treeNode.add_Nodes(collection, getNodeName, (item)=> item, (item)=> false);			
		}						
		public static TreeView  add_Nodes<T>(this TreeView treeView, IEnumerable<T> collection, Func<T,string> getNodeName, Func<T, Color> getColor)
		{
			return treeView.add_Nodes<T>(collection, getNodeName, (item)=> false, getColor);
		}		
		public static TreeNode  add_Nodes<T>(this TreeNode treeNode, IEnumerable<T> collection, Func<T,string> getNodeName, Func<T, Color> getColor)
		{
			return treeNode.add_Nodes<T>(collection, getNodeName, (item)=> false, getColor);
		}				
		public static TreeView  add_Nodes<T>(this TreeView treeView, IEnumerable<T> collection, Func<T,string> getNodeName, Func<T, bool> getAddDummyNode, Func<T, Color> getColor)
		{
			return treeView.add_Nodes<T>(collection, getNodeName, (item)=>item, getAddDummyNode, getColor);
		}		
		public static TreeNode  add_Nodes<T>(this TreeNode treeNode, IEnumerable<T> collection, Func<T,string> getNodeName, Func<T, bool> getAddDummyNode, Func<T, Color> getColor)
		{
			return treeNode.add_Nodes<T>(collection, getNodeName, (item)=>item, getAddDummyNode, getColor);
		}				
		public static TreeView  add_Nodes<T>(this TreeView treeView, IEnumerable<T> collection, Func<T,string> getNodeName, Func<T,object> getTagValue,  Func<T, bool> getAddDummyNode, Func<T, Color> getColor)
		{
			treeView.rootNode().add_Nodes(collection, getNodeName,getTagValue, getAddDummyNode, getColor);
			return treeView;
		}				
		public static TreeNode  add_Nodes<T>(this TreeNode treeNode, IEnumerable<T> collection, Func<T,string> getNodeName, Func<T,object> getTagValue, Func<T, bool> getAddDummyNode, Func<T, Color> getColor)
		{
            if (treeNode.isNull())
                "[TreeNode][add_Nodes] provided treeNode was null".error();
            else 
			    foreach(var item in collection)
			    {
				    var newNode = WinForms_ExtensionMethods_TreeView.add_Node(treeNode,getNodeName(item), getTagValue(item), getAddDummyNode(item));
				    newNode.color(getColor(item));
			    }
			return treeNode;
		}				
		public static TreeView  add_Nodes<T>(this TreeView treeView, IEnumerable<T> collection, Func<T,string> getNodeName, bool addDummyNode)
		{
			treeView.rootNode().add_Nodes(collection, getNodeName, (item)=> item,(item)=> addDummyNode);			
			return treeView;
		}				
		public static TreeNode  add_Nodes<T>(this TreeNode treeNode, IEnumerable<T> collection, Func<T,string> getNodeName, bool addDummyNode)
		{
			return treeNode.add_Nodes(collection, getNodeName, (item)=> item,(item)=> addDummyNode);			
		}		
		public static TreeView  add_Nodes<T>(this TreeView treeView, IEnumerable<T> collection, Func<T,string> getNodeName, Func<T, object> getTagValue, Func<T,bool> getAddDummyNode)
		{
			treeView.rootNode().add_Nodes(collection, getNodeName, getTagValue, getAddDummyNode);
			return treeView;
		}		
		public static TreeNode  add_Nodes<T>(this TreeNode treeNode, IEnumerable<T> collection, Func<T,string> getNodeName, Func<T, object> getTagValue, Func<T,bool> addDummyNode)
		{
			foreach(var item in collection)
				WinForms_ExtensionMethods_TreeView.add_Node(treeNode,getNodeName(item), getTagValue(item), addDummyNode(item));
			return treeNode;
		}

        public static TreeNode  insert_Node(this TreeView treeView, string text)
		{
			return treeView.insert_Node(text,0);
		}		
		public static TreeNode  insert_Node(this TreeView treeView, string text, int position)
		{
			return treeView.insert_TreeNode(text,text,position);
		}		
		public static TreeNode  insert_TreeNode(this TreeView treeView, string text, object tag, int position)
		{
			return treeView.rootNode().insert_TreeNode(text,tag, position);
		}
		public static TreeNode  insert_TreeNode(this TreeNode treeNode, string text, object tag, int position)
		{
			return (TreeNode)treeNode.treeView().invokeOnThread(
				()=>{
						var newNode = treeNode.Nodes.Insert(position, text);
						newNode.Tag = tag;
						return treeNode;
					});
		}

        public static TreeView  add_Files(this TreeView treeView, String folder)
		{
		    return treeView.add_Files(folder, "*.*",true);
		}		
		public static TreeView  add_Files(this TreeView treeView, String folder, string filter)
		{
		    return treeView.add_Files(folder, filter,true);
		}		
		public static TreeView  add_Files(this TreeView treeView, String folder, string filter, bool recursive)
		{
			return treeView.add_Files(folder.files(filter,recursive));
		}		
		public static TreeView  add_Files(this TreeView treeView, List<string> files)
		{
			return treeView.add_Nodes(files, (file)=>file.fileName());
		}		
		public static TreeView  add_File(this TreeView treeView, string file)
		{
			return treeView.add_Files(file.wrapOnList());
		}


        public static TreeNode  clear(this TreeNode treeNode)
        {
            if (treeNode != null && treeNode.treeView() !=null)
                return (TreeNode)treeNode.treeView().invokeOnThread(() =>
                    {
                        treeNode.Nodes.Clear();
                        return treeNode;
                    });
            return null;
        }
        public static TreeNode  selected(this TreeView treeView)
        {
            return treeView.current();
        }
        public static TreeNode  selectedNode(this TreeView treeView)
        {
            return treeView.current();
        }
        public static TreeNode  current(this TreeView treeView)
        {
            return (TreeNode)treeView.invokeOnThread(() => { return treeView.SelectedNode; });
        }

        public static TreeView          expand(this TreeView treeView)
        {
            treeView.nodes().forEach((node) => treeView.expand(node));
            return treeView;
        }
        public static TreeView          expand(this TreeView treeView, TreeNode treeNode)
        {
            treeView.invokeOnThread(() => treeNode.Expand());
            return treeView;
        }
        public static List<TreeNode>    expand(this List<TreeNode> treeNodes)
        {
            treeNodes.forEach<TreeNode>((node) => node.expand());
            return treeNodes;
        }
        public static TreeNode          expand(this TreeNode treeNode)
        {
            return (TreeNode)treeNode.treeView().invokeOnThread(
                () =>
                {
                    treeNode.Expand();
                    return treeNode;
                });
        }
        public static TreeView          expandAll(this TreeView treeView, bool selectFirst)
        {
            treeView.expandAll();
            if (selectFirst)
                treeView.selectFirst().showSelection();
            return treeView;
        }
        public static TreeView          expandAll(this TreeView treeView)
        {
            treeView.invokeOnThread(() => treeView.ExpandAll());
            return treeView;
        }

        public static void      selectNode(this TreeView treeView, int nodeToSelect)
        {
            treeView.invokeOnThread(
                () =>
                    {
                        if (treeView.Nodes.Count > 0)
                            treeView.SelectedNode = treeView.Nodes[0];
                    });
        }
        public static TreeView  clear(this TreeView treeView, TreeNode treeNode)
        {
            return (TreeView)treeView.invokeOnThread(() =>
                    {
                        treeNode.Nodes.Clear();
                        return treeView;
                    });
        }
        public static TreeView clear(this TreeView treeView)
        {
            return (TreeView) treeView.invokeOnThread(()
                                    =>
                                        {                                            
                                            treeView.Nodes.Clear();
                                            return treeView;
                                        });
        }
                
        public static TreeView  remove_Node(this TreeView treeView, TreeNode treeNode)
        {
            return (TreeView)treeView.invokeOnThread(
                                () =>
                                {
                                    treeView.Nodes.Remove(treeNode);
                                    return treeView;
                                });
        }
        public static TreeView  remove_Nodes(this TreeView treeView, List<TreeNode> treeNodes)
        {
            return (TreeView)treeView.invokeOnThread(
                                () =>
                                {
                                    foreach(var treeNode in treeNodes)
                                        treeView.Nodes.Remove(treeNode);
                                    return treeView;
                                });
        }
        public static TreeView  selectFirst(this TreeView treeView)
        {
            if (treeView.notNull())
            {
                var nodes = treeView.nodes();
                treeView.selectedNode(nodes.first());
            }
            return treeView;
        }
		public static TreeView	selectSecond(this TreeView treeView)
		{
			treeView.nodes().second().selected();
			return treeView;
		}
		public static TreeView	selectThird(this TreeView treeView)
		{
			treeView.nodes().third().selected();
			return treeView;
		}
        public static TreeView  selectNode(this TreeView treeView, TreeNode treeNode)
        {
            return (TreeView)treeView.invokeOnThread(
                () =>
                {
                    treeView.SelectedNode = treeNode;
                    return treeView;
                });
        }
        public static TreeView  selectedNode(this TreeView treeView, TreeNode treeNode)
        {
            if (treeView.isNull() || treeNode.isNull())
                return treeView;
            return (TreeView)treeView.invokeOnThread(
                () =>
                {
                    treeView.SelectedNode = treeNode;
                    return treeView;
                });
        }
        public static TreeView  showSelection(this TreeView treeView)
        {            
            return (TreeView)treeView.invokeOnThread(
                () =>
                {
                    treeView.HideSelection = false;
                    return treeView;
                });
        }
        public static TreeNode  selected(this TreeNode treeNode)
        {            
            return (TreeNode)treeNode.treeView().invokeOnThread(
                () =>
                {
                    treeNode.treeView().SelectedNode = treeNode;
                    return treeNode;
                });
        }

        //get Node(s)
        public static TreeNode node(this TreeView treeView, string text)
        {
            return treeView.rootNode().node(text);
        }
        public static TreeNode node(this TreeNode treeNode, string text)
        {
            if (text.valid())
                foreach (var node in treeNode.nodes())
                    if (node.get_Text() == text)
                        return node;
            return null;
        }
        public static List<TreeNode>    allNodes(this TreeView treeView)
        {
            return treeView.nodes().allNodes();
        }
        public static List<TreeNode>    allNodes(this List<TreeNode> nodes)
        {
            var ltnNodes = new List<TreeNode>();
            foreach (TreeNode tnNode in nodes)
            {
                ltnNodes.Add(tnNode);
                ltnNodes.AddRange(allNodes(tnNode.nodes()));
            }
            return ltnNodes;
        }
        public static List<TreeNode>    nodes(this TreeView treeView)
        {            
            var nodes = new List<TreeNode>();
            if (treeView.notNull())
                treeView.Nodes.toList<TreeNode>()
                              .forEach(nodes.Add);
            return nodes;
        }
        public static List<TreeNode>    nodes(this TreeNode treeNode)
        {
            var nodes = new List<TreeNode>();
            if (treeNode.notNull())
                treeNode.Nodes.toList<TreeNode>().forEach(nodes.Add);
            return nodes;
        }
        public static List<TreeNode>    nodes(this List<TreeNode> treeNodes)
        {
            var results = new List<TreeNode>();
            treeNodes.forEach<TreeNode>((node) => results.AddRange(node.nodes()));
            return results;
        }
        public static List<TreeNode>    nodes(this TreeView treeView, bool recursive)
        {
            if (recursive)
                return treeView.allNodes();
            return treeView.nodes();
        }              
        public static List<TreeNode>    colorNodes(this List<TreeNode> nodes, Color color)
		{
			if (nodes.notNull() && nodes.size()>0)
			{
				var treeView = nodes.first().treeView();
				treeView.beginUpdate();
				foreach(var node in nodes)
					node.foreColor(color);
				treeView.endUpdate();
			}
			return nodes;
		}

        public static TreeView  sort(this TreeView treeView)
        {
            treeView.invokeOnThread(() => treeView.Sort());
            return treeView;
        }
        public static TreeNode  parentTreeNode(this TreeNodeCollection treeNodeCollection)
        {
            return (TreeNode)treeNodeCollection.field("owner");
        }
        public static TreeView  parentTreeView(this TreeNodeCollection treeNodeCollection)
        {
            return treeNodeCollection.parentTreeNode().treeView();
        }
        public static TreeNode  rootNode(this TreeView treeView)
        {
            if (treeView.notNull())
                return treeView.Nodes.parentTreeNode();
            return null;
        }       

        
        public static TreeView  show_Object(this TreeView treeView, object objectToShow)
        {
            if (objectToShow != null)
            {
                treeView.autoExpandObjects();
                treeView.add_Node(objectToShow.str(), objectToShow, true);
            }
            return treeView;
        }
        public static TreeNode  show_Object(this TreeNode treeNode, object objectToShow)
        {
            if (objectToShow != null)
            {
                treeNode.treeView().autoExpandObjects();
                treeNode.add_Node(objectToShow.str(), objectToShow, true);
            }
            return treeNode;
        }

        public static TreeView  autoExpandObjects(this TreeView treeView)
        {
            return treeView.autoExpandObjects<System.Object>();
        }
        public static TreeView  autoExpandObjects<T>(this TreeView treeView)
        {
            // remove any previous BeforeExpand event handlers for type T
            var eventHandlers = treeView.getEventHandlers("BeforeExpand");
            if (eventHandlers != null)
                foreach (TreeViewCancelEventHandler eventHandler in eventHandlers)
                    if (eventHandler.Target.typeFullName().contains(typeof(T).FullName))
                        treeView.BeforeExpand -= eventHandler;

            treeView.beforeExpand<IEnumerable<T>>((collection) =>
            {
                var currentNode = treeView.current();
                currentNode.clear();
                foreach (var item in collection)
                {
                    if (item != null)
                    {
                        if (item is T)
                        {
                            currentNode.add_Node(item)
                                       .add_Nodes_WithPropertiesAsChildNodes<T>(item)
                                       .add_Nodes_WithFieldsAsChildNodes<T>(item);
                        }
                        else
                            currentNode.add_Node("value" + item.str(), item);
                    }
                }
            });

            treeView.beforeExpand<T>((value) =>
            {
                var currentNode = treeView.current();
                currentNode.clear();
                currentNode.add_Nodes_WithPropertiesAsChildNodes<T>(value)
                           .add_Nodes_WithFieldsAsChildNodes<T>(value);
            });
            "{0} BeforeExpand events".format(treeView.getEventHandlers("BeforeExpand").size()).debug();
            return treeView;
        }

        public static TreeView  add_Nodes_WithPropertiesAsChildNodes<T>(this TreeView treeView, object data)
        {
            treeView.rootNode().add_Nodes_WithPropertiesAsChildNodes<T>(data);
            return treeView;
        }
        public static TreeNode  add_Nodes_WithPropertiesAsChildNodes<T>(this TreeNode treeNode, object data)
        {
            return treeNode.add_Nodes_WithPropertiesAsChildNodes<T>(data, true);
        }
        public static TreeNode  add_Nodes_WithPropertiesAsChildNodes<T>(this TreeNode treeNode, object data, bool hideWhenNoDataIsAvailable)
        {
            if (data is ICollection)
            {
                foreach (var item in (data as ICollection))
                    treeNode.add_Nodes_WithPropertiesAsChildNodes<T>(item, hideWhenNoDataIsAvailable);
                return treeNode;
            }

            if (treeNode.Tag != data)							// protection agaist recusively adding the same node
                treeNode = treeNode.add_Node(data.typeName(), data);

            foreach (var prop in data.type().properties())
            {
                bool addNode = true;
                var name = prop.Name;
                var tag = data.prop(name);
                var hasChildNodes = false;
                if ((tag is string || tag is Int32).isFalse() &&
                    (tag is T || tag is ICollection))
                {
                    if (tag is T)
                        hasChildNodes = (tag != null && tag.type().properties().size() > 0);
                    else if (tag is ICollection)
                        hasChildNodes = (tag as ICollection).Count > 0;

                    if (hideWhenNoDataIsAvailable && hasChildNodes == false)
                        addNode = false;
                }
                else
                {
                    name = "{0}: {1}".format(name, tag ?? "[null]");
                    if (hideWhenNoDataIsAvailable && tag == null)
                        addNode = false;
                }
                if (addNode)
                    treeNode.add_Node(name, tag, hasChildNodes);
            }
            return treeNode;
        }
        
        public static TreeNode  add_Nodes_WithFieldsAsChildNodes(this TreeNode treeNode, object data)
        {
            return treeNode.add_Nodes_WithFieldsAsChildNodes<System.Object>(data);
        }
        public static TreeNode  add_Nodes_WithFieldsAsChildNodes<T>(this TreeNode treeNode, object data)
        {
            return treeNode.add_Nodes_WithFieldsAsChildNodes<T>(data, true);
        }
        public static TreeNode  add_Nodes_WithFieldsAsChildNodes<T>(this TreeNode treeNode, object data, bool hideWhenNoDataIsAvailable)
        {
            //if (true)
            //	return treeNode;
            if (data is ICollection)
            {
                foreach (var item in (data as ICollection))
                    treeNode.add_Nodes_WithFieldsAsChildNodes<T>(item, hideWhenNoDataIsAvailable);
                return treeNode;
            }

            if (treeNode.Tag != data)							// protection agaist recusively adding the same node
                treeNode = treeNode.add_Node(data.typeName(), data);

            foreach (var field in data.type().fieldInfos())
            {
                if (field.Name.contains("k__BackingField").isFalse())
                {
                    bool addNode = true;
                    var name = field.Name;
                    var tag = data.field(name);
                    var hasChildNodes = false;
                    if (tag is String || tag is Int32)
                    {
                        name = "{0}: {1}".format(name, tag ?? "[null]");
                    }
                    else if (tag is T || tag is ICollection)
                    {
                        if (tag is T)
                            hasChildNodes = (tag != null && tag.type().fieldInfos().size() > 0);// || tag.type().properties().size() > 0));
                        else if (tag is ICollection)
                            hasChildNodes = (tag as ICollection).Count > 0;
                        if (hideWhenNoDataIsAvailable && hasChildNodes == false)
                            addNode = false;
                    }
                    else
                    {
                        name = "{0}: {1}".format(name, tag ?? "[null]");
                        if (hideWhenNoDataIsAvailable && tag == null)
                            addNode = false;
                    }
                    /*"name:{0} has ChildNodes:{1} tag type: :    {2}  ".format(
                            field.Name, 
                            hasChildNodes,
                            typeof(T).FullName).info();*/
                    if (addNode)
                        treeNode.add_Node(name, tag, hasChildNodes);
                }
            }
            return treeNode;
        }
        public static TreeView  add_TreeViewWithFilter(this Control control, List<string> itemsToShow)
        {
            return control.add_TreeViewWithFilter("", itemsToShow);
        }
        public static TreeView  add_TreeViewWithFilter(this Control control, string baseFolder, List<string> itemsToShow)
        {
            var treeView = control.add_TreeView();

            Action<string> populateTreeView = 
                (filter)=>{
                            var skipRegexFilter = filter.valid().isFalse();
                            treeView.clear();
                            foreach (var item in itemsToShow)
                                if (skipRegexFilter || item.regEx(filter))
                                {
                                    var nodeText = baseFolder.valid() ? item.remove(baseFolder) : item;
                                    treeView.add_Node(nodeText, item);
                                }
							treeView.applyPatchFor_1NodeMissingNodeBug();
                          };

            treeView.insert_Above<TextBox>(25).onEnter(populateTreeView);

            populateTreeView("");
            return treeView;
        }
        public static TreeView  add_TreeViewWithFilter(this Control control, Dictionary<string, List<string>> itemsToShow)
        {
            var treeView = control.add_TreeView();

            treeView.insert_Above<TextBox>(25).onEnter(
                (text) =>
                {
                    var skipRegexFilter = text.valid().isFalse();
                    treeView.clear();
                    foreach (var item in itemsToShow)
                        if (skipRegexFilter || item.Key.regEx(text))
                            treeView.add_Node(item.Key, item.Value, item.Value.size() > 0);
					treeView.applyPatchFor_1NodeMissingNodeBug();
                });

            foreach (var item in itemsToShow)
                treeView.add_Node(item.Key, item.Value, item.Value.size() > 0);

            treeView.beforeExpand<List<string>>(
                (treeNode, items) =>
                {
                    foreach (var item in items)
                    {
                        if (itemsToShow.hasKey(item))
                        {
                            var childItems = itemsToShow[item];
                            treeNode.add_Node(item, childItems, childItems.size() > 0);
                        }
                        else
                            treeNode.add_Node(item);
                    }
                });

            return treeView;
        }

        //Text
        public static string        get_Text(this TreeView treeView)
        {
            return (string)treeView.invokeOnThread(() => { return treeView.Text; });
        }        
        public static string        get_Text(this TreeNode treeNode)
        {
            return (string)treeNode.treeView().invokeOnThread(() => { return treeNode.Text; });
        }
        public static List<string>  texts(this List<TreeNode> treeNodes)
		{
			return (from treeNode in treeNodes
					select treeNode.get_Text()).toList();
		}        
        public static TreeNode      set_Text(this TreeNode treeNode, string text)
        {
            return (TreeNode)treeNode.treeView().invokeOnThread(
                                        () =>
                                        {
                                            treeNode.Text = text;
                                            return treeNode;
                                        });
        }

        //Tag
        public static object        get_Tag(this TreeView treeView)
        {
            return (object)treeView.invokeOnThread(() => { return treeView.Tag; });
        }
        public static object        get_Tag(this TreeNode treeNode)
        {
            return (object)treeNode.treeView().invokeOnThread(() => { return treeNode.Tag; });
        }
        public static T             tag<T>(this TreeNode treeNode)
        {
            if (treeNode.Tag != null && treeNode.Tag is T)
                return (T)treeNode.Tag;
            return default(T);
        }
        public static List<T>       tags<T>(this TreeView treeView)
		{
			return treeView.nodes().tags<T>();
		}		
		public static List<T>       tags<T>(this List<TreeNode> treeNodes)
		{
			return (from treeNode in treeNodes
					where WinForms_ExtensionMethods_TreeView.get_Tag(treeNode) is T
					select (T)WinForms_ExtensionMethods_TreeView.get_Tag(treeNode)).toList();
		}

        public static TreeNode      set_Tag(this TreeNode treeNode, object value)
		{
			return (TreeNode)treeNode.treeView().invokeOnThread(
				()=>{
						treeNode.Tag = value;
						return treeNode;
					});
		}
        

        public static TreeView      showToolTip(this TreeView treeView)
        {
            if (treeView != null)
                treeView.invokeOnThread(() => treeView.ShowNodeToolTips = true);
            return treeView;
        }
        public static TreeNode      toolTip(this TreeNode treeNode, string toolTipText)
        {
            if (treeNode != null)
                treeNode.treeView().invokeOnThread(() => treeNode.ToolTipText = toolTipText);
            return treeNode;
        }
        public static TreeNode      font(this TreeNode treeNode, Font font)
    	{
			treeNode.treeView().invokeOnThread(()=> treeNode.NodeFont = font);
			return treeNode;
    	}
        public static TreeView      sort(this TreeView treeView, bool value)
        {
            return (TreeView)treeView.invokeOnThread(
                    ()=>{
                            treeView.Sorted = value;
                            return treeView;
                        });
        }
        public static int           index(this TreeNode treeNode)
        {
            return (int)treeNode.treeView().invokeOnThread(() => { return treeNode.Index; });
        }

        public static TreeView  add_TreeViewWithFilter<T>(this Control control, Dictionary<string, T> itemsToShow)
        {
            return control.add_TreeViewWithFilter(itemsToShow, false);
        }
        public static TreeView  add_TreeViewWithFilter<T>(this Control control, Dictionary<string, T> itemsToShow, bool addDummyNode)
        {
            var treeView = control.add_TreeView();

            var textBoxKey = treeView.insert_Above<TextBox>(25).onEnter(
                (text) =>
                {
                    var skipRegexFilter = text.valid().isFalse();
                    treeView.beginUpdate()
							.clear();
                    foreach (var item in itemsToShow)
                        if (skipRegexFilter || item.Key.regEx(text))
                            treeView.add_Node(item.Key, item.Value, addDummyNode);
					treeView.endUpdate()
							.applyPatchFor_1NodeMissingNodeBug();
                });

            var textBoxValue = textBoxKey.insert_Right<TextBox>(textBoxKey.width() / 2).onEnter(
                (text) =>
                {
                    var skipRegexFilter = text.valid().isFalse();
                    treeView.clear();
                    foreach (var item in itemsToShow)
                        if (skipRegexFilter || item.Value != null && item.Value.str().regEx(text))
                            treeView.add_Node(item.Key, item.Value, addDummyNode);
					treeView.applyPatchFor_1NodeMissingNodeBug();
                });

            foreach (var item in itemsToShow)
                treeView.add_Node(item.Key, item.Value, addDummyNode);
            return treeView;
        }
        public static TreeView  add_TreeView_with_PropertyGrid<T>(this T control) where T : Control 
		{
			return control.add_TreeView_with_PropertyGrid(true);
		}		
		public static TreeView  add_TreeView_with_PropertyGrid<T>(this T control, bool insertBelow)	 where T : Control 
		{			
			var treeView = control.clear().add_TreeView();				
			var targetPanel = (insertBelow) ? treeView.insert_Below() : treeView.insert_Right();
			var propertyGrid = targetPanel.add_PropertyGrid().helpVisible(false);	 	
			treeView.showSelected(propertyGrid);;
			return treeView;
		}   
                
		//show selected
		public static TreeView  showSelected(this TreeView treeView, PropertyGrid propertyGrid)			
		{
			return treeView.showSelected<object>(propertyGrid);
		}		
		public static TreeView  showSelected<T>(this TreeView treeView, PropertyGrid propertyGrid)			
		{
			return treeView.afterSelect<T>((item)=> propertyGrid.show(item));			
		}		
		public static TreeView  hideSelection(this TreeView treeView)
		{
			return treeView.showSelection(false);
		}
		public static TreeView  showSelection(this TreeView treeView, bool value)
		{
			return (TreeView)treeView.invokeOnThread(
									()=>{
											treeView.HideSelection = value.isFalse();
											return treeView;
										});
		}

        //expand
        public static TreeView collapse(this TreeView treeView)
        {
            if (treeView.notNull())
                treeView.invokeOnThread(() => treeView.CollapseAll());
            return treeView;
        }
        public static TreeNode collapse(this TreeNode treeNode)
        {
            return (TreeNode)treeNode.treeView().invokeOnThread(
                () =>
                {
                    treeNode.Collapse();
                    return treeNode;
                });
        }
        public static TreeNode expandAndCollapse(this TreeNode treeNode)
        {
            return treeNode.expand().collapse();
        }

        //CheckBoxes
        public static TreeView       checkBoxes(this TreeView treeView)
		{
			return treeView.checkBoxes(true);
		}
        public static TreeView       checkBoxes(this TreeView treeView, bool enable)
        {
            return treeView.invokeOnThread(() => { treeView.CheckBoxes = enable; return treeView; });
        }
        public static TreeNode       checkBox(this TreeNode treeNode, bool value)
        {
            return treeNode.treeView().invokeOnThread(() => { treeNode.Checked = value; return treeNode; });
        }
        public static List<T>        checkBoxes_SelectedNodes<T>(this TreeView treeView)
        {
            return treeView.checkBoxes_SelectedNodes()
                           .Where((node) => node.Tag is T)
                           .Select((node) => (T)node.Tag).toList();
        }
        public static List<TreeNode> checkBoxes_SelectedNodes(this TreeView treeView)
        {
            return treeView.nodes().where((node) => node.Checked);
        }
        public static TreeView       checkBoxes_SelectAll(this TreeView treeView)
        {
            foreach (var node in treeView.nodes(true))
                node.checkBox(true);
            return treeView;
        }
        public static TreeView       checkBoxes_DeSelectAll(this TreeView treeView)
        {
            foreach (var node in treeView.nodes(true))
                node.checkBox(false);
            return treeView;
        }
        public static TreeNode      @checked(this TreeNode treeNode)
		{
			return treeNode.@checked(true);
		}
		public static TreeNode      @checked(this TreeNode treeNode, bool value)
    	{
    		treeNode.treeView().invokeOnThread(()=> treeNode.Checked = value);
    		return treeNode;
    	}

    }

    public static class WinForms_ExtensionMethods_TreeView_Events
    { 
        public static TreeView  beforeExpand<T>(this TreeView treeView, Action<T> callback)
        {
            treeView.BeforeExpand += (sender, e)
                                    =>
            {
                if (e.Node != null && e.Node.Tag != null && e.Node.Tag is T)
                {
                    treeView.SelectedNode = e.Node;
                    callback((T)e.Node.Tag);
                }
            };
            return treeView;
        }
        public static TreeView  beforeExpand<T>(this TreeView treeView, Action<TreeNode, T> callback)
        {
            treeView.beforeExpand<T>(
                (tagData) =>
                {
                    var selectedNode = treeView.selected();
                    selectedNode.clear();
                    callback(selectedNode, tagData);
                });
            return treeView;
        }
        public static TreeView  beforeExpand_PopulateWithList<T>(this TreeView treeView)
        {
            treeView.beforeExpand<List<T>>(
                (treeNode, items) => treeNode.add_Nodes(items));
            return treeView;
        }

        public static TreeView  afterSelect<T>(this TreeView treeView, Action<T> callback)            
        {
            treeView.AfterSelect += (sender, e)
                                    =>
                                        {
                                            if (e.Node != null)
                                            {
                                                if (e.Node.Tag is T)
                                                    callback((T)e.Node.Tag);
                                                else
                                                    if (e.Node is T)
                                                        callback((T)(object)e.Node);    // it is weird that I have to first cast to object and then to T
                                            }
                                        };
            return treeView;
        }
        public static TreeView  afterSelect(this TreeView treeView, Action<TreeNode> callback)
        {
            treeView.AfterSelect += (sender, e) => callback(treeView.current());
            return treeView;
        }

        public static TreeView  onDoubleClk<T>(this TreeView treeView, Action<T> callback)
		{
            return treeView.onDoubleClick<T>(callback);
		}
        public static TreeView  onDoubleClick<T>(this TreeView treeView, Action<T> callback)
        {
            treeView.invokeOnThread(
				()=>{
						treeView.DoubleClick+= 
							(sender,e)=>{															
											object tag = WinForms_ExtensionMethods_TreeView.get_Tag(treeView.selected());
											if (tag is T)
												O2Thread.mtaThread(()=> callback((T)tag));
										 };
					});
			return treeView;		

            /*treeView.NodeMouseDoubleClick +=
                (sender, e) =>
                {
                    var tag = (T)treeView.current().tag<T>();
                    if (tag != null && callback != null)
                        callback(tag);
                };
            return treeView;*/
        }
        public static TreeView  onDoubleClick(this TreeView treeView, Action  callback)
		{
			return treeView.onDoubleClick<object>((tag)=>callback());
		}
		public static TreeView  onDoubleClick(this TreeView treeView, Action<object> callback)
		{
			return treeView.onDoubleClick<object>((tag)=>callback(tag));
		}

        public static TreeView  onDrag<T>(this TreeView treeView)
        {
            return treeView.onDrag<T, object>(null);         
        }
        public static TreeView  onDrag<T, T1>(this TreeView treeView, Func<T, T1> getDragData)
        {
            treeView.ItemDrag += (sender, e) =>
            {
                ((TreeNode)e.Item).selected();
                var tag = (T)treeView.current().tag<T>();
                if (tag != null)
                    if (getDragData != null)
                        treeView.DoDragDrop(getDragData(tag), DragDropEffects.Copy);
                    else
                        treeView.DoDragDrop(tag, DragDropEffects.Copy);
            };
            return treeView;
        }

        public static TreeView onDrop<T>(this TreeView treeView, Action<T, TreeNode> onDrop)
        {
            treeView.invokeOnThread(() =>
            {
                treeView.AllowDrop = true;
                treeView.DragEnter += (sender, e) => Dnd.setEffect(e);
                treeView.DragDrop += (sender, e) =>
                {
                    O2Thread.mtaThread(
                        () =>
                        {
                            "on Drop".info();
                            var data = Dnd.tryToGetObjectFromDroppedObject(e, typeof(T));
                            if (data.notNull())
                            {
                                var treeNode = O2Forms.getTreeNodeAtDroppedOverPoint(treeView, e.X, e.Y);
                                "{0} - {1} : {2}".info(sender.typeName(), e.typeName(), treeNode);
                                onDrop((T)data, treeNode);
                            }
                            else
                            {
                                var _object = Dnd.tryToGetObjectFromDroppedObject(e);
                                "{0}".info(_object.serialize(false));
                                "got _object: {0} : {1}".error(_object, _object.typeName());

                            }
                        });
                };
            });
            return treeView;
        }


        public static Delegate[]    getEventHandlers(this TreeView treeView, string eventName)
        {
            var eventDelegate = (MulticastDelegate)treeView.field(eventName);
            if (eventDelegate != null)
                return eventDelegate.GetInvocationList();
            if (eventName.starts("on").isFalse())
                return treeView.getEventHandlers("on" + eventName);
            return null;
        }        
        public static bool          hasEventHandler(this TreeView treeView, string eventName)
        {
            var eventDelegate = treeView.getEventHandlers(eventName);
            return (eventDelegate != null && eventDelegate.size() > 0);
        }
        public static TreeView      removeEventHandlers_BeforeExpand(this TreeView treeView)
        {
            return treeView.removeEventHandlers("BeforeExpand");
        }        
        public static TreeView      removeEventHandlers(this TreeView treeView, string eventHandlerToRemove)
        {
            if (treeView == null)
                return null;
            var eventHandlers = treeView.getEventHandlers(eventHandlerToRemove);            
            return treeView.removeEventHandlers(eventHandlerToRemove, eventHandlers.toList());                        
        }       
        public static TreeView      removeEventHandlers<T>(this TreeView treeView, string eventHandlerToRemove)
        {
            var eventHandlers = treeView.getEventHandlers(eventHandlerToRemove);
            if (eventHandlers != null)
            {
                var eventsToRemove = new List<Delegate>();
                foreach (MulticastDelegate eventHandler in eventHandlers)
                    if (eventHandler.Target.typeFullName().contains(typeof(T).FullName))
                        eventsToRemove.add(eventHandler);
                treeView.removeEventHandlers(eventHandlerToRemove,eventsToRemove);
                //treeView.BeforeExpand -= eventHandler;                    
            }
            eventHandlers = treeView.getEventHandlers(eventHandlerToRemove);
            "After remove, there are {0} '{1}' events mapped".format(
                (eventHandlers != null) ? eventHandlers.Length : 0,
                eventHandlerToRemove).debug();
            return treeView;
        }
        private static TreeView     removeEventHandlers(this TreeView treeView, string eventHandlerToRemove, List<Delegate> eventsToRemove)
        {
            if (eventsToRemove != null)
                foreach (var eventHandler in eventsToRemove)
                {
                    var eventDelegate = (MulticastDelegate)treeView.field(eventHandlerToRemove);
                    if (eventDelegate == null)
                        eventDelegate = (MulticastDelegate)treeView.field("on" + eventHandlerToRemove);
                    if (eventDelegate == null)
                        "in removeEventHandlers could not find MulticastDelegate for: {0}".format(eventHandlerToRemove).error();
                    else
                    {
                        var newList = Delegate.Remove(eventDelegate, eventHandler);
                        treeView.setEventHandlers(eventHandlerToRemove, newList);
                        //   var eventDelegate = (MulticastDelegate)treeView.field(eventName);
                        //   eventDelegate.invoke("RemoveImpl", eventHandler);
                        //var currentEvents = treeView.getEventHandlers(eventHandlerToRemove);
                        //treeView.BeforeExpand -= eventHandler;                    
                    }
                }
            return treeView;
        }
        public static TreeView      setEventHandlers(this TreeView treeView, string eventName, Delegate newValue)
        {
            var fieldInfo = PublicDI.reflection.getField(typeof(TreeView), eventName);
            if (fieldInfo == null)
                fieldInfo = PublicDI.reflection.getField(typeof(TreeView), "on" + eventName);
            if (fieldInfo != null)
            {
                PublicDI.reflection.setField(fieldInfo, treeView, newValue);
            }
            else
                "could not find event name: {0}".format(eventName).error();
            return treeView;
        }
    }

    public static class WinForms_ExtensionMethods_TreeView_Edit
    { 
        public static TreeView  allow_TreeNode_Edits(this TreeView treeView)
		{
			if (treeView.notNull())
				treeView.invokeOnThread(()=> treeView.LabelEdit = true);		
			return treeView;
		}		
		public static TreeNode  beginEdit(this TreeNode treeNode)
		{
			if (treeNode.notNull())
				treeNode.treeView().invokeOnThread(()=> treeNode.BeginEdit());
			return treeNode;
		}				
    }

    public static class WinForms_ExtensionMethods_TreeView_Colors
    {
        public static TreeNode  foreColor(this TreeNode treeNode, string colorName)
    	{
    		return treeNode.foreColor(colorName.color());    			
    	}    	
        public static TreeNode  foreColor(this TreeNode treeNode, Color color)
        {
            if (treeNode != null)
                treeNode.treeView().invokeOnThread(() => treeNode.ForeColor = color);
            return treeNode;
        }
        public static TreeNode  backColor(this TreeNode treeNode, string colorName)
    	{
    		return treeNode.backColor(colorName.color());    			
    	}    	
        public static TreeNode  backColor(this TreeNode treeNode, Color color)
        {
            if (treeNode != null)
                treeNode.treeView().invokeOnThread(() => treeNode.BackColor = color);
            return treeNode;
        }
        public static TreeNode  color(this TreeNode treeNode, Color color)
        {            
            return treeNode.setTextColor(color);
        }
        public static TreeNode  color(this TreeNode treeNode, string colorName)
    	{
    		return treeNode.foreColor(colorName);
    	}
        public static TreeNode  setTextColor(this TreeNode treeNode, Color color)
        {
            if (treeNode != null)
                treeNode.treeView().invokeOnThread(() => { treeNode.ForeColor = color; });
            return treeNode;
        }
    	public static TreeNode  textColor(this TreeNode treeNode, string colorName)
    	{
    		return treeNode.foreColor(colorName);
    	}
    	
    }


    public static class WinForms_ExtensionMethods_TreeView_ImageList
    {
        public static ImageList imageList(this TreeView treeView)
        {
            return treeView.invokeOnThread(
                () =>
                    {
                        if (treeView.ImageList.isNull())
                            treeView.ImageList = new ImageList();
                        return treeView.ImageList;
                    });
        }
        public static TreeView  add_ToImageList(this TreeView treeView, params string[] keys)
		{
			foreach(var key in keys)
				treeView.add_ToImageList(key, key.formImage());
			return treeView;
		}
		public static TreeView  add_ToImageList(this TreeView treeView, string key, Image image)
		{
			return treeView.invokeOnThread(
				()=>{
						if (key.notNull() && image.notNull()) 
							treeView.imageList().Images.Add(key, image);
						return treeView;
					});
		}
        public static TreeView  imageIndex(this TreeView treeView, int index)
		{
			treeView.invokeOnThread(()=> treeView.ImageIndex = index);
			return treeView;
		}
        public static TreeNode  image(this TreeNode treeNode, string key)
    	{
    		return treeNode.treeView().invokeOnThread(
    			()=>{
    					treeNode.ImageKey = key;
    					return treeNode;
    				});
    	}
    	public static TreeNode  image(this TreeNode treeNode, int index)
    	{
    		return treeNode.treeView().invokeOnThread(
    			()=>{
    					treeNode.ImageIndex = index; 
    					treeNode.SelectedImageIndex = index; 
    					return treeNode;
    				});
    	}
    }

    public static class WinForms_ExtensionMethods_TreeView_Hacks
    { 

        public static TreeView applyPatchFor_1NodeMissingNodeBug(this TreeView treeView)
		{
			if (treeView.nodes().size() == 1)
			{
				var firstNode = treeView.nodes()[0];
				firstNode.set_Text(firstNode.get_Text() + "");
			}
			return treeView;
		}

        // this is one of O2's weirdest bugs in the .NET Framework, but there are cases where 
		// a treeview only has 1 node and it is not shown
		public static TreeView applyPathFor_1NodeMissingNodeBug(this TreeView treeView)
		{
            return treeView.applyPatchFor_1NodeMissingNodeBug();

			/*if (treeView.nodes().size()==1)
			{
				var firstNode = treeView.nodes()[0];
				firstNode.set_Text(firstNode.get_Text() + "");	
			}
			return treeView;**/
		}
    }
}
