using Microsoft.Xna.Framework;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace RectClash
{
	class TeamInfo
	{
		public string Name { get; set; }

		public Color FillColour { get; set; } 

		public RectangleShape HomeArea { get; set; }

		public TeamInfo(World world)
		{
		}
	}
}
