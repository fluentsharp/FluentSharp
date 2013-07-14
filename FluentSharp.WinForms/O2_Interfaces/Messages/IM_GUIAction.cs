// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.Interfaces
{
    public enum IM_GUIActions 
    {
        isAscxGuiAvailable,
        openControlInGui,
        getGuiAscx,
        executeOnAscx,
        closeAscxParent,
        setAscxDockStateAndOpenIfNotAvailable
    }

    public interface IM_GUIAction : IO2Message
    {
        IM_GUIActions GuiAction { get; set; }
        Type controlType { get; set; }
        string controlTypeString { get; set; }        
        O2DockState o2DockState { get; set; }
        string controlName { get; set; }
        string targetMethod { get; set; }
        string[] methodParameters { get; set; }         // forcing the parameters to be strings so they work ok across WCF
        //Callbacks.dMethod_Object returnDataCallback { get; set; }
        Action<object> returnDataCallback { get; set; }
    }
}