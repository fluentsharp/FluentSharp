using System.Windows.Forms;
using FluentSharp.WinForms;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Controls;

namespace FluentSharp.REPL
{
    public static class ascx_FolderView_ExtensionMethods
    {
        public static ascx_FolderView open(this ascx_FolderView folderView, string path)
        {
            if (folderView.notNull())
                return folderView.loadFolder(path);			
            return folderView;
        }
				
		
        public static ascx_FolderView add_FolderViewer<T>(this T control, string path = null, bool buildGuiOnCtor = true)
            where T: Control
        {			
            return (ascx_FolderView)control.clear().invokeOnThread(
                ()=>{
                        var folderViewer = new ascx_FolderView(buildGuiOnCtor)
                            .fill();
                        control.add_Control(folderViewer);
                        if (buildGuiOnCtor)
                            folderViewer.open(path);
                        else
                            folderViewer.RootFolder = path;
                        return folderViewer;
                });
        }
		
        public static string virtualPath(this ascx_FolderView folderView, string path)
        {
            var virtualPath = path.remove(folderView.RootFolder);
            if (virtualPath.valid())
            {
                if (virtualPath[0]=='\\' || virtualPath[0]=='/')
                    return virtualPath;
                else
                    return "\\{0}".format(virtualPath);
            }
            return virtualPath;
        }
    }
}