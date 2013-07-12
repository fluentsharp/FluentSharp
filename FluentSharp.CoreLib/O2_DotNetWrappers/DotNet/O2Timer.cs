// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;



namespace FluentSharp.CoreLib.API
{
    public class O2Timer
    {
        public String Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan timeSpan { get; set; }
        public string TimeSpanString { get; set; }

        public O2Timer() // automatically start when we don't provide a description
        {
            TimeSpanString = "";
            start();
        }

        public O2Timer(String description)
        {
            Description = description;
        }

        public O2Timer(String description, Action actionToTime)
        {
            Description = description;
            start();
            actionToTime();
            stop();
        }                

        public O2Timer start()
        {
            StartDateTime = DateTime.Now;
            return this; // do this so that we can create this object and start it one line
        }

        public O2Timer start(String description)
        {
            Description = description;
            return start();
        }

        public O2Timer stop()
        {
            return stop(true);
        }

        public O2Timer stop(bool showValueOnLog)
        {
            timeSpan = DateTime.Now - StartDateTime;
            TimeSpanString = getTimeSpanString(timeSpan);
            //   if (description == "")
            //       return timeSpanString;
            if (showValueOnLog)
                PublicDI.log.debug(getTimeSpanString(timeSpan));
            return this;
            //return timeSpanString;
        }

        public string getTimeSpanString(TimeSpan tsTime)
        {
            if (Description.notValid())
                Description = "...";
            if (Description.Contains(" in ") == false)
                Description += " in ";
            if (tsTime.Hours > 0)
                return String.Format("{0}{1}h:{2}m:{3}s:{4}ms", Description, tsTime.Hours, tsTime.Minutes,
                                     tsTime.Seconds, tsTime.Milliseconds);
            if (tsTime.Minutes > 0)
                return String.Format("{0}{1}m:{2}s:{3}ms", Description, tsTime.Minutes, tsTime.Seconds,
                                     tsTime.Milliseconds);
            return String.Format("{0}{1}s:{2}ms", Description, tsTime.Seconds, tsTime.Milliseconds);
        }
        
		public override string ToString()
		{
			return this.TimeSpanString;
		}
    }

    public static class O2Timer_ExtensionMehods
    {
        public static string infoTimeSpan(this Action actionToTime)
        {
            return actionToTime.infoTimeSpan("Action");
        }
        public static string infoTimeSpan(this Action actionToTime, string description)
        {
            return new O2Timer(description, actionToTime).TimeSpanString.info();            
        }
        public static string infoTimeSpan(this string description, Action actionToTime)
        { 
            return new O2Timer(description, actionToTime).TimeSpanString.info();            
        }
    }
}
