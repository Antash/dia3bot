using ScreenshotCaptureWithMouse.ScreenCapture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dia3Bot
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        static public void DoMouseLeftClick()
        {
            //Call the imported function with the cursor's current position
            uint X = (uint) Cursor.Position.X;
            uint Y = (uint) Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        // Messages
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CHAR = 0x105;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;

        static public void DoKeyPress()
        {
            // numpad 0 works ok!
            IntPtr handle = GetForegroundWindow();
            StringBuilder stringBuilder = new StringBuilder(256);
            GetWindowText(handle, stringBuilder, stringBuilder.Capacity);
            Console.WriteLine();
            if (stringBuilder.ToString().Contains("Diablo"))
            {
                SendMessage(handle, WM_SYSKEYDOWN, Convert.ToInt32(Keys.NumPad0), 0);
                SendMessage(handle, WM_SYSKEYUP, Convert.ToInt32(Keys.NumPad0), 0);
            }
        }

        static public System.Drawing.Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                         (int)(pixel & 0x0000FF00) >> 8,
                         (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }

        static public bool DetectMovement()
        {
            return false;
        }

        static public void GetCursorType()
        {
            Rectangle rect = new Rectangle(Cursor.Position, new Size(100, 100));
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            long id = DateTime.Now.Ticks;
            bmp.Save(String.Format("D:\\screen_part_{0}.jpg", id), ImageFormat.Jpeg);

            int x = 0, y = 0;
            Bitmap c = CaptureCursor(ref x, ref y);

            c.Save(String.Format("D:\\cursor_{0}.jpg", id), ImageFormat.Jpeg);
        }

        static Bitmap CaptureCursor(ref int x, ref int y)
        {
            Bitmap bmp;
            IntPtr hicon;
            Win32Stuff.CURSORINFO ci = new Win32Stuff.CURSORINFO();
            Win32Stuff.ICONINFO icInfo;
            ci.cbSize = Marshal.SizeOf(ci);
            if (Win32Stuff.GetCursorInfo(out ci))
            {
                if (ci.flags == Win32Stuff.CURSOR_SHOWING)
                {
                    hicon = Win32Stuff.CopyIcon(ci.hCursor);
                    if (Win32Stuff.GetIconInfo(hicon, out icInfo))
                    {
                        x = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
                        y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);

                        Icon ic = Icon.FromHandle(hicon);
                        bmp = ic.ToBitmap();
                        return bmp;
                    }
                }
            }

            return null;
        }

        static void Main(string[] args)
        {
            Rectangle r = Screen.PrimaryScreen.Bounds;
            Size screenSize = r.Size;

            int offsetX = 0, offsetY = -35;
            int delta = 50;
            Point[] p = {
                            new Point(screenSize.Width / 2 + offsetX, screenSize.Height / 2 + offsetY - delta),
                            new Point(screenSize.Width / 2 + offsetX + delta, screenSize.Height / 2 + offsetY),
                            new Point(screenSize.Width / 2 + offsetX, screenSize.Height / 2 + offsetY + delta),
                            new Point(screenSize.Width / 2 + offsetX- delta, screenSize.Height / 2 + offsetY)
                        };

            int i = 0;
            while (true)
            {
                Thread.Sleep(2000);
                //Cursor.Position = p[i];
                //i += i == 3 ? -i : 1;
                //DoMouseLeftClick();
                //GetCursorType();
                //DoKeyPress();
            }
            
        }
    }
}
