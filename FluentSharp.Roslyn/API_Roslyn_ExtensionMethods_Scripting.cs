using System;
using System.Linq;
using Roslyn.Scripting.CSharp;

namespace FluentSharp.FluentRoslyn
{
    public static class API_Roslyn_ExtensionMethods_Scripting
    {
        public static dynamic execute(this string code)
        {
            return (dynamic)new ScriptEngine(null,null,null,null).Execute(code,null);
        }
    }
}
