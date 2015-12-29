using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Borderless
{
    public class Program
    {
        #region Windows related
        [DllImport("USER32.DLL")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("USER32.DLL")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public static int GWL_STYLE = -16;
        public static int GWL_EXSTYLE = -20;

        public static int WS_BORDER = 0x00800000;
        public static int WS_CAPTION  = 0x00C00000;
        public static int WS_HSCROLL = 0x00100000;
        public static int WS_THICKFRAME = 0x00040000;
        public static int WS_VSCROLL = 0x00200000;
        public static int WS_EX_WINDOWEDGE = 0x00000100;
        #endregion

        public static void Main(string[] args)
        {
            var argumentString = args.Skip(1).Aggregate(string.Empty, (current, param) => current + param + " ");

            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = args[0],
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = argumentString
            };


                using (var proc = Process.Start(startInfo))
                {
                    while (proc.MainWindowHandle == (IntPtr)0)
                    {
                        Thread.Sleep(10);
                    }

                    var pFoundWindow = proc.MainWindowHandle;
                    var gwlStyle = GetWindowLong(pFoundWindow, GWL_STYLE);
                    SetWindowLong(pFoundWindow, GWL_STYLE, gwlStyle & ~(WS_THICKFRAME | WS_CAPTION | WS_HSCROLL | WS_VSCROLL));
                    var gwlExStyle = GetWindowLong(pFoundWindow, GWL_EXSTYLE);
                    SetWindowLong(pFoundWindow, GWL_EXSTYLE, gwlExStyle & ~(WS_EX_WINDOWEDGE));
                }
        }
    }
}
