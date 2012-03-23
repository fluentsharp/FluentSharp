using System.Diagnostics;
using System.Windows.Forms;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;

public static class Processes_ExtensionMethods_WinForms
{
    // TODO: Double check if the name still matches the behaviour (and if there are no duplicate methods)

    public static Process exeConsoleOut(this TextBox textBox, string processToStart)
    {
        return textBox.exeConsoleOut(processToStart, "");
    }

    public static Process exeConsoleOut(this TextBox textBox, string processToStart, string arguments)
    {
        return textBox.startProcessAndShowConsoleOut(processToStart, arguments);
    }

    public static Process startProcessAndShowConsoleOut(this TextBox textBox, string processToStart)
    {
        return textBox.startProcessAndShowConsoleOut(processToStart, "");
    }

    public static Process startProcessAndShowConsoleOut(this TextBox textBox, string processToStart, string arguments)
    {
        textBox.setText("");
        return processToStart.startConsoleApp(arguments, (text) => textBox.append_Line(text));
    }

    public static Process exeConsoleOutWithConsoleIn(this TextBox textBox, string processToStart)
    {
        return textBox.exeConsoleOutWithConsoleIn(processToStart, "");
    }

    public static Process exeConsoleOutWithConsoleIn(this TextBox textBox, string processToStart, string arguments)
    {
        return textBox.startProcessMapConsoleOutAndReturnConsoleIn(processToStart, arguments);
    }

    public static Process startProcessMapConsoleOutAndReturnConsoleIn(this TextBox textBox, string processToStart, string arguments)
    {
        return processToStart.startConsoleAppAndRedirectInput(arguments,
                                                              (text) => textBox.append_Line(text),
                                                              (text) => textBox.append_Line(text));
    }
}