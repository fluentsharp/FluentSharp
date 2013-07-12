namespace FluentSharp.CoreLib
{
    public static class O2Utils_ExtensionMethods
    {
        public static string encodeAsO2ScriptText(this string textToEncode)
        {
            return textToEncode.encodeAsO2ScriptText(";", 0);
        }
        public static string encodeAsO2ScriptText(this string textToEncode, string lastChar, int extraTabsAfterFirstLine)
        {
            var scriptText = "";
            textToEncode = textToEncode.replace("\"", "\\\"");

            foreach (var originalLine in textToEncode.lines())
                if (originalLine.valid())
                {
                    var newLine = "\"{0}\"+".format(originalLine).line();
                    if (scriptText.valid() && extraTabsAfterFirstLine > 0)
                        newLine = '\t'.repeat(extraTabsAfterFirstLine) + newLine;
                    scriptText += newLine;
                }

            scriptText = scriptText.replaceLast("+", lastChar);
            return scriptText;
        }
    }
}
