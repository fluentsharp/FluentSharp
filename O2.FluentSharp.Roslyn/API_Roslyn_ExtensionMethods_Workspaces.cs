using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Services;

namespace O2.XRules.Database.APIs
{
    public static class API_Roslyn_ExtensionMethods_Workspaces
    {
        public static IWorkspace workspace(this string solutionFile)
        {
            return Workspace.LoadSolution(solutionFile);
        }
    }
}
