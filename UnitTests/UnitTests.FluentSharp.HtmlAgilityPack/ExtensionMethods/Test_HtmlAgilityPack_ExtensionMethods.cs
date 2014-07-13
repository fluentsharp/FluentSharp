using FluentSharp.HtmlAgilityPacK;
using NUnit.Framework;

namespace UnitTests.FluentSharp.HtmlAgilityPacK
{
    [TestFixture]
    public class Test_HtmlAgilityPack_ExtensionMethods
    {
        public string MessyHtml      { get; set; }
        public string ExpectedResult { get; set; }

        public Test_HtmlAgilityPack_ExtensionMethods()
        {
            MessyHtml      = "<html><ul><li>an item</li></ul></html>";
            ExpectedResult = "<ul>\r\n    <li>an item</li>\r\n  </ul>";
        }
        [Test]
        public void tidyHtml_String()
        {            
            Assert.AreEqual(MessyHtml.tidyHtml(), ExpectedResult);
        }

        [Test]
        public void tidyHtml_HtmlDocument()
        {         
            var htmlDocument = MessyHtml.htmlDocument();

            Assert.NotNull(htmlDocument);
            Assert.NotNull(htmlDocument.tidyHtml(), ExpectedResult);

            Assert.IsNull((null as HtmlAgilityPack.HtmlDocument).tidyHtml(), ExpectedResult);           
        }
    }
}
