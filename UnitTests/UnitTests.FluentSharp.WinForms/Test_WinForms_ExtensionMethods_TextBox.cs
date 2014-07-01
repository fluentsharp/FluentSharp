using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp_WinForms
{
    [TestFixture]
    public class Test_WinForms_ExtensionMethods_TextBox
    {
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
