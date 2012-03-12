using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//O2Ref:Merlin.dll
using Merlin;
using MerlinStepLibrary;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Windows;
using System.Threading;

namespace O2.Views.ASCX.MerlinWizard
{
    public class MerlinUtils
    {
        public static Thread runWizardWithSteps(List<IStep> steps, string wizardName)
        {
            return runWizardWithSteps(steps, wizardName,-1,-1 , null);
        }

        public static Thread runWizardWithSteps(List<IStep> steps, string wizardName, int width, int height)
        {
            return runWizardWithSteps(steps, wizardName, width, height, null);
        }

		public static Thread runWizardWithSteps(List<IStep> steps, string wizardName, int width, int height, Action<WizardController, WizardController.WizardResult> onCompletion)
		{			
			// this needs to run on an STA thread because some controls might require Drag & Drop support

			return O2Thread.staThread(
				()=> {
						WizardController wizardController = new WizardController(steps);                       
			            //wizardController.LogoImage = Resources.NerlimWizardHeader;
                        var wizardResult = wizardController.StartWizard(wizardName, true, null, width, height);
                        if (onCompletion != null)
                            onCompletion(wizardController, wizardResult);
			         });						
		}
    }
}
