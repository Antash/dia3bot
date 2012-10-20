using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dia3Bot
{
	class Utils
	{
		static public System.Drawing.Color GetPixelColor(int x, int y)
		{
			IntPtr hdc = Win32Wrapper.GetDC(IntPtr.Zero);
			uint pixel = Win32Wrapper.GetPixel(hdc, x, y);
			Win32Wrapper.ReleaseDC(IntPtr.Zero, hdc);
			Color color = Color.FromArgb((int)(pixel & 0x000000FF),
						 (int)(pixel & 0x0000FF00) >> 8,
						 (int)(pixel & 0x00FF0000) >> 16);
			return color;
		}

		static public System.Drawing.Color GetPixelColor(Point p)
		{
			return GetPixelColor(p.X, p.Y);
		}

		public static Bitmap CaptureCursor(ref int x, ref int y)
		{
			Bitmap bmp;
			IntPtr hicon;
			Win32Wrapper.CURSORINFO ci = new Win32Wrapper.CURSORINFO();
			Win32Wrapper.ICONINFO icInfo;
			ci.cbSize = Marshal.SizeOf(ci);
			if (Win32Wrapper.GetCursorInfo(out ci))
			{
				if (ci.flags == Win32Wrapper.CURSOR_SHOWING)
				{
					hicon = Win32Wrapper.CopyIcon(ci.hCursor);
					if (Win32Wrapper.GetIconInfo(hicon, out icInfo))
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

		public static bool IsMatch(Bitmap orig, Bitmap bmp)
		{
			Image<Emgu.CV.Structure.Bgr, Byte> templateImage = new Image<Emgu.CV.Structure.Bgr, Byte>(bmp);
			Image<Emgu.CV.Structure.Bgr, Byte> sourceImage = new Image<Emgu.CV.Structure.Bgr, Byte>(orig);
			Image<Emgu.CV.Structure.Gray, float> imgMatch = sourceImage.MatchTemplate(templateImage, Emgu.CV.CvEnum.TM_TYPE.CV_TM_CCOEFF_NORMED);

			float[,,] matches = imgMatch.Data;
			for (int y = 0; y < matches.GetLength(0); y++)
			{
				for (int x = 0; x < matches.GetLength(1); x++)
				{
					double matchScore = matches[y, x, 0];
					if (matchScore > 0.8)
					{
						return true;
					}

				}
			}
			return false;
		}

		public static bool IsDiabloWindowActive()
		{
			IntPtr handle = Win32Wrapper.GetForegroundWindow();
			StringBuilder stringBuilder = new StringBuilder(256);
			Win32Wrapper.GetWindowText(handle, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString().Contains("Diablo") ? true : false;
		}
	}
}
