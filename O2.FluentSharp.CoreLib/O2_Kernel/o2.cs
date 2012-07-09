using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace O2.Kernel
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
            return "4.1";
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
