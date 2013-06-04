// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using O2.Kernel.CodeUtils;
using O2.Kernel.InterfacesBaseImpl;
using FluentSharp.ExtensionMethods;
using O2.DotNetWrappers.Filters;
using System.Text.RegularExpressions;
using O2.Kernel.O2CmdShell;

namespace O2.Kernel
{   
    public static class PublicDI
    {
        public static bool Offline { get; set; }

        static PublicDI()
        {            
            //loadValuesFromConfigFile();
            O2Kernel_Web.ApplyNetworkConnectionHack();            
            log = new KO2Log();			
            reflection = new KReflection();            
            config = O2ConfigLoader.getKO2Config();
            

            O2KernelProcessName = "Generic O2 Kernel Process";
            AppDomainUtils.registerCurrentAppDomain();            

            sDefaultFileName_ReportBug_LogView = "ReportBug_LogView.Rtf";
            sDefaultFileName_ReportBug_ScreenShotImage = "ReportBug_ScreenShotImage.bmp";
            sEmailDefaultTextFromO2Gui = "enter message here";            
            sEmailHost = "ASPMX.L.GOOGLE.COM";
            sEmailToSendBugReportsTo = "dinis.cruz@owasp.org";
            sO2Website = "https://ounceopen.squarespace.com";
            LogViewerControlName = "O2 Logs";

            //loadValuesFromConfigFile();
            dFilteredFuntionSignatures = new Dictionary<string, FilteredSignature>();
            dO2Vars = new Dictionary<string, object>();
            dRegExes = new Dictionary<string, Regex>();

            dFilesLines = new Dictionary<string, List<string>>();
            
        }

        /*public static void loadValuesFromConfigFile()
        {
            try
            {
                var offlineValue = ConfigurationSettings.AppSettings["Offline"];
                PublicDI.Offline = offlineValue.toBool();
            }
            catch
            {
                
            }
        }*/

        public static KO2Log log                                                        { get; set; }
        public static KO2Config config                                                  { get; set; }
        public static KReflection reflection                                            { get; set; }
        public static string O2KernelProcessName                                        { get; set; }
        public static Dictionary<String, FilteredSignature> dFilteredFuntionSignatures  { get; set; }
        public static Dictionary<String, Object> dO2Vars                                { get; set; }
        public static Dictionary<String, Regex> dRegExes                                { get; set; }
        public static Dictionary<String, List<String>> dFilesLines                      { get; set; }
        public static O2Shell o2Shell                                                   { get; set; }

        // GUI stuff
        public static Object CurrentGUIHost { get; set; } // need to assign the existing GUIs to here

        public static string sDefaultFileName_ReportBug_LogView { get; set; }
        public static string sDefaultFileName_ReportBug_ScreenShotImage { get; set; }
        public static string sEmailDefaultTextFromO2Gui { get; set; }
        public static string sEmailHost { get; set; }
        public static string sEmailToSendBugReportsTo { get; set; }
        public static string sO2Website { get; set; }        
        public static string LogViewerControlName   { get; set; }                        

        // Scripts
        private static string _currentScript = "";
        public static string CurrentScript
        {
            get
            {
                return _currentScript;
            }
            set
            {
                if (String.IsNullOrEmpty(value) == false)
                {
                    PublicDI.log.info("Setting CurrentScript to:: {0}", value);
                    _currentScript = value;
                    CurrentScriptFolder = value.parentFolder();
                }
            }
        }

        public static string CurrentScriptFolder { get; private set; }

        public static void debugBreak()
        {
            debug._break();
        }
    }
}
