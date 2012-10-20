using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dia3Bot
{
	class MouseEvent
	{
		public enum EventType
		{
			MouseClick,
			MouseDown,
			MouseUp,
			MouseDoubleClick
		};

		public void Execute(EventType type, MouseEventArgs e)
		{
			switch (type)
			{
				case EventType.MouseClick:
					break;
 
			}
			//Win32Wrapper.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
		}
	}
}
