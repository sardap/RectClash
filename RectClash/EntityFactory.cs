using Microsoft.Xna.Framework;
using PaulECS;
using PaulECS.SFMLComs;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VelcroPhysics.Dynamics;

namespace RectClash
{
	class EntityFactory
	{
		public class FootSoliderBluePrint
		{
			public float Mass { get; set; }
			public float Width { get; set; }
			public float Height { get; set; }
			public Vector2 WorldPostion { get; set; }
			public TeamInfo Team { get; set; }
			public float MaxHealth { get; set; }
			public float CurrentHealth { get; set; }
			public float Agility { get; set; }
			public float Speed { get; set; }
			public IMovementLogic MovementLogic { get; set; }

			public FootSoliderBluePrint()
			{
				Mass = 1f;
				Width = 15f;
				Height = 15f;
				MaxHealth = 100;
				CurrentHealth = 100;
				Agility = 1f;
				MovementLogic = new MoveStraightLogic() { Volcity = new Vector2(0, 0.1f) };
			}
		}

		public ECS ECS { get; set; }

		public World World { get; set; }

		public IEntity CreateWindow()
		{
			var window = ECS.EntityCreator.CreateEntity(ECS.EntitesHolder);

			window.AddCom(new RenderWindowCom());
			window.AddCom(new InputCom());

			return window;
		}

		public IEntity CreateFootSoldier(FootSoliderBluePrint footSoliderBluePrint, IEntity window)
		{
			Debug.Assert(footSoliderBluePrint.Team != null);

			Debug.Assert(window != null);

			var redSqaure = ECS.EntityCreator.CreateEntity(window);

			redSqaure.AddCom(
				new RenderCom()
				{
					Window = window
				}
			);

			redSqaure.AddCom(
				new RectangeShapeCom()
				{
					StartingPostion = footSoliderBluePrint.WorldPostion,
					Width = footSoliderBluePrint.Width,
					Height = footSoliderBluePrint.Height,
					FillColor = footSoliderBluePrint.Team.FillColour,
					World = World,
					Restitution = 0.3f,
					Mass = footSoliderBluePrint.Mass
				}
			);

			redSqaure.AddCom(
				new MovementCom()
				{
				}
			);

			redSqaure.AddCom(
				new FootSoliderAI()
				{
					MovementLogic = footSoliderBluePrint.MovementLogic
				}
			);

			redSqaure.AddCom(
				new RectangleDataCom()
				{
					CurrentHealth = footSoliderBluePrint.CurrentHealth,
					MaxHealth = footSoliderBluePrint.MaxHealth,
					TeamInfo = footSoliderBluePrint.Team,
					Agility = footSoliderBluePrint.Agility,
					Speed = footSoliderBluePrint.Speed
				}
			);

			redSqaure.AddCom(
				new DeathCom()
				{
					Checks = new List<DeathCom.ICheckDead>()
					{
						new CheckHealthDead()
					}
				}
			);

			redSqaure.AddCom(
				new PunchAttack()
				{
					CoolDown = 500,
					Damage = 10
				}
			);


			return redSqaure;

		}

	}
}
