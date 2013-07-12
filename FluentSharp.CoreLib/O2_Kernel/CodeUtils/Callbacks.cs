// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using FluentSharp.CoreLib.Interfaces;

//O2File:../PublicDI.cs

namespace FluentSharp.CoreLib.API
{
    public class Callbacks
    {
        #region Delegates

        public delegate void dMethod();

        public delegate void dMethod_Bool(bool bValue);

        public delegate void dMethod_Int(int iInt);

        public delegate void dMethod_ListString(List<String> lsString);

        public delegate void dMethod_MethodInfo(MethodInfo methodInfo);

        public delegate void dMethod_Object(Object oObject);

        public delegate void dMethod_String(String sString);

        public delegate void dMethod_String_Object(String sString, Object oObject);

        public delegate void dMethod_String_String(String sString1, String sString2);
        
        #endregion

        public static IO2Log log = new KO2Log("Callbacks");

        // public events

        // public static event dMethod_String eScriptCompiledSuccessfully;


        /*   public static void raiseEvent_ScriptCompiledSuccessfully(String sScriptClassName)
        {
            raiseEvent_String(eScriptCompiledSuccessfully, sScriptClassName);
        }*/

        // need to find a generic way to invoke these methods since I can't seem to be able to invoke this from outside this class
        /*    public static void raiseEvent_String(dMethod_String eEventToInvoke, String sParam)
        {
            if (eEventToInvoke != null)
            {
                Delegate[] dInvocationList = eEventToInvoke.GetInvocationList();
                foreach (Delegate dDelegate in dInvocationList)
                    dDelegate.DynamicInvoke(new Object[] {sParam});
            }
            //   eEventToInvoke.BeginInvoke(sParam, null, null);
        }*/

        public static Thread raiseRegistedCallbacks(Delegate delegatesToInvoke)
        {
            return raiseRegistedCallbacks(delegatesToInvoke, new object[0]);
        }

        public static Thread raiseRegistedCallbacks(Delegate delegatesToInvoke, object[] parameters)
        {
            return O2Thread.mtaThread(() =>
                        {
                            if (delegatesToInvoke != null)
                                foreach (
                                    Delegate registerCallback in
                                        delegatesToInvoke.GetInvocationList())
                                    try
                                    {
                                        registerCallback.DynamicInvoke(parameters);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.error(
                                            "in raiseRegistedCallbacks, while invoking {0} : ",
                                            registerCallback.Method.Name,
                                            ex.Message);
                                    }
                        });
        }
    }
}
