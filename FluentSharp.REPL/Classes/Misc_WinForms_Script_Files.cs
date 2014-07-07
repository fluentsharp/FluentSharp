using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public class Misc_WinForms_Script_Files
    {
        /// <summary>
        /// Creates a file called PopupWindow_With_LogViewer.h2 in the current Temp Dir which will open a popupWindow with a log viewer
        /// </summary>
        /// <returns></returns>
        public static string PopupWindow_With_LogViewer()
        {
            var code  = @"var logViewer = ""Util - LogViewer"".popupWindow(400,140).add_LogViewer();";
            var title = "PopupWindow_With_LogViewer.h2";
            var file  = title.inTempDir();            
            code.saveAs(file);
            return file;
        }
    }
}