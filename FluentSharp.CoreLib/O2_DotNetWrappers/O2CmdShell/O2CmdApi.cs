// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Reflection;


namespace FluentSharp.CoreLib.API
{
    public class O2CmdApi
    {
        public static List<Type> typesWithCommands = new List<Type>();

        public static MethodInfo getMethod(string methodName, string[] methodParameter)
        {
            foreach (var type in typesWithCommands)
            {
                // ReSharper disable CoVariantArrayConversion
                var methodToexecute = PublicDI.reflection.getMethod(type, methodName, methodParameter);
                // ReSharper restore CoVariantArrayConversion

                if (methodToexecute != null)
                    return methodToexecute;
            }
            return null;
        }
    }
}
