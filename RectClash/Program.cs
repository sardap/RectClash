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

			var redSqaure = entityFactory.CreateFootSoldier
			(
				new EntityFactory.FootSoliderBluePrint()
				{
					WorldPostion = new Vector2(200, 400),
					Team = "Red",
					Mass = 10f,
					Volicty = new Vector2(0, -0.05f),
					FillColor = Color.Red
				}, 
				window
			);

			var blueSqaure = entityFactory.CreateFootSoldier
			(
				new EntityFactory.FootSoliderBluePrint()
				{
					WorldPostion = new Vector2(200, 0),
					Team = "Blue",
					Mass = 5f,
					Volicty = new Vector2(0, 0.1f),
					FillColor = Color.Blue,
					Agility = 2f,
					CurrentHealth = 150f
				}, 
				window
			);

			blueSqaure.GetCom<RectangleDataCom>().DamageResponses = new List<IDamageResponse>
			{
				new LowerValueDamageResponse<IMovementDataCom, Vector2>()
				{
					TotalMinusPercent = 0.1f,
					PropToGet = typeof(IMovementDataCom).GetProperty("Volicty")
				},
				new LowerValueDamageResponse<IAgilityDataCom, float>()
				{
					TotalMinusPercent = 1f,
					PropToGet = typeof(IAgilityDataCom).GetProperty("Agility")
				}
			};


			redSqaure.GetCom<RectangleDataCom>().DamageResponses = new List<IDamageResponse>
			{
				new LowerValueDamageResponse<IMovementDataCom, Vector2>()
				{
					TotalMinusPercent = 0.1f,
					PropToGet = typeof(IMovementDataCom).GetProperty("Volicty")
				},
				new LowerValueDamageResponse<IAgilityDataCom, float>()
				{
					TotalMinusPercent = 1f,
					PropToGet = typeof(IAgilityDataCom).GetProperty("Agility")
				}
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
