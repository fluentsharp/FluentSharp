using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{    
    public static class XTypedElement_ExtensionMethods_TreeView
    {
        public static TreeNode add_Node(this TreeView treeView, XElement xElement)
        {
            return treeView.add_Node(xElement.name(), xElement, xElement.hasDataForChildTreeNodes());
        }

        public static TreeNode add_Node(this TreeNode treeNode, XElement xElement)
        {
            return treeNode.add_Node(xElement.name(), xElement, xElement.hasDataForChildTreeNodes());
        }

        public static TreeNode add_Node(this TreeNode treeNode, XAttribute xAttribute)
        {
            return treeNode.add_Node("{0}: {1}".format(xAttribute.Name, xAttribute.Value));
        }

        public static string getNormalizedValue(this XElement xElement)
        {
            var value = xElement.Value;
            if (value.valid())
                xElement.Nodes().toList().forEach<XElement>(
                    (element) => value = value.replace(element.Value, ""));
            return value.trim();
        }

        public static TreeView autoExpandXElementData(this TreeView treeView)
        {
            //var onBeforeExpand = "onBeforeExpand"
            if (treeView.hasEventHandler("BeforeExpand"))  	// don't add if there is already an onBeforeExpand event already mapped        	        		
                return treeView;
            treeView.beforeExpand<XElement>(
                (xElement) =>
                {
                    treeView.current().clear();
                    xElement.Nodes().toList().forEach<XElement>(
                            (element) => treeView.current().add_Node(element));

                    xElement.Attributes().toList().forEach<XAttribute>(
                            (attribute) => treeView.current().add_Node(attribute));

                    var value = xElement.getNormalizedValue();
                    if (value.valid())
                        treeView.current().add_Node("value: {0}".format(value));
                });
            return treeView;
        }

        public static TreeView xmlShow(this TreeView treeView, string xml)
        {
            return treeView.showXml(xml);
        }

        public static TreeView showXml(this TreeView treeView, object dataToLoad)
        {
            try
            {
                XElement xElement = null;
                if (dataToLoad is string)
                    xElement = ((string)dataToLoad).xRoot();
                else if (dataToLoad.typeName()== "XTypedElement")
                    xElement = (XElement)dataToLoad.prop("Untyped");

                if (xElement != null)
                {
                    treeView.clear();
                    treeView.autoExpandXElementData();
                    treeView.add_Node(xElement);
                    treeView.expand();
                }
            }
            catch (Exception ex)
            {
                ex.log(ex.Message);
            }
            return treeView;
        }

        public static TreeView showXml(this TreeView treeView, XElement xElement)
        {
            treeView.clear();
            treeView.autoExpandXElementData();
            treeView.add_Node(xElement);
            treeView.expand();
            return treeView;
        }

        public static TreeNode showXml(this TreeNode treeNode, List<XElement> xElements)
        {
            foreach (var xElement in xElements)
                treeNode.showXml(xElement);
            return treeNode;
        }

        public static TreeNode showXml(this TreeNode treeNode, XElement xElement)
        {
            if (treeNode.TreeView != null)
                treeNode.TreeView.autoExpandXElementData();
            treeNode.add_Node(xElement);
            treeNode.expand();
            return treeNode;
        }

        public static TreeNode showXml(this TreeNode treeNode, object dataToLoad)
        {
            try
            {
                XElement xElement = null;
                if (dataToLoad is string)
                    xElement = ((string)dataToLoad).xRoot();
                else if (dataToLoad.typeName() == "XTypedElement")
                    xElement = (XElement)dataToLoad.prop("Untyped");
                
                if (xElement != null)
                {
                    if (treeNode.TreeView != null)
                        treeNode.TreeView.autoExpandXElementData();
                    treeNode.add_Node(xElement);
                    treeNode.expand();
                }
            }
            catch (Exception ex)
            {
                ex.log(ex.Message);
            }
            return treeNode;
        }

    }
}
