using Microsoft.Xna.Framework;
using PaulECS;
using PaulECS.SFMLComs;
using SFML.Graphics;
using SFML.System;
using System;
using System.Diagnostics;
using VelcroPhysics.Dynamics;

namespace RectClash
{
	class Program
	{
		static void Main(string[] args)
		{
			var ecs = new ECS();

			var world = new World(new Vector2(0, 0));

			var window = ecs.EntityCreator.CreateEntity(ecs.EntitesHolder);

			window.AddCom(new RenderWindowCom());
			window.AddCom(new InputCom());

			var redSqaure = ecs.EntityCreator.CreateEntity(window);

			redSqaure.AddCom(
				new RenderCom()
				{
					Window = window
				}
			);

			redSqaure.AddCom(
				new RectangeShapeCom()
				{
					StartingPostion = new Vector2f(400, 500),
					Size = new Vector2f(100, 100),
					FillColor = Color.Red,
					World = world,
					Restitution = 0.3f,
					Mass = 0.5f
				}
			);


			redSqaure.AddCom(
				new MovementCom()
				{
					Volicty = new Vector2f(0, -0.017f)
				}
			);

			redSqaure.AddCom(
				new ColisionResponseCom()
				{
					MaxAttack = 5
				}
			);

			redSqaure.AddCom(
				new TeamCom()
				{
					Team = "red"
				}
			);

			var blueSqaure = ecs.EntityCreator.CreateEntity(window);

			blueSqaure.AddCom(
				new RenderCom()
				{
					Window = window
				}
			);

			blueSqaure.AddCom(
				new RectangeShapeCom()
				{
					StartingPostion = new Vector2f(400, 0),
					Size = new Vector2f(100, 100),
					FillColor = Color.Blue,
					World = world,
					Restitution = 0.3f,
					Mass = 0.1f 
				}
			);

			blueSqaure.AddCom(
				new MovementCom()
				{
					Volicty = new Vector2f(0, 0.01f)
				}
			);

			blueSqaure.AddCom(
				new ColisionResponseCom()
				{
					MaxAttack = 10
				}
			);

			blueSqaure.AddCom(
				new TeamCom()
				{
					Team = "blue"
				}
			);





			var colisionDetector = new CollsionDetector();

			colisionDetector.ColisionResponseComs.Add(redSqaure.GetCom<RectangeShapeCom>());
			colisionDetector.ColisionResponseComs.Add(blueSqaure.GetCom<RectangeShapeCom>());

			var renderWindowCom = window.GetCom<RenderWindowCom>();

			renderWindowCom.GenrateWindow(800, 600, "ExampleProgram", Color.White);

			var timer = new Stopwatch();

			timer.Start();
			while (renderWindowCom.RenderWindow.IsOpen)
			{
				ecs.Step();
				world.Step((float)timer.Elapsed.TotalMilliseconds * 0.001f);
			}
		}
	}
}
