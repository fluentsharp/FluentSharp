// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSharp.CoreLib.Interfaces;


//using Mono.Cecil;
//using O2.o2AppDomainProxy;

namespace FluentSharp.CoreLib.API
{
    public class FilteredSignature
    {
        public bool bIsClass = true;
        //public bool bMakeDotNetSignatureCompatibleWithOunceRules;
        public Char cParametersSplitChar = ';';
        public List<String> lsFunctionClass_Parsed = new List<string>();
        public List<String> lsParameters_Parsed = new List<string>();
        public String sFunctionClass = "";
        public String sFunctionFullName = "";
        public String sFunctionName = "";        
        public String sOriginalSignature = "";
        public String sParameters = "";
        public String sReturnClass = "";
        public String sSignature = "";
        public String sModule = "";
        public List<String> classesToNotApplyDotNetPatch = new List<string>();
        
		public  FilteredSignature()
		{
			classesToNotApplyDotNetPatch.AddRange(new [] {"System.String","System.Char","System.Object", "System.Int16", "System.Int32", "System.Int64", "System.Boolean", "System.Double", "System.Void"});        
		}
		
        public FilteredSignature(String sSignature) : this()
        {
            this.sSignature = sSignature;
            sParseSignature();
        }        

        public FilteredSignature(String sSignature, Char cParametersSplitChar)  : this()
        {
            this.sSignature = sSignature;
            this.cParametersSplitChar = cParametersSplitChar;
            sParseSignature();
        }

        public FilteredSignature(ICirFunction cirFunction)  : this()
        {
            populateSignatureObjectsFromCirFunction(cirFunction);
        }

        public FilteredSignature(MethodInfo methodInfo)  : this()
        {
            populateSignatureObjectsFromMethodInfo(methodInfo);
        }
        

        public override string ToString()
        {
            return sSignature;
        }        

        private void populateSignatureObjectsFromCirFunction(ICirFunction cirFunction)
        {
            if (cirFunction.CecilSignature != null)
            {
                sSignature = cirFunction.FunctionSignature;
                sParseSignature();                
                //sSignature = cirFunction.FunctionSignature;
                //sFunctionClass = (cirFunction.ParentClass!=null) ? cirFunction.ParentClass.FullName: "";                 
                // hack to make the cirTrace creation work
                cirFunction.ClassNameFunctionNameAndParameters = sSignature.Replace(":" + sReturnClass,"");
            }
            else
            {
                sOriginalSignature = cirFunction.FunctionSignature;
                sFunctionName = cirFunction.FunctionName;

                sParameters = cirFunction.FunctionNameAndParameters.Replace(sFunctionName + "(", "");
                if (sParameters.Length > 0)
                {
                    sParameters = sParameters.Substring(0, sParameters.Length - 1);
                    sParameters = makeDotNetSignatureCompatibleWithOunceRules(sParameters).Replace(',', ';');
                }
                //if (cirFunction.IsConstructor)
                //{
                if (cirFunction.ParentClassName != null)
                    sFunctionName = sFunctionName.Replace(".ctor", cirFunction.ParentClassName);
                //}            
                if (cirFunction.ReturnType != null)
                    sReturnClass = cirFunction.ReturnType;
                if (sReturnClass != null)
                    sReturnClass = makeDotNetSignatureCompatibleWithOunceRules(sReturnClass);

                sFunctionClass = cirFunction.ParentClassFullName;
                if (sFunctionClass != null && false == classesToNotApplyDotNetPatch.Contains(sFunctionClass))
                    sFunctionClass = makeDotNetSignatureCompatibleWithOunceRules(sFunctionClass);

                sSignature = getSignature();
                sSignature = sSignature.Replace('/', '+');
                // final hack to deal with generic types (need a better solution
                sSignature = sSignature.Replace("!0", "string").Replace("!1", "string");
            }
        }        

