// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;

//using O2.Kernel.InterfacesBaseImpl;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface IDatabaseRules
    {
        void DeleteAllRulesFromDatabase();
        List<IO2Rule> createO2RulesForAllLddbEntriesForLanguage(SupportedLanguage language);

        List<IO2Rule> createO2RulesForAllLddbEntriesForLanguage(SupportedLanguage language,
                                                                bool addSources, bool addSinks, bool addCallbacks, bool addPropagateTaint,
                                                                bool addDontPropagateTaint,bool addAnyHigh, bool addAnyMedium, bool addAnyLow);
        List<IO2Rule> addRulesToDatabase(IEnumerable<IO2Rule> o2RulesToAdd);
        //List<IO2Rule> addRulesToDatabase(bool bDeleteDatabase, O2RulePack o2rulePack);
        bool addRuleToDatabase(IO2Rule rRule);
        void deleteRulesFromDatabase(IEnumerable<IO2Rule> rulesToDelete);
        void deleteRuleFromDatabase(IO2Rule ruleToDelete);
        List<IO2Rule> getRules_Sources(string ruleDbId);
        List<IO2Rule> getRules_Sinks(string ruleDbId);
        List<IO2Rule> getRules_DontPropagateTaint(string ruleDbId);
        List<IO2Rule> getRules_PropagateTaint(string ruleDbId);
        List<IO2Rule> getRules_AnyHigh(string ruleDbId);
        List<IO2Rule> getRules_AnyMedium(string ruleDbId);
        List<IO2Rule> getRules_AnyLow(string ruleDbId);
        List<IO2Rule> getRules_VulnType(string vulnTypeInMySql, string addAsRuleOfType, string ruleDbId);
    }
}