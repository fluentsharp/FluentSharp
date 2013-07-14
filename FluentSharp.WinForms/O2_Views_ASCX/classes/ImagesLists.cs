using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms.Utils
{
    public class ImagesLists
    {
        public static ImageList withFolderAndFile()
        {

            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryViewer));
            var imageList = new ImageList();
            imageList.ImageStream = ((ImageListStreamer)(resources.GetObject("ilDirectoriesAndFiles.ImageStream")));
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            imageList.Images.SetKeyName(0, "Explorer_Folder.ico");
            imageList.Images.SetKeyName(1, "Explorer_File.ico");
            return imageList;
        }    
    }
}
