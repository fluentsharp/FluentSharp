using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace FluentSharp.CoreLib.O2_DotNetWrappers.Windows
{
    public class Window
    {
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        public const int WM_COMMAND = 0x0112;
        public const int WM_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);
       
        [DllImport("user32.dll")] 
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);
        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
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
            List<IntPtr> windows = new List<IntPtr>();
            int retryCount = 10;
            while( retryCount > 0)
            { 
            EnumWindows(delegate(IntPtr wnd, IntPtr param)
            {
                if (windows.contains(wnd))
                    return true;
                if (GetWindowText(wnd).Contains(titleText))
                {
                    windows.Add(wnd);
                }
                return true;
            },
                        IntPtr.Zero);
            Thread.Sleep(500);
            retryCount--;
            }
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
}
