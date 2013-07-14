using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class MessageBox_ExtensionMethods
    {
        public static bool askUserQuestion(this string question)
        {
            return question.askUserQuestion("O2 Question");
        }
        public static bool askUserQuestion(this string question, string title)
        {
            return MessageBox.Show(question, title, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }
        public static DialogResult msgBox(this string message, params object[] formatParameters)
        {
            return message.messageBox(formatParameters);
        }
        public static DialogResult alert(this string message, params object[] formatParameters)
        {
            return message.messageBox(formatParameters);
        }
        public static DialogResult msgbox(this string message, params object[] formatParameters )
        {
            return message.messageBox(formatParameters);
        }
        public static DialogResult messageBox(this string message, params object[] formatParameters)
        {            
            if (formatParameters.size() > 0)
                message = message.format(formatParameters);            
            return MessageBox.Show(message, "O2 MessageBox");
        }


        //using Microsoft.VisualBasic.Interaction class
        public static string        askUser(this string question)
        {
            return question.askUser("O2 Question", "");
        }

        public static string        askUser(this string question, string title, string defaultValue)
        {
            var assembly = "Microsoft.VisualBasic".assembly();
            var intercation = assembly.type("Interaction");

            var parameters = new object[] { question, title, defaultValue, -1, -1 };
            return intercation.invokeStatic("InputBox", parameters).str();
        }
    }
}
