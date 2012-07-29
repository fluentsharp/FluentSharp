// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2.External.WinFormsUI.Forms
{
    public static class O2AscxGUI_Ext
    {
        public static object invokeOnAscx(this string ascxName, string methodToExecute)
        {
            return invokeOnAscx(ascxName, methodToExecute, new object[0]);
        }

        public static object invokeOnAscx(this string ascxName, string methodToExecute, object[] methodParameters)
        {
            return O2AscxGUI.invokeOnAscxControl(ascxName, methodToExecute, methodParameters);
        }
    }
}
