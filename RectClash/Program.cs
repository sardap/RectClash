using System;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.ECS.Input;
using RectClash.ECS.Performance;
using RectClash.game;
using RectClash.Game;
using SFML.Graphics;

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
					Size = new SFML.System.Vector2f(windowWidth, windowHeight)
				},
				new SFMLComs.SFMLMouseInput()
			);

			EntFactory.CreateDebugInfo();

			var worldEnt = EntFactory.CreateWorld();
			var worldCom = worldEnt.GetCom<WorldCom>();
			
			Engine.Instance.CreateEnt().AddCom
			(
				new PlayerInputCom()
				{
					World = worldCom
				}
			);

			/*
			for(int i = 0; i < 0; i++)
			{
				byte[] colour = new byte[3];
				rand.NextBytes(colour);
				var circle = Engine.Instance.CreateEnt(worldEnt);
				circle.PostionCom.Postion = new SFML.System.Vector2f(rand.Next(windowWidth), rand.Next(windowHeight));
				var drawCom = circle.AddCom
				(
					new DrawCircleCom()
					{ 
						Radius = rand.Next(50),
						Color = Color.White
					}
				);
			}
			*/

			while(Engine.Instance.Window.IsOpen)
			{
				Engine.Instance.Step();

				Engine.Instance.UpdateWindow();
			}

		}
	}
}