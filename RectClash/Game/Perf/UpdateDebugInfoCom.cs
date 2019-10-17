using System.Collections.Generic;
using System.Linq;
using RectClash.ECS;
using RectClash.ECS.Graphics;

namespace RectClash.Game.Perf
{
	public class UpdateDebugInfoCom: Com, IObv<string, PerfEvents>
	{
		public RenderTextCom Text { get; set; }

		protected override void InternalStart()
		{
			Text = Owner.GetCom<RenderTextCom>();
		}

		private Dictionary<PerfEvents, string> _info = new Dictionary<PerfEvents, string>();

		public override void Update()
		{
			string DoubleToString(double val)
			{
				return val.ToString("0.##");
			}

			var engine = Engine.Instance;
			if(_info.Values.Count >= 1)
			{
				Text.Text = _info.Values.Aggregate((a,b)  => a + "\n" + b);
			}
			else
			{
				Text.Text = "";
			}

			Text.Text += 
				"\nLocalMouse(" + DoubleToString(engine.Mouse.CamMouseX) + "," + DoubleToString(engine.Mouse.CamMouseY) + ")" +
				"\nWorldMouse(" + DoubleToString(engine.Mouse.WorldMouseX) + "," + DoubleToString(engine.Mouse.WorldMouseY) + ")" +
				"\nViewPos(" + DoubleToString(engine.Window.Camera.Postion.X) + "," + DoubleToString(engine.Window.Camera.Postion.Y) + ")";

		}

        public void OnNotify(string ent, PerfEvents evt)
        {
			if(!_info.ContainsKey(evt))
			{
				_info.Add(evt, ent);
			}

			_info[evt] = ent;
        }
    }
}