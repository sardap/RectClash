using PaulECS.SFMLComs;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	class CollsionDetector
	{
		public List<RectangeShapeCom> ColisionResponseComs { get; set; }

		public CollsionDetector()
		{
			ColisionResponseComs = new List<RectangeShapeCom>();
		}

		public void Step()
		{
			foreach(var outer in ColisionResponseComs)
			{
				foreach(var inner in ColisionResponseComs)
				{
				}
			}
		}

		private bool Insertect(RectangleShape a, RectangleShape b)
		{
			return a.Position.X < b.Position.X + b.Size.X &&
				a.Position.X + a.Size.X > b.Position.X &&
				a.Position.Y < b.Position.Y + b.Size.Y &&
				a.Position.Y + a.Size.Y > b.Position.Y;
		}
	}
}
