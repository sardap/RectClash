using RectClash.ECS;
using RectClash.Game.Combat;
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
					_unitInfoCom.MoveTaken = true;
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.MoveSound);
					break;

				case GameEvent.ATTACK_TARGET:
					_unitInfoCom.AttackTaken = true;
					if(PerformAttack(ent))
						Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.AttackSound);
					break;

				case GameEvent.RECEIVE_DAMAGE:
					Owner.GetCom<HealthCom>().ReceiveDamage(ent.GetCom<IDamageInfoCom>());
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.DamageSound);
					break;

				case GameEvent.UNIT_DIED:
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.DeathSound);
					Engine.Instance.DestoryEnt(Owner);
					break;
			}
		} 

		private bool PerformAttack(IEnt target)
		{
			var targetUnitCom = target.GetCom<HealthCom>();
			var attackerUnitCom = _unitInfoCom;

			var damageVariation = attackerUnitCom.DamageAmount * GameConstants.UNIT_DAMAGE_VARIATION;
			var damageModifer = Utility.RandomDouble(-(damageVariation), damageVariation);

			var result = targetUnitCom.CurrentHealth - attackerUnitCom.DamageAmount + damageModifer >= 0;

			target.GetCom<UnitActionContCom>().OnNotify
			(
				Owner,
				GameEvent.RECEIVE_DAMAGE
			);

			return result;
		}
	}
}