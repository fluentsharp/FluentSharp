// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;



namespace FluentSharp.CoreLib.API
{
    public class O2CmdHide : Attribute
    { }

    public class O2CmdExec
    {
        public static void execArgs(string[] args)
        {
            // add these methods by default
            O2CmdApi.typesWithCommands.Add(typeof(O2Cmd)); 
            try
            {
                O2Cmd.welcome();

                string methodName;
                var methodParams = new List<String>();
                switch (args.Length)
                {
                    case 0:
                        O2Cmd.help();                        
                        return;                        
                        //case 1:
                        //methodName = args[0];
                        //break;
                    default:
                        methodName = getMethodName(args[0]);
                        methodParams.AddRange(args);
                        methodParams.RemoveAt(0);
                        break;
                }

                MethodInfo methodToInvoke = O2CmdApi.getMethod(methodName, methodParams.ToArray());
                if (methodToInvoke == null)
                    O2Cmd.log.error("Could not find command to execute: {0}", methodName);
                else                    
                    // ReSharper disable CoVariantArrayConversion
                    methodToInvoke.Invoke(null, methodParams.ToArray());
                    // ReSharper restore CoVariantArrayConversion

                O2Cmd.log.write("\n\n                                  All Done... exiting now...\n");
                O2Cmd.log.write("                                       Have a good Day\n");
            }
            catch (Exception ex)
            {
                O2Cmd.log.error("Error in O2CmdExec.execArgs: {0}", ex.Message);
                if (ex.InnerException != null)
                {
                    O2Cmd.log.error("Error in O2CmdExec.execArgs: {0}\n{1}", ex.InnerException.Message, ex.InnerException.StackTrace);

                }
                PublicDI.log.ex(ex, "in O2.Cmd.FindingsFilter.Main");
            }
        }

        private static string getMethodName(string arg0)
        {
            string[] splittedArg0 = arg0.Split('!');
            switch (splittedArg0.Length)
            {
                case 2:
                    string file = splittedArg0[0];
                    string methodName = splittedArg0[1];
                    O2Cmd.log.write("Trying to load file {0} and execute method {1}", file, methodName);
                    switch (Path.GetExtension(file))
                    {
                        case ".cs":
                            O2Cmd.log.write("\n*.cs file provided, compiling code\n");
                            Assembly assembly = new CompileEngine().compileSourceFile(file);
                            if (assembly == null)
                                O2Cmd.log.write("Error: could not compile provided code");
                            else
                            {
                                O2Cmd.log.write("Source code successfully compiled, loading types\n");
                                foreach (Type typeToLoad in assembly.GetTypes())
                                    O2CmdApi.typesWithCommands.Add(typeToLoad);
                                O2Cmd.log.write("\nHere is the updated list of available command line methods\n");
                                O2Cmd.help();
                                O2Cmd.log.write("\nExecuting method:{0}\n", methodName);
                                return methodName;
                            }
                            break;
                        case ".dll":
                            break;
                        default:
                            O2Cmd.log.error("ERROR: unsupported file extension (it must be *.cs or *.dll");
                            break;
                    }
                    return "";
                default:
                    return arg0;
            }
            //return arg0;
        }
    }
}
