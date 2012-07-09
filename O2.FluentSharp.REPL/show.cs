using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel.CodeUtils;
using System.Windows.Forms;
using System.Drawing;
using O2.Views.ASCX.DataViewers;

//O2File:ExtensionMethods/Views_ExtensionMethods.cs
//O2File:CodeUtils/O2Kernel_O2Thread.cs
//O2File:open.cs

namespace O2.Kernel
{
    public class show
    {
        public static ascx_ShowInfo propertyGrid()
        {
            return "Property Grid".popupWindow(300,300)
                                  .add_Control< ascx_ShowInfo>();
            // open.viewAscx("ascx_ShowInfo", "Property Grid", 300, 300);
        }        

        public static void info(object _object)
        {
            _object.showInfo();            
        }

        public static TextBox file(string fileToShow)
        {
            return open.file(fileToShow);
        }

        public static PictureBox image(string imageToShow)
        {
            return open.image(imageToShow);
        }        

        public static RichTextBox document(string documentToShow)
        {
            return open.document(documentToShow);
        }

        public static T control<T>(T type)
        {
            return type;
        }
    }
    
}
