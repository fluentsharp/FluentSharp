using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FluentSharp.CoreLib;

namespace FluentSharp.WPF
{
    public static class ListView_ExtensionMethods
    {

        #region ListView        

        public static System.Windows.Controls.ListView add_ListView_Wpf<T>(this  T uiElement)
            where T : UIElement
        {
            return uiElement.add_Control_Wpf<System.Windows.Controls.ListView>();
        }

        public static System.Windows.Controls.ListView add_Item(this System.Windows.Controls.ListView listView, string text)        
        {
            return listView.add_Items(text);
        }
		
        public static System.Windows.Controls.ListView add_Items(this System.Windows.Controls.ListView listView, params string[] texts)        
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {	foreach(var text in texts) 
                        listView.Items.Add(text);
                        return listView;
                    });

        }

        public static System.Windows.Controls.ListView add_Item<T>(this System.Windows.Controls.ListView listView, T item)
            where T : UIElement
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {
                        listView.Items.Add(item);
                        return listView;
                    });
        }

        public static System.Windows.Controls.ListView add_Image_Wpf(this System.Windows.Controls.ListView listView, string pathToImage)
        {
            return listView.add_Image_Wpf(pathToImage, -1, -1);
        }

        public static System.Windows.Controls.ListView add_Image_Wpf(this System.Windows.Controls.ListView listView, string pathToImage, int width, int height)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {
                        var image = new Image().open(pathToImage);
                        if (image.isNull())
                            return listView;
                        if (width > -1)
                            image.width_Wpf(width);
                        if (height > -1)
                            image.height_Wpf(height);
                        listView.add_Item(image);
                        return listView;
                    });
        }

        public static System.Windows.Controls.ListView add_Images_Wpf(this System.Windows.Controls.ListView listView, List<string> pathToImages)
        {
            return listView.add_Images_Wpf(pathToImages, -1, -1);
        }

        public static System.Windows.Controls.ListView add_Images_Wpf(this System.Windows.Controls.ListView listView, List<string> pathToImages, int width, int height)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {
                        foreach (var image in pathToImages.images_Wpf(width, height))
                            listView.Items.Add(image);
                        return listView;
                    });
        }

        public static object selectedValue(this System.Windows.Controls.ListView listView)
        {
            return (object)listView.wpfInvoke(() => listView.SelectedValue);
        }
        
        public static List<object> selectedValues(this System.Windows.Controls.ListView listView)
        {
            return (List<object>)listView.wpfInvoke(
                ()=>{
                        var selectedValues = new List<object>();
                        if (listView.SelectedItems.notNull() && listView.SelectedItems is System.Collections.ICollection)
                            foreach(var item in listView.SelectedItems)
                                selectedValues.Add(item);
                        return selectedValues;
                });
        }

        /*public static object selected_Items(this ListView listView)
        {
            return (object)listView.wpfInvoke(()=> listView.SelectedItems);					
        }*/

        public static int selectedIndex(this System.Windows.Controls.ListView listView)
        {
            return (int)listView.wpfInvoke(
                ()=>{
                        return listView.SelectedIndex;
                });
        }               
        
        public static T selected<T>(this System.Windows.Controls.ListView listView)
        {
            return (T)listView.wpfInvoke(
                () =>
                    {
                        if (listView.SelectedValue is T)
                            return (T)listView.SelectedValue;
                        return default(T);
                    });
        }

        public static System.Windows.Controls.ListView afterSelect(this System.Windows.Controls.ListView listView, Action callback)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {
                        listView.SelectionChanged += (sender, e) => callback();
                        return listView;
                    });
        }

        public static System.Windows.Controls.ListView afterSelect<T>(this System.Windows.Controls.ListView listView, Action<T> callback)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {                	
                        listView.SelectionChanged +=
                            (sender, e) =>
                                {                        	
                                    listView.wpfInvoke(
                                        () =>{                                	
                                                 if (listView.SelectedValue is T && listView.SelectedItems.size() ==1)
                                                     callback((T)listView.SelectedValue);
                                        });                            
                                };
                        return listView;
                    });
        }
        
        public static System.Windows.Controls.ListView afterSelects<T>(this System.Windows.Controls.ListView listView, Action<List<T>> callback)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {
                        listView.SelectionChanged +=
                            (sender, e) =>
                                {
                                    listView.wpfInvoke(
                                        () =>
                                            {                                	
                                                var selectedItems = new List<T>();
                                                foreach(var selectedItem in listView.SelectedItems)  		                                	                                		
                                                    if (selectedItem is T)
                                                        selectedItems.Add((T)selectedItem);                                    
                                                if (selectedItems.size()>0)
                                                    callback(selectedItems);
                                            });
                                };
                        return listView;
                    });
        }

        public static System.Windows.Controls.ListView remove_SelectedValue(this System.Windows.Controls.ListView listView)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(() => listView.remove_Item(listView.selectedValue()));
        }

        public static System.Windows.Controls.ListView remove_Item(this System.Windows.Controls.ListView listView, object itemToRemove)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {
                        if (itemToRemove.notNull())
                            listView.Items.Remove(itemToRemove);
                        return listView;
                    });
        }
        
        public static System.Windows.Controls.ListView remove_Item(this System.Windows.Controls.ListView listView, List<object> itemsToRemove)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {
                        foreach(var itemToRemove in itemsToRemove)
                            if (itemToRemove.notNull())
                                listView.Items.Remove(itemToRemove);	                    
                        return listView;
                    });
        }

        public static System.Windows.Controls.ListView remove_Items(this System.Windows.Controls.ListView listView, List<object> itemsToRemove)
        {
            foreach (var itemToRemove in itemsToRemove)
                listView.remove_Item(itemToRemove);
            return listView;
        }
        
        public static System.Windows.Controls.ListView remove_SelectedItems(this System.Windows.Controls.ListView listView)
        {
            var originalSelectedIndex = listView.selectedIndex();	
            var selectedValues = listView.selectedValues();        	        
            listView.remove_Items(selectedValues);
            listView.selectIndex(originalSelectedIndex);
            return listView;
        }
        
        public static List<object> items(this System.Windows.Controls.ListView listView)
        {
            return (List<object>)listView.wpfInvoke(
                () =>
                    {
                        return (from object item in listView.Items
                                select item).toList();
                    });
        }

        public static List<T> items<T>(this System.Windows.Controls.ListView listView)
        {
            return (List<T>)listView.wpfInvoke(
                () =>
                    {
                        return (from object item in listView.Items
                                where item is T
                                select (T)item).toList();
                    });

        }

        public static System.Windows.Controls.ListView clear(this System.Windows.Controls.ListView listView)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {
                        listView.Items.Clear();
                        return listView;
                    });
        }

        public static System.Windows.Controls.ListView remove_All(this System.Windows.Controls.ListView listView)
        {
            return listView.clear();
        }

        public static System.Windows.Controls.ListView selectIndex(this System.Windows.Controls.ListView listView, int index)
        {
            return (System.Windows.Controls.ListView)listView.wpfInvoke(
                () =>
                    {
                        var items = listView.items();
                        if (index > -1 && index < items.size())
                        {
                            listView.SelectedIndex = index;
                            listView.SelectedItem = items[index];
                        }                    
                        return listView;
                    });
        }

        public static System.Windows.Controls.ListView selectFirst(this System.Windows.Controls.ListView listView)
        {
            return listView.selectIndex(0);

        }
		
        public static System.Windows.Controls.ListView onDeleteKey_Remove_SelectedItems(this System.Windows.Controls.ListView listView)
        {
            listView.onKeyPress_Wpf(Key.Delete, ()=> listView.remove_SelectedItems());
            return listView;
        }
		
        public static System.Windows.Controls.ListView useWrapPanel(this System.Windows.Controls.ListView listView)
        {
            listView.wpfInvoke(
                ()=>{
                        var frameworkElementFactory = new System.Windows.FrameworkElementFactory(typeof(WrapPanel));
                        //frameworkElementFactory.SetValue(WrapPanel.OrientationProperty, Orientation.Vertical);  
                        var itemsPanelTemplate = new ItemsPanelTemplate(frameworkElementFactory);					
                        listView.ItemsPanel = itemsPanelTemplate;
                });
            return listView;
        }
		
        #endregion

    }
}