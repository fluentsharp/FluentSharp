using ICSharpCode.NRefactory;

namespace FluentSharp.CSharpAST.Utils
{
    public class AstValue <T>
    {
        public string Text {get;set;}
        public T OriginalObject {get; set;}
        public Location StartLocation  {get; set;}
        public Location EndLocation  {get; set;}
		
        public AstValue(string text,  T originalObject  , Location startLocation , Location endLocation)
        {
            Text = text;
            OriginalObject= originalObject;
            StartLocation = startLocation;
            EndLocation = endLocation;
        }
    }
}