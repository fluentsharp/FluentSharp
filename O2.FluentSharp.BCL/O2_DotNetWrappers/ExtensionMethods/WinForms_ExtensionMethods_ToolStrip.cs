using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using O2.DotNetWrappers.DotNet;
using O2.Platform.BCL.O2_Views_ASCX;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public class ToolStripCheckBox : ToolStripControlHost
    {
        public ToolStripCheckBox() : base(new CheckBox())
        {
            BackColor = Color.Transparent;
            Margin = new Padding(1, 3, 1, 1);
        }

        public override sealed Color BackColor  // to fix Resharper warning of 'Virtual member call in constructor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
    }

    public static class WinForms_ExtensionMethods_ToolStrip
    {
        public static T item<T>(this ToolStrip toolStrip) where T : ToolStripItem
        {
            return toolStrip.items()
                            .OfType<T>()
                            .FirstOrDefault();
        }

        public static T item<T>(this ToolStrip toolStrip, string text) where T : ToolStripItem
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
            return toolStrip.invokeOnThread(()=>{
                                                    toolStrip.Items.Clear();
                                                    return toolStrip;
                                                });
        }
        public static T add_Control<T>(this ToolStripItem toolStripItem) where T : ToolStripItem
        {
            return toolStripItem.add_Control(default(Action<T>));
        }
        public static T add_Control<T>(this ToolStripItem toolStripItem, Action<T> onCtor) where T : ToolStripItem
        {
            return toolStripItem.toolStrip().add_Control(onCtor);
        }
        public static T add_Control<T>(this ToolStrip toolStrip) where T : ToolStripItem
        {
            return toolStrip.add_Control(default(Action<T>));
        }
        public static T add_Control<T>(this ToolStrip toolStrip, Action<T> onCtor) where T : ToolStripItem
        {
            return toolStrip.invokeOnThread(() =>
                        {
                            var item = (T)typeof(T).ctor();
                            if (toolStrip.Items.IsReadOnly)
                            {
                                "[ToolStrip][add_Control] Items collection is in ReadOnly mode".error();
                                return null;
                            }                    
                            toolStrip.Items.Add(item);
                            if (onCtor.notNull())
                                onCtor(item);
                            return item;                
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
        public static ToolStrip		 add_Label(this ToolStrip toolStrip, string text, ref ToolStripLabel label)
        {
            label = toolStrip.add_Label(text);
            return toolStrip;
        }

        public static ToolStripButton   add_Button_Open (this ToolStripItem toolStripItem, Action onClick)
        {
            return toolStripItem.add_Button_Open("open", onClick);
        }
        public static ToolStripButton   add_Button_Open (this ToolStripItem toolStripItem, string text, Action onClick)
        {
            return toolStripItem.add_Button(text, onClick).with_Icon_Open();
        }
        public static ToolStripButton   add_Button  (this ToolStripItem toolStripItem, string text, Action onClick)
        {
            return toolStripItem.add_Button(text, "", onClick);
        }
        public static ToolStripButton   add_Button  (this ToolStripItem toolStripItem, string text, Image image, Action onClick)
        {
            return toolStripItem.toolStrip().add_Button(text, image, onClick);
        }

        public static ToolStripButton   add_Button  (this ToolStrip toolStrip, string text, Action onClick)
        {
            return toolStrip.add_Button(text, "", onClick);
        }
        public static ToolStripButton   add_Button  (this ToolStripItem toolStripItem, string text, string resourceName, Action onClick)
        {
            return toolStripItem.toolStrip().add_Button(text, resourceName, onClick);
        }
        public static ToolStripButton   add_Button  (this ToolStrip toolStrip, string text, string resourceName, Action onClick)
        {
            return toolStrip.add_Button(text, resourceName, null, onClick);
        }		
        public static ToolStripButton   add_Button  (this ToolStrip toolStrip, string text, Image image, Action onClick)
        {
            return toolStrip.invokeOnThread(
                () =>
                {
                    var newButton = new ToolStripButton(text);
                    newButton.Image = image;
                    newButton.Click += (sender, e) => O2Thread.mtaThread(() => onClick());
                    toolStrip.Items.Add(newButton);
                    return newButton;
                });
        }		//Todo: refactor with one below
        public static ToolStripButton   add_Button  (this ToolStrip toolStrip, string text, string resourceName, Image image, Action onClick)
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
                        if (image.notNull())
                            button.Image = image;
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
        public static ToolStripTextBox  add_TextBox (this ToolStripItem toolStripItem, string text)
        {
            return toolStripItem.toolStrip().add_TextBox(text);
        }
        public static ToolStripTextBox  add_TextBox (this ToolStrip toolStrip, string text)
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
        public static ToolStripButton   click       (this ToolStripButton button)
        {
            return (ToolStripButton)button.toolStrip().invokeOnThread(
                () =>
                {
                    button.PerformClick();
                    return button;
                });
        }
        public static string            get_Text    (this ToolStripControlHost toolStripControlHost)
        {
            return (string)toolStripControlHost.toolStrip().invokeOnThread(() => toolStripControlHost.Text);
        }
        public static ToolStripTextBox  onEnter      (this ToolStripTextBox toolStripTextBox, Action<String> callback)
        {
            return toolStripTextBox.onKeyPress(Keys.Enter, callback);
        }
        public static ToolStripTextBox  onKeyPress   (this ToolStripTextBox toolStripTextBox, Keys onlyFireOnKey, Action<String> callback)
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

        public static T applyResources<T>(this T toolStripItem, string name) where T : ToolStripItem
        {
            toolStripItem.toolStrip().applyResources(toolStripItem, name);
            return toolStripItem;
        }
        public static T applyResources<T>(this T control, ToolStripItem toolStripItem, string name) where T : Control
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
            return control;
        }
        public static T with_Icon_Open<T>(this T toolStripItem) where T : ToolStripItem
        {
            return toolStripItem.with_Icon(FormImages.btOpenFile_Image);
        }
        public static T with_Icon_New<T>(this T toolStripItem) where T : ToolStripItem
        {
            return toolStripItem.with_Icon(FormImages.document_new);
        }
        public static T with_Icon_Save<T>(this T toolStripItem) where T : ToolStripItem
        {
            return toolStripItem.with_Icon(FormImages.btSaveFile_Image);
        }
        public static T with_Icon<T>(this T toolStripItem, Image image) where T : ToolStripItem
        {
            if (toolStripItem.isNull())
                "[toolStripItem][with_Icon]: provided toolStripItem value was null".error();
            else
                toolStripItem.toolStrip().invokeOnThread(() => { toolStripItem.Image = image; });
            return toolStripItem;
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
        public static ToolStrip layoutStyle(this ToolStrip toolStrip, ToolStripLayoutStyle layoutStyle)
        {
            return toolStrip.invokeOnThread(
                () =>
                {
                    toolStrip.LayoutStyle = layoutStyle;
                    return toolStrip;
                });
        }
        public static ToolStrip layout_Flow(this ToolStrip toolStrip)
        {
            return toolStrip.layoutStyle(ToolStripLayoutStyle.Flow);
        }
        public static ToolStrip layout_HorizontalStackWithOverflow(this ToolStrip toolStrip)
        {
            return toolStrip.layoutStyle(ToolStripLayoutStyle.HorizontalStackWithOverflow);
        }
        public static ToolStrip layout_VerticalStackWithOverflow(this ToolStrip toolStrip)
        {
            return toolStrip.layoutStyle(ToolStripLayoutStyle.VerticalStackWithOverflow);
        }
        public static ToolStrip layout_Table(this ToolStrip toolStrip)
        {
            return toolStrip.layoutStyle(ToolStripLayoutStyle.Table);
        }
        public static ToolStrip layout_StackWithOverflow(this ToolStrip toolStrip)
        {
            return toolStrip.layoutStyle(ToolStripLayoutStyle.StackWithOverflow);
        }

        public static ToolStrip insert_ToolStrip(this Control control)
        {
            var tooStrip = control.insert_Above(30).add_ToolStrip();
            return tooStrip.splitContainerFixed();			
        }
        public static ToolStrip insert_Above_ToolStrip(this Control control)
        {
            return control.insert_ToolStrip();
        }
        public static ToolStrip insert_Below_ToolStrip(this Control control)
        {
            var tooStrip = control.insert_Below(30).add_ToolStrip();
            return tooStrip.splitContainerFixed();
        }		
        public static ToolStrip insert_Left_ToolStrip(this Control control)
        {
            var tooStrip = control.insert_Left(40).add_ToolStrip()
                                  .layout_VerticalStackWithOverflow();
            return tooStrip.splitContainerFixed();
        }
        public static ToolStrip insert_Right_ToolStrip(this Control control)
        {
            var tooStrip = control.insert_Right(40).add_ToolStrip()
                                  .layout_VerticalStackWithOverflow();
            return tooStrip.splitContainerFixed();
        }

        public static ToolStrip add_CheckBox(this ToolStrip toolStrip, string text, Action<bool> onValueChange)
        {
            CheckBox checkBox_Ref = null;
            return toolStrip.add_CheckBox(text, ref checkBox_Ref, onValueChange);
        }
        public static ToolStrip add_CheckBox(this ToolStrip toolStrip, string text, ref CheckBox checkBox_Ref)
        {
            return toolStrip.add_CheckBox(text, ref checkBox_Ref, (value) => { });
        }
        public static ToolStrip add_CheckBox(this ToolStrip toolStrip, string text, ref CheckBox checkBox_Ref, Action<bool> onValueChange)
        {
            return toolStrip.add_CheckBox(text, null, ref checkBox_Ref, onValueChange);
        }
        public static ToolStrip add_CheckBox(this ToolStrip toolStrip, string text, Image image, ref CheckBox checkBox_Ref, Action<bool> onValueChange)
        {
            var checkBox = toolStrip.add_Control<ToolStripCheckBox>().Control as CheckBox;
            checkBox_Ref = checkBox;		// need to do this because we can't use the ref object inside the lambda method below
            return toolStrip.invokeOnThread(
                () =>
                {
                    checkBox.Text = text;
                    checkBox.Image = image;
                    checkBox.CheckedChanged += (sender, e) =>
                        O2Thread.mtaThread(() => onValueChange(checkBox.value()));
                    return toolStrip;
                });

        }
        public static ToolStripDropDown add_DropDown(this ToolStrip toolStrip, string text)
        {
            return toolStrip.add_DropDown(text, null);
        }
        public static ToolStripDropDown add_DropDown(this ToolStrip toolStrip, string text, Image image)
        {
            return toolStrip.invokeOnThread(
                () =>
                {
                    var dropDown = new ToolStripDropDown();
                    var menuButton = new ToolStripDropDownButton(text);
                    menuButton.Image = image;
                    menuButton.DropDown = dropDown;
                    toolStrip.Items.Add(menuButton);
                    return dropDown;
                });
        }
        public static ToolStripDropDown add_DropDown_Button(this ToolStripDropDown dropDown, string text, Action onClick)
        {
            return dropDown.add_DropDown_Button(text, null, onClick);
        }
        public static ToolStripDropDown add_DropDown_Button(this ToolStripDropDown dropDown, string text, Image image, Action onClick)
        {
            return dropDown.invokeOnThread(
                () =>
                {
                    var newButton = new ToolStripButton(text);
                    newButton.Click += (sender, e) => O2Thread.mtaThread(() => onClick());
                    newButton.Image = image;
                    dropDown.Items.Add(newButton);
                    return dropDown;
                });
        }


        public static ToolStrip add_TextBox(this ToolStrip toolStrip, string text, ref Action<string> updateTextBox)
        {
            return toolStrip.add_TextBox(text, -1, ref updateTextBox);
        }
        public static ToolStrip add_TextBox(this ToolStrip toolStrip, string text, int width, ref Action<string> updateTextBox)
        {
            var textBox = toolStrip.add_TextBox(text);
            if (width > -1)
                textBox.width(width);
            updateTextBox = (newText) =>
            {
                toolStrip.invokeOnThread(        // this can be removed after the next update of FluentSharp.BCL
                    () =>
                    {
                        textBox.set_Text(newText);
                        return textBox;
                    });
            };
            return toolStrip;
        }
        public static ToolStrip add_TextBox(this ToolStrip toolStrip, string text, Action<string> onTextChange)
        {
            var textBox = toolStrip.add_TextBox(text);
            textBox.TextChanged += (sender, e) =>
            {
                var newText = textBox.get_Text();
                O2Thread.mtaThread(() => onTextChange(newText));
            };
            return toolStrip;
        }
        public static ToolStrip add_Text(this ToolStrip toolStrip, string text)
        {
            toolStrip.add_Button(text, () => { });
            return toolStrip;
        }
        public static ToolStrip add(this ToolStrip toolStrip, string text, Action onClick)
        {
            ToolStripButton button = null;
            return toolStrip.add(text, ref button, onClick);
        }
        public static ToolStrip add(this ToolStrip toolStrip, string text, ref ToolStripButton button, Action onClick)
        {
            button = toolStrip.add_Button(text, onClick);
            return toolStrip;
        }
        public static ToolStrip add(this ToolStrip toolStrip, Image image, Action onClick)
        {
            ToolStripButton button = null;
            return toolStrip.add(image, ref button, onClick);
        }
        public static ToolStrip add(this ToolStrip toolStrip, Image image,  ref ToolStripButton button, Action onClick)
        {
            return toolStrip.add("", image, ref button, onClick);
        }
        public static ToolStrip add(this ToolStrip toolStrip, string text, string imageKey, Action onClick)
        {
            return toolStrip.add(text, imageKey.formImage(), onClick);
        }
        public static ToolStrip add(this ToolStrip toolStrip, string text, Image image, Action onClick)
        {
            ToolStripButton button = null;
            return toolStrip.add(text, image, ref button, onClick);
        }
        public static ToolStrip add(this ToolStrip toolStrip, string text, Image image, ref ToolStripButton button, Action onClick)
        {
            button = toolStrip.add_Button(text, image, onClick);
            return toolStrip;
        }

        public static ToolStrip add_New(this ToolStrip toolStrip, Action onClick)
        {
            toolStrip.add_Button("New", onClick).with_Icon_New();
            return toolStrip;
        }
        public static ToolStrip add_Open(this ToolStrip toolStrip, Action onClick)
        {
            toolStrip.add_Button("Open", onClick).with_Icon_Open();
            return toolStrip;
        }
        public static ToolStrip add_Save(this ToolStrip toolStrip, Action onClick)
        {
            toolStrip.add_Button("Save", onClick).with_Icon_Save();
            return toolStrip;
        }
        public static ToolStrip add_Save(this ToolStrip toolStrip, string text, Action onClick)
        {
            toolStrip.add_Button(text, onClick).with_Icon_Save();
            return toolStrip;
        }
        public static ToolStrip add_Play(this ToolStrip toolStrip, Action onClick)
        {
            return toolStrip.add("Play", "btExecuteSelectedMethod_Image".formImage(), onClick);
        }
        public static ToolStrip add_Play(this ToolStrip toolStrip, string text, Action onClick)
        {
            return toolStrip.add(text, "btExecuteSelectedMethod_Image".formImage(), onClick);
        }
        public static ToolStrip add_Stop(this ToolStrip toolStrip, Action onClick)
        {
            return toolStrip.add("Stop", "process_stop".formImage(), onClick);
        }
        public static ToolStrip add_Copy(this ToolStrip toolStrip, Action onClick)
        {
            return toolStrip.add("Copy", "edit_copy".formImage(), onClick);
        }
        public static ToolStrip add_Cut(this ToolStrip toolStrip, Action onClick)
        {
            return toolStrip.add("Cut", "edit_cut".formImage(), onClick);
        }
        public static ToolStrip add_Refresh(this ToolStrip toolStrip, Action onClick)
        {
            return toolStrip.add("Refresh", "view_refresh".formImage(), onClick);
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
        public static ToolStripStatusLabel textColor(this ToolStripStatusLabel label, Control hostControl, Color color)
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
            return contextMenu.invokeOnThread(
                () =>
                {
                    var textBox = new ToolStripTextBox();
                    textBox.Text = text;
                    contextMenu.Items.Add(textBox);
                    return textBox;
                });
        }
        public static ToolStripTextBox add_TextBox(this ToolStripMenuItem menuItem, string text)
        {
            return menuItem.Owner.invokeOnThread(
                    () =>
                    {
                        var textBox = new ToolStripTextBox();
                        textBox.Text = text;
                        menuItem.DropDownItems.Add(textBox);
                        return textBox;
                    });
        }
        public static string get_Text(this ToolStripTextBox textBox)
        {
            return textBox.Owner.invokeOnThread(
                    () =>
                    {
                        return textBox.Text;
                    });
        }
        public static ToolStripTextBox set_Text(this ToolStripTextBox textBox, string text)
        {
            return textBox.Owner.invokeOnThread(
                    () =>
                    {
                        textBox.Text = text;
                        return textBox;
                    });
        }
        public static ToolStripTextBox width(this ToolStripTextBox textBox, int width)
        {
            return textBox.Owner.invokeOnThread(
                    () =>
                    {
                        textBox.Width = width;
                        textBox.TextBox.Width = width;
                        return textBox;
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
                () =>
                {
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
                    () =>
                    {
                        toolStripButton.Visible = value;
                    });
            return toolStripButton;
        }
    }    

}