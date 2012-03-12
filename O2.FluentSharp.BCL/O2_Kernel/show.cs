using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel.ExtensionMethods;
using O2.Kernel.CodeUtils;
using System.Windows.Forms;

//O2File:ExtensionMethods/Views_ExtensionMethods.cs
//O2File:CodeUtils/O2Kernel_O2Thread.cs
//O2File:open.cs

namespace O2.Kernel
{
    public class show
    {
        public static object propertyGrid()
        {
            return open.viewAscx("ascx_ShowInfo", "Property Grid", 300, 300);
        }        

        public static void info(object _object)
        {
            O2Kernel_O2Thread.mtaThread(
                () =>
                {
                    if (_object != null)
                    {                        
                        var propertyGrid = open.viewAscx("ascx_ShowInfo", _object.typeName(), 300, 300);
                        propertyGrid.invoke("show", _object);
                    }
                });
        }

        public static TextBox file(string fileToShow)
        {
            return open.file(fileToShow);
        }

        public static PictureBox image(object imageToShow)
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
