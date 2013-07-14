using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_ListBox
    { 
        public static ListBox add_ListBox(this Control control)
        {
            return control.add_Control<ListBox>();
        }		
        public static ListBox add_Item(this ListBox listBox, object item)
        {
            return listBox.add_Items(item);
        }		
        public static ListBox add_Items(this ListBox listBox, params object[] items)
        {
            return (ListBox)listBox.invokeOnThread(
                ()=>{
                        listBox.Items.AddRange(items);
                        return listBox;
                });					
        }		
        public static object selectedItem(this ListBox listBox)
        {
            return (object)listBox.invokeOnThread(
                ()=>{	
                        return listBox.SelectedItem;	
                });
        }		
        public static T selectedItem<T>(this ListBox listBox)
        {			
            var selectedItem = listBox.selectedItem();
            if (selectedItem is T) 
                return (T) selectedItem;
            return default(T);					
        }		
        public static ListBox select(this ListBox listBox, int selectedIndex)
        {
            return (ListBox)listBox.invokeOnThread(
                ()=>{
                        if (listBox.Items.size() > selectedIndex)
                            listBox.SelectedIndex = selectedIndex;
                        return listBox;
                });					
        }		
        public static ListBox selectFirst(this ListBox listBox)
        {
            return listBox.select(0);
        }
    }
}