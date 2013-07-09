// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Threading;
using O2.Kernel;
using O2.DotNetWrappers.DotNet;
using FluentSharp.CoreLib;
using O2.Kernel.ExtensionMethods;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.IO;

//O2File:WPF_WinFormIntegration_ExtensionMethods.cs

//O2Ref:O2_API_Visualization.dll

//O2Ref:PresentationCore.dll
//O2Ref:PresentationFramework.dll
//O2Ref:WindowsBase.dll
//O2Ref:System.Xaml.dll

namespace O2.XRules.Database.Utils
{
    public static class WPF_Controls_ExtensionMethods_Generic
    {    	
		#region generic methods								
    	
        public static T createInThread_Wpf<T>(this UIElement uiElement)
        {
            return (T)uiElement.wpfInvoke(() => typeof(T).ctor());
        }

        public static T add_Control_Wpf<T>(this UIElement uiElement, T uiElementToAdd)
            where T : UIElement
        {
            if ((uiElement is IAddChild).isFalse())
            {
                "in add_Control_Wpf, the host control must implement the IAddChild interface, and '{0}' did not".error(typeof(T).name());
                return null;
            }
            return (T)uiElement.wpfInvoke(
                () =>
                {
                    try
                    {
                        if (uiElementToAdd != null)
                            (uiElement as IAddChild).AddChild(uiElementToAdd);
                        return uiElementToAdd;
                    }
                    catch (Exception ex)
                    {
                        ex.log("in add_Wpf_Control");
                        return null;
                    }
                });

        }

        public static T add_Control_Wpf<T>(this UIElement uiElement)
            where T : UIElement
        {
            if (uiElement.isNull())
            {
                "in add_Wpf_Control, the host control was null".error();
                return null;
            }
            if ((uiElement is IAddChild).isFalse())
            {
                "in add_Wpf_Control, the host control must implement the IAddChild interface, and '{0}' did not".error(typeof(T).name());
                return null;
            }
            return (T)uiElement.wpfInvoke(
                () =>
                {
                    try
                    {
                        var newControl = typeof(T).ctor();
                        if (newControl.isNull())
                        {
                            "in add_Wpf_Control, could not create control of type: {0}".error(typeof(T).name());
                            return null;
                        }
                        (uiElement as IAddChild).AddChild(newControl);
                        return newControl;
                    }
                    catch (Exception ex)
                    {
                        ex.log("in add_Wpf_Control");
                        return null;
                    }
                });
        }

    	/*public static T add_Control<T>(this ContentControl uiElement) where T : UIElement
    	{
    		return (T)uiElement.wpfInvoke(
    			()=>{
    					try
			            {  
	    					var wpfControl = typeof(T).ctor();					    				
	    					if (wpfControl is UIElement)
	    					{
	    						uiElement.invoke("AddChild",((UIElement)wpfControl));
	    						return (T)wpfControl;			    								    					
	    					}
						}			
			            catch (System.Exception ex)
			            {
			                ex.log("in add_Control");
			            }
			            return null;			    				
    				});     		
    	}
        */        
		public static T set<T>(this ContentControl control) where T : UIElement
		{
			return (T)control.wpfInvoke(
    			()=>{ 
    					control.Content = (T)typeof(T).ctor();
    					return control.Content;
    				});
		}  
		
		public static T set<T>(this T control, object value) where T : ContentControl
		{
			return (T)control.wpfInvoke(
    			()=>{ 
    					control.Content = value;
    					return control;
    				});
		}  
		
		public static T newInThread<T>(this ContentControl control) where T : UIElement
		{
			return (T)control.wpfInvoke(
    			()=>{ 
    					return (T)typeof(T).ctor();    					
    				});
		}
		
		public static ContentControl set(this ContentControl control,UIElement uiElement)
		{
			return (ContentControl)control.wpfInvoke(
    			()=>{
    					control.Content = uiElement;
    					return control;
    				});
		}
		
		public static T getFirst<T>(this List<object> list) where  T : UIElement
		{
			foreach(var item in list)
				if (item is T)
					return (T)item;
			return null;
		}


        public static T set_Tag<T>(this T control, object tagObject)
            where T : Control
        {
            control.wpfInvoke(() => control.Tag = tagObject);
            return control;
        }

