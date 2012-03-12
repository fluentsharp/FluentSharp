// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
//using System.Linq;
using O2.Interfaces.Rules;

namespace O2.Kernel.InterfacesBaseImpl
{
    public class O2RulePack 
    {
        public string RulePackName { get; set; }
        public List<O2Rule> o2Rules { get; set; }
        
        public O2RulePack()
        {
            RulePackName = "O2RulePack";
            o2Rules = new List<O2Rule>();
        }

        public O2RulePack(string rulePackName) : this()
        {
            RulePackName = rulePackName;
        }

        public O2RulePack(string rulePackName,  List<O2Rule>_o2Rules)
        {
            RulePackName = rulePackName;
            o2Rules = _o2Rules;
        }

        public O2RulePack(string rulePackName,  IEnumerable<IO2Rule> _o2Rules)
        {
            RulePackName = rulePackName;
            o2Rules = getO2RuleList(_o2Rules);//_o2Rules.Cast<O2Rule>().ToList();
        }
           
        public List<IO2Rule> getIO2Rules()
        {
            return getIO2RulesList(o2Rules);  //  o2Rules.Cast<IO2Rule>().ToList();            
        }

        private static List<IO2Rule> getIO2RulesList(IEnumerable<O2Rule> o2RuleList)
        {
            var _o2Rules = new List<IO2Rule>();
            foreach (var o2Rule in o2RuleList)
                _o2Rules.Add(o2Rule);
            return _o2Rules;
        }

        private static List<O2Rule> getO2RuleList(IEnumerable<IO2Rule> _o2Rules)
        {
            var o2RuleList = new List<O2Rule>();
            foreach (var o2Rule in _o2Rules)
                o2RuleList.Add((O2Rule)o2Rule);
            return o2RuleList;
        }      
        
    }
}
