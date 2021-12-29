using System.Diagnostics;
using System.Runtime.InteropServices;

namespace utils {
    // partly copied from "https://stackoverflow.com/a/67010648/4971866"
    static class WindowUtility {
        [DllImport ("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow (string lpClassName, string lpWindowName);

        [DllImport ("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos (IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;

        private static Size GetScreenSize () => new Size (GetSystemMetrics (0), GetSystemMetrics (1));

        private struct Size {
            public int Width { get; set; }
            public int Height { get; set; }

            public Size (int width, int height) {
                Width = width;
                Height = height;
            }
        }

        [DllImport ("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern int GetSystemMetrics (int nIndex);

        [DllImport ("user32.dll")]
        [
            return :MarshalAs (UnmanagedType.Bool)
        ]
        private static extern bool GetWindowRect (HandleRef hWnd, out Rect lpRect);

        [StructLayout (LayoutKind.Sequential)]
        private struct Rect {
            public int Left; // x position of upper-left corner
            public int Top; // y position of upper-left corner
            public int Right; // x position of lower-right corner
            public int Bottom; // y position of lower-right corner
        }

        private static Size GetWindowSize (IntPtr window) {
            if (!GetWindowRect (new HandleRef (null, window), out Rect rect))
                throw new Exception ("Unable to get window rect!");

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            return new Size (width, height);
        }

        public static void MoveWindowToCenter () {
            IntPtr window = Process.GetCurrentProcess ().MainWindowHandle;

            if (window == IntPtr.Zero)
                throw new Exception ("Couldn't find a window to center!");

            Size screenSize = GetScreenSize ();
            Size windowSize = GetWindowSize (window);

            int x = (screenSize.Width - windowSize.Width) / 2;
            int y = (screenSize.Height - windowSize.Height) / 2;

            SetWindowPos (window, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
        }

        private const int MF_BYCOMMAND = 0x00000000;

        public enum WINDOW_ACTIONS : int {
            CLOSE = 0xF060,
            MINIMIZE = 0xF020,
            MAXIMIZE = 0xF030,
            SIZE = 0xF000
        }

        [DllImport ("user32.dll")]
        public static extern int DeleteMenu (IntPtr hMenu, int nPosition, int wFlags);

        [DllImport ("user32.dll")]
        private static extern IntPtr GetSystemMenu (IntPtr hWnd, bool bRevert);

        [DllImport ("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow ();

        public static void RemoveWindowActions (params WINDOW_ACTIONS[] actions) {
            IntPtr window = Process.GetCurrentProcess ().MainWindowHandle;
            IntPtr sysMenu = GetSystemMenu (window, false);
            if (window == IntPtr.Zero)
                throw new Exception ("Couldn't find a window to center!");
            foreach (var action in actions) {
                DeleteMenu (sysMenu, (int) action, MF_BYCOMMAND);
            }
        }
    }
}