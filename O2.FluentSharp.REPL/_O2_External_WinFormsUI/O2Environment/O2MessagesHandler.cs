// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using O2.DotNetWrappers.Windows;
using O2.External.WinFormsUI.Forms;
using O2.Interfaces.Messages;
using O2.Kernel;
using O2.Kernel.InterfacesBaseImpl;

namespace O2.External.WinFormsUI.O2Environment
{
    /// <summary>
    /// Singleton that handles IM_GUIAction O2Messages 
    /// </summary>
    public class O2MessagesHandler
    {
        static O2MessagesHandler()
        {            
            KO2MessageQueue.getO2KernelQueue().onMessages += (o2MessageQueue_onMessages);
        }

        private static void o2MessageQueue_onMessages(IO2Message o2Message)
        {
            try
            {
                if (o2Message is IM_GUIAction)
                {
                    var mGuiAction = (IM_GUIAction) o2Message;
                    PublicDI.log.info("O2GuiWithDockPanel received IM_GUIAction of action: {0}", mGuiAction.GuiAction);
                    switch (mGuiAction.GuiAction)
                    {
                        case IM_GUIActions.isAscxGuiAvailable:
                            isAscxGuiAvailable();
                            break;
                            //   case (IM_GUIActions.openControlInGui): // don't handle these here                     
                        case IM_GUIActions.getGuiAscx:
                            if (mGuiAction.returnDataCallback != null)
                                mGuiAction.returnDataCallback(O2DockUtils.getAscx(mGuiAction.controlName));
                            break;
                        case IM_GUIActions.executeOnAscx:
                            if (mGuiAction.controlName == null || mGuiAction.targetMethod == null ||
                                mGuiAction.methodParameters == null)
                                PublicDI.log.error(
                                    "in O2Environment.O2MessagesHandler.o2MessageQueue_onMessages received a O2Message for IM_GUIActions.executeOnAscx, but either the targetMethod or methodParameters are null");
                            else
                            {
                                var ascxControlToExecute = O2AscxGUI.getAscx(mGuiAction.controlName);
                                if (ascxControlToExecute == null)
                                    PublicDI.log.error(
                                        "in O2MessagesHandler...IM_GUIActions.executeOnAscx, could not get control: {0}",
                                        mGuiAction.controlName);
                                else
                                    o2Message.returnData = PublicDI.reflection.invoke(ascxControlToExecute,
                                                                                mGuiAction.targetMethod,
                                                                                mGuiAction.methodParameters);
                            }
                            break;
                        case IM_GUIActions.closeAscxParent:
                            O2AscxGUI.closeAscxParent(mGuiAction.controlName);
                            break;
                            
                        case IM_GUIActions.openControlInGui:            // this is a special case since we should only handle this if the main GUI is not loaded
                            if (false == O2AscxGUI.isGuiLoaded())                    // this tends to happen on Unit tests where we only have one control loaded
                            {
                                // and if the Gui is not loaded open this control as a stand alone FORM
                                O2AscxGUI.openAscxAsForm(mGuiAction.controlType, mGuiAction.controlName);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, " in O2MessagesHandler.o2MessageQueue_onMessages");
            }                                        
       }

        public static bool isAscxGuiAvailable()
        {
            return O2AscxGUI.o2GuiWithDockPanel != null;
        }
    }
}
