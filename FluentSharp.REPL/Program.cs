using FluentSharp.CoreLib;

namespace FluentSharp.REPL.Utils
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ResolveO2CoreLib.resolve();
            start(args);
        }

        //we need to do this on a separate method so that the JIT doesn't compile this before Main execution
        public static void start(string[] args)
        {                     
            //if (args.size() > 0)
            //    O2Launch.o2Gui(args);
            //else
            open.csharpREPL();
            
        }
    }   
}
