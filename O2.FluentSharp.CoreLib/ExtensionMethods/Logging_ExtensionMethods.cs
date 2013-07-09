using System.Collections.Generic;
using System;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Logging_ExtensionMethods
    {
        public static string info(this string formatString, params object[] parameters)
        {
            var message = formatString.format(parameters);
            PublicDI.log.info(message);
            return message;
        }

        public static string debug(this string formatString, params object[] parameters)
        {
            var message = formatString.format(parameters);
            PublicDI.log.debug(message);
            return message;
        }

        public static string error(this string formatString, params object[] parameters)
        {
            var message = formatString.format(parameters);
            PublicDI.log.error(message);
            return message;
        }

        public static void info(this bool value)
        {
            value.str().info();
        }

        public static string debug(this bool value)
        {
            return value.str().debug();
        }

        public static void ifInfo(this bool enabled, string infoFormat, params object[] parameters)
        {
            if (enabled)
                PublicDI.log.info(infoFormat, parameters);
        }

        public static void ifDebug(this bool enabled, string debugFormat, params object[] parameters)
        {
            if (enabled)
                PublicDI.log.debug(debugFormat, parameters);
        }

        public static void ifError(this bool enabled, string errorFormat, params object[] parameters)
        {
            if (enabled)
                PublicDI.log.error(errorFormat, parameters);
        }

        /*public static void info(this object _object, string infoMessage)
        {
            PublicDI.log.info("[{0}] {1}", _object.type().Name, infoMessage);
        }

        public static void info(this object _object, string messageFormat, params object[] parameters)
        {
            PublicDI.log.info(messageFormat.format(parameters));
        }

        public static void debug(this object _object, string debugMessage)
        {
            PublicDI.log.debug("[{0}] {1}", _object.type().Name, debugMessage);
        }

        public static void debug(this object _object, string messageFormat, params object[] parameters)
        {
            PublicDI.log.debug(messageFormat.format(parameters));
        }

        public static void error(this object _object, string messageFormat, params object[] parameters)
        {
            PublicDI.log.info(messageFormat.format(parameters));
        }

        */


        public static void showInLog(this List<string> list)
        {
            PublicDI.log.debug("Showing {0} items from List<string>:");
            foreach (var item in list)
                PublicDI.log.info("   {0}", item);
        }

        public static void log(this string _string)
        {
            PublicDI.log.info(_string);
        }

        /*public static void info(this string _string)
        {
            PublicDI.log.info(_string);
        }

        public static void debug(this string _string)
        {
            PublicDI.log.debug(_string);
        }

        public static void error(this string _string)
        {
            PublicDI.log.error(_string);
        }*/

        public static Exception log(this Exception ex)
        {
            ex.log("");
            return ex;
        }

        public static void log(this Exception ex, string textFormat, params object[] parameters)
        {            
            PublicDI.log.ex(ex, textFormat.format(parameters), false);
        }        

        public static void logWithStackTrace(this Exception ex, string text)
        {
            PublicDI.log.ex(ex, text, true);
        }
        
        //ints

        public static string info(this int value)
        {
            return value.str().info();
        }

        public static string debug(this int value)
        {
            return value.str().debug();
        }

    }
}