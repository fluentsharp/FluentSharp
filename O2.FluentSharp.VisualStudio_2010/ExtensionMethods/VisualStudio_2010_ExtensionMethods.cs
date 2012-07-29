using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE80;
using O2.FluentSharp;
//O2File:VS_ErrorListProvider_ExtensionMethods.cs
//O2File:VS_Menus_ExtensionMethods.cs

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class VisualStudio_2010_ExtensionMethods
    {        
		public static DTE2 dte(this VisualStudio_2010 visualStudio)
		{
			return VisualStudio_2010.DTE2;	
		}
    }
}
