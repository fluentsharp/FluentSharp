using System.Windows.Controls;
using FluentSharp.CoreLib;

namespace FluentSharp.WPF
{
    public static class WebBrowser_ExtensionMethods
    {
	
        #region WebBrowser (WPF one which is a wrapper on the WinForms one)

        public static WebBrowser open(this WebBrowser webBrowser, string url)
        {
            if (url.isUri())
            {
                "[WPF WebBrowser] opening page: {0}".debug(url);
                webBrowser.wpfInvoke(() => webBrowser.Navigate(url.uri()));
            }
            return webBrowser;
        }

        #endregion

    }
}