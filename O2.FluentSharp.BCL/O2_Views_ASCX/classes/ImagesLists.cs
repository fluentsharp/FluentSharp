using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Views.ASCX.CoreControls;
using System.Windows.Forms;

namespace O2.Views.ASCX.classes
{
    public class ImagesLists
    {
        public static ImageList withFolderAndFile()
        {

            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ascx_Directory));
            var imageList = new System.Windows.Forms.ImageList();
            imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilDirectoriesAndFiles.ImageStream")));
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            imageList.Images.SetKeyName(0, "Explorer_Folder.ico");
            imageList.Images.SetKeyName(1, "Explorer_File.ico");
            return imageList;
        }    
    }
}
