using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using PaulECS;
using PaulECS.SFMLComs;
using SFML.System;
using VelcroPhysics;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace RectClash
{
	class FootSoliderAI : IHaveStart, ICom, IHaveUpdate
	{
		private enum States
		{
			Advance,
			ClashingWithEnemy,
			Broken,
			None
		}

		private static Random _random = new Random();

		private RectangeShapeCom _shapeCom;

		private States _state;

		private Contact _contact;

		private const double BROKEN_COOLDOWN = 500f;

		private double _brokenCooldown;

		private Vector2? _retreatTarget = null;

		public long ID { get; set; }

		public IEntity Owner { get; set; } 

		public IMovementLogic MovementLogic { get; set; }

		public void Start()
		{
			_shapeCom = Owner.GetCom<RectangeShapeCom>();
			_shapeCom.Body.OnCollision += OnCollisionHandler;
			_shapeCom.Body.OnSeparation += OnSeparationHandler;

			ChangeState(States.Advance);
		}

		public void Update(double deltaTime)
		{
			switch (_state)
			{
				case States.Advance:
					Advance();
					return;

				case States.ClashingWithEnemy:
					HandleClashWithEnemy();
					break;

				case States.Broken:
					StepBroken(deltaTime);
					break;
			}
		}

		private IEntity GetOther(Fixture a, Fixture b)
		{
			return a.Body == _shapeCom.Body ? (IEntity)b.Body.UserData : (IEntity)a.Body.UserData;
		}

		private void OnSeparationHandler(Fixture a, Fixture b, Contact contact)
		{
			if(_state != States.Broken)
			{
				ChangeState(States.None);
				_contact = null;

				var other = GetOther(a, b);

				CombatResovlver.Instance.BreakCombat(Owner, other);
			}
		}

		private void OnCollisionHandler(Fixture a, Fixture b, Contact contact)
		{
			Owner.GetCom<MovementCom>().Stop();

			var other = GetOther(a, b);

			var otherTeamCom = other.GetCom<ITeamData>();
			var thisTeamCom = Owner.GetCom<ITeamData>();


			if (otherTeamCom != null && otherTeamCom.TeamInfo != thisTeamCom.TeamInfo)
			{
				if(_state == States.Broken)
				{
					Owner.KillMe();
					return;
				}

				ChangeState(States.ClashingWithEnemy);

				_contact = contact;
			}
		}

		private void StepBroken(double deltaTime)
		{
			RunFromEnemy();

			_brokenCooldown -= deltaTime;

			if (_brokenCooldown <= 0 && !Owner.GetCom<MovementCom>().Moving)
			{
				Debug.WriteLine(RectClashDebug.GenIDPart(Owner) + " KILLING NOT MOVING AND COOLDOWN ", DebugCatagroys.AI_STATE_MACHINE);
				//Owner.KillMe();
			}
		}

		private void Advance()
		{
			MovementLogic.Step(Owner);
		}

		private void HandleClashWithEnemy()
		{
			if (Owner.GetCom<IHealthCom>().CurrentHealth < Owner.GetCom<IHealthCom>().MaxHealth * 0.3)
			{
				ChangeState(States.Broken);
				_brokenCooldown = 5000f;
			}

			var other = GetOther(_contact.FixtureA, _contact.FixtureB);

			var otherTeamCom = other.GetCom<ITeamData>();
			var thisTeamCom = Owner.GetCom<ITeamData>();
				
			if (otherTeamCom.TeamInfo != thisTeamCom.TeamInfo)
			{
				CombatResovlver.Instance.AddCombat(Owner, other);
			}
		}

		private void RunFromEnemy()
		{
			if (_retreatTarget == null)
			{
				var postion = Owner.GetCom<IMovementDataCom>().Postion;

				var home = Owner.GetCom<ITeamData>().TeamInfo.HomeArea;

				var a = home.Position.Y - home.Position.Y + home.Size.Y;
				var b = (home.Position.X + home.Size.X) - home.Position.X;
				var c = (home.Position.X * (home.Position.Y + home.Size.Y)) - ((home.Position.X + home.Size.X) * home.Position.Y);

				var p = postion;

				var xc = (Math.Pow(b, 2) * p.X - a * (b * p.Y) + c) / (Math.Pow(a, 2) + Math.Pow(b, 2));
				var yc = (Math.Pow(a, 2) * p.Y - b * (b * p.X) + c) / (Math.Pow(a, 2) + Math.Pow(b, 2));

				_retreatTarget = new Vector2((float)xc, (float)yc);

				var dest = (Vector2)_retreatTarget;

				var deltaPos = dest - p;

				var angle = Math.Atan2(deltaPos.Y, deltaPos.X);

				var speed = Owner.GetCom<IMovementDataCom>().Speed;
				var magnitude = speed;

				var velX = (float)(Math.Cos(angle) * magnitude);
				var velY = (float)(Math.Sin(angle) * magnitude);


				var direction = dest - postion;

				direction.Normalize();

				var body = Owner.GetCom<RectangeShapeCom>().Body;

				var dist = dest - postion;

				var time = 10f;

				var vol = dist * (1 / time);

				var forceDirection = Vector2.UnitY;
				var test = Matrix.CreateFromAxisAngle(new Vector3(0, 0, 1), body.Rotation);
				forceDirection = Vector2.Transform(facing, test);


				Owner.GetCom<MovementCom>().ApplyVolicty(vol);


				var xVelocity = (float)(speed * Math.Cos(angle));
				var yVelocity = (float)(speed * Math.Sin(angle));
			}
		}

		private void ChangeState(States state)
		{
			Debug.WriteLine("ID:" + Owner.ID + " NEW STATE : " + Enum.GetName(typeof(States), state), DebugCatagroys.AI_STATE_MACHINE);
			_state = state;
		}
	}
}
