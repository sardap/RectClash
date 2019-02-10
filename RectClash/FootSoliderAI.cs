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
			JustBroken,
			Broken
		}

		private static Dictionary<Body, IEntity> _bodyToEntryTable = new Dictionary<Body, IEntity>();

		private static Random _random = new Random();

		private RectangeShapeCom _shapeCom;

		private int _health = 10;

		private States _state;

		private Contact _contact;

		private IAttackCom _currentAttack;

		private const double BROKEN_COOLDOWN = 500f;

		private double _brokenCooldown;

		public long ID { get; set; }

		public IEntity Owner { get; set; } 

		public void Start()
		{
			_shapeCom = Owner.GetCom<RectangeShapeCom>();
			_shapeCom.Body.OnCollision += OnCollisionHandler;
			_shapeCom.Body.OnSeparation += OnSeparationHandler;

			_bodyToEntryTable.Add(_shapeCom.Body, Owner);

			_state = States.Advance;
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

				case States.JustBroken:
					RunFromEnemy();
					break;

				case States.Broken:
					StepBroken(deltaTime);
					break;
			}
		}

		private IEntity GetOther(Fixture a, Fixture b)
		{
			return a.Body == _shapeCom.Body ? _bodyToEntryTable[b.Body] : _bodyToEntryTable[a.Body];
		}

		private void OnSeparationHandler(Fixture a, Fixture b, Contact contact)
		{
			if(_state != States.JustBroken || _state != States.Broken)
			{
				_state = States.Advance;
				_contact = null;

				var other = GetOther(a, b);

				CombatResovlver.Instance.BreakCombat(Owner, other);
			}
		}

		private void OnCollisionHandler(Fixture a, Fixture b, Contact contact)
		{
			var other = GetOther(a, b);

			var otherTeamCom = other.GetCom<ITeamData>();
			var thisTeamCom = Owner.GetCom<ITeamData>();


			if (otherTeamCom != null && otherTeamCom.Team != thisTeamCom.Team)
			{
				Debug.WriteLine(RectClashDebug.GenIDPart(Owner) + " CLASHING ", DebugCatagroys.AI_STATE_MACHINE);

				_state = States.ClashingWithEnemy;

				_contact = contact;
			}
		}

		private void StepBroken(double deltaTime)
		{
			_brokenCooldown -= deltaTime;
			Owner.GetCom<MovementCom>().ApplyVolicty();

			if (_brokenCooldown <= 0 && !Owner.GetCom<MovementCom>().Moving)
			{
				Debug.WriteLine(RectClashDebug.GenIDPart(Owner) + " KILLING NOT MOVING AND COOLDOWN ", DebugCatagroys.AI_STATE_MACHINE);
				Owner.KillMe();
			}
		}

		private void Advance()
		{
			if (!Owner.GetCom<MovementCom>().Moving)
			{
				Debug.WriteLineIf(Owner.GetCom<IMovementDataCom>().Volicty.X == 0f && Owner.GetCom<IMovementDataCom>().Volicty.Y == 0f, "ID:" + Owner.ID + " Advanceing ", DebugCatagroys.AI_STATE_MACHINE);
				Owner.GetCom<MovementCom>().ApplyVolicty();
			}
		}

		private void HandleClashWithEnemy()
		{

			if (Owner.GetCom<IHealthCom>().CurrentHealth < Owner.GetCom<IHealthCom>().MaxHealth * 0.3)
			{
				_state = States.JustBroken;
			}

			if(_currentAttack == null || _currentAttack.State == AttackStates.None)
			{
				var other = _bodyToEntryTable[_contact.FixtureA.Body] != _shapeCom.Body ? _bodyToEntryTable[_contact.FixtureB.Body] : _bodyToEntryTable[_contact.FixtureA.Body];

				var otherTeamCom = other.GetCom<ITeamData>();
				var thisTeamCom = Owner.GetCom<ITeamData>();
				
				if (otherTeamCom.Team != thisTeamCom.Team)
				{
					CombatResovlver.Instance.AddCombat(Owner, other);
				}
			}
		}

		private void RunFromEnemy()
		{
			Owner.GetCom<IMovementDataCom>().Volicty = (Owner.GetCom<IMovementDataCom>().Volicty * -1);

			Debug.WriteLine("ID:" + Owner.ID + " Broken ", DebugCatagroys.AI_STATE_MACHINE);

			_brokenCooldown = BROKEN_COOLDOWN;

			_state = States.Broken;
		}
	}
}
