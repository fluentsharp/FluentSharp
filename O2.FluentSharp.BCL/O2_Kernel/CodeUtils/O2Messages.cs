// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using O2.Interfaces.CIR;
using O2.Interfaces.Messages;
using O2.Interfaces.Views;
using O2.Kernel.InterfacesBaseImpl;

namespace O2.Kernel.CodeUtils
{
    
    [Serializable]
    public class O2Messages
    {
        //public delegate void callbackFor_O2Message(IO2Message o2Message);    
    
        public static Thread dotNetAssemblyAvailable(string pathToAssembly)
        {
            return DI.o2MessageQueue.sendMessage(new KM_DotNetAssemblyAvailable(pathToAssembly));
        }

        public static Thread fileErrorHighlight(string pathToFile, int line, int column)
        {
            return DI.o2MessageQueue.sendMessage(new KM_FileOrFolderSelected(pathToFile, line,column, true ));
        }

        public static Thread fileOrFolderSelected(string pathToFileOrFolder)
        {
            return DI.o2MessageQueue.sendMessage(new KM_FileOrFolderSelected(pathToFileOrFolder));
        }

        public static Thread fileOrFolderSelected(string pathToFileOrFolder, string _lineNumber)
        {
            var lineNumber = 0;
            if (Int32.TryParse(_lineNumber, out lineNumber))

                return DI.o2MessageQueue.sendMessage(new KM_FileOrFolderSelected(pathToFileOrFolder, lineNumber));
            DI.log.error("in fileOrFolderSelected could not convert linenumber into an int:{0}", _lineNumber);
            return null;
        }

        public static Thread fileOrFolderSelected(string pathToFileOrFolder, int lineNumber)
        {
            DI.log.info("in O2Kernel: FileOfFolderSelected event: {0}:{1}", pathToFileOrFolder, lineNumber);
            return DI.o2MessageQueue.sendMessage(new KM_FileOrFolderSelected(pathToFileOrFolder, lineNumber));
        }

        public static void openControlInGUI(Type controlType, O2DockState o2DockState, string controlName)
        {
            DI.o2MessageQueue.sendMessage(KM_GUIAction.openControlInGui(controlType, o2DockState, controlName));
        }

        public static Control openControlInGUISync(Type controlType, O2DockState o2DockState, string controlName)
        {            
            DI.o2MessageQueue.sendMessageSync(KM_GUIAction.openControlInGui(controlType, o2DockState, controlName));
            return getAscxSync(controlName);
        }

        public static void openControlInGUI(string controlTypeAndAssembly, O2DockState o2DockState, string controlName)
        {
            // lets try to find this control in the current AppDomain
            var o2AppDomainFactory = AppDomainUtils.getO2AppDomainFactoryForCurrentO2Kernel();
            Type proxyObjectType = o2AppDomainFactory.getProxyType(controlTypeAndAssembly);
            if (proxyObjectType == null)
                DI.log.error("in openControlInGUI, could not find control: {0}", controlTypeAndAssembly);
            else
                openControlInGUI(proxyObjectType, o2DockState, controlName);
        }


        public static Thread getAscx(string ascxToGet, Action<object> actionReturnData)
        {
            return DI.o2MessageQueue.sendMessage(KM_GUIAction.getGuiAscx(ascxToGet, actionReturnData));
        }

        public static Control getAscxSync(string ascxToGet)
        {
            var guiControlReady = new AutoResetEvent(false);
            object controlOpen = null;

            getAscx(ascxToGet, ascxControl =>
            {
                if (ascxControl != null)
                    controlOpen = ascxControl;
                else
                    DI.log.error("in openControlInGUISync could not get opened control");
                guiControlReady.Set();
            }
                );
            guiControlReady.WaitOne();
            return (Control)controlOpen;
        }

        public static object setAscxDockStateAndOpenIfNotAvailable(string typeOfControl, O2DockState o2DockState, string controlName)
        {
            return DI.o2MessageQueue.sendMessage(KM_GUIAction.setAscxDockStateAndOpenIfNotAvailable(typeOfControl, o2DockState, controlName));
        }

        public static Thread executeOnAscx(string ascxToExecuteMethod, string targetMethod, string[] methodParameters)
        {
            return DI.o2MessageQueue.sendMessage(KM_GUIAction.executeOnAscx(ascxToExecuteMethod, targetMethod, methodParameters));
        }

        public static object executeOnAscxSync(string ascxToExecuteMethod, string targetMethod, string[] methodParameters)
        {
            var o2Message = KM_GUIAction.executeOnAscx(ascxToExecuteMethod, targetMethod, methodParameters);
            DI.o2MessageQueue.sendMessageSync(o2Message);
            return o2Message.returnData;
        }


        public static void closeAscxParent(string ascxControlName)
        {
            DI.o2MessageQueue.sendMessage(KM_GUIAction.closeAscxParent(ascxControlName));
        }

