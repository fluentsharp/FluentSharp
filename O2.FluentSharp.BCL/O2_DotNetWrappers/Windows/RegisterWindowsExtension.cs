using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace O2.Platform
{
	public class RegisterWindowsExtension
	{
		[DllImport("Shell32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		public static extern void SHChangeNotify(UInt32 wEventId, UInt32 uFlags, IntPtr dwItem1, IntPtr dwItem2);

		static UInt32 SHCNE_ASSOCCHANGED = 0x8000000;
		static UInt32 SHCNF_IDLIST = 0;


		public static void registerO2Extensions()
		{
			RegisterWindowsExtension.register_CurrentApplication(".h2", "O2 Platform v.4", @"Icons\H2Logo.ico");
			RegisterWindowsExtension.register_CurrentApplication(".o2", "O2 Platform v.4", @"Icons\O2Logo.ico");			
		}

		public static void raiseChangeNotify()
		{		
			SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
				
		}
		
		public static void register_CurrentApplication(string extension, string name, string icon_VirtualPath)
		{

			register(extension, name, Path.Combine( Application.StartupPath, icon_VirtualPath), Application.ExecutablePath);
		}

		public static void register(string extension, string name , string icon_Path, string exe_Path)
		{			
			var classesRoot = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Classes", true);

			var key_Extension = classesRoot.CreateSubKey(extension);
			
			key_Extension.SetValue("", name);
			//key_Extension.SetValue("DefaultIcon", icon_Path);

			var key_command = key_Extension.CreateSubKey("Shell\\Open\\command", RegistryKeyPermissionCheck.ReadWriteSubTree);
			//key = key.CreateSubKey("command");

			key_command.SetValue("", "\"" + exe_Path + "\" \"%L\"");
			key_command.Close();

			var key_defaultIcon = key_Extension.CreateSubKey("DefaultIcon");
			key_defaultIcon.SetValue("", icon_Path);
			key_defaultIcon.Close();

			key_Extension.Close();

			raiseChangeNotify();
		}

	}
}
