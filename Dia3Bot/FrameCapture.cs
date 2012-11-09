using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dia3Bot
{
	class FrameCapture
	{
		const int frameWidth = 110,
			frameHeigth = 170;
		//Rectangle rect;

		System.Threading.Timer t;
		internal void Start()
		{
			//var p = new Point(Screen.PrimaryScreen.Bounds.Size.Width / 2 - 40, Screen.PrimaryScreen.Bounds.Size.Height / 2 - 170);
			//var s = new Size(frameWidth, frameHeigth);
			//rect = new Rectangle(p, s);
			t = new System.Threading.Timer(TimerCallback, null, 0, 100);
		}

		public delegate void ImageGrabbedHandler(object sender, EventArgs e);

		public event ImageGrabbedHandler ImageGrabbed;
	
		internal Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> RetrieveBgrFrame()
		{
			//draw motion frame bounds
			FrameChecker.CheckHeroFrame();
			var rect = FrameChecker.GetHeroRectangle();
			Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmp);
			g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
			//bmp.Save(String.Format("D:\\1\\screen_part_{0}.jpg", DateTime.Now.Ticks), ImageFormat.Jpeg);
			var img = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte>(bmp);
			return img;
		}

		void TimerCallback(object state)
		{
			if (ImageGrabbed != null)
				ImageGrabbed(this, null);
		}
	}
}
