using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_ComboBox
    { 
        public static ComboBox add_ComboBox(this Control control)
        {
            return control.add_ComboBox(0, 0, null).fill();
        }
        public static ComboBox add_ComboBox(this Control control, int top, int left)
        {
            return control.add_ComboBox(top, left, null);
        }
        public static ComboBox add_ComboBox(this Control control, int top, int left, List<string> items)
        {
            var comboBox = control.add_Control<ComboBox>(top, left);
            comboBox.invokeOnThread(
                () =>
                    {
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        if (items != null)
                            comboBox.add_Items(items);
                    });
            return comboBox;
        }        
        public static string get_Text(this ComboBox comboBox)
        {
            return (string)comboBox.invokeOnThread(
                () =>
                    {
                        return comboBox.Text;
                    });
        }
        public static ComboBox set_Text(this ComboBox comboBox, string text)
        {
            return comboBox.invokeOnThread(
                () =>
                    {
                        comboBox.Text = text;
                        return comboBox;
                    });
        }        
        public static ComboBox insert_Item(this ComboBox comboBox, object itemToInsert)
        {
            return (ComboBox)comboBox.invokeOnThread(
                () =>
                    {
                        comboBox.Items.Insert(0, itemToInsert);
                        return comboBox;
                    });
        }
        public static ComboBox add_Item(this ComboBox comboBox, object itemToInsert)
        {
            return (ComboBox)comboBox.invokeOnThread(
                () =>
                    {
                        if (itemToInsert != null)
                            comboBox.Items.Add(itemToInsert);
                        return comboBox;
                    });
        }
        public static ComboBox add_Items<T>(this ComboBox comboBox, List<T> itemsToInsert)
        {
            return (ComboBox)comboBox.invokeOnThread(
                () =>
                    {
                        if (itemsToInsert != null)
                            foreach (var itemToInsert in itemsToInsert)
                                comboBox.Items.Add(itemToInsert);
                        return comboBox;
                    });
        }
        public static ComboBox select_Item(this ComboBox comboBox, int index)
        {
            return (ComboBox)comboBox.invokeOnThread(
                () =>
                    {
                        if (index < comboBox.Items.Count)
                            comboBox.SelectedIndex = index;
                        else
                            "in ComboBox.select_Item, provided index is bigger than the current items collection: {0} > {1}".error(index, comboBox.Items.Count);
                        return comboBox;
                    });
        }
        public static object selected(this ComboBox comboBox)
        {
            return comboBox.invokeOnThread(
                () =>
                    {
                        return comboBox.SelectedItem;
                    });
        }
        public static T selected<T>(this ComboBox comboBox)
        {
            return (T)comboBox.invokeOnThread(
                () =>
                    {
                        if (comboBox.SelectedItem is T)
                            return (T)comboBox.SelectedItem;
                        return default(T);
                    });
        }
        public static ComboBox onSelection(this ComboBox comboBox, MethodInvoker callback)
        {
            if (callback != null)
            {
                comboBox.SelectedIndexChanged += (sender, e) => callback();
            }
            return comboBox;
        }
        public static ComboBox onSelection<T>(this ComboBox comboBox, Action<T> callback)
        {
            if (comboBox.notNull() && callback.notNull())
            {
                comboBox.SelectedIndexChanged += (sender, e) =>
                    {
                        if (comboBox.SelectedItem != null && comboBox.SelectedItem is T)
                            callback((T)comboBox.SelectedItem);
                    };
            }
            return comboBox;
        }
        public static ComboBox clear(this ComboBox comboBox)
        {
            return (ComboBox)comboBox.invokeOnThread(
                () =>
                    {
                        comboBox.Items.Clear();
                        return comboBox;
                    });
        }
        public static List<object> items(this ComboBox comboBox)
        {
            return comboBox.invokeOnThread(
                () =>
                    {
                        var items = new List<object>();
                        foreach (var item in comboBox.Items)
                            items.add(item);
                        return items;
                    });

        }
        public static ComboBox selectFirst(this ComboBox comboBox)
        {
            if (comboBox.items().size() > 0)
                comboBox.select_Item(0);
            return comboBox;
        }
        public static ComboBox dropDownList(this ComboBox comboBox)
        {
            return (ComboBox)comboBox.invokeOnThread(
                () =>
                    {
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        return comboBox;
                    });
        }
        public static ComboBox sorted(this ComboBox comboBox)
        {
            return comboBox.sorted(true);    
        }
        public static ComboBox sorted(this ComboBox comboBox, bool value)
        {
            return (ComboBox)comboBox.invokeOnThread(
                () =>
                    {
                        comboBox.Sorted = value;
                        return comboBox;
                    });            
        }
        public static ComboBox add_Items(this ComboBox comboBox, params object[] items)
        {
            foreach(var item in items)			
                comboBox.add_Item(item);			
            return comboBox;
        }						
        public static ComboBox selectLast(this ComboBox comboBox)
        {
            return comboBox.select_Item(comboBox.items().size()-1);
        }		
        public static object selectedItem(this ComboBox comboBox)
        {
            return comboBox.invokeOnThread(
                ()=>{
                        return comboBox.SelectedItem;
                });
        }		
        public static ComboBox onSelection(this ComboBox comboBox, Action<object> callback)
        {			
            comboBox.onSelection(
                ()=>{						
                        callback(comboBox.selectedItem());
                });
            return comboBox;
        }			

        public static ComboBox comboBoxHeight(this ComboBox comboBox, int height)
        {
            return comboBox.dropDownHeight(height);
        }		
        public static ComboBox dropDownHeight(this ComboBox comboBox, int height)
        {
            return (ComboBox)comboBox.invokeOnThread(
                ()=>{
                        comboBox.DropDownHeight = height;
                        return comboBox;
                });
        }    
    }
}