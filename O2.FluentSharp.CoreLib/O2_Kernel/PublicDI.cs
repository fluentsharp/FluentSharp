// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using O2.Kernel.CodeUtils;
using O2.Kernel.InterfacesBaseImpl;
using O2.Kernel.Objects;

//O2File:DI.cs
//O2File:InterfacesBaseImpl/KO2Log.cs
//O2File:InterfacesBaseImpl/KO2Config.cs
//O2File:InterfacesBaseImpl/KReflection.cs
//O2File:InterfacesBaseImpl/KO2MessageQueue.cs

namespace O2.Kernel
{
    /// <summary>
    /// These are public DI objects which can be used and manipulated by O2 modules 
    /// For example the log one is a good candidate for the GUI controls to take over
    /// </summary>
    /// todo: merge this with the O2.Kernel.DI class so that only this PublicDI.exists
    public static class PublicDI
    {
        static PublicDI()
        {
            log = (KO2Log)DI.log;
            config = (KO2Config)DI.config;
            reflection = (KReflection) DI.reflection;
            
            appDomainsControledByO2Kernel = DI.appDomainsControledByO2Kernel;
            O2KernelProcessName = DI.O2KernelProcessName;
           /* // all these variables need to be setup
            appDomainsControledByO2Kernel = new Dictionary<string, O2AppDomainFactory>();            
            log = new KO2Log();
            reflection = new KReflection();
            o2MessageQueue = KO2MessageQueue.getO2KernelQueue();            
            O2KernelProcessName = DI.O2KernelProcessName;

            // before we load the O2Config data (which is loaded from the local disk)
            config = O2ConfigLoader.getKO2Config(); 
            */

            sDefaultFileName_ReportBug_LogView = "ReportBug_LogView.Rtf";
            sDefaultFileName_ReportBug_ScreenShotImage = "ReportBug_ScreenShotImage.bmp";
            sEmailDefaultTextFromO2Gui = "enter message here";            
            sEmailHost = "ASPMX.L.GOOGLE.COM";
            sEmailToSendBugReportsTo = "dinis.cruz@owasp.org";
            sO2Website = "https://ounceopen.squarespace.com";
            LogViewerControlName = "O2 Logs";

            
        }

        public static KO2Log log { get; set; }
        public static KO2Config config { get; set; }
        public static KReflection reflection { get; set; }        
        public static Dictionary<string, O2AppDomainFactory> appDomainsControledByO2Kernel;
        public static string O2KernelProcessName { get; set; }

        // GUI stuff
        public static Object CurrentGUIHost { get; set; } // need to assign the existing GUIs to here

        public static string sDefaultFileName_ReportBug_LogView { get; set; }
        public static string sDefaultFileName_ReportBug_ScreenShotImage { get; set; }
        public static string sEmailDefaultTextFromO2Gui { get; set; }
        public static string sEmailHost { get; set; }
        public static string sEmailToSendBugReportsTo { get; set; }
        public static string sO2Website { get; set; }

        public static string LogViewerControlName { get; set; }                        

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
                }
            }
        }

        public static void debugBreak()
        {
            debug.@break();
        }
    }
}
