// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class Thread_Invoke_ExtensionMethods
    {        
        private static T    invokeThread<T>(Func<T> codeToInvoke)
        { 
            try
            {
                return codeToInvoke();
            }
            catch (Exception ex)
            {
                ex.log("[in invokeThread<T>]");
                return default(T);
            }                                                 
        }
		public static T     invokeOnThread<T>(this Control control, Func<T> codeToInvoke)
		{
			return control.invokeOnThread(-1, codeToInvoke);
		}
		public static T     invokeOnThread<T>(this Control control,int maxInvokeWait, Func<T> codeToInvoke)
        {            
            try
            {
                if (control.isNull() || control.IsDisposed)
                    return default(T);
                if (control.InvokeRequired)
                {
                    T returnData = default(T);
                    var sync = new AutoResetEvent(false);
                    if (control.IsDisposed)
                        return default(T);                    
                    control.Invoke(new EventHandler((sender, e) =>
                        {                            
                            if (control.IsDisposed.isFalse())
                                returnData = invokeThread(codeToInvoke);
                            sync.Set();
                        }));
					if (maxInvokeWait == -1)
						sync.WaitOne();
					else
						if (sync.WaitOne(maxInvokeWait).isFalse())
						{
							"[in invokeOnThread] Func<T> codeToInvoke took more than {0}s to execute".error(maxInvokeWait);
							return default(T);
						}                                        
                    return returnData;
                }
                return codeToInvoke();
            }
            catch (System.ComponentModel.InvalidAsynchronousStateException)
            { 
                //ignore these
            }
            catch (Exception ex)
            {
                ex.log("[in invokeOnThread<T>]");                
            }
            return default(T);
        }        
        private static void invokeThread(Action codeToInvoke)
        {
            try
            {
                codeToInvoke();
            }
            catch (Exception ex)
            {
                ex.log("[invokeThread]");                
            }
        }
        public static void  invokeOnThread(this Control control, Action codeToInvoke)
        {
            if (control.isNull() || control.IsDisposed)
            {
                //"Control.invokeOnThread, provided control value was null or dispose(so not invoking code)".error();
                return;
            }
            try
            {
                if (control.InvokeRequired)
                {                    
                    control.Invoke(new EventHandler((sender, e)=>
                        {
                            if (control.IsDisposed)
                                return;
                            invokeThread(codeToInvoke);
                        }));
                }
                else
                    codeToInvoke();
            }

            catch (Exception ex)
            {
                ex.log("[invokeOnThread]");                
            }
        }
		public static void  invokeOnThread(this ToolStrip toolStrip, Action codeToInvoke)
		{
			try
			{
				if (toolStrip.InvokeRequired)
                    toolStrip.Invoke(new EventHandler((sender, e) =>
                        {
                            invokeThread(codeToInvoke); 
                        }));
				else
					codeToInvoke();
			}

			catch (Exception ex)
			{
                ex.log("[in invokeOnThread]");
			}
		}
        public static void  okThreadSync(this Control control, EventHandler target)
        {
            if (control != null && false == control.IsDisposed)
                try
                {
                    if (control.InvokeRequired)
                    {
                        IAsyncResult asyncResult = control.BeginInvoke(target);
                        asyncResult.AsyncWaitHandle.WaitOne();
                    }
                    else
                    {
                        target.Invoke(null, null);
                    }

                }
                catch (Exception ex)
                {
                    ex.log("[in okThreadSync]");                    
                }
            //return null;
        }        
        public static bool  okThread(this Control control, EventHandler target)            
        {
            if (control == null || control.IsDisposed)
            {     
                // can't send this message here since we create a stack overflow when this happens on the DebugMsg okThread
                //PublicDI.log.info("in okThread control variable was null or disposed, so returning false");
                return false;
            }
            try
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new EventHandler(target));
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.log("[in okThread]");
                return false;
            }
        }
        public static bool  okThread(this Form form, EventHandler target)            
        {
            if (form == null || form.IsDisposed)
            {                
                return false;
            }
            try
            {
                if (form.InvokeRequired)
                {
                    form.Invoke(new EventHandler(target));
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.log("[in okThread]");
                return false;
            }
        }
        public static T     onThread<T>(this T control, Action action) where T : Control
        {
            return control.update((c) => action());
        }
        public static T     onThread<T>(this T control, Action<T> action) where T : Control
        {
            return (T)control.invokeOnThread(
                    () =>
                    {
                        action(control);
                        return control;
                    });
        }
        public static T     update<T>(this T control, Action updateAction) where T : Control
        {
            return control.update((c) => updateAction());
        }
        public static T     update<T>(this T control, Action<T> updateAction) where T : Control
        {
            return (T)control.invokeOnThread(
                        () =>
                        {
                            control.refresh_Disable();
                            updateAction(control);
                            control.refresh_Enable();
                            return control;
                        });
        }
    }
}