using System;
using System.Windows.Threading;
using FluentSharp.CoreLib;

//O2Ref:WindowsBase.dll
//O2Ref:System.Core.dll

namespace FluentSharp.WPF
{
    public static class Threading_ExtensionMethods
    {    
    	public static TRet wpfInvoke<T,TRet>(this T source, Func<TRet> func) where T:DispatcherObject
    	{            
            //wrap func with a try catch
            Func<TRet> funcWithCatch = 
                ()=>{
                        try
                        { 
                            return func();
                        }
                        catch (Exception ex)
                        {
                            ex.log("in wpfInvoke");
                            return default(TRet);		
                        }
                    };

    		try
			{
                if (source.Dispatcher.CheckAccess())
                    return funcWithCatch();
                return (TRet)source.Dispatcher.Invoke(funcWithCatch, DispatcherPriority.Normal);
        	}			
            catch (Exception ex)
            {
                ex.log("in wpfInvoke");
                return default(TRet);			
            }
        	
    	}
    	
    	public static void wpfInvoke<T>(this T source, Action action) where T:DispatcherObject 
    	{             
            //wrap func with a try catch
            Action actionWithCatch = 
                ()=>{
                        try
                        { 
                            action();
                        }
                        catch (Exception ex)
                        {
                            ex.log("in wpfInvoke");                            
                        }
                    };
    		try
			{
                if (source.Dispatcher.CheckAccess())
                    actionWithCatch();
                else
                    source.Dispatcher.Invoke(actionWithCatch, DispatcherPriority.Normal);
        	}			
            catch (Exception ex)
            {
                ex.log("in wpfInvoke");
            }
    	}    	    	    	    	   
    }
}