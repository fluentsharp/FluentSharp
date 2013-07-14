using System;
using System.Windows.Forms;
using FluentSharp.WinForms;
using FluentSharp.WinForms.Controls;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Utils;

namespace FluentSharp.REPL.Controls
{
    public class ascx_Panel_With_Inspector : Control
    {
        public Panel panel;
        public ascx_Simple_Script_Editor inspector;

        public static ascx_Panel_With_Inspector runControl()
        {
            return runControl(null);
        }
        public static ascx_Panel_With_Inspector runControl(string file)
        {            
            var currentProcess = Processes.getCurrentProcess();
            var panelWithInspector = O2Gui.load<ascx_Panel_With_Inspector>("C# REPL Editor             ({0})".format(clr.details())); //and name: {1})".format(clr.details(), currentProcess.ProcessName));
            if (file.valid())
                panelWithInspector.inspector.openFile(file);
            return panelWithInspector;
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
                this.Width = 800;
                this.Height = 600;

                var controls = this.add_1x1("Panel", "Inspector", false, 200);

                panel = controls[0].add_Panel();

                //graph.testGraph();			
                inspector = controls[1].add_Script();

                if (inspector.isNull())
                {
                    "[ascx_Panel_With_Inspector] add_Script failed, inspector variable was null".error();
                    return;
                }
                inspector.defaultCode = "//var topPanel = \"{name}\".popupWindow(700,400);".line() +
                                         "var topPanel = panel.clear().add_Panel();".line() +                   // so that we don't get an autosave for this test code
                                        "var textBox = topPanel.add_TextBox(true);".line() +
                                        "textBox.set_Text(\"hello world\");";
                inspector.Code = inspector.defaultCode;
                inspector.InvocationParameters.Add("panel", panel);
                inspector.showLogViewer();
                inspector.compile();
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
