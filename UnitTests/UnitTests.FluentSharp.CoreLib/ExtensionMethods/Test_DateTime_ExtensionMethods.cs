using System;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods
{
    [TestFixture]
    public class Test_DateTime_ExtensionMethods
    {
        [Test(Description="Returns a string that represents the TimeSpan between the date provided and now")]
        public void duration_to_Now()
        {
            var startTime       = DateTime.Now;    
            10.sleep();
            var durationToNow1   = startTime.duration_To_Now();
            var durationToNow2   = startTime.AddSeconds(1  ).duration_To_Now();
            var durationToNow3   = startTime.AddSeconds(-1 ).duration_To_Now();
            var durationToNow4   = startTime.AddMinutes(-1 ).duration_To_Now();
            var durationToNow5   = startTime.AddMinutes(-100).duration_To_Now();
            var durationToNow6   = startTime.AddHours  (-1  ).duration_To_Now();

            durationToNow1.info();
            durationToNow2.info();
            durationToNow3.info();
            durationToNow4.info();
            durationToNow5.info();
            durationToNow6.info();
            Assert.IsTrue(durationToNow1.contains("00:00:00"));            
            Assert.IsTrue(durationToNow2.contains("-00:00:01") || durationToNow2.contains("-00:00:00"));
            Assert.IsTrue(durationToNow3.contains("00:00:01"));
            Assert.IsTrue(durationToNow4.contains("00:01:00"));
            Assert.IsTrue(durationToNow5.contains("01:40:00"));
            Assert.IsTrue(durationToNow6.contains("01:00:00"));

            Assert.IsNotNull((default(DateTime)).duration_To_Now());
        }

        [Test(Description="Returns a bool value if the provided string is a date")]
        public void isDate()
        {
            var value1    = DateTime.Now.str();
            var value2    = "12/12/12";
            var value3    = "2013";
            var value4    = "43";
            var value5    = "";
            var value6    = (null as string);                     
            Assert.AreEqual(value1.isDate(), true);
            Assert.AreEqual(value2.isDate(), true);
            Assert.AreEqual(value3.isDate(), false);
            Assert.AreEqual(value4.isDate(), false);
            Assert.AreEqual(value5.isDate(), false);
            Assert.AreEqual(value6.isDate(), false);            
        }

        [Test(Description="Gets the a Data object from an UTC value")]
        public void fromFileTimeUtc()
        {
            var date1     = DateTime.Now.ToUniversalTime();
            var utc1_A    = date1.toFileTimeUtc();
            var utc1_B    = (int)date1.toFileTimeUtc();
            var result1_A = utc1_A.fromFileTimeUtc();
            var result1_B = utc1_B.fromFileTimeUtc();

            Assert.Less       (0, utc1_A);
            Assert.AreNotEqual(0, utc1_B);
            Assert.AreEqual   (date1, result1_A);
            Assert.AreNotEqual(date1, result1_B);
            Assert.AreEqual   (date1, date1.toFileTimeUtc().fromFileTimeUtc());
            
            var date2    = 1000.fromFileTimeUtc();
            var result2  = date2.toFileTimeUtc(); 

            Assert.AreNotEqual(date2, default(DateTime));
            Assert.AreEqual   (1000, result2);            

            

            //handle bad data
            Assert.AreEqual(default(int)     , 1000.toDate().toFileTimeUtc());
            Assert.AreEqual(default(DateTime), (-1).fromFileTimeUtc());
        }

        [Test(Description="Gets an long that represents the provided DateTime UTC value (first converted ToUniversalTime)")]
        public void toFileTimeUtc()
        {
            //fromFileTimeUtc()
        }
        
        [Test(Description="Gets a Date Object from ticks or a date string")]
        public void toDate()
        {
            var value1    = DateTime.Now.str();
            var value2    = "12/12/12";
            var value3    = "2013";
            var value4    = "43";
            var value5    = "";
            var value6    = default(string);
            Assert.AreEqual(value1.toDate(), DateTime.Parse(value1));
            Assert.AreEqual(value2.toDate(), DateTime.Parse(value2));
            Assert.AreEqual(value3.toDate(), default(DateTime));
            Assert.AreEqual(value4.toDate(), default(DateTime));
            Assert.AreEqual(value5.toDate(), default(DateTime));
            Assert.AreEqual(value6.toDate(), default(DateTime));   
         
            var value7    = DateTime.Now.Ticks;
            var value8    = 10;
            var value9    = -10;
            var value10   = (long)10;
            var value11   = (long)-10;
            var value12   = default(int);
            var value13   = default(long);
            Assert.AreEqual(value7 .toDate(), new DateTime(value7));
            Assert.AreEqual(value8 .toDate(), new DateTime(value8));
            Assert.AreEqual(value9 .toDate(), default(DateTime));
            Assert.AreEqual(value10.toDate(), new DateTime(value10));
            Assert.AreEqual(value11.toDate(), default(DateTime));
            Assert.AreEqual(value12.toDate(), default(DateTime));            
            Assert.AreEqual(value13.toDate(), default(DateTime));            
        }
    
        [Test] public void jsDate()
        {
            var now     = DateTime.Now;
            var jsDate  = now     .jsDate();
            var date    = jsDate  .fromJsDate();
            var jsDate2 = date    .jsDate();            // round trip test
            var date2   = jsDate2 .fromJsDate();

            Assert.NotNull (jsDate);            
            Assert.AreEqual(jsDate   , jsDate2);
            Assert.AreEqual(date     , date2);
            Assert.AreEqual(now.ToUniversalTime().str(), date.str());            
            Assert.AreEqual(now.ToUniversalTime().str(), date2.str());
            Assert.AreEqual(now.ToUniversalTime().str(), date .ToUniversalTime().str());
            Assert.AreEqual(now.ToUniversalTime().str(), date2.ToUniversalTime().str());
            
            //check null and default value handling
            Assert.AreEqual(default(DateTime),(null as string)  .fromJsDate());
            Assert.AreEqual(""               , default(DateTime).toJsDate());
            Assert.AreEqual(String.Empty     , default(DateTime).toJsDate());

        }
    }
}
