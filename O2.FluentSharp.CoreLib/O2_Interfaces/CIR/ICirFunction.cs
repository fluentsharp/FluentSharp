// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface ICirFunction
    {
        List<ICirAttribute> FunctionAttributes { get; set; }
        List<ICirAttribute> FunctionParameterAttributes { get; set; }       
  
        List<ICirFunction>          FunctionsCalledUniqueList { get; set; }
        List<ICirFunctionCall>      FunctionsCalled { get; set; }
                
        List<ICirFunctionCall>      FunctionIsCalledBy { get; set; }
        List<ICirFunctionParameter> FunctionParameters { get; set; }

        ICirClass ParentClass { get; set; }

        string FunctionSignature { get; set; }
        string ReturnType { get; set; }        
        string FunctionNameAndParameters { get; set; }
        string ClassNameFunctionNameAndParameters { get; set; }
        string FunctionName { get; set; }        
        string ParentClassFullName { get; set; }
        string ParentClassName { get; set; }
        string Module { get; set; }

        List<string> UsedTypes { get; set; }
        Dictionary<string, ICirSsaVariable> dSsaVariables { get; set; }
        Dictionary<string, ICirFunctionVariable> dVariables { get; set; }       

        bool HasControlFlowGraph { get; set; }
        bool OnlyShowFunctionNameInToString { get; set; }

        string SymbolDef { get; set; }
        string CecilSignature { get; set; }        
        string ReflectionSignature { get; set; }
        string O2MDbgSignature { get; set; }

        // Reference to file location (or the source code in most cases)
        string File { get; set; }
        string FileLine { get; set; }

        bool IsTainted { get; set; }            // to be used to indicate functions that should be marked as callbacks

        bool IsPrivate { get; set; }
        bool IsStatic { get; set; }
        bool IsConstructor { get; set; }
        bool IsUnmanaged { get; set; }
        bool IsUnmanagedExport { get; set; }
        bool IsVirtual { get; set; }
        bool IsSetter { get; set; }
        bool IsGetter { get; set; }
        bool IsRuntime { get; set; }
        bool IsPublic { get; set; }
        bool IsPInvokeImpl { get; set; }
        bool IsNative { get; set; }
        bool IsManaged { get; set; }
        bool IsInternalCall { get; set; }
        bool IsIL { get; set; }
        bool IsAbstract { get; set; }
        bool HasSecurity { get; set; }
        bool HasBody { get; set; }

        string ToString();

        bool HasBeenProcessedByCirFactory { get; set; }
    }
}