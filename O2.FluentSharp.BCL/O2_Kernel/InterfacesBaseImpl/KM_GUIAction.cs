// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using O2.Interfaces.Messages;
using O2.Interfaces.Views;
using O2.Kernel.CodeUtils;

namespace O2.Kernel.InterfacesBaseImpl
{
    //[Serializable]
    class KM_GUIAction : KO2Message, IM_GUIAction
    {
        public IM_GUIActions GuiAction { get; set; }
        public Type controlType { get; set; }
        public string controlTypeString { get; set; }
        public O2DockState o2DockState { get; set; }
        public string controlName { get; set; }
        public string targetMethod { get; set; }
        public string[] methodParameters { get; set; }

        public Action<object> returnDataCallback { get; set; }


        public static KM_GUIAction openControlInGui(Type _controlType)
        {
            return openControlInGui(_controlType, O2DockState.Float);
        }
        public static KM_GUIAction openControlInGui(Type _controlType, O2DockState _o2DockState)
        {
            return openControlInGui(_controlType, _o2DockState, _controlType.Name);
        }

        public static KM_GUIAction openControlInGui(Type _controlType, O2DockState _o2DockState, string _controlName)
        {
            var kmGuiAction = new KM_GUIAction
                                  {
                                      GuiAction = IM_GUIActions.openControlInGui,
                                      messageGUID = new Guid(),
                                      messageText = "KM_OpenControlInGUI",
                                      controlType = _controlType,
                                      o2DockState = _o2DockState,
                                      controlName = _controlName
                                  };
            return kmGuiAction;
        }

        public static KM_GUIAction getGuiAscx(string ascxToGet, Action<object> _returnDataCallback)
        {
            var kmGuiAction = new KM_GUIAction
                                  {
                                      GuiAction = IM_GUIActions.getGuiAscx,
                                      controlName = ascxToGet,
                                        returnDataCallback = _returnDataCallback
                                  };
            return kmGuiAction;
        }

        public static KM_GUIAction isAscxGuiAvailable()
        {
            var kmGuiAction = new KM_GUIAction
            {
                GuiAction = IM_GUIActions.isAscxGuiAvailable                
            };
            return kmGuiAction;            
        }

        internal static KM_GUIAction executeOnAscx(string ascxToExecuteMethod, string targetMethod, string[] methodParameters)
        {
            var kmGuiAction = new KM_GUIAction
            {
                GuiAction = IM_GUIActions.executeOnAscx,
                controlName = ascxToExecuteMethod,
                targetMethod = targetMethod,
                methodParameters = methodParameters
            };
            return kmGuiAction;  
        }

        internal static KM_GUIAction closeAscxParent(string targetAscxControl)
        {
            var kmGuiAction = new KM_GUIAction
            {
                GuiAction = IM_GUIActions.closeAscxParent,
                controlName = targetAscxControl,                
            };
            return kmGuiAction;  
        }

        internal static KM_GUIAction setAscxDockStateAndOpenIfNotAvailable(string typeOfControl, O2DockState _o2DockState, string _controlName)
        {
            var kmGuiAction = new KM_GUIAction
                                  {
                                      GuiAction = IM_GUIActions.setAscxDockStateAndOpenIfNotAvailable,
                                      controlTypeString = typeOfControl,
                                      o2DockState = _o2DockState,
                                      controlName = _controlName
            };
            return kmGuiAction;  
        }
    }
}
