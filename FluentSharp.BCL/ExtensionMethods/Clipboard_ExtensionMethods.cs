using System;
using System.Threading;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using System.Windows.Forms;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms
{
    public static class Clipboard_ExtensionMethods
    {
        public static void      toClipboard(this string newClipboardText)
        {
            newClipboardText.clipboardText_Set();
        }
        public static string    clipboardText_Set(this string newClipboardText)
        {
            var sync = new AutoResetEvent(false);
            O2Thread.staThread(
                () =>
                {
                    O2Forms.setClipboardText(newClipboardText);
                    sync.Set();
                });
            sync.WaitOne(2000);
            return newClipboardText;
        }
        public static string    clipboardText_Get(this object _object)
        {
            var sync = new AutoResetEvent(false);
            string clipboardText = null;
            O2Thread.staThread(
                () =>
                {
                    clipboardText = O2Forms.getClipboardText();
                    sync.Set();
                });
            sync.WaitOne(2000);
            return clipboardText;
        }
        public static string    saveImageFromClipboard(this object _object)
        {
            var sync = new AutoResetEvent(false);
            string savedImage = null;
            O2Thread.staThread(
                () =>
                {
                    var bitmap = new Control().fromClipboardGetImage();
                    if (bitmap.notNull())
                    {
                        savedImage = bitmap.save();
                        savedImage.toClipboard();
                        "Image in clipboard was saved to: {0}".info(savedImage);
                    }
                    sync.Set();
                });

            sync.WaitOne(2000);

            return savedImage;
        }
        public static string    saveImageFromClipboardToFolder(this string targetFolder)
		{
			var targetFile = targetFolder.pathCombine(DateTime.Now.safeFileName() + ".jpg");
			return targetFile.saveImageFromClipboardToFile();
		}		
		public static string    saveImageFromClipboardToFile(this string targetFile)
		{
			var clipboardImagePath = targetFile.saveImageFromClipboard();
			if (clipboardImagePath.fileExists())
			{				
				var fileToSave = (targetFile.valid()) 
										? targetFile
										: O2Forms.askUserForFileToSave(PublicDI.config.O2TempDir,"*.jpg");
				if (fileToSave.valid())
				{
					Files.moveFile(clipboardImagePath, fileToSave);
					"Clipboard Image saved to: {0}".info(fileToSave);
					return fileToSave;
				}				
			}
			return null;
		}	
    }
}
