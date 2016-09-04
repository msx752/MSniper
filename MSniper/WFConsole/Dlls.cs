using System;
using System.Runtime.InteropServices;

namespace MSniper.WFConsole
{
    public static class Dlls
    {
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void BringToFront(IntPtr hWnd)
        {
            SetForegroundWindow(hWnd);
        }
    }
}
