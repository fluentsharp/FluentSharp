// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Text;

namespace FluentSharp.CoreLib.API
{
    public class vars
    {
        public static event Callbacks.dMethod eVars_OnChange;

        public static Object resolveVar(String sVarKey)
        {
            if (sVarKey.Length > 0 && sVarKey[0] == '$')
            {
                String sResolvedVarKey = sVarKey.Substring(1);
                if (PublicDI.dO2Vars.ContainsKey(sResolvedVarKey))
                    return PublicDI.dO2Vars[sResolvedVarKey];

                PublicDI.log.error("In vars.resolveVar, variable does not exist in cache {0}", sResolvedVarKey);
            }
            return sVarKey;
        }

        public static Object get_(Object sVarKey)
        {
            return get(sVarKey.ToString());
        }

        public static Object get(String sVarKey)
        {
            if (PublicDI.dO2Vars.ContainsKey(sVarKey))
                return PublicDI.dO2Vars[sVarKey];

            PublicDI.log.info("In vars.get, variable does not exist in cache {0}", sVarKey);
            return null;
        }

        public static String info()
        {
            return "This class contains methods that allow access to O2's dynamic script vars";
        }

        public static String set(String sKey, String sValue) // Set a String or a var
        {
            if (PublicDI.dO2Vars.ContainsKey(sKey))
                PublicDI.dO2Vars[sKey] = resolveVar(sValue);
            else // add if doesn't exit            
                PublicDI.dO2Vars.Add(sKey, resolveVar(sValue));
            fireOnChangeEvent();
            return list();
        }

        public static String set_(String sKey, Object oValue) // set an Object
        {
            if (PublicDI.dO2Vars.ContainsKey(sKey))
                PublicDI.dO2Vars[sKey] = oValue;
            else // add if doesn't exit
                PublicDI.dO2Vars.Add(sKey, oValue);
            fireOnChangeEvent();
            return list();
        }

        public static void addtestData()
        {
            PublicDI.dO2Vars.Add("Hello", "World");
//            GlobalStaticVars.dO2Vars.Add("Macros dir", "config.getDefaultDir_O2Macros()");
//            GlobalStaticVars.dO2Vars.Add("Files in macro dir", Files.getFilesFromDir(Config.getDefaultDir_O2Macros()));
        }

        public static String list()
        {
            try
            {
                var sbData = new StringBuilder();
                sbData.AppendLine("Current variable list" + Environment.NewLine);
                foreach (string sVar in PublicDI.dO2Vars.Keys)
                {
                    String sVarData;
                    if (PublicDI.dO2Vars[sVar] == null)
                        sVarData = "[NULL VALUE]";
                    else
                    {
                        try
                        {
                            sVarData = PublicDI.dO2Vars[sVar].ToString();
                        }
                        catch
                        {
                            sVarData = "[ERROR when fetching ToString() value]";
                        }
                    }
                    if (sVarData.Length > 50)
                    {
                        sVarData = sVarData.Substring(0, 50);
                    }
                    sbData.AppendFormat("   - {0} = {1} {2} ", sVar, sVarData, Environment.NewLine);
                }
                return sbData.ToString();
            }
            catch (Exception ex)
            {
                PublicDI.log.error(" list : {0}", ex.Message);
                return null;
            }
        }

        public static void fireOnChangeEvent()
        {
            if (eVars_OnChange != null)
            {
                Delegate[] dInvocationList = eVars_OnChange.GetInvocationList();
                foreach (Delegate dDelegate in dInvocationList)
                    dDelegate.DynamicInvoke(new Object[] {});
            }
        }
    }
}
