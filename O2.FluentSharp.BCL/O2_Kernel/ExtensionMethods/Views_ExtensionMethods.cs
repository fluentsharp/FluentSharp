using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace O2.Kernel.ExtensionMethods
{
    public static class Views_ExtensionMethods
    {
        public static void showInfo(this object _object)
        {
            show.info(_object);
        }

        public static Control viewsAscxControl(this string controlName)
        {
            return controlName.viewsAscxControl(controlName, -1, -1);
        }

        public static Control viewsAscxControl(this string controlName, string title)
        {
            return controlName.viewsAscxControl(title, -1, -1);
        }

        public static Control viewsAscxControl(this string controlName, string title, int width, int height)
        {
            try
            {
                var objectType = "O2_FluentSharp_BCL.dll".type(controlName);
                if (objectType == null)
                    "could not load control type: {0}".format(controlName).error();
                else
                    return objectType.openControlAsForm(title, width, height);
            }
            catch (Exception ex)
            {
                "in show.control ({0},{1},{2},{3}): {4}".format(controlName, title, width, height, ex.Message).error();
            }
            return null;
        }

        public static T openControlAsForm<T>() where T : Control
        {
            return (T)openControlAsForm(typeof(T), typeof(T).Name, -1, -1);
        }

        public static T openControlAsForm<T>(this Type type) where T : Control
        {
            return (T)openControlAsForm(type, type.Name, -1, -1);
        }

        public static T openControlAsForm<T>(this Type type, string title, int width, int height) where T : Control
        {
            return (T)openControlAsForm(type, title, width, height);
        }

        public static T openControlAsForm<T>(this string title) where T : Control
        {
            return (T)openControlAsForm(typeof(T), title, -1, -1);
        }

        public static T openControlAsForm<T>(this string title, int width, int height) where T : Control
        {
            return (T)openControlAsForm(typeof(T), title, width, height);
        }

        public static Control openControlAsForm(this Type controlType, string title, int width, int height)
        {
            if (controlType == null)
            {
                "in openControlAsForm, provided controlType variable was null".error();
                return null;
            }    
            var winFormsType = "O2_Views_Ascx.dll".type("WinForms");
            if (winFormsType != null)
            {
                var control = winFormsType.ctor().invoke("showAscxInForm", controlType, title, width, height);
                if (control != null)
                    return (Control)control;
            }
            "could not create control type: {0}".format(controlType).error();
            return null;
        }

        public static Control createAndAddControl(this Control hostControl, string assemblyName, string typeName)
        {
            return createAndAddControl(hostControl,assemblyName.type(typeName));
        }

        public static Control createAndAddControl(this Control hostControl, Type controlToCreate)
        {
            if (controlToCreate == null)
                return null;            
            var ascxExtensioMethods = "O2_DotNetWrappers.dll".type("Control_ExtensionMethods");
            return (Control)ascxExtensioMethods.invokeStatic("add_Control", hostControl, controlToCreate);        
        }

        public static T openControlAsForm<T>(this string title, int width, int height, string ctorMethod,object ctorData, string setMethod, object setData) where T : Control
        {
            var panel = typeof(Panel).openControlAsForm(title, width, height);
            var ascxExtensioMethods = "O2_DotNetWrappers.dll".type("Ascx_ExtensionMethods");            
            var control = (ctorData != null) 
                           ? (T)ascxExtensioMethods.invokeStatic(ctorMethod, panel, ctorData)
                           : (T)ascxExtensioMethods.invokeStatic(ctorMethod, panel);
                        
            ascxExtensioMethods.invokeStatic(setMethod, control, setData);
            return control;
        }

        public static T cast<T>(this object _object) where T : Control
        {
            if (_object is T)
                return (T)_object;
            return null;
        }
    }
}
