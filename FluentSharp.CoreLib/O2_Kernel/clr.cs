using System;



namespace FluentSharp.CoreLib.API
{
    public class clr
    {
        public static string version()
        {
            return Environment.Version.str();
        }
        public static bool clr2()
        {
            return Environment.Version.Major == 2;
        }

        public static bool clr4()
        {
            return Environment.Version.Major == 4;
        }

        public static bool x86()
        {                
            return IntPtr.Size == 4;
        }

        public static bool x64()
        {
            return IntPtr.Size == 8;
        }

        public static string details()
        {
            return "CLR: {1} in {2} bit process with id: {0} ".format(
                        Processes.getCurrentProcessID(), 
                        clr4() ? "4.0" : "3.5",
                        x86() ? "32" : "64");                       
        }
		public static bool mono()
		{
			return Type.GetType ("Mono.Runtime").isNotNull();
		}
		public static bool not_Mono()
		{
			return mono().isFalse();
		}
    }
}
