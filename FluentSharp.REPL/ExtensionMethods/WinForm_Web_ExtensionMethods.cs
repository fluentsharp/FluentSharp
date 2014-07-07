using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.Web35;
using FluentSharp.WinForms;

namespace O2.FluentSharp.REPL.ExtensionMethods
{
    public static class WinForm_Web_ExtensionMethods
    {
        public static PictureBox loadFromUri(this PictureBox pictureBox, Uri uri)
        {
            "loading image from Uri into PictureBox".debug();
            pictureBox.Image = uri.getImageAsBitmap();
            return pictureBox;
        }

        public static WebBrowser submitRequest_POST(this WebBrowser webBrowser, string url, string targetFrame, Dictionary<string, string> parameters)
        {
            var postString = "";
            if (parameters != null)
                foreach (var parameter in parameters.Keys)
                    postString += "{0}={1}&".format(parameter, parameters[parameter].urlDecode()); //WebEncoding.urlEncode(parameters[parameter]));
            return webBrowser.submitRequest_POST(url, targetFrame, postString);            
        }

        public static WebBrowser submitRequest_GET(this WebBrowser webBrowser, string url, string targetFrame, Dictionary<string, string> parameters)
        {
            var parametersString = "";
            if (parameters != null)
                foreach (var parameter in parameters.Keys)
                    parametersString += "{0}={1}&".format(parameter, parameters[parameter].urlDecode());//WebEncoding.urlEncode(parameters[parameter]));
            return webBrowser.submitRequest_GET(url, targetFrame, parametersString);
        }
    }
}