        public static object get_Tag<T>(this T frameworkElement)
            where T : FrameworkElement
        {
            return (object)frameworkElement.wpfInvoke(() => frameworkElement.Tag);
        }

        public static T get_Tag<T>(this Control control)
        {
            return (T)control.wpfInvoke(
                () =>
                {
                    var tag = control.Tag;
                    if (tag is T)
                        return (T)tag;
                    return default(T);
                });
        }

        /*public static string get_Text<T>(this T control)
            where T : ContentControl
        {
            return (string)control.wpfInvoke(() => control.Content);
        }
        */

        public static string get_Text_Wpf<T>(this T control)
            where T : ContentControl
        {
            return (string)control.wpfInvoke(
                () =>
                {
                    return control.Content;
                });
        }
        /* unfortunatly I can't seem to be call this set_Text able to do this since there are conflits when WPF and WinForms Extension methods are
         * used at the same time */
        public static T set_Text_Wpf<T>(this T control, string value)
            where T : ContentControl
        {
            return (T)control.wpfInvoke(() =>
                        {
                            control.Content = value;
                            return control;
                        });
        }

        public static T set_Content<T>(this T control, string value)
            where T : ContentControl
        {
            return control.set_Text_Wpf(value);
        }

        public static Brush get_Color<T>(this T control)
            where T : Control
        {
            return (Brush)control.wpfInvoke(() => control.Foreground);
        }
     
     	public static T content<T>(this UIElement uiElement)
		{
			var content = uiElement.content();
			if (content is T)
				return (T)content;
			return default(T);
		}
		
		public static object content(this UIElement uiElement)
        {
        	return uiElement.wpfInvoke(
        		()=>{
        				return uiElement.prop("Content");
        			});
        }
		#endregion
	}
	
	public static class WPF_Controls_ExtensionMethods_WPF_Child_Controls
	{
        #region WPF child controls

        	public static List<T> allUIElements<T>(this UIElement uiElement)
       		where T : UIElement
       	{
			return (from element in uiElement.allUIElements()
					where element is T
					select (T)element).toList();
       	}
       	
        public static List<UIElement> allUIElements(this UIElement uiElement)
        {        	
        	return (List<UIElement>)uiElement.wpfInvoke(
        		()=>{
        				var uiElements = new List<UIElement>();        				        				
        				
        				uiElements.Add(uiElement);
        				
        				if (uiElement is ContentControl)
        				{
        					var content = (uiElement as ContentControl).Content;
        					if (content is UIElement)
        						uiElements.AddRange((content as UIElement).allUIElements());
        				}
        				if (uiElement is Panel)
        				{
        					var children = (uiElement as Panel).Children;
        					foreach(var child in children)
        						if (child is UIElement)
        							uiElements.AddRange((child as UIElement).allUIElements()); 
        				}
        				return uiElements;
					});        			        	                	
        }
        
        public static List<T> controls_Wpf<T>(this UIElement uiElement)
        	where T : UIElement
        {
        	return uiElement.allUIElements<T>();
        }    
    
        public static List<UIElement> controls_Wpf(this UIElement uiElement)
        {
        	return uiElement.allUIElements();
        }

		public static List<UIElement> children(this UIElement uiElement)
        {
        	return uiElement.allUIElements();
        }
        
		public static T control_Wpf<T>(this UIElement uiElement)
			where T : UIElement
		{
			foreach(var control in uiElement.allUIElements())
				if (control is T)
					return (T)control;
			return null;
		}


        #endregion       
	}
	
	public static class WPF_Controls_ExtensionMethods_Events
    {
    	#region generic events
    	
    	public static T onKeyPress_Wpf<T>(this T control, Key keyWanted,  Action callback)    		
    		where T : UIElement
    	{    		
    		return control.onKeyPress_Wpf(
    			(keyPressed)=>{
    							if (keyPressed == keyWanted)
    								callback();
    						  });
    	}
    	
    	public static T onKeyPress_Wpf<T>(this T control, Action<Key> callback)
    		where T : UIElement
    	{
    		control.wpfInvoke(    		
    			()=>{
    					control.KeyUp += (sender,e)=> callback(e.Key);
    				});
			return control;
    	}    	

