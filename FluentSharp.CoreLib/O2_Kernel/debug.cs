using System.Diagnostics;

namespace FluentSharp.CoreLib.API
{
    public class debug
    {        
        public static void _break()
        {
            Debugger.Break();
        }        
    }
}
