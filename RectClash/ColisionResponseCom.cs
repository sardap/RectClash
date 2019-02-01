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
	class ColisionResponseCom : IHaveStart, ICom, IHaveUpdate
	{
		private static Dictionary<Body, IEntity> _bodyToEntryTable = new Dictionary<Body, IEntity>();

		private static Random _random = new Random();

		private RectangeShapeCom _shapeCom;

		private int _health = 10;

		public long ID { get; set; }

		public IEntity Owner { get; set; } 

		public int MaxAttack { get; set; }

		public void Start()
		{
			_shapeCom = Owner.GetCom<RectangeShapeCom>();
			_shapeCom.Body.OnCollision += OnCollisionHandler;
			_bodyToEntryTable.Add(_shapeCom.Body, Owner);
		}

		public void Update(double deltaTime)
		{
		}

		private void OnCollisionHandler(Fixture a, Fixture b, Contact contact)
		{
			var other = _bodyToEntryTable[a.Body] != _shapeCom.Body ? _bodyToEntryTable[b.Body] : _bodyToEntryTable[a.Body];

			var otherTeamCom = other.GetCom<TeamCom>();
			var thisTeamCom = Owner.GetCom<TeamCom>();

			if (otherTeamCom != null && otherTeamCom.Team != thisTeamCom.Team)
			{
				var damage = _random.Next(0, MaxAttack);

				other.GetCom<ColisionResponseCom>()._health -= damage;

				Debug.WriteLine(otherTeamCom.Team + " DELT " + damage + " TO " + thisTeamCom.Team);
			}
		}
	}
}
