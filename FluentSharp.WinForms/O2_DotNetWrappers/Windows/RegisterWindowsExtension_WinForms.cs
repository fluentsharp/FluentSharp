using System;
using System.IO;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{
    public class RegisterWindowsExtension_WinForms : RegisterWindowsExtension
    {
        public static void register_CurrentApplication(string extension, string name, string icon_VirtualPath)
        {
            try
            {
                register(extension, name, Path.Combine( Application.StartupPath, icon_VirtualPath), Application.ExecutablePath);
            }
            catch(Exception ex)
            {
               ex.log(); 
            }
        }        
    }
}