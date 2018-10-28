using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using LSLEditor.Docking.Win32;

namespace LSLEditor.Docking
{
    internal static class NativeMethods
    {
		[DllImport("user32", CharSet=CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DragDetect(IntPtr hWnd, Point pt);

        [DllImport("user32", CharSet=CharSet.Auto)]
        public static extern IntPtr GetFocus();

        [DllImport("user32", CharSet=CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32", CharSet=CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("user32", CharSet=CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("user32", CharSet=CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hWnd, short cmdShow);

        [DllImport("user32", CharSet=CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, FlagsSetWindowPos flags);

		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern int GetWindowLong(IntPtr hWnd, int Index);

		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern int SetWindowLong(IntPtr hWnd, int Index, int Value);

		[DllImport("user32", CharSet=CharSet.Auto)]
		public static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

		[DllImport("user32", CharSet=CharSet.Auto)]
        //*********************************
        // FxCop bug, suppress the message
        //*********************************
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
		public static extern IntPtr WindowFromPoint(Point point);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        public static extern int GetCurrentThreadId();

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern IntPtr SetWindowsHookEx(Win32.HookType code, HookProc func, IntPtr hInstance, int threadID);

        [DllImport("user32")]
        public static extern int UnhookWindowsHookEx(IntPtr hhook);

        [DllImport("user32")]
        public static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wParam, IntPtr lParam);
	}
}