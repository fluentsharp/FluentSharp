using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Invoke
    {
        public static List<Action>          invoke(this List<Action> actions)
        {
            if (actions.notNull())
                foreach (var action in actions)
                    action.invoke();
            return actions;
        }
        public static List<Action<T>>       invoke<T>(this List<Action<T>> actions, T param1)
        {
            if (actions.notNull())
                foreach (var action in actions)
                    action.invoke(param1);
            return actions;
        }
        public static List<Action<T1,T2>>   invoke<T1, T2>(this List<Action<T1, T2>> actions, T1 param1, T2 param2)
        {
            if (actions.notNull())
                foreach (var action in actions)
                    action.invoke(param1, param2);
            return actions;
        }
        public static Action                invoke(this Action action)
        {
            if (action.notNull())
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    ex.log("[in Action.invoke]");
                }
            }
            return action;
        }
        public static Action<T>             invoke<T>(this Action<T> action, T param)
        {
            if (action.notNull())
                try
                {
                    action(param);
                }
                catch (Exception ex)
                {
                    ex.log("[in Action.invoke<T>]");
                }
            return action;
        }
        public static Action<T1,T2>         invoke<T1,T2>(this Action<T1,T2> action, T1 param1, T2 param2)
        {
            if (action.notNull())
                try
                {
                    action(param1, param2);
                }
                catch (Exception ex)
                {
                    ex.log("[in Action.invoke<T>]");
                }
            return action;
        }
        public static T                     invoke<T>(this Func<T> func)
        {
            if (func.notNull())            
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    ex.log("[in Func.invoke<T>]");
                }
            
            return default(T);
        }
        public static T2                    invoke<T1,T2>(this Func<T1,T2> func, T1 param)
        {
            if (func.notNull())
                try
                {
                    return func(param);
                }
                catch (Exception ex)
                {
                    ex.log("[in Func.invoke<T1,T2>]");
                }
            return default(T2);
        }
        public static object                invoke(this object liveObject, MethodInfo methodInfo, params object[] parameters)
        {
            return methodInfo.invoke_on_LiveObject(liveObject, parameters);
        }
        public static object                invoke(this object liveObject, string methodName, params object[] parameters)
        {
            return PublicDI.reflection.invoke(liveObject, methodName, parameters);
        }
        public static object                invokeStatic(this Type type, string methodName, params object[] parameters)
        {
            return PublicDI.reflection.invokeMethod_Static(type, methodName, parameters);
        }  
        public static object                invoke(this MethodInfo methodInfo, params object[] parameters)
        {
            return PublicDI.reflection.invoke(methodInfo, parameters);
        }        
        public static object                invoke_on_LiveObject(this MethodInfo methodInfo, object liveObject, object[] parameters)
        {         
            return PublicDI.reflection.invoke(liveObject, methodInfo, parameters);            
        }    
        public static Thread				invokeStatic_StaThread(this MethodInfo methodInfo, params object[] invocationParameters)
        {
            return O2Thread.staThread(()=>methodInfo.invokeStatic(invocationParameters));
        }		        
        public static object				invokeStatic(this MethodInfo methodInfo, params object[] invocationParameters)
        {            
            if (invocationParameters.notNull())
                return PublicDI.reflection.invokeMethod_Static(methodInfo, invocationParameters);
            //return PublicDI.reflection.invokeMethod_Static(methodInfo, new object[] { invocationParameters});			
            return PublicDI.reflection.invokeMethod_Static(methodInfo);
        }
        public static object				invokeStatic(this Assembly assembly, string type, string method, params object[] parameters)
        {
            return assembly.type(type).invokeStatic(method, parameters);
        }

        /// <summary>
        /// Invokes the static constructor of a particular class
        /// </summary>
        /// <param name="type">type to invoke the static ctor</param>
        /// <returns>This represents the success of failure of the ctor invokation (note that by design a static ctor does not create an object we can use)</returns>
        public static bool                  invoke_Ctor_Static(this Type type)
        {
            return PublicDI.reflection.invoke_Ctor_Static(type);
        }
    }
}