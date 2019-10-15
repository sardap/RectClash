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

			EntFactory.Instance.CreateDebugInfo();

			var worldEnt = EntFactory.Instance.CreateWorld();
			EntFactory.Instance.CreatePlayerInput();
			

			while(Engine.Instance.Window.IsOpen)
			{
				Engine.Instance.Step();

				Engine.Instance.UpdateWindow();
			}

		}
	}
}