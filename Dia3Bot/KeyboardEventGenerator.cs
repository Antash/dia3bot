using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dia3Bot
{
	class KeyboardEventGenerator : EventGenerator
	{
		public override void RaiseEvent()
		{
			//Win32Wrapper.SendMessage(handle, Win32Wrapper.WM_SYSKEYDOWN, Convert.ToInt32(Keys.NumPad0), 0);
			//Win32Wrapper.SendMessage(handle, Win32Wrapper.WM_SYSKEYUP, Convert.ToInt32(Keys.NumPad0), 0);
			throw new NotImplementedException();
		}
	}
}
