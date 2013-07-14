using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using System.Drawing;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Control_Object
    {
        //get
        public static T             get<T>(this Control controlToSearch) where T : Control
        {
            foreach (var control in controlToSearch.controls())
                if (control is T)
                    return (T)control;
                else
                {
                    var childMatch = control.get<T>();
                    if (childMatch != null)
                        return childMatch;
                }
            return null;
        }
        public static T             get<T>(this List<Control> controls) where T : Control
        {
            foreach (Control control in controls)
                if (control.type() == typeof(T))
                    return (T)control;
            return null;
        }
        public static T             castOrCreate<T>(this Control control, object objectToCheck, Func<T> createFunction)
        {
            if (objectToCheck != null && objectToCheck is T)
                return (T)objectToCheck;
            control.clear();
            return createFunction();
        }
        public static Control       parent(this Control control)
        {
            return (Control)control.invokeOnThread(
                () =>
                {
                    if (control.notNull())
                        return control.Parent;
                    return null;
                });
        }
        public static T             parent<T>(this List<Control> controls) where T : Control
        {
            foreach (var control in controls)
            {
                var match = control.parent<T>();
                if (match != null)
                    return (T)match;
            }
            return null;
        }
        public static T             parent<T>(this Control control) where T : Control
        {
            if (control != null && control.Parent != null)
            {
                var parent = control.Parent;
                if (control == parent)
                    "[in parent<T>] the control == control.Parent!! {0}".error(control);
                else
                {
                    if (parent is T)
                        return (T)parent;
                    var match = parent.parent<T>();
                    if (match != null)
                        return (T)match;
                }
            }
            return null;
        }
        public static string        get_Text(this Control control)
        {
            return (string)control.invokeOnThread(() => { return control.Text; });
        }
        public static T             text<T>(this List<T> controls, string textToFind) where T : Control
        {
            foreach (var control in controls)
                if (control.get_Text() == textToFind)
                    return control;
            return null;
        }
        public static List<string>  texts<T>(this List<T> controls)  where T : Control
        {
            return (from control in controls
                    select control.get_Text()).toList();
        }

        //Misc
        public static Form          parentForm(this Control control)
        {
            if (control is Form)
                return (Form)control;
            return control.parent<Form>();
        }
        public static T             set_Text<T>(this T control, string text) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {

                    control.Text = text;
                    return (T)control;
                });
        }
        public static T             createInThread<T>(this Control control) where T : Control
        {
            return control.newInThread<T>();
        }
        public static T             newInThread<T>(this Control control) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    try
                    {
                        return (T)typeof(T).ctor();
                    }
                    catch (Exception ex)
                    {
                        ex.log("in Control.invokeOnThread");
                        return null;
                    }
                });
        }
        public static Form          newFormInThread(this Control control)
        {
            return control.newFormInThread("New Form in thread of: {0}".format(control.get_Text()));
        }
        public static Form          newFormInThread(this Control control, string text)
        {
            return (Form)control.invokeOnThread(
                () =>
                {
                    var newForm = new Form();
                    newForm.set_H2Icon();
                    newForm.Text = text;
                    newForm.Show();
                    return newForm;
                });
        }
        public static T             autoSize<T>(this T control, bool value) where T : Control
        {
            return control.invokeOnThread(
                () =>
                {
                    control.AutoSize = value;
                    return control;
                });
        }
        public static T             fill<T>(this T control) where T : Control
        {
            return control.fill(true);
        }
        public static T             fill<T>(this T control, bool status) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Dock = (status) ? DockStyle.Fill : DockStyle.None;
                    return (T)control;
                });

        }
        public static T             enabled<T>(this T control) where T : Control
        {
            return control.enabled(true);
        }
        public static T             enabled<T>(this T control, bool state) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Enabled = state;
                    return (T)control;
                });
        }
        public static T             visible<T>(this T control) where T : Control
        {
            return control.visible(true);
        }
        public static T             visible<T>(this T control, bool state) where T : Control
        {
            if (control.isNull())
                return null;
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Visible = state;
                    return (T)control;
                });
        }
        public static T             mapToWidth<T>(this Control hostControl, T control, bool alignToTop) where T : Control
        {
            if (alignToTop)
                control.anchor_TopLeftRight();
            else
                control.anchor_BottomLeftRight();

            const int pad = 5;
            control.Left = hostControl.Left + pad;
            control.Width = hostControl.Width - pad - pad;
            return control;
        }
        public static T             close<T>(this T userControl) where T : UserControl
        {
            return userControl.invokeOnThread(
                () =>
                    {
                        userControl.parentForm().Close();                        
                        return userControl;
                });
        }
        public static T             foreColor<T>(this T control, Color color) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.ForeColor = color;
                    return control;
                });
        }
        public static T             backColor<T>(this T control, Color color) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.BackColor = color;
                    return control;
                });
        }
        public static T             backColor<T>(this T control, string colorName) where T : Control
        {
            return control.backColor(Color.FromName(colorName));
        }
        public static List<Control> controls(this Control control)
        {
            return control.controls(false);
        }
        public static List<T>       controls<T>(this Control control, bool recursiveSearch) where T : Control
        {
            return (from childControl in control.controls(recursiveSearch)
                    where childControl is T
                    select (T)childControl).toList();
        }
        public static List<Control> controls(this Control control, bool recursiveSearch)
        {
            if (recursiveSearch)
            {    
                var allControls = control.controls();
                foreach(var childControl in control.controls())
                    allControls.AddRange(childControl.controls(true));
                return allControls;
            }
            else
                return control.Controls.list();
        }
        public static T             controls<T>(this Control rootControl, int depthToReturn) where T : Control
        {
            var controls = rootControl.controls(depthToReturn);
            return controls.control<T>();
        }
        public static List<Control> controls(this Control control, int depthToReturn)
        {
            var allControls = new List<Control>();
            allControls.add(control);
            for (int i = 0; i < depthToReturn; i++)
                allControls = allControls.controls(false);
            return allControls;
        }
        public static List<T>       controls<T>(this List<Control> controls) where T : Control
        {
            return (from control in controls
                    where control is T
                    select (T)control).toList();            
        }
        public static List<Control> controls(this List<Control> controls)
        {
            return controls.controls(false);
        }        
        public static List<Control> controls(this List<Control> controls, bool includeRoot)
        {
            var allControls = new List<Control>();
            foreach (var control in controls)
            {
                if (includeRoot)
                    allControls.add(control);
                allControls.add(control.controls());
            }
            return allControls;
        }
        public static T             controls<T>(this Control control) where T : Control
        {
            foreach (var childControl in control.controls())
                if (childControl is T)
                    return (T)childControl;
            return null;
        }
        public static T             control<T>(this List<Control> controls) where T : Control
        {
            foreach (var control in controls)
                if (control is T)
                    return (T)control;
            return null;
        }
        public static T             control<T>(this Control control) where T : Control
        {
            return control.control<T>(true);
        }
        public static Control       control<T>(this T hostControl, string controlName, bool recursive) where T : Control
        {
            var controls = hostControl.controls(recursive);
            foreach (var control in controls)
                if (control.typeName() == controlName)
                    return control;
            return null;
        }
        public static T             control<T>(this Control control, bool searchRecursively) where T : Control
        {
            try
            {
                if (control.notNull())
                    foreach (var childControl in control.controls(searchRecursively))
                        if (childControl is T)
                            return (T)childControl;
            }
            catch {}
            return null;
        }

        public static List<Control> list(this Control.ControlCollection controlCollection)
        {
            var controls = new List<Control>();
            foreach (Control control in controlCollection)
                controls.Add(control);
            return controls;
        }
        public static IntPtr handle(this Control control)
        {
            try
            {
                return control.invokeOnThread(() => control.Handle);
            }
            catch 
            {
                return IntPtr.Zero;
            }
        }
        public static bool hasHandle(this Control control)
        {
            return control.handle() != IntPtr.Zero;
        }
        public static bool noHandle(this Control control)
        {
            return control.handle() == IntPtr.Zero;
        }
        public static T focus<T>(this T control) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Focus();
                    return control;
                });
        }
        public static T top<T>(this T control, int top) where T : Control
        {
            return (T)control.invokeOnThread(
                    () =>
                    {
                        if (top > -1)
                            control.Top = top;
                        return control;
                    });
        }
        public static T left<T>(this T control, int left) where T : Control
        {
            return (T)control.invokeOnThread(
                    () =>
                    {
                        if (left > -1)
                            control.Left = left;
                        return control;
                    });
        }
        public static T width<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    if (value > -1)
                        control.Width = value;
                    return (T)control;
                });
        }
        public static T height<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    if (value > -1)
                        control.Height = value;
                    return (T)control;
                });
        }
        public static int width(this Control control)
        {
            return (int)control.invokeOnThread(
                () =>
                {
                    return control.Width;
                });
        }
        public static int height(this Control control)
        {
            return (int)control.invokeOnThread(
                () =>
                {
                    return control.Height;
                });
        }
        public static T widthAdd<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.width(control.Width + value);
                    return control;
                });

        }
        public static T heightAdd<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.height(control.Height + value);
                    return control;
                });

        }
        public static T topAdd<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.top(control.Top + value);
                    return control;
                });

        }
        public static T leftAdd<T>(this T control, int value) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.left(control.Left + value);
                    return control;
                });

        }
        public static T putBitmapOnClipboard<T>(this T control, Bitmap bitmap) where T : Control
        {
            control.invokeOnThread(() => Clipboard.SetImage(bitmap));
            return control;
        }
        public static Bitmap fromClipboardGetImage(this Control control)
        {
            return (Bitmap)control.invokeOnThread(
                () =>
                {
                    if (Clipboard.ContainsImage())
                    {
                        return Clipboard.GetImage();
                    }
                    "in fromClipboardGetImage, the Clipboard doesn't contain an image".debug();
                    return null;
                });
        }
        public static T front<T>(this T control) where T : Control
        {
            return control.bringToFront();
        }
        public static T bringToFront<T>(this T control) where T : Control
        {
            control.invokeOnThread(() => control.BringToFront());
            return control;
        }
        public static T back<T>(this T control) where T : Control
        {
            return control.sendToBack();
        }
        public static T sendToBack<T>(this T control) where T : Control
        {
            control.invokeOnThread(() => control.SendToBack());
            return control;
        }
        public static T hide<T>(this T control) where T : Control
        {
            return control.visible(false);
        }
        public static T show<T>(this T control) where T : Control
        {
            return control.visible(true);
        }

        //events 
        public static T onDrop<T, T1>(this T control, Action<T1> onDrop) where T : Control
        {
            control.invokeOnThread(() =>
            {
                control.AllowDrop = true;
                control.DragEnter += (sender, e) => Dnd.setEffect(e);
                control.DragDrop += (sender, e) =>
                {
                    sender.showInfo();
                    e.showInfo();

                    "{0} - {1}".info(sender.typeName(), e.typeName());
                    var data = Dnd.tryToGetObjectFromDroppedObject(e, typeof(T1));
                    if (data.notNull())
                    {
                        onDrop((T1)data);
                    }
                };
            });
            return (T)control;
        }
        public static T onDrop<T>(this T control, Action<string> onDropFileOrFolder) where T : Control
        {
            return (T)control.invokeOnThread(() =>
                {

                    control.AllowDrop = true;
                    control.DragEnter += (sender, e) => Dnd.setEffect(e);
                    control.DragDrop += (sender, e)=>{
                                                         var fileOrFolder = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
                                                         if (fileOrFolder.valid())
                                                         O2Thread.mtaThread(
                                                            () =>{
                                                                  
                                                                        onDropFileOrFolder(fileOrFolder);
                                                                 });
                                                    };
                    return control;
                });
            //
        }
        public static T onHandleCreated<T>(this T control, Action onHandleCreated) where T : Control
        {
            control.HandleCreated += (sender, e) =>
            {
                onHandleCreated();
            };
            return control;
        }        
        public static T onControlAdded<T>(this T control, Action onControlAdded) where T : Control
        {
            control.ControlAdded += (sender, e) =>
            {
                onControlAdded();
            };
            return control;
        }
        public static T onClosed<T>(this T control, MethodInvoker onClosed) where T : Control
        {
            var parentForm = control.parentForm();
            if (parentForm == null)
            {
                "in Control.onClosed, provided form value was null".error();
                return null;
            }
            parentForm.Closed += (sender, e) => onClosed();
            return control;
        }

        //remove
        public static void remove_ContextMenu(this Control control)
        {
            control.ContextMenuStrip = null;
        }
        public static T clear<T>(this T control) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    if (control.Controls.IsReadOnly)
                        "cannot clear control list for type {0} since it is marked as read only".format(control.typeName()).error();
                        control.Controls.Clear();                    
                    return control;
                });
        }
        public static T removeOtherControls<T>(this Control hostControl, T controlToKeep) where T : Control
        {
            return (T)hostControl.invokeOnThread(
                () =>
                {
                    foreach (var control in hostControl.controls())
                        if (control != controlToKeep)
                            hostControl.Controls.Remove(control);
                    return controlToKeep;
                });
        }             
        public static T remove<T>(this T hostControl, Control controlToRemove) where T : Control
        {
            return (T)hostControl.invokeOnThread(
                () =>
                {
                    foreach (var control in hostControl.controls(true))
                        if (control != controlToRemove)
                            hostControl.Controls.Remove(controlToRemove);
                    return hostControl;
                });
        }        

        //location
        public static T location<T>(this T control, int top) where T : Control
        {
            return control.location(top, -1, -1, -1);
        }
        public static T location<T>(this T control, int top, int left) where T : Control
        {
            return control.location(top, left, -1, -1);
        }
        public static T location<T>(this T control, int top, int left, int width) where T : Control
        {
            return control.location(top, left, width, -1);
        }
        public static T location<T>(this T control, int top, int left, int width, int height) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Dock = DockStyle.None;
                    if (top > -1)
                        control.Top = top;
                    if (left > -1)
                        control.Left = left;
                    if (top > -1)
                        control.Top = top;
                    if (width > -1)
                        control.Width = width;
                    if (height > -1)
                        control.Height = height;
                    return control;
                });
        }

        // align
        public static T align_Left<T>(this T control, Control controlToAlignWith) where T : Control
        {
            return control.align_Left(controlToAlignWith, 0);
        }
        public static T align_Left<T>(this T control, Control controlToAlignWith, int border) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.Dock = DockStyle.None;
                    control.Left = controlToAlignWith.Left + border;
                    control.anchor_Left();
                    return control;
                });
        }
        public static T align_Right<T>(this T control, Control controlToAlignWith) where T : Control
        {
            return control.align_Right(controlToAlignWith, 0);
        }
        public static T align_Right<T>(this T control, Control controlToAlignWith, int border)           where T : Control
        {
            return (T)control.invokeOnThread(
                        () =>
                        {
                            control.Dock = DockStyle.None;
                            control.Width = controlToAlignWith.Width - control.Left - border;
                            control.anchor_Right();
                            return control;
                        });
        }
        public static T align_Top<T>(this T control, Control controlToAlignWith) where T : Control
        {
            return control.align_Top(controlToAlignWith, 0);
        }
        public static T align_Top<T>(this T control, Control controlToAlignWith, int border) where T : Control
        {
            return (T)control.invokeOnThread(
                        () =>
                        {
                            control.Dock = DockStyle.None;
                            control.Top = controlToAlignWith.Top + border;
                            control.anchor_Top();
                            return control;
                        });
        }
        public static T align_Bottom<T>(this T control, Control controlToAlignWith) where T : Control
        {
            return control.align_Bottom(controlToAlignWith, 0);
        }
        public static T align_Bottom<T>(this T control, Control controlToAlignWith, int border) where T : Control
        {
            return (T)control.invokeOnThread(
                        () =>
                        {
                            control.Dock = DockStyle.None;
                            PublicDI.log.debug("controlToAlignWith.Height: {0}", controlToAlignWith.Height);
                            PublicDI.log.debug("control.Top: {0}", control.Top);
                            control.Height = controlToAlignWith.Height - control.Top - border;
                            control.anchor_Bottom();
                            return control;
                        });
        }
        
        // anchor
        public static T anchor<T>(this T control) where T : Control
        {
            control.Anchor = AnchorStyles.None;
            return control;
        }
        public static T anchor_Top<T>(this T control) where T : Control
        {
            control.Anchor = control.Anchor | AnchorStyles.Top;
            return control;
        }
        public static T anchor_Bottom<T>(this T control) where T : Control
        {
            control.Anchor = control.Anchor | AnchorStyles.Bottom;
            return control;
        }
        public static T anchor_Left<T>(this T control) where T : Control
        {
            control.Anchor = control.Anchor | AnchorStyles.Left;
            return control;
        }
        public static T anchor_Right<T>(this T control) where T : Control
        {
            control.Anchor = control.Anchor | AnchorStyles.Right;
            return control;
        }        
        public static T anchor_TopLeft<T>(this T control) where T : Control
        {
            control.anchor().anchor_Top().anchor_Left();
            return control;
        }
        public static T anchor_BottomLeft<T>(this T control) where T : Control
        {
            control.anchor().anchor_Bottom().anchor_Left();
            return control;
        }
        public static T anchor_TopRight<T>(this T control) where T : Control
        {
            control.anchor().anchor_Top().anchor_Right();
            return control;
        }
        public static T anchor_BottomRight<T>(this T control) where T : Control
        {
            control.anchor().anchor_Bottom().anchor_Right();
            return control;
        }
        public static T anchor_TopLeftRight<T>(this T control) where T : Control
        {
            control.anchor().anchor_Top().anchor_Left().anchor_Right();
            return control;
        }
        public static T anchor_BottomLeftRight<T>(this T control) where T : Control
        {
            control.anchor().anchor_Bottom().anchor_Left().anchor_Right();
            return control;
        }
        public static T anchor_All<T>(this T control) where T : Control
        {
            control.anchor().anchor_Top().anchor_Right().anchor_Bottom().anchor_Left();
            return control;
        }



        // Key Events
        public static T onKeyPress<T>(this T control, Func<Keys, bool> callback) where T : Control
        {
            control.KeyDown += (sender, e) => e.Handled = callback(e.KeyData);
            return control;
        }  
        public static T onKeyPress<T>(this T control, Action<Keys> callback) where T : Control
        {
            control.KeyUp += (sender, e) => callback(e.KeyData);
            return control;
        }
        public static T onKeyPress<T>(this T control, Action<Keys, String> callback) where T : Control
        {
            control.KeyUp += (sender, e) => callback(e.KeyData, control.Text);
            return control;
        }
        public static T onKeyPress<T>(this T control, Keys onlyFireOnKey, MethodInvoker callback) where T : Control
        {
            control.KeyUp += (sender, e) =>
            {
                if (e.KeyData == onlyFireOnKey)
                    callback();
            };
            return control;
        }
        public static T onKeyPress<T>(this T control, Keys onlyFireOnKey, Action<String> callback) where T : Control
        {
            control.KeyDown += (sender, e) =>
            {
                if (e.KeyData == onlyFireOnKey)
                {   
                    var text = control.Text;
                    O2Thread.mtaThread(() => callback(text));
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };
            return control;
        }
        public static T onKeyPress_getChar<T>(this T control, Func<char, bool> callback)            where T : Control
        {
            control.KeyPress += (sender, e) => e.Handled = callback(e.KeyChar);
            return control;
        }        
        public static T onKeyPress_getChar<T>(this T control, Action<char> callback)            where T : Control
        {
            control.KeyPress += (sender, e) => callback(e.KeyChar);
            return control;
        }
        public static T onEnter<T>(this T control, Action<String> callback) where T : Control
        {
            return control.onKeyPress(Keys.Enter, callback);
        }

        // Control - Sync
        public static T syncTextBoxWithControl<T>(this T control, Action<string> onTextChanged) where T : Control
        {
            return control.syncTextBoxWithControl(true, onTextChanged);
        }
        public static T syncTextBoxWithControl<T>(this T control, bool onlyFireOnEnter, Action<string> onTextChanged) where T : Control
        {
            var textBox = control.insert_Above<TextBox>(20);
            textBox.KeyUp += (sender, e) =>
            {
                if (onlyFireOnEnter.isFalse() || e.KeyCode == Keys.Enter)
                    O2Thread.mtaThread(()=>onTextChanged(textBox.get_Text()));
            };
            return control;
        }
        public static T syncComboBoxWithControl<T>(this T control, Action<string> onTextChanged) where T : Control
        {
            return control.syncComboBoxWithControl(true, true, onTextChanged);
        }
        public static T syncComboBoxWithControl<T>(this T control, bool onlyFireOnEnter, bool addToHistory, Action<string> onTextChanged) where T : Control
        {
            var comboBox = control.insert_Above<ComboBox>(20);
            comboBox.KeyUp += (sender, e) =>
            {
                //	comboBox.invokeOnThread(
                //		()=>{
                var text = comboBox.Text;
                if (onlyFireOnEnter.isFalse() || e.KeyCode == Keys.Enter)
                {
                    O2Thread.mtaThread(()=> onTextChanged(text));
                    if (addToHistory)
                    {
                        comboBox.Items.Insert(0, text);
                        comboBox.Text = "";
                    }
                }
                //			});					
            };
            return control;
        }
       
        //SendKeys
        public static T sendKeys<T>(this T control, string textToSend) where T : Control
        {
            return (T)control.invokeOnThread(
                () =>
                {
                    control.focus();
                    SendKeys.Send(textToSend);
                    return control;
                });
        }
        public static T sendEnter<T>(this T control) where T : Control
        {
            return control.sendKeys("".line());
        }

        
        //askUserFor...
        public static string askUserForFileToOpen(this Control control)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForFileToOpen());
        }
        public static string askUserForFileToOpen(this Control control,string baseDirectory)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForFileToOpen(baseDirectory));
        }
        public static string askUserForFileToOpen(this Control control, string baseDirectory, string filter)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForFileToOpen(baseDirectory, filter));
        }
        public static string askUserForFileToSave(this Control control, string baseDirectory)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForFileToSave(baseDirectory));
        }
        public static string askUserForDirectory(this Control control, string baseDirectory)
        {
            return (string)control.invokeOnThread(
                () => O2Forms.askUserForDirectory(baseDirectory));
        }        

        public static T      size<T>(this T control, int value)			where T : Control
        {
            return control.textSize(value);
        }		
        public static T      size<T>(this T control, string value)			where T : Control
        {
            return control.textSize(value.toInt());
        }		
        public static T      font<T>(this T control, Font font) where T : Control
        {
            return control.invokeOnThread(()=>{
                                                 control.Font = font;
                                                 return control;
                                              });			
        }
        public static T      font<T>(this T control, string fontFamily, string size)			where T : Control
        {
            return control.font(fontFamily, size.toInt());
        }		
        public static T      font<T>(this T control, string fontFamily, int size)			where T : Control
        {
            return control.font(new FontFamily(fontFamily), size);
        }		
        public static T      font<T>(this T control, FontFamily fontFamily, string size)			where T : Control
        {
            return control.font(fontFamily, size.toInt());
        }		
        public static T      font<T>(this T control, FontFamily fontFamily, int size)			where T : Control
        {
            if (control.isNull())
                return null;
            control.invokeOnThread(
                ()=>{
                        if (fontFamily.isNull())
                            fontFamily = control.Font.FontFamily;
                        control.Font = new Font(fontFamily, size);
                    });
            return control;
        }		
        public static T      font<T>(this T control, string fontFamily)			where T : Control
        {
            return control.fontFamily(fontFamily);
        }		
        public static T      fontFamily<T>(this T control, string fontFamily)			where T : Control
        {
            control.invokeOnThread(
                ()=> control.Font = new Font(new FontFamily(fontFamily), control.Font.Size));			
            return control;
        }		
        public static T      textSize<T>(this T control, int value)			where T : Control
        {
            control.invokeOnThread(
                ()=> control.Font = new Font(control.Font.FontFamily, value));			
            return control;
        }		
        public static T      font_bold<T>(this T control)					where T : Control
        {
            // just bold() conficts with WPF version
            control.invokeOnThread(
                ()=> control.Font = new Font( control.Font, control.Font.Style | FontStyle.Bold ));
            return control;
        }		
        public static T      font_italic<T>(this T control)			where T : Control
        {
            control.invokeOnThread(
                ()=> control.Font = new Font( control.Font, control.Font.Style | FontStyle.Italic ));
            return control;
        }		
        public static Font   font(this string fontName)
		{
			return fontName.font(8);
		}
		public static Font   font(this string fontName, int size)
		{
			return new Font(fontName, size);
		}		
		public static Font   style_Add(this Font font, FontStyle fontStyle)
		{
			var currentStyle = (FontStyle)font.field("fontStyle");
			var newStyle = currentStyle | fontStyle;
			font.field("fontStyle", newStyle);
			return font;
		}		
		public static Font   bold(this Font font)
		{
			return font.style_Add(FontStyle.Bold);
		}
		public static Font   italic(this Font font)
		{
			return font.style_Add(FontStyle.Italic);
		}
        public static Font   strikeout(this Font font)
		{
			return font.style_Add(FontStyle.Strikeout);
		}
		public static Font   underline(this Font font)
		{
			return font.style_Add(FontStyle.Underline);
		}
  

        //misc to place in correct location 
        public static List<T>   action<T>(this List<T> controls, Action<T> action) where T : Control
        {
            foreach (var control in controls)
                action(control);
            return controls;
        }
        public static List<T>   color<T>(this List<T> controls, Color color) where T : Control
        {
            return controls.action((control) => control.color(color));
        }
        public static Color     color(this string colorName)
        {
            var color = Color.FromName(colorName);
            if (color.IsKnownColor.isFalse())
                "In color, provided colorName was not found: {0}".error(colorName);
            return color;
        }
        public static T         color<T>(this T control, Color color) where T : Control
        {
            return control.backColor(color);
        }
        public static List<T>   color<T>(this List<T> controls, string colorName) where T : Control
        {
            return controls.color(Color.FromName(colorName));
        }
        public static T         color<T>(this T control, string colorName) where T : Control
        {
            return control.foreColor(colorName);
        }
        public static T         foreColor<T>(this T control, string colorName) where T : Control
        {
            return control.foreColor(colorName.color());    			
        }    	
        public static T         textColor<T>(this T control, string colorName) where T : Control
        {
            return control.foreColor(colorName);
        }
        
        public static List<T>   pink<T>(this List<T> controls) where T : Control
        {
            return controls.color(Color.LightPink);
        }
        public static List<T>   red<T>(this List<T> controls) where T : Control
        {
            return controls.color(Color.Red);
        }
        public static List<T>   green<T>(this List<T> controls) where T : Control
        {
            return controls.color(Color.LightGreen);
        }
        public static List<T>   azure<T>(this List<T> controls) where T : Control
        {
            return controls.color(Color.Azure);
        }
        public static List<T>   white<T>(this List<T> controls) where T : Control
        {
            return controls.color(Color.White);
        }
        public static List<T>   blue<T>(this List<T> controls) where T : Control
        {
            return controls.color(Color.LightBlue);
        }
        public static T         white<T>(this T control)			where T : Control
        {
            return control.backColor(Color.White);
        }		
        public static T pink<T>(this T control)			    where T : Control
        {
            return control.backColor(Color.LightPink);
        }
        public static T red<T>(this T control)              where T : Control
        {
            return control.backColor(Color.Red);
        }
        public static T blue<T>(this T control) where T : Control
        {
            return control.backColor(Color.LightBlue);
        }		
        public static T green<T>(this T control)			where T : Control
        {
            return control.backColor(Color.LightGreen);
        }			
        public static T azure<T>(this T control)			where T : Control
        {
            return control.backColor(Color.Azure);
        }			
        public static int top<T>(this T control)			where T : Control
        {
            return control.invokeOnThread(()=>  control.Top);
        }
        public static int left<T>(this T control)			where T : Control
        {
            return (int)control.invokeOnThread(()=>  control.Left);
        }		
        public static int width<T>(this T control)			where T : Control
        {
            return (int)control.invokeOnThread(()=>  control.Width);
        }		
        public static int height<T>(this T control)			where T : Control
        {
            return (int)control.invokeOnThread(()=>  control.Height);
        }		
        public static T align_Right<T>(this T control)			where T : Control
        {
            return control.align_Right(control.parent());
        }


        //Add and Append controls

        public static LinkLabel add_Link(this Control control, string label, Action onClickCallback)
        {
            return control.add_Link(label, 0,0, ()=> onClickCallback());
        }		
        public static LinkLabel append_Link_Below(this Control control, string label,int left, Action onClickCallback)
        {
            return control.append_Link_Below(label,onClickCallback).left(left);
        }		
        public static LinkLabel append_Link_Below(this Control control, string label, Action onClickCallback)
        {
            return control.append_Below_Link(label,onClickCallback);
        }		
        public static LinkLabel append_Below_Link(this Control control, string label, Action onClickCallback)
        {
            return control.parent().add_Link(label, control.top() + 20 , control.left(), ()=> onClickCallback());
        }		
        public static Label append_Below_Label(this Control control, string label)
        {
            return control.parent().add_Label(label, control.top() + 22 , control.left());
        }

        //Refresh enable and disable
        public static T refresh_Disable<T>(this T control) where T : Control
        {
            return control.beginUpdate();
        }
        public static T refresh_Enable<T>(this T control) where T : Control
        {
            return control.endUpdate();
        }
        public static T beginUpdate<T>(this T control) where T : Control
        {
            return control.invokeOnThread(() =>
                {
                    control.invoke("BeginUpdateInternal");
                    return control;
                });            
        }
        public static T endUpdate<T>(this T control) where T : Control
        {
            return control.invokeOnThread(() =>
                {
                    control.invoke("EndUpdateInternal");
                    return control;
                });
        }

        
    }
}