using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Scripting.CSharp;

namespace O2.XRules.Database.APIs
{
    public static class API_Roslyn_ExtensionMethods_Scripting
    {
        public static dynamic execute(this string code)
        {
            return (dynamic)new ScriptEngine().Execute(code);
        }
    }
}
