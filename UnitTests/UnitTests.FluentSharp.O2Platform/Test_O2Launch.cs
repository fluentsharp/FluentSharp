using System;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using FluentSharp.O2Platform.Utils;
using FluentSharp.WinForms;
using NUnit.Framework;
using FluentSharp.O2Platform;

namespace UnitTests.FluentSharp.O2Platform
{
    [TestFixture]
    public class Test_O2_Start : NUnitTests
    {
        [Test]
        public void O2_Start_Ctor()
        {
            var o2Start = new O2_Start();
            assert_Not_Null(o2Start);
            assert_Not_Null(o2Start.o2PlatformConfig);
            assert_Not_Null(o2Start.o2PlatformScripts);
        }

        [Test]
        public void OpenStartGui()
        {          
            var o2Start = new O2_Start();
            assert_Is_True(o2Start.O2PlatformScriptsExist());
            
            var currentProcess = Processes.getCurrentProcess();
            
            assert_Are_Equal(currentProcess.MainWindowHandle, IntPtr.Zero);

            assert_Size_Is(Application.OpenForms,0);

            o2Start.OpenStartGui();  // this should open the main UI
            
            currentProcess.waitFor_2nd_MainWindowHandle();      

            assert_Are_Not_Equal(currentProcess.MainWindowHandle, IntPtr.Zero);
            var openForms = Application.OpenForms.toList<Form>(); 
            var firstWindowTitle = "OWASP O2 Platform 5.4 - Launcher";
            var secondWindowTitle = "{0} : {1}".format("OWASP O2 Platform v5.4.0.0" , clr.details());

            //get expected form references (note: resharper runnner sometimes doesn't catch the 1st one)
            var lauchedUI_Form    = firstWindowTitle.applicationWinForm();
            var o2PlatformUI_Form = secondWindowTitle.applicationWinForm();
            
            assert_Not_Null(o2PlatformUI_Form);

            //close open forms
            o2PlatformUI_Form.close().waitForClose();
            if(lauchedUI_Form.notNull())
                lauchedUI_Form.close().waitForClose();

            assert_Size_Is(Application.OpenForms,0);

            /*if(openForms.size() == 2)
            {
                assert_Size_Is(openForms,2);
            
                assert_Are_Equal(firstWindowTitle, openForms.first().get_Text());
                assert_Are_Equal(secondWindowTitle, openForms.second().get_Text());

                openForms.first().close().waitForClose();
                openForms.second().close().waitForClose();       
                assert_Size_Is(Application.OpenForms,0);
            }
            else 
            { }*/
            
        }
    }
}
