using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
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
		public enum CursorType { Arrow, Sword, Sell, Talk, Hand, None }

		static List<KeyValuePair<CursorType, Bitmap>> cursors = new List<KeyValuePair<CursorType, Bitmap>>();

		static public void InitCursorCollection()
		{
			cursors.Add(new KeyValuePair<CursorType, Bitmap>(CursorType.Arrow, new Bitmap(Bitmap.FromFile(@"..\..\..\cursors\arrow.jpg"))));
			cursors.Add(new KeyValuePair<CursorType, Bitmap>(CursorType.Hand, new Bitmap(Bitmap.FromFile(@"..\..\..\cursors\hand.jpg"))));
			cursors.Add(new KeyValuePair<CursorType, Bitmap>(CursorType.Sell, new Bitmap(Bitmap.FromFile(@"..\..\..\cursors\sell.jpg"))));
			cursors.Add(new KeyValuePair<CursorType, Bitmap>(CursorType.Sword, new Bitmap(Bitmap.FromFile(@"..\..\..\cursors\sword.jpg"))));
			cursors.Add(new KeyValuePair<CursorType, Bitmap>(CursorType.Talk, new Bitmap(Bitmap.FromFile(@"..\..\..\cursors\talk.jpg"))));
		}

		//static public void GetCursor()
		//{
		//	//Rectangle rect = new Rectangle(Cursor.Position, new Size(100, 100));
		//	//Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
		//	//Graphics g = Graphics.FromImage(bmp);
		//	//g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

		//	//long id = DateTime.Now.Ticks;
		//	//bmp.Save(String.Format("D:\\screen_part_{0}.jpg", id), ImageFormat.Jpeg);


		//}

		const int offsetX = 0, offsetY = -35;
		const int delta = 50;
		static Point[] p = {
                            new Point(Screen.PrimaryScreen.Bounds.Size.Width / 2 + offsetX, Screen.PrimaryScreen.Bounds.Size.Height / 2 + offsetY - delta),
                            new Point(Screen.PrimaryScreen.Bounds.Size.Width / 2 + offsetX + delta, Screen.PrimaryScreen.Bounds.Size.Height / 2 + offsetY),
                            new Point(Screen.PrimaryScreen.Bounds.Size.Width / 2 + offsetX, Screen.PrimaryScreen.Bounds.Size.Height / 2 + offsetY + delta),
                            new Point(Screen.PrimaryScreen.Bounds.Size.Width / 2 + offsetX- delta, Screen.PrimaryScreen.Bounds.Size.Height / 2 + offsetY)
                        };

		static void Main(string[] args)
		{
			InitCursorCollection();

			MotionDetector md = new MotionDetector();
			md.NoMotion += md_NoMotion;
			//some shit
			//System.Threading.Timer t = new System.Threading.Timer(TimerCallback, null, 0, 500);

			while (true) ;
			//{
			//    Thread.Sleep(2000);
			//    //Cursor.Position = p[i];
			//    //i += i == 3 ? -i : 1;
			//    //DoMouseLeftClick();
			//    //GetCursorType();
			//    //DoKeyPress();
			//}
		}

		public static void MouseLeftClick(Point p)
		{
			//| Win32Wrapper.MOUSEEVENTF_LEFTUP
			Win32Wrapper.mouse_event(Win32Wrapper.MOUSEEVENTF_LEFTDOWN , (uint)p.X, (uint)p.Y, 0, 0);
		}

		public static void MouseLeftUp(Point p)
		{
			//| Win32Wrapper.MOUSEEVENTF_LEFTUP
			Win32Wrapper.mouse_event(Win32Wrapper.MOUSEEVENTF_LEFTUP, (uint)p.X, (uint)p.Y, 0, 0);
		}

		static int i = 0;

		static void md_NoMotion(object sender, EventArgs e)
		{
			MouseLeftUp(p[i]);
			i = i == 3 ? 0 : i+1;
		}

		static void bar()
		{
			//Bitmap bmpSnip = new Bitmap(@"D:\arrow.jpg");
			//Bitmap bmpSnip = new Bitmap(@"D:\gold_pattern.jpg");
			Bitmap bmpSnip = new Bitmap(@"D:\magic.jpg");
			//Bitmap bmpSnip = new Bitmap(@"D:\char.jpg");
			//Bitmap bmpSnip = new Bitmap(@"D:\arrow2.png");
			Bitmap bmpSource = new Bitmap(@"D:\2.jpg");
			Image<Emgu.CV.Structure.Bgr, Byte> templateImage = new Image<Emgu.CV.Structure.Bgr, Byte>(bmpSnip);
			Image<Emgu.CV.Structure.Bgr, Byte> sourceImage = new Image<Emgu.CV.Structure.Bgr, Byte>(bmpSource);

			List<MCvBox2D> boxList = new List<MCvBox2D>();

			//Load the image from file

			//Convert the image to grayscale and filter out the noise
			//Image<Gray, Byte> gray = sourceImage.Convert<Gray, Byte>().PyrDown().PyrUp();

			Gray cannyThreshold = new Gray(180);
			Gray cannyThresholdLinking = new Gray(120);
			Gray circleAccumulatorThreshold = new Gray(120);

			Image<Gray, Byte> cannyEdges = sourceImage.Canny(0, 200, 3);

			using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
				for (Contour<Point> contours = cannyEdges.FindContours(); contours != null; contours = contours.HNext)
				{
					Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);

					if (contours.Area > 600) //only consider contours with area greater than 250

						if (currentContour.Total == 4) //The contour has 4 vertices.
						{
							#region determine if all the angles in the contour are within the range of [80, 100] degree
							bool isRectangle = true;
							Point[] pts = currentContour.ToArray();
							LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

							for (int i = 0; i < edges.Length; i++)
							{
								double angle = Math.Abs(
								   edges[(i + 1) % edges.Length].GetExteriorAngleDegree(edges[i]));
								if (angle < 80 || angle > 100)
								{
									isRectangle = false;
									break;
								}
							}
							#endregion

							if (isRectangle) boxList.Add(currentContour.GetMinAreaRect());
						}
				}
			foreach (var rect in boxList)
			{
				RectangleF r = new RectangleF(rect.center.X, rect.center.Y, rect.size.Width, rect.size.Height);
				sourceImage.Draw(rect, new Bgr(Color.Red), 2);
			}

			//sourceImage = sourceImage.SmoothBlur(1, 0);
			sourceImage.Save(String.Format("D:\\2_{0}.jpg", DateTime.Now.Ticks));
		}

		static bool ismoving = false;
		static DateTime datestartmov;

		private static void TimerCallback(object state)
		{
			MouseLeftClick(p[i]);
		}
	}
}
