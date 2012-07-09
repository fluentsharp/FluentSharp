using System;
using System.Windows.Forms;
using System.Collections.Generic;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Windows;
using O2.Views.ASCX.classes.MainGUI;
using O2.External.SharpDevelop.Ascx;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.XRules.Database.Utils;

namespace O2.XRules.Database.Utils
{
    public class ascx_Panel_With_Inspector : Control
    {
        public Panel panel;
        public ascx_Simple_Script_Editor inspector;

        public static ascx_Panel_With_Inspector runControl()
        {
            var currentProcess = Processes.getCurrentProcess();
            return O2Gui.load<ascx_Panel_With_Inspector>("C# REPL Editor             ({0})".format(clr.details())); //and name: {1})".format(clr.details(), currentProcess.ProcessName));
        }

        public static ascx_Panel_With_Inspector runControl_withParameter(string parameterName, object parameterObject)
        {
            var panelWithInspector = runControl();
            panelWithInspector.inspector.InvocationParameters.Add(parameterName, parameterObject);
            return panelWithInspector;
        }

        public ascx_Panel_With_Inspector()
        {
            try
            {
                this.Width = 600;
                this.Height = 400;

                var controls = this.add_1x1("Panel", "Inspector", false, 200);

                panel = controls[0].add_Panel();

                //graph.testGraph();			
                inspector = controls[1].add_Script();

                if (inspector.isNull())
                {
                    "[ascx_Panel_With_Inspector] add_Script failed, inspector variable was null".error();
                    return;
                }
                inspector.defaultCode = "//var topPanel = O2Gui.open<Panel>(\"{name}\",700,400);".line() +
                                         "var topPanel = panel.clear().add_Panel();".line() +                   // so that we don't get an autosave for this test code
                                        "var textBox = topPanel.add_TextBox(true);".line() +
                                        "textBox.set_Text(\"hello world\");";
                inspector.Code = inspector.defaultCode;
                inspector.InvocationParameters.Add("panel", panel);
                //inspector.InvocationParameters.Add("inspector", inspector);
                //inspector.enableCodeComplete();
            }
            catch (Exception ex)
            {
                ex.log();
            }
        }
    }

}
