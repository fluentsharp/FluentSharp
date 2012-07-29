// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using System.Diagnostics;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Threads_ExtensionMethods
    {
        /// <summary>
        /// Sync execution of code on the the Control thread
        /// </summary>
        /// <param name="control"></param>
        /// <param name="codeToInvoke"></param>
        /// <returns></returns>
        /// 
        private static T invokeThread<T>(Func<T> codeToInvoke)
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
        public static T invokeOnThread<T>(this Control control, Func<T> codeToInvoke)
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
                    sync.WaitOne();
                    /*if (sync.WaitOne(20000).isFalse())
                    {
                        "[in invokeOnThread] Func<T> codeToInvoke took more than 2s to execute"
                        return default(T);
                    } */
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

        /// <summary>
        /// ASync execution of code on the the Control thread unless we are on the correct thread
        /// and the execution will be sync
        /// </summary>
        /// 
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
        public static void invokeOnThread(this Control control, Action codeToInvoke)
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

		public static void invokeOnThread(this ToolStrip toolStrip, Action codeToInvoke)
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
        /// <summary>
        /// invokes target if control.InvokeRequired 
        /// Although if control.InvokeRequired the exection
        /// </summary>
        /// <param name="control"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static void okThreadSync(this Control control, EventHandler target)
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

        /// <summary>
        /// invokes target if control.InvokeRequired 
        /// return value is just a confirmation of not if the execution was sucessfull
        /// _note that the target code will be executed that's controls thread and execution will continue on the original thread
        ///(use okThreadSync to wait for execution of target)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool okThread(this Control control, EventHandler target)
            // Control liveObject, string methodToInvoke, object[] delegateParams)
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

        public static bool okThread(this Form form, EventHandler target)
            // Control liveObject, string methodToInvoke, object[] delegateParams)
        {
            if (form == null || form.IsDisposed)
            {
                //  PublicDI.log.info("in okThread form variable was null or dispose, so returning false");
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

        public static T onThread<T>(this T control, Action action) where T : Control
        {
            return control.update((c) => action());
        }
        public static T onThread<T>(this T control, Action<T> action) where T : Control
        {
            return (T)control.invokeOnThread(
                    () =>
                    {
                        action(control);
                        return control;
                    });
        }
        public static T update<T>(this T control, Action updateAction) where T : Control
        {
            return control.update((c) => updateAction());
        }
        public static T update<T>(this T control, Action<T> updateAction) where T : Control
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