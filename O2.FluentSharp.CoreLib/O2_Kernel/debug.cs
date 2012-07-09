using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using O2.DotNetWrappers.DotNet;

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
