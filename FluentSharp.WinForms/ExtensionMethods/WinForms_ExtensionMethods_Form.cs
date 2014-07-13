using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Form
    {
        public static Form form(this string text)
        {
            return text.openForm();
        }
        public static Form openForm(this string text)
        {
            foreach (Form form in Application.OpenForms)
                if (form.get_Text() == text)
                    return form;
            return null;        
        }
        public static T form<T>(this FormCollection formCollection) where T : Form
        {
            var forms = new List<T>();
            foreach (Form form in Application.OpenForms)
                if (form is T)
                    return (T)form;
            return null;
        }
        public static List<T> forms<T>(this FormCollection formCollection) where T : Form
        {
            var forms = new List<T>();
            foreach (Form form in Application.OpenForms)
                if (form is T)
                    forms.Add((T)form);
            return forms;
        }
        public static List<T> applicationWinForms<T>(this object _object) where T : Form
        {
            return (from form in _object.applicationWinForms()
                    where form is T
                    select (T)form).toList();
        }
        public static List<Form> applicationWinForms(this object _object)
        {
            var forms = new List<Form>();
            foreach (Form form in Application.OpenForms)
                forms.Add(form);
            return forms;
            //foreach(var form in Application.OpenForms)
            //return (from form in 
            //		select form).toList();    		
        }

        public static Form lastFormLoaded(this string dummyString)
        {
            return dummyString.lastWindowShown();
        }
        
        public static Form lastWindowShown(this string dummyString)
        {
            return dummyString.applicationWinForms().Last();
        }
        
        public static double form_Opacity<T>(this T control) where T : Control
        {
            return control.parentForm().opacity();
        }
        public static bool opacity_Zero<T>(this T form) where T : Form
        {
            return form.opacity().Equals(0.0);
        }
        public static double opacity(this Form form)
        {
            if (form.notNull())
                return form.invokeOnThread(() => form.Opacity);            
            return 0;
        }
        public static Form opacity(this Form form, double value)
        {
            form.invokeOnThread(
                () =>
                {
                    form.Opacity = value;
                });
            return form;
        }
        
        public static Panel popupWindow_Hidden(this string title)
        {
            return title.showAsForm(startHidden: true);
        }

        public static T     popupWindow<T>(this string title)			where T : Control
        {
            //title+=" - test" ;
            return title.showAsForm().add_Control<T>();
        }        
        public static T     popupWindow<T>(this string title, int width, int height, bool startHidden = false) where T : Control
        {
            return title.showAsForm(width, height,startHidden)
                        .add_Control<T>();
        }        
        public static Panel popupWindow(this string title, bool startHidden = false)
        {            
            return (startHidden) ? title.popupWindow_Hidden()
                                 : title.showAsForm();
        }        
        public static Panel popupWindow(this string title, int width, int height, bool startHidden = false)
        {
            return title.showAsForm(width, height,startHidden);
        }
                
        public static Panel createForm(this string title)
        {
            return title.showAsForm();
        }
        public static Panel showAsForm(this string title)
        {
            return title.showAsForm<Panel>(600, 400);
        }
        public static Panel showAsForm(this string title, int width, int height)
        {
            return O2Gui.open<Panel>(title, width, height);
        }
        public static Panel showAsForm(this string title, bool startHidden)
        {
            return title.showAsForm(600, 400, startHidden);
        }
        public static Panel showAsForm(this string title, int width, int height, bool startHidden)
        {
            return O2Gui.load<Panel>(title, width, height, startHidden);
        }
        public static T	    openForm<T>(this string textToAppendToFormTitle) where T : Form
        {
            T form = null;
            O2Thread.staThread(
                        () =>
                        {
                            form = (T)typeof(T).ctor();
                            form.Text += textToAppendToFormTitle.valid() ? " - {0}".format(textToAppendToFormTitle) : "";
                            form.ShowDialog();
                        });
            MiscUtils.waitForNotNull(ref form);
            return form;
        } 
        public static T		showAsForm<T>(this string title) where T : Control
        {
            return title.showAsForm<T>(600, 400);
        }
        public static T		showAsForm<T>(this string title, int width, int height) where T : Control
        {
            return O2Gui.open<T>(title, width, height)
                            .add_H2Icon();
        }
        
        public static T show_PopupWindow<T>(this T control) where T : Control
        {
            control.parentForm().showDialog();            
            return control;
        }
        
        public static Form showDialog(this Form form, bool useNewStaThread = true)
        {
            if (form.notNull())
            { 
                var controlCreation = new AutoResetEvent(false);
            
                form.Load += (sender, e) =>
                {
                    controlCreation.Set();
                };
            
                if (useNewStaThread)
        	        O2Thread.staThread(()=>
        	        {
        	                form.ShowDialog();
        	        });
                else
                    form.ShowDialog();

                var maxTimeOut = Debugger.IsAttached ? -1 : 20000;
            
                if (controlCreation.WaitOne(maxTimeOut).failed())
                    "[Form][showDialog] Something went wrong with the creation of the form since it took more than 20s to start".error();
            }
            return form;            
        }
        public static T         closeForm<T>(this T control) where T : Control
        {
            control.parentForm().close();
            return control;
        }
        public static T         closeForm_InNSeconds<T>(this T control, int seconds) where T : Control
        {
            O2Thread.mtaThread(
                () =>
                {
                    control.sleep(seconds * 1000);
                    control.closeForm();
                });
            return control;
        }
        public static T         resizeFormToControlSize<T>(this T control, Control controlToSync) where T : Control
        {
            if (controlToSync.notNull())
            {
                var parentForm = control.parentForm();
                if (parentForm.notNull())
                {
                    var top = controlToSync.PointToScreen(System.Drawing.Point.Empty).Y;
                    var left = controlToSync.PointToScreen(System.Drawing.Point.Empty).X;
                    var width = controlToSync.Width;
                    var height = controlToSync.Height;
                    "Setting parentForm location to {0}x{1} : {2}x{3}".info(top, left, width, height);
                    parentForm.Top = top;
                    parentForm.Left = left;
                    parentForm.Width = width;
                    parentForm.Height = height;
                }
            }
            return control;
        }
        public static Form      onClosed(this Form form, MethodInvoker onClosed)
        {
            if (form == null)
            {
                "in Form.onClosed, provided form value was null".error();
                return null;
            }
            form.Closed += (sender, e) => onClosed();
            return form;
        }
        public static Form      close(this Form form)
        {
            form.invokeOnThread(form.Close);
            return form;
        }
        public static T         waitForClose<T>(this T control) where T: Control
        {       
            try
            {
                if (control.IsDisposed.isFalse())
                {
                    var form = control.parentForm();
                    if (form.notNull())
                        if (form.IsDisposed.isFalse())
                        {
                            var formClosed = new AutoResetEvent(false);
                            form.onClosed(() => formClosed.Set());
                            while (form.IsDisposed.isFalse())
                            {
                                if (formClosed.WaitOne(1000))
                                    return control;
                            }                    
                        }
                }
            }
            catch(Exception ex)
            {
                ex.log("[Control][waitForClose]");
            }
            return control;
        }
        public static Form      form_Currently_Running(this string titleOrType)
        {
            return titleOrType.applicationWinForm();
        }
        public static Form      applicationWinForm(this string titleOrType)
        {
            return titleOrType.applicationWinForms().first();
        }
        public static List<Form> applicationWinForms(this string titleOrType)
        {
            var forms = new List<Form>();
            foreach (Form form in Application.OpenForms)
                if (form.get_Text() == titleOrType || form.str() == titleOrType ||
                    form.typeFullName() == titleOrType || form.typeName() == titleOrType)
                {
                    forms.Add(form);
                }
            return forms;
        }
        public static Form      maximize(this Form form)
        {
            return form.invokeOnThread(() =>{
                                                form.WindowState = FormWindowState.Maximized;
                                                return form;
                                            });
        }
        public static T         minimize<T>(this T control)            where T : Control
        {
            return control.windowState(FormWindowState.Minimized);
        }
        public static T         maximize<T>(this T control)            where T : Control
        {
            return control.windowState(FormWindowState.Maximized);
        }
        public static T         normal<T>(this T control)            where T : Control
        {
            return control.windowState(FormWindowState.Normal);
        }
        public static bool      isMaximized(this Form form)
        {
            return form.isWindowState(FormWindowState.Maximized);
        }
        public static bool      isMinimized(this Form form)
        {
            return form.isWindowState(FormWindowState.Minimized);
        }
        public static bool      isNormal(this Form form)
        {
            return form.isWindowState(FormWindowState.Normal);
        }
        public static bool      isWindowState(this Form form, FormWindowState state)
        {
            if (form.isNull())
                return false;
            
            return form.invokeOnThread(() => form.WindowState == state);
        }
        public static T         windowState<T>(this T control, FormWindowState state)            where T : Control
        {
            return control.invokeOnThread(() =>{
                                                    control.parentForm().WindowState = state;
                                                    return control;
                                               });
        }
        public static T         parentForm_AlwaysOnTop<T>(this T control)			where T : Control
        {
            control.parentForm().alwaysOnTop();
            return control;
        }				
        public static T         alwaysOnTop<T>(this T form)			where T : Form
        {
            form.invokeOnThread(()=> form.TopMost= true);
            return form;
        }
        public static Form      title(this Form form, string title)
        {
            return form.set_Text(title);
        }
        public static string    title(this Form form)
        {
            return form.get_Text();
        }
        public static Form      fadeOutIn(this Form form)
        {
            form.fadeOut();
            return form.fadeIn();
        }
        public static Form      fadeOut(this Form form)
        {
            "Fading Out Form: {0}".info(form);
            var value = 1.0;
            10.loop(100, () => form.opacity(value -= 0.1));
            return form;
        }
        public static Form      fadeIn(this Form form)
        {
            "Fading In Form: {0}".info(form);
            var value = 0.0;
            10.loop(100, () => form.opacity(value += 0.1));
            return form;
        }
        public static Form      hide(this Form form)
        {
            return   form.opacity(0);           // using opacity to hide the form since calling visible=false was trigering the o2Gui.Dispose() call
        }
        public static Form      show(this Form form) 
        {
            if (form.opacity_Zero())        // handle the case when the opacity has been set to 0 (which means that we also need to reset it, or the control will not be shown)
                form.opacity(1);
            form.visible(true);
            return form;
        }
        public static T         showInTaskbar<T>(this  T control, bool value = true) where T: Control
        {
            if (control.isNull())
                return null;
            var form = control.parentForm();
            if (form.notNull())
                form.invokeOnThread(() => form.ShowInTaskbar == value);
            return control;        
        }
    }

    public static class WinForms_ExtensionMethods_MDIForms
    {
        public static Form mdiForm(this string title)
        {
            return title.mdiHost();
        }
        public static Form mdiHost(this string title)
        {
            return title.popupWindow()
                        .parentForm()
                        .isMdiContainer();
        }
        public static Form isMdiContainer(this Form form)
        {
            return form.isMdiContainer(true);
        }
        public static Form isMdiContainer(this Form form, bool value)
        {
            return form.invokeOnThread(()=>{
                                               form.Controls.Clear();
                                               form.IsMdiContainer = true;
                                               return form;
                                           });
        }
        public static Form add_MdiChild(this Form parentForm)
        {
            return parentForm.add_MdiChild("");
        }
        public static Form add_MdiChild(this Form parentForm, string title)
        {
            return parentForm.add_MdiChild<Form>(title);
        }
        public static T add_MdiChild<T>(this Form parentForm, string title) where T : Form
        {
            return (T)parentForm.invokeOnThread(
                () =>
                {
                    var mdiChild = (Form)typeof(T).ctor();
                    mdiChild.Text = title;
                    mdiChild.MdiParent = parentForm;
                    mdiChild.Show();
                    return mdiChild;
                });
        }
        public static Form add_MdiChild(this Form parentForm, Func<Form> formCtor)
        {
            return parentForm.invokeOnThread(()=>{
                                                    var mdiChild = formCtor();
                                                    mdiChild.MdiParent = parentForm;
                                                    mdiChild.Show();
                                                    return mdiChild;
                                                });
        }
        public static Form layout(this Form parentForm, MdiLayout layout)
        {
            return parentForm.invokeOnThread(()=>{
                                                    parentForm.LayoutMdi(layout);
                                                    return parentForm;
                                                 });
        }
        public static Form layout_TileHorizontal(this Form parentForm)
        {
            return parentForm.layout(MdiLayout.TileHorizontal);
        }
        public static Form layout_TileVertical(this Form parentForm)
        {
            return parentForm.layout(MdiLayout.TileVertical);
        }
        public static Form layout_Cascade(this Form parentForm)
        {
            return parentForm.layout(MdiLayout.Cascade);
        }
        public static Form layout_ArrangeIcons(this Form parentForm)
        {
            return parentForm.layout(MdiLayout.ArrangeIcons);
        }
        public static Form tileHorizontaly(this Form parentForm)
        {
            return parentForm.layout(MdiLayout.TileHorizontal);
        }
        public static Form tileVerticaly(this Form parentForm)
        {
            return parentForm.layout(MdiLayout.TileVertical);
        }
        public static Form cascade(this Form parentForm)
        {
            return parentForm.layout(MdiLayout.Cascade);
        }
        public static ascx_LogViewer add_Mdi_LogViewer(this Form parentForm)
        {
            return parentForm.add_MdiChild()
                             .add_LogViewer();
        }

    }
}