        public static T onMouseDoubleClick<T1, T>(this T control, Action<T1> callback)
            where T : Control
        {
            control.MouseDoubleClick += (sender, e) =>
            {
                var tag = control.get_Tag();
                if (tag != null && tag is T1)
                    callback((T1)tag);
            };
            return control;
        }

        #endregion	
		
	}
	
				
	public static class WPF_Controls_ExtensionMethods_ContentControl
	{
		
		public static T bold<T>(this T contentControl)
			where T : ContentControl
    	{
    		return contentControl.bold(true);
    	}
    	
    	public static T bold<T>(this T contentControl, bool value)
			where T : ContentControl
    	{
    		contentControl.wpfInvoke(
    			()=>{
    					if (value)
    						contentControl.FontWeight = FontWeights.Bold;
    					else
    						contentControl.FontWeight = FontWeights.Normal;    					
    				});
			return contentControl;
    	}
    
    
    }	        
			
	public static class WPF_Controls_ExtensionMethods_Label
	{	
        #region Label

        public static Label set_Text(this Label label, string value)
    	{
            label.set_Text_Wpf(value);    		
			return label;
    	}
    	
    	public static Label add_Label_Wpf<T>(this  T uiElement, string text)
    		where T : UIElement
    	{
    		return uiElement.add_Label_Wpf(text,false);
    	}
    	
    	public static Label add_Label_Wpf<T>(this  T uiElement, string text, bool bold)
            where T : UIElement
        {
        	var label = uiElement.add_Control_Wpf<Label>();
        	if (bold) 
        		label.bold();
        	label.set_Text_Wpf(text);
        	return label;
        }                	
    	
    	#endregion
 
 	}
	
	public static class WPF_Controls_ExtensionMethods_TextBox
	{
    	#region TextBox
		
		public static TextBox set_Text(this TextBox textBox, string value)
    	{
    		textBox.wpfInvoke(()=> textBox.Text = value);    		
			return textBox;
    	}

        public static string get_Text_Wpf(this TextBox textBox)
        {
            return (string)textBox.wpfInvoke(
                ()=>{
	                    return textBox.Text;
	                });
        }

        public static TextBox set_Text_Wpf(this TextBox textBox, string text)
        {
            return (TextBox)textBox.wpfInvoke(
                ()=>{
	                    textBox.Text = text;
	                    return textBox;
	                });
        }

        public static TextBox multiLine(this TextBox textBox)
        {
            return (TextBox)textBox.wpfInvoke(
                () =>
                {
                    textBox.TextWrapping = TextWrapping.Wrap;
                    textBox.AcceptsReturn = true;
                    textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    return textBox;
                });
        }
        
        public static TextBox onTextChange(this TextBox textBox, Action<string> onTextChange)
        {
            return (TextBox)textBox.wpfInvoke(
                ()=>{
                		textBox.TextChanged += (sender,e)=> onTextChange(textBox.get_Text_Wpf());
                		return textBox;
                	});
        }
		
		// not working since the Enter is not captured
        /*public static TextBox onEnter_Wpf(this TextBox textBox, Action<string> callback)
        {        
			textBox.onKeyPress_Wpf(Key.Return, ()=> callback(textBox.get_Text_Wpf()));
			return textBox;
        }*/
    	#endregion
 	}
	
	public static class WPF_Controls_ExtensionMethods_FrameworkElement
	{               
        #region FrameworkElement

        // this is the generic one
    	public static T prop<T>(this T frameworkElement, string propertyName, object value) where T : FrameworkElement
    	{    		
    		frameworkElement.wpfInvoke(
    			()=>{ PublicDI.reflection.setProperty(propertyName, frameworkElement, value); });    				
			return frameworkElement;
    	}
    	
    	
    	// specific to frameworkElement

        public static double width_Wpf<T>(this T frameworkElement)
            where T : FrameworkElement
        {
            return (double)frameworkElement.wpfInvoke(
                () =>
                {
                    return frameworkElement.Width;
                });
        }

        public static double height_Wpf<T>(this T frameworkElement)
            where T : FrameworkElement
        {
            return (double)frameworkElement.wpfInvoke(
                () =>
                {
                    return frameworkElement.Height;
                });

        }
        public static T width_Wpf<T>(this T frameworkElement, double width)
            where T : FrameworkElement
        {
            return (T)frameworkElement.wpfInvoke(
                () =>
                {
                    if (width > -1)
                        frameworkElement.Width = width;
                    return frameworkElement;
                });
        }

