using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSharp.BCL
{
    public static class IO_ExtensionMethods_WinForms
    {
        public static bool askUserQuestion(this string question)
        {
            return question.askUserQuestion("O2 Question");
        }

        public static bool askUserQuestion(this string question, string title)
        {
            return System.Windows.Forms.MessageBox.Show(question, title, System.Windows.Forms.MessageBoxButtons.YesNo)
                == System.Windows.Forms.DialogResult.Yes;
        }

    }
}
