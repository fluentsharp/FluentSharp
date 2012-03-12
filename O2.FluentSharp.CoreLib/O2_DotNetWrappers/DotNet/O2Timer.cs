// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;

namespace O2.DotNetWrappers.DotNet
{
    public class O2Timer
    {
        private String Description { get; set; }
        private DateTime StartDateTime { get; set; }
        public string TimeSpanString { get; set; }

        public O2Timer() // automatically start when we don't provide a description
        {
            TimeSpanString = "";
            start();
        }

        public O2Timer(String _description)
        {
            Description = _description;
        }

        public O2Timer start()
        {
            StartDateTime = DateTime.Now;
            return this; // do this so that we can create this object and start it one line
        }

        public O2Timer start(String _description)
        {
            Description = _description;
            return start();
        }

        public O2Timer stop()
        {
            return stop(true);
        }

        public O2Timer stop(bool showValueOnLog)
        {
            TimeSpan tsTime = DateTime.Now - StartDateTime;
            TimeSpanString = getTimeSpanString(tsTime);
            //   if (description == "")
            //       return timeSpanString;
            if (showValueOnLog)
                DI.log.debug(getTimeSpanString(tsTime));
            return this;
            //return timeSpanString;
        }

        public string getTimeSpanString(TimeSpan tsTime)
        {
            if (Description != "" && Description.Contains(" in ") == false)
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
}
