using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EZBlocker
{
    public static class WindowUtilities
    {
        private const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOWAPP =
            RegisterWindowMessage("WM_SHOWAPP|{0}", Program.appGuid); // register unique window message

        private delegate bool EnumWindowsProc(IntPtr windowHandle, IntPtr lParam);

        [DllImport("user32")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndStart, EnumWindowsProc callback, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        private static extern int RegisterWindowMessage(string message);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private static List<string> windowTitles = new List<string>();

        private static uint pid;

        public static List<string> GetWindowTitles(bool includeChildren, uint processId)
        {
            windowTitles.Clear();
            pid = processId;
            EnumWindows(EnumWindowsCallback, includeChildren ? (IntPtr)1 : IntPtr.Zero);
            return windowTitles;
        }

        private static bool EnumWindowsCallback(IntPtr testWindowHandle, IntPtr includeChildren)
        {
            string title = GetWindowTitle(testWindowHandle);
            if (TitleMatches(title))
            {
                uint pid;
                GetWindowThreadProcessId(testWindowHandle, out pid);
                if (pid == WindowUtilities.pid)
                    windowTitles.Add(title);
            }
            if (includeChildren.Equals(IntPtr.Zero) == false)
            {
                EnumChildWindows(testWindowHandle, EnumWindowsCallback, IntPtr.Zero);
            }
            return true;
        }

        private static string GetWindowTitle(IntPtr windowHandle)
        {
            uint SMTO_ABORTIFHUNG = 0x0002;
            uint WM_GETTEXT = 0xD;
            int MAX_STRING_SIZE = 32768;
            IntPtr result;
            string title = string.Empty;
            IntPtr memoryHandle = Marshal.AllocCoTaskMem(MAX_STRING_SIZE);
            Marshal.Copy(title.ToCharArray(), 0, memoryHandle, title.Length);
            SendMessageTimeout(windowHandle, WM_GETTEXT, (IntPtr)MAX_STRING_SIZE, memoryHandle, SMTO_ABORTIFHUNG, (uint)1000, out result);
            title = Marshal.PtrToStringAuto(memoryHandle);
            Marshal.FreeCoTaskMem(memoryHandle);
            return title;
        }

        private static bool TitleMatches(string title)
        {
            bool match = title.Contains("Spotify");
            return match;
        }

        private static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public static void ShowFirstInstance()
        {
            SendNotifyMessage(
                (IntPtr)HWND_BROADCAST,
                (uint)WM_SHOWAPP,
                UIntPtr.Zero,
                IntPtr.Zero);
        }
    }
}
