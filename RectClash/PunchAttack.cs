using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using PaulECS;

namespace RectClash
{
	class PunchAttack : IAttackCom, IHaveUpdate
	{
		private double _countDown;

		private IEntity _target;

		public long ID { get; set; }

		public IEntity Owner { get; set; }

		public double CoolDown { get; set; }

		public float Damage { get; set; }

		public AttackStates State { get; set; }

		public void Attack(IEntity toAttack)
		{
			Debug.Assert(Owner != toAttack);

			_target = toAttack;
			State = AttackStates.Waiting;
			_countDown = CoolDown;
		}

		public void Interupt()
		{
			_countDown = CoolDown;
		}

		public void Complete()
		{
			var otherHealthCom = _target.GetCom<IHealthCom>();

			var damage = Utiliy.NextFloat(0, Damage);

			otherHealthCom.DealDamage(damage);

			_target = null;

			Debug.WriteLine(RectClashDebug.GenIDPart(Owner) + "Punched " + RectClashDebug.GenIDPart(otherHealthCom.Owner) + " Damage: " + damage, DebugCatagroys.ATTACK);

		}

		public void Update(double deltaTime)
		{
			_countDown -= deltaTime;

			if (State == AttackStates.Waiting && _countDown <= 0)
			{

				State = AttackStates.Complete;
			} 
		}
	}
}
