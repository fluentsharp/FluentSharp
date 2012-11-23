using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.CommandBars;
using EnvDTE80;
using O2.FluentSharp.VisualStudio;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using EnvDTE;



namespace O2.FluentSharp.VisualStudio.ExtensionMethods
{
	public static class VisualStudio_2010_ExtensionMethods_Document
	{
		public static string activeFile(this VisualStudio_2010 visualStudio)
		{
			return visualStudio.activeDocument_FullName();
		}
		public static string activeDocument_FullName(this VisualStudio_2010 visualStudio)
		{
			var activeDocument = visualStudio.activeDocument();
			if (activeDocument.notNull())
				return activeDocument.FullName;
			return null;
		}
		public static Document activeDocument(this VisualStudio_2010 visualStudio)
		{
			return VisualStudio_2010.DTE2.ActiveDocument;
		}
	}
}