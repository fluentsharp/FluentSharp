// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;

namespace O2.Views.ASCX.SourceCodeEdit.ScriptSamples
{
    public class For_UnitTest_HotMethodPatch
    {
        public static int counterB;

        public static void Main()
        {
            Console.WriteLine("This is a Embeded in O2 C# file that contains a nice multi-thread loop " +
                              "which we can use to test HOT patching of functions using the O2MDbg");
            Console.WriteLine("\n\nPress Enter to end process");
            try
            {
                Console.WriteLine("Starting Thread A");
                //    new Thread(methodRunningInThreadA).Start();
                Console.WriteLine("Starting Thread B");
                new Thread(methodRunningInThreadB).Start();
            }
            catch (Exception)
            {
            }
            Console.ReadLine();
        }

        public static void methodRunningInThreadA()
        {
            int counterA = 0;
            while (true)
            {
                Console.WriteLine("Thread A - every 2 sec ping #" + counterA++);
                Thread.Sleep(2000);
            }
        }

        public static void methodRunningInThreadB()
        {
            while (true)
            {
                Console.WriteLine(messageForThreadB());
                Thread.Sleep(1000);
            }
        }

        public static string messageForThreadB()
        {
            Console.WriteLine("In messageForThreadB");

            string threadBText = "T";
            threadBText += "h";
            threadBText += "r";
            threadBText += "e";
            threadBText += "a";
            threadBText += "d";
            threadBText += "B";

            string message = string.Format("[{0}:{1}] {2}", DateTime.Now.Second, DateTime.Now.Millisecond, threadBText);
            return addCounterToThreadB(message);
        }

        public static string addCounterToThreadB(string textToProcess)
        {
            string message = string.Format("{0} - every 1 sec ping #{1} ..v1.1 ", textToProcess, counterB++);
            return message;
        }

        public static string newMessageForThreadB()
        {
            Console.WriteLine("In newMessageForThreadB");
            string message = string.Format("[{0}:{1}] This is a new message for Thread B", DateTime.Now.Second,
                                           DateTime.Now.Millisecond);
            return message;
        }
    }
}
