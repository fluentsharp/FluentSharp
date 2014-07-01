using System;
using System.Threading;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Controls
{
    public class WinForms_Show
    {
        public static Control showAscxInForm(Type controlType)
    	{
    		return showAscxInForm(controlType, controlType.Name);
    	}
        
        public static Control showAscxInForm(Type controlType, string formTitle, int width = -1, int height = -1, bool startHidden = false)
        {
        	var controlCreation = new AutoResetEvent(false);
        	Control control = null;
            O2Thread.staThread(
                ()=> {
                    O2Gui o2Gui = null;
                    try
                    {
                        control = (Control)PublicDI.reflection.createObjectUsingDefaultConstructor(controlType);
                        if (control != null)
                        {
                            control.Dock = DockStyle.Fill;
                            o2Gui = new O2Gui(width, height) ;        // I might need to adjust these width, height so that the control is the one with this size (and not the hosting form)
                            o2Gui.Text = formTitle;
                            if (width > -1)
                                control.Width = width;
                            else
                                o2Gui.Width = control.Width;            // if it is not defined resize the form to fit the control

                            if (height > -1)
                                control.Height = height;
                            else
                                o2Gui.Height = control.Height;          // if it is not defined resize the form to fit the control                                                   

                            o2Gui.clientSize(control.Width, o2Gui.Height);  // reset the form size to the control's size
                            o2Gui.Controls.Add(control);
                            o2Gui.Load += (sender, e) => controlCreation.Set();
                                
                            if (startHidden)                         // very useful from UnitTests and cases where more should be added to the UI before showing it
                            { 
                                o2Gui.Opacity = 0;
                                o2Gui.ShowInTaskbar = false;
                            }
                            
                            o2Gui.showDialog(false);                            
                        }
                        else
                            controlCreation.Set();
                    }
                    catch (Exception ex)
                    {
                        "in showAscxInForm: {0}".format(ex).error();
                        controlCreation.Set();
                    }
                    finally
                    {
                        if (o2Gui != null)
                            o2Gui.Dispose();
                    }
                });
            var maxTimeOut = System.Diagnostics.Debugger.IsAttached ? -1 : 20000;
            if (controlCreation.WaitOne(maxTimeOut).failed())
                "[WinForms] Something went wrong with the creation of the {0} control with title '{1}' since it took more than 20s to start".error(controlType, formTitle);
            return control;
        }
    }
}