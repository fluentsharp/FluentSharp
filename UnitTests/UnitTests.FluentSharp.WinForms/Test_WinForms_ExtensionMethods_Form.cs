using System.Drawing;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp_WinForms
{
    [TestFixture]
    public class Test_WinForms_ExtensionMethods_Form : NUnitTests
    {
        [Ignore]
        [Test(Description = "Returns true is the provided form window state is 'Normal' (related to isMinimized and isMaximized)")]
        public void isNormal()
        {
            var form = "test Form".popupWindow().parentForm();            

            assert_Is_True (form.isNormal());
            assert_Is_False(form.isMinimized());
            assert_Is_False(form.isMaximized());

            form.minimize();

            assert_Is_False(form.isNormal());
            assert_Is_True(form.isMinimized());
            assert_Is_False(form.isMaximized());
            
            form.maximize();

            assert_Is_False(form.isNormal());
            assert_Is_False(form.isMinimized());
            assert_Is_True(form.isMaximized());

            form.close();

            assert_Is_False(form.isNormal());
            assert_Is_False(form.isMinimized());
            assert_Is_False(form.isMaximized());
        }
       
        [Test(Description = "Returns a Image stored in the FluentSharp.WinForms assembly as a resource")]
        public void formImage()
        {
            var image   = "folder".formImage();
            var bitmap = image.asBitmap();
            var iconFromImage = image.asIcon();
            var iconFromBitmap = bitmap.asIcon();

            Assert.NotNull(image);
            Assert.NotNull(bitmap);
            Assert.NotNull(iconFromImage);
            Assert.NotNull(iconFromBitmap);

            Assert.IsInstanceOf<Image >(image);
            Assert.IsInstanceOf<Bitmap>(bitmap);
            Assert.IsInstanceOf<Icon>  (iconFromImage);
            Assert.IsInstanceOf<Icon>  (iconFromBitmap);
        }
        [Test(Description = "Opens a new Windows Form with the title and size provided")]
        public void popupWindow()
        {
            var title = "popup test";            
            var popupWindow = title.popupWindow();
            var parentForm = popupWindow.parentForm(); 
            Assert.IsNotNull(popupWindow);
            Assert.AreEqual (parentForm.get_Text(),title);
            Assert.IsInstanceOf<Panel>(popupWindow);
            Assert.IsInstanceOf<Form>(parentForm);            
            parentForm.close();                        
        }
    
        [Test(Description = "Created a popupWindow that is hidden")]
        public void popupWindow_Hidden()
        {            
            var popupWindow = "test".popupWindow_Hidden();
            assert_Not_Null (popupWindow);
            assert_Is_True  (popupWindow.isVisible());                        
            assert_Is_True  (popupWindow.parentForm().isVisible());
            assert_Is_True  (popupWindow.parentForm().opacity_Zero());
            assert_Are_Equal(popupWindow.parentForm().opacity(), 0.0);
            assert_Are_Equal(popupWindow.form_Opacity(), 0.0);

            //check that we can add a text area ok
            assert_Is_Null (popupWindow.controls<TextBox>());             // make sure there isn't one there

            var textBox = popupWindow.add_TextArea();                     // add TextArea
            
            assert_Not_Null (textBox);
            assert_Are_Equal(textBox,popupWindow.controls<TextBox>());    // check that it now exists
            assert_Are_Equal(textBox,popupWindow.controls().first());    
            assert_Are_Equal(textBox.get_Text(),"");            
            assert_Are_Equal(textBox.set_Text("123"),textBox);
            assert_Are_Equal(textBox.get_Text(),"123");
            assert_Is_True  (textBox.isVisible());
        }
    
        [Test(Description="Makes the form visible (and sets opacity to 1 if opacity was set to 0")]
        public void show()
        {
            var topPanel = "opacity".popupWindow_Hidden();
            var form     = topPanel.parentForm();

            assert_Are_Equal(topPanel.form_Opacity(),0.0);
            assert_Are_Equal(form.form_Opacity()    ,0.0);
            assert_Are_Equal(form.opacity()         ,0.0);
            assert_Are_Equal(topPanel.isVisible()   , true);
            assert_Are_Equal(form.isVisible()       , true);
            
            topPanel.show();     // calling show on the panel should not change the opacity

            assert_Are_Equal(topPanel.form_Opacity(),0.0);
            assert_Are_Equal(topPanel.isVisible(), true);
            
            form.show();        // the opacity is only changed in the form's .show extension method

            assert_Are_Equal(topPanel.form_Opacity(),1.0);
            assert_Are_Equal(topPanel.isVisible(), true);

            form.hide();       // hide should only affect the opacity value (calling visible=false was trigering the o2Gui.Dispose() call)
            
            assert_Are_Equal(topPanel.form_Opacity(),0.0);
            assert_Are_Equal(topPanel.isVisible(), true);

            form.show();        // confirming that .show() resets the opacity value to 1.0

            assert_Are_Equal(topPanel.form_Opacity(),1.0);
            assert_Are_Equal(topPanel.isVisible(), true);

            form.close();
        }

        //Workflows
        [Test]
        public void Add_WebBrowser_To_Hidden_PopupWindow()
        {            
            var popupWindow = "webBrowser".popupWindow_Hidden();                     
            var browser = popupWindow.add_Control<WebBrowser>();
            assert_Not_Null(popupWindow);
            assert_Not_Null(browser);
            assert_Are_Equal(browser.html(), "");
            browser.showMessage("abc");
            assert_Is_True(browser.html().contains("abc"));
        }
    }
}
