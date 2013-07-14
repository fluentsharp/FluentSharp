using System.IO;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{
    public class RegisterWindowsExtension_WinForms : RegisterWindowsExtension
    {
        public static void register_CurrentApplication(string extension, string name, string icon_VirtualPath)
        {
            register(extension, name, Path.Combine( Application.StartupPath, icon_VirtualPath), Application.ExecutablePath);
        }
        public static void registerO2Extensions()
        {
            register_CurrentApplication(".h2", "O2 Platform v.4.1", @"Icons\H2Logo.ico");
            register_CurrentApplication(".o2", "O2 Platform v.4.1", @"Icons\O2Logo.ico");			
        }
    }
}