        // DC note: one thing that needs to be better defined is when the conversion into a standard naming convension is done
        private void populateSignatureObjectsFromMethodInfo(MethodInfo methodInfo)
        {
            try
            {
                sOriginalSignature = methodInfo.str();
                //   PublicDI.log.info(" --   :{0}", methodInfo.Name);
                sFunctionName = methodInfo.Name;
                foreach (ParameterInfo parameter in methodInfo.GetParameters())
                {
                    var parameterValue = (parameter.ParameterType.IsGenericType)
                                       ? getGenericSignature(parameter.ParameterType)
                                       : parameter.ParameterType.FullName;
                    sParameters += parameterValue; //makeDotNetSignatureCompatibleWithOunceRules(parameterValue);
                    //sParameters += ", ";
                    sParameters += ";";
                }
                if (sParameters != "")
                    //sParameters = sParameters.Substring(0, sParameters.Length - 2);
                    sParameters = sParameters.Substring(0, sParameters.Length - 1);

                //sReturnClass = makeDotNetSignatureCompatibleWithOunceRules(getGenericSignature(methodInfo.ReturnType));
                sReturnClass = getGenericSignature(methodInfo.ReturnType);

                //sFunctionClass = makeDotNetSignatureCompatibleWithOunceRules(methodInfo.ReflectedType.ToString());
                sFunctionClass = methodInfo.ReflectedType.ToString();

                sSignature = string.Format("{0}.{1}({2}):{3}",
                        sFunctionClass, sFunctionName, sParameters, sReturnClass);
            }
            catch (Exception ex)
            {
                ex.log("in populateSignatureObjectsFromMethodInfo, could not filter signature for method: {0}".format(methodInfo.Name));
            }
        }

        public static string getGenericSignature(Type genericType)
        {
            if (genericType == null)
                return "";
            if (!genericType.IsGenericType || genericType.Name != "List`1")
                return genericType.FullName;
            string genericArguments = "";
            foreach (Type type in genericType.GetGenericArguments())
                genericArguments += type.FullName + ",";
            if (genericArguments != "")
                genericArguments = genericArguments.Substring(0, genericArguments.Length - 1);
            //return string.Format("List<{0}>", genericArguments);
            return string.Format("System.Collections.Generic.List`1<{0}>", genericArguments); // this will make it compatible with the signatures we get from Mono.Cecil
            
        }

        public string getReflectorView()
        {
            return string.Format("{0}({1}) : {2}", sFunctionName, sParameters, sReturnClass);
        }

        // HACK to deal with Cecil Definitions
        public string transformCecilSignature(string cecilSignature)
        {
            var returnTypeFuncionAndParameters = extractModule(cecilSignature);
            var indexOfFirstSpace = returnTypeFuncionAndParameters.IndexOf(' ');
            if (indexOfFirstSpace > -1)
            {
                var returnType = returnTypeFuncionAndParameters.Substring(0, indexOfFirstSpace);
                var functionAndParameters = returnTypeFuncionAndParameters.Substring(indexOfFirstSpace);
                                
                functionAndParameters = functionAndParameters.Replace("::", ".").Replace('/', '+').Replace(',', ';');
                return string.Format("{0}:{1}", functionAndParameters, returnType);
            }
            return returnTypeFuncionAndParameters;
        }        
        public void sParseSignature()
        {
            sOriginalSignature = sSignature;            
            if ((sSignature.index("::") > -1))// && (sSignature.IndexOf("!") > -1))
                sSignature = transformCecilSignature(sSignature).Trim();
            else
                if (sSignature.index("!") > -1)
                    sSignature = extractModule(sSignature).Trim();

            //if (bMakeDotNetSignatureCompatibleWithOunceRules)
            //    makeDotNetSignatureCompatibleWithOunceRules();


            int iIndexOfFirstLeftParenthesis = sSignature.IndexOf('(');
            //int iIndexOfFirstRightParenthesis = sSignature.IndexOf(')');
            int iIndexOfLastRightParenthesis = sSignature.LastIndexOf(')');
            int iIndexOfFirstColon = sSignature.IndexOf(':');

            //if (iIndexOfFirstLeftParenthesis > -1 && iIndexOfFirstRightParenthesis > -1)
            if (iIndexOfFirstLeftParenthesis > -1 && iIndexOfLastRightParenthesis > -1)
            {
                // sParameters = sSignature.Substring(iIndexOfFirstLeftParenthesis + 1, iIndexOfFirstRightParenthesis - iIndexOfFirstLeftParenthesis - 1);
                sParameters = sSignature.Substring(iIndexOfFirstLeftParenthesis + 1,
                                                   iIndexOfLastRightParenthesis - iIndexOfFirstLeftParenthesis - 1);
                lsParameters_Parsed.AddRange(sParameters.Split(cParametersSplitChar));
                bIsClass = false;
            }
            else
            {
                lsFunctionClass_Parsed.AddRange(sSignature.Split('.'));
                sFunctionClass = sSignature;
                return;
            }

            if (iIndexOfFirstColon > -1)
            {
                sReturnClass = sSignature.Substring(iIndexOfFirstColon + 1);
                sFunctionFullName = sSignature.Replace(String.Format("({0}):{1}", sParameters, sReturnClass), "");
            }
            else
                sFunctionFullName = sSignature.Replace(String.Format("({0})", sParameters), "");
            int iIndexOfLastDot = sFunctionFullName.LastIndexOf('.');
            if (iIndexOfLastDot > -1)
            {
                sFunctionName = sFunctionFullName.Substring(iIndexOfLastDot + 1);
                sFunctionClass = sFunctionFullName.Substring(0, iIndexOfLastDot);
                lsFunctionClass_Parsed.AddRange(sFunctionClass.Split('.'));
            }
            else
            {
                sFunctionName = sFunctionFullName;
                sFunctionClass = "";
            }

           // sFunctionNameAndParams = 
            //sFunctionNameAndParamsAndReturnClass = 
        }
        //public String sFunctionNameAndParamsAndReturnClass = "";

