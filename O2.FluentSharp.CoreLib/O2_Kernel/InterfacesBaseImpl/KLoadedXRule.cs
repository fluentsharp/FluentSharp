using System.Collections.Generic;
using System.Reflection;
using O2.Interfaces.XRules;

namespace O2.Kernel.Interfaces.XRules
{
    public class KLoadedXRule : ILoadedXRule
    {
        public IXRule XRule { get; set; }
        public string Source { get; set; }

        public Dictionary<XRuleAttribute, MethodInfo> methods { get; set; }

        public KLoadedXRule(IXRule xRule, string source)
        {
            XRule = xRule;
            Source = source;
            methods = new Dictionary<XRuleAttribute, MethodInfo>();
            foreach (var method in DI.reflection.getMethods(XRule.GetType()))
            {
                var attribute = (XRuleAttribute)DI.reflection.getAttribute(method, typeof(XRuleAttribute));
                if (attribute != null && false == methods.ContainsKey(attribute))
                    methods.Add(attribute, method);
            }
        }

        public override string ToString()
        {
            var toString = (XRule != null) ? (XRule.Name ?? "") : "";
            toString += (string.IsNullOrEmpty(Source)) ? "" : "    (" + Source + ")";
            return toString;
        }
    }
}