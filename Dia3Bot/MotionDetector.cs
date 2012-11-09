using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;
using Emgu.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dia3Bot
{
	class MotionDetector
	{
		public delegate void NoMotionHandler(object sender, EventArgs e);

		public event NoMotionHandler NoMotion;

		private FrameCapture _capture;
		private IBGFGDetector<Bgr> _forgroundDetector;
		private MotionHistory _motionHistory;

		public MotionDetector()
		{
			//try to create the capture
			if (_capture == null)
			{
				try
				{
					_capture = new FrameCapture();
				}
				catch (NullReferenceException excpt)
				{   //show errors if there is any
					MessageBox.Show(excpt.Message);
				}
			}

			if (_capture != null) //if camera capture has been successfully created
			{
				_motionHistory = new MotionHistory(
					1.0, //in second, the duration of motion history you wants to keep
					0.05, //in second, maxDelta for cvCalcMotionGradient
					0.5); //in second, minDelta for cvCalcMotionGradient

				_capture.ImageGrabbed += ProcessFrame;
				_capture.Start();
			}
		}

		private void ProcessFrame(object sender, EventArgs e)
		{
			using (Image<Bgr, Byte> image = _capture.RetrieveBgrFrame())
			using (MemStorage storage = new MemStorage()) //create storage for motion components
			{
				if (_forgroundDetector == null)
				{
					//_forgroundDetector = new BGCodeBookModel<Bgr>();
					_forgroundDetector = new FGDetector<Bgr>(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
					//_forgroundDetector = new BGStatModel<Bgr>(image, Emgu.CV.CvEnum.BG_STAT_TYPE.FGD_STAT_MODEL);
				}

				_forgroundDetector.Update(image);

				//update the motion history
				_motionHistory.Update(_forgroundDetector.ForgroundMask);

				//save motion frames bitmap
				//_forgroundDetector.ForgroundMask.Save(String.Format("D:\\1\\motion_mask_{0}.jpg", DateTime.Now.Ticks));

				storage.Clear(); //clear the storage
				Seq<MCvConnectedComp> motionComponents = _motionHistory.GetMotionComponents(storage);

				Console.WriteLine(String.Format("{0}", motionComponents.Count<MCvConnectedComp>()));
				if (motionComponents.Count<MCvConnectedComp>() < 5)
				{
					if (NoMotion != null)
					{
						NoMotion(this, null);
					}
				}
			}
		}
	}
}
