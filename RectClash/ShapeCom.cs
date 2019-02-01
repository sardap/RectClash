using Microsoft.Xna.Framework;
using PaulECS;
using PaulECS.SFMLComs;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace RectClash
{
	class RectangeShapeCom : ICom, IRenderableCom, IHaveStart
	{
		public long ID { get; set; }

		public IEntity Owner { get; set; }

		public Vector2f StartingPostion { get; set; }

		public Body Body { get; set; }

		public Color FillColor { get; set; }

		public Vector2f Size { get; set; }

		public World World { get; set; }

		public float Restitution { get; set; }

		public float Mass { get; set; }

		public RectangleShape Rectangle
		{
			get
			{
				return new RectangleShape() { Position = Utiliy.ToSFMLVec(Body.Position), Size = Size, FillColor = FillColor }; 
			}
		}

		public Shape Shape
		{
			get
			{
				return Rectangle;
			}
		}

		public Drawable Drawable
		{
			get
			{
				return Shape;
			}
		}

		public bool Draw { get; set; }

		public RectangeShapeCom()
		{
			Draw = true;
		}

		public void Start()
		{
			Body = BodyFactory.CreateRectangle(World, Size.X, Size.Y, 1, new Vector2(StartingPostion.X, StartingPostion.Y));
			Body.BodyType = BodyType.Dynamic;
			Body.Mass = Mass;
			Body.Restitution = Restitution;
			Body.Friction = 0.5f;
		}
	}
}
