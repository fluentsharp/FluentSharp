using ICSharpCode.NRefactory;

namespace O2.External.SharpDevelop.AST
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