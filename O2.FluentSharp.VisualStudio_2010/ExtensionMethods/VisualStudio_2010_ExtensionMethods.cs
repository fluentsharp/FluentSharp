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
using Microsoft.VisualStudio.Platform.WindowManagement; 
using Microsoft.VisualStudio.Platform.WindowManagement.DTE;
using System.Windows.Forms.Integration;
using WinForms = System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.FluentSharp.VisualStudio;


namespace O2.FluentSharp.VisualStudio.ExtensionMethods
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
					var type = typeof(O2.FluentSharp.VisualStudio.WindowPane_WPF);
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
            return title.create_WinForms_Window(VSFRAMEMODE.VSFM_Dock);
    	}

        public static System.Windows.Forms.Panel create_WinForms_Window(this string title, VSFRAMEMODE frameMode)
    	{
    		var visualStudio = new VisualStudio_2010();    				
			var _panel = visualStudio.invokeOnThread(
			()=>{					
					var type = typeof(O2.FluentSharp.VisualStudio.WindowPane_WinForms);
					var window = (ToolWindowPane)visualStudio.package().invoke("CreateToolWindow", type, 64000.random());			
			        		
					window.Caption = title;
					IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;					
					//if(floating)
                    //    windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_Float);
                    windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, frameMode);					
					windowFrame.Show();				
					var content= (Control_WinForms)window.Content;
					var windowsFormHost = (System.Windows.Forms.Integration.WindowsFormsHost)content.Content;			
					var panel = new System.Windows.Forms.Panel();			
					panel.backColor("Control");
					windowsFormHost.Child = panel;					
					return panel;					
				});			
			return _panel;
    	}
        public static System.Windows.Forms.Panel create_WinForms_Window_Float(this string title, int width, int height)
        {
            var panel = title.create_WinForms_Window_Float();
            panel.dte_Window().width(width).height(height);
            return panel;
        }
        public static System.Windows.Forms.Panel create_WinForms_Window_Float(this string title)
        {                        
            return title.create_WinForms_Window(VSFRAMEMODE.VSFM_Float);                        
        }
        public static System.Windows.Forms.Panel create_WinForms_Window_MdiChild(this string title)
        {
            return title.create_WinForms_Window(VSFRAMEMODE.VSFM_MdiChild);
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
    public static class VisualStudio_2010_ExtensionMethods_WindowBase
    {
        public static WindowBase windowBase(this WinForms.Control control)
        {
            try
            {
                return (WindowBase)control.dte_Window();
            }
            catch (Exception ex)
            {
                ex.log("[in control.windowBase]");
                return null;
            }
        }
        public static List<WindowBase> windows(this VisualStudio_2010 visualStudio)
        {
            var windows = new List<WindowBase>();
            foreach (WindowBase window in visualStudio.dte().Windows)
                windows.Add(window);
            return windows;
        }

        public static WindowBase    window(this VisualStudio_2010 visualStudio, string caption)
        {
            return visualStudio.windows().Where((window) => window.Caption == caption).first();
        }
        public static WindowBase    get_Window(this VisualStudio_2010 visualStudio, string caption)
        {
            return visualStudio.window(caption);
        }
        public static List<string>  names(this List<WindowBase> windows)
        {
            return windows.captions();
        }
        public static List<string>  titles(this List<WindowBase> windows)
        {
            return windows.captions();
        }
        public static List<string>  captions(this List<WindowBase> windows)
        {
            return windows.Select((window) => window.Caption).toList();
        }
        public static WindowBase    floating(this WindowBase window, bool value)
        {
            try
            {
                window.IsFloating = value;
            }
            catch (Exception ex)
            {
                ex.log("[window.floating]");
            }
            return window;
        }
        public static WindowBase    linkable(this WindowBase window, bool value)
        {
            try
            {
                window.Linkable = value;
            }
            catch (Exception ex)
            {
                ex.log("[window.linkable]");
            }
            return window; 
        }
        public static WindowBase    autoHide(this WindowBase window, bool value)
        {
            try
            {
                window.AutoHides = value;
            }
            catch (Exception ex)
            {
                ex.log("[window.autoHide]");
            }
            return window;
        }
        public static WindowBase    visible(this WindowBase window, bool value)
        {
            try
            {
                window.Visible = value;                
            }
            catch (Exception ex)
            {
                ex.log("[window.visible]");
            }
            return window;
        }
        public static WindowBase    title(this WindowBase window, string value)
        {    
            try
            {
                window.Caption = value;
            }
            catch (Exception ex)
            {
                ex.log("[window.title]");
            }
            return window;            
        }
        public static WindowBase    left(this WindowBase window, int value)
        {
            try
            {
                window.Left = value;
            }
            catch (Exception ex)
            {
                ex.log("[window.left]");
            }
            return window;
        }
        public static WindowBase    top(this WindowBase window, int value)
        {
            try
            {
                window.Top = value;
            }
            catch (Exception ex)
            {
                ex.log("[window.top]");
            }
            return window;
        }
        public static WindowBase    width(this WindowBase window, int value)
        {
            try
            {
                window.Width = value;
            }
            catch (Exception ex)
            {
                ex.log("[window.width]");
            }
            return window;
        }
        public static WindowBase    height(this WindowBase window, int value)
        {
            try
            {
                window.Height = value;
            }
            catch (Exception ex)
            {
                ex.log("[window.height]");
            }
            return window;
        }
        public static WindowBase    focus(this WindowBase window)
        {
            return window.show();
        }  
        public static WindowBase    show(this WindowBase window)
        {
            try
            {
                window.visible(true);
                window.Activate();
            }
            catch (Exception ex)
            {
                ex.log("[window.show]");
            }
            return window;
        }
        public static WindowBase    hide(this WindowBase window)
        {
            window.visible(false);
            return window;
        }
    }
    public static class VisualStudio_2010_ExtensionMethods_DTE_Documents
    {
        public static List<EnvDTE.Document> documents(this VisualStudio_2010 visualStudio)
        {
            return (from window in visualStudio.windows()
                    where window.Document.notNull()
                    select window.Document).toList();
        }
        public static EnvDTE.Document document(this VisualStudio_2010 visualStudio, string path)
        {
            //first search by fullpath
            var match = (EnvDTE.Document)(from document in visualStudio.documents()
                                          where document.FullName == path
                                          select document).first(); 
            if (match.notNull())
                return match;
            
            //then by filename
            return (EnvDTE.Document)(from document in visualStudio.documents()
                                     where document.FullName.fileName() == path
                                     select document).first(); 
            
        }
    }
    public static class VisualStudio_2010_ExtensionMethods_DTE_Window
    {
        public static EnvDTE.Window     dte_Window(this System.Windows.Forms.Control control)
        {
            return control.toolWindowPane().dte_Window();
        }
        public static EnvDTE.Window     dte_Window(this ToolWindowPane toolWindowPane)
        {        	
            return new VisualStudio_2010().invokeOnThread(
                ()=>{	 
		                IVsWindowFrame windowFrame = (IVsWindowFrame)toolWindowPane.Frame;						
		                return VsShellUtilities.GetWindowObject(windowFrame);
                    });
        }
        public static string            title(this EnvDTE.Window window)
        { 
            return new VisualStudio_2010().invokeOnThread(
                ()=> window.Caption);
        }
        public static EnvDTE.Window     title(this EnvDTE.Window window, string value)
        {
            return new VisualStudio_2010().invokeOnThread(() =>
                {
                    window.Caption = value;
                    return window;
                });
        }
        public static EnvDTE.Window     width(this EnvDTE.Window window, int value)
        {
            return new VisualStudio_2010().invokeOnThread(() =>
            {
                window.Width = value;
                return window;
            });
        }
        public static EnvDTE.Window     height(this EnvDTE.Window window, int value)
        {
            return new VisualStudio_2010().invokeOnThread(() =>
            {
                window.Height = value;
                return window;
            });
        }
        public static bool              close(this EnvDTE.Window window)
        {
            try
            {
                window.Close(); //will throw exeption if window has been closed
                return true;
            }
            catch (Exception ex)
            {
                ex.log("[in EnvDTE.window.close]");
                return false;
            }
        }        
        public static bool              visible(this EnvDTE.Window window)
        {
            try
            {
                return window.Visible; //will throw exeption if window has been closed
            }
            catch (Exception ex)
            {
                ex.log("[in EnvDTE.window.visible]");
                return false;
            }
        }
        public static EnvDTE.Window     visible(this EnvDTE.Window window, bool value)
        {
            try
            {
                window.Visible = value; //will throw exeption if window has been closed                
                return window;
            }
            catch (Exception ex)
            {
                ex.log("[in EnvDTE.window.visible]");
                return null;
            }
        }
        public static T close_in_NSeconds<T>(this T window, int seconds) where T : EnvDTE.Window
        {
            O2Thread.mtaThread(() => window.wait(seconds * 1000).close());
            return window;
        }        
    }

    public static class VisualStudio_2010_ExtensionMethods_DTE_StatusBar
    { 
        public static string            statusBar(this VisualStudio_2010 visualStudio)
        {
            return visualStudio.dte().StatusBar.Text;
        }
        public static VisualStudio_2010 statusBar(this VisualStudio_2010 visualStudio, string text)
        {
            visualStudio.dte().StatusBar.Text = text;
            return visualStudio;
        }
    }

    public static class VisualStudio_2010_ExtensionMethods_DTE_OutputWindow
    {
        public static EnvDTE.OutputWindowPane outputWindow(this VisualStudio_2010 visualStudio)
        {
            return visualStudio.dte().ToolWindows.OutputWindow.ActivePane;
        }
        public static EnvDTE.OutputWindowPane outputWindow(this VisualStudio_2010 visualStudio, string name)
        {
            try
            {
                return visualStudio.dte().ToolWindows.OutputWindow.OutputWindowPanes.Item(name);
            }
            catch
            {
                "could not find output Window with name: {0}".error(name);
                return null;
            }
        }
        public static EnvDTE.OutputWindowPane outputWindow_Create(this VisualStudio_2010 visualStudio, string name)
        {
            var outputWindow = visualStudio.outputWindow(name);
            if (outputWindow.notNull())
            {
                "[create_OutputWindow] there was already an output window called '{0}' so returning the existing one".debug(name);
                return outputWindow;
            }
            try
            {
                return visualStudio.dte().ToolWindows.OutputWindow.OutputWindowPanes.Add(name);
            }
            catch (Exception ex)
            {
                ex.log("[in create_OutputWindow]");
                return null;                
            }
        }
        public static EnvDTE.OutputWindowPane writeLine(this EnvDTE.OutputWindowPane outputWindow, string text)
        {
            outputWindow.OutputString(text.line());
            return outputWindow;
        }
    }

    public static class VisualStudio_2010_ExtensionMethods_DTE_CommandWindow
    {
        public static EnvDTE.CommandWindow commandWindow(this VisualStudio_2010 visualStudio)
        {
            return visualStudio.dte().ToolWindows.CommandWindow;
        }
        public static EnvDTE.CommandWindow writeLine(this EnvDTE.CommandWindow commandWindow, string text)
        {
            commandWindow.OutputString(text.line());
            return commandWindow;
        }
        public static EnvDTE.CommandWindow sendInput_and_Execute(this EnvDTE.CommandWindow commandWindow, string input)
        {
            commandWindow.SendInput(input,true);
            return commandWindow;
        }
        public static EnvDTE.CommandWindow execute(this EnvDTE.CommandWindow commandWindow, string input)
        {
            commandWindow.sendInput_and_Execute(input);
            return commandWindow;
        }        
    }
    
    public static class VisualStudio_2010_ExtensionMethods_WinFormsIntegration
    { 
    	public static WindowsFormsHost  windowsFormHost(this System.Windows.Forms.Control control)
    	{
    		try
    		{                
	    		var containerControl = control.parent<System.Windows.Forms.ContainerControl>();                
                if (containerControl.isNull()) 
                    return null;
	    		if (containerControl.typeName() == "WinFormsAdapter")
	    			return (WindowsFormsHost)containerControl.field("_host");
                return containerControl.windowsFormHost();
	    	}
	    	catch(Exception ex)
	    	{
	    		ex.log("[in windowsFormHost(this Control control]");	    		
	    	}
	    	return null;    			
    	}
        public static UserControl       userControl(this WindowsFormsHost windowsFormsHost)
        {
            return windowsFormsHost.userControl<UserControl>();
        }
    	public static T                 userControl<T>(this WindowsFormsHost windowsFormsHost) where T :  UserControl
    	{
    		try
    		{
    			return (T)windowsFormsHost.Parent;
    		}
    		catch(Exception ex)
    		{
    			ex.log();
    			return null;
    		}
    		
    	}
        public static ToolWindowPane    toolWindowPane(this UserControl userControl)
        {
            if (VisualStudio_2010.ToolWindowPanes.hasKey(userControl))
                return VisualStudio_2010.ToolWindowPanes[userControl];
            return null;
        }
        public static ToolWindowPane    toolWindowPane(this System.Windows.Forms.Control control)
        {
            return control.windowsFormHost()
                          .userControl()
                          .toolWindowPane();
        }
    }
 
}
