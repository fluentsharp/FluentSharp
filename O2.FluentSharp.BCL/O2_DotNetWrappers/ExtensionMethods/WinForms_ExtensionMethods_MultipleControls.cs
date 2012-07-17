using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using O2.DotNetWrappers.Windows;
using O2.Kernel;
using O2.DotNetWrappers.ExtensionMethods;
using System.Collections;
using O2.DotNetWrappers.DotNet;
using System.ComponentModel;
using O2.Views.ASCX.O2Findings;
using O2.Views.ASCX.DataViewers;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class WinForms_ExtensionMethods_Component
    {
        public static T onDisposed<T>(this T component, Action onDisposed)  
            where T : Component
        {
            component.Disposed += (sender, e) => onDisposed();
            return component;
        }
    }
    
    public static class WinForms_ExtensionMethods_Button
    {     
        public static Button add_Button(this Control control, string text)
        {
            return control.add_Button(text, -1);
        }
        public static Button add_Button(this Control control, string text, int top)
        {
            return control.add_Button(text, top, -1);
        }
        public static Button add_Button(this Control control, string text, int top, int left)
        {
            return control.add_Button(text, top, left, -1, -1);
        }
        public static Button add_Button(this Control control, string text, int top, int left, int height)
        {
            return control.add_Button(text, top, left, height, -1);
        }
        public static Button add_Button(this Control control, string text, int top, int left, int height, int width)
        {
            return control.add_Button(text, top, left, height, width, null);
        }
        public static Button add_Button(this Control control, string text, int top, int left, int height, int width, MethodInvoker onClick)
        {
            return (Button)control.invokeOnThread(
                               () =>
                                   {
                                       var button = new Button();
										button.Text = text;
                                       if (top > -1)
                                           button.Top = top;
                                       if (left > -1)
                                           button.Left = left;
                                       if (width == -1 && height == -1)
                                           button.AutoSize = true;
                                       else
                                       {
                                           if (width > -1)
                                               button.Width = width;
                                           if (height > -1)
                                               button.Height = height;
                                       }
                                       button.onClick(onClick);
                                       /*if (onClick != null)
                                    button.Click += (sender, e) => onClick();*/
                                       control.Controls.Add(button);
                                       return button;
                                   });
        }
        public static Button add_Button(this Control control, string text, int top, int left, MethodInvoker onClick)
        {
            return control.add_Button(text, top, left, -1, -1, onClick);
        }
        public static Button add_Button(this Control control, int top, string buttonText)
        {
            return control.add_Button(top, 0, buttonText);
        }

        public static Button add_Button(this Control control, int top, int left, string buttonText)
        {
            return control.add_Button(buttonText, top, left);
        }

        public static Button onClick(this Button button, MethodInvoker onClick)
        {
            if (onClick != null)
                button.Click += (sender, e) => O2Thread.mtaThread(() => onClick());
            return button;
        }
        public static Button click(this Button button)
        {            
            O2Thread.mtaThread(
                () =>
                {
                    button.invokeOnThread(() => button.PerformClick());
                });
            return button;
        }
        public static Button set_Text(this Button button, string text)
        {
            return (Button)button.invokeOnThread(
                () =>
                {
                    button.Text = text;
                    return button;
                });

        }
        public static List<Button> buttons(this Control control)
        {
            return control.controls<Button>(true);
        }
        public static Button button(this Control control, string text)
        {
            foreach (var button in control.buttons())
                if (button.get_Text() == text)
                    return button;
            return null;
        }

    }

    public static class WinForms_ExtensionMethods_CheckBox
    {             
        public static CheckBox add_CheckBox(this Control control, string text, int top, int left, Action<bool> onChecked)
        {
            return (CheckBox) control.invokeOnThread(
                                  () =>{
											var checkBox = new CheckBox();
											checkBox.Text = text;
                                          	checkBox.CheckedChanged += (sender, e) => onChecked(checkBox.Checked);
                                          	if (top > -1)
                                              	checkBox.Top = top;
                                          	if (left > -1)
                                          	    checkBox.Left = left;
                                          	control.Controls.Add(checkBox);
                                          	return checkBox;
                                      	});
        }
        public static CheckBox add_CheckBox(this Control control, int top, string checkBoxText)
        {
            return control.add_CheckBox(top, 0, checkBoxText);
        }
        public static CheckBox add_CheckBox(this Control control, int top, int left, string checkBoxText)
        {
            return control.add_CheckBox(checkBoxText, top, left, (value) => { })
                          .autoSize();
        }
        public static bool @checked(this CheckBox checkBox)
        {
            return checkBox.value();
        }
        public static bool value(this CheckBox checkBox)
        {
            return (bool)checkBox.invokeOnThread(
                () =>
                {
                    return checkBox.Checked;
                });
        }
        public static CheckBox @checked(this CheckBox checkBox, bool value)
        {
            return checkBox.value(value);
        }
        public static CheckBox value(this CheckBox checkBox, bool value)
        {
            return (CheckBox)checkBox.invokeOnThread(
                () =>
                {
                    checkBox.Checked = value;
                    return checkBox;
                });
        }
        public static CheckBox check(this CheckBox checkBox)
        {
            return checkBox.value(true);
        }
        public static CheckBox uncheck(this CheckBox checkBox)
        {
            return checkBox.value(false);
        }
        public static CheckBox tick(this CheckBox checkBox)
        {
            return checkBox.value(true);
        }
        public static CheckBox untick(this CheckBox checkBox)
        {
            return checkBox.value(false);
        }
        public static CheckBox autoSize(this CheckBox checkBox)
        {
            return checkBox.autoSize(true);
        }
        public static CheckBox autoSize(this CheckBox checkBox, bool value)
        {
            checkBox.invokeOnThread(() => checkBox.AutoSize = value);
            return checkBox;
        }
        public static CheckBox append_CheckBox(this Control control, string text, Action<bool> action)
		{
			return control.append_Control<CheckBox>()
						  .set_Text(text)
						  .autoSize()
						  .onChecked(action);
		}
		public static CheckBox onClick(this CheckBox checkBox, Action<bool> action)
		{
			return checkBox.onChecked(action);
		}		
		public static CheckBox onChecked(this CheckBox checkBox, Action<bool> action)
		{
			return checkBox.checkedChanged(action);
		}
		public static CheckBox checkedChanged(this CheckBox checkBox, Action<bool> action)
		{
			checkBox.invokeOnThread(
				()=> checkBox.CheckedChanged+= (sender,e) => {action(checkBox.value());});
			return checkBox;
		}

    }

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
            return (ComboBox)comboBox.invokeOnThread(
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
            if (callback != null)
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
            return (List<object>)comboBox.invokeOnThread(
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

    public static class WinForms_ExtensionMethods_MainMenu
    {
        public static MainMenu          mainMenu(this Control control)
		{
			if (control.notNull())
			{
				var parentForm = control.parentForm();
				if (parentForm.notNull())
				{
					return (MainMenu)control.invokeOnThread(
						()=>{						
								if (parentForm.Menu.notNull())
									return parentForm.Menu;
								var mainMenu =  new MainMenu();
								parentForm.Menu = mainMenu;
								return mainMenu;
							});					
				}
			}
			"provided control is null or is not hosted by a Form".error();
			return null;
		}
        public static MainMenu          add_MainMenu(this Control control)
        {
            return control.mainMenu().clear();
        }		
		public static Form              parentForm(this MenuItem menuItem)
		{
			if (menuItem.isNull())
				return null;
			return menuItem.Parent.GetMainMenu().GetForm();
		}						
		public static MainMenu          mainMenu(this MenuItem menuItem)
		{
			return menuItem.Parent.GetMainMenu();
		}
        public static MenuItem          add(this MenuItem menuItem, string name, Action onSelect)
        {
            return menuItem.add_MenuItem(name, onSelect);
        }
		public static MenuItem          add_Menu(this MainMenu mainMenu , string name)
		{
			return (MenuItem)mainMenu.GetForm()
								     .invokeOnThread(
										()=>{
												var newMenuItem = new MenuItem();
												newMenuItem.Text = name;
												mainMenu.MenuItems.Add(newMenuItem);
												return newMenuItem; 
											});					
		}		
		public static MenuItem          add_Menu(this MenuItem menuItem, string name)
		{
			return (MenuItem)menuItem.parentForm()
								     .invokeOnThread(
										()=>{
												return menuItem.mainMenu().add_Menu(name);
											});
		}	
		public static MenuItem          add_MenuItem(this MenuItem menuItem, string name)
		{
			return menuItem.add_MenuItem(name,false,null);
		}		
		public static MenuItem          add_MenuItem(this MenuItem menuItem , string name, Action onClick)
		{
			return menuItem.add_MenuItem(name, false, onClick);
		}		
		public static MenuItem          add_MenuItem(this MenuItem menuItem , string name, bool returnNewMenuItem, Action onClick)
		{
			return (MenuItem)menuItem.parentForm()
								     .invokeOnThread(
										()=>{
												var newMenuItem = new MenuItem();
												newMenuItem.Text = name;
												if(onClick.notNull())
													newMenuItem.Click+= (sender,e)=>{
																						O2Thread.mtaThread(()=> onClick());
																					 };
												menuItem.MenuItems.Add(newMenuItem);							 
												if (returnNewMenuItem)
													return newMenuItem;
												return menuItem; 
											});					
		}        
        public static MenuItem          add_Separator(this MenuItem menuItem)
        {
            return menuItem.add_MenuItem("-", () => { });
        }
        public static List<MenuItem>    items(this MainMenu mainMenu)
        {
            return (List<MenuItem>)mainMenu.GetForm()
                                           .invokeOnThread(
                                                () =>
                                                {
                                                    var menuItems = new List<MenuItem>();
                                                    foreach (MenuItem menuItem in mainMenu.MenuItems)  // Linq query doesn't work
                                                        menuItems.Add(menuItem);
                                                    return menuItems;
                                                });
        }
        public static MenuItem          menu(this MainMenu mainMenu, string name)
        {
            return (MenuItem)mainMenu.GetForm()
                                     .invokeOnThread(
                                        () =>
                                        {
                                            var topMenu = mainMenu.items().where((item) => item.Text == name).first();
                                            if (topMenu.notNull())
                                                return topMenu;
                                            return null;
                                        });
        }
        public static MainMenu          clear(this MainMenu mainMenu)
        {
            return (MainMenu)mainMenu.GetForm()
                                 .invokeOnThread(
                                    () =>
                                    {
                                        mainMenu.MenuItems.Clear();
                                        return mainMenu;
                                    });

        }		
        
    }

    public static class WinForms_ExtensionMethods_ContextMenuStrip
    {    
        public static ContextMenuStrip add_ContextMenu(this Control control)
        {
            if (control.isNull())
                return null;
            var contextMenu = new ContextMenuStrip();
            control.ContextMenuStrip = contextMenu;
            return contextMenu;
        }
        public static ToolStripMenuItem add_MenuItem(this ContextMenuStrip contextMenu, string text)
        {
            return contextMenu.add_MenuItem(text, () => { });
        }
        public static ToolStripMenuItem add_MenuItem(this ContextMenuStrip contextMenu, string text, MethodInvoker onClick)
        {
            return contextMenu.add_MenuItem(text, (item) => O2Thread.mtaThread(() => onClick()));
        }
        public static ToolStripMenuItem add_MenuItem(this ContextMenuStrip contextMenu, string text, Action<ToolStripMenuItem> onClick)
        {
            if (contextMenu.isNull())
                return null;
            var menuItem = new ToolStripMenuItem();
			menuItem.Text = text;
            contextMenu.Items.Add(menuItem);
            menuItem.Click += (sender, e) => O2Thread.mtaThread(() => onClick(menuItem));
            return menuItem;
        }
        public static ToolStripMenuItem add_MenuItem(this ToolStripMenuItem menuItem, string text)
        {
            return menuItem.add_MenuItem(text, true);
        }
        public static ToolStripMenuItem add_MenuItem(this ToolStripMenuItem menuItem, string text, bool returnParentMenuItem)
        {
            return menuItem.add_MenuItem(text, returnParentMenuItem, () => { });
        }
        public static ToolStripMenuItem add_MenuItem(this ToolStripMenuItem menuItem, string text, MethodInvoker onClick)
        {
            return menuItem.add_MenuItem(text, true, onClick);
        }
        public static ToolStripMenuItem add_MenuItem(this ToolStripMenuItem menuItem, string text, bool returnParentMenuItem, MethodInvoker onClick)
        {
            return menuItem.add_MenuItem(text, returnParentMenuItem, (item) => onClick());
        }
        public static ToolStripMenuItem add_MenuItem(this ToolStripMenuItem menuItem, string text, Action<ToolStripMenuItem> onClick)
        {
            return menuItem.add_MenuItem(text, true, onClick);
        }
        public static ToolStripMenuItem add_MenuItem(this ToolStripMenuItem menuItem, string text, bool returnParentMenuItem, Action<ToolStripMenuItem> onClick)
        {
            return (ToolStripMenuItem)menuItem.toolStrip().invokeOnThread(
                ()=>{
                        if (menuItem.isNull())
                            return null;
                        var clildMenuItem = new ToolStripMenuItem();
						clildMenuItem.Text = text;
                        clildMenuItem.Click +=
                            (sender, e) => O2Thread.mtaThread(() => onClick(clildMenuItem)); 
                        menuItem.DropDownItems.Add(clildMenuItem);
                        if (returnParentMenuItem)
                            return menuItem;
                        return clildMenuItem;
                    });
        }
		public static ToolStripComboBox enabled(this ToolStripComboBox toolStripComboBox)
		{
			return toolStripComboBox.enabled(true);
		}
		public static ToolStripComboBox enabled(this ToolStripComboBox toolStripComboBox, bool value)
		{
			toolStripComboBox.Control.enabled(value);
			
			//				toolStripComboBox.Enabled = value;
//						});
			return toolStripComboBox;
		}
		public static ToolStripButton enabled(this ToolStripButton toolStripButton)
		{
			return toolStripButton.enabled(true);
		}
		public static ToolStripButton enabled(this ToolStripButton toolStripButton, bool value)
		{
			toolStripButton.Enabled = value;
			return toolStripButton;
		}
		public static ToolStripButton visible(this ToolStripButton toolStripButton)
		{
			return toolStripButton.visible(true);
		}
		public static ToolStripButton visible(this ToolStripButton toolStripButton, bool value)
		{
			toolStripButton.Owner.invokeOnThread(
					()=>{						
							toolStripButton.Visible = value;
						});
			return toolStripButton;
		}
    }    

    public static class WinForms_ExtensionMethods_FlowLayoutPanel
    {             
        public static FlowLayoutPanel add_FlowLayoutPanel(this Control control)
        {
            return control.add_Control<FlowLayoutPanel>();
        }
        public static FlowLayoutPanel clear(this FlowLayoutPanel flowLayoutPanel)
        {
            return (FlowLayoutPanel)flowLayoutPanel.invokeOnThread(
                    () =>
                    {
                        flowLayoutPanel.Controls.Clear();
                        return flowLayoutPanel;
                    });
        }
    }

    public static class WinForms_ExtensionMethods_GroupBox
    {     
        public static GroupBox add_GroupBox(this Control control, string groupBoxText)
        {
            return (GroupBox) control.invokeOnThread(
                                  ()=>{
                                    		var groupBox = new GroupBox();
											groupBox.Dock = DockStyle.Fill;
											groupBox.Text = groupBoxText;
	                                        control.Controls.Add(groupBox);
	                                        return groupBox;
                                      });
        }
        public static GroupBox title(this Control control, string title)
		{
			return control.add_GroupBox(title);
		}
    }

    public static class WinForms_ExtensionMethods_Label
    { 
        public static Label add_Label(this Control control, string text, int top)
        {
            return control.add_Label(text, top, -1);
        }
        public static Label add_Label(this Control control, string text, int top, int left)
        {
            Label label = control.add_Label(text);
            label.invokeOnThread(() =>
                {
                    if (top > -1)
                        label.Top = top;
                    if (left > -1)
                        label.Left = left;
                });
            return label;
        }
        public static Label add_Label(this Control control, string labelText)
        {
            return (Label) control.invokeOnThread(
                               ()=>{
										var label = new Label();
										label.AutoSize = true;
										label.Text = labelText;
                                       	control.Controls.Add(label);
                                       	return label;
                                   });
        }
        public static System.Windows.Forms.Label append_Label<T>(this T control, string text)            where T : Control
        {
            return control.append_Control<System.Windows.Forms.Label>()
                          .set_Text(text)
                          .autoSize();
        }
        public static Label set_Text(this Label label, string text)
        {
            return (Label)label.invokeOnThread(
                                    () =>
                                    {
                                        label.Text = text;
                                        return label;
                                    });
        }
        public static Label append_Text(this Label label, string text)
        {
            return (System.Windows.Forms.Label)label.invokeOnThread(
                () =>
                {
                    label.Text += text;
                    return label;
                });

        }
        public static string get_Text(this Label label)
        {
            return (string)label.invokeOnThread(
                () =>
                {
                    return label.Text;
                });
        }
        public static Label textColor(this Label label, Color color)
        {
            return (Label)label.invokeOnThread(
                () =>
                {
                    label.ForeColor = color;
                    return label;
                });
        }
        public static Label autoSize(this Label label)
        {
            label.invokeOnThread(() => label.AutoSize = true);
            return label;
        }        
        public static Label autoSize(this Label label, bool value)
		{
			label.invokeOnThread(
				()=>{						
						label.AutoSize = value;
					});
			return label;
		}		
		public static Label text_Center(this Label label)			
		{			
			label.invokeOnThread(
				()=>{						
						label.autoSize(false);
						label.TextAlign = ContentAlignment.MiddleCenter;
					});
			return label;
		}	
    }

    public static class WinForms_ExtensionMethods_LinkLabel
    { 
        public static LinkLabel add_Link(this Control control, string text, int top, int left, MethodInvoker onClick)
        {
            return (LinkLabel)control.invokeOnThread(
								()=>{
										var link = new LinkLabel();
										link.Left = left;
										link.Top = top;
										link.Text = text;
										link.AutoSize = true;
                                        link.LinkClicked += 
                                              (sender, e)=> { 
                                                                if (onClick != null) 
                                                                    O2Thread.mtaThread(()=> onClick()); 
                                                            };
                                        control.Controls.Add(link);
                                        return link;
                                     });

        }
        public static LinkLabel append_Link(this Control control, string text, MethodInvoker onClick)
        {
            return control.Parent.add_Link(text, control.Top, control.Left + control.Width + 5, onClick);
        }
        public static void click(this LinkLabel linkLabel)
        {
            var e = new LinkLabelLinkClickedEventArgs((LinkLabel.Link)(linkLabel.prop("FocusLink")));
            linkLabel.invoke("OnLinkClicked", e);
        }
        public static List<LinkLabel> links(this Control control)
		{
			return control.controls<LinkLabel>(true);
		}		
		public static LinkLabel link(this Control control, string text)
		{
			foreach(var link in control.links())
				if (link.get_Text() == text)
					return link;
			return null;
		}		
		public static LinkLabel onClick(this LinkLabel link, Action callback)
		{
			link.invokeOnThread(	
				()=>{
						link.Click += (sender,e) => callback();
					});
			return link;
		}
    }

    public static class WinForms_ExtensionMethods_ToolStrip
    {
        public static T item<T>(this ToolStrip toolStrip)
            where T : ToolStripItem
        {
            foreach (var item in toolStrip.items())
                if (item is T)
                    return (T)item;
            return null;
        }
        public static T item<T>(this ToolStrip toolStrip, string text)
            where T : ToolStripItem
        {
            var item = toolStrip.item(text);
            if (item.notNull() && item is T)
                return (T)item;
            return null;
        }

        public static ToolStrip add_ToolStrip(this Control control)
        {
            return control.add_Control<ToolStrip>();
        }
        public static ToolStrip toolStrip(this ToolStripItem toolStripItem)
        {
            if (toolStripItem.isNull())
            {
                "[ToolStripItem] in toolStrip() provided toolStripItem was null".error();
                //debug.@break();
                return null;
            }
            return toolStripItem.Owner;
        }
        public static ToolStripItem item(this ToolStrip toolStrip, string text)
        {
            return toolStrip.items().where((item) => item.str() == text).first();
        }
        public static List<ToolStripItem> items(this ToolStrip toolStrip)
        {
            //return (from item in toolStrip.Items			//doesn't work
            //		select item).toList();
            var items = new List<ToolStripItem>();
            foreach (ToolStripItem item in toolStrip.Items)
                items.add(item);
            return items;
        }
        public static ToolStrip clearItems(this ToolStrip toolStrip)
        {
            return (ToolStrip)toolStrip.invokeOnThread(
                () =>
                {
                    toolStrip.Items.Clear();
                    return toolStrip;
                });
        }
        public static T add_Control<T>(this ToolStripItem toolStripItem)
            where T : ToolStripItem
        {
            return toolStripItem.add_Control(default(Action<T>));
        }
        public static T add_Control<T>(this ToolStripItem toolStripItem, Action<T> onCtor)
            where T : ToolStripItem
        {
            return toolStripItem.toolStrip().add_Control(onCtor);
        }
        public static T add_Control<T>(this ToolStrip toolStrip)
            where T : ToolStripItem
        {
            return toolStrip.add_Control(default(Action<T>));
        }
        public static T add_Control<T>(this ToolStrip toolStrip, Action<T> onCtor)
            where T : ToolStripItem
        {
            return (T)toolStrip.invokeOnThread(
                () =>
                {
                    var item = (T)typeof(T).ctor();
                    if (toolStrip.Items.IsReadOnly)
                    {
                        "[ToolStrip][add_Control] Items collection is in ReadOnly mode".error();
                        return null;
                    }
                    else
                    {
                        toolStrip.Items.Add(item);
                        if (onCtor.notNull())
                            onCtor(item);
                        return item;
                    }
                });
        }
        public static ToolStripSeparator add_Separator(this ToolStripItem toolStripItem)
        {
            return toolStripItem.toolStrip().add_Control<ToolStripSeparator>();
        }
        public static ToolStripLabel add_Label(this ToolStripItem toolStripItem, string text)
        {
            return toolStripItem.toolStrip().add_Label(text);
        }

        public static ToolStripLabel add_Label(this ToolStrip toolStrip, string text)
        {
            return toolStrip.add_Control<ToolStripLabel>((label) => label.Text = text);
        }

        public static ToolStripButton add_Button_Open(this ToolStripItem toolStripItem, Action onClick)
        {
            return toolStripItem.add_Button_Open("open", onClick);
        }

        public static ToolStripButton add_Button_Open(this ToolStripItem toolStripItem, string text, Action onClick)
        {
            return toolStripItem.add_Button(text, onClick).with_Icon_Open();
        }

        public static ToolStripButton add_Button(this ToolStripItem toolStrip, string text, Action onClick)
        {
            return toolStrip.add_Button(text, null, onClick);
        }

        public static ToolStripButton add_Button(this ToolStrip toolStrip, string text, Action onClick)
        {
            return toolStrip.add_Button(text, null, onClick);
        }

        public static ToolStripButton add_Button(this ToolStripItem toolStripItem, string text, string resourceName, Action onClick)
        {
            return toolStripItem.toolStrip().add_Button(text, resourceName, onClick);
        }

        public static ToolStripButton add_Button(this ToolStrip toolStrip, string text, string resourceName, Action onClick)
        {
            if (toolStrip.isNull())
            {
                "[ToolStripButton][add_Button] provided toolStrip was null".error();
                return null;
            }
            return (ToolStripButton)toolStrip.invokeOnThread(
                () =>
                {
                    var button = new ToolStripButton();
                    toolStrip.Items.Add(button);
                    try
                    {
                        if (resourceName.valid())
                        {
                            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                            button.applyResources(resourceName);
                        }
                        if (text.valid())
                            button.Text = text;

                        button.Click += (sender, e) =>
                        {
                            if (onClick.notNull())
                                onClick();
                        };
                    }
                    catch (Exception ex)
                    {
                        ex.log("inside toolStrip add_Button");
                    }
                    Application.DoEvents();
                    if (button.toolStrip() != toolStrip)
                    {
                        "[ToolStripButton][add_Button] parent was not set, so setting it manuallly".error();
                        //button.prop("Parent", toolStrip);
                        Reflection_ExtensionMethods_Properties.prop("button", "Parent", toolStrip);
                    }
                    return button;
                });
        }
        public static ToolStripTextBox add_TextBox(this ToolStripItem toolStripItem, string text)
        {
            return toolStripItem.toolStrip().add_TextBox(text);
        }

        public static ToolStripTextBox add_TextBox(this ToolStrip toolStrip, string text)
        {
            return (ToolStripTextBox)toolStrip.invokeOnThread(
                () =>
                {
                    var textBox = new ToolStripTextBox();
                    toolStrip.Items.Add(textBox);
                    textBox.Text = text;
                    return textBox;
                });
        }

        //public static ToolStripButton add_Link(this ToolStrip toolStrip, string text, string resourceName = null, Action onClick = null)

        //Events
        public static ToolStripButton click(this ToolStripButton button)
        {
            return (ToolStripButton)button.toolStrip().invokeOnThread(
                () =>
                {
                    button.PerformClick();
                    return button;
                });
        }

        public static string get_Text(this ToolStripControlHost toolStripControlHost)
        {
            return (string)toolStripControlHost.toolStrip().invokeOnThread(() => toolStripControlHost.Text);
        }



        //Prob:
        // if I use the original: 
        //   	public static T onKeyPress<T>(this T control, Keys onlyFireOnKey, Action<String> callback) where T : Control
        // I get the error:
        //    [12:02:19 PM] ERROR: [CSharp_FastCompiler] Compilation Error: 32::3::CS0311::The type 'System.Windows.Forms.ToolStripTextBox' cannot be used as type parameter 'T' in the generic type or method 'O2.DotNetWrappers.ExtensionMethods.WinForms_ExtensionMethods_Control_Object.onKeyPress<T>(T, System.Windows.Forms.Keys, System.Action<string>)'. There is no implicit reference conversion from 'System.Windows.Forms.ToolStripTextBox' to 'System.Windows.Forms.Control'.::c:\Users\o2\AppData\Local\Temp\gxrzax0r.0.cs
        // but it I use: 
        //  	public static T _onKeyPress<T>(this T toolStripControlHost, Keys onlyFireOnKey, Action<String> callback) where T : ToolStripControlHost
        // I get the error:
        //    [12:04:09 PM] ERROR: [CSharp_FastCompiler] Compilation Error: 32::3::CS0121::The call is ambiguous between the following methods or properties: 'O2.DotNetWrappers.ExtensionMethods.WinForms_ExtensionMethods_Control_Object.onKeyPress<System.Windows.Forms.ToolStripTextBox>(System.Windows.Forms.ToolStripTextBox, System.Windows.Forms.Keys, System.Action<string>)' and 'O2.XRules.Database.Utils._Extra_extensionMethods_ToolStrip.onKeyPress<System.Windows.Forms.ToolStripTextBox>(System.Windows.Forms.ToolStripTextBox, System.Windows.Forms.Keys, System.Action<string>)'::c:\Users\o2\AppData\Local\Temp\pgzwszmn.0.cs
        //
        // so the solution is to hard-code the onEnter and onKeyPress calls to ToolStripTextBox (which is not ideal)

        public static ToolStripTextBox onEnter(this ToolStripTextBox toolStripTextBox, Action<String> callback)
        {
            return toolStripTextBox.onKeyPress(Keys.Enter, callback);
        }

        public static ToolStripTextBox onKeyPress(this ToolStripTextBox toolStripTextBox, Keys onlyFireOnKey, Action<String> callback)
        {

            toolStripTextBox.KeyDown += (sender, e) =>
            {
                if (e.KeyData == onlyFireOnKey)
                {
                    callback(toolStripTextBox.get_Text());
                    e.SuppressKeyPress = true;
                }
            };
            return toolStripTextBox;
        }	

        public static T applyResources<T>(this T toolStripItem, string name)
            where T : ToolStripItem
        {
            toolStripItem.toolStrip().applyResources(toolStripItem, name);
            return (T)toolStripItem;
        }

        public static T applyResources<T>(this T control, ToolStripItem toolStripItem, string name)
            where T : Control
        {
            if (control.isNull())
                return control;
            try
            {
                control.componentResourceManager()
                       .ApplyResources(toolStripItem, name);
            }
            catch
            {
                control.Parent.applyResources(toolStripItem, name);
            }
            return (T)control;
        }

        public static T with_Icon_Open<T>(this T toolStripItem) where T : ToolStripItem
        {
            return toolStripItem.with_Icon(typeof(ascx_FindingsViewer), "btOpenFile");
        }

        public static T with_Icon_Save<T>(this T toolStripItem) where T : ToolStripItem
        {
            return toolStripItem.with_Icon(typeof(ascx_FindingsViewer), "btSave");
        }

        public static T with_Icon<T>(this T toolStripItem, Type hostType, string name) where T : ToolStripItem
        {
            if (toolStripItem.isNull())
                "[toolStripItem][with_Icon]: provided toolStripItem value was null".error();
            else
                hostType.componentResourceManager().ApplyResources(toolStripItem, name);
            return toolStripItem;
        }

        public static ToolStripLabel visible(this ToolStripLabel toolStripLabel, bool value) 
        {
            return toolStripLabel.toolStrip().invokeOnThread(() =>
                {
                        toolStripLabel.Visible = value;
                        return toolStripLabel;
                });
        }
    }

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

    public static class WinForms_ExtensionMethods_MenuStrip
    {             
        public static MenuStrip add_Menu(this Form form)
        {
            return (MenuStrip)form.invokeOnThread(
                () =>
                {
                    var menuStrip = new MenuStrip();
                    form.Controls.Add(menuStrip);
                    form.MainMenuStrip = menuStrip;
                    return menuStrip;
                });
        }
        public static ToolStripMenuItem add_MenuItem(this MenuStrip menuStrip, string text)
        {
            return menuStrip.add_MenuItem(text, null);
        }
        public static ToolStripMenuItem add_MenuItem(this MenuStrip menuStrip, string text, MethodInvoker callback)
        {
            return (ToolStripMenuItem)menuStrip.invokeOnThread(
                () =>
                {
                    var fileMenuItem = new ToolStripMenuItem();
					fileMenuItem.Text = text;
                    menuStrip.Items.Add(fileMenuItem);
                    if (callback != null)
                        menuStrip.Click += (sender, e) => callback();
                    return fileMenuItem;
                });
        }
        public static ContextMenuStrip add_MenuItem(this ContextMenuStrip contextMenu, string text, bool dummyValue, MethodInvoker onClick)
        {
            // since we can't have two different return types the dummyValue is there for the cases where we want to get the reference to the 
            // ContextMenuStrip and not the menu item created

            if (dummyValue.isFalse())
                "invalid value in ContextMenuStrip add_MenuItem, only true creates the expected behaviour".error();
            contextMenu.add_MenuItem(text, onClick);
            return contextMenu;
        }
    }

    public static class WinForms_ExtensionMethods_Panel
    {
        public static Panel add_Panel(this Control control, bool clear)
        {
            if (clear)
                control.clear();
            return control.add_Panel();
        }

        public static Panel add_Panel(this Control control)
        {
            return control.add_Control<Panel>();
        }
    }

    public static class WinForms_ExtensionMethods_PictureBox
    {             
        public static PictureBox add_Image(this Control control)
        {
            return control.add_PictureBox();
        }
        public static PictureBox add_Image(this Control control, string pathToImage)
        {
            return control.add_PictureBox(pathToImage);
        }
        public static PictureBox add_PictureBox(this Control control)
        {
            return control.add_PictureBox(-1, -1);
        }
        public static PictureBox add_PictureBox(this Control control, int top, int left)
        {
            return (PictureBox)control.invokeOnThread(
                                   ()=>{
											var pictureBox = new PictureBox();
												pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
                                           	if (top == -1 && left == -1)
                                               pictureBox.fill();
                                           	else
                                           	{
                                               if (top > -1)
                                                   pictureBox.Top = top;
                                               if (left > -1)
                                                   pictureBox.Left = left;
                                           	}
                                           	control.Controls.Add(pictureBox);
                                           	return pictureBox;
                                        });
        }
        public static PictureBox load(this PictureBox pictureBox, Image image)
        {
            pictureBox.BackgroundImage = image;
            return pictureBox;
        }
        public static PictureBox add_PictureBox(this Control control, string pathToImage)
        {
            var pictureBox = control.add_PictureBox();
            return pictureBox.load(pathToImage);
        }
        public static PictureBox show(this PictureBox pictureBox, string pathToImage)
        {
            return pictureBox.load(pathToImage);
        }
        public static PictureBox open(this PictureBox pictureBox, string pathToImage)
        {
            return pictureBox.load(pathToImage);
        }
        public static PictureBox load(this PictureBox pictureBox, string pathToImage)
        {
            if (pathToImage.fileExists())
            {
                var image = Image.FromFile(pathToImage);
                pictureBox.load(image);
                return pictureBox;
            }
            return null;
        }
        public static PictureBox loadFromUri(this PictureBox pictureBox, Uri uri)
        {
            "loading image from Uri into PictureBox".debug();
            pictureBox.Image = uri.getImageAsBitmap();
            return pictureBox;
        }

        public static PictureBox onClick(this PictureBox pictureBox, Action callback)
        {
            pictureBox.Click += (sender, e) => callback();
            return pictureBox;
        }

        public static PictureBox onDoubleClick(this PictureBox pictureBox, Action callback)
        {
            pictureBox.DoubleClick += (sender, e) => callback();
            return pictureBox;
        }
    }

    public static class WinForms_ExtensionMethods_ProgressBar
    { 
        // order of params is: int top, int left, int width, int height)
        public static ProgressBar add_ProgressBar(this Control control, params int[] position)
        {
            if (control.notNull())
                return control.add_Control<ProgressBar>(position);
            return null;
        }
        public static ProgressBar maximum(this ProgressBar progressBar, int value)
        {
            if (progressBar.notNull())
                progressBar.invokeOnThread(() => progressBar.Maximum = value);
            return progressBar;
        }
        public static ProgressBar value(this ProgressBar progressBar, int value)
        {
            if (progressBar.notNull())
                progressBar.invokeOnThread(() => progressBar.Value = value);
            return progressBar;
        }
        public static ProgressBar increment(this ProgressBar progressBar, int value)
        {
            if (progressBar.notNull())
                progressBar.invokeOnThread(() => progressBar.Increment(value));
            return progressBar;
        }
    }

    public static class WinForms_ExtensionMethods_PropertyGrid
    {
        public static void showInfo(this object _object)
        {
            "Property Grid".popupWindow(300, 300)
                           .add_Control<ascx_ShowInfo>().show(_object);            
        }

        public static PropertyGrid add_PropertyGrid(this Control control)
        {
            return control.add_Control<PropertyGrid>();
        }
        public static void show(this PropertyGrid propertyGrid, Object _object)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.SelectedObject = _object);
        }
        public static PropertyGrid loadInPropertyGrid(this object objectToLoad)
        {
            var propertyGrid = new PropertyGrid();
            propertyGrid.show(objectToLoad);
            return propertyGrid;
        }
        public static PropertyGrid toolBarVisible(this PropertyGrid propertyGrid, bool value)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.ToolbarVisible = value);

            return propertyGrid;
        }
        public static PropertyGrid helpVisible(this PropertyGrid propertyGrid, bool value)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.HelpVisible = value);
            return propertyGrid;
        }
        public static PropertyGrid sort_Alphabetical(this PropertyGrid propertyGrid)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.PropertySort = PropertySort.Alphabetical);
            return propertyGrid;
        }
        public static PropertyGrid sort_Categorized(this PropertyGrid propertyGrid)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.PropertySort = PropertySort.Categorized);
            return propertyGrid;
        }
        public static PropertyGrid sort_CategorizedAlphabetical(this PropertyGrid propertyGrid)
        {
            propertyGrid.invokeOnThread(() => propertyGrid.PropertySort = PropertySort.CategorizedAlphabetical);
            return propertyGrid;
        }
        public static PropertyGrid onValueChange(this PropertyGrid propertyGrid, Action callback)
		{
			propertyGrid.invokeOnThread(()=>propertyGrid.PropertyValueChanged+=(sender,e)=>callback() );
			return propertyGrid;
		}

    }

    public static class WinForms_ExtensionMethods_RichTextBox
    { 
        public static RichTextBox add_RichTextBox(this Control control)
        {
            return control.add_RichTextBox("");
        }
        public static RichTextBox add_RichTextBox(this Control control, string text)
        {
            return (RichTextBox) control.invokeOnThread(
                                     ()=>{
                                            var richTextBox = new RichTextBox();
											richTextBox.Text = text;
											richTextBox.Dock = DockStyle.Fill;
                                            control.Controls.Add(richTextBox);
                                            return richTextBox;
                                         });
        }
        public static RichTextBox set_Text(this RichTextBox richTextBox, string contents)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                    {
                        richTextBox.SuspendLayout();
                        richTextBox.Text = contents;
                        richTextBox.ResumeLayout();
                        return richTextBox;
                    });
            
        }
        public static RichTextBox set_Rtf(this RichTextBox richTextBox, string contents)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                {
                    try
                    {
                        richTextBox.Rtf = contents;
                    }
                    catch
                    {
                        richTextBox.Text = contents;
                    }
                    return richTextBox;
                });

        }
        public static RichTextBox append_Line(this RichTextBox richTextBox, string contents)
        {
            return (RichTextBox) richTextBox.invokeOnThread(
                () =>
                    {
                        richTextBox.append_Text(Environment.NewLine + contents);
                       return richTextBox;
                    });
            
        }
        public static RichTextBox append_Text(this RichTextBox richTextBox, string contents)
        {
            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                    {
                        if (contents!= null)
                            richTextBox.AppendText(contents);
                        return richTextBox;
                    });            
        }
        public static RichTextBox textColor(this RichTextBox richTextBox, Color color)
        {
            return (RichTextBox) richTextBox.invokeOnThread(
                                     () =>
                                         {
                                             richTextBox.ForeColor = color;
                                             return richTextBox;
                                         });
        }
        public static string get_Text(this RichTextBox richTextBox)
        {
            return (string)richTextBox.invokeOnThread(() => richTextBox.Text);
        }
        public static string get_Rtf(this RichTextBox richTextBox)
        {
            return (string)richTextBox.invokeOnThread(() => richTextBox.Rtf);
        }
        public static RichTextBox insertText(this RichTextBox richTextBox, string textToInsert)
        {

            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                {
                    richTextBox.SelectionLength = 0;
                    richTextBox.SelectedText = textToInsert;
                    return richTextBox;
                });
        }
        public static RichTextBox replaceText(this RichTextBox richTextBox, string textToFind, string textToInsert)
        {

            return (RichTextBox)richTextBox.invokeOnThread(
                () =>
                {
                    var selectionStart = richTextBox.SelectionStart;
                    richTextBox.Rtf = richTextBox.Rtf.Replace(textToFind, textToInsert);
                    richTextBox.SelectionStart = selectionStart;            // put the cursor roughly about where it was
                    return richTextBox;
                });
        }
        public static RichTextBox scrollToCaret(this RichTextBox richTextBox)
		{
			return (RichTextBox)richTextBox.invokeOnThread(
						()=>{
								richTextBox.ScrollToCaret();
								return richTextBox;
							});						
		}		
		public static RichTextBox scrollToEnd(this RichTextBox richTextBox)
		{
			return (RichTextBox)richTextBox.invokeOnThread(
						()=>{
								richTextBox.SelectionStart = richTextBox.get_Text().size()-1;
								richTextBox.ScrollToCaret();
								return richTextBox;
							});						
		}
		public static RichTextBox scrollToStart(this RichTextBox richTextBox)
		{
			return (RichTextBox)richTextBox.invokeOnThread(
						()=>{
								richTextBox.SelectionStart = 0;
								richTextBox.ScrollToCaret();
								return richTextBox;
							});						
		}		
		public static RichTextBox wordWrap(this RichTextBox richTextBox, bool value)
		{
			return (RichTextBox)richTextBox.invokeOnThread(
						()=>{
								richTextBox.WordWrap = value;
								return richTextBox;
							});						
		}		
		public static RichTextBox hideSelection(this RichTextBox richTextBox, bool value)
		{
			return (RichTextBox)richTextBox.invokeOnThread(
						()=>{
								richTextBox.HideSelection = value;
								return richTextBox;
							});						
		}		
		public static RichTextBox hideSelection(this RichTextBox richTextBox)
		{
			return richTextBox.hideSelection(true);
		}		
		public static RichTextBox showSelection(this RichTextBox richTextBox)
		{
			return richTextBox.hideSelection(false);
		}	
    }

    public static class WinForms_ExtensionMethods_SplitContainer
    { 
        public static SplitContainer add_SplitContainer(this Control control)
        {
            return add_SplitContainer(control, false, false, false);
        }
        public static SplitContainer add_SplitContainer(this Control control, bool setOrientationToVertical, bool setDockStyleoFill, bool setBorderStyleTo3D)        
        {
            return add_SplitContainer(
                control,
                (setOrientationToVertical) ? Orientation.Vertical : Orientation.Horizontal,
                setDockStyleoFill,
                setBorderStyleTo3D);
        }       
        public static SplitContainer add_SplitContainer(this Control control, Orientation orientation, bool setDockStyleToFill, bool setBorderStyleTo3D)
        {
            return (SplitContainer) control.invokeOnThread(
                                        () =>
                                            {
                                                var splitContainer = new SplitContainer();
                                                splitContainer.Orientation = orientation;
                                                splitContainer.minimumSize(1);                                                
                                                if (setDockStyleToFill)
                                                    splitContainer.Dock = DockStyle.Fill;
                                                if (setBorderStyleTo3D)
                                                    splitContainer.BorderStyle = BorderStyle.Fixed3D;
                                                control.Controls.Add(splitContainer);
                                                return splitContainer;
                                            });
        }
        public static SplitContainer border3D(this SplitContainer control)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {
                        control.BorderStyle = BorderStyle.Fixed3D;
                        return control;
                    });
        }
        public static SplitContainer borderNone(this SplitContainer control)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {
                        control.BorderStyle = BorderStyle.None;
                        return control;
                    });
        }        
        public static SplitContainer fixedPanel1(this SplitContainer control)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                    {
                        control.FixedPanel = FixedPanel.Panel1;
                        return control;
                    });
        }
        public static SplitContainer fixedPanel2(this SplitContainer control)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                {
                    control.FixedPanel = FixedPanel.Panel2;
                    return control;
                });
        }
        public static SplitContainer panel2Collapsed(this SplitContainer control, bool value)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                {
                    control.Panel2Collapsed = value;
                    return control;
                });
        }
        public static SplitContainer panel1Collapsed(this SplitContainer control, bool value)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                {
                    control.Panel1Collapsed = value;
                    return control;
                });
        }
        public static SplitContainer distance(this SplitContainer control, int value)
        {
            return control.splitterDistance(value);
        }
        //DCFIX: check if value is > 0
        public static SplitContainer splitterDistance(this SplitContainer control, int value)
        {
            return (SplitContainer)control.invokeOnThread(
                () =>
                {   
                    control.SplitterDistance = value;
                    return control;
                });
        }
        public static T              splitterDistance<T>(this T control, int distance)			where T : Control
		{
			var splitContainer = control.splitContainer();
			if (splitContainer.notNull())
				splitterDistance(splitContainer,distance);
			return control;
		}
        public static SplitContainer verticalOrientation(this SplitContainer splitContainer)
        {
            splitContainer.invokeOnThread(() => splitContainer.Orientation = Orientation.Vertical);
            return splitContainer;
        }
        public static SplitContainer horizontalOrientation(this SplitContainer splitContainer)
        {
            splitContainer.invokeOnThread(() => splitContainer.Orientation = Orientation.Horizontal);
            return splitContainer;
        }
        public static SplitContainer minimumSize(this SplitContainer splitContainer, int size)
        {
            splitContainer.invokeOnThread(
                () =>
                {
                    splitContainer.Panel1MinSize = size;
                    splitContainer.Panel2MinSize = size;
                });
            return splitContainer;
        }       
 		public static SplitContainer splitContainer(this Control control)
		{
			return control.parent<SplitContainer>();
		}		
		public static SplitContainer splitterWidth(this SplitContainer splitContainer, int value)
		{
			splitContainer.invokeOnThread(()=> splitContainer.SplitterWidth = value);
			return splitContainer;
		}		
		public static SplitContainer splitContainerFixed(this Control control)
		{
			return control.splitContainer().isFixed(true);
		}		
		public static SplitContainer @fixed(this SplitContainer splitContainer, bool value)
		{
			return 	splitContainer.isFixed(value);
		}		
		public static SplitContainer isFixed(this SplitContainer splitContainer, bool value)
		{
			splitContainer.invokeOnThread(()=> splitContainer.IsSplitterFixed = value);
			return splitContainer;
		}

    }

    public static class WinForms_ExtensionMethods_TabControl
    { 
        public static TabControl add_TabControl(this Control control)
        {
            return (TabControl) control.invokeOnThread(
                                    () =>
                                        {
                                            var tabControl = new TabControl();
											tabControl.Dock = DockStyle.Fill;
                                            control.Controls.Add(tabControl);
                                            return tabControl;
                                        });
        }
        public static TabPage add_Tab(this TabControl tabControl, string tabTitle)
        {
            return (TabPage) tabControl.invokeOnThread(
                                 () =>
                                     {
                                         var tabPage = new TabPage();
                                         tabPage.Text = tabTitle;
                                         tabControl.TabPages.Add(tabPage);
                                         return tabPage;
                                     });
        }
        public static TabPage onSelected(this TabPage tabPage, MethodInvoker callback)
        {
            if (callback != null)
            {
                tabPage.invokeOnThread(() =>
                    {
                        var tabControl = tabPage.parent<TabControl>();
                        tabControl.SelectedIndexChanged +=
                            (sender, e) =>
                            {
                                if (tabControl.SelectedTab == tabPage)
                                    callback();
                            };
                        // handle the case where the tabPage is the current selected tab
                        if (tabControl.SelectedTab == tabPage)
                            callback();
                    });
            }
            return tabPage;
        }
        public static TabControl remove_Tab(this TabControl tabControl, TabPage tabPage)
        {
            return (TabControl)tabControl.invokeOnThread(
                () =>
                {
                    tabControl.TabPages.Remove(tabPage);
                    return tabControl;
                });
        }
        public static TabControl select_Tab(this TabControl tabControl, TabPage tabPage)
        {
            return (TabControl)tabControl.invokeOnThread(
                () =>
                {
                    tabControl.SelectedTab = tabPage;
                    return tabControl;
                });
        }
        public static TabControl remove_Tab(this TabControl tabControl, string text)
		{
			var tabToRemove = tabControl.tab(text);
			if (tabToRemove.notNull())
				tabControl.remove_Tab(tabToRemove);
			return tabControl;
		}		
		public static bool has_Tab(this TabControl tabControl, string text)
		{
			return tabControl.tab(text).notNull();
		}		
		public static TabPage tab(this TabControl tabControl, string text)
		{
			foreach(var tab in tabControl.tabs())
				if (tab.get_Text() == text)
					return tab;
			return null;
		}
		public static List<TabPage> tabs(this TabControl tabControl)
		{
			return tabControl.tabPages();
		}		
		public static List<TabPage> tabPages(this TabControl tabControl)
		{
			return (List<TabPage>)tabControl.invokeOnThread(
									()=>{
											var tabPages = new List<TabPage>();
											foreach(TabPage tabPage in tabControl.TabPages)
												tabPages.Add(tabPage);
											return tabPages;											
										});
		}
        public static TabControl selectedIndex(this TabControl tabControl, int index)
		{
			return (TabControl)tabControl.invokeOnThread(
											()=>{
													tabControl.SelectedIndex = index;
													return tabControl;
												});
		}

    }

    public static class WinForms_ExtensionMethods_ToolStripStatus
    {    
        public static ToolStripStatusLabel add_StatusStrip(this UserControl _control)
        {
            return _control.add_StatusStrip(Color.LightGray);
        }
        public static ToolStripStatusLabel add_StatusStrip(this ContainerControl containerControl, Color backColor)
        {
            return (ToolStripStatusLabel)containerControl.invokeOnThread(
                () =>
                {
                    var label = new ToolStripStatusLabel();
                    label.Spring = true;
                    var statusStrip = new StatusStrip();
                    if (backColor != null)
                        statusStrip.BackColor = backColor;
                    statusStrip.Items.Add(label);
                    containerControl.Controls.Add(statusStrip);
                    return label;
                });
        }
        public static ToolStripStatusLabel add_StatusStrip(this Control control)
        {
            if (control is UserControl)
                return (control as UserControl).add_StatusStrip(Color.FromName("Control"));
            return null;
        }
        public static ToolStripStatusLabel add_StatusStrip(this System.Windows.Forms.Form form, bool spring)
        {
            var statusStrip = form.add_StatusStrip();
            form.invokeOnThread(() => statusStrip.Spring = spring); // make it align left	
            return statusStrip;
        }     
        public static ToolStripStatusLabel add_StatusStrip(this System.Windows.Forms.Form form)
        {
            return form.add_StatusStrip(Color.FromName("Control"));
        }				
        public static ToolStripStatusLabel set_Text(this ToolStripStatusLabel label, string message)
        {
            //return (ToolStripStatusLabel)label.invokeOnThread(
            // 	()=>{
            if (label.notNull())
                label.Text = message;
            return label;
            //		});
        }
        public static string get_Text(this ToolStripStatusLabel label)
        {
            return label.Text;
        }        
        public static ToolStripStatusLabel textColor(this ToolStripStatusLabel label,Control hostControl, Color color)
        {
            // I have to provide an hostControl so that I can get the running Thread since ForeColor doesn't seem to be Thread safe
            return (ToolStripStatusLabel)hostControl.invokeOnThread(
                    () =>
                    {
                        if (label.IsLink)
                            label.LinkColor = color;
                        else
                            label.ForeColor = color;
                        return label;
                    });
        }
        public static object tag(this Panel panel)
        {
            return (object)panel.invokeOnThread(() => panel.Tag);
        }
        public static T tag<T>(this Panel panel)
        {
            return (T)panel.invokeOnThread(
                () =>
                {
                    var tag = panel.Tag;
                    if (tag is T)
                        return (T)tag;
                    return default(T);
                });
        }
        public static Panel tag(this Panel panel, object tag)
        {
            panel.invokeOnThread(() => panel.Tag = tag);
            return panel;
        }
    }

    public static class WinForms_ExtensionMethods_ToolStripTextBox
    { 
        public static ToolStripTextBox add_TextBox(this ContextMenuStrip contextMenu, string text)
        {
            var textBox = new ToolStripTextBox();
            textBox.Text = text;
            contextMenu.Items.Add(textBox);
            return textBox;
        }
        public static ToolStripTextBox add_TextBox(this ToolStripMenuItem menuItem, string text)
        {
            var textBox = new ToolStripTextBox();
            textBox.Text = text;
            menuItem.DropDownItems.Add(textBox);
            return textBox;
        }
        public static string get_Text(this ToolStripTextBox textBox)
        {
            return textBox.Text;
        }
        public static ToolStripTextBox set_Text(this ToolStripTextBox textBox, string text)
        {
            textBox.Text = text;
            return textBox;
        }
        public static ToolStripTextBox width(this ToolStripTextBox textBox, int width)
        {
            textBox.Width = width;
            textBox.TextBox.Width = width;
            return textBox;
        }
    }

    public static class WinForms_ExtensionMethods_GuiHelpers
    { 
        public static T     add_LabelAndTextAndButton<T>(this T control, string labelText, string textBoxText, string buttonText, Action<string> onButtonClick)            where T : Control
        {
            //create controls
            var label = control.add_Label(labelText);
            var textBox = label.append_Control<TextBox>();
            var button = textBox.append_Control<System.Windows.Forms.Button>();

            //set text (the label needs to set on the ctor so that the append_Control puts the textbox on its right
            textBox.set_Text(textBoxText);
            button.set_Text(buttonText);

            //position controls
            button.anchor_TopRight();
            button.left(control.width() - button.width());
            textBox.align_Right(control);
            textBox.width(textBox.width() - button.width());

            //final tweaks
            label.topAdd(3);
            textBox.widthAdd(-5);
            button.widthAdd(-2);
            button.heightAdd(-2);

            //events
            button.onClick(() => onButtonClick(textBox.get_Text()));
            textBox.onEnter((text) => onButtonClick(text));
            return control;
        }
        public static T     add_LabelAndComboBoxAndButton<T>(this T control, string labelText, string comboBoxText, string buttonText, Action<string> onButtonClick)            where T : Control
        {
            //create controls
            var label = control.add_Label(labelText);
            var comboBox = label.append_Control<ComboBox>();
            var button = comboBox.append_Control<System.Windows.Forms.Button>();

            //set text (the label needs to set on the ctor so that the append_Control puts the textbox on its right
            comboBox.set_Text(comboBoxText);
            button.set_Text(buttonText);

            //position controls
            button.anchor_TopRight();
            button.left(control.width() - button.width());
            comboBox.align_Right(control);
            comboBox.width(comboBox.width() - button.width());

            //final tweaks
            label.topAdd(3);
            comboBox.widthAdd(-5);
            button.widthAdd(-2);
            button.heightAdd(-2);

            Action<String> onNewItem =
                (newItem) =>
                {
                    if (comboBox.items().Contains(newItem).isFalse())
                        comboBox.insert_Item(newItem);
                    onButtonClick(newItem);
                };


            //events
            button.onClick(() => onNewItem(comboBox.get_Text()));
            comboBox.onEnter((text) => onNewItem(text));
            comboBox.onSelection(() => onNewItem(comboBox.get_Text()));
            return control;
        }
    }
    public static class WinForms_ExtensionMethods_TrackBar
    {
        public static TrackBar insert_Above_Slider(this Control control)
		{					
			return control.insert_Above(20).add_TrackBar();
		}
		
		public static TrackBar add_Slider(this Control control)
		{
			return control.add_TrackBar();
		}
		public static TrackBar add_TrackBar(this Control control)
		{
			return control.add_Control<TrackBar>();  
		}
		
		public static TrackBar maximum(this TrackBar trackBar, int value)
		{
			return (TrackBar)trackBar.invokeOnThread(
				()=>{
						trackBar.Maximum = value;
						return trackBar;
					});
		}
		
		public static TrackBar set_Data<T>(this TrackBar trackBar, List<T> data)
		{
			trackBar.Tag = data;  
			trackBar.maximum(data.size());
			return trackBar;
		}
		public static TrackBar onSlide<T>(this TrackBar trackBar, Action<T> onSlide)
		{
			return trackBar.onSlide((index)=>
				{
					var tag = trackBar.Tag;
					if (tag is List<T>)
					{
						var items = (List<T>)tag;
						if (index > items.size()-1)
							"[TrackBar][onSlide] provided index is bigger that items list".error();
						else
							onSlide(items[index]);
					}					
				});				
		}
		
		public static TrackBar onSlide(this TrackBar trackBar, Action<int> onSlideCallback)
		{
			return (TrackBar)trackBar.invokeOnThread(
				()=>{
						trackBar.Scroll+= (sender,e) => onSlideCallback(trackBar.Value);
						return trackBar;
					});
		}
    }

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

    public static class WinForms_ExtensionMethods_ComponentResourceManager
    {
        public static ComponentResourceManager componentResourceManager(this Control control)
        {
            return control.type().componentResourceManager();
        }

        public static ComponentResourceManager componentResourceManager(this Type type)
        {
            return new ComponentResourceManager(type);
        }
    }

    public static class WinForms_ExtensionMethods_MessageBox
    {
        public static DialogResult msgBox(this string message, params string[] formatParameters)
        {
            return message.messageBox(formatParameters);
        }
        public static DialogResult alert(this string message, params string[] formatParameters)
        {
            return message.messageBox(formatParameters);
        }

        public static DialogResult msgbox(this string message, params string[] formatParameters )
        {
            return message.messageBox(formatParameters);
        }

        public static DialogResult messageBox(this string message, params string[] formatParameters)
        {            
            if (formatParameters.size() > 0)
                message = message.format(formatParameters);            
            return MessageBox.Show(message, "O2 MessageBox");
        }
    }
}
;