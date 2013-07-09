using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using Microsoft.Win32;
using System.IO;


namespace FluentSharp.BCL.Utils
{
	public class RegisterWindowsExtension
	{
		[DllImport("Shell32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		public static extern void SHChangeNotify(UInt32 wEventId, UInt32 uFlags, IntPtr dwItem1, IntPtr dwItem2);

		static UInt32 SHCNE_ASSOCCHANGED = 0x8000000;
		static UInt32 SHCNF_IDLIST = 0;

         
		public static void registerO2Extensions()
		{
			RegisterWindowsExtension.register_CurrentApplication(".h2", "O2 Platform v.4.1", @"Icons\H2Logo.ico");
			RegisterWindowsExtension.register_CurrentApplication(".o2", "O2 Platform v.4.1", @"Icons\O2Logo.ico");			
		}

		public static void raiseChangeNotify()
		{		
			SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
				
		}
		
		public static void register_CurrentApplication(string extension, string name, string icon_VirtualPath)
		{

			register(extension, name, Path.Combine( Application.StartupPath, icon_VirtualPath), Application.ExecutablePath);
		}

        public static void register(string extension, string name, string icon_Path, string exe_Path)
        {
            var classesRoot = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Classes", true);

            var key_Extension = classesRoot.CreateSubKey(extension);

            key_Extension.SetValue("", name);
            //key_Extension.SetValue("DefaultIcon", icon_Path);

            var key_command = key_Extension.CreateSubKey("Shell\\Open\\command", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //key = key.CreateSubKey("command");
            var expectedValue = "\"" + exe_Path + "\" \"%L\"";
            var currentValue = (string)key_command.GetValue("");
            if (expectedValue != currentValue)                          // only do it if not there
                if (currentValue.Contains("O2 Platform.exe").isFalse()) // don't do this for the O2 exes
                {
                    "Value of key {0} was '{1}' so setting it to '{2}'".info(key_command.Name, currentValue, expectedValue);
                    key_command.SetValue("", expectedValue);
                    key_command.Close();

                    var key_defaultIcon = key_Extension.CreateSubKey("DefaultIcon");
                    key_defaultIcon.SetValue("", icon_Path);
                    key_defaultIcon.Close();

                    key_Extension.Close();

                    raiseChangeNotify();
                }
        }

	}
}
