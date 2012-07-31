using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using O2.Kernel;
using O2.DotNetWrappers.ExtensionMethods;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Design;


namespace O2.FluentSharp
{
    /// <summary>
    /// Wrapper class for a number of VisualStudio Objects and methods that have to be created via MEF Extensions
    /// </summary>
    public class VisualStudio_2010
    {
        public const string PackageGUID      = "E0640001-BDB6-4946-84C1-03A58367895A";

        public static Package               Package                 { get; set; }        
        public static ErrorListProvider     ErrorListProvider       { get; set; }        
        public static EnvDTE80.DTE2         DTE2                    { get; set; }
        public static IVsUIShell            IVsUIShell              { get; set; }
        public static OleMenuCommandService OleMenuCommandService   { get; set; }

        /*public static List<Action> On_NoSolution { get; set; }
        public static List<Action> On_CodeWindow { get; set; }
        public static List<Action> On_SolutionExists { get; set; }
        public static List<Action> On_Debugging { get; set; }
        public static List<Action> On_NotBuildingAndNotDebugging { get; set; }
        public static List<Action> On_SolutionBuilding { get; set; }*/
        public static List<Action> On_SolutionOpened { get; set; }
        public static List<Action> On_BuildBegin { get; set; }
        public static List<Action> On_BuildDone { get; set; }

        public static List<Action<string>> On_ProjectBuild_OK { get; set; }
        public static List<Action<string>> On_ProjectBuild_Failed { get; set; }

        public static List<WindowPane_WPF>      CreatedToolWindows_WPF { get; set; }
        public static List<WindowPane_WinForms> CreatedToolWindows_WinForms { get; set; }

        static VisualStudio_2010()
        {            
            Trace.WriteLine("In VisualStudio_2010 static ctor");
            /*            On_NoSolution = new List<Action>();
                        On_CodeWindow = new List<Action>();
                        On_SolutionExists = new List<Action>();
                        On_Debugging = new List<Action>();
                        On_NotBuildingAndNotDebugging = new List<Action>();
                        On_SolutionBuilding = new List<Action>();*/
            On_SolutionOpened = new List<Action>();
            On_BuildBegin = new List<Action>();
            On_BuildDone = new List<Action>();
            On_ProjectBuild_OK = new List<Action<string>>();
            On_ProjectBuild_Failed = new List<Action<string>>();
            CreatedToolWindows_WPF = new List<WindowPane_WPF>();
            CreatedToolWindows_WinForms = new List<WindowPane_WinForms>();
        }
    }
    
    [Guid(VisualStudio_2010.PackageGUID)]
    [PackageRegistration(UseManagedResourcesOnly = true)]    
    [InstalledProductRegistration("O2 Platform VisualStudio","See http://o2platform.com for more details", VisualStudio_2010.PackageGUID)]    
    //[ProvideToolWindow(typeof(WindowPane_WPF), MultiInstances = true)]
    //[ProvideToolWindow(typeof(WindowPane_WinForms), MultiInstances = true)]
    [ProvideToolWindow(typeof(WindowPane_WPF)     ,  MultiInstances = true,  Style=VsDockStyle.Linked, Orientation = ToolWindowOrientation.Top, Window=EnvDTE.Constants.vsWindowKindOutput)]
    [ProvideToolWindow(typeof(WindowPane_WinForms), MultiInstances = true, Style = VsDockStyle.Linked, Orientation = ToolWindowOrientation.Top, Window = EnvDTE.Constants.vsWindowKindSolutionExplorer)]//  "3ae79031-e1bc-11d0-8f78-00a0c9110057")] //EnvDTE.Constants.vsProjectKindSolutionItems)]
    
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]              // this should be the first one to be called    
    public class NoSolution_Package : Package
    {
        public NoSolution_Package()
        {
            Trace.WriteLine("In NoSolution_Package ctor");
        }

        protected override void Initialize()
        {
            try
            {
                
                //open.logViewer();
                
                //if (Control.ModifierKeys == Keys.Shift)
                //    VisualStudio_O2_Utils.open_ScriptEditor();
                populateDefaultVSComObjects();

                //VisualStudio_O2_Utils.open_ScriptEditor();

                //VisualStudio_O2_Utils.createO2PlatformMenu();
                if (Control.ModifierKeys == Keys.Shift)
                    VisualStudio_O2_Utils.createO2PlatformDockWindow();

                VisualStudio_O2_Utils.installO2Scripts_IfDoesntExist();
                //   if (Control.ModifierKeys == Keys.Shift)            
                //       logViewer.closeForm();

                //VisualStudio_2010.On_NoSolution.invoke();
                
                base.Initialize();
            }
            catch (Exception ex)
            {
                ex.log("[open_ScriptEditor]");
                Debug.WriteLine("[open_ScriptEditor] " + ex.Message);
            }            
        }

        public static EnvDTE.Events Events;
        public static EnvDTE.BuildEvents BuildEvents;
        public static EnvDTE.SolutionEvents SolutionEvents;

        public void populateDefaultVSComObjects()
        {

            VisualStudio_2010.Package = this;
            VisualStudio_2010.ErrorListProvider = new ErrorListProvider(this);
            VisualStudio_2010.IVsUIShell = this.getService<IVsUIShell>();
            VisualStudio_2010.DTE2 = this.getService<EnvDTE.DTE>() as EnvDTE80.DTE2;
            VisualStudio_2010.OleMenuCommandService = this.getService<OleMenuCommandService>();

            Events = VisualStudio_2010.DTE2.Events;
            BuildEvents = Events.BuildEvents;
            SolutionEvents = Events.SolutionEvents;

            BuildEvents.OnBuildBegin += (scope, action) => VisualStudio_2010.On_BuildBegin.invoke();
            BuildEvents.OnBuildDone += (scope, action) => VisualStudio_2010.On_BuildDone.invoke();            



            BuildEvents.OnBuildProjConfigDone += 
	            (Project , ProjectConfig , Platform , SolutionConfig , Success ) =>
	                {                        
		                @"On OnBuildProjConfigDone: project: {0} , ProjectConfig: {1} , Platform: {2},  SolutionConfig: {3} , Success: {4}".debug(Project,ProjectConfig, Platform, SolutionConfig,Success);
                        if (Success)
                            VisualStudio_2010.On_ProjectBuild_OK.invoke(Project);
                        else
                            VisualStudio_2010.On_ProjectBuild_Failed.invoke(Project);
	                };

            SolutionEvents.Opened +=
                () =>
                {
                    VisualStudio_2010.On_SolutionOpened.invoke();
                    //"SolutionEvents Opened".alert();
                    //"Solution Opened".error();                    
                };

        }

        protected override int QueryClose(out bool canClose)
        {
            foreach (var toolWindowPane in VisualStudio_2010.CreatedToolWindows_WPF)
            {
                try
                {
                    (toolWindowPane.Frame as IVsWindowFrame).Hide();
                }
                catch (Exception ex)
                {
                    ex.log("[in QueryClose]");
                }
            }
            foreach (var toolWindowPane in VisualStudio_2010.CreatedToolWindows_WinForms)
            {
                try
                {
                    (toolWindowPane.Frame as IVsWindowFrame).Hide();
                }
                catch (Exception ex)
                {
                    ex.log("[in QueryClose]");
                }
            }
            canClose = true;
            return 1;
        }
        
    }

    public static class Package_ExtensionMethod
    {
        public static T getService<T>(this Package package)
        {
            return (T)package.invoke("GetService", typeof(T));
        }
    }


}
