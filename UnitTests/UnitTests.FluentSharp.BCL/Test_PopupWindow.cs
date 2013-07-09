using System.Windows.Forms;
using FluentSharp.BCL;
using NUnit.Framework;
using FluentSharp.CoreLib;

namespace UnitTests.FluentSharp_BCL
{
    [TestFixture]
    public class Test_PopupWindow
    {
        [Test]
        public void PopupWindow()
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

        [Test]
        public void TextArea()
        {
            var text = "Some text";
            var textArea = "textarea".popupWindow().add_TextArea();
            Assert.IsNotNull(textArea);
            textArea.set_Text(text);
            Assert.AreEqual(text,textArea.get_Text());            
            textArea.closeForm();
        }
    }
}
