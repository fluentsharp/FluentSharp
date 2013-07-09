// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.CoreLib.API
{
    public class O2RulePack 
    {
        public string RulePackName { get; set; }
        public List<O2Rule> O2Rules { get; set; }
        
        public O2RulePack()
        {
            RulePackName = "O2RulePack";
            O2Rules = new List<O2Rule>();
        }

        public O2RulePack(string rulePackName) : this()
        {
            RulePackName = rulePackName;
        }

        public O2RulePack(string rulePackName,  List<O2Rule>o2Rules)
        {
            RulePackName = rulePackName;
            O2Rules = o2Rules;
        }

        public O2RulePack(string rulePackName,  IEnumerable<IO2Rule> o2Rules)
        {
            RulePackName = rulePackName;
            O2Rules = getO2RuleList(o2Rules);//_o2Rules.Cast<O2Rule>().ToList();
        }
           
        public List<IO2Rule> getIO2Rules()
        {
            return getIO2RulesList(O2Rules);  //  o2Rules.Cast<IO2Rule>().ToList();            
        }

        private static List<IO2Rule> getIO2RulesList(IEnumerable<O2Rule> o2RuleList)
        {
            var o2Rules = new List<IO2Rule>();
            foreach (var o2Rule in o2RuleList)
                o2Rules.Add(o2Rule);
            return o2Rules;
        }

        private static List<O2Rule> getO2RuleList(IEnumerable<IO2Rule> o2Rules)
        {
            var o2RuleList = new List<O2Rule>();
            foreach (var o2Rule in o2Rules)
                o2RuleList.Add((O2Rule)o2Rule);
            return o2RuleList;
        }      
        
    }
}
