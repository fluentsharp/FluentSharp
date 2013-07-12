namespace FluentSharp.CoreLib.API
{
    public class o2
    {
        public static int theAnswer()
        {
            return 42;
        }

        public static string hello()
        {
            return "Ola";
        }

        public static string version()
        {             
            return O2ConfigSettings.O2Version;
        } 
        
        public static string test()
        {
        	return "123456";
        }
        
        public static string say(string something)
        {
        	return something;
        }


       
    }
}
