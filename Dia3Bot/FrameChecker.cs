using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dia3Bot
{
    public static class FrameChecker
    {
        private static readonly Screen Screen = Screen.PrimaryScreen;


        public static void CheckFullScreenFrame()
        {
           // Point cursorLocation;

           // Win32Wrapper.CURSORINFO cursorInfo;
           // Win32Wrapper.GetCursorInfo(out cursorInfo);



           // var p = new Point(cursorInfo.ptScreenPos.x, cursorInfo.ptScreenPos.y);
           // Screen screen = Screen.FromPoint(p);
          

           //Point upperLeftPoint = new Point(_screen.Bounds.Left, _screen.Bounds.Y);
           //Point upperRightPoint = new Point(_screen.Bounds.Right, _screen.Bounds.Y);
           //Point bottomLeftPoint = new Point(_screen.Bounds.X, _screen.Bounds.Top);
           //Point bottomRightPoint = new Point(_screen.Bounds.X, _screen.Bounds.Bottom);

           using (Graphics graphics = Graphics.FromHwnd(Win32Wrapper.GetDesktopWindow()))
           {
               graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red), 5), Screen.Bounds);
           }
        }

        public static void CheckHeroFrame()
        {
            using (Graphics graphics = Graphics.FromHwnd(Win32Wrapper.GetDesktopWindow()))
            {
                graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red), 3), GetHeroRectangle());
            }
        }

        public static Rectangle GetHeroRectangle()
        {
            var centerWidth = Screen.Bounds.Width / 2;
            var centerHeight = Screen.Bounds.Height / 2;
            var centerPoint = new Point(centerWidth, centerHeight);
            const int frameWidth = 110, frameHeigth = 170, offset = 100;
            var heroPoint = new Point(centerPoint.X - frameWidth / 2, centerPoint.Y - frameHeigth / 2 - offset);
            var s = new Size(frameWidth, frameHeigth);
            var rect = new Rectangle(heroPoint, s);
            return rect;
        }
    }
}
