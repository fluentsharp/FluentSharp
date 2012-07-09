// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Views.ASCX.ExtensionMethods;
using O2.Views.ASCX.classes.MainGUI;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.External.SharpDevelop.Ascx;
//O2File:ascx_SimpleFileSearch.cs

namespace O2.XRules.Database.Utils
{ 
    public static class FileViewers
    {
    	public static T add_FilesViewer<T>(this T control) where T : Control
    	{
    		return control.add_FilesViewer(PublicDI.config.LocalScriptsFolder);
    	}
    	public static T add_FilesViewer<T>(this T control, string targetFolder) where T : Control
    	{
    		return control.add_FilesViewer(targetFolder, "*.cs");
    	}
    	public static T add_FilesViewer<T>(this T control, string targetFolder, string fileFilter) where T : Control
    	{
    		return control.add_FilesViewer(targetFolder, fileFilter, new string[] { fileFilter});
    	}
    	public static T add_FilesViewer<T>(this T control, string targetFolder, string highlightStrategy, params string[] fileFilters) where T : Control
    	{
    		return control.add_FilesViewer(targetFolder, highlightStrategy, true, fileFilters);
    	}
    	public static T add_FilesViewer<T>(this T control, string targetFolder, string highlightStrategy, bool recursive, params string[] fileFilters)
    		where T : Control
    	{
    		var topPanel = control.clear().add_Panel();
			var sourceCodeViewer = topPanel.add_SourceCodeViewer();
			var treeView = sourceCodeViewer.insert_Left<Panel>(300).add_TreeView().sort();			  
			treeView.afterSelect<string>((file)=>sourceCodeViewer.open(file).editor().setDocumentHighlightingStrategy(highlightStrategy));
			Action<string,string[]> loadFilesFromFolder = 
				(folder,filters)=>{
										treeView.clear();
										foreach(var file in folder.files(recursive,filters))
											treeView.add_Node(file.remove(folder),file);
										treeView.selectFirst();	 
									};
			loadFilesFromFolder(targetFolder, fileFilters);
			return control;															
    	}
    	
    	public static ascx_SimpleFileSearch add_FilesSearch<T>(this T control, string targetFolder, params string[] fileFilters)
    		where T : Control
    	{
			var simpleFileSearch = control.clear().add_Control<ascx_SimpleFileSearch>();
			var filesToShow = targetFolder.files(true,fileFilters);
			simpleFileSearch.loadFiles(targetFolder, filesToShow); 
	    	return simpleFileSearch;
    	}
	}
}
