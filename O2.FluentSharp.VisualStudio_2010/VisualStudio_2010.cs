using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using WPF_UserControl = System.Windows.Controls.UserControl;
using O2.Kernel;
using O2.DotNetWrappers.ExtensionMethods;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Design;
using O2.Views.ASCX.Ascx.MainGUI;


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
        public static List<Action<EnvDTE.Document>>  on_DocumentOpened { get; set; }
        public static List<Action<EnvDTE.Document>>  on_DocumentClosing { get; set; }
        public static List<Action<EnvDTE.Document>>  on_DocumentSaved { get; set; }
        public static List<Action<string, bool>>     on_DocumentOpening { get; set; }
        public static List<Action<EnvDTE.TextPoint, EnvDTE.TextPoint>> on_LineChanged { get; set; }

        //public static Form O2_LogViewer { get; set; }

        public static Dictionary<WPF_UserControl, ToolWindowPane> ToolWindowPanes { get; set; }        

        static VisualStudio_2010()
        {            
            Trace.WriteLine("In VisualStudio_2010 static ctor");            
            On_SolutionOpened = new List<Action>();
            On_BuildBegin = new List<Action>();
            On_BuildDone = new List<Action>();
            On_ProjectBuild_OK = new List<Action<string>>();
            On_ProjectBuild_Failed = new List<Action<string>>();
            on_DocumentOpened = new List<Action<EnvDTE.Document>>();
            on_DocumentClosing = new List<Action<EnvDTE.Document>>();
            on_DocumentSaved = new List<Action<EnvDTE.Document>>();
            on_DocumentOpening = new List<Action<string, bool>>();
            on_LineChanged = new List<Action<EnvDTE.TextPoint, EnvDTE.TextPoint>>();

            ToolWindowPanes = new Dictionary<WPF_UserControl, ToolWindowPane>();            
        }
    }
    
    [Guid(VisualStudio_2010.PackageGUID)]
    [PackageRegistration(UseManagedResourcesOnly = true)]    
    [InstalledProductRegistration("O2 Platform VisualStudio","See http://o2platform.com for more details", VisualStudio_2010.PackageGUID)]    
    //[ProvideToolWindow(typeof(WindowPane_WPF), MultiInstances = true)]
    //[ProvideToolWindow(typeof(WindowPane_WinForms), MultiInstances = true)]
    [ProvideToolWindow(typeof(WindowPane_WPF)     , MultiInstances = true, Style=VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Top, Window = EnvDTE.Constants.vsWindowKindOutput)]
    [ProvideToolWindow(typeof(WindowPane_WinForms), MultiInstances = true, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Top, Window = EnvDTE.Constants.vsWindowKindSolutionExplorer)]    
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
                //if (Control.ModifierKeys == Keys.Shift)
                    VisualStudio_O2_Utils.open_LogViewer();
                populateDefaultVSComObjects();

                //VisualStudio_2010.O2_LogViewer = open.logViewer().parentForm();
                
                //if (Control.ModifierKeys == Keys.Shift)
                //    VisualStudio_O2_Utils.open_ScriptEditor();
                

                //VisualStudio_O2_Utils.open_ScriptEditor();

                VisualStudio_O2_Utils.createO2PlatformMenu();

                

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
        public static EnvDTE.CommandEvents CommandEvents;
        public static EnvDTE.DebuggerEvents DebuggerEvents;
        public static EnvDTE.DocumentEvents DocumentEvents;
        public static EnvDTE.DTEEvents DTEEvents;
        public static EnvDTE.FindEvents FindEvents;
        public static EnvDTE.ProjectItemsEvents MiscFilesEvents;
        public static EnvDTE.OutputWindowEvents OutputWindowEvents;
        public static EnvDTE.SelectionEvents SelectionEvents;
        public static EnvDTE.ProjectItemsEvents SolutionItemsEvents;        
        public static EnvDTE.SolutionEvents SolutionEvents;
        public static EnvDTE.TaskListEvents TaskListEvents;
        public static EnvDTE.TextEditorEvents TextEditorEvents;
        public static EnvDTE.WindowEvents WindowEvents;        

        public void populateDefaultVSComObjects()
        {

            VisualStudio_2010.Package = this;
            VisualStudio_2010.ErrorListProvider = new ErrorListProvider(this);
            VisualStudio_2010.IVsUIShell = this.getService<IVsUIShell>();
            VisualStudio_2010.DTE2 = this.getService<EnvDTE.DTE, EnvDTE80.DTE2>();
            VisualStudio_2010.OleMenuCommandService = this.getService<OleMenuCommandService>();

            Events = VisualStudio_2010.DTE2.Events;

            BuildEvents = Events.BuildEvents;                        
            CommandEvents = Events.CommandEvents;
            DebuggerEvents =Events.DebuggerEvents;
            DocumentEvents = Events.DocumentEvents;
            DTEEvents = Events.DTEEvents;
            FindEvents = Events.FindEvents;
            MiscFilesEvents = Events.MiscFilesEvents;
            OutputWindowEvents = Events.OutputWindowEvents;
            SelectionEvents = Events.SelectionEvents;                        
            SolutionEvents = Events.SolutionEvents;
            SolutionItemsEvents = Events.SolutionItemsEvents;
            TaskListEvents = Events.TaskListEvents;
            TextEditorEvents = Events.TextEditorEvents;
            WindowEvents = Events.WindowEvents;
            


            BuildEvents.OnBuildBegin += (scope, action) => VisualStudio_2010.On_BuildBegin.invoke();
            BuildEvents.OnBuildDone += (scope, action) => VisualStudio_2010.On_BuildDone.invoke();            

            BuildEvents.OnBuildProjConfigDone += 
	            (Project , ProjectConfig , Platform , SolutionConfig , Success ) =>
	                {                        
		                //@"On OnBuildProjConfigDone: project: {0} , ProjectConfig: {1} , Platform: {2},  SolutionConfig: {3} , Success: {4}".debug(Project,ProjectConfig, Platform, SolutionConfig,Success);
                        if (Success)
                            VisualStudio_2010.On_ProjectBuild_OK.invoke(Project);
                        else
                            VisualStudio_2010.On_ProjectBuild_Failed.invoke(Project);
	                };

            SolutionEvents.Opened += () => VisualStudio_2010.On_SolutionOpened.invoke();

            DocumentEvents.DocumentOpened+= (document)=> VisualStudio_2010.on_DocumentOpened.invoke(document);
            DocumentEvents.DocumentClosing+= (document)=> VisualStudio_2010.on_DocumentClosing.invoke(document);
            DocumentEvents.DocumentSaved+= (document)=> VisualStudio_2010.on_DocumentSaved.invoke(document);
            DocumentEvents.DocumentOpening += (documentPath, readOnly) => VisualStudio_2010.on_DocumentOpening.invoke(documentPath, readOnly);
            TextEditorEvents.LineChanged += (startPoint, endPoint, hInt) => VisualStudio_2010.on_LineChanged.invoke(startPoint, endPoint);
        }

        protected override int QueryClose(out bool canClose)
        {
            foreach (var toolWindowPane in VisualStudio_2010.ToolWindowPanes.Values)
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
            /*foreach (var toolWindowPane in VisualStudio_2010.CreatedToolWindows_WinForms.Values)
            {
                try
                {
                    (toolWindowPane.Frame as IVsWindowFrame).Hide();
                }
                catch (Exception ex)
                {
                    ex.log("[in QueryClose]");
                }
            }*/
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

        public static T1 getService<T,T1>(this Package package)
        {
           var service = package.invoke("GetService", typeof(T));
           return (T1)service;
        }
    }


}
