using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.CodeDom;

using O2.DotNetWrappers.ExtensionMethods;
using System.Collections;
using ICSharpCode.SharpDevelop.Dom;

namespace O2.API.AST.ExtensionMethods
{
    public static class CodeDom_ExtensionMethods
    {
        public static TreeNode show_CodeDom<T>(this TreeNode treeNode, List<T> list)
            where T : CodeObject
        {
            treeNode.TreeView.configureTreeViewForCodeDomViewAndNRefactoryDom();
            treeNode.add_Nodes_WithPropertiesAsChildNodes<CodeObject>(list);
            return treeNode;
        }

        public static TreeView show_CodeDom<T>(this TreeView treeView, List<T> list)
            where T : CodeObject
        {
            treeView.configureTreeViewForCodeDomViewAndNRefactoryDom();
            treeView.add_Nodes_WithPropertiesAsChildNodes<CodeObject>(list);
            return treeView;
        }

        public static TreeView show_CodeDom<T>(this TreeView treeView, T codeObject)
            where T : CodeObject
        {
            treeView.configureTreeViewForCodeDomViewAndNRefactoryDom();
            treeView.add_Nodes_WithPropertiesAsChildNodes<T>(codeObject);
            return treeView;
        }

        /*public static TreeView configureTreeViewForCodeDomView(this TreeView treeView)
        {
            if (treeView.hasEventHandler("BeforeExpand"))
                return treeView;
            treeView.beforeExpand<IEnumerable>((collection) =>
            {
                if (collection.typeFullName().contains("System.CodeDom"))  // ensure we only handle the System.CodeDom cases
                {
                    var currentNode = treeView.current();
                    currentNode.clear();
                    foreach (var item in collection)
                    {
                        var itemNode = currentNode.add_Node(item);
                        itemNode.add_Nodes_WithPropertiesAsChildNodes<CodeObject>(item);
                    }
                }
            });

            treeView.beforeExpand<CodeObject>((codeObject) =>
            {
                var currentNode = treeView.current();
                currentNode.clear();
                currentNode.add_Nodes_WithPropertiesAsChildNodes<CodeObject>(codeObject);
            });
            return treeView;
        }*/
        public static TreeView configureTreeViewForCodeDomViewAndNRefactoryDom(this TreeView treeView)
        {
            if (treeView.hasEventHandler("BeforeExpand"))
                return treeView;
            treeView.beforeExpand<IEnumerable>((collection) =>
            {
                //"before: {0}".format(collection.typeFullName()).info();
                if (collection.typeFullName().contains("System.CodeDom") ||
                    collection.typeFullName().contains("ICSharpCode.SharpDevelop.Dom")) // ensure we only handle the System.CodeDom or ICSharpCode.SharpDevelop.Dom cases                    
                {
                    var currentNode = treeView.current();
                    currentNode.clear();
                    foreach (var item in collection)
                    {
                        var itemNode = currentNode.add_Node(item);
                        itemNode.add_Nodes_WithPropertiesAsChildNodes<CodeObject>(item);
                        itemNode.add_Nodes_WithPropertiesAsChildNodes<IFreezable>(item);
                        itemNode.add_Nodes_WithPropertiesAsChildNodes<string>(item);
                    }
                }
                if (collection.typeFullName().contains("System.String, mscorlib"))
                {
                    var currentNode = treeView.current();
                    currentNode.clear();
                    foreach (var item in collection)
                        currentNode.add_Node(item);
                }
            });

            treeView.beforeExpand<CodeObject>((codeObject) =>
            {
                var currentNode = treeView.current();
                currentNode.clear();
                currentNode.add_Nodes_WithPropertiesAsChildNodes<CodeObject>(codeObject);
            });

            treeView.beforeExpand<IFreezable>((codeObject) =>
            {
                var currentNode = treeView.current();
                currentNode.clear();
                currentNode.add_Nodes_WithPropertiesAsChildNodes<IFreezable>(codeObject);
            });
            return treeView;
        }
    }
}
