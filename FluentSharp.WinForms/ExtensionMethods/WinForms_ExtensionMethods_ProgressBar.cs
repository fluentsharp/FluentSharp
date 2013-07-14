using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_ProgressBar
    { 
        // order of params is: int top, int left, int width, int height)
        public static ProgressBar add_ProgressBar(this Control control, params int[] position)
        {
            if (control.notNull())
                return control.add_Control<ProgressBar>(position);
            return null;
        }
        public static ProgressBar maximum(this ProgressBar progressBar, int value)
        {
            if (progressBar.notNull())
                progressBar.invokeOnThread(() => progressBar.Maximum = value);
            return progressBar;
        }
        public static ProgressBar value(this ProgressBar progressBar, int value)
        {
            if (progressBar.notNull())
                progressBar.invokeOnThread(() => progressBar.Value = value);
            return progressBar;
        }
        public static ProgressBar increment(this ProgressBar progressBar, int value)
        {
            if (progressBar.notNull())
                progressBar.invokeOnThread(() => progressBar.Increment(value));
            return progressBar;
        }
    }
}