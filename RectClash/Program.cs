using System;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.ECS.Input;
using RectClash.ECS.Performance;
using RectClash.game;
using RectClash.Game;

namespace RectClash
{
	class Program
	{
		private static Random rand = new Random();

		static void Main(string[] args)
		{
			int windowWidth = 1280;
			int windowHeight = 720;

			Engine.Initialise
			(
				new SFMLComs.SFMLKeyboardInput(), 
				new SFMLComs.SFMLWindow()
				{
					Size = new Misc.Vector2<int>(windowWidth, windowHeight)
				}
			);

			var font = new Font()
			{
				FileLocation = "calibri.ttf"
			};

			var debug = Engine.Instance.CreateEnt();
			debug.PostionCom.X = 10;
			debug.PostionCom.Y = 10;
			debug.AddCom
			(
				new RenderTextCom()
				{
					Font = font,
					Colour = new Colour(byte.MaxValue, byte.MaxValue, 0, 0),
					Floating = true
				}
			);
			debug.AddCom(new UpdateDebugInfoCom());

			EntFactory entFactory = new EntFactory();

			var worldEnt = entFactory.CreateWorld();
			var worldCom = worldEnt.GetCom<WorldCom>();
			
			/*
			for(int i = 0; i < 100; i++)
			{
				byte[] colour = new byte[3];
				rand.NextBytes(colour);

				var circle = Engine.Instance.CreateEnt(worldEnt);
				var postion = circle.AddCom
				(
					new PostionCom()
					{
						Postion = new Misc.Vector2<double>(rand.Next(windowWidth), rand.Next(windowHeight))
					}
				);
				var drawCom = circle.AddCom
				(
					new DrawCircleCom()
					{ 
						Radius = rand.Next(50),
						Colour = new Colour(byte.MaxValue, colour[0], colour[1], colour[2]),
						PostionCom = postion
					}
				);
				circle.AddCom
				(
					new CircleMove()
					{
						Speed = 0.1,
						DrawCircle = (DrawCircleCom)drawCom
					}
				);
				var unitInfoCom = circle.AddCom
				(
					new UnitInfoCom()
					{
						Postion = (PostionCom)postion
					}
				);
				worldCom.Units.Add(unitInfoCom);
			}
			*/

			Engine.Instance.CreateEnt().AddCom(new PlayerInputCom());

			while(Engine.Instance.Window.IsOpen)
			{
				Engine.Instance.Step();

				Engine.Instance.UpdateWindow();
			}
		}
	}
}