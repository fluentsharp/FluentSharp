// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Reflection;

namespace O2.DotNetWrappers.O2CmdShell
{
    public class O2CmdApi
    {
        public static List<Type> typesWithCommands = new List<Type>();

        public static MethodInfo getMethod(string methodName, string[] methodParameter)
        {
            foreach (Type type in typesWithCommands)
            {
                MethodInfo methodToexecute = DI.reflection.getMethod(type, methodName, methodParameter);
                if (methodToexecute != null)
                    return methodToexecute;
            }
            return null;
        }
    }
}
