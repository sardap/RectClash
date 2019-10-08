using PaulECS;
using System;
using System.Collections.Generic;
using System.Text;
using VelcroPhysics.Collision.ContactSystem;

namespace RectClash
{
	class Combat
	{
		public class CombatPair
		{
			public IEntity Entity { get; set; } 
			public IAttackCom CurrentAttack { get; set; }

			public CombatPair(IEntity entity, IAttackCom attackCom)
			{
				Entity = entity;
				CurrentAttack = attackCom;
			}
		}

		public CombatPair First { get; set; }
		public CombatPair Secound { get; set; }
	}
}
