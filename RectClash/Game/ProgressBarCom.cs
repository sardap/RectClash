using RectClash.ECS;
using RectClash.ECS.Graphics;

namespace RectClash.Game
{
	public class ProgressBarCom : Com
	{
		private double _precent;

		public DrawRectCom Background { get; set; }
		
		public DrawRectCom Forground { get; set; }

		public double Percent 
		{ 
			get => _precent;
			set
			{
				_precent = value;
				Forground.PostionCom.LocalScale = new SFML.System.Vector2f((float)_precent, 1f);
			}
		}

		protected override void InternalStart()
		{
			Percent = 1f;
		}
	}
}