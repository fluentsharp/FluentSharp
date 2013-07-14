// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;
using FluentSharp.WinForms.Interfaces;

namespace FluentSharp.WinForms.Utils
{
    public class KM_SelectedTypeOrMethod : KO2Message, IM_SelectedTypeOrMethod
    {        
        public string assemblyName { get; set; }
        public string typeName { get; set; }
        public string methodName { get; set; }
        public object[] methodParameters { get; set; }
        public MethodInfo methodInfo { get; set; }

        public KM_SelectedTypeOrMethod()
        {            
            messageText = "KM_FileOrFolderSelected";
        }

        public KM_SelectedTypeOrMethod(MethodInfo _methodInfo) : this()
        {
            methodInfo = _methodInfo;
        }

        public KM_SelectedTypeOrMethod(string _assemblyName, string _typeName, string _methodName, object[] _methodParameters) : this()
        {
            
            assemblyName = _assemblyName;
            typeName = _typeName;
            methodName = _methodName;
            methodParameters = _methodParameters;
        }
      
    }
}
