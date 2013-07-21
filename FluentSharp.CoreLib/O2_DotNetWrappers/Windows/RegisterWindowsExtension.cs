using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace FluentSharp.CoreLib.API
{
    public class RegisterWindowsExtension
	{
		[DllImport("Shell32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		public static extern void SHChangeNotify(UInt32 wEventId, UInt32 uFlags, IntPtr dwItem1, IntPtr dwItem2);

		static UInt32 SHCNE_ASSOCCHANGED = 0x8000000;
		static UInt32 SHCNF_IDLIST = 0;
         		

		public static void raiseChangeNotify()
		{		
			SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
				
		}
				
        public static bool register(string extension, string name, string icon_Path, string exe_Path)
        {
            try
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
                    if (currentValue.contains("O2Platform.exe", "O2 Platform.exe").isFalse()) // don't do this for the O2 exes
                    {
                        "Value of key {0} was '{1}' so setting it to '{2}'".info(key_command.Name, currentValue, expectedValue);
                        key_command.SetValue("", expectedValue);
                        key_command.Close();

                        var key_defaultIcon = key_Extension.CreateSubKey("DefaultIcon");
                        key_defaultIcon.SetValue("", icon_Path);
                        key_defaultIcon.Close();

                        key_Extension.Close();

                        raiseChangeNotify();
                        return true;
                    }
            }
            catch(Exception ex)
            {
                ex.log("[RegisterWindowsExtension][register]");                
            }
            return false;
        }

	}
}
