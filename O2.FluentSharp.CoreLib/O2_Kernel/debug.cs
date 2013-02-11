using System.Diagnostics;

namespace O2.Kernel
{
    public class debug
    {        
        public static void _break()
        {
            Debugger.Break();
        }        
    }
}
