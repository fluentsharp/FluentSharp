// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;

using ICSharpCode.NRefactory.Ast;
using O2.External.SharpDevelop.Ascx;
using O2.External.SharpDevelop.ExtensionMethods;

// This code contains a bunch of code snippets from the SharpDevelop_3.1.1.5327_Source\samples\NRefactoryDemo sample app

namespace O2.External.SharpDevelop.AST
{
    public static class AstTreeView
    {
        public static void show_List(this TreeView treeView, IEnumerable list, string textParameterName)
        {
            treeView.invokeOnThread(
                () =>
                    {
                        treeView.clear();
                        foreach (var item in list)
                        {
                            var parameterValue = PublicDI.reflection.getProperty(textParameterName, item);
                            var nodeText = (parameterValue != null)
                                               ? parameterValue.ToString()
                                               : "[null value for '" + textParameterName +
                                                 "' parameter in object of type: " + item.ToString();
                            treeView.add_Node(nodeText, item);
                        }
                        treeView.Sort();
                    });
        }
        
        public static void show_List(this TreeView treeView, List<string> list)
        {
            treeView.invokeOnThread(
                () =>
                    {
                        treeView.clear();
                        foreach (var item in list)
                            treeView.add_Node(item);
                        treeView.Sort();
                    });
        }
	
        public static void show_Ast(this TreeView treeView, string sourceCode)
        {
            treeView.invokeOnThread(
                () => treeView.show_SourceCode_Ast_InTreeView(sourceCode));
        }
		
        public static void show_Ast(this TreeView treeView, Ast_CSharp ast)
        {
            treeView.invokeOnThread(
                () => treeView.show_SourceCode_Ast_InTreeView(ast.CompilationUnit));
        }

        public static TreeView show_Asts(this TreeView treeView, Dictionary<string, List<INode>> nodesToShow)
        {
            foreach (var item in nodesToShow)
            {
                var treeNode = treeView.add_Node(item.Key);
                foreach (var iNode in item.Value)
                    treeNode.show_Ast(iNode);
            }
            return treeView;
        }

        public static TreeNode show_Asts<T>(this TreeNode treeNode, List<T> astNodes)
            where T : INode
        {
            foreach (var astNode in astNodes)
                treeNode.show_Ast(astNode);
            return treeNode;
        }

        public static TreeView show_Ast(this TreeView treeView, INode iNode)
        {
            treeView.clear();
            treeView.rootNode().show_Ast(iNode);
            return treeView;
        }

        public static TreeNode show_Ast<T>(this TreeNode treeNode, T astNode)
            where T : INode
        {
            treeNode.TreeView.invokeOnThread(() =>
            {
                treeNode.Nodes.Add(new AstTreeView.ElementNode(astNode));
            });
            return treeNode;
        }
        
        public static void show_SourceCode_Ast_InTreeView(this TreeView treeView, string sourceCodeFile)
        {
            treeView.invokeOnThread(
                () =>
                    {
                        var ast = new Ast_CSharp(sourceCodeFile);
                        treeView.show_SourceCode_Ast_InTreeView(ast.CompilationUnit);
                    });
        }				
		
        public static void show_SourceCode_Ast_InTreeView(this TreeView treeView, CompilationUnit compilationUnit)
        {
            treeView.invokeOnThread(
                () =>
                    {
                        treeView.clear();
                        treeView.add_Node(new CollectionNode("CompilationUnit", compilationUnit.Children));
                    });
        }
	    
        static TreeNode CreateNode(object child)
        {
            if (child == null) {
                return new TreeNode("*null reference*");
            } else if (child is INode) {
                return new ElementNode(child as INode);
            } else {
                return new TreeNode(child.ToString());
            }
        }

        public static TreeView afterSelect_ShowAstInSourceCodeEditor(this TreeView treeView, ascx_SourceCodeEditor codeEditor)
        {
            return (TreeView)codeEditor.invokeOnThread(() =>
            {
                treeView.afterSelect<AstTreeView.ElementNode>((node) =>
                {
                    var element = (INode)node.field("element");
                    codeEditor.setSelectionText(element.StartLocation, element.EndLocation);
                });
                treeView.afterSelect<INode>((node) =>
                {
                    codeEditor.setSelectionText(node.StartLocation, node.EndLocation);
                });
                return treeView;
            });
        }

        public class CollectionNode : TreeNode
        {
            internal IList collection;
            string baseName;
			
            public CollectionNode(string text, IList children) : base(text)
            {
                this.baseName = text;
                this.collection = children;
                Update();
            }
			
            public void Update()
            {
                if (collection.Count == 0) {
                    Text = baseName + " (empty collection)";
                } else if (collection.Count == 1) {
                    Text = baseName + " (collection with 1 element)";
                } else {
                    Text = baseName + " (collection with " + collection.Count + " elements)";
                }
                Nodes.Clear();
                foreach (object child in collection) {
                    Nodes.Add(CreateNode(child));
                }
                Expand();
            }
        }
		
        public class ElementNode : TreeNode
        {
            public INode element;
			
            public ElementNode(INode node)
            {
                this.element = node;
                Update();
            }
			
            public void Update()
            {
                Nodes.Clear();
                Type type = element.GetType();
                Text = type.Name;
                if (Tag != null) { // HACK: after editing property element
                    Text = Tag.ToString() + " = " + Text;
                }
                if (!(element is INullable && (element as INullable).IsNull)) {
                    AddProperties(type, element);
                    if (element.Children.Count > 0) {
                        Nodes.Add(new CollectionNode("Children", element.Children));
                    }
                }
            }
			
            void AddProperties(Type type, INode node)
            {
                if (type == typeof(AbstractNode))
                    return;
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                    if (pi.DeclaringType != type) // don't add derived properties
                        continue;
                    if (pi.Name == "IsNull")
                        continue;
                    object value = pi.GetValue(node, null);
                    if (value != node)          // to prevent stack overflow (since we would be adding the current value recursively (happened with CompilationUnit.CurrentBock)
                        if (value is IList)
                        {
                            Nodes.Add(new CollectionNode(pi.Name, (IList)value));
                        }
                      //  else if (value is string)
                        //{
                         //   Text += " " + pi.Name + "='" + value + "'";
                            //Nodes.Add(pi.Name + "='" + value 
                        //}
                        else
                        {
                            TreeNode treeNode = CreateNode(value);
                            treeNode.Text = pi.Name + " = " + treeNode.Text;
                            treeNode.Tag = pi.Name;
                            Nodes.Add(treeNode);
                        }
                    else
                    {
                        PublicDI.log.info("skipping recursive property add for: {0}",pi.Name);
                    }
                }
                AddProperties(type.BaseType, node);
            }
        }
    }
}