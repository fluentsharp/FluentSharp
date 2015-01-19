using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace FluentSharp.CoreLib.API
{
    public class Win32_Window
    {
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        public const int WM_COMMAND = 0x0112;
        public const int WM_CLOSE = 0xF060;

        [DllImport("user32.dll")                           ] public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] public static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);       
        [DllImport("user32.dll")                           ] public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        public static string GetWindowText(IntPtr hWnd)
        {
            var size = GetWindowTextLength(hWnd);
            if (size++ > 0)
            {
                var builder = new StringBuilder(size);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }
        /// <summary>
        /// Returns a list of windows with specified caption text.
        /// </summary>
        /// <param name="titleText"></param>
        /// <returns></returns>
        public static List<IntPtr> FindWindowsWithText(string titleText)
        {
            var windows = new List<IntPtr>();            
            
            EnumWindows(delegate(IntPtr wnd, IntPtr param)
            {
                if (windows.not_Contains(wnd) &&  GetWindowText(wnd).contains(titleText))                    
                    windows.add(wnd);                    
                return true;
            }, IntPtr.Zero);
            
            return windows;
        } 
        /// <summary>
        /// Given a pointer to a window , close it ( terminate)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static bool CloseWindow(IntPtr hWnd)
        {
            return 0 == SendMessage(hWnd.ToInt32(), WM_COMMAND, WM_CLOSE, 0);
        }
    }

    public static class Win32_Window_ExtensionMethods
    {
        public static bool win32_CloseWindow(this IntPtr hWnd)
        {
            return Win32_Window.CloseWindow(hWnd);
        }
        public static IntPtr win32_Desktop_Window_With_Title(this string title, bool waitForWindow = true)
        {
            var wait_Count = 10;
            var wait_Value = 100;
            var window = Win32_Window.FindWindowsWithText(title).first();    
            if (window != default(IntPtr))
                return window;
            if (waitForWindow)            
                for(var i=0; i < wait_Count; i++)
                {
                    window = Win32_Window.FindWindowsWithText(title).first();    
                    if (window != default(IntPtr))
                        return window;
                    wait_Value.sleep();
                }            
            return default(IntPtr);
        }
    }
}
