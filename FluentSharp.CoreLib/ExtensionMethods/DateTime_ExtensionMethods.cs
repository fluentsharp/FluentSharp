using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSharp.CoreLib
{
    public static class DateTime_ExtensionMethods
    {
        public static string    duration_To_Now  (this DateTime startDateTime)  
		{
			return  (DateTime.Now -  startDateTime).timeSpan_ToString();
		}				
        public static bool      isDate           (this string dateString)       
		{
			return dateString.toDate() != default(DateTime);
		}		
        public static DateTime  fromFileTimeUtc  (this int fileTimeUtc)
        {
            try
            {
                return ((long)fileTimeUtc).fromFileTimeUtc();
            }
            catch(Exception ex)
            {
                ex.log("[fileTimeUtc(int)]");
                return default(DateTime);
            }
        }
		public static DateTime  fromFileTimeUtc  (this long fileTimeUtc)        
        {
            return DateTime.FromFileTimeUtc(fileTimeUtc);
        }		
        //todo: format result better
		public static string    timeSpan_ToString(this TimeSpan timeSpan)       
		{
            try
            {
                //var dateTime = timeSpan.Ticks.toDate();
                //return dateTime.ToShortTimeString();            // doesn't show the seconds
                //return timeSpan.ToString("mm'm 'ss's 'ff'ms'");  //4.0 dependent
                return timeSpan.ToString();     
            }
            catch(Exception ex)
            {
                ex.log("[TimeSpan][timeSpan_ToString]");
                return null;
            }			
		}   
        public static long      toFileTimeUtc    (this DateTime dateTime)       
        {
            try
            {
                return dateTime.ToUniversalTime().ToFileTimeUtc();
            }
            catch(Exception ex)
            {
                ex.log("[toFileTimeUtc(dateTime)]");
                return default(long);
            }
        }		        
        public static DateTime  toDate           (this string dateString)       
        {
            try
			{
				return DateTime.Parse(dateString);				
			}
			catch(Exception ex)
			{
                ex.log("[DateTime][toDate(string)]");
				return default(DateTime);
			}
        }
        public static DateTime  toDate           (this int    ticks)
        {
            return ((long)ticks).toDate();
        }
        public static DateTime  toDate            (this long   ticks)
        {
            try
			{
				return new DateTime(ticks);   
			}
			catch(Exception ex)
			{
                ex.log("[DateTime][toDate(int)]");
				return default(DateTime);
			}            
        }
        public static string    timeSpan_ToString(this DateTime startDateTime)  
		{
			return startDateTime.duration_To_Now();
		}		
        public static long      unixTime         (this DateTime dateTime)       
		{
			return (dateTime - new DateTime(1970, 1, 1)).TotalSeconds.toLong();
		}		
		public static long      unixTime_Now     (this int secondsToAdd)        
		{
			return DateTime.UtcNow.unixTime().add(secondsToAdd);
		}				
    }
}
