using System;
using System.Reflection;
using FluentSharp.CoreLib;
using FluentSharp.Git.Utils;
using NUnit.Framework;
using Sharpen;

namespace UnitTests.FluentSharp_NGit
{
    public class Test_NGit_Classes
    {
        //GitProgress
        [Test]
        public void GitProgress()
        {
            var gitProcess = new GitProgress();
            Assert.IsTrue  (gitProcess.Log_BeginTask);
            Assert.IsFalse (gitProcess.Log_Start);
            Assert.IsFalse (gitProcess.Log_Update);
            Assert.IsFalse (gitProcess.Log_EndTask);
            Assert.IsFalse (gitProcess.Cancel);
            Assert.AreEqual("",gitProcess.FullMessage.str());

            var beginTask_Text      = "AAAA";
            var beginTask_Expected  = "[GitProgress] BeginTask : {0} : 0".format(beginTask_Text);
            var noExtraLog_Expected = "\rAAAA:                   0\rAAAA:                   0";
            var start_expected      = "[GitProgress] Start :  : 10";
            var update_expected     = "[GitProgress] Update :  : 1";
            var endTask_expected    = "[GitProgress] EndTask :  : -1";

            Action<int, string> checkFullMessage =
                (lineCount, lastMessage) =>
                    {
                        var lines = gitProcess.FullMessage.str().fixCRLF().lines();
                        Assert.AreEqual(lines.size(), lineCount);
                        Assert.AreEqual(lines.last(), lastMessage);
                    };

            gitProcess.BeginTask (beginTask_Text, 0);
            checkFullMessage(1, beginTask_Expected);
            gitProcess.Start(0);
            gitProcess.Update(0);   // will add an small entry to  gitProcess.FullMessage
            gitProcess.EndTask();   // will add an small entry to  gitProcess.FullMessage
            checkFullMessage(2, noExtraLog_Expected);

            gitProcess.Log_Start = true;
            gitProcess.Start(10);
            checkFullMessage(3, start_expected);

            gitProcess.Log_Update = true;
            gitProcess.Update(1);
            checkFullMessage(4, update_expected);

            gitProcess.Log_EndTask = true;
            gitProcess.EndTask(); 
            checkFullMessage(5, endTask_expected);
        }

        //Ngit_Factory
        [Test(Description = "returns the Sharpen.dll assembly")]
        public void Dll_Sharpen()
        {
            var assembly = NGit_Factory.Dll_Sharpen();
            Assert.IsNotNull(assembly);
            Assert.IsInstanceOf<Assembly>(assembly);
            Assert.AreEqual(assembly.name(),"Sharpen");
        }

        [Test(Description = "returns the Type object of the private class Sharpen.ByteArrayOutputStream")]
        public void Type_ByteArrayOutputStream()
        {
            var type = NGit_Factory.Type_ByteArrayOutputStream();
            Assert.IsNotNull(type);
            Assert.IsInstanceOf<Type>(type);
            Assert.AreEqual(type.name(),"ByteArrayOutputStream");
            Assert.AreEqual(type.fullName(),"Sharpen.ByteArrayOutputStream");
        }

        [Test(Description = "returns a new instance of the OutputStream object")]
        public void New_OutputStream()
        {
            var outputStream = NGit_Factory.New_OutputStream();
            Assert.IsNotNull(outputStream);
            Assert.IsInstanceOf<OutputStream>(outputStream);            
        }
    }
}
