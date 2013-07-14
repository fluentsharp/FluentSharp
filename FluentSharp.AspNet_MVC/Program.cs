using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CassiniDev;
using FluentSharp.WinForms;
using FluentSharp.CassiniDev;
using FluentSharp.CoreLib;

namespace FluentSharp.AspNet_MVC
{
    public class Program
    {
        public static void Main()
        {           
            var server = new Server("test".tempDir());
            var host = server.invoke("GetHost");


            var cassini = new API_Cassini();
            cassini.start();
            var browser = "FluentSharp.AspNet_Mvc".popupWindow()
                                                 .add_WebBrowser()
                                                 .add_NavigationBar();
            browser.open(cassini.url());
            browser.waitForClose();
            cassini.stop();
        }
    }
}
