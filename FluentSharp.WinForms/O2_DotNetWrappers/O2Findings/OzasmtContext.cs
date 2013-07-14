// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.O2Findings
{
    public class OzasmtContext
    {
        public static string getVariableNameFromThisObject(IO2Finding o2Finding)
        {
            return getVariableNameFromThisObject(o2Finding.context);
        }

        public static string getVariableNameFromThisObject(IO2Trace o2Trace)
        {
            return getVariableNameFromThisObject(o2Trace.context);
        }

        public static string getVariableNameFromThisObject(string context)
        {
            int indexOfSpace = context.IndexOf(' ');
            if (indexOfSpace > 0)
            {
            }
            string variable = context.Substring(0, indexOfSpace);
            variable = variable.Replace("this->", "");
            return variable;
        }
    }
}
