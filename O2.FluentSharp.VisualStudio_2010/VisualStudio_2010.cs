using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel;
using O2.DotNetWrappers.ExtensionMethods;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;

namespace O2.FluentSharp
{
    /// <summary>
    /// Wrapper class for a number of VisualStudio Objects and methods that have to be created via MEF Extensions
    /// </summary>
    public class VisualStudio_2010
    {
        public static Package               Package                 { get; set; }        
        public static ErrorListProvider     ErrorListProvider       { get; set; }
        public static EnvDTE80.DTE2         DTE2
        {
            get 
            {
                return Package.getService<EnvDTE.DTE>() as EnvDTE80.DTE2;
            }
        }
        public static IVsUIShell            IVsUIShell
        {
            get
            {
                return Package.getService<IVsUIShell>();
            }
        }
        public static OleMenuCommandService OleMenuCommandService
        {
            get 
            {
                return Package.getService<OleMenuCommandService>();
            }
        }

        static VisualStudio_2010()
        {
            Debug.WriteLine("In VisualStudio_2010 static ctor");
            Trace.WriteLine("In VisualStudio_2010 static ctor");
        }
    }

    
    //[ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [PackageRegistration]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]             // check if this also fires when VS is opened via a solution        
    public class DefaultPackage : Package
    {
        static DefaultPackage()
        {
            Debug.WriteLine("In DefaultPackage static ctor");
            Trace.WriteLine("In DefaultPackage static ctor");
        }
        public DefaultPackage()
        {
            if (Control.ModifierKeys == Keys.Shift)
                open.logViewer();
            try
            {                                
                VisualStudio_2010.Package               = this;                
                VisualStudio_2010.ErrorListProvider     = new ErrorListProvider(this);                

                VisualStudio_O2_Utils.open_ScriptEditor();
            }
            catch (Exception ex)
            {
                ex.log("[open_ScriptEditor]");
                Debug.WriteLine("[open_ScriptEditor] " + ex.Message);
            }            
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
