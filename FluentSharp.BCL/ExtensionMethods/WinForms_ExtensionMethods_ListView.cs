using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_ListView
    {
        public static ListView showSelection(this ListView listView)
        {
            return listView.showSelection(true);
        }
        public static ListView showSelection(this ListView listView,bool value)
        {
            return (ListView)listView.invokeOnThread(
                ()=>{
                        listView.HideSelection = value.isFalse();
                        return listView;
                });
        }
		
        public static ListViewItem add_Row(this ListView listView, params string[] items)
        {
            return listView.add_Row(items.toList());
        }
		
        public static ListViewItem add_Row(this ListView listView, List<string> items)
        {
            return (ListViewItem)listView.invokeOnThread(
                ()=>{
                        if (items.size() < 2)
                        {                            
                            return listView.Items.Add(items.first() ?? "");						                            
                        }													
                        var listViewItem = new ListViewItem();
                        listViewItem.Text = items.first();
                        items.remove(0);
                        listViewItem.SubItems.AddRange(items.ToArray());
                        listView.Items.Add(listViewItem);
                        return listViewItem;						
                });				
        }				
		
        public static ListViewItem tag(this ListViewItem listViewItem, object tag)
        {
            return (ListViewItem)listViewItem.ListView.invokeOnThread(
                ()=>{
                        listViewItem.Tag = tag;
                        return listViewItem;
                });
        }
		
        public static List<ListViewItem> items(this ListView listView)
        {
            return (List<ListViewItem>)listView.invokeOnThread(
                ()=>{				
                        var items = new List<ListViewItem>();
                        foreach(ListViewItem item in listView.Items)
                            items.add(item);
                        return items;
                });
			
        }
		
        public static List<ListViewItem> selectedItems(this ListView listView)
        {
            return (List<ListViewItem>)listView.invokeOnThread(
                ()=>{				
                        var items = new List<ListViewItem>();
                        foreach(ListViewItem item in listView.SelectedItems)
                            items.add(item);
                        return items;
                });
			
        }
		
        public static ListViewItem selected(this ListView listView)
        {
            return listView.selectedItem();
        }
        public static ListViewItem selectedItem(this ListView listView)
        {
            return listView.selectedItems().first();
        }
		
        public static ListViewItem select(this ListView listView, int position)
        {
            var items = listView.items();
            if (position < 1 || position > items.size() + 1)
            {
                "[ListViewItem] in select, invalid position value '{0}' (there are {1} items in ListView)".error(position, items.size());
                return null;
            }
            return items[position -1].select();
        }
		
        public static ListViewItem select(this ListViewItem listViewItem)
        {
            return (ListViewItem)listViewItem.ListView.invokeOnThread(
                () =>{
                         listViewItem.Selected = true;
                         return listViewItem;
                });
        }
		
        public static object tag(this ListViewItem listViewItem)
        {
            return (object)listViewItem.ListView.invokeOnThread(() => listViewItem.Tag );
        }
		
        public static object tag<T>(this ListViewItem listViewItem)
        {
            try
            {
                if (listViewItem.notNull())
                {
                    var tag = listViewItem.tag();
                    if (tag.notNull() && tag is T)
                        return (T)tag;
                }	
            }
            catch(Exception ex)
            {
                ex.log("[ListViewItem] tag");
            }
            return default(T);
        }
		
		
        public static ListView afterSelected(this ListView listView, Action<ListViewItem> onSelectedCallback)
        {
            return (ListView)listView.invokeOnThread(
                ()=>{		
                        listView.SelectedIndexChanged+=(sender,e)=>
                            {
                                try
                                {
                                    if (listView.selected() != null)
                                        onSelectedCallback(listView.selected());
                                }
                                catch(Exception ex)
                                {
                                    ex.log("[ListViewItem] afterSelected");
                                }
                            };
                        return listView;
                });	
        }
		
        public static ListView afterSelected<T>(this ListView listView, Action<T> onSelectedCallback)
        {
            return (ListView)listView.invokeOnThread(
                ()=>{		
                        listView.SelectedIndexChanged+=(sender,e)=>
                            {
                                try
                                {
                                    if (listView.selected() != null)
                                    {
                                        var tag = listView.selected().tag();
                                        if (tag.notNull() && tag is T)
                                            onSelectedCallback((T)tag);
                                    }
                                } 
                                catch(Exception ex)
                                {
                                    ex.log("[ListViewItem] afterSelected<T>");
                                }
                            };
                        return listView;
                });	
        }
    }
}