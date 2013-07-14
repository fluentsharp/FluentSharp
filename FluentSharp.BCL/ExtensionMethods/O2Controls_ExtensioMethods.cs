using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FluentSharp.CoreLib;


namespace FluentSharp.WinForms
{
    // these are controls created via reflection (only available if the target dll is locally available
    public static class O2Controls_ExtensioMethods
    {
        public static Control add_ControlUsingExtensionMethod(this Control hostControl, string assembly, string extensionMethodClass, string ctorMethod, object ctorData, string setMethod, object setData)
        {
            var type = assembly.type(extensionMethodClass);
            var newControl = (ctorData != null)
                           ? type.invokeStatic(ctorMethod, hostControl, ctorData)
                           : type.invokeStatic(ctorMethod, hostControl);
            if (newControl is Control)
            {
                if (setMethod != null)
                    type.invokeStatic(setMethod, newControl, setData);
                return (Control)newControl;
            }
            return null;
        }

        public static object createAndInvokeUsingExtensionMethod(this Control hostControl, string assembly, string extensionMethodClass, string ctorMethod, object ctorData, string setMethod, object setData)
        {
            var type = assembly.type(extensionMethodClass);
            var newControl = (ctorData != null)
                           ? type.invokeStatic(ctorMethod, hostControl, ctorData)
                           : type.invokeStatic(ctorMethod, hostControl);

            if (setMethod != null)
                type.invokeStatic(setMethod, newControl, setData);
            return newControl;            
        }
    }
}
