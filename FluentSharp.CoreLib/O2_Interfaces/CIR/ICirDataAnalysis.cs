// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface ICirDataAnalysis
    {
        Dictionary<ICirClass, ICirData> dCirClass { get; set; }
        Dictionary<string, ICirClass> dCirClass_bySignature { get; set; }
        Dictionary<string, ICirData> dCirDataFilesLoaded { get; set; }
        Dictionary<ICirFunction, ICirClass> dCirFunction { get; set; }
        Dictionary<string, ICirFunction> dCirFunction_bySignature { get; set; }
        List<String> lCirClass_bySuperClass { get; set; }
        String sDb_Id { get; set; }
        bool onlyShowFunctionsOrClassesWithControlFlowGraphs { get; set; }
        bool onlyShowExternalFunctionsThatAreInvokedFromCFG { get; set; }
        bool onlyShowFunctionsWithCallersOrCallees { get; set; }

        //void clear();

        /*ICirData addO2CirDataFile(ICirData sO2CirFileToAdd);
        ICirData addO2CirDataFile(String sO2CirFileToAdd);
        void calculateXrefs_SuperClases();
        void calculateXrefs_SuperClases_recursive(String sSuperClassCustomSignature, ICirClass ccClassToFollow);
        void removeO2CirDataFile(String sO2CirDataFileToRemove);
        void showStatsOfLoadedData();
        ICirDataSearchResult executeSearch();
        */
        List<T> CirClasses<T>();
        List<T> CirFunctions<T>();

        bool Save(string savedCirDataFile);
    }
}