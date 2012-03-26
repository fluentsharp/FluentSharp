// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Threading;
using System;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.DotNetWrappers.DotNet
{
    public class O2Thread
    {
        #region Delegates

        public delegate Thread FuncThread(); // they forgot to include this one :)
        public delegate void FuncVoid(); // they forgot to include this one :)
        public delegate void FuncVoidT1<T1>(T1 arg1);
        public delegate void FuncVoidT1T2<T1,T2>(T1 arg1,T2 arg2);
        public delegate void FuncVoidT1T2T3<T1, T2,T3>(T1 arg1, T2 arg2, T3 arg3);
        public delegate void FuncVoidT1T2T3T4<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        #endregion

        //var threadEnded = new AutoResetEvent(false);

        /*public static void staThreadSync(FuncVoid codeToExecute)
        {            
            var thread = staThread(codeToExecute);
            thread.Join();
            if (thread.IsAlive)
            {
            }
        }*/

        public static Thread staThread(FuncVoid codeToExecute)
        {
            return staThread(codeToExecute, ThreadPriority.Normal);
        }

        public static Thread staThread(FuncVoid codeToExecute, ThreadPriority threadPriority)            
        {
            var stackTrace = getCurrentStackTrace();    // used for cross thread debugging purposes
            var staThread = new Thread(()=>{
                                                try 
	                                            {	        
                                                    codeToExecute();
	                                            }
	                                            catch (Exception ex)
	                                            {
		                                            ex.log("in staThread");
                                                }
	                                        });
   
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Priority = threadPriority;
            staThread.Start();            
            return staThread;
        }

       /* public static void mtaThreadSync(FuncVoid codeToExecute)
        {
            var thread = mtaThread(codeToExecute);            
            thread.Join();
            if (thread.IsAlive)
            {
            }
        }*/

        public static Thread mtaThread(FuncVoid codeToExecute)
        {
            return mtaThread("[O2 Mta Thread]", codeToExecute);
        }

        public static Thread mtaThread(FuncVoid codeToExecute, ThreadPriority threadPriority)
        {
            return mtaThread("[O2 Mta Thread]", codeToExecute, threadPriority);
        }

        public static Thread mtaThread(string threadName, FuncVoid codeToExecute)
        {
            return mtaThread(threadName, codeToExecute, ThreadPriority.Normal);
        }

        public static Thread mtaThread(string threadName, FuncVoid codeToExecute, ThreadPriority threadPriority)
        {
            var stackTrace = getCurrentStackTrace();    // used for cross thread debugging purposes
            var mtaThread = new Thread(() =>
                                        {
                                            try
                                            {
                                                codeToExecute();
                                            }
                                            catch (Exception ex)
                                            {
                                                DI.log.ex(ex,"in mtaThread", true);
                                            }
                                        })
                                        // Thread() contructor
                                        {
                                            Name = threadName
                                        };
            mtaThread.SetApartmentState(ApartmentState.MTA);
            mtaThread.Priority = threadPriority;
            mtaThread.Start();
            return mtaThread;
        }

        public static Thread mtaThread(Semaphore semaphore, FuncVoid codeToExecute)
        {
            var stackTrace = getCurrentStackTrace(); // used for cross thread debugging purposes
            if (semaphore == null)
                return mtaThread(codeToExecute);
            // if no use the mtaThread function with no semaphore support

            var _mtaThread = new Thread(() =>
                                            {
                                                semaphore.WaitOne();
                                                codeToExecute();
                                                semaphore.Release();
                                            });
            _mtaThread.SetApartmentState(ApartmentState.MTA);
            _mtaThread.Start();
            return _mtaThread;

        }

        public static string getCurrentStackTrace()
        {
            return "";      // enable the line below when needed
            //return new System.Diagnostics.StackTrace().ToString();
        }

        public static void setPriority_Lowest()
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;
        }

        public static void setPriority_BelowNormal()
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.BelowNormal;
        }

        public static void setPriority_Normal()
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Normal;
        }

        public static void setPriority_AboveNormal()
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.AboveNormal;
        }

        public static void setPriority_Highest()
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
        }               
    }
}
