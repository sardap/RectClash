using PaulECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	enum AttackStates
	{
		None,
		Waiting,
		Complete
	}

	interface IAttackCom : ICom
	{
		void Attack(IEntity toAttack);

		void Interupt();

		void Complete();

		AttackStates State { get; }

		float Damage { get; set; }
	}
}