        public static T height_Wpf<T>(this T frameworkElement, double height)
            where T : FrameworkElement
        {
            return (T)frameworkElement.wpfInvoke(
                () =>
                {
                    if (height > -1)
                        frameworkElement.Height = height;
                    return frameworkElement;
                });
        }

        public static T left_Wpf<T>(this T frameworkElement, double left)
            where T : FrameworkElement
        {
            return (T)frameworkElement.wpfInvoke(
                () =>
                {
                    if (left > -1)
                        frameworkElement.SetValue(Canvas.LeftProperty, (double)left);
                    return frameworkElement;
                });
        }

        public static T top_Wpf<T>(this T frameworkElement, double top)
            where T : FrameworkElement
        {
            return (T)frameworkElement.wpfInvoke(
                () =>
                {
                    if (top > -1)
                        frameworkElement.SetValue(Canvas.TopProperty, (double)top);
                    return frameworkElement;
                });
        }

    	/*public static T width<T>(this T frameworkElement, double value) where T : FrameworkElement
    	{
    		return frameworkElement.prop("Width",value); 
    	}
    	
    	public static T height<T>(this T frameworkElement, double value) where T : FrameworkElement
    	{
    		return frameworkElement.prop("Height",value); 
    	}
        */
        /*public static double width(this FrameworkElement frameworkElement)
        {
            return (double)frameworkElement.wpfInvoke(() => { return frameworkElement.Width; });
        }

        public static double height(this FrameworkElement frameworkElement)
        {
            return (double)frameworkElement.wpfInvoke(() => { return frameworkElement.Height; });
        }*/
    	
    	public static T tag<T>(this T frameworkElement, object value) where T : FrameworkElement
    	{
    		return frameworkElement.prop("Tag",value); 
    	}
    	
    	// specific to Panel (Ideally i should be able to merge this with the Control ones, but it doesn't seem to be possible    	
    	public static Canvas background(this Canvas canvas, Brush value) 
    	{
    		return canvas.prop("Background",value); 
    	}
    	// specific to Control    	
    	public static T background<T>(this T frameworkElement, Brush value) 
            where T : Control
    	{
    		return frameworkElement.prop("Background",value); 
    	}

        public static T backColor<T>(this T frameworkElement, Brush value)
            where T : FrameworkElement
        {
            return frameworkElement.prop<T>("Background", value);
        }

    	public static T fontSize<T>(this T frameworkElement, double value) where T : Control
    	{
    		return frameworkElement.prop("FontSize",value); 
    	}
    	
    	public static T fontColor<T>(this T frameworkElement, Brush value) where T : Control
    	{
    		return frameworkElement.prop("Foreground",value); 
    	}
    	
    	public static T foreground<T>(this T frameworkElement, Brush value) where T : Control
    	{
    		return frameworkElement.prop("Foreground",value); 
    	}
    	
    	public static T borderThickness<T>(this T frameworkElement, double value) where T : Control
    	{
    		return frameworkElement.borderThickness(new Thickness(value));    		
    	}
    	
    	public static T borderThickness<T>(this T frameworkElement, Thickness value) where T : Control
    	{
    		return frameworkElement.prop("BorderThickness",value); 
    	}
    	
    	public static T padding<T>(this T frameworkElement, double value) where T : Control
    	{
    		return frameworkElement.padding(new Thickness(value));    		
    	}
    	
    	public static T padding<T>(this T frameworkElement, Thickness value) where T : Control
    	{
    		return frameworkElement.prop("Padding",value); 
    	}
    	
    	public static T borderBrush<T>(this T frameworkElement, Brush value) where T : Control
    	{
    		return frameworkElement.prop("BorderBrush",value); 
    	}
    	
    	public static T vertical<T>(this T frameworkElement, VerticalAlignment value) where T : Control
    	{
    		return frameworkElement.verticalContentAlignment(value);
    	}
    	public static T verticalContentAlignment<T>(this T frameworkElement, VerticalAlignment value) where T : Control
    	{
    		return frameworkElement.prop("VerticalContentAlignment",value); 
    	}
    	
