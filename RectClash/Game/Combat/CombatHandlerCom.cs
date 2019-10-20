using RectClash.ECS;
using RectClash.Game.Unit;
using System.Collections.Generic;

namespace RectClash.Game.Combat
{
	public class CombatHandlerCom : Com, IGameObv
	{
		private IEnt _attackTarget;
		private IEnt _attacker;

		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.ATTACK_TARGET:
					_attackTarget = ent;
					break;

				case GameEvent.ATTACK_ATTACKER:
					_attacker = ent;
					break;
				
				case GameEvent.ATTACK_CONF:
					if(_attacker == null || _attackTarget == null)
						throw new System.Exception("Attacker of attack target null");
					PerformAttack();
					break;
			}
		}

		private void PerformAttack()
		{
			var targetUnitCom = _attackTarget.GetCom<HealthCom>();
			var attackerUnitCom = _attacker.GetCom<UnitInfoCom>();

			targetUnitCom.CurrentHealth -= attackerUnitCom.Damage;
		}
	}
}