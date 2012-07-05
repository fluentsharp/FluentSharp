using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class O2Utils_ExtensionMethods
    {
        public static string encodeAsO2ScriptText(this string textToEncode)
        {
            return textToEncode.encodeAsO2ScriptText(";", 0);
        }

        public static string encodeAsO2ScriptText(this string textToEncode, string lastChar, int extraTabsAfter1stLine)
        {
            var scriptText = "";
            textToEncode = textToEncode.replace("\"", "\\\"");

            foreach (var originalLine in textToEncode.lines())
                if (originalLine.valid())
                {
                    var newLine = "\"{0}\"+".format(originalLine).line();
                    if (scriptText.valid() && extraTabsAfter1stLine > 0)
                        newLine = '\t'.repeat(extraTabsAfter1stLine) + newLine;
                    scriptText += newLine;
                }

            scriptText = scriptText.replaceLast("+", lastChar);
            return scriptText;
        }
    }
}
