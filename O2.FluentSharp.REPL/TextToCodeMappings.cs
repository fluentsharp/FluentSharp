using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using O2.DotNetWrappers.ExtensionMethods;

namespace O2.External.SharpDevelop.AST
{
    public class TextToCodeMappings
    {
        // this code does multiple passes in creating and ast from the code 
        //(in an attempt to make it more user friendly to the user
        public static string tryToFixRawCode(string originalCode, Func<string,string> codeCreationEngine)
        {
            var code = originalCode;//.trim();
            string parsedCode = null;
            string tempCode = null;
            // first see if the code provide is a predefined command
            code = code.eq("notepad") ? "exec.cmd (\"notepad\");" : code;
            code = code.eq("cmd") ? "exec.cmd (\"cmd\");" : code;
            code = (code.starts("dir ") && code.lines().size() == 1) ? "exec.cmdViaConsole (\"cmd\",\"/c {0}\");".format(code) : code;
            // some for fun :)
            code = code.ToLower().Trim().eq("hello") || code.ToLower().Trim().eq("hi") ? "\"Hi, Welcome to O2\"" : code;
            code = code.ToLower().Trim().eq("how are you?") || code.ToLower().Trim().eq("how are you doing") ? "\"I'm fine thanks\"" : code;
            code = code.ToLower().Trim().eq("good morning") || code.ToLower().Trim().eq("good afternoon") ? "\"Hello there, {0} to you too\"".format(code.trim()) : code;
            //code.ToLower().eq(new string[] { "hello", "hi" }, () => code = "Hi, Welcome to O2");
            //code = code.ToLower().eq(new []{"how are you?" ,"how are you doing" }) ? "I'm fine thanks" : code;            
            // first try with the code provided
            parsedCode = codeCreationEngine(code);

            // todo: move logic below to a more optimized and refactored code :)
            //if it failed:
            tempCode = code;
            if (parsedCode == null)
                // and it has one line, wrap it around a return statement;
                if (code.lines().size() == 1 && code.Length >0)
                {
                    if (parsedCode == null && code.starts("=")) 
                    {
                        tempCode = "return {0};".format(code.Substring(1));
                        parsedCode = codeCreationEngine(tempCode);
                    }
                    if (parsedCode == null && tempCode.contains("\"").isFalse())
                    {
                        tempCode = tempCode.contains("()")
                                ? "{0};".format(code)
                                : "{0}();".format(code);
                        parsedCode = codeCreationEngine(tempCode);
                    }
                    if (parsedCode == null)
                    {
                        tempCode = "return {0};".format(code);
                        parsedCode = codeCreationEngine(tempCode);
                    }
                    if (parsedCode == null)
                    {
                        var firstSpace = code.IndexOf(' ');
                        if (firstSpace > 0)
                        {
                            tempCode = code.Insert(firstSpace, "(");
                            tempCode = "{0});".format(tempCode);
                            parsedCode = codeCreationEngine(tempCode);
                        }
                    }

                }
            return parsedCode;

        }
    }
}
