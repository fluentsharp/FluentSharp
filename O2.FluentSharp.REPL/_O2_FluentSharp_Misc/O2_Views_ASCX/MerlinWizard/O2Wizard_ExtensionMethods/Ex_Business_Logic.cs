using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Merlin;
using O2.Views.ASCX.CoreControls;

namespace O2.Views.ASCX.MerlinWizard.O2Wizard_ExtensionMethods
{
    public static class Ex_Business_Logic
    {
        // misc business logic 
        public static string getPathFromStep(this IStep step, int stepId)
        {
            if (step.Controller.steps.Count > stepId)
            {
                var firstControl = step.Controller.steps[stepId].FirstControl;
                if (firstControl != null && firstControl is ascx_Directory)
                {
                    var directory = (ascx_Directory)firstControl;
                    return directory.getCurrentDirectory();
                }
            }
            return "";
        }
    }
}
