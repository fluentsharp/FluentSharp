// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface ICirClass
    {
        List<ICirAttribute> ClassAttributes { get; set; }       
        Dictionary<string, ICirFunction> dFunctions { get; set; }
        Dictionary<string, ICirClass> dIsSuperClassedBy { get; set; }
        Dictionary<string, ICirClass> dSuperClasses { get; set; }
      
        string Signature { get; set; }
        string Module { get; set; }
        string Name { get; set; }
        string FullName { get; set; }
        string Namespace { get; set; }        

        // Reference to file location (or the source code in most cases)
        string File { get; set; }
        string FileLine { get; set; }

        bool bClassHasMethodsWithControlFlowGraphs { get; set; }
        Dictionary<string, ICirFieldClass> dField_Class { get; set; }
        Dictionary<string, ICirFieldMember> dField_Member { get; set; }
        List<ICirFunction> lcmIsUsedByMethod_Argument { get; set; }
        List<ICirFunction> lcmIsUsedByMethod_InternalVariable { get; set; }
        List<ICirFunction> lcmIsUsedByMethod_ReturnType { get; set; }

        int IsAnonymous { get; set; }
        bool IsAbstract { get; set; }                
        bool IsClass { get; set; }
        bool IsEnum { get; set; }
        bool IsInterface { get; set; }
        bool IsImport { get; set; }
        bool IsNotPublic { get; set; }
        bool IsPublic { get; set; }
        bool HasSecurity { get; set; }

        bool HasBeenProcessedByCirFactory { get; set; }
        string SymbolDef { get; set; }

        string ToString();
        
        ICirClass clone_SimpleMode();

        ICirFunction getFunction(string functionSignature);
    }
}