    	public static T horizontal<T>(this T frameworkElement, HorizontalAlignment value) where T : Control
    	{
    		return frameworkElement.horizontalContentAlignment(value);
    	}
    	public static T horizontalContentAlignment<T>(this T frameworkElement, HorizontalAlignment value) where T : Control
    	{
    		return frameworkElement.prop("HorizontalContentAlignment",value); 
    	}
    	
    	// specific to TextBoxBase
    	
    	public static T acceptsReturn<T>(this T frameworkElement, bool value) where T : TextBoxBase
    	{
    		return frameworkElement.prop("AcceptsReturn",value); 
    	}

		public static T acceptsTab<T>(this T frameworkElement, bool value) where T : TextBoxBase
    	{
    		return frameworkElement.prop("AcceptsTab",value); 
    	}    	
    	
    	/*
    	
textBox1.prop("VerticalContentAlignment",VerticalAlignment.Center);  
textBox1.prop("HorizontalContentAlignment",System.Windows.HorizontalAlignment.Center);  
textBox1.prop("",true);

    	*/
    	
    	
    	
    	#endregion

 	}
	
	public static class WPF_Controls_ExtensionMethods_ItemsControl
	{
		public static List<object> items(this ItemsControl itemsControl)
		{
			return (List<object>)itemsControl.wpfInvoke(
				()=>{
						var items = new List<Object>();
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
	
	public static class WPF_Controls_ExtensionMethods_TreeView
	{
        #region TreeView

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
            return treeView.set("FontSize", size);
        }

        // generic setter for treeView properties
        public static TreeView set(this TreeView treeView, string propertyToSet, object value)
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

        #endregion

 	}
	
	public static class WPF_Controls_ExtensionMethods_Image
	{
	
        #region Image

        public static Image open(this Image image, string imageLocation)
        {
            return image.open(imageLocation.uri(), -1, -1);
        }

        public static Image open(this Image image, string imageLocation, int width, int height)
        {
            return image.open(imageLocation.uri(), width, height);
        }

        public static Image open(this Image image, Uri imageLocation)
        {
            return image.open(imageLocation, -1, -1);
        }

        public static Image open(this Image image, Uri imageLocation, int width, int height)
        {
            return (Image)image.wpfInvoke(
                () =>
                {
                    if (imageLocation.notNull())
                    {
                        var bitmap = new BitmapImage(imageLocation);
                        image.Source = bitmap;
                        if (width > -1)
                        {
                            image.width_Wpf<Image>((double)width);
                        }
                        if (height > -1)
                        {
                            image.height_Wpf<Image>((double)height);
                        }
                    }
                    return image;
                });
        }


        public static Image add_Image_Wpf<T>(this  T uiElement)
            where T : UIElement
        {
            return (Image)uiElement.wpfInvoke(
            () =>
            {
                return uiElement.add_Control_Wpf<Image>();
            });
        }

        public static Image add_Image_Wpf<T>(this  T uiElement, string pathToImage)
            where T : UIElement
        {
            return uiElement.add_Image_Wpf(pathToImage, -1, -1);
        }

        public static Image add_Image_Wpf<T>(this  T uiElement, string pathToImage, int width, int height)
            where T : UIElement
        {
            return (Image)uiElement.wpfInvoke(
                () =>
                {
                    var image = pathToImage.image_Wpf(width, height);
                    if (image.notNull())
                        uiElement.add_Control_Wpf(image);
                    return image;
                });
        }

        public static Image image_Wpf(this string pathToImage)
        {
            return pathToImage.image_Wpf(-1, -1);
        }

        public static Image image_Wpf(this string pathToImage, int width, int height)
        {
            try
            {
                var image = new Image().open(pathToImage);
                if (width > -1)
                    image.width_Wpf(width);
                if (height > -1)
                    image.height_Wpf(height);
                return image;
            }
            catch (Exception ex)
            {
                ex.log("in pathToImage image_Wpf");
                return null;
            }
        }
        public static List<Image> images_Wpf(this List<string> pathToImages)
        {
            return pathToImages.images_Wpf(-1, -1);
        }

        public static List<Image> images_Wpf(this List<string> pathToImages, int width, int height)
        {
            var images = new List<Image>();
            foreach (var pathToImage in pathToImages)
            {
                var image = pathToImage.image_Wpf(width, height);
                if (image.notNull())
                    images.Add(image);
            }
            return images;
        }

        public static Image show(this Image targetImage, Image sourceImage)
        {
            return (Image)targetImage.wpfInvoke(
                () =>
                {
                    targetImage.Source = sourceImage.Source;
                    return targetImage;
                });
        }

        public static List<string> saveAs_Gifs(this List<Image> images)
        {
            var files = new List<string>();
            foreach (var image in images)
            {
                var file = image.saveAs_Gif();
                if (file.valid())
                    files.Add(file);
            }
            return files;
        }

        public static string saveAs_Gif(this Image image)
        {
            return image.saveAs_Gif(PublicDI.config.getTempFileInTempDirectory(".gif"));
        }
        
        public static string saveAs_Gif(this Image image, string pathToSaveImage)
        {
            return (string)image.wpfInvoke(
                () =>
                {
                    try
                    {
                        using (FileStream outStream = new FileStream(pathToSaveImage, FileMode.Create))
                        {
                            var gifBitmapEncoder = new GifBitmapEncoder();
                            gifBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapImage)image.Source));//BitmapFrame.Create(image));

                            gifBitmapEncoder.Save(outStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.log("in WPF Image saveas_Gif");
                    }
                    if (pathToSaveImage.fileExists())
                        return pathToSaveImage;
                    return "";
                });


        }

        #endregion

 	}
	
	
	public static class WPF_Controls_ExtensionMethods_ComboBox
	{
	
        #region ComboBox

        public static ComboBox add_Item(this ComboBox comboBox, string itemText)
        {
            return comboBox.add_Items(itemText);
        }

        public static ComboBox add_Items(this ComboBox comboBox, params string[] itemTexts)
        {
            return (ComboBox)comboBox.wpfInvoke(
                () =>
                {
                    foreach (var itemText in itemTexts)
                    {
                        var comboBoxItem = new ComboBoxItem();
                        comboBoxItem.Content = itemText;
                        comboBox.Items.Add(comboBoxItem);
                    }
                    return comboBox;
                });
        }

        public static ComboBox selectFirst(this ComboBox comboBox)
        {
            return (ComboBox)comboBox.wpfInvoke(
                () =>
                {
                    if (comboBox.Items.size() > 0)
                        comboBox.SelectedIndex = 0;
                    return comboBox;
                });
        }                

        public static string get_Text_Wpf(this ComboBox comboBox)
        {
            return (string)comboBox.wpfInvoke(
                () =>
                {
                    if (comboBox.SelectedItem.notNull() && comboBox.SelectedItem is ComboBoxItem)
                        return (comboBox.SelectedItem as ComboBoxItem).Content;
                    return "";
                });
        }

        #endregion


 	}
	
	public static class WPF_Controls_ExtensionMethods_Button
	{
	
        #region Button

        public static Button onClick_Wpf(this Button button, Action callback)
        {
            return (Button)button.wpfInvoke(
                () =>
                {
                	if (button.notNull())
                    	button.Click += (sender, e) => 
                    		O2Thread.mtaThread(()=> 
                    			{
                    				try
                    				{
                    					callback();
                    				}
                    				catch(Exception ex)
                    				{
                    					ex.log("in WPF button onClick_Wpf");
                    				}
                    			});
                    return button;
                });
        }
        
        public static Button name(this List<Button> buttons, string text)
        {            
        	foreach(var button in buttons)
        		if (button.get_Text_Wpf() == text)
        			return button;
        	return null;
        }
	
		public static Button click (this Button button)
		{		
			return (Button)button.wpfInvoke(
				()=>{
						button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
						return button;
					});
		}
        #endregion

 	}
	
	public static class WPF_Controls_ExtensionMethods_ListView
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
            			var selectedValues = new List<Object>();
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
	
	public static class WPF_Controls_ExtensionMethods_StackPanel
	{	
        #region StackPanel

        public static StackPanel add_StackPanel(this UIElement uiElement)
        {
            return uiElement.add_Control_Wpf<StackPanel>();
        }

        #endregion

 	}
	
	public static class WPF_Controls_ExtensionMethods_WrapPanel
	{	
        #region WrapPanel

        public static WrapPanel add_WrapPanel(this UIElement uiElement)
        {
            return uiElement.add_Control_Wpf<WrapPanel>();
        }

        #endregion

 	}
 	
 	public static class WPF_Controls_ExtensionMethods_TextBlock
	{	
        #region WrapPanel

        public static TextBlock add_TextBlock(this UIElement uiElement)
        {
        	return (TextBlock)uiElement.wpfInvoke(
        		()=>{
			            var textBlock = uiElement.add_Control_Wpf<TextBlock>();
			            textBlock.TextWrapping = TextWrapping.Wrap;
			            textBlock.Padding = new Thickness(4);
			            return textBlock;
			        });
        }
        
        public static TextBlock set_Text_Wpf(this TextBlock textBlock, string text)
        {
        	
        	textBlock.wpfInvoke(()=> textBlock.Text = text);
            return textBlock;
        }

        #endregion

 	}

	public static class WPF_Controls_ExtensionMethods_Grid
	{
	
        #region Grid

        public static Grid add_Grid_Wpf(this UIElement uiElement)
        {
            return uiElement.add_Control_Wpf<Grid>();
        }

        #endregion

 	}
	
	public static class WPF_Controls_ExtensionMethods_WebBrowser
	{
	
        #region WebBrowser (WPF one which is a wrapper on the WinForms one)

        public static WebBrowser open(this WebBrowser webBrowser, string url)
        {
            if (url.isUri())
            {
                "[WPF WebBrowser] opening page: {0}".debug(url);
                webBrowser.wpfInvoke(() => webBrowser.Navigate(url.uri()));
            }
            return webBrowser;
        }

        #endregion

 	}
	
	public static class WPF_Controls_ExtensionMethods_Control
	{
		public static T opacity<T>(this T uiElement, double value) where T : UIElement
        {
            return (T)uiElement.wpfInvoke(
                ()=>{
                		uiElement.Opacity = value;
                		return uiElement;
                	});
        }	      
        public static T color<T>(this T control, string colorName) where T : Control
        {
            var color = new BrushConverter().ConvertFromString(colorName);
            if (color is Brush)
                control.fontColor((Brush)color);
            return control;
        }
        public static T black<T>(this T control) where T : Control
        {
            return control.fontColor(Brushes.Black);
        }
        public static T blue<T>(this T control) where T : Control
        {
            return control.fontColor(Brushes.Blue);
        }
        public static T red<T>(this T control) where T : Control
        {
            return control.fontColor(Brushes.Red);            
        }
 	}
	
	public static class WPF_Controls_ExtensionMethods_Animation
	{
        #region Animation

        public static T rotate<T>(this T uiElement)
            where T : UIElement
        {
            return uiElement.rotate(0, 360, 3, false);
        }

        public static T rotate<T>(this T uiElement, bool loopAnimation)
            where T : UIElement
        {
            return uiElement.rotate(0, 360, 3, loopAnimation);
        }

        public static T rotate<T>(this T uiElement, double fromValue, double toValue, int durationInSeconds, bool loopAnimation)
            where T : UIElement
        {
            return (T)uiElement.wpfInvoke(
                () =>
                {
                    DoubleAnimation doubleAnimation = new DoubleAnimation(fromValue, toValue, new Duration(TimeSpan.FromSeconds(durationInSeconds)));
                    RotateTransform rotateTransform = new RotateTransform();
                    uiElement.RenderTransform = rotateTransform;
                    uiElement.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                    if (loopAnimation)
                        doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                    rotateTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
                    return uiElement;
                });
        }

        public static T fadeIn<T>(this T uiElement, int durationInSeconds)
            where T : UIElement
        {
            return uiElement.fadeFromTo(0, 1, durationInSeconds, false);
        }

        public static T fadeOut<T>(this T uiElement, int durationInSeconds)
            where T : UIElement
        {
            return uiElement.fadeFromTo(1, 0, durationInSeconds, false);
        }

        public static T fadeFromTo<T>(this T uiElement, double fromOpacity, double toOpacity, int durationInSeconds, bool loopAnimation)
            where T : UIElement
        {
            return (T)uiElement.wpfInvoke(() =>
            {
                var doubleAnimation = new DoubleAnimation(fromOpacity, toOpacity, new Duration(TimeSpan.FromSeconds(durationInSeconds)));
                if (loopAnimation)
                    doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                uiElement.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);
                return uiElement;
            });
        }

        #endregion

    }
}
