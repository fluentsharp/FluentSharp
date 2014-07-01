using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_WebBrowser_Add_Control
    {
        public static WebBrowser    add_WebBrowser(this Control control)
        {
            return control.add_WebBrowser_Control();
        }
        public static WebBrowser    add_WebBrowser_Control<T>(this T control) where T : Control
        {
            return control.add_Control<WebBrowser>();
        }        
        public static WebBrowser    add_WebBrowser_with_NavigationBar(this Control control)
        {
            return control.add_WebBrowser_Control().add_NavigationBar();
        }
        public static WebBrowser    add_NavigationBar(this WebBrowser webBrowser)
        {
            Action<string> openUrl =
                (url) =>
                {
                    "[WebBrowser] opening: {0}".info(url);
                    webBrowser.open(url);
                };

            var actionPanel = webBrowser.insert_Above(40, "location")
                                        .add_LabelAndComboBoxAndButton("Url", "", "Go", (text) => { });
            var comboBox = actionPanel.controls<ComboBox>();
            var button = actionPanel.controls<Button>().onClick(() => openUrl(comboBox.get_Text()));
            comboBox.onEnter(openUrl);
            webBrowser.on_Navigated(
                (url) =>
                {
                    if (url.str() != "about:blank")
                        comboBox.add_Item(url).selectLast();
                });


            return webBrowser;
        }
        public static WebBrowser    show_in_Browser(this string url)
        {
            return url.uri().show_in_Browser();
        }
        public static WebBrowser    show_in_Browser(this Uri uri)
        {
            return "Web Brower for: {0}".format(uri.str())
                                        .popupWindow()
                                        .add_WebBrowser_Control().add_NavigationBar()
                                        .open(uri.str());

        }
        public static WebBrowser    view_Html(this string html)
        {
            return html.view_Html("View Html in Browser".popupWindow());
        }
        public static WebBrowser    view_Html(this string html, Control control)
        {
            return control.clear().add_WebBrowser().set_Text(html);
        }
        public static HtmlElement   id(this WebBrowser webBrowser, string id)
        {
            return webBrowser.getElementById(id);
        }
        public static List<string>  ids(this WebBrowser webBrowser)
        {
            return webBrowser.all().where((htmlElement) => htmlElement.Id.valid())
                                   .Select((htmlElement) => htmlElement.Id).toList();
        }
        public static List<HtmlElement> names(this WebBrowser webBrowser)
        {
            return webBrowser.all().where((htmlElement) => htmlElement.Name.valid());
        }
        public static string            html(this HtmlElement htmlElement)
        {
            return htmlElement.outerHtml();
        }
    }

    public static class WinForms_ExtensionMethods_WebBrowser_Open
    {
        public static WebBrowser open(this WebBrowser webBrowser, string url)
        {
            webBrowser.invokeOnThread(() =>
                {
                    webBrowser.Navigate(url);
                    return webBrowser;
                });
            if (url.uri().isWebRequest())
                webBrowser.waitForCompleted();
            return webBrowser;
        }
        public static WebBrowser open_ASync(this WebBrowser webBrowser, string url)
        { 
            O2Thread.mtaThread(
                    ()=>{
                            webBrowser.open(url);
                        });
            return webBrowser;
        }
        public static WebBrowser openDirectoryInWebBrowser(this WebBrowser wbWebBrowser, String sDirectoryToOpen)
        {
            sDirectoryToOpen = sDirectoryToOpen.Replace(@"\\", @"\");
            // Web Browser Navigate object doesn't like \\ in the path
            
            return wbWebBrowser.open(sDirectoryToOpen.dirExists() 
                                        ? "file://" + sDirectoryToOpen
                                        : "about:blank");
        }
        public static WebBrowser submitRequest_POST(this WebBrowser webBrowser, string url, string targetFrame, Dictionary<string, string> parameters)
        {
            var postString = "";
            if (parameters != null)
                foreach (var parameter in parameters.Keys)
                    postString += "{0}={1}&".format(parameter, parameters[parameter].urlDecode()); //WebEncoding.urlEncode(parameters[parameter]));
            return webBrowser.submitRequest_POST(url, targetFrame, postString);            
        }
        public static WebBrowser submitRequest_POST(this WebBrowser webBrowser, string url, string targetFrame, string postString)
        {
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return webBrowser.submitRequest_POST(url, targetFrame, postData);            
        }
        public static WebBrowser submitRequest_POST(this WebBrowser webBrowser, string url, string targetFrame, byte[] postData)
        {
            return webBrowser.invokeOnThread(() =>
                {
                    try
                    {
                        var uri = new Uri(url);
                        const string additionalHeaders = "Content-Type: application/x-www-form-urlencoded";
                        webBrowser.Navigate(uri, targetFrame, postData, additionalHeaders);
                        return webBrowser.waitForCompleted();
                    }
                    catch (Exception ex)
                    {
                        ex.log("in submitRequest_POST");
                        return webBrowser;
                    }                    
                });

        }
        public static WebBrowser submitRequest_GET(this WebBrowser webBrowser, string url)
        {
            return webBrowser.submitRequest_GET(url, "", "");
        }
        public static WebBrowser submitRequest_GET(this WebBrowser webBrowser, string url, string targetFrame, Dictionary<string, string> parameters)
        {
            var parametersString = "";
            if (parameters != null)
                foreach (var parameter in parameters.Keys)
                    parametersString += "{0}={1}&".format(parameter, parameters[parameter].urlDecode());//WebEncoding.urlEncode(parameters[parameter]));
            return webBrowser.submitRequest_GET(url, targetFrame, parametersString);
        }
        public static WebBrowser submitRequest_GET(this WebBrowser webBrowser, string url, string targetFrame, string parametersString)
        {
            return webBrowser.invokeOnThread(() =>
                {
                    try
                    {
                        if (parametersString.valid())
                            url += "?" + parametersString;
                        var uri = new Uri(url);
                        webBrowser.Navigate(uri, targetFrame);
                        return webBrowser.waitForCompleted();
                    }
                    catch (Exception ex)
                    {
                        ex.log("in submitRequest_GET");
                        return webBrowser;
                    }
                    
                });
        }
        public static WebBrowser stop(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => { browser.Stop(); return browser; });
        }
        
    }
    public static class WinForms_ExtensionMethods_WebBrowser_Dialogs
    {
        public static bool showContextMenu(this WebBrowser browser)
        {
            return browser.showContextMenu(-1, -1);
        }
        public static bool showContextMenu(this WebBrowser browser, int x, int y)
        {
            return browser.invokeOnThread(() => (bool)browser.invoke("ShowContextMenu", x,y));
        }
        public static WebBrowser showPageSetupDialog(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => { browser.ShowPageSetupDialog(); return browser; });
        }
        public static WebBrowser showPrintDialog(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => { browser.ShowPrintDialog(); return browser; });
        }
        public static WebBrowser showPrintPreviewDialog(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => { browser.ShowPrintPreviewDialog(); return browser; });
        }
        public static WebBrowser showPropertiesDialog(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => { browser.ShowPropertiesDialog(); return browser; });
        }
        public static WebBrowser showSaveAsDialog(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => { browser.ShowSaveAsDialog(); return browser; });
        }        
    }

    public static class WinForms_ExtensionMethods_WebBrowser_Events
    {
        public static int        WEBBROWSER_WAITFORCOMPLETE_MAXTIME = 25000; //25 seconds delay time
        public static WebBrowser waitForCompleted(this WebBrowser browser)
        {
            return browser.waitForCompleted(WEBBROWSER_WAITFORCOMPLETE_MAXTIME);
        }
        public static WebBrowser waitForCompleted(this WebBrowser browser, int maxWait)
        {
            var url = browser.url();            
            var o2Timer = new O2Timer("Loaded page: {0}".format(url)).start();
            var sync = new AutoResetEvent(false);
            var abortThread = false;            
            var thread = O2Thread.mtaThread(
                () =>
                {
                    //if (browser.readyState() == WebBrowserReadyState.Complete && browser.isBusy().isFalse() && url.str().contains("about:blank").isFalse())
                    //    "[waitForCompleted] browser.readyState() was set to Complete and browser.isBusy was false when this was called".error();
                    while ((browser.readyState() != WebBrowserReadyState.Complete || 
                           browser.isBusy().isTrue())  && abortThread.isFalse())
                        browser.wait(100, false);
					if (url.str() != "about:blank")
					{
						"Waited for completed for: {0} : {1}".debug(url, browser.url());
						o2Timer.stop();
					}
                    sync.Set();
                });
            if (sync.WaitOne(maxWait).isFalse())
            {
                "Waiting for completion FAILED for: {0}".error(browser.url());
                abortThread = true;
            }
            return browser;
        }

        //captured events
        public static WebBrowser on_DocumentCompleted       (this WebBrowser webBrowser, Action<Uri> callback)
        {
            "[WebBrowser] setting: on_DocumentCompleted".info();
            webBrowser.invokeOnThread(() => webBrowser.DocumentCompleted += (sender, e) => callback(e.Url));
            return webBrowser;
        }
        public static WebBrowser on_DocumentTitleChanged    (this WebBrowser webBrowser, Action callback)
        {
            "[WebBrowser] setting: on_DocumentTitleChanged".info();
            webBrowser.invokeOnThread(() => webBrowser.DocumentTitleChanged += (sender, e) => callback());
            return webBrowser;
        }
        public static WebBrowser on_EncryptionLevelChanged  (this WebBrowser webBrowser, Action callback)
        {
            "[WebBrowser] setting: on_EncryptionLevelChanged".info();
            webBrowser.invokeOnThread(() => webBrowser.EncryptionLevelChanged += (sender, e) => callback());
            return webBrowser;
        }        
        public static WebBrowser on_FileDownload            (this WebBrowser webBrowser, Action callback)
        {
            "[WebBrowser] setting: on_FileDownload".info();
            webBrowser.invokeOnThread(() => webBrowser.FileDownload += (sender, e) => callback());
            return webBrowser;
        }
        public static WebBrowser on_Navigated               (this WebBrowser webBrowser, Action<Uri> callback)
        {
            "[WebBrowser] setting: on_Navigated".info();
            webBrowser.invokeOnThread(() => webBrowser.Navigated += (sender, e) => callback(e.Url));
            return webBrowser;
        }
        public static WebBrowser on_Navigating              (this WebBrowser webBrowser, Func<string, Uri,bool> callback)
        {
            "[WebBrowser] setting: on_Navigating".info();
            webBrowser.invokeOnThread(() => webBrowser.Navigating += (sender, e) => 
                {
                    e.Cancel = callback(e.TargetFrameName, e.Url);
                });
            return webBrowser;
        }
        public static WebBrowser on_NewWindow               (this WebBrowser webBrowser, Func<bool> callback)
        {
            "[WebBrowser] setting: on_NewWindow".info();
            webBrowser.invokeOnThread(() => webBrowser.NewWindow += (sender, e) => 
                {
                    e.Cancel = callback();
                });
            return webBrowser;
        }
        public static WebBrowser on_ProgressChanged         (this WebBrowser webBrowser, Action<long,long> callback)
        {
            "[WebBrowser] setting: on_ProgressChanged".info();
            webBrowser.invokeOnThread(() => webBrowser.ProgressChanged += (sender, e) => 
                callback(e.CurrentProgress, e.MaximumProgress));
            return webBrowser;
        }
        public static WebBrowser on_StatusTextChanged       (this WebBrowser webBrowser, Action callback)
        {
            "[WebBrowser] setting: on_StatusTextChanged".info();
            webBrowser.invokeOnThread(() => webBrowser.StatusTextChanged += (sender, e) => 
                    callback());
            return webBrowser;
        }

        //event logging
        public static WebBrowser log_DocumentCompleted      (this WebBrowser browser)
        {
            browser.on_DocumentCompleted((url) => "[WebBrowser][on_DocumentCompleted] {0}    [isBusy: {1}] [readyState: {2}".format(url, browser.isBusy(), browser.readyState()).debug());
            return browser;
        }
        public static WebBrowser log_DocumentTitleChanged   (this WebBrowser browser)
        {
            browser.on_DocumentTitleChanged(() => "[WebBrowser][on_DocumentTitleChanged] {0}".format(browser.document_Title()).debug());
            return browser;
        }
        public static WebBrowser log_EncryptionLevelChanged (this WebBrowser browser)
        {
            browser.on_EncryptionLevelChanged(() => "[WebBrowser][on_EncryptionLevelChanged] {0}".format(browser.encryptionLevel()).info());
            return browser;
        }
        public static WebBrowser log_FileDownload           (this WebBrowser browser)
        {
            browser.on_FileDownload(() => "[WebBrowser][on_FileDownload]".debug());
            return browser;
        }
        public static WebBrowser log_OnNavigated            (this WebBrowser browser)
        {
            browser.on_Navigated((url) => "[WebBrowser][Navigated] {0} [isBusy: {1}] [readystate: {2}]".format(url, browser.isBusy() , browser.readyState()).debug());
            return browser;
        }
        public static WebBrowser log_Navigating             (this WebBrowser browser)
        {
            browser.on_Navigating((targetFrame, url) => 
                    {
                       "[WebBrowser][Navigating] {0} : {1}".format(targetFrame, url).debug();
                        return false;
                    });
            return browser;
        }
        public static WebBrowser log_NewWindow              (this WebBrowser browser)
        {
            browser.on_NewWindow(() =>
                {
                    "[WebBrowser][on_NewWindow]".format().debug();
                    return false;
                });
            return browser;
        }
        public static WebBrowser log_ProgressChanged        (this WebBrowser browser)
        {
            browser.on_ProgressChanged((currentProgress, maximumProgress) => "[WebBrowser][on_ProgressChanged] CurrentProgress: {0}  MaximumProgress: {1}".format(currentProgress, maximumProgress).info());
            return browser;
        }
        public static WebBrowser log_StatusTextChanged      (this WebBrowser browser)
        {
            browser.on_StatusTextChanged(() => "[WebBrowser][on_StatusTextChanged] {0}".format(browser.statusText()).info());
            return browser;
        }
        public static WebBrowser log_All_AvailableEvents    (this WebBrowser browser)
        {
            return browser.log_DocumentCompleted()
                          //.log_DocumentTitleChanged()
                          //.log_EncryptionLevelChanged()
                          //.log_FileDownload()
                          .log_OnNavigated()
                          .log_NewWindow()
                          //.log_ProgressChanged()
                          //.log_StatusTextChanged()
                          ;
        }
    }

    public static class WinForms_ExtensionMethods_WebBrowser_Html
    {
        public static string        html(this WebBrowser browser)
        {
            return browser.get_Html();
        }
        public static WebBrowser    html(this WebBrowser browser, string text)
        {
            return browser.set_Html(text);
        }
        public static WebBrowser    set_Text(this WebBrowser browser, string text)
        {
            return browser.set_Html(text);
        }
        public static WebBrowser    set_Html(this WebBrowser browser, string text)
        {
            browser.invokeOnThread(
                    () =>
                    {                        
                        browser.DocumentText = text;						
                        return browser;
                    });
			browser.waitForCompleted();
			return browser;
        }
        public static string        get_Text(this WebBrowser browser)
        {
            return browser.get_Html();
        }
        public static string        get_Html(this WebBrowser browser)
        {
            return browser.invokeOnThread(
                    () =>
                    {
                        try
                        {
                            return browser.DocumentText;
                        }
                        catch (Exception ex)
                        {
                            ex.log();
                            return null;
                        }
                    });
        }        
        public static WebBrowser    showMessage(this WebBrowser browser, string message)
        {
            return browser.showMessage(message, false);
        }
        public static WebBrowser    showMessage(this WebBrowser browser, string message, int sleepValue)
        {
            browser.showMessage(message);
            browser.wait(sleepValue, false);
            return browser;
        }
        public static WebBrowser    showMessage(this WebBrowser browser, string message, bool runOnSeparateThread)
        {
            message = message.Replace("".line(), "<br/>");
            var messageTemplate = "<html><body><div style = \"position:absolute; top:50%; width:100%; text-align: center;font-size:20pt; font-family:Arial\">{0}</div></body></html>";

            if (runOnSeparateThread)
                O2Thread.mtaThread(() => { browser.set_Html(messageTemplate.format(message)); });
            else
                browser.set_Html(messageTemplate.format(message));
            return browser;
        }
    }

    public static class WinForms_ExtensionMethods_WebBrowser_Browser
    {
        public static bool                      allowNavigation(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.AllowNavigation);
        }
        public static WebBrowser                allowNavigation(this WebBrowser browser, bool value)
        {
            return browser.invokeOnThread(() => { browser.AllowNavigation = value; return browser; });
        }
        public static WebBrowserEncryptionLevel encryptionLevel(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.EncryptionLevel);
        }        
        public static bool                      isBusy(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.IsBusy);
        }
        public static WebBrowserReadyState      readyState(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.ReadyState);
        }
        public static bool                      scriptErrorsSuppressed(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.ScriptErrorsSuppressed);
        }
        public static WebBrowser                scriptErrorsSuppressed(this WebBrowser browser, bool value)
        {
            return browser.invokeOnThread(() => { browser.ScriptErrorsSuppressed = value; return browser; });
        }
        public static bool                      scrollBarsEnabled(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.ScrollBarsEnabled);
        }
        public static WebBrowser                scrollBarsEnabled(this WebBrowser browser, bool value)
        {
            return browser.invokeOnThread(() => { browser.ScrollBarsEnabled = value; return browser; });
        }
        public static string                    statusText(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.StatusText);
        }
        public static Uri                       url(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.Url);
        }
        public static WebBrowser                url(this WebBrowser browser, Uri value)
        {
            return browser.invokeOnThread(() => { browser.Url = value; return browser; });
        }
        public static Version                   version(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.Version);
        }
        public static bool                      isSilent(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => (bool)browser.ActiveXInstance.prop("Silent"));
        }
        public static WebBrowser                silent(this WebBrowser browser, bool value)
        {
            return browser.invokeOnThread(() =>
                {
                    browser.ActiveXInstance.prop("Silent", value);
                    return browser;
                });

        }
    }

    public static class WinForms_ExtensionMethods_WebBrowser_HtmlDocument
    {
        public static HtmlDocument  document(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.Document);
        }
        public static string        document_Title(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.DocumentTitle);
        }
        public static string        document_Type(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.DocumentType);
        }
        public static Stream        document_Stream(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.DocumentStream);
        }
        public static WebBrowser    document_Type(this WebBrowser browser, Stream stream)
        {
            return browser.invokeOnThread(() =>
                    {
                        browser.DocumentStream = stream;
                        return browser;
                    });
        }

        public static string        cookie(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.document().Cookie);
        }
        public static WebBrowser    cookie(this WebBrowser browser, string value)
        {
            return browser.invokeOnThread(
                () =>
                {
                    var document = browser.document();
                    if (document != null)
                        document.Cookie = value;
                    return browser;
                });
        }

        public static WebBrowser    clearCookie(this WebBrowser browser)
        {
            return browser.cookie("");
        }

    }

    public static class WinForms_ExtensionMethods_WebBrowser_Scripting
    {
        public static object invokeScript(this WebBrowser browser, string functionName, params object[] parameters)
        {
            return browser.invokeOnThread(() => browser.Document.InvokeScript(functionName, parameters));
        }
        public static object eval(this WebBrowser browser, string evalCode)
        {
            return browser.invokeScript("eval", evalCode);
        }
    }

    public static class WinForms_ExtensionMethods_WebBrowser_DefaultSites
    {
        public static WebBrowser noPage(this WebBrowser browser)
    	{
    		return browser.about_blank();
    	}
    	public static WebBrowser about_blank(this WebBrowser browser)
    	{
            return browser.open("about:blank");
    	}
    	public static WebBrowser google(this WebBrowser browser)
    	{
            return browser.open("http://google.com");
    	}	
    	public static WebBrowser owasp(this WebBrowser browser)
    	{
            return browser.open("http://owasp.org");
    	}   	
    	public static WebBrowser bbc(this WebBrowser browser)
    	{
            return browser.open("http://news.bbc.co.uk");
    	}  	
    	public static WebBrowser teamMentor(this WebBrowser browser)
    	{
            return browser.open("http://owasp.teammentor.net");
    	}
    }
    //public static class WinForms_ExtensionMethods_WebBrowser_



    //*****************************************
    //TO MOVE TO CORRECT CLASS
    //*****************************************


    public static class Extra_Browser
	{
		public static WebBrowser alert(this WebBrowser browser, string command)
		{
			browser.eval("alert({0})".format(command));
			return browser;
		}
		
		
		public static WebBrowser inject_JQuery(this WebBrowser browser)
		{
			//can also be done like this
			//ie.add_Script_To_Head(@"jquery-1.5.2.min.js".local().fileContents());
			browser.eval(@"jquery-1.5.2.min.js".local().fileContents());			
			return browser;
		}	
		
		public static WebBrowser emptyPage(this WebBrowser browser)
		{
			var emptyPageHtml = @"<!DOCTYPE html><head></head><body></body></html>";
			return browser.html(emptyPageHtml);			
		}
		
		public static WebBrowser add_CSS_Url(this WebBrowser browser, string url)
		{
			var link = browser.createElement("link");
			link.attribute("href", url)
				.attribute("rel","stylesheet");
			browser.head().appendChild(link);	
			return browser;
		}
		public static WebBrowser add_Javascript_Url(this WebBrowser browser, string url)
		{
			var link = browser.createElement("script");
			link.attribute("src", url)
				.attribute("type","text/javascript");
			browser.head().appendChild(link);	
			return browser;
		}
		
	}
	public static class Extra_Browser_Document
	{
        //COM Objects (so that we don't have to add a reference to mshtml
        [ComImport, ComVisible(true), Guid(@"3050f28b-98b5-11cf-bb82-00aa00bdce0b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        [TypeLibType(TypeLibTypeFlags.FDispatchable)]
        public interface IHTMLScriptElement
        {
            [DispId(1006)]
            string text { set; [return: MarshalAs(UnmanagedType.BStr)] get; }
        }

		public static WebBrowser add_Script_To_Head(this WebBrowser browser, string scriptText)
		{
			try
			{
				var script = browser.createElement(@"script");			    
		        var element = (IHTMLScriptElement)script.DomElement;
		        element.text = scriptText;
		            
		        browser.head().appendChild(script);
		    }
		    catch(Exception ex)
		    {
		    	ex.log();
		    }
		    return browser;
		}
		
		//this is a massive hack because IE doesn't seem to like the dynamic creation of style elements (we get the error: "Property is not supported on this type of HtmlElement.") when trying to set the style element value
		public static WebBrowser add_Style_To_Body(this WebBrowser browser, string styleText)
		{
			browser.body().innerHtml_Append("<span style='block:hidden'>&nbsp</span>" + // need to add some html so that IE accepts the style command below (no idea why we need this, but it works)
											"<style>" + styleText + "</style>");
			return browser;
		}
	
	
		public static HtmlElement activeElement(this WebBrowser browser)
		{
			return browser.invokeOnThread(()=> browser.document().ActiveElement);
		}
		
		public static List<HtmlElement> all(this WebBrowser browser)
		{			
			return browser.invokeOnThread(()=> browser.document().All).toList();			
		}
		
		public static List<HtmlElement> all(this WebBrowser browser, string tagName)
		{
			return browser.getElementsByTagName(tagName);
		}
		
		public static HtmlElement body(this WebBrowser browser)
		{
			return browser.invokeOnThread(()=> browser.document().Body);
		}
		
		public static string html_Rendered(this WebBrowser browser)
		{
			return browser.all("HTML").first().outerHtml();
		}
		
		public static HtmlElement head(this WebBrowser browser)
		{
			return browser.getElementsByTagName("head").first();  
		}
				
		public static string        domain(this WebBrowser browser)
        {
            return browser.invokeOnThread(() => browser.document().Domain);
        }
        public static WebBrowser    Domain(this WebBrowser browser, string value)
        {
            return browser.invokeOnThread(
                () =>
                {
                    browser.document().Domain = value;                    
                    return browser;
                });
        }        
        public static bool focused(this WebBrowser browser)
		{
			return browser.invokeOnThread(()=> browser.document().Focused);
		}
		public static List<HtmlElement> forms(this WebBrowser browser)
		{			
			return browser.invokeOnThread(()=> browser.document().Forms).toList();			
		}
		public static List<HtmlElement> images(this WebBrowser browser)
		{			
			return browser.invokeOnThread(()=> browser.document().Images).toList();			
		}
		public static List<HtmlElement> links(this WebBrowser browser)
		{			
			return browser.invokeOnThread(()=> browser.document().Links).toList();			
		}
		public static HtmlWindow window(this WebBrowser browser)
		{
			return browser.invokeOnThread(()=> browser.document().Window);
		}		
		public static List<HtmlElement> toList(this HtmlElementCollection htmlElements)
		{
			var list =  new List<HtmlElement>();
			if (htmlElements.notNull())
			{
				try
				{
					foreach(HtmlElement htmlElement in htmlElements)			
						list.add(htmlElement);
				}
				catch(Exception ex)
				{
					ex.log("in List<HtmlElement> toList()");
				}
			}
			return list;
		}
		
		public static WebBrowser write(this WebBrowser browser, string html)
		{
			return browser.invokeOnThread(()=>
				{
					browser.document().Write(html);
					return browser; 
				});
		}
		
		public static HtmlDocument newHtmlDocument(this WebBrowser browser)
		{
			return browser.invokeOnThread(()=>
				{
					return browser.document().OpenNew(false);					
				});
		}

		//actions

		public static HtmlElement createElement(this WebBrowser browser, string tagName , HtmlElement targetElement)
		{
			var newElement = browser.createElement(tagName);
			targetElement.appendChild(newElement);
			return newElement;
		}
		public static HtmlElement createElement(this WebBrowser browser, string tagName)
		{
			return browser.invokeOnThread(() => browser.document().CreateElement(tagName));
		}
		
		public static WebBrowser createElement(this WebBrowser browser, string command, bool showUI, object value)
		{
			return browser.invokeOnThread(()=>
				{
					browser.document().ExecCommand(command, showUI, value);
					return browser;
				});
		}
		
		public static WebBrowser focus(this WebBrowser browser)
		{
			return browser.invokeOnThread(()=>
				{
					browser.document().Focus();
					return browser;
				});
		}
		
		public static HtmlElement getElementById(this WebBrowser browser, string id)
		{
			return browser.invokeOnThread(()=> browser.document().GetElementById(id));
		}
		public static HtmlElement getElementFromPoint(this WebBrowser browser, Point point)
		{
			return browser.invokeOnThread(()=> browser.document().GetElementFromPoint(point));
		}
		public static List<HtmlElement> getElementsByTagName(this WebBrowser browser, string tagName)
		{
			return browser.invokeOnThread(()=> browser.document().GetElementsByTagName(tagName)).toList();
		}
		
	}
	//need to see if there are any threading issues here
    public static class Extra_Browser_HtmlElement
    {
        public static List<HtmlElement> ofType(this List<HtmlElement> htmlElements, string typeName)
        {
            return htmlElements.tag(typeName);
        }
        public static List<HtmlElement> tag(this List<HtmlElement> htmlElements, string tag)
        {
            return htmlElements.where((htmlElement) => htmlElement.TagName == tag);
        }

        public static List<HtmlElement> all(this HtmlElement htmlElement)
        {
            return htmlElement.All.toList();
        }

        public static List<HtmlElement> all(this HtmlElement htmlElement, string tagName)
        {
            return htmlElement.GetElementsByTagName(tagName).toList();
        }

        public static HtmlElement called(this List<HtmlElement> htmlElements, string name)
        {
            return htmlElements.withName(name);
        }

        public static HtmlElement withName(this List<HtmlElement> htmlElements, string name)
        {
            return htmlElements.Where((htmlElement) => htmlElement.Name == name).first();
        }

        public static string attribute(this HtmlElement htmlElement, string name)
        {
            return htmlElement.GetAttribute(name);
        }

        public static HtmlElement attribute(this HtmlElement htmlElement, string name, string value)
        {
            htmlElement.SetAttribute(name, value);
            return htmlElement;
        }

        public static string value(this HtmlElement htmlElement)
        {
            return htmlElement.attribute("value");
        }

        public static HtmlElement value(this HtmlElement htmlElement, string value)
        {
            return htmlElement.attribute("value", value);
        }
		
        public static HtmlElement appendChild(this HtmlElement htmlElement, HtmlElement newElement)
        {
            htmlElement.AppendChild(newElement);
            return htmlElement;
        }

        public static HtmlElement focus(this HtmlElement htmlElement)
        {
            htmlElement.Focus();
            return htmlElement;
        }

        public static HtmlElement scrollIntoView(this HtmlElement htmlElement)
        {
            htmlElement.ScrollIntoView(true); //alignWithTop = true
            return htmlElement;
        }

        public static HtmlElement with_InnerText(this List<HtmlElement> htmlElements, string value)
        {
            return htmlElements.Where((htmlElement) => htmlElement.InnerText == value).first();
        }
        public static HtmlElement with_InnerHtml(this List<HtmlElement> htmlElements, string value)
        {
            return htmlElements.Where((htmlElement) => htmlElement.InnerHtml == value).first();
        }
        public static HtmlElement with_OuterText(this List<HtmlElement> htmlElements, string value)
        {
            return htmlElements.Where((htmlElement) => htmlElement.OuterText == value).first();
        }
        public static HtmlElement with_OuterHtml(this List<HtmlElement> htmlElements, string value)
        {
            return htmlElements.Where((htmlElement) => htmlElement.OuterHtml == value).first();
        }


        public static string innerText(this HtmlElement htmlElement)
        {
            return htmlElement.InnerText;
        }
        public static string innerHtml(this HtmlElement htmlElement)
        {
            return htmlElement.InnerHtml;
        }
		
        public static string outerText(this HtmlElement htmlElement)
        {
            return htmlElement.OuterText;
        }
        public static string outerHtml(this HtmlElement htmlElement)
        {
            return htmlElement.OuterHtml;
        }

		public static HtmlElement innerHtml_Append(this HtmlElement htmlElement, string value)
		{
			var currentValue = htmlElement.InnerHtml ?? "";
			htmlElement.innerHtml(currentValue + value);
			return htmlElement;
		}

        public static HtmlElement innerText(this HtmlElement htmlElement, string value)
        {
            try
            {
                htmlElement.InnerText = value;
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return htmlElement;
        }
        public static HtmlElement innerHtml(this HtmlElement htmlElement, string value)
        {
            try
            {
                htmlElement.InnerHtml = value;				
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return htmlElement;
        }
        public static HtmlElement outerText(this HtmlElement htmlElement, string value)
        {
            try
            {
                htmlElement.OuterText = value;
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return htmlElement;
        }
        public static HtmlElement outerHtml(this HtmlElement htmlElement, string value)
        {
            try
            {
                htmlElement.OuterHtml = value;
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return htmlElement;
        }


        //events
        public static HtmlElement click(this HtmlElement htmlElement)
        {
            return htmlElement.invokeMember("Click");
        }
        public static HtmlElement invokeMember(this HtmlElement htmlElement, string member)
        {
            htmlElement.InvokeMember(member);
            return htmlElement;
        }

        public static HtmlElement raiseEvent(this HtmlElement htmlElement, string name)
        {
            htmlElement.RaiseEvent(name);
            return htmlElement;
        }

    }
}