        public static Thread raiseO2MDbgCommandExecutionMessage(IM_O2MdbgActions _o2MdbgAction, string _lastCommandExecutionMessage)
        {
            return DI.o2MessageQueue.sendMessage(new KM_O2MdbgAction { o2MdbgAction = _o2MdbgAction, lastCommandExecutionMessage = _lastCommandExecutionMessage });
        }

        
        public static Thread raiseO2MDbgAction(IM_O2MdbgActions _o2MdbgAction)
        {
            return DI.o2MessageQueue.sendMessage(new KM_O2MdbgAction { o2MdbgAction = _o2MdbgAction});
        }

        public static Thread raiseO2MDbgBreakEvent(string filename, int line)
        {
            return DI.o2MessageQueue.sendMessage(new KM_O2MdbgAction { o2MdbgAction = IM_O2MdbgActions.breakEvent, filename = filename, line = line });
        }

        public static Thread raiseO2MDbgDebugProcessRequest(string assemblyToDebug)
        {
            return DI.o2MessageQueue.sendMessage(new KM_O2MdbgAction { o2MdbgAction = IM_O2MdbgActions.debugProcessRequest, filename = assemblyToDebug});
        }

        public static Thread raiseO2MDbgDebugMethodInfoRequest(MethodInfo methodToDebug, string loadDllsFrom)
        {
            return DI.o2MessageQueue.sendMessage(new KM_O2MdbgAction { o2MdbgAction = IM_O2MdbgActions.debugMethodInfoRequest, method = methodToDebug, loadDllsFrom = loadDllsFrom});
        }

        public static Thread raiseO2MDbg_SetBreakPointOnFile(string fileName, int lineNumber)
        {
            return DI.o2MessageQueue.sendMessage(new KM_O2MdbgAction { o2MdbgAction = IM_O2MdbgActions.setBreakpointOnFile, filename = fileName, line = lineNumber });
        }
        
        //O2Messages.raiseO2MDbgAction(IM_O2MdbgActions.startDebugSession);


        public static Thread selectedTypeOrMethod(string _assemblyName, string _typeName, string _methodName, object[] _methodParameters)
        {
            return DI.o2MessageQueue.sendMessage(new KM_SelectedTypeOrMethod(_assemblyName, _typeName, _methodName, _methodParameters));
        }

        public static Thread selectedTypeOrMethod(MethodInfo methodInfo)
        {
            return DI.o2MessageQueue.sendMessage(new KM_SelectedTypeOrMethod(methodInfo));
        }

    

        public static bool openAscxGui()
        {
            var o2AppDomainFactory = AppDomainUtils.getO2AppDomainFactoryForCurrentO2Kernel();
            var result = o2AppDomainFactory.invoke("O2AscxGUI O2_External_WinFormsUI", "launch", null);
            if (result is bool)
                return (bool) result;
            return false;
            //o2WcfProxy = O2WcfUtils.createClientProxy(newO2KernelProcessName);
            //o2WcfProxy.invokeOnAppDomainObject(newAppDomain, "O2AscxGUI O2_External_WinFormsUI","launch",null)
        }

        public static void openAscxAsForm(object ascxControlToLoad, string controlName)
        {
            var o2AppDomainFactory = AppDomainUtils.getO2AppDomainFactoryForCurrentO2Kernel();
            o2AppDomainFactory.invoke("O2AscxGUI O2_External_WinFormsUI", "openAscxAsForm", new [] { ascxControlToLoad, controlName });            
        }

        public static bool closeAscxGui()
        {
            var o2AppDomainFactory = AppDomainUtils.getO2AppDomainFactoryForCurrentO2Kernel();
            var result = o2AppDomainFactory.invoke("O2AscxGUI O2_External_WinFormsUI", "close", null);
            if (result is bool)
                return (bool)result;
            return false;
        }

        public static void waitForAscxGuiEnd()
        {
            var o2AppDomainFactory = AppDomainUtils.getO2AppDomainFactoryForCurrentO2Kernel();
            o2AppDomainFactory.invoke("O2AscxGUI O2_External_WinFormsUI", "waitForAscxGuiClose", null);                        
        }

        public static Thread setCirData(ICirData cirData)
        {
            return DI.o2MessageQueue.sendMessage(KM_CirAction.setCirData(cirData));
        }

        public static Thread setCirDataAnalysis(ICirDataAnalysis cirDataAnalysis)
        {
            return DI.o2MessageQueue.sendMessage(KM_CirAction.setCirDataAnalysis(cirDataAnalysis));
        }

        public static bool isGuiLoaded()
        {
            try
            {
                if (DI.reflection.getType("O2AscxGUI") == null) // first see if the assembly O2_External_WinFormsUI is loaded 
                    return false;
                var o2AppDomainFactory = AppDomainUtils.getO2AppDomainFactoryForCurrentO2Kernel();
                return (bool)o2AppDomainFactory.invoke("O2AscxGUI O2_External_WinFormsUI", "isGuiLoaded");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool isDebuggerAvailable()
        {            
            // need to find a different way to do this
            
            try
            {                                
                if (DI.reflection.getType("O2MDbgUtils") == null) // first see if the assembly O2_External_WinFormsUI is loaded 
                    return false;
                var o2AppDomainFactory = AppDomainUtils.getO2AppDomainFactoryForCurrentO2Kernel();
                return (bool)o2AppDomainFactory.invoke("O2MDbgUtils O2_Debugger_Mdbg", "IsDebuggerAvailable");
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
