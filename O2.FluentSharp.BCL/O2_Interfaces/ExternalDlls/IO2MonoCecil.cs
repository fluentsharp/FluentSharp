// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using System.Windows.Forms;

namespace O2.Interfaces.ExternalDlls
{
    public interface IO2MonoCecil
    {
        // Core MonoCeil
        object loadAssembly(string assemblyToLoad);
        bool canAssemblyBeLoaded(string assemblyToLoad);
        object getAssemblyEntryPoint(string p);
        //string getMethodDefinitionDeclaringTypeModuleName(object methodDefinition);
        //string getMethodDefinitionDeclaringTypeName(object methodDefinition);
        //string getMethodDefinitionName(object methodDefinition);

        // Using Cecil Decompiler
        string getIL(object methodDefinition);
        string getSourceCode(object methodDefinition);
        string getILfromClonedMethod(object targetMethod);

        // for Window Forms/ASCX
        List<TreeNode> populateTreeNodeWithObjectChilds(TreeNode node, object tag, bool populateFirstChildNodes);
    }
}