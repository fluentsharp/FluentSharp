using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp_WinForms
{
    [TestFixture]
    public class Test_WinForms_ExtensionMethods_WebBrowser  : NUnitTests
    {
        public string url_O2Platform = "http://o2platform.com/";

        [SetUp]
        public void setUp()
        {
            if (Web.Online.isFalse())
                assert_Ignore("Skipping tests since we are offline");
        }
        [Test(Description="Adds a WebBrowser control to an parent Control (usually a Panel)")]
        public void add_WebBrowser()
        {            
            var browser = "webBrower".popupWindow_Hidden()
                                     .add_WebBrowser();
            browser.assert_Is_Not_Null()
                   .assert_Is_Instance_Of<WebBrowser>();
            
            assert_Are_Equal(browser.html(), "");
        }
        [Test(Description = "Opens a webpage in a ASync way (i.e. the execution will continue strait away (call .waitForCompleted() if you need the page to load)")]
        public void open()
        {
            var browser = "webBrower".popupWindow_Hidden()
                                     .add_WebBrowser();
            assert_Is_Null  (browser.url());
            assert_Are_Equal(browser.html()     ,"");
            
            browser.open(url_O2Platform).waitForCompleted();

            assert_Not_Equal(browser.html()     ,"");
            assert_Are_Equal(browser.url().str(),url_O2Platform);
            assert_Contains (browser.html()     ,"O2 Platform");
        }
    }
}
