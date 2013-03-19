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
using O2.DotNetWrappers.DotNet;
using System.Reflection;

namespace O2.FluentSharp.VisualStudio
{
    [Guid(VisualStudio_2010.PackageGUID)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("O2 Platform VisualStudio", "See http://o2platform.com for more details", VisualStudio_2010.PackageGUID)]
    [ProvideToolWindow(typeof(WindowPane_WPF), MultiInstances = true, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Top, Window = EnvDTE.Constants.vsWindowKindOutput)]
    [ProvideToolWindow(typeof(WindowPane_WinForms), MultiInstances = true, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Top, Window = EnvDTE.Constants.vsWindowKindSolutionExplorer)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]              // ensures this gets called on VisualStudio start
    public class NoSolution_Package : Package
    {

        public NoSolution_Package()
        {
			//O2ConfigSettings.O2Version = "VisualStudio_v1.5.4";
            //Trace.WriteLine("In NoSolution_Package ctor");
        }

        /*public void showErrorInOutputWindow(Exception exToShow)
        {
            try
            {
                var dte = (EnvDTE80.DTE2)this.GetService(typeof(EnvDTE.DTE));
                var outputWindow = dte.ToolWindows.OutputWindow.OutputWindowPanes.Add("O2 Platform - FluentSharp");
                outputWindow.OutputString("[O2.FLuentSharp.VisualStudio] Error: " + exToShow.Message);
            }
            catch (Exception ex)
            {
                
                Debug.WriteLine("[O2.FLuentSharp.VisualStudio] Error in showErrorInOutputWindow: " + ex.Message);
            }
        }*/
        protected override void Initialize()
        {
            try
            {				
                //These two assemblies must be on the localPath
                if (Assembly.Load("FluentSharp.CoreLib") == null || Assembly.Load("FluentSharp.REPL") == null)
                {
                    Debug.WriteLine("[O2.FLuentSharp.VisualStudio] Error in loading FluentSharp.CoreLib.dll or FluentSharp.REPL.dll assemblies");
                    return;
                }
                AssemblyResolver.Init();    //set's up assembly resolver (the O2.FluentSharp.CoreLib.dll must be on local path);
                LocalInitialize();          //run intilializer
                base.Initialize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[O2.FLuentSharp.VisualStudio] Error in Initialize: " + ex.Message);
            }
        }

        //helps to debug cases where there is a assembly load error triggered by the JIT
        public void LocalInitialize()
        {
           
            if (VisualStudio_2010.Initialized.isFalse())
            {
                try
                {
                    if (Control.ModifierKeys == Keys.Shift)
                        VisualStudio_O2_Utils.open_LogViewer();
					
                    populateDefaultVSComObjects();
                    VisualStudio_2010.Initialized = true;
                    //VisualStudio_O2_Utils.compileAndExecuteScript(@"VS_O2_PlugIns\O2_Platform_Gui.cs", "O2_Platform_Gui", "buildGui");

                }
                catch (Exception ex)
                {
                    ex.log("[open_ScriptEditor]");
                    Debug.WriteLine("[open_ScriptEditor] " + ex.Message);
                }
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
            DebuggerEvents = Events.DebuggerEvents;
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
                (Project, ProjectConfig, Platform, SolutionConfig, Success) =>
                {
                    //@"On OnBuildProjConfigDone: project: {0} , ProjectConfig: {1} , Platform: {2},  SolutionConfig: {3} , Success: {4}".debug(Project,ProjectConfig, Platform, SolutionConfig,Success);
                    if (Success)
                        VisualStudio_2010.On_ProjectBuild_OK.invoke(Project);
                    else
                        VisualStudio_2010.On_ProjectBuild_Failed.invoke(Project);
                };

            SolutionEvents.Opened += () => VisualStudio_2010.On_SolutionOpened.invoke();

            DocumentEvents.DocumentOpened += (document) => VisualStudio_2010.on_DocumentOpened.invoke(document);
            DocumentEvents.DocumentClosing += (document) => VisualStudio_2010.on_DocumentClosing.invoke(document);
            DocumentEvents.DocumentSaved += (document) => VisualStudio_2010.on_DocumentSaved.invoke(document);
            DocumentEvents.DocumentOpening += (documentPath, readOnly) => VisualStudio_2010.on_DocumentOpening.invoke(documentPath, readOnly);
            TextEditorEvents.LineChanged += (startPoint, endPoint, hInt) => VisualStudio_2010.on_LineChanged.invoke(startPoint, endPoint);

			WindowEvents.WindowActivated += (windowGotFocus, windowLostFocus) => {
																					if (windowGotFocus.Document.notNull())
																						VisualStudio_2010.on_ActiveDocumentChange.invoke(windowGotFocus.Document);
																				 };

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

        public static T1 getService<T, T1>(this Package package)
        {
            var service = package.invoke("GetService", typeof(T));
            return (T1)service;
        }
    }
}