using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.PlatformUI;
using EnvDTE;
using EnvDTE80;
using O2.FluentSharp;
//O2File:VS_ErrorListProvider_ExtensionMethods.cs
//O2File:VS_Menus_ExtensionMethods.cs
//O2Ref:Microsoft.VisualStudio.Shell.UI.Internal.dll
//O2Ref:WindowsFormsIntegration.dll

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class VisualStudio_2010_ExtensionMethods
    {        
		public static DTE2 dte(this VisualStudio_2010 visualStudio)
		{
			return VisualStudio_2010.DTE2;	
		}
    }
    
    public static class VisualStudio_2010_ExtensionMethods_Packages
    {
    	public static Package package(this VisualStudio_2010 visualStudio)
    	{
    		return VisualStudio_2010.Package;
    	}
    	public static T getService<T>(this VisualStudio_2010 visualStudio)
		{
			return VisualStudio_2010.Package.getService<T>();
		}
    }

    public static class VisualStudio_2010_ExtensionMethods_DocumentViewer
    {
        public static IVsWindowFrame open_Document(this string file)
        {
            try
            {        
                if (file.fileExists().isFalse())
                    "[open_Document] provided file doesn't exist: {0}".info(file);
                else
                {
                    var package = VisualStudio_2010.Package; 
                    var openDoc = package.getService<IVsUIShellOpenDocument>();
                    IVsWindowFrame frame;  
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider;  
                    IVsUIHierarchy hierarchy;  
                    uint itemId;  
                    Guid logicalView = VSConstants.LOGVIEWID_Code; 
                    openDoc.OpenDocumentViaProject(file, ref logicalView, out serviceProvider, out hierarchy, out itemId, out frame);
                    if (frame.notNull())
                    {
                        frame.Show();
                        return frame;
                    }
                    "[open_Document] could not get IVsWindowFrame for file: {0}".info(file);
                }
            }
            catch(Exception ex)
            {
                ex.log("[in file.open_Document]");
            }
            return null;
        }
    }
    
    public static class VisualStudio_2010_ExtensionMethods_DTE_Projects
    {
    	public static Project project(this int index)
    	{
    		try
    		{
    			return (Project)VisualStudio_2010.DTE2.Solution.Projects.toList()[index];
    		}
    		catch
    		{
    			"[VisualStudio_2010] could not find project with index: {0}".info(index);
    			return null;
    		}
    	}    	
    	public static Project project(this string projectName)
    	{
    		try
    		{
    			return VisualStudio_2010.DTE2.Solution.Projects.Item(projectName);
    		}
    		catch
    		{
    			"[VisualStudio_2010] could not find project with name: {0}".info(projectName);
    			return null;
    		}
    	}
    	public static string pathTo_CompiledAssembly(this Project project)
    	{    	
			try
			{
				var relativeOutputPath = project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath")
											.Value.str();
				var projectPath = 	project.ProjectItems.ContainingProject.FullName.parentFolder();
				var outputPath  = projectPath.pathCombine(relativeOutputPath);
				var outputFileName = project.Properties.Item("OutputFileName").Value.str();
				var fullPathToCompiledAssembly = outputPath.pathCombine(outputFileName);
				return fullPathToCompiledAssembly;
			}
			catch(Exception ex)
			{
				ex.log("[VisualStudio_2010][pathTo_CompiledAssembly]");
				return null;
			}
		}		
        public static List<ProjectItem> projectItems(this Project project)
        {
            var projectItems = new List<ProjectItem>();
            foreach (ProjectItem projectItem in project.ProjectItems)
                projectItems.add(projectItem);
            return projectItems;
        }
        public static List<string> names(this List<ProjectItem> projectItems)
        { 
            return projectItems.Select((projectItem)=>projectItem.Name).toList();
        }
        public static string filePath(this ProjectItem projectItem)
        {
        	if (projectItem.FileCount == 1)        	
        		return  projectItem.FileNames[0];
        	"in ProjectItem.filePath, the FileCount for {0} was {1}".error(projectItem.Name, projectItem.FileCount);
        	return null;
        }        
    }

    public static class VisualStudio_2010_ExtensionMethods_WPF_Application
    {
        public static T invokeOnThread<T>(this VisualStudio_2010 visualStudio, Func<T> func)
        {
            return System.Windows.Application.Current.wpfInvoke(() => func());
        }
        public static VisualStudio_2010 invokeOnThread(this VisualStudio_2010 visualStudio, Action action)
        {
            return (VisualStudio_2010)System.Windows.Application.Current.wpfInvoke(() =>
                        {
                            action();
                            return visualStudio;
                        });
        }
        public static Application application(this VisualStudio_2010 visualStudio)
        {
            return visualStudio.invokeOnThread(() => System.Windows.Application.Current);
        }
        public static MainWindow mainWindow(this VisualStudio_2010 visualStudio)
        {
            return (MainWindow)visualStudio.invokeOnThread(() => visualStudio.application().MainWindow);
        }
    }
    
    
    
    //Extra WPF Extension Methods
    public static class WPF_ExtensionMethods_Window
    {
    	public static string title<T>(this T window) where T : System.Windows.Window
    	{
    		return window.wpfInvoke(()=> window.Title);
    	}    	
    	public static T title<T>(this T window, string title) where T : System.Windows.Window
    	{
    		return window.wpfInvoke(()=>{  window.Title = title; return window;} );
    	}
    }
    
    
    
    public static class VisualStudio_2010_ExtensionMethods_ToolWindowsPane
    {
    	public static int lastWindowId = 0;    	
    	public static Grid create_WPF_Window(this string title)
    	{
    		ToolWindowPane toolWindow = null;
    		return title.create_WPF_Window(ref toolWindow);
    	}    	
    	public static Grid create_WPF_Window(this string title, ref ToolWindowPane toolWindow)
    	{
    		var visualStudio = new VisualStudio_2010();
    		ToolWindowPane window = null;
			var grid = visualStudio.invokeOnThread(
			()=>{					
					var type = typeof(O2.FluentSharp.WindowPane_WPF);
					window = (ToolWindowPane)visualStudio.package().invoke("CreateToolWindow", type, ++lastWindowId);			
					window.Caption = title;
					IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;					
					windowFrame.Show();					
					var content = (Control_WPF)window.Content;
					
					return (Grid)content.Content;							
				});
			toolWindow = window;
			return grid;
    	}
    	public static System.Windows.Forms.Panel create_WinForms_Window(this string title)
    	{
    		ToolWindowPane toolWindow = null;
    		return title.create_WinForms_Window(ref toolWindow);
    	}
    	public static System.Windows.Forms.Panel create_WinForms_Window(this string title, ref ToolWindowPane toolWindow)
    	{
    		var visualStudio = new VisualStudio_2010();
    		ToolWindowPane window = null;    		
			var _panel = visualStudio.invokeOnThread(
			()=>{					
					var type = typeof(O2.FluentSharp.WindowPane_WinForms);
					window = (ToolWindowPane)visualStudio.package().invoke("CreateToolWindow", type, 64000.random());			
					
					window.Caption = title;
					IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;					
					
					//windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_Dock);
					
					windowFrame.Show();				
					var content= (Control_WinForms)window.Content;
					var windowsFormHost = (System.Windows.Forms.Integration.WindowsFormsHost)content.Content;			
					var panel = new System.Windows.Forms.Panel();			
					panel.backColor("Control");
					windowsFormHost.Child = panel;					
					return panel;					
				});
			toolWindow = window;
			return _panel;
    	}

        public static System.Windows.Forms.Panel create_WinForms_Window_Float(this string title)
        {
            ToolWindowPane toolWindow = null;
            var panel = title.create_WinForms_Window(ref toolWindow);
            toolWindow.as_Float();
            return panel;
        }
        
        public static System.Windows.Forms.Panel create_WinForms_Window(this VisualStudio_2010 visualStudio, string title)
        {
        	return title.create_WinForms_Window();
        }
        
        public static System.Windows.Forms.Panel create_WinForms_Window_Float(this VisualStudio_2010 visualStudio, string title)
        {
        	return title.create_WinForms_Window_Float();
        }
    	
    	public static string caption<T>(this T toolWindowPane) where T : ToolWindowPane
    	{
    		return new VisualStudio_2010().invokeOnThread(()=> toolWindowPane.Caption);
    	}
    	public static T caption<T>(this T toolWindowPane, string title) where T : ToolWindowPane
    	{
    		return new VisualStudio_2010().invokeOnThread(()=>{ toolWindowPane.Caption = title ; return toolWindowPane;});
    	}

        public static ToolWindowPane as_MdiChild(this ToolWindowPane toolWindow)
        {
            if (toolWindow.notNull())
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)toolWindow.Frame;
                windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_MdiChild);
            }
            return toolWindow;
        }

        public static ToolWindowPane as_Float(this ToolWindowPane toolWindow)
        {
            if (toolWindow.notNull())
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)toolWindow.Frame;
                windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_Float);
            }
            return toolWindow;
        }

        public static ToolWindowPane as_Dock(this ToolWindowPane toolWindow)
        {
            if (toolWindow.notNull())
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)toolWindow.Frame;
                windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_Dock);
            }
            return toolWindow;
        }        
    }
}
