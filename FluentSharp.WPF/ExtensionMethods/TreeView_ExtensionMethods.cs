using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FluentSharp.CoreLib;

namespace FluentSharp.WPF
{
    public static class TreeView_ExtensionMethods
    {
        //#region TreeView

        public static TreeView add_WPF_TreeView(this System.Windows.Forms.Control control)
        {
            return control.add_WPF_Control<TreeView>();
        }

        public static TreeViewItem treeViewItem(this UIElement uiElement, object tag)
        {
            return (TreeViewItem)uiElement.wpfInvoke(
                () =>
                    {
                        return (tag != null)
                                   ? uiElement.treeViewItem(tag.str(), tag)
                                   : new TreeViewItem();
                    });
        }

        public static TreeViewItem treeViewItem(this UIElement uiElement, string itemText, object tag)
        {
            return (TreeViewItem)uiElement.wpfInvoke(
                () =>
                    {
                        if (tag is TreeViewItem) 			// to prevent recursive add
                            return (TreeViewItem)tag;
                        var treeViewItem = new TreeViewItem();
                        treeViewItem.Header = itemText;
                        treeViewItem.Tag = tag;
                        return treeViewItem;
                    });
        }

        public static List<TreeViewItem> treeViewItems<T>(this UIElement uiElement, IEnumerable<T> collection)
        {
            return (List<TreeViewItem>)uiElement.wpfInvoke(
                () =>
                    {
                        var newTreeViewItems = new List<TreeViewItem>();
                        //newTreeViewItems.Add(uiElement.treeViewItem(collection[0]));
                        foreach (var item in collection)
                            newTreeViewItems.Add(uiElement.treeViewItem(item));
                        return newTreeViewItems;
                    });
        }

        public static List<TreeViewItem> add_Nodes<T>(this TreeView treeView, IEnumerable<T> collection)
        {
            var newTreeViewItems = treeView.treeViewItems(collection);
            foreach (var newTreeViewItem in newTreeViewItems)
                treeView.add_Node(newTreeViewItem);
            return newTreeViewItems;
        }

        public static List<TreeViewItem> add_Nodes<T>(this TreeViewItem treeViewItem, IEnumerable<T> collection)
        {
            var newTreeViewItems = treeViewItem.treeViewItems(collection);
            foreach (var newTreeViewItem in newTreeViewItems)
                treeViewItem.add_Node(newTreeViewItem);
            return newTreeViewItems;
        }

        /*public static List<TreeViewItem> add<T>(this TreeView treeView, List<TreeViewItem> treeViewItems)
        {
            foreach(var treeViewItem in treeViewItems)
                treeView.add(treeViewItem);
            return treeViewItems;
        }*/

        public static TreeViewItem add_Node(this TreeView treeView, object newItem)
        {
            return (newItem != null)
                       ? treeView.add_Node(newItem.str(), newItem)
                       : null;
        }

        public static TreeViewItem add_Node(this TreeView treeView, string itemText, object tag)
        {
            return (TreeViewItem)treeView.wpfInvoke(
                () =>
                    {
                        var newItem = treeView.treeViewItem(itemText, tag);
                        treeView.Items.add_Node(newItem);
                        return newItem;
                    });
        }

        public static TreeViewItem add_Node(this TreeViewItem treeViewItem, object newItem)
        {
            return (newItem != null)
                       ? treeViewItem.add_Node(newItem.str(), newItem)
                       : treeViewItem;
        }

        public static TreeViewItem add_Node(this TreeViewItem treeViewItem, string itemText, object tag)
        {
            return (TreeViewItem)treeViewItem.wpfInvoke(
                () =>
                    {
                        var newItem = treeViewItem.treeViewItem(itemText, tag);
                        treeViewItem.Items.Add(newItem);
                        return newItem;
                    });
        }

        public static TreeViewItem add_Node(this ItemCollection items, TreeViewItem item)
        {
            items.Add(item);
            return item;
        }

        public static TreeView clear(this TreeView treeView)
        {
            return (TreeView)treeView.wpfInvoke(
                () =>
                    {
                        treeView.Items.Clear();
                        return treeView;
                    });
        }

        public static TreeViewItem clear(this TreeViewItem treeViewItem)
        {
            return (TreeViewItem)treeViewItem.wpfInvoke(
                () =>
                    {
                        treeViewItem.Items.Clear();
                        return treeViewItem;
                    });
        }

        public static TreeViewItem colapse(this TreeViewItem treeViewItem)
        {
            return (TreeViewItem)treeViewItem.wpfInvoke(
                () =>
                    {
                        treeViewItem.IsExpanded = false;
                        return treeViewItem;
                    });
        }

        public static TreeViewItem expand(this TreeViewItem treeViewItem)
        {
            return treeViewItem.expand(false);
        }
        public static TreeViewItem expand(this TreeViewItem treeViewItem, bool recursive)
        {
            return (TreeViewItem)treeViewItem.wpfInvoke(
                () =>
                    {
                        treeViewItem.IsExpanded = true;
                        if (recursive)
                            foreach (var childItem in treeViewItem.Items)
                                if (childItem is TreeViewItem)
                                    ((TreeViewItem)childItem).expand(true);
                        return treeViewItem;
                    });
        }
        public static TreeView expand(this TreeView treeView)
        {
            return treeView.expand(false);
        }
        public static TreeView expand(this TreeView treeView, bool recursive)
        {
            return (TreeView)treeView.wpfInvoke(
                () =>
                    {
                        foreach (var item in treeView.Items)
                            if (item is TreeViewItem)
                                ((TreeViewItem)item).expand(recursive);
                        return treeView;
                    });
        }
        public static TreeView fontSize(this TreeView treeView, double size)
        {
            return treeView.set_Value("FontSize", size);
        }
        // generic setter for treeView properties
        public static TreeView set_Value(this TreeView treeView, string propertyToSet, object value)
        {
            return (TreeView)treeView.wpfInvoke(
                () =>
                    {
                        treeView.prop(propertyToSet, value);
                        return treeView;
                    });
        }
        public static TreeView beforeExpand<T>(this TreeView treeView, Action<TreeViewItem, T> onBeforeExpand)
        {
            return (TreeView)treeView.wpfInvoke(
                () =>
                    {
                        treeView.AddHandler(
                            TreeViewItem.ExpandedEvent,
                            new RoutedEventHandler(
                                (sender, e) =>
                                    {
                                        if (e.Source is TreeViewItem)
                                        {
                                            var treeViewItem = (TreeViewItem)e.Source;
                                            if (treeViewItem.Tag != null && treeViewItem.Tag is T)
                                                onBeforeExpand(treeViewItem, (T)treeViewItem.Tag);
                                        }
                                    }));
                        return treeView;
                    });
        }

    }
}