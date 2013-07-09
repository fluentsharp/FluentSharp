using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FluentSharp.WPF
{
    public static class ItemsControl_ExtensionMethods
    {
        public static List<object> items(this ItemsControl itemsControl)
        {
            return (List<object>)itemsControl.wpfInvoke(
                ()=>{
                        var items = new List<object>();
                        foreach(var item in itemsControl.Items)
                            items.Add(item);
                        return items;
							
                        //return itemsControl.Items;						
                });
        }
        public static List<T> items<T>(this ItemsControl itemsControl)
            where T : UIElement
        {
            return (List<T>)itemsControl.wpfInvoke(
                ()=>{
                        var items = new List<T>();
                        foreach(var item in itemsControl.Items)
                            if (item is T)
                                items.Add((T)item);
                        return items;						
                });
        }
    }
}