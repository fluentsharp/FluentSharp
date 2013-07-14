// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Reflection;

namespace FluentSharp.WinForms.Interfaces
{
    public interface IM_SelectedTypeOrMethod : IO2Message
    {
        string assemblyName { get; set; }
        string typeName { get; set; }
        string methodName { get; set; }
        object[] methodParameters { get; set; }
        MethodInfo methodInfo { get; set; }
    }
}