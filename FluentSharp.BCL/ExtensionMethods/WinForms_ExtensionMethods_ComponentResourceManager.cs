using System;
using System.ComponentModel;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
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
}