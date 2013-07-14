// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;
//using O2.Kernel.CodeUtils;

namespace FluentSharp.WinForms.Interfaces
{
    public interface IAssemblyAnalysis
    {
        event Action<string> onMethodSelectedGetILCode;
        event Action<string> onMethodSelectedGetSourceCode;

        bool canAssemblyBeLoaded(string assemblyToLoad);

        void processTreeNodeAndRaiseCallbacks(TreeNode node);
        void populateTreeNodeWithObjectChilds(TreeNode node);
        void populateTreeNodeWithObjectChilds(TreeNode node, object tag, bool populateFirstChildNodes);

        object loadAssemblyUsingMonoCecil(string assemblyToLoad);
        object loadAssemblyUsingReflection(string assemblyToLoad);
        //object loadAssemblyUsingCir(string assemblyToLoad);    
    }
}