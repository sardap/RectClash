using PaulECS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static RectClash.Combat;

namespace RectClash
{
	class CombatResovlver
	{
		private static CombatResovlver _singleton;

		public static CombatResovlver Instance { get { return _singleton ?? new CombatResovlver(); } }

		private CombatResovlver()
		{
			_singleton = this;
		}

		private Dictionary<string, Combat> _combats = new Dictionary<string, Combat>();

		private string GetKey(IEntity a, IEntity b, List<IEntity> entities)
		{
			entities.Add(a);
			entities.Add(b);

			var sortedEnumerable = entities.OrderBy(i => i.ID);

			return sortedEnumerable.First().ID + ":" + sortedEnumerable.Last().ID;
		}

		public void AddCombat(IEntity a, IEntity b)
		{
			var entities = new List<IEntity>();

			var key = GetKey(a, b, entities);

			if (_combats.ContainsKey(key))
				return;

			_combats.Add(key, new Combat() { First = new CombatPair(entities[0], null), Secound = new CombatPair(entities[1], null) });

			StartCombat(_combats[key]);
		}

		public void BreakCombat(IEntity a, IEntity b)
		{
			var entities = new List<IEntity>();

			var key = GetKey(a, b, entities);

			_combats.Remove(key);
		}

		public void Step()
		{
			var removeStack = new Stack<string>();

			foreach(var combat in _combats)
			{
				if (StepCombat(combat.Value))
					removeStack.Push(combat.Key);
			}

			while (removeStack.Count > 0)
				_combats.Remove(removeStack.Pop());
		}

		private bool StepCombat(Combat combat)
		{
			StepCombatPair(combat.First, combat.Secound);
			StepCombatPair(combat.Secound, combat.First);

			return combat.First.CurrentAttack.State == AttackStates.Complete && combat.Secound.CurrentAttack.State == AttackStates.Complete;
		}

		private void StepCombatPair(CombatPair attacker, CombatPair defender)
		{
			if (attacker.CurrentAttack.State == AttackStates.Complete)
			{
				var defTempAgi = Utiliy.NextDouble(0, defender.Entity.GetCom<IAgilityDataCom>().Agility);
				var atkTempAgi = Utiliy.NextDouble(0, attacker.Entity.GetCom<IAgilityDataCom>().Agility);

				if(defTempAgi > atkTempAgi * 2)
				{
					defender.CurrentAttack.Interupt();
					Debug.WriteLine(RectClashDebug.GenIDPart(attacker.Entity) + "Interrupted " + RectClashDebug.GenIDPart(defender.Entity), DebugCatagroys.COMBAT);
				}

				attacker.CurrentAttack.Complete();
			}
		}

		private void StartCombat(Combat combat)
		{
			combat.First.CurrentAttack = Utiliy.RandomElement(combat.First.Entity.GetComs<IAttackCom>());
			combat.First.CurrentAttack.Attack(combat.Secound.Entity);

			combat.Secound.CurrentAttack = Utiliy.RandomElement(combat.Secound.Entity.GetComs<IAttackCom>());
			combat.Secound.CurrentAttack.Attack(combat.First.Entity);
		}
	}
}
