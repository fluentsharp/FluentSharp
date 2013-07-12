// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System.Diagnostics;
using System.Threading;
using System;

using System.Collections.Generic;


namespace FluentSharp.CoreLib.API
{
    public class O2Thread
    {
        public static List<Thread> ThreadsCreated { get; set; }

        static O2Thread()
        {
            ThreadsCreated = new List<Thread>();
        }

        
        // ReSharper disable TypeParameterCanBeVariant
        /*public delegate Thread FuncThread(); 
        public delegate void FuncVoidT1<T1>(T1 arg1);
        public delegate void FuncVoidT1T2<T1,T2>(T1 arg1,T2 arg2);
        public delegate void FuncVoidT1T2T3<T1, T2,T3>(T1 arg1, T2 arg2, T3 arg3);
        public delegate void FuncVoidT1T2T3T4<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);*/
        // ReSharper restore TypeParameterCanBeVariant
        

        public static Thread staThread(Action codeToExecute)
        {
            var stackTrace = getCurrentStackTrace();                
            return staThread(stackTrace.str(),ThreadPriority.Normal, codeToExecute);
        }
        public static Thread staThread(string threadName_StackTrace, ThreadPriority threadPriority, Action codeToExecute)            
        {
            //var stackTrace = getCurrentStackTrace();    // used for cross thread debugging purposes
            var staThread = new Thread(()=>{
                                                try 
	                                            {	        
                                                    codeToExecute();
	                                            }
	                                            catch (Exception ex)
	                                            {
		                                            ex.log("in staThread");
                                                }
	                                        }) 
                                                {Name = "[O2 Sta Thread][StackTrace_OnStart]: " + threadName_StackTrace};
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Priority = threadPriority;
            staThread.Start();            
            ThreadsCreated.add(staThread);            
            return staThread;
        }

        public static Thread mtaThread(Action codeToExecute)
        {
            var stackTrace = getCurrentStackTrace();                
            return mtaThread( stackTrace.str(), ThreadPriority.Normal, codeToExecute);
        }        
        public static Thread mtaThread(string threadName_StackTrace, Action codeToExecute)
        {
            return mtaThread(threadName_StackTrace, ThreadPriority.Normal, codeToExecute);
        }
        public static Thread mtaThread(string threadName_StackTrace,ThreadPriority threadPriority,  Action codeToExecute )
        {            
            var mtaThread = new Thread(()=>{
                                                try
                                                {
                                                    codeToExecute();
                                                }
                                                catch (Exception ex)
                                                {
                                                    PublicDI.log.ex(ex,"in mtaThread", true);
                                                }
                                            }) 
                                                {Name = "[Mta Thread][StackTrace_OnStart]: " + threadName_StackTrace};
            mtaThread.SetApartmentState(ApartmentState.MTA);
            mtaThread.Priority = threadPriority;
            mtaThread.Start();
            ThreadsCreated.add(mtaThread);         
            return mtaThread;
        }

        public static StackTrace getCurrentStackTrace()
        {
            try
            {
                return new StackTrace(2, true);
            }
            catch (Exception ex)
            {
                ex.log("[in getCurrentStackTrace");
                return null;
            }                        
        }
        public static void setPriority_Lowest()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        }
        public static void setPriority_BelowNormal()
        {
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
        }
        public static void setPriority_Normal()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Normal;
        }
        public static void setPriority_AboveNormal()
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }
        public static void setPriority_Highest()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }               

        //Legacy (not used in FluentSharp at the moment)
        public static Thread mtaThread(Semaphore semaphore, Action codeToExecute)
        {
            //var stackTrace = getCurrentStackTrace(); // used for cross thread debugging purposes
            if (semaphore == null)
                return mtaThread(codeToExecute);
            // if no use the mtaThread function with no semaphore support

            var thread = new Thread(()=>{
                                            semaphore.WaitOne();
                                            codeToExecute();
                                            semaphore.Release();
                                        });
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
            ThreadsCreated.add(thread);
            return thread;

        }

    }
}
