using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dia3Bot
{
	class MouseEventGenerator : EventGenerator
	{

		static public void DoMouseLeftClick()
		{
			uint X = (uint)Cursor.Position.X;
			uint Y = (uint)Cursor.Position.Y;
			//Win32Wrapper.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
		}

		public override void RaiseEvent()
		{
			throw new NotImplementedException();
		}
	}
}
