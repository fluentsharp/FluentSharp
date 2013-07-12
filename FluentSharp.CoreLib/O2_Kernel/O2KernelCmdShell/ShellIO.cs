// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Diagnostics;
using System.IO;

namespace FluentSharp.CoreLib.API
{
    public class ShellIO
    {
        public TextReader   InputTextReader   { get; set; }
        public TextWriter   OutputTextWriter  { get; set; }
        public string       lastExecutionResult   { get; set; }

        public ShellIO()
        {
            Console.OpenStandardInput();
            Console.OpenStandardOutput();
            InputTextReader = Console.In;
            OutputTextWriter = Console.Out;
        }
        public ShellIO(TextWriter outputTextWriter) : this()
        {
            OutputTextWriter = outputTextWriter;
        }


        public void write(string textWithFormat, params object[] formatArguments)
        {
            try
            {
                OutputTextWriter.Write(textWithFormat, formatArguments);
            }
            catch (Exception ex)
            {
                Trace.Write("Error in ShellIO.write: " + textWithFormat + " : " + ex.Message);
            }
        }

        public void writeLine(string textWithFormat, params object[] formatArguments)
        {
            try
            {
                OutputTextWriter.WriteLine(textWithFormat, formatArguments);
            }
            catch (Exception ex)
            {
                Trace.Write("Error in ShellIO.writeLine: " + textWithFormat + " : " + ex.Message);
            }
        }

        public void writeLineWithPreAndPostNewLine(string textWithFormat, params object[] formatArguments)
        {
            try
            {
                OutputTextWriter.WriteLine();
                OutputTextWriter.WriteLine(textWithFormat, formatArguments);
                OutputTextWriter.WriteLine();
            }
            catch (Exception ex)
            {
                Trace.Write("Error in ShellIO.writeLine: " + textWithFormat + " : " + ex.Message);
            }
        }

        public string readLine()
        {
            return InputTextReader.ReadLine();
        }
        
    }
}
