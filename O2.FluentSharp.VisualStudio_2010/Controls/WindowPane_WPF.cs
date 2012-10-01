using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace O2.FluentSharp.VisualStudio
{

    [Guid("bbe3bf58-bd64-4e05-ac03-d00f1dedc3e5")]
    public class WindowPane_WPF : ToolWindowPane
    {
        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public WindowPane_WPF() :
            base(null)
        {
            this.Caption = "Window WPF";            
            //this.BitmapResourceID = 301;
            //this.BitmapIndex = 1;
            var userControl = new Control_WPF();
            base.Content = userControl;
            VisualStudio_2010.ToolWindowPanes.Add(userControl, this);
        }
    }
}
