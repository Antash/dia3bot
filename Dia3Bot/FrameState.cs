using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dia3Bot
{
	class FrameState
	{
		List<KeyValuePair<Point, Color>> state;

		public FrameState(Point []pts)
		{
			state = new List<KeyValuePair<Point, Color>>();
			pts.ToList().ForEach(p => { state.Add(new KeyValuePair<Point, Color>(p, Utils.GetPixelColor(p))); });
		}

		
		public bool Compare(FrameState o)
		{
			int diff = 0;
			for (int i = 0; i < state.Count; i++)
			{
				if (state[i].Value != o.state[i].Value)
					diff++;
			}
			//Console.WriteLine(diff);
			return diff >= state.Count * 0.8;
		}

		//public bool operator == (FrameState o)
		//{
		//	int diff = 0;
		//	for (int i = 0; i < state.Count; i++)
		//	{
		//		if (state[i].Value != o.state[i].Value)
		//			diff++;
		//	}
		//	return diff < state.Count * 0.8;
		//}

		//public bool operator != (FrameState o)
		//{
		//				int diff = 0;
		//	for (int i = 0; i < state.Count; i++)
		//	{
		//		if (state[i].Value != o.state[i].Value)
		//			diff++;
		//	}
		//	return diff >= state.Count * 0.8;
		//}
	}
}