        public String sFunctionNameAndParams
        {
            get
            {
                return String.Format("{0}({1})", sFunctionName, sParameters);
            }
        }

        public String sFunctionNameAndParamsAndReturnClass
        {
            get
            {
                return String.Format("{0}({1}):{2}", sFunctionName, sParameters, sReturnClass);
            }
        }

        public static string makeDotNetSignatureCompatibleWithOunceRules(string stringToFilter)
        {
            // fix names
            if (stringToFilter != null)
            return stringToFilter
                .Replace("System.String", "string")
                .Replace("System.Char", "char")                
                .Replace("System.Object", "object")
                .Replace("System.Int16", "short")
                .Replace("System.Int32", "int")
                .Replace("System.Int64", "long")
               // .Replace("Boolean", "bool")
                .Replace("System.Boolean", "bool")
                .Replace("System.Double", "double")
                .Replace("System.Void", "void")
                .Replace(" ByRef", "")
                .Replace("`1", "")
                .Replace("`2", "")
                .Replace("&", "");
            return null;
            // this doesn't seem to be included in the CIR signature                 
        }

        private string extractModule(string signature)
        {
     //       bMakeDotNetSignatureCompatibleWithOunceRules = true;    // enable this since this is the only time we need to make this conversion
            //signature = signature.Replace("!!", "!"); // fix odd case where there are two ! after the module name
            var indexOfFirstExclamationMark = signature.IndexOf('!');
            if (indexOfFirstExclamationMark > -1)
            {
            //var splittedSignature = signature.Split('!');
            //if (splittedSignature.Length == 2)
            //{
              //  sModule = splittedSignature[0];
              //  return splittedSignature[1];
                sModule = signature.Substring(0, indexOfFirstExclamationMark);
                return signature.Substring(indexOfFirstExclamationMark  + 1);
            }
            return signature;
        }

        public String getFilteredSignature(bool bShowParameters, bool bShowReturnClass, bool bShowNamespace,
                                           int iNamespaceDepth)
        {
            String sFilteredSignature = sFunctionName;
            if (bShowParameters)
                sFilteredSignature += "(" + sParameters + ")";
            if (bShowReturnClass)
                sFilteredSignature += ":" + sReturnClass;
            if (bShowNamespace)
                sFilteredSignature = getClassName(iNamespaceDepth) + "." + sFilteredSignature;
            return sFilteredSignature;
        }

        public String getClassName(Int32 depth) // filters class names from the left to right
        {
            if (depth > -1 && lsFunctionClass_Parsed.Count - depth > -1)
            {
                // String asd = String.Join(".", lsFunctionClass_Parsed.ToArray(), lsFunctionClass_Parsed.Count - Depth, Depth);

                return String.Join(".", lsFunctionClass_Parsed.ToArray(), lsFunctionClass_Parsed.Count - depth, depth);
            }

            return sFunctionClass;
        }

        public String getClassName_Rev(Int32 depth) // filters class names from the right to left
        {
            if (depth > -1 && lsFunctionClass_Parsed.Count > depth)
            {
                //     String asd = String.Join(".", lsFunctionClass_Parsed.ToArray(), Depth, lsFunctionClass_Parsed.Count - Depth);

                return String.Join(".", lsFunctionClass_Parsed.ToArray(), 0, depth);
            }

            return sFunctionClass;
        }

        public static String filterSignature(String sStringToFilter, bool bShowParameters, bool bShowReturnClass,
                                             bool bShowNamespace, int iNamespaceDepth)
        {
            if (PublicDI.dFilteredFuntionSignatures.ContainsKey(sStringToFilter))
                return PublicDI.dFilteredFuntionSignatures[sStringToFilter].getFilteredSignature(
                    bShowParameters, bShowReturnClass, bShowNamespace, iNamespaceDepth);

            var fsFilteredSignature = new FilteredSignature(sStringToFilter);
            PublicDI.dFilteredFuntionSignatures.Add(sStringToFilter, fsFilteredSignature);
            return fsFilteredSignature.getFilteredSignature(bShowParameters, bShowReturnClass, bShowNamespace,
                                                            iNamespaceDepth);
        }

