using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    
        public static string  jsDate(this DateTime date)
        {
            return date.toJsDate();
        }
        public static string  toJsDate(this DateTime date)
        {
            if (date == default(DateTime))
                return String.Empty;
            var dateTime_1970       = new DateTime(1970, 1, 1);
            var dateTime_Universal  = date.ToUniversalTime();
            var timeSpan            = new TimeSpan(dateTime_Universal.Ticks - dateTime_1970.Ticks);
            return timeSpan.TotalMilliseconds.ToString("#");
        }
        public static DateTime fromJsDate(this string date_Milliseconds_After_1970)
        {
            if (date_Milliseconds_After_1970.valid() && date_Milliseconds_After_1970.isDouble())
            {
                var dateTime = new DateTime(1970, 1, 1).ToUniversalTime();
                return dateTime.AddMilliseconds(date_Milliseconds_After_1970.toDouble());                
            }
            return default(DateTime);
        }
    }
    public static class Stopwatch_ExtensionMethods
    {
        public static TimeSpan stop(this Stopwatch stopwatch)
		{
			if(stopwatch.notNull())
				stopwatch.Stop();
			return stopwatch.Elapsed;
		}
		
		public static string milliseconds(this TimeSpan timeSpan)
		{
			if (timeSpan.notNull())
				return "{0}ms".format(timeSpan.Milliseconds);
			return null;
		}		
		public static string seconds(this TimeSpan timeSpan)
		{
			if (timeSpan.notNull())
				return "{0}s".format(timeSpan.Seconds);
			return null;
		}		
		public static string minutes(this TimeSpan timeSpan)
		{
			if (timeSpan.notNull())
				return "{0}m".format(timeSpan.Minutes);
			return null;
		}		
		public static string seconds_And_Miliseconds(this TimeSpan timeSpan)
		{
			if (timeSpan.notNull())
				return "{0}s {1}ms".format(timeSpan.Seconds, timeSpan.Milliseconds);
			return null;
		}
		public static string minutes_Seconds_And_Miliseconds(this TimeSpan timeSpan)
		{
			if (timeSpan.notNull())
				return "{0}m {0}s {1}ms".format(timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
			return null;
		}
    }
}
