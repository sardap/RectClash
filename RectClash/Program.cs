using System;
using RectClash.ECS;
using RectClash.ECS.Graphics;
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


			/*
			var firstRect = Engine.Instance.CreateEnt("firstRect");
			firstRect.PostionCom.LocalScale = new SFML.System.Vector2f(10f, 10f);
			firstRect.AddCom
			(
				new DrawRectCom()
				{
					FillColor = Color.White,
					Priority = DrawPriority.GRID_BACKGROUND
				}
			);

			var secRect = Engine.Instance.CreateEnt(firstRect, "secRect");
			secRect.PostionCom.LocalPostion = new SFML.System.Vector2f(0, 0);
			secRect.AddCom
			(
				new DrawRectCom()
				{
					FillColor = Color.Red,
					Priority = DrawPriority.GRID_OVERLAY
				}
			);

			var thirdRect = Engine.Instance.CreateEnt(firstRect, "thirdRect");
			secRect.PostionCom.LocalPostion = new SFML.System.Vector2f(1, 0);
			thirdRect.AddCom
			(
				new DrawRectCom()
				{
					FillColor = Color.Blue,
					Priority = DrawPriority.UNITS
				}
			);
			*/

			while(Engine.Instance.Window.IsOpen)
			{
				Engine.Instance.Step();

				Engine.Instance.UpdateWindow();
			}

		}
	}
}