        public static String filterName(String sStringToFilter, bool bShowParameters, bool bShowReturnClass)
        {
            return filterSignature(sStringToFilter, bShowParameters, bShowReturnClass, false /*bShowNamespace*/, 0
                /*iNamespaceDepth*/);
        }


        public static String filterName(String sStringToFilter, bool bShowParameters, bool bShowReturnClass,
                                        bool bShowNamespace, int iNamespaceDepth, bool bFilterName,
                                        bool bOnlyShowClasses, int iShowClassesLevel)
        {
            string sFilteredText = filterName(sStringToFilter, bShowParameters, bShowReturnClass, bShowNamespace,
                                              iNamespaceDepth, bFilterName);
            if (bOnlyShowClasses)
            {
                for (int iLoop = 0; iLoop < iShowClassesLevel; iLoop++)
                {
                    int iLastIndexOfDot = sFilteredText.LastIndexOf('.');
                    if (iLastIndexOfDot > -1)
                        sFilteredText = sFilteredText.Substring(0, iLastIndexOfDot);
                }
            }
            return sFilteredText;
        }

        public static String filterName(String sStringToFilter, bool bShowParameters, bool bShowReturnClass,
                                        bool bShowNamespace, int iNamespaceDepth)
        {
            return filterName(sStringToFilter, bShowParameters, bShowReturnClass, bShowNamespace, iNamespaceDepth,
                              true /*bFilterName*/);
        }

        public static String filterName(String sStringToFilter, bool bShowParameters, bool bShowReturnClass,
                                        bool bShowNamespace, int iNamespaceDepth, bool bFilterName)
        {
            if (bFilterName)
                return filterSignature(sStringToFilter, bShowParameters, bShowReturnClass, bShowNamespace,
                                       iNamespaceDepth);

            return sStringToFilter;
        }
        

        public static bool isSignatureCached(string signatureToSearch)
        {
            return PublicDI.dFilteredFuntionSignatures.ContainsKey(signatureToSearch);
        }

        public static FilteredSignature getFromCache(string signatureToGet)
        {
            if (isSignatureCached(signatureToGet))
                return PublicDI.dFilteredFuntionSignatures[signatureToGet];
            return null;
        }

        public static FilteredSignature createFilteredSignatureFromJavaMethod(string className, string functionName, string methodDescriptor)
        {
            var newFilteredSignature = new FilteredSignature();
            newFilteredSignature.sFunctionClass = className;
            newFilteredSignature.sFunctionName = functionName;
            // process descriptor
            var lastParentesis = methodDescriptor.LastIndexOf(')');
            if (lastParentesis > -1)
            {
                methodDescriptor = methodDescriptor.Replace('/', '.');
                lastParentesis++;
                newFilteredSignature.sParameters = methodDescriptor.Substring(0, lastParentesis);
                newFilteredSignature.sReturnClass = methodDescriptor.Substring(lastParentesis);
                // BUG: Major hack to fix the sParameters created by JavaMethod descriptors                
                if (newFilteredSignature.sReturnClass == "V")
                {
                    newFilteredSignature.sReturnClass = "void";
                }
                newFilteredSignature.sParameters = newFilteredSignature.sParameters.Replace("(I)", "(int)").Replace("(IL", "(int;").Replace(";IL", ";int;");
                newFilteredSignature.sParameters = newFilteredSignature.sParameters.Replace("(L", "(").Replace("(IL", "(").Replace(";L", ";").Replace(";)", ")");
                if (newFilteredSignature.sParameters[0] == '(' && newFilteredSignature.sParameters[newFilteredSignature.sParameters.Length -1] == ')')
                    newFilteredSignature.sParameters = newFilteredSignature.sParameters.Substring(1, newFilteredSignature.sParameters.Length - 2);
                // fix the sReturnClass
                if (newFilteredSignature.sReturnClass[0] == 'L' && newFilteredSignature.sReturnClass[newFilteredSignature.sReturnClass.Length - 1] == ';')
                    newFilteredSignature.sReturnClass = newFilteredSignature.sReturnClass.Substring(1,newFilteredSignature.sReturnClass.Length - 2);
                newFilteredSignature.sSignature = newFilteredSignature.getSignature();
            }
            return newFilteredSignature;
        }

        public string getSignature()
        {
            return string.Format("{0}.{1}({2}):{3}", sFunctionClass, sFunctionName, sParameters, sReturnClass);
        }
    }
}
