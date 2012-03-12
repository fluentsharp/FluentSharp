using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Interfaces.O2Core;
using O2.Interfaces.O2Findings;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using System.Windows.Forms;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Form_ExtensionMethods
    {
        public static T form<T>(this FormCollection formCollection)
            where T : Form
        {
            var forms = new List<T>();
            foreach (Form form in Application.OpenForms)
                if (form is T)
                    return (T)form;
            return null;
        }

        public static List<T> forms<T>(this FormCollection formCollection)
            where T : Form
        {
            var forms = new List<T>();
            foreach (Form form in Application.OpenForms)
                if (form is T)
                    forms.Add((T)form);
            return forms;
        }

        public static List<T> applicationWinForms<T>(this object _object)
            where T : Form
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

        public static Form opacity(this Form form, double value)
        {
            form.invokeOnThread(
                () =>
                {
                    form.Opacity = value;
                });
            return form;
        }
    }
}
