using Microsoft.Xna.Framework;
using PaulECS;
using PaulECS.SFMLComs;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
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

			var entityFactory = new EntityFactory()
			{
				ECS = ecs,
				World = world
			};

			var window = entityFactory.CreateWindow();

			var redTeam = new TeamInfo(world) { FillColour = Color.Red, Name = "Red", HomeArea = new RectangleShape(new Vector2f(1000, 1000) { Y = 500 }) };
			var blueTeam = new TeamInfo(world) { FillColour = Color.Blue, Name = "Blue", HomeArea = new RectangleShape(new Vector2f(1000, 1000) { Y = 0 }) };

			var redSqaure = entityFactory.CreateFootSoldier
			(
				new EntityFactory.FootSoliderBluePrint()
				{
					WorldPostion = new Vector2(200, 400),
					Mass = 1f,
					Team = redTeam,
					MovementLogic = new MoveStraightLogic() { Volcity = new Vector2(0, -0.1f) },
					Speed = 5f,
					CurrentHealth = 200,
					MaxHealth = 200
				}, 
				window
			);

			var blueSqaure = entityFactory.CreateFootSoldier
			(
				new EntityFactory.FootSoliderBluePrint()
				{
					WorldPostion = new Vector2(200, 0),
					Mass = 1f,
					Agility = 2f,
					Team = blueTeam,
					MovementLogic = new MoveStraightLogic() { Volcity = new Vector2(0, 0.1f) },
					Speed = 5f
				}, 
				window
			);

			redSqaure.GetCom<RectangleDataCom>().DamageResponses = new List<IDamageResponse>
			{
				new LowerValueDamageResponse<IMovementDataCom, float>()
				{
					TotalMinusPercent = 0.3f,
					PropToGet = typeof(IMovementDataCom).GetProperty("Speed")
				},
			};

			blueSqaure.GetCom<RectangleDataCom>().DamageResponses = new List<IDamageResponse>
			{
				new LowerValueDamageResponse<IMovementDataCom, float>()
				{
					TotalMinusPercent = 0.5f,
					PropToGet = typeof(IMovementDataCom).GetProperty("Speed")
				},
			};

			var renderWindowCom = window.GetCom<RenderWindowCom>();

			renderWindowCom.GenrateWindow(800, 600, "ExampleProgram", Color.White);

			var timer = new Stopwatch();

			timer.Start();
			while (renderWindowCom.RenderWindow.IsOpen)
			{
				ecs.Step();
				world.Step((float)timer.Elapsed.TotalMilliseconds * 0.001f);
				CombatResovlver.Instance.Step();
			}
		}
	}
}
