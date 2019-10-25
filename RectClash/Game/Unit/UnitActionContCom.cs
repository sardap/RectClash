using RectClash.ECS;
using RectClash.Misc;

namespace RectClash.Game.Unit
{
    public class UnitActionContCom : Com, IGameObv
    {
        private UnitInfoCom _unitInfoCom { get; set; }

		protected override void InternalStart()
		{
			_unitInfoCom = Owner.GetCom<UnitInfoCom>();
		}

		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.UNIT_SELECTED:
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.SelectSound);
					break;

				case GameEvent.UNIT_MOVED:
					_unitInfoCom.TurnTaken = true;
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.MoveSound);
					break;

				case GameEvent.ATTACK_TARGET:
					_unitInfoCom.TurnTaken = true;
					if(PerformAttack(ent))
						Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.AttackSound);
					break;
				case GameEvent.UNIT_DIED:
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.DeathSound);
					Owner.Destory();
					break;
			}
		} 

		private bool PerformAttack(IEnt target)
		{
			var targetUnitCom = target.GetCom<HealthCom>();
			var attackerUnitCom = _unitInfoCom;

			var damageVariation = attackerUnitCom.Damage * GameConstants.DAMAGE_VARIATION;
			var damageModifer = Utility.RandomDouble(-(damageVariation), damageVariation);

			var result = targetUnitCom.CurrentHealth - attackerUnitCom.Damage + damageModifer >= 0;

			targetUnitCom.CurrentHealth -= attackerUnitCom.Damage + damageModifer;

			return result;
		}
	}
}