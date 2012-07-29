using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;
namespace O2.FluentSharp.VSIX
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.  
    [ProvideToolWindow(typeof(WindowPane_WPF), MultiInstances = true)]
    [ProvideToolWindow(typeof(WindowPane_WinForms), MultiInstances = true)]
//    [ProvideToolWindow(typeof(WindowPane_Default), MultiInstances = true)]
    [Guid(GuidList.guidO2_FluentSharp_VSIXPkgString)]
    public sealed class O2_FluentSharp_VSIXPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public O2_FluentSharp_VSIXPackage()
        {
            try
            {
                "in O2_FluentSharp_VSIXPackage()".alert();
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
            }
            catch (Exception ex)
            {
                ex.log();
            }
            //open.scriptEditor();
            

        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
/*        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = this.FindToolWindow(typeof(MyToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {

                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }*/


        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            //try
            {
                "in O2_FluentSharp_VSIXPackage.Initialize()".alert();

                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
                base.Initialize();

                // Add our command handlers for menu (commands must exist in the .vsct file)
                OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
                if (null != mcs)
                {
                    // Create the command for the menu item.
                    CommandID menuCommandID = new CommandID(GuidList.guidO2_FluentSharp_VSIXCmdSet, (int)PkgCmdIDList.openREPL);
                    MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                    mcs.AddCommand(menuItem);
                }
            }
            /*catch (Exception ex)
            {
                ex.log();
            }*/
        }
        #endregion

        public object getService(Type type)
        {
            return this.GetService(type);
        }

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {

            // Show a Message Box to prove we were here
            uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));                        
            dte = (EnvDTE80.DTE2)GetService(typeof(EnvDTE.DTE));
            errorListProvider = new ErrorListProvider(this);

            //var addit= (EnvDTE.AddIn)GetService(typeof(EnvDTE.AddIn));
            
            vsixPackage = this;
                        
            if (openEditor)
            {
                O2.Kernel.open.scriptEditor();
                openEditor = false;
            }


            //showWindow_WinForms();
           // showWindow_WPF();
        }
        public static ErrorListProvider errorListProvider;
        public static O2_FluentSharp_VSIXPackage vsixPackage;
        public static IVsUIShell uiShell;
        public static EnvDTE80.DTE2 dte;                

        public int id = 0;
        public bool openEditor = true;

        public WindowPane_WinForms showWindow_WinForms()
        {
            var window = (WindowPane_WinForms)this.CreateToolWindow(typeof(WindowPane_WinForms), id++);
            if (window != null)
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
                return window;
            }
            return null;
        }

        public WindowPane_WPF showWindow_WPF()
        {
            //var a = this.CreateToolWindow(typeof(WindowPane_WPF), id++);
            var window = (WindowPane_WPF)this.CreateToolWindow(typeof(WindowPane_WPF), id++);
            if (window != null)
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
                return window;
            }
            return null;
        }

    }
}
