using Merlin;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.Views.ASCX.MerlinWizard.O2Wizard_ExtensionMethods
{
    public static class Ex_Wizard_GUI
    {
        public static void allowNext(this IStep step, bool value)
        {
            step.Controller.wizardForm.invokeOnThread(() => step.Controller.allowNext(value));
        }

        public static void allowBack(this IStep step, bool value)
        {
            step.Controller.wizardForm.invokeOnThread(() => step.Controller.allowBack(value));
        }

        public static void allowCancel(this IStep step, bool value)
        {
            step.Controller.wizardForm.invokeOnThread(() => step.Controller.allowCancel(value));
        }

        public static void next(this IStep step)
        {
            step.Controller.invoke("Advance");
        }

        public static void previous(this IStep step)
        {
            step.Controller.invoke("GoToPrevious");
        }        

        public static void finish(this IStep step)
        {
            step.Controller.invoke("endWizard");
        }

    }
}
