using System;
using System.Collections.Generic;
using WPF_UserControl = System.Windows.Controls.UserControl;
using O2.Kernel;
using O2.DotNetWrappers.ExtensionMethods;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
//using System.ComponentModel.Design;
//using O2.Views.ASCX.Ascx.MainGUI;


namespace O2.FluentSharp.VisualStudio
{
    /// <summary>
    /// Wrapper class for a number of VisualStudio Objects and methods that have to be created via MEF Extensions
    /// </summary>
    public class VisualStudio_2010
    {        
        public const string PackageGUID = "de85ae01-f53e-464f-9466-aa9089c0ce17";//"E0640001-BDB6-4946-84C1-03A58367895A";
        
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

        public static Dictionary<WPF_UserControl, ToolWindowPane> ToolWindowPanes   { get; set; }
        public static bool                                        Initialized        { get; set; }

        static VisualStudio_2010()
        {                        
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
}
