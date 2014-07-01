using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using FluentSharp.WinForms;
using FluentSharp.WinForms.Controls;
using NUnit.Framework;

namespace UnitTests.FluentSharp_WinForms
{
    [TestFixture]
    public class Test_WinForms_Show : NUnitTests
    {
        [Test(Description="Creates a new O2Gui (which is an Windows Form) object, with the provided controlType as its child object")]
        public void showAscxInForm()
        {
            //Note: this code is pre the use of Extension methods in FluentSharp (these days this object is created using the .popupWindow method)
            var title       = "Test_WinForms_Show".add_RandomLetters(10);
            var width       = 300 + 50.random(); 
            var height      = 300 + 50.random();
            var startHidden = false;            
            var controlType = typeof(Panel); 

            var control    = WinForms_Show.showAscxInForm(controlType, title, width, height, startHidden);
            var parentForm = control.parentForm();
            assert_Not_Null( control);            
            assert_Is_True  (control.isVisible());            
            assert_Is_True  (control.isEnabled());   
            assert_Is_True  (control.type().baseTypes(true).contains(typeof(Panel)));            
            assert_Are_Equal(control.width() , width);
            assert_Are_Equal(control.height(), height);

            assert_Is_Type    <Panel>(control);
            assert_Is_Not_Type<Form >(control);
            
            assert_Not_Null (parentForm);
            assert_Is_True  (parentForm.isVisible()); 
            assert_Is_True  (parentForm.isEnabled());   
            assert_Is_True  (parentForm.type().baseTypes(true).contains(typeof(Form)));                        
            assert_Is_False (parentForm.type().baseTypes(true).contains(typeof(Panel)));            
            assert_Is_Bigger(parentForm.width() , width);
            assert_Is_Bigger(parentForm.height(), height);
            assert_Are_Equal(parentForm.title() , title);            
            
        }

        [Test(Description="When startHidden is set to false the O2Gui/Form is set with opacity=0 and ShowInTaskbar = false")]
        public void showAscxInForm__startHidden_true()
        {
            var title       = "showAscxInForm__startHidden_False".add_RandomLetters(10);
            var width       = 300 + 50.random(); 
            var height      = 300 + 50.random();
            var startHidden = true;            
            var controlType = typeof(Panel); 

            var topPanel    = WinForms_Show.showAscxInForm(controlType, title, width, height, startHidden);

            var parentForm = topPanel.parentForm();
            assert_Not_Null (topPanel);       
            assert_Not_Null (parentForm);
            assert_Are_Equal(parentForm.title(), title);
            assert_Are_Equal(parentForm.opacity(), 0);
            assert_Is_True  (topPanel  .isVisible());            
            assert_Is_True  (topPanel  .isEnabled());  
            assert_Is_True  (parentForm.isVisible());                        
            assert_Is_True  (parentForm.isEnabled());  
                                    
            //add a textarea to confirm events are still happening
            
            var textArea1 = topPanel.add_TextArea().set_Text("123");
            assert_Not_Null (textArea1);    
            assert_Is_True  (textArea1.isVisible());                        
            assert_Are_Equal(textArea1.get_Text(), "123");

            //add another text area (to double check
            var textArea2 = topPanel.insert_Right().add_TextArea().set_Text("abc");
            assert_Not_Null (textArea2);    
            assert_Is_True  (textArea2.isVisible());                        
            assert_Are_Equal(textArea2.get_Text(), "abc");                        
        }

        //Workflows

        [Test]
        public void Open_File_Contents_On_An_Hidden_Form()
        {
            var title       = "Open_File_Contents_On_An_Hidden_Form".add_RandomLetters(10);
            var width       = 300 + 50.random(); 
            var height      = 300 + 50.random();
            var startHidden = false;            
            
            var topPanel    = WinForms_Show.showAscxInForm( typeof(Panel), title, width, height, startHidden);
            var textArea    = topPanel.add_TextArea();
            textArea.assert_Not_Null()
                    .assert_Is_Instance_Of<TextBox>();
                        
            var tmpFile = 200.randomLetters().saveAs("tempFile".tempFile());
            textArea.set_Text(tmpFile.fileContents());

            assert_Are_Equal(textArea.get_Text(), tmpFile.fileContents());

            tmpFile.file_Delete();
            assert_Is_False(tmpFile.fileExists());
        }
    }
}
